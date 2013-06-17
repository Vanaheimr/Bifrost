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

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Balder;
using eu.Vanaheimr.Hermod.HTTP;
using eu.Vanaheimr.Hermod.Datastructures;
using eu.Vanaheimr.Illias.Commons.Collections;
using eu.Vanaheimr.Illias.Commons.Transactions;

#endregion

namespace eu.Vanaheimr.Bifrost.HTTP.Client
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

        public Styx.IVotingNotification<Balder.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnVertexAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertex(Balder.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertex(TVertexLabel Label, Balder.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertex(TIdVertex Id, TVertexLabel Label, Balder.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertexIfNotExists(TIdVertex Id, TVertexLabel Label, Balder.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexInitializer = null, Balder.VertexAction<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OnDuplicateVertex = null, Balder.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> ElseDo = null, Balder.VertexInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AnywayDo = null)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Balder.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnVertexRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Balder.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, TEdgeLabel Label, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdgeIfNotExists(Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, TEdgeLabel Label, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null, Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> ElseDo = null, Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AnywayDo = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(TIdEdge Id, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, TEdgeLabel Label, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdgeIfNotExists(TIdEdge Id, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, TEdgeLabel Label, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null, Balder.EdgeAction<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OnDuplicateEdge = null, Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> ElseDo = null, Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AnywayDo = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, TEdgeLabel Label = default(TEdgeLabel), Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> InVertex, TIdEdge EdgeId, TEdgeLabel Label = default(TEdgeLabel), Balder.EdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeInitializer = null)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Balder.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Balder.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnMultiEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(Balder.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector, params Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(TMultiEdgeLabel Label, Balder.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector, params Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(TMultiEdgeLabel Label, Balder.MultiEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeInitializer, Balder.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector, params Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(TIdMultiEdge Id, TMultiEdgeLabel Label, Balder.MultiEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeInitializer, Balder.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector, params Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Balder.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnMultiEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Styx.IVotingNotification<Balder.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnHyperEdgeAddition
        {
            get { throw new NotImplementedException(); }
        }

        public Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(Balder.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexSelector, params Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(THyperEdgeLabel Label, Balder.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexSelector, params Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(THyperEdgeLabel Label, Balder.HyperEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeInitializer, Balder.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexSelector, params Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(TIdHyperEdge Id, THyperEdgeLabel Label, Balder.HyperEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeInitializer, Balder.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexSelector, params Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(Balder.IGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OutVertex, THyperEdgeLabel Label, Balder.HyperEdgeInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeInitializer, params Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] InVertices)
        {
            throw new NotImplementedException();
        }

        public Styx.IVotingNotification<Balder.IReadOnlyGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>, bool> OnHyperEdgeRemoval
        {
            get { throw new NotImplementedException(); }
        }

        public Transaction<TIdVertex, TIdVertex, Balder.IGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> BeginTransaction(string Name = "", bool Distributed = false, bool LongRunning = false, IsolationLevel IsolationLevel = IsolationLevel.Write, DateTime? CreationTime = null, DateTime? InvalidationTime = null)
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

        public event Balder.GraphShuttingdownEventHandler<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OnGraphShuttingdown;

        public void Shutdown(string Message = "")
        {
            throw new NotImplementedException();
        }

        public event Balder.GraphShutteddownEventHandler<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> OnGraphShutteddown;

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

        public bool Equals(Balder.IReadOnlyGraphElement<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex> other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Balder.IReadOnlyGraphElement<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex> other)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexById(TIdVertex VertexId)
        {
            throw new NotImplementedException();
        }

        public bool TryGetVertexById(TIdVertex VertexId, out Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Vertex)
        {
            throw new NotImplementedException();
        }

        public bool HasVertexId(TIdVertex VertexId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> VerticesById(params TIdVertex[] VertexIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> VerticesByLabel(params TVertexLabel[] VertexLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Vertices(Balder.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfVertices(Balder.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeById(TIdEdge EdgeId)
        {
            throw new NotImplementedException();
        }

        public bool TryGetEdgeById(TIdEdge EdgeId, out Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Edge)
        {
            throw new NotImplementedException();
        }

        public bool HasEdgeId(TIdEdge EdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> EdgesById(params TIdEdge[] EdgeIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> EdgesByLabel(params TEdgeLabel[] EdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> Edges(Balder.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfEdges(Balder.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeById(TIdMultiEdge MultiEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> MultiEdgesById(params TIdMultiEdge[] MultiEdgeIds)
        {
            throw new NotImplementedException();
        }

        public bool HasMultiEdgeId(TIdMultiEdge MultiEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> MultiEdgesByLabel(params TMultiEdgeLabel[] MultiEdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> MultiEdges(Balder.MultiEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfMultiEdges(Balder.MultiEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeById(TIdHyperEdge HyperEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> HyperEdgesById(params TIdHyperEdge[] HyperEdgeIds)
        {
            throw new NotImplementedException();
        }

        public bool HasHyperEdgeId(TIdHyperEdge HyperEdgeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> HyperEdgesByLabel(params THyperEdgeLabel[] HyperEdgeLabels)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> HyperEdges(Balder.HyperEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public ulong NumberOfHyperEdges(Balder.HyperEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertex(Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Vertex)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddVertexIfNotExists(Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Vertex, Balder.CheckVertexExistanceInGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> CheckExistanceDelegate = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveVerticesById(params TIdVertex[] VertexIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertices(params Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Vertices)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertices(Balder.VertexFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexFilter = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdge(Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Edge)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddEdgeIfNotExists(Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Edge, Balder.CheckEdgeExistanceInGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> CheckExistanceDelegate = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdgesById(params TIdEdge[] EdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdges(params Balder.IReadOnlyGenericPropertyEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] Edges)
        {
            throw new NotImplementedException();
        }

        public void RemoveEdges(Balder.EdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddMultiEdge(Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdge)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdgesById(params TIdMultiEdge[] MultiEdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdges(params Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] MultiEdges)
        {
            throw new NotImplementedException();
        }

        public void RemoveMultiEdges(Balder.MultiEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdgeFilter = null)
        {
            throw new NotImplementedException();
        }

        public Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> AddHyperEdge(Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdge)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdgesById(params TIdHyperEdge[] HyperEdgeIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdges(params Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>[] HyperEdges)
        {
            throw new NotImplementedException();
        }

        public void RemoveHyperEdges(Balder.HyperEdgeFilter<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeFilter = null)
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

        public bool Equals(Balder.IGraphElement<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex> other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Balder.IGraphElement<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex> other)
        {
            throw new NotImplementedException();
        }

        #endregion



        public bool TryGetMultiEdgeById(TIdMultiEdge Id, out Balder.IReadOnlyGenericPropertyMultiEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> MultiEdge)
        {
            throw new NotImplementedException();
        }


        public bool TryGetHyperEdgeById(TIdHyperEdge Id, out Balder.IReadOnlyGenericPropertyHyperEdge<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdge)
        {
            throw new NotImplementedException();
        }

        public Func<Illias.Commons.Votes.IVote<bool>> VoteCreator
        {
            get { throw new NotImplementedException(); }
        }

        public Balder.IReadOnlyGenericPropertyVertex<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> SuperVertex
        {
            get { throw new NotImplementedException(); }
        }


        public IGenericPropertyGraph<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> New(TIdVertex GraphId, string Description = null, GraphInitializer<TIdVertex, TRevIdVertex, TVertexLabel, TKeyVertex, TValueVertex, TIdEdge, TRevIdEdge, TEdgeLabel, TKeyEdge, TValueEdge, TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge, TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> GraphInitializer = null)
        {
            throw new NotImplementedException();
        }
    }

}
