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
using System.IO;
using System.Reflection;

namespace PanGu.Dict
{
    class StopWord
    {
        Dictionary<string, string> _StopwordTbl = new Dictionary<string, string>();

        public bool IsStopWord(string word, bool filterEnglish, int filterEnglishLength, 
            bool filterNumeric, int filterNumericLength) 
        {
            if (word == null || word == "")
            {
                return false;
            }

            string key;

            if (word[0] < 128)
            {
                if (filterEnglish)
                {
                    if (word.Length > filterEnglishLength && (word[0] < '0' || word[0] > '9'))
                    {
                        return true;
                    }
                }

                if (filterNumeric)
                {
                    if (word.Length > filterNumericLength && (word[0] >= '0' && word[0] <= '9'))
                    {
                        return true;
                    }
                }


                key = word.ToLower();
            }
            else
            {
                key = word;
            }

            return _StopwordTbl.ContainsKey(key);
        }

        public void LoadStopwordsDict(String fileName)
        {
            _StopwordTbl = new Dictionary<string, string>();
            StreamReader sw;
            Stream stream = null;
            if (File.Exists(fileName))
            {
                sw = new StreamReader(fileName, Encoding.GetEncoding("UTF-8"));
            }
            else
            { 
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyName = assembly.GetName().Name;
                stream = assembly.GetManifestResourceStream(assemblyName + ".Resources." + Path.GetFileName(fileName));
                sw = new StreamReader(stream, Encoding.UTF8);

            }
            //加载中文停用词
            while (!sw.EndOfStream)
            {
                //按行读取中文停用词
                string stopWord = sw.ReadLine();

                if (string.IsNullOrEmpty(stopWord))
                {
                    continue;
                }

                string key;

                if (stopWord[0] < 128)
                {
                    key = stopWord.ToLower();
                }
                else
                {
                    key = stopWord;
                }

                //如果哈希表中不包括该停用词则添加到哈希表中
                if (!_StopwordTbl.ContainsKey(key))
                {
                    _StopwordTbl.Add(key, stopWord);
                }
            }
            sw.Close();
            if (stream != null)
                stream.Close();
        }
    }
}
