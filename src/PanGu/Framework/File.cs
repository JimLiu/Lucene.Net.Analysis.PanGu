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

namespace PanGu.Framework
{
    public class File
    {
        //The process cannot access the file because it is being used by another process
        const uint ERR_PROCESS_CANNOT_ACCESS_FILE = 0x80070020;

        /// <summary>
        /// Get file length
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public long GetFileLength(string fileName)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            return fileInfo.Length;
        }


        static public String ReadFileToString(String FileName, Encoding encode)
        {
            if (!System.IO.File.Exists(FileName))
            {
                string name = System.IO.Path.GetFileName(FileName);
                string result = null;
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyName = assembly.GetName().Name;
                //string[] names = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
                using (var textStream = assembly.GetManifestResourceStream(assemblyName + ".Resources." + name))
                {
                    //var textStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                    if (textStream != null)
                    {
                        using (var reader = new StreamReader(textStream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                return result;

            }
            else
            {
                FileStream fs = null;
                String str;

                int times = 0;
                while (true)
                {
                    try
                    {
                        fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                        Stream.ReadStreamToString(fs, out str, encode);
                        fs.Close();
                        return str;
                    }
                    catch (IOException e)
                    {
                        uint hResult = (uint)System.Runtime.InteropServices.Marshal.GetHRForException(e);
                        if (hResult == ERR_PROCESS_CANNOT_ACCESS_FILE)
                        {
                            if (times > 10)
                            {
                                //Maybe another program has some trouble with file
                                //We must exit now
                                throw e;
                            }

                            System.Threading.Thread.Sleep(200);
                            times++;
                        }
                        else
                        {
                            throw e;
                        }
                    }
                }
            }
        }

        static public MemoryStream ReadFileToStream(String FileName)
        {
            if (!System.IO.File.Exists(FileName))
            {
                string name = System.IO.Path.GetFileName(FileName);
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyName = assembly.GetName().Name;

                MemoryStream stream = (MemoryStream)assembly.GetManifestResourceStream(assemblyName + ".Resources." + name);
                return stream;
            }
            else
            {
                byte[] Bytes = new byte[32768];
                int read = 0;
                int offset = 0;
                FileStream fs = null;

                int times = 0;
                while (true)
                {
                    try
                    {
                        MemoryStream mem = new MemoryStream();
                        fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                        mem.Position = 0;

                        while ((read = fs.Read(Bytes, 0, Bytes.Length)) > 0)
                        {
                            offset += read;
                            mem.Write(Bytes, 0, read);
                        }

                        fs.Close();
                        return mem;
                    }
                    catch (IOException e)
                    {
                        uint hResult = (uint)System.Runtime.InteropServices.Marshal.GetHRForException(e);
                        if (hResult == ERR_PROCESS_CANNOT_ACCESS_FILE)
                        {
                            if (times > 10)
                            {
                                //Maybe another program has some trouble with file
                                //We must exit now
                                throw e;
                            }

                            System.Threading.Thread.Sleep(200);
                            times++;
                        }
                        else
                        {
                            throw e;
                        }
                    }
                }
            }
        }

        public static void WriteStream(String FileName, MemoryStream In)
        {
            FileStream fs = null;

            int times = 0;
            while (true)
            {
                try
                {
                    if (System.IO.File.Exists(FileName))
                    {
                        System.IO.File.Delete(FileName);
                    }

                    fs = new FileStream(FileName, FileMode.CreateNew);
                    In.WriteTo(fs);
                    fs.Close();
                    return;
                }
                catch (IOException e)
                {
                    uint hResult = (uint)System.Runtime.InteropServices.Marshal.GetHRForException(e);
                    if (hResult == ERR_PROCESS_CANNOT_ACCESS_FILE)
                    {
                        if (times > 10)
                        {
                            //Maybe another program has some trouble with file
                            //We must exit now
                            throw e;
                        }

                        System.Threading.Thread.Sleep(200);
                        times++;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        public static void WriteLine(String FileName, String str)
        {
            int times = 0;

            while (true)
            {
                try
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Append))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine(str);
                        }
                    }

                    return;
                }
                catch (IOException e)
                {
                    uint hResult = (uint)System.Runtime.InteropServices.Marshal.GetHRForException(e);
                    if (hResult == ERR_PROCESS_CANNOT_ACCESS_FILE)
                    {
                        if (times > 10)
                        {
                            //Maybe another program has some trouble with file
                            //We must exit now
                            throw e;
                        }

                        System.Threading.Thread.Sleep(200);
                        times++;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        public static void WriteString(String FileName, String str, Encoding encode)
        {
            TextWriter writer = null;
            FileStream fs = null;

            int times = 0;
            while (true)
            {
                try
                {
                    if (System.IO.File.Exists(FileName))
                    {
                        System.IO.File.Delete(FileName);
                    }

                    fs = new FileStream(FileName, FileMode.CreateNew);
                    writer = new StreamWriter(fs, encode);
                    writer.Write(str);
                    writer.Close();
                    fs.Close();
                    return;
                }
                catch (IOException e)
                {
                    uint hResult = (uint)System.Runtime.InteropServices.Marshal.GetHRForException(e);
                    if (hResult == ERR_PROCESS_CANNOT_ACCESS_FILE)
                    {
                        if (times > 10)
                        {
                            //Maybe another program has some trouble with file
                            //We must exit now
                            throw e;
                        }

                        System.Threading.Thread.Sleep(200);
                        times++;
                    }
                    else
                    {
                        throw e;
                    }
                }

            }
        }

        public static void DeleteFile(string path, string fileName, bool recursive)
        {
            if (path[path.Length - 1] != '\\')
            {
                path += '\\';
            }

            if (!recursive)
            {
                System.IO.File.Delete(path + fileName);
            }
            else
            {
                System.IO.File.Delete(path + fileName);

                string[] subFolders = System.IO.Directory.GetDirectories(path);

                foreach (string folder in subFolders)
                {
                    DeleteFile(folder, fileName, recursive);
                }
            }

        }
    
    }
}
