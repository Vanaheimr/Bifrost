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
    /// HTML content representation.
    /// </summary>
    public class BifrostService_HTML<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
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

        #region BifrostService_HTML()

        /// <summary>
        /// HTML content representation.
        /// </summary>
        public BifrostService_HTML()
            : base(HTTPContentType.HTML_UTF8)
        { }

        #endregion

        #region BifrostService_HTML(IHTTPConnection)

        /// <summary>
        /// HTML content representation.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        public BifrostService_HTML(IHTTPConnection IHTTPConnection)
            : base(IHTTPConnection, HTTPContentType.HTML_UTF8)
        { }

        #endregion

        #endregion


        #region (private) HTMLBuilder(Headline, StringBuilderFunc)

        private String HTMLBuilder(String Headline, Action<StringBuilder> StringBuilderFunc)
        {

            var _StringBuilder = new StringBuilder();

            _StringBuilder.AppendLine("<!doctype html>");
            _StringBuilder.AppendLine("<html><head><meta charset=\"UTF-8\">");
            _StringBuilder.AppendLine("<title>GraphServer v0.1</title>");
            _StringBuilder.AppendLine("</head>");
            _StringBuilder.AppendLine("<body>");
            _StringBuilder.Append("<h2>").Append(Headline).AppendLine("</h2>");
            _StringBuilder.AppendLine("<table>");
            _StringBuilder.AppendLine("<tr>");
            _StringBuilder.AppendLine("<td style=\"width: 100px\">&nbsp;</td>");
            _StringBuilder.AppendLine("<td>");

            StringBuilderFunc(_StringBuilder);

            _StringBuilder.AppendLine("</td>");
            _StringBuilder.AppendLine("</tr>");
            _StringBuilder.AppendLine("</table>");
            _StringBuilder.AppendLine("</body>").AppendLine("</html>").AppendLine();

            return _StringBuilder.ToString();

        }

        #endregion


        #region /graphs

        /// <summary>
        /// Return an overview of all graphs.
        /// </summary>
        public override HTTPResponse GET_Graphs()
        {

            var AllGraphs = GraphServer.Select(graph => "<a href=\"/graph/" + graph.Id + "\">" + graph.Id + " - " + graph[VertexPropertyKeyParser("Description")] + "</a> " +
                                                        "<a href=\"/graph/" + graph.Id + "/vertices\">[All Vertices]</a> " +
                                                        "<a href=\"/graph/" + graph.Id + "/edges\">[All Edge]</a>").
                                        Aggregate((a, b) => a + "<br>" + b);

            return new HTTPResponseBuilder()
            {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = HTTPContentType.HTML_UTF8,
                Content        = HTMLBuilder("", StringBuilder => StringBuilder.AppendLine(AllGraphs)).ToUTF8Bytes()
            };

        }

        /// <summary>
        /// Get a list of all graphs.
        /// </summary>
        //  COUNT /graphs
        //  "HTTPBody: {",
        //     "\"GraphFilter\" : \"...\"",
        //     "\"SELECT\"      : [ \"Name\", \"Age\" ],",
        //  "}",
        public override HTTPResponse COUNT_Graphs()
        {

            var Result = base.COUNT_Graphs_protected();

            if (Result.HasErrors)
                return Result.Error;

            return new HTTPResponseBuilder()
            {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = this.HTTPContentTypes.First(),
                Content        = HTMLBuilder("Number of graphs", StringBuilder => StringBuilder.AppendLine(Result.Data.ToString())).ToUTF8Bytes()
            };

        }

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

                StringBuilder.Append("<table>");

                StringBuilder.Append("<tr><td>Id</td><td>").                  Append(GraphResult.Data.Id).                                     AppendLine("</td></tr>").
                              Append("<tr><td>RevisionId</td><td>").          Append(GraphResult.Data.RevId).                                  AppendLine("</td></tr>").
                              Append("<tr><td>Description</td><td>").         Append(GraphResult.Data[VertexPropertyKeyParser("Description")]).AppendLine("</td></tr>").
                              AppendLine("<tr><td>&nbsp;</td></tr>").
                              Append("<tr><td>Number of vertices</td><td>").  Append(GraphResult.Data.NumberOfVertices()).                     AppendLine("</td></tr>").
                              Append("<tr><td>Number of edges</td><td>").     Append(GraphResult.Data.NumberOfEdges()).                        AppendLine("</td></tr>").
                              Append("<tr><td>Number of multiedges</td><td>").Append(GraphResult.Data.NumberOfMultiEdges()).                   AppendLine("</td></tr>").
                              Append("<tr><td>Number of hyperedges</td><td>").Append(GraphResult.Data.NumberOfHyperEdges()).                   AppendLine("</td></tr>").
                              AppendLine("<tr><td>&nbsp;</td></tr>");

                GraphResult.Data.ForEach(KeyValuePair =>
                    {
                        if (!KeyValuePair.Key.Equals(GraphResult.Data.IdKey) &&
                            !KeyValuePair.Key.Equals(GraphResult.Data.RevIdKey) &&
                            !KeyValuePair.Key.Equals(GraphResult.Data.LabelKey))
                            StringBuilder.Append("<tr><td>").
                                          Append(KeyValuePair.Key.ToString()).
                                          Append("</td><td>").
                                          Append(KeyValuePair.Value.ToString()).
                                          Append("</td></tr>");
                    });


                StringBuilder.Append("</table>");

            }

            return new HTTPResponseBuilder()
            {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = this.HTTPContentTypes.First(),
                Content        = HTMLBuilder("Graph " + GraphResult.Data.Id, _StringBuilder => _StringBuilder.AppendLine(StringBuilder.ToString())).ToUTF8Bytes()
            };

        }

        #endregion




        #region GET_VertexById(GraphId, VertexId)

        /// <summary>
        /// Return the vertex referenced by the given vertex identifier.
        /// If no vertex is referenced by the identifier return null.
        /// </summary>
        /// <param name="GraphId">The identification of the graph.</param>
        /// <param name="VertexId">The vertex identification.</param>
        public override HTTPResponse GET_VertexById(String GraphId, String VertexId)
        {

            var StringBuilder = new StringBuilder();
            var VertexResult  = base.GET_VertexById_protected(GraphId, VertexId);

            if (VertexResult.HasErrors)
                return VertexResult.Error;

            if (VertexResult.Data.Any())
            {

                StringBuilder.Append("<table>");

                VertexResult.Data.ForEach(KeyValuePair =>
                    StringBuilder.Append("<tr><td>").
                                  Append(KeyValuePair.Key.ToString()).
                                  Append("</td><td>").
                                  Append(KeyValuePair.Value.ToString()).
                                  Append("</td></tr>"));

                StringBuilder.Append("</table>");

            }

            return new HTTPResponseBuilder()
            {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = this.HTTPContentTypes.First(),
                Content        = HTMLBuilder("Vertex", _StringBuilder => _StringBuilder.Append(StringBuilder.ToString())).ToUTF8Bytes()
            };

        }

        #endregion

        #region Vertices(GraphId)

        /// <summary>
        /// Return all vertices of the given graph.
        /// </summary>
        /// <param name="GraphId">The identification of the graph.</param>
        public override HTTPResponse GET_Vertices(String GraphId)
        {
            
            var StringBuilder  = new StringBuilder();
            var VerticesResult = base.GET_Vertices_protected(GraphId);

            if (VerticesResult.HasErrors)
                return VerticesResult.Error;

            if (VerticesResult.Data.Any())
            {

                StringBuilder.Append("<table>");

                VerticesResult.Data.ForEach(Vertex =>
                    {

                        StringBuilder.Append("<tr><td>");
                        StringBuilder.Append("<table>");

                        Vertex.ForEach(KeyValuePair =>
                            StringBuilder.Append("<tr><td>").
                                          Append(KeyValuePair.Key.ToString()).
                                          Append("</td><td>").
                                          Append((KeyValuePair.Value != null) ? KeyValuePair.Value.ToString() : "").
                                          Append("</td></tr>"));

                        StringBuilder.Append("</table>");
                        StringBuilder.AppendLine("</td></tr>");

                    });

                StringBuilder.Append("</table>");

            }

            return new HTTPResponseBuilder() {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = this.HTTPContentTypes.First(),
                Content        = HTMLBuilder("All vertices", sb => sb.Append(StringBuilder.ToString())).ToUTF8Bytes()
            };

        }

        #endregion


        #region GET_EdgeById(GraphId, VertexId)

        /// <summary>
        /// Return the edge referenced by the given edge identifier.
        /// If no edge is referenced by the identifier return null.
        /// </summary>
        /// <param name="GraphId">The identification of the graph.</param>
        /// <param name="EdgeId">The edge identification.</param>
        public override HTTPResponse GET_EdgeById(String GraphId, String EdgeId)
        {

            var StringBuilder = new StringBuilder();
            var EdgeResult    = base.GET_EdgeById_protected(GraphId, EdgeId);

            if (EdgeResult.HasErrors)
                return EdgeResult.Error;

            if (EdgeResult.Data.Any())
            {

                StringBuilder.Append("<table>");

                EdgeResult.Data.ForEach(KeyValuePair =>
                    StringBuilder.Append("<tr><td>").
                                  Append(KeyValuePair.Key.ToString()).
                                  Append("</td><td>").
                                  Append(KeyValuePair.Value.ToString()).
                                  Append("</td></tr>"));

                StringBuilder.Append("</table>");

            }

            return new HTTPResponseBuilder()
            {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = this.HTTPContentTypes.First(),
                Content        = HTMLBuilder("Edge", sb => sb.Append(StringBuilder.ToString())).ToUTF8Bytes()
            };

        }

        #endregion

        #region Edges(GraphId)

        /// <summary>
        /// Return all edges of the given graph.
        /// </summary>
        /// <param name="GraphId">The identification of the graph.</param>
        public override HTTPResponse GET_Edges(String GraphId)
        {

            var StringBuilder = new StringBuilder();
            var EdgesResult   = base.GET_Edges_protected(GraphId);

            if (EdgesResult.HasErrors)
                return EdgesResult.Error;

            if (EdgesResult.Data.Any())
            {

                StringBuilder.AppendLine("<table>");

                EdgesResult.Data.ForEach(Edge =>
                {

                    StringBuilder.Append("<tr><td>");
                    StringBuilder.Append("<table>");

                    Edge.ForEach(KeyValuePair =>
                        StringBuilder.Append("<tr><td>").
                                      Append(KeyValuePair.Key.ToString()).
                                      Append("</td><td>").
                                      Append(KeyValuePair.Value.ToString()).
                                      Append("</td></tr>"));

                    StringBuilder.Append("</table>");
                    StringBuilder.AppendLine("</td></tr>");

                });

                StringBuilder.Append("</table>");

            }

            return new HTTPResponseBuilder()
            {
                HTTPStatusCode = HTTPStatusCode.OK,
                ContentType    = this.HTTPContentTypes.First(),
                Content        = HTMLBuilder("All edges", sb => sb.Append(StringBuilder.ToString())).ToUTF8Bytes()
            };

        }

        #endregion


        #region (protected) VerticesSerialization(...)

        /// <summary>
        /// Serialize an enumeration of vertices.
        /// </summary>
        /// <param name="Vertices">An enumeration of vertices.</param>
        /// <returns>The serialized vertex.</returns>
        protected override Byte[] VerticesSerialization(IEnumerable<IGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Vertices)
        {

            return HTMLBuilder("www.graph-database.org v0.1", b =>
            {

                b.AppendLine("Vertices<br>");

                foreach (var Vertex in Vertices)
                {

                    foreach (var KVP in Vertex)
                        b.Append(KVP.Key).
                          Append(" = ").
                          Append(KVP.Value.ToString()).
                          AppendLine("<br>");

                    b.AppendLine("<br>");

                }

            }).ToUTF8Bytes();

        }

        #endregion


    }

}
