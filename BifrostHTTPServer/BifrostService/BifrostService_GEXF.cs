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
using System.Text;
using System.Reflection;

using de.ahzf.Illias.Commons;
using de.ahzf.Vanaheimr.Hermod;
using de.ahzf.Vanaheimr.Hermod.HTTP;
using de.ahzf.Vanaheimr.Blueprints;
using System.Collections.Generic;

#endregion

namespace de.ahzf.Vanaheimr.Bifrost.HTTP.Server
{

    /// <summary>
    /// GEXF content representation.
    /// </summary>
    public class BifrostService_GEXF : ABifrostService
    {

        #region Constructor(s)

        #region BifrostService_GEXF()

        /// <summary>
        /// GEXF content representation.
        /// </summary>
        public BifrostService_GEXF()
            : base(HTTPContentType.GEXF_UTF8)
        { }

        #endregion

        #region BifrostService_GEXF(IHTTPConnection)

        /// <summary>
        /// GEXF content representation.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        public BifrostService_GEXF(IHTTPConnection IHTTPConnection)
            : base(IHTTPConnection, HTTPContentType.GEXF_UTF8)
        {
            this.CallingAssembly = Assembly.GetExecutingAssembly();
        }

        #endregion

        #endregion


        #region /graph/{GraphId}

        /// <summary>
        /// Return the graph associated with the given graph identification.
        /// </summary>
        /// <param name="GraphId">The identification of the graph to return.</param>
        public override HTTPResponse GET_GraphById(String GraphId)
        {

            var StringBuilder = new StringBuilder();
            var GraphResult   = base.GET_GraphById_protected(GraphId);

            if (GraphResult.HasErrors)
                return GraphResult.Error;

            if (GraphResult.Data != null)
            {

                StringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                StringBuilder.AppendLine("<gexf xmlns=\"http://www.gexf.net/1.2draft\" version=\"1.2\">");
                StringBuilder.AppendLine("  <meta lastmodifieddate=\"2009-03-20\">");
                StringBuilder.AppendLine("    <creator>Vanaheimr Walkyr</creator>");
                StringBuilder.AppendLine("    <description>" + GraphResult.Data.Description + "</description>");
                StringBuilder.AppendLine("  </meta>");
                StringBuilder.AppendLine("  <graph mode=\"static\" defaultedgetype=\"directed\">");
                StringBuilder.AppendLine("    <nodes>");

                GraphResult.Data.Vertices().ForEach(Vertex => StringBuilder.AppendLine("      <node id=\"" + Vertex.Id + "\" label=\"" + Vertex.Label + "\" />"));
                
                StringBuilder.AppendLine("    </nodes>");
                StringBuilder.AppendLine("    <edges>");

                GraphResult.Data.Edges().ForEach(Edge => StringBuilder.AppendLine("      <edge id=\"" + Edge.Id + "\" source=\"" + Edge.OutVertex.Id + "\" target=\"" + Edge.InVertex.Id + "\" />"));

                StringBuilder.AppendLine("    </edges>");
                StringBuilder.AppendLine("  </graph>");
                StringBuilder.AppendLine("</gexf>");

            }

            return new HTTPResponseBuilder()
            {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = HTTPContentTypes.First(),
                Content        = StringBuilder.ToString().ToUTF8Bytes()
            };

        }

        #endregion


    }

}