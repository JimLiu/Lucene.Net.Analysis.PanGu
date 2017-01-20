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
using PanGu.Framework;

namespace PanGu.Setting
{
    [Serializable, System.Xml.Serialization.XmlRoot(Namespace = "http://www.codeplex.com/pangusegment")] 
    public class PanGuSettings
    {
        #region static members
        private static PanGuSettings _Config;
        
        public static PanGuSettings Config
        {
            get
            {
                return _Config;
            }
        }

        static public void Load(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open,
                         System.IO.FileAccess.Read))
                    {
                        _Config = XmlSerialization<PanGuSettings>.Deserialize(fs);
                    }
                }
                catch
                {
                    _Config = new PanGuSettings();
                }
            }
            else
            {
                _Config = new PanGuSettings();
            }
        }

        static public void Save(string fileName)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create,
                 System.IO.FileAccess.ReadWrite))
            {
                XmlSerialization<PanGuSettings>.Serialize(Config, Encoding.UTF8, fs);
            }
        }

        #endregion

        public string GetDictionaryPath()
        {
            string path = DictionaryPath;

            string currentDir = System.IO.Directory.GetCurrentDirectory();
            System.IO.Directory.SetCurrentDirectory(Framework.Path.GetAssemblyPath());
            path = System.IO.Path.GetFullPath(path);
            System.IO.Directory.SetCurrentDirectory(currentDir);

            return Path.AppendDivision(path, '\\');

        }

        #region Properties

        private string _DictionaryPath = "Dict";

        public string DictionaryPath
        {
            get
            {
                return _DictionaryPath;
            }

            set
            {
                _DictionaryPath = value;
            }
        }

        private Match.MatchOptions _MatchOptions = new PanGu.Match.MatchOptions();

        public Match.MatchOptions MatchOptions
        {
            get
            {
                return _MatchOptions;
            }

            set
            {
                _MatchOptions = value;
            }
        }

        private Match.MatchParameter _Parameters = new PanGu.Match.MatchParameter();

        public Match.MatchParameter Parameters
        {
            get
            {
                return _Parameters;
            }

            set
            {
                _Parameters = value;
            }
        }

        #endregion

        public Match.MatchOptions GetOptionsCopy()
        {
            Match.MatchOptions options = new PanGu.Match.MatchOptions();

            options.ChineseNameIdentify = this.MatchOptions.ChineseNameIdentify;
            options.FrequencyFirst = this.MatchOptions.FrequencyFirst;
            options.MultiDimensionality = this.MatchOptions.MultiDimensionality;
            options.FilterStopWords = this.MatchOptions.FilterStopWords;
            options.IgnoreSpace = this.MatchOptions.IgnoreSpace;
            options.ForceSingleWord = this.MatchOptions.ForceSingleWord;
            options.TraditionalChineseEnabled = this.MatchOptions.TraditionalChineseEnabled;
            options.OutputSimplifiedTraditional = this.MatchOptions.OutputSimplifiedTraditional;

            return options;
        }

        public Match.MatchParameter GetParameterCopy()
        {
            Match.MatchParameter parameter = new PanGu.Match.MatchParameter();

            parameter.Redundancy = this.Parameters.Redundancy;
            parameter.UnknowRank = this.Parameters.UnknowRank;
            parameter.BestRank = this.Parameters.BestRank;
            parameter.SecRank = this.Parameters.SecRank;
            parameter.ThirdRank = this.Parameters.ThirdRank;
            parameter.SingleRank = this.Parameters.SingleRank;
            parameter.NumericRank = this.Parameters.NumericRank;
            parameter.EnglishRank = this.Parameters.EnglishRank;
            parameter.SymbolRank = this.Parameters.SymbolRank;
            parameter.SimplifiedTraditionalRank = this.Parameters.SimplifiedTraditionalRank;

            return parameter;
        }

    }
}
