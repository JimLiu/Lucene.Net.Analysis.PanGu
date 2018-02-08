using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu.Framework
{
    public enum LexicalFunction
    {
        None = 0,
        OutputIdentifier = 1,
        DoSpace = 2,
        OutputSpace = 3,
        OutputNumeric = 4,
        OutputChinese = 5,

        Other = 255,
    }

    class LexicalState : DFAState<int, LexicalFunction>
    {
        public LexicalState(int id, bool isQuit, LexicalFunction function, IDictionary<int, int> nextStateIdDict)
        {
            m_Id = id;
            IsQuitState = isQuit;
            Func = function;

            NoFunction = Func == LexicalFunction.None;
            NextStateIdDict = nextStateIdDict;
        }

        public LexicalState(int id, bool isQuit, LexicalFunction function):
            this(id, isQuit, function, null)
        {

        }

        public LexicalState(int id, bool isQuit) :
            this(id, isQuit, LexicalFunction.None, null)
        {

        }

        public LexicalState(int id, bool isQuit, IDictionary<int, int> nextStateIdDict) :
            this(id, isQuit, LexicalFunction.None, nextStateIdDict)
        {

        }

        public LexicalState(int id) :
            this(id, false, LexicalFunction.None, null)
        {

        }

        public LexicalState(int id, IDictionary<int, int> nextStateIdDict) :
            this(id, false, LexicalFunction.None, nextStateIdDict)
        {

        }

        private void GetTextElse(DFA<int, LexicalFunction> dfa)
        {
            int endIndex = dfa.CurrentToken;

            Lexical lexical = (Lexical)dfa;

            lexical.OutputToken.Position = lexical.beginIndex;

            lexical.OutputToken.Word = lexical.InputText.Substring(lexical.beginIndex, endIndex - lexical.beginIndex);

            lexical.beginIndex = endIndex;
        }

        private void GetText(DFA<int, LexicalFunction> dfa)
        {
            int endIndex = dfa.CurrentToken;

            Lexical lexical = (Lexical)dfa;

            if (endIndex == lexical.InputText.Length)
            {
                lexical.OutputToken.Position = lexical.beginIndex;
                
                lexical.OutputToken.Word = lexical.InputText.Substring(lexical.beginIndex,
                    endIndex - lexical.beginIndex);
            }
            else
            {
                lexical.OutputToken.Position = lexical.beginIndex;

                lexical.OutputToken.Word = lexical.InputText.Substring(lexical.beginIndex,
                    endIndex - lexical.beginIndex + 1);
            }

            lexical.beginIndex = endIndex + 1;
        }

        private void GetString(DFA<int, LexicalFunction> dfa)
        {
            int endIndex = dfa.CurrentToken;

            Lexical lexical = (Lexical)dfa;

            lexical.OutputToken.Word = lexical.InputText.Substring(lexical.beginIndex + 1, endIndex - lexical.beginIndex - 2);
            lexical.OutputToken.Position = lexical.beginIndex + 1;

            lexical.beginIndex = endIndex;
        }

        #region Dothings
        public override void DoThings(int action, DFA<int, LexicalFunction> dfa)
        {
            Lexical lexical = (Lexical)dfa;

            switch (Func)
            {
                case LexicalFunction.OutputIdentifier:
                    lexical.OutputToken = new WordInfo(); 
                    GetTextElse(dfa);
                    lexical.OutputToken.WordType = WordType.English; 

                    break;
                case LexicalFunction.OutputSpace:
                    lexical.OutputToken = new WordInfo();
                    GetTextElse(dfa);
                    lexical.OutputToken.WordType = WordType.Space;
                    break;
                case LexicalFunction.OutputNumeric:
                    lexical.OutputToken = new WordInfo();
                    GetTextElse(dfa);
                    lexical.OutputToken.WordType = WordType.Numeric;
                    break;
                case LexicalFunction.OutputChinese:
                    lexical.OutputToken = new WordInfo();
                    GetTextElse(dfa);
                    lexical.OutputToken.WordType = WordType.SimplifiedChinese;
                    break;
                case LexicalFunction.Other:
                    lexical.OutputToken = new WordInfo();
                    GetText(dfa);
                    lexical.OutputToken.WordType = WordType.Symbol;
                    break;
            }
        }

        #endregion
    }

    public class Lexical:DFA<int, LexicalFunction>
    {
        public int beginIndex = 0;

        public string InputText = null;
        public WordInfo OutputToken = null;

        private static DFAState<int, LexicalFunction> s0 = AddState(new LexicalState(0)); //Start state;
        private static DFAState<int, LexicalFunction> sother = AddState(new LexicalState(255, true, LexicalFunction.Other)); //Start state;

        private static void InitIdentifierStates()
        {
            DFAState<int, LexicalFunction> s1 = AddState(new LexicalState(1)); //Identifier begin state;
            DFAState<int, LexicalFunction> s2 = AddState(new LexicalState(2, true, LexicalFunction.OutputIdentifier)); //Identifier quit state;

            //s0 [_a-zA-Z] s1
            s0.AddNextState('_', s1.Id);
            s0.AddNextState('a', 'z', s1.Id);
            s0.AddNextState('A', 'Z', s1.Id);
            s0.AddNextState('ａ', 'ｚ', s1.Id);
            s0.AddNextState('Ａ', 'Ｚ', s1.Id);

            //s1 [_a-zA-Z0-9] s1
            s1.AddNextState('_', s1.Id);
            s1.AddNextState('a', 'z', s1.Id);
            s1.AddNextState('A', 'Z', s1.Id);
            s1.AddNextState('0', '9', s1.Id);
            s1.AddNextState('ａ', 'ｚ', s1.Id);
            s1.AddNextState('Ａ', 'Ｚ', s1.Id);
            s1.AddNextState('０', '９', s1.Id);

            //s1 ^[_a-zA-Z0-9] s2
            s1.AddElseState(s2.Id);
        }

        private static void InitSpaceStates()
        {
            DFAState<int, LexicalFunction> s3 = AddState(new LexicalState(3, false)); //Space begin state;
            DFAState<int, LexicalFunction> s4 = AddState(new LexicalState(4, true, LexicalFunction.OutputSpace)); //Space quit state;

            //s0 [ \t\r\n] s3
            s0.AddNextState(new int[]{' ', '\t', '\r', '\n'}, s3.Id);

            //s3 [ \t\r\n] s3
            s3.AddNextState(new int[] { ' ', '\t', '\r', '\n' }, s3.Id);

            //s3 ^[ \t\r\n] s4
            s3.AddElseState(s4.Id);
        }

        private static void InitNumericStates()
        {
            DFAState<int, LexicalFunction> s5 = AddState(new LexicalState(5, false)); //Numeric begin state;
            DFAState<int, LexicalFunction> s6 = AddState(new LexicalState(6, false)); //Number dot state;
            DFAState<int, LexicalFunction> s7 = AddState(new LexicalState(7, true, LexicalFunction.OutputNumeric)); //Number quit state;

            //s0 [0-9] s5
            s0.AddNextState('0', '9', s5.Id);
            s0.AddNextState('０', '９', s5.Id);

            //s5 [0-9] s5
            s5.AddNextState('0', '9', s5.Id);
            s5.AddNextState('０', '９', s5.Id);

            //s5 [\.] s6
            s5.AddNextState('.', s6.Id);

            //s5 else s7 (integer)
            s5.AddElseState(s7.Id);

            //s6 [0-9] s6
            s6.AddNextState('0', '9', s6.Id);
            s6.AddNextState('０', '９', s6.Id);

            //s6 else s7 (float)
            s6.AddElseState(s7.Id);

        }

        private static void InitChineseStates()
        {
            DFAState<int, LexicalFunction> s8 = AddState(new LexicalState(8, false)); //Numeric begin state;
            DFAState<int, LexicalFunction> s9 = AddState(new LexicalState(9, true, LexicalFunction.OutputChinese)); //Number quit state;

            //s0 [4e00-9fa5] s5
            s0.AddNextState(0x4e00, 0x9fa5, s8.Id);

            s8.AddNextState(0x4e00, 0x9fa5, s8.Id);

            s8.AddElseState(s9.Id);
        }

        private static void InitOtherStates()
        {
            s0.AddElseState(sother.Id);
        }

        private static void InitDFAStates()
        {
            InitIdentifierStates();
            InitSpaceStates();
            InitNumericStates();
            InitChineseStates();

            InitOtherStates();
        }

        public static void Initialize()
        {
            lock (InitLockObj)
            {
                if (!Inited)
                {
                    InitDFAStates();
                    Inited = true;
                }
            }
        }

        protected override void Init()
        {
            Initialize();
        }

        public Lexical(string inputText)
        {
            InputText = inputText;
        }

        new public DFAResult Input(int action, int token)
        {
            return base.Input(action, token);
        }
    }
}
