/*
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
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using de.ahzf.Illias.Commons;
using de.ahzf.Vanaheimr.Blueprints;
using de.ahzf.Vanaheimr.Blueprints.InMemory;
using de.ahzf.Vanaheimr.Hermod.HTTP;
using de.ahzf.Vanaheimr.Hermod.Datastructures;
using de.ahzf.Illias.Commons.Collections;
using de.ahzf.Vanaheimr.Blueprints;
using de.ahzf.Illias.Commons.Transactions;

#endregion

namespace de.ahzf.Vanaheimr.Bifrost.HTTP.Client
{

    /// <summary>
    /// Access a remote PropertyGraph via HTTP/REST (client).
    /// </summary>
    public class RemotePropertyGraph : IPropertyGraph
    {

        #region Data

        private readonly HTTPClient HTTPClient;

        #endregion

        #region Properties

        #region RemoteIPAddress

        /// <summary>
        /// The IP address to connect to.
        /// </summary>
        public IIPAddress RemoteIPAddress
        {

            get
            {
                return HTTPClient.RemoteIPAddress;
            }

            set
            {
                HTTPClient.RemoteIPAddress = value;
            }

        }

        #endregion

        #region RemotePort

        /// <summary>
        /// The IP port to connect to.
        /// </summary>
        public IPPort RemotePort
        {

            get
            {
                return HTTPClient.RemotePort;
            }

            set
            {
                HTTPClient.RemotePort = value;
            }

        }

        #endregion

        #region RemoteSocket

        /// <summary>
        /// The IP socket to connect to.
        /// </summary>
        public IPSocket RemoteSocket
        {

            get
            {
                return HTTPClient.RemoteSocket;
            }

            set
            {
                HTTPClient.RemoteSocket = value;
            }

        }

        #endregion


        #region HTTPContentType

        public HTTPContentType HTTPContentType { get; set; }

        #endregion


        #region Id

        public ulong Id { get; set; }

        #endregion

        #region Description

        // {
        //   "AllGraphs": {
        //     "123": "the first graph",
        //     "512": "the second graph"
        //   }
        // }

        /// <summary>
        /// Provides a description of this graph element.
        /// </summary>
        public Object Description
        {

            get
            {
                var JSON = JSONResponse.ParseJSON(WaitGET("/graph/" + Id + "/description"));
                return (String) JSON.Result["description"];

                //var a = (j as JToken).SelectToken("description", errorWhenNoMatch: false);

                //JObject googleSearch = JObject.Parse(googleSearchText);

                //// get JSON result objects into a list
                //IList<JToken> results = googleSearch["responseData"]["results"].Children().ToList();

                //// serialize JSON results into .NET objects
                //IList<SearchResult> searchResults = new List<SearchResult>();
                //foreach (JToken result in results)
                //{
                //    SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(result.ToString());
                //    searchResults.Add(searchResult);
                //}

            }

            set
            {
                throw new NotImplementedException();
            }

        }

        #endregion

        #region NumberOfVertices

        public ulong NumberOfVertices(VertexFilter<ulong, long, string, string, object,
                                                   ulong, long, string, string, object,
                                                   ulong, long, string, string, object,
                                                   ulong, long, string, string, object> VertexFilter = null)
        {

            if (VertexFilter != null)
                throw new NotImplementedException();

            throw new NotImplementedException();

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region GraphClient(RemoteIPAddress = null, RemotePort = null)

        /// <summary>
        /// Create a new GraphClient using the given optional parameters.
        /// </summary>
        /// <param name="RemoteIPAddress">The IP address to connect to.</param>
        /// <param name="RemotePort">The IP port to connect to.</param>
        public RemotePropertyGraph(IIPAddress RemoteIPAddress = null, IPPort RemotePort = null)
        {

            this.HTTPClient      = new HTTPClient(RemoteIPAddress, RemotePort);
            this.HTTPContentType = HTTPContentType.JSON_UTF8;

            //HTTPClient.SetProtocolVersion(HTTPVersion.HTTP_1_1).
            //           SetUserAgent("Hermod HTTP Client v0.1").
            //           SetConnection("keep-alive");

        }

        #endregion

        #region GraphClient(RemoteSocket)

        /// <summary>
        /// Create a new GraphClient using the given optional parameters.
        /// </summary>
        /// <param name="RemoteSocket">The IP socket to connect to.</param>
        public RemotePropertyGraph(IPSocket RemoteSocket)
        {
            this.HTTPClient      = new HTTPClient(RemoteSocket);
            this.HTTPContentType = HTTPContentType.JSON_UTF8;
        }

        #endregion

        #endregion


        #region (private) WaitGET(Url, HTTPContentType = null)

        private String WaitGET(String Url, HTTPContentType HTTPContentType = null)
        {
            var result = "";
            DoGET(Url, HTTPContentType, r => { result = r; }).Wait();
            return result;
        }

        #endregion

        #region (private) DoGET(Url, HTTPContentType = null, Action = null)

        private Task<HTTPClient> DoGET(String Url, HTTPContentType HTTPContentType = null, Action<String> Action = null)
        {

            var _HTTPContentType = (HTTPContentType != null) ? HTTPContentType : this.HTTPContentType;

            var _HTTPClient = new HTTPClient(RemoteIPAddress, RemotePort);
            var _Request    = _HTTPClient.GET(Url).
                                            SetProtocolVersion(HTTPVersion.HTTP_1_1).
                                            SetUserAgent("GraphClient v0.1").
                                            SetConnection("keep-alive").
                                            AddAccept(_HTTPContentType, 1);

            return HTTPClient.Execute(_Request, response => { if (Action != null) Action(response.Content.ToUTF8String()); } );

        }

        #endregion


        #region District of chaos, discord and confusion!


        public Blueprints.IPropertyVertex AddVertex(Blueprints.VertexInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyVertex AddVertex(string VertexLabel, Blueprints.VertexInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyVertex AddVertex(ulong VertexId, string VertexLabel, Blueprints.VertexInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyVertex AddVertex(Blueprints.IPropertyVertex Vertex)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyVertex VertexById(ulong VertexId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IPropertyVertex> VerticesById(params ulong[] VertexIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IPropertyVertex> VerticesByLabel(params string[] VertexLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IPropertyVertex> Vertices(Blueprints.VertexFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveVerticesById(params ulong[] VertexIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertices(params Blueprints.IPropertyVertex[] Vertices)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertices(Blueprints.VertexFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyEdge AddEdge(Blueprints.IPropertyVertex OutVertex, string Label, Blueprints.IPropertyVertex InVertex, Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyEdge AddEdge(ulong EdgeId, Blueprints.IPropertyVertex OutVertex, string Label, Blueprints.IPropertyVertex InVertex, Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyEdge AddEdge(Blueprints.IPropertyVertex OutVertex, Blueprints.IPropertyVertex InVertex, string Label = default(String), Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyEdge AddEdge(Blueprints.IPropertyVertex OutVertex, Blueprints.IPropertyVertex InVertex, ulong EdgeId, string Label = default(String), Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyEdge AddEdge(Blueprints.IPropertyEdge IPropertyEdge)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IPropertyEdge EdgeById(ulong EdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IPropertyEdge> EdgesById(params ulong[] EdgeIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IPropertyEdge> EdgesByLabel(params string[] EdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IPropertyEdge> Edges(Blueprints.EdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfEdges(Blueprints.EdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdgesById(params ulong[] EdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdges(params Blueprints.IPropertyEdge[] Edges)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdges(Blueprints.EdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, bool> OnVertexAddition
        {
            get { throw new NotImplementedException(); }
        }

        Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Blueprints.IGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.AddVertex(Blueprints.VertexInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Blueprints.IGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.AddVertex(string Label, Blueprints.VertexInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Blueprints.IGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.AddVertex(ulong Id, string Label, Blueprints.VertexInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddVertexIfNotExists(ulong Id, string Label, Blueprints.VertexInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexInitializer = null, Blueprints.VertexInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AnywayDo = null)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, bool> OnVertexRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, bool> OnEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddEdge(Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> OutVertex, string Label, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> InVertex, Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddEdge(ulong EdgeId, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> OutVertex, string Label, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> InVertex, Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddEdgeIfNotExists(ulong EdgeId, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> OutVertex, string Label, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> InVertex, Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null, Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AnywayDo = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddEdge(Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> OutVertex, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> InVertex, string Label = default(string), Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddEdge(Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> OutVertex, Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> InVertex, ulong EdgeId, string Label = default(string), Blueprints.EdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, bool> OnEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, bool> OnMultiEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddMultiEdge(params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddMultiEdge(string Label, params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddMultiEdge(string Label, Blueprints.MultiEdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> MultiEdgeInitializer, params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddMultiEdge(ulong MultiEdgeId, string Label, Blueprints.MultiEdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> MultiEdgeInitializer, params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, bool> OnMultiEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, bool> OnHyperEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddHyperEdge(params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddHyperEdge(string Label, params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddHyperEdge(string Label, Blueprints.HyperEdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> HyperEdgeInitializer, params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddHyperEdge(ulong HyperEdgeId, string Label, Blueprints.HyperEdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> HyperEdgeInitializer, params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddHyperEdge(Blueprints.IGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> OutVertex, string Label, Blueprints.HyperEdgeInitializer<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> HyperEdgeInitializer, params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] InVertices)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>, bool> OnHyperEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public event Blueprints.GraphShuttingdownEventHandler<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> OnGraphShuttingdown;

        public void Shutdown(string Message = "")
        {
            throw new NotImplementedException();
        }

        public event Blueprints.GraphShutteddownEventHandler<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> OnGraphShutteddown;

        public bool Equals(ulong other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(ulong other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public long RevId
        {
            get { throw new NotImplementedException(); }
        }

        public string Label
        {
            get { throw new NotImplementedException(); }
        }

        public string IdKey
        {
            get { throw new NotImplementedException(); }
        }

        public string RevIdKey
        {
            get { throw new NotImplementedException(); }
        }

        public string DescriptionKey
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<object> Values
        {
            get { throw new NotImplementedException(); }
        }

        public bool ContainsKey(string Key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsValue(object Value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string Key, object Value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, object> KeyValuePair)
        {
            throw new NotImplementedException();
        }

        public object this[string Key]
        {
            get { throw new NotImplementedException(); }
        }

        public bool TryGetProperty(string Key, out object Value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetProperty<T>(string Key, out T Value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> GetProperties(KeyValueFilter<string, object> KeyValueFilter = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Equals(Blueprints.IReadOnlyGraphElement<ulong, long, string, string, object> other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Blueprints.IReadOnlyGraphElement<ulong, long, string, string, object> other)
        {
            throw new NotImplementedException();
        }

        Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Blueprints.IReadOnlyVertexMethods<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.VertexById(ulong VertexId)
        {
            throw new NotImplementedException();
        }

        public bool TryGetVertexById(ulong VertexId, out Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Vertex)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> Blueprints.IReadOnlyVertexMethods<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.VerticesById(params ulong[] VertexIds)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> Blueprints.IReadOnlyVertexMethods<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.VerticesByLabel(params string[] VertexLabels)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> Blueprints.IReadOnlyVertexMethods<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.Vertices(Blueprints.VertexFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Blueprints.IReadOnlyEdgeMethods<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.EdgeById(ulong EdgeId)
        {
            throw new NotImplementedException();
        }

        public bool TryGetEdgeById(ulong EdgeId, out Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Edge)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> Blueprints.IReadOnlyEdgeMethods<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.EdgesById(params ulong[] EdgeIds)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> Blueprints.IReadOnlyEdgeMethods<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.EdgesByLabel(params string[] EdgeLabels)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> Blueprints.IReadOnlyEdgeMethods<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>.Edges(Blueprints.EdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> MultiEdgeById(ulong MultiEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> MultiEdgesById(params ulong[] MultiEdgeIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> MultiEdgesByLabel(params string[] MultiEdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> MultiEdges(Blueprints.MultiEdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfMultiEdges(Blueprints.MultiEdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> HyperEdgeById(ulong HyperEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> HyperEdgesById(params ulong[] HyperEdgeIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> HyperEdgesByLabel(params string[] HyperEdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>> HyperEdges(Blueprints.HyperEdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> HyperEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfHyperEdges(Blueprints.HyperEdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> HyperEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddVertex(Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Vertex)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddVertexIfNotExists(Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Vertex, Blueprints.CheckVertexExistance<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> CheckExistanceDelegate = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertices(params Blueprints.IReadOnlyGenericPropertyVertex<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddEdge(Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Edge)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddEdgeIfNotExists(Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> Edge, Blueprints.CheckEdgeExistance<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> CheckExistanceDelegate = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdges(params Blueprints.IReadOnlyGenericPropertyEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddMultiEdge(Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> MultiEdge)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdgesById(params ulong[] MultiEdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdges(params Blueprints.IReadOnlyGenericPropertyMultiEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] MultiEdges)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdges(Blueprints.MultiEdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> AddHyperEdge(Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> HyperEdge)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdgesById(params ulong[] HyperEdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdges(params Blueprints.IReadOnlyGenericPropertyHyperEdge<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object>[] HyperEdges)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdges(Blueprints.HyperEdgeFilter<ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object, ulong, long, string, string, object> HyperEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public event PropertyAddingEventHandler<string, object> OnPropertyAdding;

        public event PropertyAddedEventHandler<string, object> OnPropertyAdded;

        public event PropertyChangingEventHandler<string, object> OnPropertyChanging;

        public event PropertyChangedEventHandler<string, object> OnPropertyChanged;

        public event PropertyRemovingEventHandler<string, object> OnPropertyRemoving;

        public event PropertyRemovedEventHandler<string, object> OnPropertyRemoved;

        public IProperties<string, object> SetProperty(string Key, object Value)
        {
            throw new NotImplementedException();
        }

        public object Remove(string Key)
        {
            throw new NotImplementedException();
        }

        public object Remove(string Key, object Value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> Remove(KeyValueFilter<string, object> KeyValueFilter = null)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Blueprints.IGraphElement<ulong, long, string, string, object> other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Blueprints.IGraphElement<ulong, long, string, string, object> other)
        {
            throw new NotImplementedException();
        }

        #endregion



        public Illias.Commons.Transactions.Transaction<ulong, ulong, IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                            UInt64, Int64, String, String, Object,
                                                            UInt64, Int64, String, String, Object,
                                                            UInt64, Int64, String, String, Object>> BeginTransaction(string Name = "", bool Distributed = false, bool LongRunning = false, Illias.Commons.Transactions.IsolationLevel IsolationLevel = IsolationLevel.Write, DateTime? CreationTime = null, DateTime? InvalidationTime = null)
        {
            throw new NotImplementedException();
        }


        public bool HasVertexId(ulong VertexId)
        {
            throw new NotImplementedException();
        }


        public bool HasEdgeId(ulong EdgeId)
        {
            throw new NotImplementedException();
        }


        public bool HasMultiEdgeId(ulong MultiEdgeId)
        {
            throw new NotImplementedException();
        }


        public bool HasHyperEdgeId(ulong HyperEdgeId)
        {
            throw new NotImplementedException();
        }
    }

}
