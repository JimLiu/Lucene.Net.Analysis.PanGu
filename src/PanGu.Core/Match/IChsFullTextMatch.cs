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

namespace PanGu.Match
{

    /// <summary>
    /// interface for Chinese full text match 
    /// </summary>
    public interface IChsFullTextMatch
    {
        MatchOptions Options { get; set; }

        MatchParameter Parameters { get; set; }

        /// <summary>
        /// Do match
        /// </summary>
        /// <param name="positionLenArr">array of position length</param>
        /// <param name="count">count of items of position length list</param>
        /// <returns>Word Info list</returns>
        SuperLinkedList<WordInfo> Match(Dict.PositionLength[] positionLenArr, string orginalText, int count);
        
    }
}
