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
using System.Diagnostics;

namespace PanGu.Framework
{
    public class Path
    {
        static public string GetAssemblyPath()
        {
            const string _PREFIX = @"file:///";
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            codeBase = codeBase.Substring(_PREFIX.Length, codeBase.Length - _PREFIX.Length).Replace("/", "\\");
            return System.IO.Path.GetDirectoryName(codeBase) + @"\";
        }

        static public string ProcessDirectory
        {
            get
            {
                string curFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                return System.IO.Path.GetDirectoryName(curFileName);
            }
        }

        static public String AppendDivision(String path, char division)
        {
            Debug.Assert(path != null);

            if (path == "")
            {
                return path + division;
            }

            if (path[path.Length - 1] != '\\' &&
                path[path.Length - 1] != '/')
            {
                return path + division;
            }
            else
            {
                if (path[path.Length - 1] != division)
                {
                    return path.Substring(0, path.Length - 1) + division;
                }
            }

            return path;
        }


        static public String GetFolderName(String path)
        {
            path = System.IO.Path.GetFullPath(path);

            int len = path.Length - 1;
            while (path[len] == System.IO.Path.DirectorySeparatorChar && len >= 0)
            {
                len--;
            }

            String ret = "";
            for (int i = len; i >= 0; i--)
            {
                if (path[i] == System.IO.Path.DirectorySeparatorChar)
                {
                    break;
                }

                ret = path[i] + ret;
            }

            return ret;
        }
    }
}
