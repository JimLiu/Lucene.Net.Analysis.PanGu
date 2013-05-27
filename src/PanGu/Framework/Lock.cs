/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PanGu.Framework
{
    /// <summary>
    /// Share or Mutex lock
    /// </summary>
    public class Lock
    {
        public enum Mode
        {
            Share = 0,
            Mutex = 1,
        }

        enum State
        {
            Share = 0,
            Mutex = 1,
        }

        State _State = State.Share;

        int _ShareCounter = 0;

        public void Enter(Mode mode)
        {
            bool waitShareCounterZero = false;
            bool waitForShareState = false;
        Loop:
            lock (this)
            {
                switch (mode)
                {
                    case Mode.Share:
                        switch (_State)
                        {
                            case State.Share:
                                _ShareCounter++;
                                return;
                            case State.Mutex:
                                waitForShareState = true;
                                break;
                        }
                        break;

                    case Mode.Mutex:
                        switch (_State)
                        {
                            case State.Share:
                                waitShareCounterZero = true;
                                _State = State.Mutex;
                                break;
                            case State.Mutex:
                                waitForShareState = true;
                                break;
                        }
                        break;
                }
            }

            if (waitShareCounterZero)
            {
                int counter;
                int times = 0;
                do
                {
                    lock (this)
                    {
                        counter = _ShareCounter;
                    }

                    if (counter > 0)
                    {
                        if (times++ < 10)
                        {
                            Thread.Sleep(0);
                        }
                        else
                        {
                            Thread.Sleep(1);
                        }
                    }

                } while (counter > 0);
            }
            else if (waitForShareState)
            {
                int times = 0;
                State state;

                do
                {
                    lock (this)
                    {
                        state = _State;
                    }

                    if (state != State.Share)
                    {
                        if (times++ < 10)
                        {
                            Thread.Sleep(0);
                        }
                        else
                        {
                            Thread.Sleep(1);
                        }
                    }
                } while (state != State.Share);
                waitShareCounterZero = false;
                waitForShareState = false;
                goto Loop;
            }
        }

        public void Leave()
        {
            lock (this)
            {
                if (_ShareCounter > 0)
                {
                    _ShareCounter--;
                    return;
                }

                if (_State == State.Mutex)
                {
                    _State = State.Share;
                }
            }
        }
    }
}
