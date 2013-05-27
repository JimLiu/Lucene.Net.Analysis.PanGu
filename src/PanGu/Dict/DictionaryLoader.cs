using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PanGu.Dict
{
    class DictionaryLoader
    {
        public static Framework.Lock Lock = new PanGu.Framework.Lock();

        private string _DictionaryDir;

        public string DictionaryDir
        {
            get
            {
                return _DictionaryDir;
            }
        }

        private DateTime _MainDictLastTime;
        private DateTime _ChsSingleLastTime;
        private DateTime _ChsName1LastTime;
        private DateTime _ChsName2LastTime;
        private DateTime _StopWordLastTime;
        private DateTime _SynonymLastTime;
        private DateTime _WildcardLastTime;

        private Thread _Thread;

        private DateTime GetLastTime(string fileName)
        {
            return System.IO.File.GetLastWriteTime(DictionaryDir + fileName);
        }

        public DictionaryLoader(string dictDir)
        {
            _DictionaryDir = Framework.Path.AppendDivision(dictDir, '\\');
            _MainDictLastTime = GetLastTime("Dict.dct");
            _ChsSingleLastTime = GetLastTime(Dict.ChsName.ChsSingleNameFileName);
            _ChsName1LastTime = GetLastTime(Dict.ChsName.ChsDoubleName1FileName);
            _ChsName2LastTime = GetLastTime(Dict.ChsName.ChsDoubleName2FileName);
            _StopWordLastTime = GetLastTime("Stopword.txt");
            _SynonymLastTime = GetLastTime(Dict.Synonym.SynonymFileName);
            _WildcardLastTime = GetLastTime(Dict.Wildcard.WildcardFileName);

            _Thread = new Thread(MonitorDictionary);
            _Thread.IsBackground = true;
            _Thread.Start();
        }

        private bool MainDictChanged()
        {
            try
            {
                return _MainDictLastTime != GetLastTime("Dict.dct");
            }
            catch
            {
                return false;
            }
        }

        private bool ChsNameChanged()
        {
            try
            {
                return (_ChsSingleLastTime != GetLastTime(Dict.ChsName.ChsSingleNameFileName) ||
                    _ChsName1LastTime != GetLastTime(Dict.ChsName.ChsDoubleName1FileName) ||
                    _ChsName2LastTime != GetLastTime(Dict.ChsName.ChsDoubleName2FileName));
            }
            catch
            {
                return false;
            }
        }

        private bool StopWordChanged()
        {
            try
            {
                return _StopWordLastTime != GetLastTime("Stopword.txt");
            }
            catch
            {
                return false;
            }
        }

        private bool SynonymChanged()
        {
            try
            {
                return _SynonymLastTime != GetLastTime(Dict.Synonym.SynonymFileName);
            }
            catch
            {
                return false;
            }
        }

        private bool WildcardChanged()
        {
            try
            {
                return _WildcardLastTime != GetLastTime(Dict.Wildcard.WildcardFileName);
            }
            catch
            {
                return false;
            }

        }


        private void MonitorDictionary()
        {
            while (true)
            {
                Thread.Sleep(30000);

                try
                {
                    if (MainDictChanged())
                    {
                        try
                        {
                            DictionaryLoader.Lock.Enter(PanGu.Framework.Lock.Mode.Mutex);
                            Segment._WordDictionary.Load(_DictionaryDir + "Dict.dct");
                            _MainDictLastTime = GetLastTime("Dict.dct");
                        }
                        finally
                        {
                            DictionaryLoader.Lock.Leave();
                        }
                    }

                    if (ChsNameChanged())
                    {
                        try
                        {
                            DictionaryLoader.Lock.Enter(PanGu.Framework.Lock.Mode.Mutex);

                            Segment._ChsName.LoadChsName(_DictionaryDir);
                            _ChsSingleLastTime = GetLastTime(Dict.ChsName.ChsSingleNameFileName);
                            _ChsName1LastTime = GetLastTime(Dict.ChsName.ChsDoubleName1FileName);
                            _ChsName2LastTime = GetLastTime(Dict.ChsName.ChsDoubleName2FileName);
                        }
                        finally
                        {
                            DictionaryLoader.Lock.Leave();
                        }
                    }

                    if (StopWordChanged())
                    {
                        try
                        {
                            DictionaryLoader.Lock.Enter(PanGu.Framework.Lock.Mode.Mutex);

                            Segment._StopWord.LoadStopwordsDict(_DictionaryDir + "Stopword.txt");
                            _StopWordLastTime = GetLastTime("Stopword.txt");
                        }
                        finally
                        {
                            DictionaryLoader.Lock.Leave();
                        }
                    }

                    if (Segment._Synonym.Inited)
                    {
                        if (SynonymChanged())
                        {
                            try
                            {
                                DictionaryLoader.Lock.Enter(PanGu.Framework.Lock.Mode.Mutex);

                                Segment._Synonym.Load(_DictionaryDir);
                                _SynonymLastTime = GetLastTime(Dict.Synonym.SynonymFileName);
                            }
                            finally
                            {
                                DictionaryLoader.Lock.Leave();
                            }
                        }
                    }

                    if (Segment._Wildcard.Inited)
                    {
                        if (WildcardChanged())
                        {
                            try
                            {
                                Segment._Wildcard.Load(_DictionaryDir);
                                _WildcardLastTime = GetLastTime(Dict.Wildcard.WildcardFileName);
                            }
                            finally
                            {
                            }
                        }
                    }

                }
                catch
                {
                }

                
            }
        }
    }
}
