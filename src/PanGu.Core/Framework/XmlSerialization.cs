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
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace PanGu.Framework
{
    public class XmlSerialization
    {
        public static System.IO.Stream Serialize(object obj, Encoding encode, System.IO.Stream s)
        {
            TextWriter writer = null;
            writer = new StreamWriter(s, encode);

            XmlSerializer ser = new XmlSerializer(obj.GetType());

            ser.Serialize(writer, obj);
            return s;

        }

        public static MemoryStream Serialize(object obj, Encoding encode)
        {
            MemoryStream s = new MemoryStream();
            Serialize(obj, encode, s);
            s.Position = 0;
            return s;
        }

        public static MemoryStream Serialize(object obj, String encode)
        {
            return Serialize(obj, Encoding.GetEncoding(encode));
        }

        public static MemoryStream Serialize(object obj)
        {
            return Serialize(obj, Encoding.UTF8);
        }

        public static object Deserialize(System.IO.Stream In, Type objType)
        {
            In.Position = 0;
            XmlSerializer ser = new XmlSerializer(objType);
            return ser.Deserialize(In);
        }
    }


    public class XmlSerialization<T>
    {
        public static System.IO.Stream Serialize(T obj, Encoding encode, System.IO.Stream s)
        {
            TextWriter writer = null;
            writer = new StreamWriter(s, encode);

            XmlSerializer ser = new XmlSerializer(typeof(T));

            ser.Serialize(writer, obj);
            return s;
        }

        public static MemoryStream Serialize(T obj, Encoding encode)
        {
            MemoryStream s = new MemoryStream();
            Serialize(obj, encode, s);
            s.Position = 0;
            return s;
        }

        public static MemoryStream Serialize(T obj, String encode)
        {
            return Serialize(obj, Encoding.GetEncoding(encode));
        }

        public static MemoryStream Serialize(T obj)
        {
            return Serialize(obj, Encoding.UTF8);
        }

        public static T Deserialize(System.IO.Stream In)
        {
            In.Position = 0;
            XmlSerializer ser = new XmlSerializer(typeof(T));
            return (T)ser.Deserialize(In);
        }
    }

}
