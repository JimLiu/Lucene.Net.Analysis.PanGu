using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace PanGu.Framework
{
    public enum DFAResult
    {
        Continue = 0,
        Quit = 1,
        ElseQuit = 2,
        End = 3,
    }

    public abstract class DFAState<Token, Function>
    {
        public bool NoFunction = true;

        protected int m_Id = 0;

        protected Function Func;

        public bool IsQuitState = false;

        protected IDictionary<int, int> NextStateIdDict = null;

        protected int[] NextStateIds;

        protected int ElseStateId = -1;

        public int Id
        {
            get
            {
                return m_Id;
            }
        }


             
        public virtual void AddNextState(int action, int nextstate)
        {
            Debug.Assert(action >= 0);

            if (NextStateIdDict != null)
            {
                NextStateIdDict.Add(action, nextstate);
            }
            else
            {
                if (NextStateIds == null)
                {
                    NextStateIds = new int[action + 1];

                    for (int i = 0; i < NextStateIds.Length; i++)
                    {
                        NextStateIds[i] = -1;
                    }
                }
                else
                {
                    if (NextStateIds.Length < action + 1)
                    {
                        int[] old = NextStateIds;

                        NextStateIds = new int[action + 1];

                        old.CopyTo(NextStateIds, 0);

                        for (int i = old.Length; i < NextStateIds.Length; i++)
                        {
                            NextStateIds[i] = -1;
                        }
                    }
                }

                NextStateIds[action] = nextstate;
            }
        }

        public void AddNextState(int beginAction, int endAction, int nextstate)
        {
            for (int action = endAction; action >= beginAction; action--)
            {
                AddNextState(action, nextstate);
            }
        }

        public void AddNextState(int[] actions, int nextstate)
        {
            Debug.Assert(actions != null);


            Array.Sort(actions);

            for (int i = actions.Length - 1; i >= 0; i--)
            {
                AddNextState(actions[i], nextstate);
            }
        }


        public void AddElseState(int nextstate)
        {
            ElseStateId = nextstate;
        }

        /// <summary>
        /// Get next state
        /// </summary>
        /// <param name="action">action (letter eg.), if ation less then 0, means get else state</param>
        /// <param name="dfa">DFA that call this state</param>
        /// <returns>next state id</returns>
        public virtual int NextState(int action, DFA<Token, Function> dfa, out bool isElseAction)
        {
            Debug.Assert(NextStateIds != null);

            isElseAction = false;

            if (action < 0)
            {
                isElseAction = true;
                return ElseStateId;
            }


            if (NextStateIdDict != null)
            {
                int nextState;
                if (NextStateIdDict.TryGetValue(action, out nextState))
                {
                    if (nextState < 0)
                    {
                        isElseAction = true;
                        return ElseStateId;
                    }
                    else
                    {
                        return nextState;
                    }
                }
                else
                {
                    isElseAction = true;
                    return ElseStateId;
                }
            }
            else
            {
                if (NextStateIds == null)
                {
                    isElseAction = true;
                    return ElseStateId;
                }

                if (action >= NextStateIds.Length)
                {
                    isElseAction = true;
                    return ElseStateId;
                }
                else
                {
                    int nextState = NextStateIds[action];

                    if (nextState < 0)
                    {
                        isElseAction = true;
                        return ElseStateId;
                    }
                    else
                    {
                        return nextState;
                    }
                }
            }

        }

        public abstract void DoThings(int action, DFA<Token, Function> dfa);
    }

    public abstract class DFA <Token, Function>
    {
        protected static DFAState<Token, Function>[] States = new DFAState<Token, Function>[32];

        protected static bool Inited = false;
        protected static object InitLockObj = new object();

        protected static int EofAction = 0;

        protected abstract void Init();

        public int OldState = 0;
        public int CurrentState = 0;

        public bool QuitManually = false;

        public Token CurrentToken;

        public static DFAState<Token, Function> AddState(DFAState<Token, Function> state)
        {
            if (state.Id >= States.Length)
            {
                int newLength;

                if (state.Id < 2 * States.Length)
                {
                    newLength = 2 * States.Length;
                }
                else
                {
                    newLength = state.Id + 1;
                }

                DFAState<Token, Function>[] oldStates = States;

                States = new DFAState<Token, Function>[newLength];

                oldStates.CopyTo(States, 0);
            }

            if (States[state.Id] != null)
            {
                throw new DFAException("state.Id must be equal to States index!", 0, state.Id);
            }

            States[state.Id] = state;

            return state;
        }


        /// <summary>
        /// Input action
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="token">token (additional data)</param>
        /// <returns>if quit return true</returns>
        public virtual DFAResult Input(int action, Token token)
        {
            Debug.Assert(States != null);

            if (States.Length == 0)
            {
                return DFAResult.Continue;
            }

            CurrentToken = token;

            if (!Inited)
            {
                Init();
            }

            if (CurrentState == 0 && action == EofAction)
            {
                return DFAResult.End;
            }

            bool isElseAction;
            OldState = CurrentState;
            CurrentState = States[CurrentState].NextState(action, this, out isElseAction);

            if (CurrentState < 0)
            {
                CurrentState = 0;
                throw new DFAException(string.Format("Invalid next DFA state! action={0} currentstate={1}", action, OldState),
                    action, OldState);
            }

            if (CurrentState >= States.Length)
            {
                throw new DFAException("Bad DFA state!", action, CurrentState);
            }

            if (States[CurrentState] == null)
            {
                throw new DFAException("Bad DFA state!", action, CurrentState);
            }

            if (!States[CurrentState].NoFunction)
            {
                States[CurrentState].DoThings(action, this);
            }

            if (!States[CurrentState].IsQuitState && !QuitManually)
            {
                return DFAResult.Continue;
            }
            else
            {
                QuitManually = false;

                OldState = CurrentState;
                CurrentState = 0;

                if (isElseAction)
                {
                    //Else action
                    return DFAResult.ElseQuit;
                }
                else
                {
                    return DFAResult.Quit;
                }

            }

        }

    }


    public class DFAException : Exception
    {
        private int _Action;

        public int Action
        {
            get
            {
                return _Action;
            }
        }

        private int _State;

        public int State
        {
            get
            {
                return _State;
            }
        }

        public DFAException(string message, int action, int state)
            : base(message)
        {
            _Action = action;
            _State = state;
        }
    }
}
