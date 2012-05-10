﻿/*
 * Copyright (c) 2010-2012, Achim 'ahzf' Friedland <achim@graph-database.org>
 * This file is part of Bifrost <http://www.github.com/Vanaheimr/Bifrost>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Reflection;

using de.ahzf.Illias.Commons;
using de.ahzf.Hermod;
using de.ahzf.Hermod.HTTP;

#endregion

namespace de.ahzf.Bifrost.HTTP.Server
{

    /// <summary>
    /// EVENTSTREAM content representation.
    /// </summary>
    public class BifrostService_EVENTSTREAM : ABifrostService
    {

        #region Constructor(s)

        #region BifrostService_EVENTSTREAM()

        /// <summary>
        /// EVENTSTREAM content representation.
        /// </summary>
        public BifrostService_EVENTSTREAM()
            : base(HTTPContentType.EVENTSTREAM)
        { }

        #endregion

        #region BifrostService_EVENTSTREAM(IHTTPConnection)

        /// <summary>
        /// EVENTSTREAM content representation.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        public BifrostService_EVENTSTREAM(IHTTPConnection IHTTPConnection)
            : base(IHTTPConnection, HTTPContentType.EVENTSTREAM)
        {
            this.CallingAssembly = Assembly.GetExecutingAssembly();
        }

        #endregion

        #endregion


        #region GetEvents()

        /// <summary>
        /// Subscribe to the events of a graph.
        /// </summary>
        public override HTTPResponse GetEvents()
        {

            var _RequestHeader      = IHTTPConnection.InHTTPRequest;
            var _LastEventId        = 0UL;
            var _Client_LastEventId = 0UL;
            var _EventSource        = IHTTPConnection.URLMapping.EventSource("GraphEvents");

            if (_RequestHeader.TryGet<UInt64>("Last-Event-Id", out _Client_LastEventId))
                _LastEventId = _Client_LastEventId + 1;

            var _Random = new Random();
            _EventSource.Submit("vertexadded", "{\"radius\": " + _Random.Next(5, 50) + ", \"x\": " + _Random.Next(50, 550) + ", \"y\": " + _Random.Next(50, 350) + "}");

            //var _ResourceContent = new StringBuilder();
            //_ResourceContent.AppendLine("event:vertexadded");
            //_ResourceContent.AppendLine("id: " + _LastEventId);
            //_ResourceContent.Append("data: ");
            //_ResourceContent.Append("{\"radius\": " + _Random.Next(5, 50));
            //_ResourceContent.Append(", \"x\": "     + _Random.Next(50, 550));
            //_ResourceContent.Append(", \"y\": "     + _Random.Next(50, 350) + "}");
            //_ResourceContent.AppendLine().AppendLine();

            var _ResourceContent = _EventSource.GetEvents(_Client_LastEventId);
            var _ResourceContent2 = _ResourceContent.Select(e => e.ToString()).Aggregate((a, b) => { return a + Environment.NewLine + b; });
            var _ResourceContent3 = _ResourceContent2.ToUTF8Bytes();

            return new HTTPResponseBuilder()
                        {
                            HTTPStatusCode = HTTPStatusCode.OK,
                            ContentType    = HTTPContentType.EVENTSTREAM,
                            ContentLength  = (UInt64) _ResourceContent3.Length,
                            CacheControl   = "no-cache",
                            Connection     = "keep-alive",
                            Content        = _ResourceContent3
                        };

        }

        #endregion

    }

}
