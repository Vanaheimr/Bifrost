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
    public class RemotePropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                     TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                     TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                     TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

                     : IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
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

        public TKeyVertex LabelKey
        {
            get { throw new NotImplementedException(); }
        }

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

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnVertexAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertex(Blueprints.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertex(TVertexLabel Label, Blueprints.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertex(TIdVertex Id, TVertexLabel Label, Blueprints.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertexIfNotExists(TIdVertex Id, TVertexLabel Label, Blueprints.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexInitializer = null, Blueprints.VertexAction<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OnDuplicateVertex = null, Blueprints.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> ElseDo = null, Blueprints.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AnywayDo = null)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnVertexRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, TEdgeLabel Label, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdgeIfNotExists(Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, TEdgeLabel Label, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null, Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> ElseDo = null, Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AnywayDo = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(TIdEdge Id, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, TEdgeLabel Label, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdgeIfNotExists(TIdEdge Id, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, TEdgeLabel Label, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null, Blueprints.EdgeAction<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OnDuplicateEdge = null, Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> ElseDo = null, Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AnywayDo = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, TEdgeLabel Label = default(TEdgeLabel), Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, TIdEdge EdgeId, TEdgeLabel Label = default(TEdgeLabel), Blueprints.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnMultiEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(Blueprints.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector, params Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(TMultiEdgeLabel Label, Blueprints.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector, params Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(TMultiEdgeLabel Label, Blueprints.MultiEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeInitializer, Blueprints.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector, params Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(TIdMultiEdge Id, TMultiEdgeLabel Label, Blueprints.MultiEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeInitializer, Blueprints.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector, params Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnMultiEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnHyperEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(Blueprints.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexSelector, params Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(THyperEdgeLabel Label, Blueprints.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexSelector, params Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(THyperEdgeLabel Label, Blueprints.HyperEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeInitializer, Blueprints.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexSelector, params Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(TIdHyperEdge Id, THyperEdgeLabel Label, Blueprints.HyperEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeInitializer, Blueprints.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexSelector, params Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(Blueprints.IGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, THyperEdgeLabel Label, Blueprints.HyperEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeInitializer, params Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] InVertices)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Blueprints.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnHyperEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Transaction<TIdVertex, TIdVertex, Blueprints.IGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> BeginTransaction(string Name = "", bool Distributed = false, bool LongRunning = false, IsolationLevel IsolationLevel = IsolationLevel.Write, DateTime? CreationTime = null, DateTime? InvalidationTime = null)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public TKeyEdge EdgeIdKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyEdge EdgeRevIdKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyEdge EdgeLabelKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyEdge EdgeDescriptionKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyMultiEdge MultiEdgeIdKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyMultiEdge MultiEdgeRevIdKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyMultiEdge MultiEdgeLabelKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyMultiEdge MultiEdgeDescriptionKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyHyperEdge HyperEdgeIdKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyHyperEdge HyperEdgeRevIdKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyHyperEdge HyperEdgeLabelKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyHyperEdge HyperEdgeDescriptionKey
        {
            get { throw new NotImplementedException(); }
        }

        public event Blueprints.GraphShuttingdownEventHandler<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OnGraphShuttingdown;

        public void Shutdown(string Message = "")
        {
            throw new NotImplementedException();
        }

        public event Blueprints.GraphShutteddownEventHandler<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OnGraphShutteddown;

        TIdVertex IIdentifier<TIdVertex>.Id
        {
            get { throw new NotImplementedException(); }
        }

        public bool Equals(TIdVertex other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(TIdVertex other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public TRevIdVertex RevId
        {
            get { throw new NotImplementedException(); }
        }

        public TVertexLabel Label
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyVertex IdKey
        {
            get { throw new NotImplementedException(); }
        }

        public TKeyVertex RevIdKey
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<TKeyVertex> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<TValueVertex> Values
        {
            get { throw new NotImplementedException(); }
        }

        public bool ContainsKey(TKeyVertex Key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsValue(TValueVertex Value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(TKeyVertex Key, TValueVertex Value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKeyVertex, TValueVertex> KeyValuePair)
        {
            throw new NotImplementedException();
        }

        public TValueVertex this[TKeyVertex Key]
        {
            get { throw new NotImplementedException(); }
        }

        public bool TryGetProperty(TKeyVertex Key, out TValueVertex Value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetProperty<T>(TKeyVertex Key, out T Value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<TKeyVertex, TValueVertex>> GetProperties(KeyValueFilter<TKeyVertex, TValueVertex> KeyValueFilter = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKeyVertex, TValueVertex>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Equals(Blueprints.IReadOnlyGraphElement<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex> other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Blueprints.IReadOnlyGraphElement<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex> other)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexById(TIdVertex VertexId)
        {
            throw new NotImplementedException();
        }

        public bool TryGetVertexById(TIdVertex VertexId, out Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Vertex)
        {
            throw new NotImplementedException();
        }

        public bool HasVertexId(TIdVertex VertexId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> VerticesById(params TIdVertex[] VertexIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> VerticesByLabel(params TVertexLabel[] VertexLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Vertices(Blueprints.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfVertices(Blueprints.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeById(TIdEdge EdgeId)
        {
            throw new NotImplementedException();
        }

        public bool TryGetEdgeById(TIdEdge EdgeId, out Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Edge)
        {
            throw new NotImplementedException();
        }

        public bool HasEdgeId(TIdEdge EdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> EdgesById(params TIdEdge[] EdgeIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> EdgesByLabel(params TEdgeLabel[] EdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Edges(Blueprints.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfEdges(Blueprints.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeById(TIdMultiEdge MultiEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> MultiEdgesById(params TIdMultiEdge[] MultiEdgeIds)
        {
            throw new NotImplementedException();
        }

        public bool HasMultiEdgeId(TIdMultiEdge MultiEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> MultiEdgesByLabel(params TMultiEdgeLabel[] MultiEdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> MultiEdges(Blueprints.MultiEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfMultiEdges(Blueprints.MultiEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeById(TIdHyperEdge HyperEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> HyperEdgesById(params TIdHyperEdge[] HyperEdgeIds)
        {
            throw new NotImplementedException();
        }

        public bool HasHyperEdgeId(TIdHyperEdge HyperEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> HyperEdgesByLabel(params THyperEdgeLabel[] HyperEdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> HyperEdges(Blueprints.HyperEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfHyperEdges(Blueprints.HyperEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertex(Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Vertex)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertexIfNotExists(Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Vertex, Blueprints.CheckVertexExistance<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> CheckExistanceDelegate = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveVerticesById(params TIdVertex[] VertexIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertices(params Blueprints.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertices(Blueprints.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Edge)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdgeIfNotExists(Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Edge, Blueprints.CheckEdgeExistance<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> CheckExistanceDelegate = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdgesById(params TIdEdge[] EdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdges(params Blueprints.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdges(Blueprints.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdge)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdgesById(params TIdMultiEdge[] MultiEdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdges(params Blueprints.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] MultiEdges)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdges(Blueprints.MultiEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdge)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdgesById(params TIdHyperEdge[] HyperEdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdges(params Blueprints.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] HyperEdges)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdges(Blueprints.HyperEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public event PropertyAddingEventHandler<TKeyVertex, TValueVertex> OnPropertyAdding;

        public event PropertyAddedEventHandler<TKeyVertex, TValueVertex> OnPropertyAdded;

        public event PropertyChangingEventHandler<TKeyVertex, TValueVertex> OnPropertyChanging;

        public event PropertyChangedEventHandler<TKeyVertex, TValueVertex> OnPropertyChanged;

        public event PropertyRemovingEventHandler<TKeyVertex, TValueVertex> OnPropertyRemoving;

        public event PropertyRemovedEventHandler<TKeyVertex, TValueVertex> OnPropertyRemoved;

        public IProperties<TKeyVertex, TValueVertex> Set(TKeyVertex Key, TValueVertex Value)
        {
            throw new NotImplementedException();
        }

        public TValueVertex Remove(TKeyVertex Key)
        {
            throw new NotImplementedException();
        }

        public TValueVertex Remove(TKeyVertex Key, TValueVertex Value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<TKeyVertex, TValueVertex>> Remove(KeyValueFilter<TKeyVertex, TValueVertex> KeyValueFilter = null)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Blueprints.IGraphElement<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex> other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Blueprints.IGraphElement<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex> other)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
