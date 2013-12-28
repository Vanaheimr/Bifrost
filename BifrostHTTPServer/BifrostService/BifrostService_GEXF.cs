/*
 * Copyright (c) 2010-2014, Achim 'ahzf' Friedland <achim@graphdefined.org>
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

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Hermod;
using eu.Vanaheimr.Hermod.HTTP;
using eu.Vanaheimr.Balder;
using System.Collections.Generic;

#endregion

namespace eu.Vanaheimr.Bifrost.HTTP.Server
{

    /// <summary>
    /// GEXF content representation.
    /// </summary>
    public class BifrostService_GEXF<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                     TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                     TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                     TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

                                     : ABifrostService<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                       TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                       TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                       TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>


        where TIdVertex        : IEquatable<TIdVertex>,       IComparable<TIdVertex>,       IComparable, TValueVertex
        where TIdEdge          : IEquatable<TIdEdge>,         IComparable<TIdEdge>,         IComparable, TValueEdge
        where TIdMultiEdge     : IEquatable<TIdMultiEdge>,    IComparable<TIdMultiEdge>,    IComparable, TValueMultiEdge
        where TIdHyperEdge     : IEquatable<TIdHyperEdge>,    IComparable<TIdHyperEdge>,    IComparable, TValueHyperEdge

        where TRevIdVertex     : IEquatable<TRevIdVertex>,    IComparable<TRevIdVertex>,    IComparable, TValueVertex
        where TRevIdEdge       : IEquatable<TRevIdEdge>,      IComparable<TRevIdEdge>,      IComparable, TValueEdge
        where TRevIdMultiEdge  : IEquatable<TRevIdMultiEdge>, IComparable<TRevIdMultiEdge>, IComparable, TValueMultiEdge
        where TRevIdHyperEdge  : IEquatable<TRevIdHyperEdge>, IComparable<TRevIdHyperEdge>, IComparable, TValueHyperEdge

        where TVertexLabel     : IEquatable<TVertexLabel>,    IComparable<TVertexLabel>,    IComparable, TValueVertex
        where TEdgeLabel       : IEquatable<TEdgeLabel>,      IComparable<TEdgeLabel>,      IComparable, TValueEdge
        where TMultiEdgeLabel  : IEquatable<TMultiEdgeLabel>, IComparable<TMultiEdgeLabel>, IComparable, TValueMultiEdge
        where THyperEdgeLabel  : IEquatable<THyperEdgeLabel>, IComparable<THyperEdgeLabel>, IComparable, TValueHyperEdge

        where TKeyVertex       : IEquatable<TKeyVertex>,      IComparable<TKeyVertex>,      IComparable
        where TKeyEdge         : IEquatable<TKeyEdge>,        IComparable<TKeyEdge>,        IComparable
        where TKeyMultiEdge    : IEquatable<TKeyMultiEdge>,   IComparable<TKeyMultiEdge>,   IComparable
        where TKeyHyperEdge    : IEquatable<TKeyHyperEdge>,   IComparable<TKeyHyperEdge>,   IComparable
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
        { }

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
                StringBuilder.AppendLine("    <description>" + GraphResult.Data[VertexPropertyKeyParser("Description")] + "</description>");
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
