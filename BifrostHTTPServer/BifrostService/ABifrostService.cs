/*
 * Copyright (c) 2010-2013, Achim 'ahzf' Friedland <achim@graph-database.org>
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
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Balder;
using eu.Vanaheimr.Hermod.HTTP;

#endregion

namespace eu.Vanaheimr.Bifrost.HTTP.Server
{

    /// <summary>
    /// This class provides the generic IBifrostService functionality
    /// without being bound to any specific content representation.
    /// </summary>
    public abstract class ABifrostService<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                          TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                          TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                          TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

                                          : AHTTPService,
                                            IBifrostService<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
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

        public Func<String, TIdVertex>     VertexIdParser    { get; set; }
        public Func<String, TIdEdge>       EdgeIdParser      { get; set; }
        public Func<String, TIdMultiEdge>  MultiEdgeIdParser { get; set; }
        public Func<String, TIdHyperEdge>  HyperEdgeIdParser { get; set; }

        public Func<String, TKeyVertex>    VertexPropertyKeyParser     { get; set; }
        public Func<String, TKeyEdge>      EdgePropertyKeyParser       { get; set; }
        public Func<String, TKeyMultiEdge> MultiEdgePropertyKeyParser  { get; set; }
        public Func<String, TKeyHyperEdge> HeyperEdgePropertyKeyParser { get; set; }


        #region Data

        private ThreadLocal<HTTPResponse> HTTPErrorResponse;


        /// <summary>
        /// The graph parameter for this http request.
        /// </summary>
        protected ThreadLocal<IReadOnlyGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                            TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                            TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                            TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Graph;

        /// <summary>
        /// The vertex parameter for this http request.
        /// </summary>
        protected ThreadLocal<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Vertex;

        /// <summary>
        /// The edge parameter for this http request.
        /// </summary>
        protected ThreadLocal<IReadOnlyGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Edge;

        /// <summary>
        /// The multiedge parameter for this http request.
        /// </summary>
        protected ThreadLocal<IReadOnlyGenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> MultiEdge;

        /// <summary>
        /// The hyperedge parameter for this http request.
        /// </summary>
        protected ThreadLocal<IReadOnlyGenericPropertyHyperEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> HyperEdge;



        /// <summary>
        /// The 'Callback' parameter within the query string
        /// </summary>
        protected ThreadLocal<String> Callback;

        /// <summary>
        /// The 'Skip' parameter within the query string
        /// </summary>
        protected ThreadLocal<UInt64> Skip;

        /// <summary>
        /// The 'Take' parameter within the query string
        /// </summary>
        protected ThreadLocal<UInt64> Take;

        #endregion

        #region Properties

        /// <summary>
        /// The internal GraphServer object.
        /// </summary>
        public IBifrostHTTPServer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                  TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                  TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                  TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> GraphServer { get; set; }

        #endregion

        #region Constructor(s)

        #region ABifrostService()

        /// <summary>
        /// Creates a new abstract graph service.
        /// </summary>
        public ABifrostService()
        { }

        #endregion

        #region ABifrostService(HTTPContentType)

        /// <summary>
        /// Creates a new abstract graph service.
        /// </summary>
        /// <param name="HTTPContentType">A content type.</param>
        public ABifrostService(HTTPContentType HTTPContentType)
            : base(HTTPContentType)
        { }

        #endregion

        #region ABifrostService(HTTPContentTypes)

        /// <summary>
        /// Creates a new abstract graph service.
        /// </summary>
        /// <param name="HTTPContentTypes">A content type.</param>
        public ABifrostService(IEnumerable<HTTPContentType> HTTPContentTypes)
            : base(HTTPContentTypes)
        { }

        #endregion

        #region ABifrostService(IHTTPConnection, HTTPContentType)

        /// <summary>
        /// Creates a new abstract graph service.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        /// <param name="HTTPContentType">A http content type.</param>
        public ABifrostService(IHTTPConnection IHTTPConnection, HTTPContentType HTTPContentType)
            : base(IHTTPConnection, HTTPContentType, "GraphServer.resources.")
        {

            this.Graph    = new ThreadLocal<IReadOnlyGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Vertex   = new ThreadLocal<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Edge     = new ThreadLocal<IReadOnlyGenericPropertyEdge  <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Callback = new ThreadLocal<String>();
            this.Skip     = new ThreadLocal<UInt64>();
            this.Take     = new ThreadLocal<UInt64>();

        }

        #endregion

        #region ABifrostService(IHTTPConnection, HTTPContentTypes)

        /// <summary>
        /// Creates a new abstract graph service.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        /// <param name="HTTPContentTypes">An enumeration of content types.</param>
        public ABifrostService(IHTTPConnection IHTTPConnection, IEnumerable<HTTPContentType> HTTPContentTypes)
            : base(IHTTPConnection, HTTPContentTypes, "GraphServer.resources.")
        {

            this.Graph    = new ThreadLocal<IReadOnlyGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Vertex   = new ThreadLocal<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Edge     = new ThreadLocal<IReadOnlyGenericPropertyEdge  <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Callback = new ThreadLocal<String>();
            this.Skip     = new ThreadLocal<UInt64>();
            this.Take     = new ThreadLocal<UInt64>();

        }

        #endregion

        #region (protected) ABifrostService(IHTTPConnection, HTTPContentType, ResourcePath)

        /// <summary>
        /// Creates a new abstract graph service.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        /// <param name="HTTPContentType">A http content type.</param>
        /// <param name="ResourcePath">The path to internal resources.</param>
        protected ABifrostService(IHTTPConnection IHTTPConnection, HTTPContentType HTTPContentType, String ResourcePath)
            : base(IHTTPConnection, HTTPContentType, ResourcePath)
        {

            this.Graph    = new ThreadLocal<IReadOnlyGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Vertex   = new ThreadLocal<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Edge     = new ThreadLocal<IReadOnlyGenericPropertyEdge  <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Callback = new ThreadLocal<String>();
            this.Skip     = new ThreadLocal<UInt64>();
            this.Take     = new ThreadLocal<UInt64>();

        }

        #endregion

        #region (protected) ABifrostService(IHTTPConnection, HTTPContentTypes, ResourcePath)

        /// <summary>
        /// Creates a new abstract graph service.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        /// <param name="HTTPContentTypes">An enumeration of http content types.</param>
        /// <param name="ResourcePath">The path to internal resources.</param>
        protected ABifrostService(IHTTPConnection IHTTPConnection, IEnumerable<HTTPContentType> HTTPContentTypes, String ResourcePath)
            : base(IHTTPConnection, HTTPContentTypes, ResourcePath)
        {

            this.Graph    = new ThreadLocal<IReadOnlyGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Vertex   = new ThreadLocal<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Edge     = new ThreadLocal<IReadOnlyGenericPropertyEdge  <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

            this.Callback = new ThreadLocal<String>();
            this.Skip     = new ThreadLocal<UInt64>();
            this.Take     = new ThreadLocal<UInt64>();

        }

        #endregion

        #endregion


        #region (protected) ParseAndCheckGraphId(GraphId)

        /// <summary>
        /// Parse and check the HTTP parameter GraphId.
        /// </summary>
        /// <param name="GraphId"></param>
        protected void ParseAndCheckGraphId(String GraphId)
        {

            IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                  TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                  TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                  TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> _Graph = null;

            if (!GraphServer.TryGetGraph(VertexIdParser(GraphId), out _Graph))

                HTTPErrorResponse = new ThreadLocal<HTTPResponse>(
                    () => new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotFound, "The given 'GraphId' is unknown!").Error);

            else
                Graph.Value = _Graph;

        }

        #endregion

        #region (protected) ParseAndCheckVertexId(VertexId)

        /// <summary>
        /// Parse and check the HTTP parameter VertexId.
        /// </summary>
        /// <param name="VertexId"></param>
        protected void ParseAndCheckVertexId(String VertexId)
        {

            IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> _Vertex = null;

            if (!Graph.Value.TryGetVertexById(VertexIdParser(VertexId), out _Vertex))

                HTTPErrorResponse = new ThreadLocal<HTTPResponse>(
                    () => new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotFound, "The given 'VertexId' is unknown!").Error);

            else
                Vertex.Value = _Vertex;

        }

        #endregion

        #region (protected) ParseEdgeId(EdgeId)

        /// <summary>
        /// Parse and check the parameter EdgeId.
        /// </summary>
        /// <param name="EdgeId"></param>
        protected void ParseAndCheckEdgeId(String EdgeId)
        {

            IReadOnlyGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                         TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                         TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                         TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> _Edge = null;

            if (!Graph.Value.TryGetEdgeById(EdgeIdParser(EdgeId), out _Edge))

                HTTPErrorResponse = new ThreadLocal<HTTPResponse>(
                    () => new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotFound, "The given 'EdgeId' is unknown!").Error);

            else
                Edge.Value = _Edge;

        }

        #endregion

        #region (protected) ParseMultiEdgeId(MultiEdgeId)

        /// <summary>
        /// Parse and check the parameter MultiEdgeId.
        /// </summary>
        /// <param name="MultiEdgeId"></param>
        protected void ParseAndCheckMultiEdgeId(String MultiEdgeId)
        {

            IReadOnlyGenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                              TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                              TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                              TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> _MultiEdge = null;

            if (!Graph.Value.TryGetMultiEdgeById(MultiEdgeIdParser(MultiEdgeId), out _MultiEdge))

                HTTPErrorResponse = new ThreadLocal<HTTPResponse>(
                    () => new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotFound, "The given 'MultiEdgeId' is unknown!").Error);

            else
                MultiEdge.Value = _MultiEdge;

        }

        #endregion

        #region (protected) ParseHyperEdgeId(EdgeId)

        /// <summary>
        /// Parse and check the parameter HyperEdgeId.
        /// </summary>
        /// <param name="HyperEdgeId"></param>
        protected void ParseAndCheckHyperEdgeId(String HyperEdgeId)
        {

            IReadOnlyGenericPropertyHyperEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                              TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                              TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                              TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> _HyperEdge = null;

            if (!Graph.Value.TryGetHyperEdgeById(HyperEdgeIdParser(HyperEdgeId), out _HyperEdge))

                HTTPErrorResponse = new ThreadLocal<HTTPResponse>(
                    () => new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotFound, "The given 'HyperEdgeId' is unknown!").Error);

            else
                HyperEdge.Value = _HyperEdge;

        }

        #endregion




        #region GET_Root()

        /// <summary>
        /// Get the landing page.
        /// </summary>
        public virtual HTTPResponse GET_Root()
        {
            return HTTPTools.MovedTemporarily("/graphs");
        }

        #endregion

        #region /graphs

        #region GET_Graphs()

        /// <summary>
        /// Get a list of all graphs.
        /// </summary>
        /// <example>GET /graphs</example>
        public virtual HTTPResponse GET_Graphs()
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        #endregion

        #region FILTER_Graphs()

        /// <summary>
        /// Get a list of all graphs.
        /// </summary>
        //  FILTER /graphs
        //  "HTTPBody: {",
        //     "\"GraphFilter\" : \"...\"",
        //     "\"SELECT\"      : [ \"Name\", \"Age\" ],",
        //  "}",
        public virtual HTTPResponse FILTER_Graphs()
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        #endregion

        #region COUNT_Graphs()

        /// <summary>
        /// Get a list of all graphs.
        /// </summary>
        //  COUNT /graphs
        //  "HTTPBody: {",
        //     "\"GraphFilter\" : \"...\"",
        //     "\"SELECT\"      : [ \"Name\", \"Age\" ],",
        //  "}",
        public virtual HTTPResponse COUNT_Graphs()
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        protected HTTPResult<UInt64> COUNT_Graphs_protected()
        {
            return new HTTPResult<UInt64>(GraphServer.NumberOfGraphs());
        }

        #endregion

        #region CREATE_Graph()

        /// <summary>
        /// Create a new graph having an autogenerated Id.
        /// </summary>
        public HTTPResponse CREATE_Graph()
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        #endregion

        #endregion

        #region /graph/{GraphId}

        #region GET_GraphById(GraphId)

        /// <summary>
        /// Return general information of a graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        public virtual HTTPResponse GET_GraphById(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        /// <summary>
        /// Return general information of a graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        protected HTTPResult<IReadOnlyGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>

            GET_GraphById_protected(String GraphId)

        {

            ParseAndCheckGraphId(GraphId);

            return new HTTPResult<IReadOnlyGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>(Graph.Value);

        }

        #endregion

        #region CREATE_Graph(GraphId)

        /// <summary>
        /// Create a new graph.
        /// </summary>
        public virtual HTTPResponse CREATE_Graph(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        #endregion

        #endregion

        #region /graph/{GraphId}/p

        public HTTPResponse GET_GraphProperties(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        #endregion

        #region /graph/{GraphId}/p/{Key}

        public HTTPResponse GET_GraphProperty(String GraphId, String Key)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        #endregion


        #region GET_VertexById(GraphId, VertexId)

        /// <summary>
        /// Return the vertex referenced by the given vertex identifier.
        /// If no vertex is referenced by the identifier return null.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        /// <param name="VertexId">The vertex identification.</param>
        public virtual HTTPResponse GET_VertexById(String GraphId, String VertexId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        protected HTTPResult<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                            TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                            TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                            TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>

            GET_VertexById_protected(String GraphId, String VertexId)

        {

            ParseAndCheckGraphId(GraphId);
            ParseAndCheckVertexId(VertexId);

            return new HTTPResult<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>(Vertex.Value);

        }

        #endregion


        #region GET_VerticesById(GraphId)

        /// <summary>
        /// Return the vertices referenced by the given array of vertex identifiers.
        /// If no vertex is referenced by a given identifier this value will be
        /// skipped.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        /// <remarks>Include a JSON array having vertex identifiers.</remarks>
        public virtual HTTPResponse GET_VerticesById(String GraphId)
        {

            throw new NotImplementedException();

            //#region Process request

            //IEnumerable<UInt64> Ids = null;

            //List<String> List = null;
            //var QueryString = IHTTPConnection.InHTTPRequest.QueryString;

            //if (QueryString != null)
            //{
            //    if (QueryString.TryGetValue("Id", out List))
            //        if (List != null && List.Count >= 1)
            //            Ids = from s in List select UInt64.Parse(s);
            //}

            //#endregion

            //var Vertices = GraphServer.GetPropertyGraph(GraphId).VerticesById(Ids.ToArray());

            //#region Process response

            //if (Vertices == null || !Vertices.Any())
            //    return HTTPErrors.HTTPErrorResponse(IHTTPConnection.InHTTPRequest, HTTPStatusCode.NotFound);

            //var Content = VerticesSerialization(Vertices);
            //if (Content == null)
            //    return HTTPErrors.HTTPErrorResponse(IHTTPConnection.InHTTPRequest, HTTPStatusCode.BadRequest);

            //return new HTTPResponseBuilder()
            //{
            //    HTTPStatusCode = HTTPStatusCode.OK,
            //    ContentType    = this.HTTPContentTypes.First(),
            //    Content        = Content
            //};

            //#endregion

        }

        #endregion


        #region GET_Vertices(GraphId)

        /// <summary>
        /// Return all vertices of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        public virtual HTTPResponse GET_Vertices(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        /// <summary>
        /// Return all vertices of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        protected HTTPResult<IEnumerable<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>>

            GET_Vertices_protected(String GraphId)

        {

            ParseAndCheckGraphId(GraphId);
            ParseSkipParameter();
            ParseTakeParameter(25);

            // Return the appropriate enumeration
            var _Vertices = Graph.Value.Vertices();

            if (Skip.Value != 0)
                _Vertices = _Vertices.Skip(Skip.Value);

            if (Take.Value != 0)
                _Vertices = _Vertices.Take(Take.Value);

            return new HTTPResult<IEnumerable<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                             TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                             TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                             TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>>(_Vertices);

        }

        #endregion

        #region FILTER_Vertices(GraphId)

        /// <summary>
        /// Return all vertices of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        public virtual HTTPResponse FILTER_Vertices(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        /// <summary>
        /// Return all vertices of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        protected HTTPResult<IEnumerable<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>>

            FILTER_Vertices_protected(String GraphId)

        {

            ParseAndCheckGraphId(GraphId);
            ParseSkipParameter();
            ParseTakeParameter(25);

            // Return the appropriate enumeration
            var _Vertices = Graph.Value.Vertices();

            if (Skip.Value != 0)
                _Vertices = _Vertices.Skip(Skip.Value);

            if (Take.Value != 0)
                _Vertices = _Vertices.Take(Take.Value);

            return new HTTPResult<IEnumerable<IReadOnlyGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                             TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                             TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                             TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>>(_Vertices);

        }

        #endregion

        #region COUNT_Vertices(GraphId)

        /// <summary>
        /// Return all vertices of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        public virtual HTTPResponse COUNT_Vertices(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        /// <summary>
        /// Return all vertices of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        protected HTTPResult<UInt64>

            COUNT_Vertices_protected(String GraphId)

        {

            ParseAndCheckGraphId(GraphId);

            return new HTTPResult<UInt64>(Graph.Value.NumberOfVertices());

        }

        #endregion











        #region GET_EdgeById(GraphId, EdgeId)

        /// <summary>
        /// Return the edge referenced by the given edge identifier.
        /// If no edge is referenced by the identifier return null.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        /// <param name="EdgeId">The edge identification.</param>
        public virtual HTTPResponse GET_EdgeById(String GraphId, String EdgeId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        protected HTTPResult<IReadOnlyGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                          TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                          TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                          TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>

            GET_EdgeById_protected(String GraphId, String EdgeId)

        {

            ParseAndCheckGraphId(GraphId);
            ParseAndCheckEdgeId(EdgeId);

            return new HTTPResult<IReadOnlyGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                               TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                               TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                               TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>(Edge.Value);

        }

        #endregion

        #region GET_Edges(GraphId)

        /// <summary>
        /// Get all edges of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        public virtual HTTPResponse GET_Edges(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        /// <summary>
        /// Return all edges of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        protected HTTPResult<IEnumerable<IReadOnlyGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                      TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                      TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                      TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>>

            GET_Edges_protected(String GraphId)

        {

            ParseAndCheckGraphId(GraphId);
            ParseSkipParameter();
            ParseTakeParameter(25);

            // Return the appropriate enumeration
            var _Edges = Graph.Value.Edges();

            if (Skip.Value != 0)
                _Edges = _Edges.Skip(Skip.Value);

            if (Take.Value != 0)
                _Edges = _Edges.Take(Take.Value);

            return new HTTPResult<IEnumerable<IReadOnlyGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>>(_Edges);

        }

        #endregion

        #region FILTER_Edges(GraphId)

        /// <summary>
        /// Return all edges of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        public virtual HTTPResponse FILTER_Edges(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        /// <summary>
        /// Return all edges of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        protected HTTPResult<IEnumerable<IReadOnlyGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                      TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                      TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                      TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>>

            FILTER_Edges_protected(String GraphId)

        {

            ParseAndCheckGraphId(GraphId);
            ParseSkipParameter();
            ParseTakeParameter(25);

            // Return the appropriate enumeration
            var _Edges = Graph.Value.Edges();

            if (Skip.Value != 0)
                _Edges = _Edges.Skip(Skip.Value);

            if (Take.Value != 0)
                _Edges = _Edges.Take(Take.Value);

            return new HTTPResult<IEnumerable<IReadOnlyGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>>(_Edges);

        }

        #endregion

        #region COUNT_Edges(GraphId)

        /// <summary>
        /// Return all edges of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        public virtual HTTPResponse COUNT_Edges(String GraphId)
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        /// <summary>
        /// Return all edges of the given graph.
        /// </summary>
        /// <param name="GraphId">The unique identification of the graph.</param>
        protected HTTPResult<UInt64>

            COUNT_edges_protected(String GraphId)

        {

            ParseAndCheckGraphId(GraphId);

            return new HTTPResult<UInt64>(Graph.Value.NumberOfEdges());

        }

        #endregion



        #region GetEvents()

        /// <summary>
        /// Subscribe to the events of this graph.
        /// </summary>
        public virtual HTTPResponse GetEvents()
        {
            return new HTTPResult<Object>(IHTTPConnection.RequestHeader, HTTPStatusCode.NotAcceptable).Error;
        }

        #endregion


        #region (protected) VertexSerialization(...)

        /// <summary>
        /// Serialize a single vertex.
        /// </summary>
        /// <param name="Vertex">A single vertex.</param>
        /// <returns>The serialized vertex.</returns>
        protected virtual Byte[] VertexSerialization(IGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                            TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                            TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                            TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Vertex)
        {
            return null;
        }

        #endregion

        #region (protected) VerticesSerialization(...)

        /// <summary>
        /// Serialize an enumeration of vertices.
        /// </summary>
        /// <param name="Vertices">An enumeration of vertices.</param>
        /// <returns>The serialized vertex.</returns>
        protected virtual Byte[] VerticesSerialization(IEnumerable<IGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                                          TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                                          TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                                          TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Vertices)
        {
            return null;
        }

        #endregion


        public HTTPResponse GET_VerticesById(string GraphId, string VertexIds)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GET_VerticesByLabel(string GraphId, string VertexLabel)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GET_OutEdgesFromVertex(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse FILTER_OutEdgesFromVertex(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GET_InEdgesFromVertex(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GET_BothEdgesFromVertex(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GET_OutVerticesFromVertex(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GET_InVerticesFromVertex(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GET_BothVerticesFromVertex(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GET_SubgraphId(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }



        public HTTPResponse FILTER_VerticesById(string GraphId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse COUNT_VerticesById(string GraphId)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse FILTER_VerticesByLabel(string GraphId, string VertexLabels)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse COUNT_VerticesByLabel(string GraphId, string VertexLabels)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse COUNT_OutEdgesFromVertex(string GraphId, string VertexId)
        {
            throw new NotImplementedException();
        }



    }

}
