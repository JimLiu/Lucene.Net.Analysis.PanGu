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

namespace PanGu.Framework
{
    public class Stream
    {
        static public void ReadToBuf(System.IO.Stream s, byte[] buf, int offset, int count)
        {
            int read_offset = offset;

            int len = s.Read(buf, read_offset, count - (read_offset - offset));

            read_offset += len;

            while (read_offset < offset + count && len > 0)
            {
                len = s.Read(buf, read_offset, count - (read_offset - offset));
                read_offset += len;
            }
        }

        public static void ReadStreamToString(System.IO.Stream input, out String str, String Encode)
        {
            ReadStreamToString(input, out str, Encoding.GetEncoding(Encode));
        }

        public static void ReadStreamToString(System.IO.Stream input, out String str, Encoding Encode)
        {
            input.Position = 0;
            System.IO.StreamReader readStream = new System.IO.StreamReader(input, Encode);
            str = readStream.ReadToEnd();
        }

        public static System.IO.Stream WriteStringToStream(String str, String Encode)
        {
            return WriteStringToStream(str, Encoding.GetEncoding(Encode));
        }

        public static System.IO.Stream WriteStringToStream(String str, Encoding Encode)
        {
            MemoryStream output = new MemoryStream();
            StreamWriter writeStream = new StreamWriter(output, Encode);
            writeStream.Write(str);
            writeStream.Flush();
            output.Position = 0;
            return output;
        }
    }
}
