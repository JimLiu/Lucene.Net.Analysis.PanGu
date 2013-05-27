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

namespace PanGu
{
    [Flags]
    public enum POS
    {
        /// <summary>
        /// 形容词 形语素
        /// </summary>
        POS_D_A = 0x40000000,	//	形容词 形语素

        /// <summary>
        /// 区别词 区别语素
        /// </summary>
        POS_D_B = 0x20000000,	//	区别词 区别语素

        /// <summary>
        /// 连词 连语素
        /// </summary>
        POS_D_C = 0x10000000,	//	连词 连语素

        /// <summary>
        /// 副词 副语素
        /// </summary>
        POS_D_D = 0x08000000,	//	副词 副语素

        /// <summary>
        /// 叹词 叹语素
        /// </summary>
        POS_D_E = 0x04000000,	//	叹词 叹语素

        /// <summary>
        /// 方位词 方位语素
        /// </summary>
        POS_D_F = 0x02000000,	//	方位词 方位语素

        /// <summary>
        /// 成语
        /// </summary>
        POS_D_I = 0x01000000,	//	成语

        /// <summary>
        /// 习语
        /// </summary>
        POS_D_L = 0x00800000,	//	习语

        /// <summary>
        /// 数词 数语素
        /// </summary>
        POS_A_M = 0x00400000,	//	数词 数语素

        /// <summary>
        /// 数量词
        /// </summary>
        POS_D_MQ = 0x00200000,	//	数量词

        /// <summary>
        /// 名词 名语素
        /// </summary>
        POS_D_N = 0x00100000,	//	名词 名语素

        /// <summary>
        /// 拟声词
        /// </summary>
        POS_D_O = 0x00080000,	//	拟声词

        /// <summary>
        /// 介词
        /// </summary>
        POS_D_P = 0x00040000,	//	介词

        /// <summary>
        /// 量词 量语素
        /// </summary>
        POS_A_Q = 0x00020000,	//	量词 量语素

        /// <summary>
        /// 代词 代语素
        /// </summary>
        POS_D_R = 0x00010000,	//	代词 代语素

        /// <summary>
        /// 处所词
        /// </summary>
        POS_D_S = 0x00008000,	//	处所词

        /// <summary>
        /// 时间词
        /// </summary>
        POS_D_T = 0x00004000,	//	时间词

        /// <summary>
        /// 助词 助语素
        /// </summary>
        POS_D_U = 0x00002000,	//	助词 助语素

        /// <summary>
        /// 动词 动语素
        /// </summary>
        POS_D_V = 0x00001000,	//	动词 动语素

        /// <summary>
        /// 标点符号
        /// </summary>
        POS_D_W = 0x00000800,	//	标点符号

        /// <summary>
        /// 非语素字
        /// </summary>
        POS_D_X = 0x00000400,	//	非语素字

        /// <summary>
        /// 语气词 语气语素
        /// </summary>
        POS_D_Y = 0x00000200,	//	语气词 语气语素

        /// <summary>
        /// 状态词
        /// </summary>
        POS_D_Z = 0x00000100,	//	状态词

        /// <summary>
        /// 人名
        /// </summary>
        POS_A_NR = 0x00000080,	//	人名

        /// <summary>
        /// 地名
        /// </summary>
        POS_A_NS = 0x00000040,	//	地名

        /// <summary>
        /// 机构团体
        /// </summary>
        POS_A_NT = 0x00000020,	//	机构团体

        /// <summary>
        /// 外文字符
        /// </summary>
        POS_A_NX = 0x00000010,	//	外文字符

        /// <summary>
        /// 其他专名
        /// </summary>
        POS_A_NZ = 0x00000008,	//	其他专名

        /// <summary>
        /// 前接成分
        /// </summary>
        POS_D_H = 0x00000004,	//	前接成分

        /// <summary>
        /// 后接成分
        /// </summary>
        POS_D_K = 0x00000002,	//	后接成分

        /// <summary>
        /// 未知词性
        /// </summary>
        POS_UNK = 0x00000000,   //  未知词性
    }


}
