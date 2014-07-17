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

using eu.Vanaheimr.Hermod.HTTP;

#endregion

namespace eu.Vanaheimr.Bifrost.HTTP.Server
{

    ///// <summary>
    ///// The base inetrface for all graph services.
    ///// </summary>
    ////[HTTPService(Host: "localhost:8080", ForceAuthentication: true)]
    //[HTTPService(HostAuthentication: false)]
    //public interface IBifrostService<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

    //                                 : IHTTPBaseService


    //    where TIdVertex        : IEquatable<TIdVertex>,       IComparable<TIdVertex>,       IComparable, TValueVertex
    //    where TIdEdge          : IEquatable<TIdEdge>,         IComparable<TIdEdge>,         IComparable, TValueEdge
    //    where TIdMultiEdge     : IEquatable<TIdMultiEdge>,    IComparable<TIdMultiEdge>,    IComparable, TValueMultiEdge
    //    where TIdHyperEdge     : IEquatable<TIdHyperEdge>,    IComparable<TIdHyperEdge>,    IComparable, TValueHyperEdge

    //    where TRevIdVertex     : IEquatable<TRevIdVertex>,    IComparable<TRevIdVertex>,    IComparable, TValueVertex
    //    where TRevIdEdge       : IEquatable<TRevIdEdge>,      IComparable<TRevIdEdge>,      IComparable, TValueEdge
    //    where TRevIdMultiEdge  : IEquatable<TRevIdMultiEdge>, IComparable<TRevIdMultiEdge>, IComparable, TValueMultiEdge
    //    where TRevIdHyperEdge  : IEquatable<TRevIdHyperEdge>, IComparable<TRevIdHyperEdge>, IComparable, TValueHyperEdge

    //    where TVertexLabel     : IEquatable<TVertexLabel>,    IComparable<TVertexLabel>,    IComparable, TValueVertex
    //    where TEdgeLabel       : IEquatable<TEdgeLabel>,      IComparable<TEdgeLabel>,      IComparable, TValueEdge
    //    where TMultiEdgeLabel  : IEquatable<TMultiEdgeLabel>, IComparable<TMultiEdgeLabel>, IComparable, TValueMultiEdge
    //    where THyperEdgeLabel  : IEquatable<THyperEdgeLabel>, IComparable<THyperEdgeLabel>, IComparable, TValueHyperEdge

    //    where TKeyVertex       : IEquatable<TKeyVertex>,      IComparable<TKeyVertex>,      IComparable
    //    where TKeyEdge         : IEquatable<TKeyEdge>,        IComparable<TKeyEdge>,        IComparable
    //    where TKeyMultiEdge    : IEquatable<TKeyMultiEdge>,   IComparable<TKeyMultiEdge>,   IComparable
    //    where TKeyHyperEdge    : IEquatable<TKeyHyperEdge>,   IComparable<TKeyHyperEdge>,   IComparable

    //{

    //    /// <summary>
    //    /// The internal graph server.
    //    /// </summary>
    //    IBifrostHTTPServer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                       TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                       TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                       TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> GraphServer { get; set; }


    //    #region Events

    //    /// <summary>
    //    /// Get Events
    //    /// </summary>
    //    /// <returns>Endless text</returns>
    //    [HTTPEventMappingAttribute("GraphEvents", "/Events", HTTPMethods.GET), NoAuthentication]
    //    HTTPResponse GetEvents();

    //    #endregion



    //    #region /graphs

    //    /// <summary>
    //    /// Get a list of all graphs.
    //    /// </summary>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graphs"), NoAuthentication]
    //    HTTPResponse GET_Graphs();

    //    /// <summary>
    //    /// Get a list of all graphs.
    //    /// </summary>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    //  "FILTER /graphs",
    //    //  "HTTPBody: {",
    //    //    "\"GraphFilter\" : \"...\"",
    //    //    "\"SELECT\"      : [ \"Name\", \"Age\" ],",
    //    //  "}",
    //    [HTTPMapping(HTTPMethods.FILTER, "/graphs"), NoAuthentication]
    //    HTTPResponse FILTER_Graphs();

    //    /// <summary>
    //    /// Get a list of all graphs.
    //    /// </summary>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    //  "COUNT /graphs",
    //    //  "HTTPBody: {",
    //    //    "\"GraphFilter\" : \"...\"",
    //    //    "\"SELECT\"      : [ \"Name\", \"Age\" ],",
    //    //  "}",
    //    [HTTPMapping(HTTPMethods.COUNT, "/graphs"), NoAuthentication]
    //    HTTPResponse COUNT_Graphs();

    //    /// <summary>
    //    /// Create a new graph having an autogenerated Id.
    //    /// </summary>
    //    [HTTPMapping(HTTPMethods.CREATE, "/graphs"), NoAuthentication]
    //    HTTPResponse CREATE_Graph();

    //    #endregion

    //    #region /graph/{GraphId}

    //    /// <summary>
    //    /// Return the general information of a graph.
    //    /// </summary>
    //    /// <param name="GraphId">The identification of the graph.</param>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}"), NoAuthentication]
    //    HTTPResponse GET_GraphById(String GraphId);

    //    /// <summary>
    //    /// Create a new graph.
    //    /// </summary>
    //    [HTTPMapping(HTTPMethods.CREATE, "/graph/{GraphId}"), NoAuthentication]
    //    HTTPResponse CREATE_Graph(String GraphId);

    //    #endregion

    //    #region /graph/{GraphId}/p

    //    /// <summary>
    //    /// Return the general information of a graph.
    //    /// </summary>
    //    /// <param name="GraphId">The identification of the graph.</param>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/p"), NoAuthentication]
    //    HTTPResponse GET_GraphProperties(String GraphId);

    //    #endregion

    //    #region /graph/{GraphId}/p/{Key}

    //    /// <summary>
    //    /// Return the general information of a graph.
    //    /// </summary>
    //    /// <param name="GraphId">The identification of the graph.</param>
    //    /// <param name="Key">The key of the property to return.</param>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/p/{Key}"), NoAuthentication]
    //    HTTPResponse GET_GraphProperty(String GraphId, String Key);

    //    #endregion



    //    // Invalid characters for Ids & Labels: ","
    //    // QueryString ends with "#"! remaining is called fragment!
    //    // QueryString ";" may be used instead of "&"!

    //    #region Vertex methods

    //    #region GET_VertexById(GraphId, VertexId)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SELECT"></httpparam>
    //    /// <httpparam name="SELECTEdges"></httpparam>
    //    /// <httpparam name="SELECTMultiEdges"></httpparam>
    //    /// <httpparam name="SELECTHyperEdges"></httpparam>
    //    /// <httpparam name="RevId"></httpparam>
    //    /// <example>GET /graph/1/vertex/Alice</example>
    //    /// <example>GET /graph/1/vertex/Alice?SELECT=Name,Age</example>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertex/{VertexId}"), NoAuthentication]
    //    HTTPResponse GET_VertexById(String GraphId, String VertexId);

    //    #endregion

    //    #region GET|FILTER|COUNT_VerticesById(GraphId, VertexIds)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexIds">An array of vertex identifiers.</param>
    //    /// <httpparam name="SELECT"></httpparam>
    //    /// <httpparam name="SELECTEdges"></httpparam>
    //    /// <httpparam name="SELECTMultiEdges"></httpparam>
    //    /// <httpparam name="SELECTHyperEdges"></httpparam>
    //    /// <httpparam name="RevId"></httpparam>
    //    /// <example>GET /graph/1/vertices/ids/Alice,Bob,Carol?SELECT=Name,Age</example>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertices/ids/{VertexIds}"), NoAuthentication]
    //    HTTPResponse GET_VerticesById(String GraphId, String VertexIds);

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <httpparam name="SELECT"></httpparam>
    //    /// <httpparam name="SELECTEdges"></httpparam>
    //    /// <httpparam name="SELECTMultiEdges"></httpparam>
    //    /// <httpparam name="SELECTHyperEdges"></httpparam>
    //    /// <httpparam name="RevId"></httpparam>
    //    /// <example>FILTER /graph/1/vertices/ids?SELECT=Name,Age HTTPBody: { "VertexIds" : [ "Alice", "Bob", "Carol" ], "VertexFilter" : "..." }</example>
    //    [HTTPMapping(HTTPMethods.FILTER, "/graph/{GraphId}/vertices/ids"), NoAuthentication]
    //    HTTPResponse FILTER_VerticesById(String GraphId);

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <httpparam name="SELECT"></httpparam>
    //    /// <httpparam name="SELECTEdges"></httpparam>
    //    /// <httpparam name="SELECTMultiEdges"></httpparam>
    //    /// <httpparam name="SELECTHyperEdges"></httpparam>
    //    /// <httpparam name="RevId"></httpparam>
    //    /// <example>COUNT /graph/1/vertices/ids HTTPBody: { "VertexIds" : [ "Alice", "Bob", "Carol" ], "VertexFilter" : "..." }</example>
    //    [HTTPMapping(HTTPMethods.COUNT, "/graph/{GraphId}/vertices/ids"), NoAuthentication]
    //    HTTPResponse COUNT_VerticesById(String GraphId);

    //    #endregion

    //    #region GET|FILTER|COUNT_VerticesByLabel(GraphId, VertexLabel)

    //    /// <summary>
    //    /// Return the vertices referenced by any entry of the given array
    //    /// of vertex labels. If no vertex is referenced by a given label
    //    /// this value will be skipped. The result set may include duplicates
    //    /// and will not be ordered.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexLabels">An array of vertex labels.</param>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    /// <httpparam name="SELECTEdges"></httpparam>
    //    /// <httpparam name="SELECTMultiEdges"></httpparam>
    //    /// <httpparam name="SELECTHyperEdges"></httpparam>
    //    /// <httpparam name="RevId"></httpparam>
    //    /// <example>GET /graph/1/vertices/labels/likes,loves,is_part_of</example>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertices/labels/{VertexLabels}"), NoAuthentication]
    //    HTTPResponse GET_VerticesByLabel(String GraphId, String VertexLabels);

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex labels.
    //    /// If no vertex is referenced by a given label this value will be skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexLabels">An array of vertex labels.</param>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    /// <httpparam name="SELECTEdges"></httpparam>
    //    /// <httpparam name="SELECTMultiEdges"></httpparam>
    //    /// <httpparam name="SELECTHyperEdges"></httpparam>
    //    /// <httpparam name="RevId"></httpparam>
    //    /// <example>FILTER /graph/1/vertices/labels HTTPBody: { "VertexLabels" : [ "likes", "loves", "is_part_of" ], "VertexFilter" : "..." }</example>
    //    [HTTPMapping(HTTPMethods.FILTER, "/graph/{GraphId}/vertices/labels/{VertexLabels}"), NoAuthentication]
    //    HTTPResponse FILTER_VerticesByLabel(String GraphId, String VertexLabels);

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex labels.
    //    /// If no vertex is referenced by a given label this value will be skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexLabels">An array of vertex labels.</param>
    //    /// <httpparam name="RevId"></httpparam>
    //    /// <example>COUNT /graph/1/vertices/labels/likes,loves,is_part_of</example>
    //    /// <example>COUNT /graph/1/vertices/labels/ HTTPBody: { "VertexLabels" : [ "likes", "loves", "is_part_of" ], "VertexFilter" : "..." }</example>
    //    [HTTPMapping(HTTPMethods.COUNT, "/graph/{GraphId}/vertices/labels/{VertexLabels}"), NoAuthentication]
    //    HTTPResponse COUNT_VerticesByLabel(String GraphId, String VertexLabels);

    //    #endregion

    //    #region GET|FILTER|COUNT_Vertices(GraphId)

    //    /// <summary>
    //    /// Get an enumeration of all vertices in the graph.
    //    /// An optional vertex filter may be applied for filtering.
    //    /// </summary>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertices"), NoAuthentication]
    //    HTTPResponse GET_Vertices(String GraphId);

    //    /// <summary>
    //    /// Get an enumeration of all vertices in the graph.
    //    /// An optional vertex filter may be applied for filtering.
    //    /// </summary>
    //    /// <httpparam name="SKIP">Skip the given number of entries from the beginning of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    /// <httpbody>Include $somescript for vertex filtering.</httpbody>
    //    [HTTPMapping(HTTPMethods.FILTER, "/graph/{GraphId}/vertices"), NoAuthentication]
    //    HTTPResponse FILTER_Vertices(String GraphId);

    //    /// <summary>
    //    /// Get an enumeration of all vertices in the graph.
    //    /// An optional vertex filter may be applied for filtering.
    //    /// </summary>
    //    /// <httpparam name="SKIP">Skip the given number of entries of the result set.</httpparam>
    //    /// <httpparam name="TAKE">Return only the given number of entries from the result set.</httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.COUNT, "/graph/{GraphId}/vertices"), NoAuthentication]
    //    HTTPResponse COUNT_Vertices(String GraphId);

    //    #endregion



    //    #region GET|FILTER|COUNT_OutEdgesFromVertex(GraphId, VertexId)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="LABEL"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertex/{VertexId}/OutE"), NoAuthentication]
    //    HTTPResponse GET_OutEdgesFromVertex(String GraphId, String VertexId);

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="LABEL"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.FILTER, "/graph/{GraphId}/vertex/{VertexId}/OutE"), NoAuthentication]
    //    HTTPResponse FILTER_OutEdgesFromVertex(String GraphId, String VertexId);

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="LABEL"></httpparam>
    //    [HTTPMapping(HTTPMethods.COUNT, "/graph/{GraphId}/vertex/{VertexId}/OutE"), NoAuthentication]
    //    HTTPResponse COUNT_OutEdgesFromVertex(String GraphId, String VertexId);

    //    #endregion

    //    #region GET_InEdgesFromVertex(GraphId, VertexId)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertex/{VertexId}/InE"), NoAuthentication]
    //    HTTPResponse GET_InEdgesFromVertex(String GraphId, String VertexId);

    //    #endregion

    //    #region GET_BothEdgesFromVertex(GraphId, VertexId)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertex/{VertexId}/BothE"), NoAuthentication]
    //    HTTPResponse GET_BothEdgesFromVertex(String GraphId, String VertexId);

    //    #endregion

    //    #region GET_OutVerticesFromVertex(GraphId, VertexId)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="DEPTH"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertex/{VertexId}/Out"), NoAuthentication]
    //    HTTPResponse GET_OutVerticesFromVertex(String GraphId, String VertexId);

    //    #endregion

    //    #region GET_InVerticesFromVertex(GraphId, VertexId)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="DEPTH"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertex/{VertexId}/In"), NoAuthentication]
    //    HTTPResponse GET_InVerticesFromVertex(String GraphId, String VertexId);

    //    #endregion

    //    #region GET_BothVerticesFromVertex(GraphId, VertexId)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="DEPTH"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertex/{VertexId}/Both"), NoAuthentication]
    //    HTTPResponse GET_BothVerticesFromVertex(String GraphId, String VertexId);

    //    #endregion




    //    #region GET_SubgraphId(GraphId, VertexId)

    //    /// <summary>
    //    /// Return the vertices referenced by the given array of vertex identifiers.
    //    /// If no vertex is referenced by a given identifier this value will be
    //    /// skipped.
    //    /// </summary>
    //    /// <param name="GraphId">The graph identification.</param>
    //    /// <param name="VertexId">The id of the vertex.</param>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="DEPTH"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/vertex/{VertexId}/SubgraphId"), NoAuthentication]
    //    HTTPResponse GET_SubgraphId(String GraphId, String VertexId);

    //    #endregion

    //    #endregion



    //    #region GET_Edges(GraphId)

    //    /// <summary>
    //    /// Get an enumeration of all edges in the graph.
    //    /// An optional edge filter may be applied for filtering.
    //    /// </summary>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.GET, "/graph/{GraphId}/edges"), NoAuthentication]
    //    HTTPResponse GET_Edges(String GraphId);

    //    #endregion

    //    #region FILTER_Edges(GraphId)

    //    /// <summary>
    //    /// Get an enumeration of all edges in the graph.
    //    /// An optional vertex filter may be applied for filtering.
    //    /// </summary>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    /// <httpbody>Include $somescript for vertex filtering.</httpbody>
    //    [HTTPMapping(HTTPMethods.FILTER, "/graph/{GraphId}/edges"), NoAuthentication]
    //    HTTPResponse FILTER_Edges(String GraphId);

    //    #endregion

    //    #region COUNT_Edges(GraphId)

    //    /// <summary>
    //    /// Get an enumeration of all edges in the graph.
    //    /// An optional vertex filter may be applied for filtering.
    //    /// </summary>
    //    /// <httpparam name="SKIP"></httpparam>
    //    /// <httpparam name="TAKE"></httpparam>
    //    /// <httpparam name="SELECT"></httpparam>
    //    [HTTPMapping(HTTPMethods.COUNT, "/graph/{GraphId}/edges"), NoAuthentication]
    //    HTTPResponse COUNT_Edges(String GraphId);

    //    #endregion

    // }

}
