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
using System.Collections.Generic;

using de.ahzf.Illias.Commons;
using de.ahzf.Vanaheimr.Hermod;
using de.ahzf.Vanaheimr.Hermod.HTTP;
using de.ahzf.Vanaheimr.Blueprints;

using Newtonsoft.Json.Linq;

#endregion

namespace de.ahzf.Vanaheimr.Bifrost.HTTP.Server
{

    /// <summary>
    /// XML content representation.
    /// </summary>
    public class BifrostService_XML : ABifrostService
    {

        #region Constructor(s)

        #region BifrostService_XML()

        /// <summary>
        /// XML content representation.
        /// </summary>
        public BifrostService_XML()
            : base(HTTPContentType.XML_UTF8)
        { }

        #endregion

        #region BifrostService_XML(IHTTPConnection)

        /// <summary>
        /// XML content representation.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        public BifrostService_XML(IHTTPConnection IHTTPConnection)
            : base(IHTTPConnection, HTTPContentType.XML_UTF8)
        {
            this.CallingAssembly = Assembly.GetExecutingAssembly();
        }

        #endregion

        #endregion


        #region GetRoot()

        /// <summary>
        /// Return the landing page.
        /// </summary>
        public override HTTPResponse GET_Root()
        {
            return GET_Graphs();
        }

        #endregion

        #region GET_Graphs()

        /// <summary>
        /// Return an overview of all graphs.
        /// </summary>
        public override HTTPResponse GET_Graphs()
        {

            var _Content = new JObject(
                                   new JProperty("AllGraphs",
                                       new JObject(
                                           from Graph in GraphServer select new JProperty(Graph.Id.ToString(), Graph.Description)
                                       )
                                   )
                               ).ToString();

            return new HTTPResponseBuilder()
            {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = HTTPContentType.JSON_UTF8,
                Content        = _Content.ToUTF8Bytes()
            };

        }

        #endregion



        #region (protected) VertexSerialization(...)

        /// <summary>
        /// Serialize a single vertex.
        /// </summary>
        /// <param name="Vertex">A single vertex.</param>
        /// <returns>The serialized vertex.</returns>
        protected override Byte[] VertexSerialization(IGenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                                      UInt64, Int64, String, String, Object,
                                                                      UInt64, Int64, String, String, Object,
                                                                      UInt64, Int64, String, String, Object> Vertex)
        {

            return new JObject(
                       new JProperty("PropertyVertex",
                           new JObject(
                               from   KeyValuePair
                               in     Vertex
                               select new JProperty(KeyValuePair.Key, KeyValuePair.Value)
                           )
                       )
                     ).ToString().
                       ToUTF8Bytes();

        }

        #endregion

        #region (protected) VerticesSerialization(...)

        /// <summary>
        /// Serialize an enumeration of vertices.
        /// </summary>
        /// <param name="Vertices">An enumeration of vertices.</param>
        /// <returns>The serialized vertex.</returns>
        protected override Byte[] VerticesSerialization(IEnumerable<IGenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                                                           UInt64, Int64, String, String, Object,
                                                                                           UInt64, Int64, String, String, Object,
                                                                                           UInt64, Int64, String, String, Object>> Vertices)
        {

            return new JArray(  ( from Vertex
                                  in   Vertices
                                  select
                                      new JObject(
                                          new JProperty("PropertyVertex",
                                              new JObject(
                                                  from   KeyValuePair
                                                  in     Vertex
                                                  select new JProperty(KeyValuePair.Key, KeyValuePair.Value)
                                              )
                                          )
                                      )
                                  ).ToArray()
                              ).ToString().
                                ToUTF8Bytes();

        }

        #endregion


    }

}