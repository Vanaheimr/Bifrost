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
using System.Collections.Generic;

using eu.Vanaheimr.Balder;
using eu.Vanaheimr.Hermod.HTTP;
using eu.Vanaheimr.Hermod;

#endregion

namespace eu.Vanaheimr.Bifrost.HTTP.Server
{

    //#region BifrostHTTPServer

    ///// <summary>
    ///// A property graph HTTP server.
    ///// </summary>
    //public class BifrostHTTPServer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                               TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                               TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                               TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

    //                               : BifrostHTTPServer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge,
    //                                                   IBifrostService<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>


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

    //    #region BifrostHTTPServer(NewGraphCreator)

    //    /// <summary>
    //    /// Initialize the Bifrost HTTP server using IPAddress.Any, http port 8182 and start the server.
    //    /// </summary>
    //    /// <param name="NewGraphCreator">A delegate to create a new property graph.</param>
    //    public BifrostHTTPServer(Func<TIdVertex,
    //                                  String,
    //                                  GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                                  IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator)

    //        : base(NewGraphCreator)

    //    {
    //        base.OnNewHTTPService += GraphService => { GraphService.GraphServer = this; };
    //    }

    //    #endregion

    //    #region BifrostHTTPServer(NewGraphCreator, Port, AutoStart = true)

    //    /// <summary>
    //    /// Initialize the Bifrost HTTP server using IPAddress.Any and the given parameters.
    //    /// </summary>
    //    /// <param name="NewGraphCreator">A delegate to create a new property graph.</param>
    //    /// <param name="Port">The listening port</param>
    //    /// <param name="Autostart"></param>
    //    public BifrostHTTPServer(Func<TIdVertex,
    //                                  String,
    //                                  GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                                  IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator,
    //                             IPPort  Port,
    //                             Boolean Autostart = true)

    //        : base(NewGraphCreator, Port, Autostart)

    //    {
    //        base.OnNewHTTPService += GraphService => { GraphService.GraphServer = this; };
    //    }

    //    #endregion

    //    #region BifrostHTTPServer(NewGraphCreator, IIPAddress, Port, AutoStart = true)

    //    /// <summary>
    //    /// Initialize the Bifrost HTTP server using the given parameters.
    //    /// </summary>
    //    /// <param name="NewGraphCreator">A delegate to create a new property graph.</param>
    //    /// <param name="IIPAddress">The listening IP address(es)</param>
    //    /// <param name="Port">The listening port</param>
    //    /// <param name="Autostart"></param>
    //    public BifrostHTTPServer(Func<TIdVertex,
    //                                  String,
    //                                  GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                                  IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator,
    //                             IIPAddress IIPAddress,
    //                             IPPort     Port,
    //                             Boolean    Autostart = false)

    //        : base(NewGraphCreator, IIPAddress, Port, Autostart)

    //    {
    //        base.OnNewHTTPService += GraphService => { GraphService.GraphServer = this; };
    //    }

    //    #endregion

    //    #region BifrostHTTPServer(NewGraphCreator, IPSocket, Autostart = true)

    //    /// <summary>
    //    /// Initialize the Bifrost HTTP server using the given parameters.
    //    /// </summary>
    //    /// <param name="NewGraphCreator">A delegate to create a new property graph.</param>
    //    /// <param name="IPSocket">The listening IPSocket.</param>
    //    /// <param name="Autostart"></param>
    //    public BifrostHTTPServer(Func<TIdVertex,
    //                                  String,
    //                                  GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                                  IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator,
    //                             IPSocket IPSocket,
    //                             Boolean  Autostart = true)

    //        : base(NewGraphCreator, IPSocket.IPAddress, IPSocket.Port, Autostart)

    //    {
    //        base.OnNewHTTPService += GraphService => { GraphService.GraphServer = this; };
    //    }

    //    #endregion

    //}

    //#endregion

    //#region BifrostHTTPServer<HTTPServiceInterface>

    ///// <summary>
    ///// A property graph HTTP server.
    ///// </summary>
    //public class BifrostHTTPServer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                               TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                               TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                               TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge,
    //                               HTTPServiceInterface>

    //                               : HTTPServer<HTTPServiceInterface>,
    //                                 IBifrostHTTPServer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

    //    where HTTPServiceInterface : class,
    //                                 IBifrostService<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>


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

    //    #region Data

    //    private readonly Func<TIdVertex,
    //                          String,
    //                          GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                          IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator;

    //    private readonly IDictionary<TIdVertex, IGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> GraphLookup;

    //    private readonly IDictionary<TIdVertex, IGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> VertexLookup;

    //    #endregion

    //    #region Constructor(s)

    //    #region BifrostHTTPServer(NewGraphCreator)

    //    /// <summary>
    //    /// Initialize the Bifrost HTTP server using IPAddress.Any, http port 8080 and start the server.
    //    /// </summary>
    //    /// <param name="NewGraphCreator">A delegate to create a new property graph.</param>
    //    public BifrostHTTPServer(Func<TIdVertex,
    //                                  String,
    //                                  GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                                  IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator)

    //        : base(IPv4Address.Any, new IPPort(8080), Autostart: true)

    //    {

    //        this.ServerName       = DefaultServerName;
    //        this.NewGraphCreator  = NewGraphCreator;

    //        this.GraphLookup      = new Dictionary<TIdVertex, IGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

    //        this.VertexLookup     = new Dictionary<TIdVertex, IGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

    //    }

    //    #endregion

    //    #region BifrostHTTPServer(NewGraphCreator, Port, AutoStart = true)

    //    /// <summary>
    //    /// Initialize the Bifrost HTTP server using IPAddress.Any and the given parameters.
    //    /// </summary>
    //    /// <param name="NewGraphCreator">A delegate to create a new property graph.</param>
    //    /// <param name="Port">The listening port</param>
    //    /// <param name="Autostart">Autostart the http server.</param>
    //    public BifrostHTTPServer(Func<TIdVertex,
    //                                  String,
    //                                  GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                                  IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator,
    //                             IPPort  Port,
    //                             Boolean Autostart = true)

    //        : base(IPv4Address.Any, Port, Autostart: Autostart)

    //    {

    //        this.ServerName       = DefaultServerName;
    //        this.NewGraphCreator  = NewGraphCreator;

    //        this.GraphLookup      = new Dictionary<TIdVertex, IGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

    //        this.VertexLookup     = new Dictionary<TIdVertex, IGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

    //    }

    //    #endregion

    //    #region BifrostHTTPServer(NewGraphCreator, IIPAddress, Port, AutoStart = true)

    //    /// <summary>
    //    /// Initialize the Bifrost HTTP server using the given parameters.
    //    /// </summary>
    //    /// <param name="NewGraphCreator">A delegate to create a new property graph.</param>
    //    /// <param name="IIPAddress">The listening IP address(es)</param>
    //    /// <param name="Port">The listening port</param>
    //    /// <param name="Autostart">Autostart the http server.</param>
    //    public BifrostHTTPServer(Func<TIdVertex,
    //                                  String,
    //                                  GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                                  IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator,
    //                             IIPAddress IIPAddress,
    //                             IPPort     Port,
    //                             Boolean    Autostart = true)

    //        : base(IIPAddress, Port, Autostart: Autostart)

    //    {

    //        this.ServerName       = DefaultServerName;
    //        this.NewGraphCreator  = NewGraphCreator;

    //        this.GraphLookup      = new Dictionary<TIdVertex, IGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

    //        this.VertexLookup     = new Dictionary<TIdVertex, IGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

    //    }

    //    #endregion

    //    #region BifrostHTTPServer(NewGraphCreator, IPSocket, Autostart = true)

    //    /// <summary>
    //    /// Initialize the Bifrost HTTP server using the given parameters.
    //    /// </summary>
    //    /// <param name="NewGraphCreator">A delegate to create a new property graph.</param>
    //    /// <param name="IPSocket">The listening IPSocket.</param>
    //    /// <param name="Autostart">Autostart the http server.</param>
    //    public BifrostHTTPServer(Func<TIdVertex,
    //                                  String,
    //                                  GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,
    //                                  IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> NewGraphCreator,
    //                             IPSocket IPSocket,
    //                             Boolean  Autostart = true)

    //        : base(IPSocket.IPAddress, IPSocket.Port, Autostart: Autostart)

    //    {

    //        this.ServerName       = DefaultServerName;
    //        this.NewGraphCreator  = NewGraphCreator;

    //        this.GraphLookup      = new Dictionary<TIdVertex, IGenericPropertyGraph <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

    //        this.VertexLookup     = new Dictionary<TIdVertex, IGenericPropertyVertex<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>();

    //    }

    //    #endregion

    //    #endregion


    //    #region AddGraph(Graph)

    //    /// <summary>
    //    /// Adds the given property graph to the server.
    //    /// </summary>
    //    /// <param name="Graph">A property graph.</param>
    //    public IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

    //        AddGraph(IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                       TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                       TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                       TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Graph)

    //    {

    //        GraphLookup.Add(Graph.Id, Graph);
    //        return Graph;

    //    }

    //    #endregion

    //    #region CreateNewGraph(GraphId, Description = null, GraphInitializer = null)

    //    /// <summary>
    //    /// Creates a new class-based in-memory implementation of a property graph
    //    /// and adds it to the server.
    //    /// </summary>
    //    /// <param name="GraphId">A unique identification for this graph (which is also a vertex!).</param>
    //    /// <param name="Description">The description of the graph.</param>
    //    /// <param name="GraphInitializer">A delegate to initialize the new property graph.</param>
    //    public IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

    //        CreateNewGraph(TIdVertex GraphId,
    //                       String    Description = null,
    //                       GraphInitializer<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> GraphInitializer = null)

    //    {

    //        return AddGraph(NewGraphCreator(GraphId, Description, GraphInitializer));

    //    }

    //    #endregion


    //    #region GetGraph(GraphId)

    //    /// <summary>
    //    /// Return the graph identified by the given GraphId.
    //    /// If the graph does not exist rturn null.
    //    /// </summary>
    //    /// <param name="GraphId">The unique identifier of the graph to return.</param>
    //    public IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                 TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                 TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                 TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> GetGraph(TIdVertex GraphId)
    //    {

    //        IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                              TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                              TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                              TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> _Graph;

    //        if (GraphLookup.TryGetValue(GraphId, out _Graph))
    //            return _Graph;

    //        return null;

    //    }

    //    #endregion

    //    #region TryGetGraph(GraphId, out Graph)

    //    /// <summary>
    //    /// Try to return the graph identified by the given GraphId.
    //    /// </summary>
    //    /// <param name="GraphId">The unique identifier of the graph to return.</param>
    //    /// <param name="Graph">The Graph to return.</param>
    //    public Boolean TryGetGraph(TIdVertex GraphId,
    //                               out IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                                         TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                                         TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                                         TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Graph)
    //    {
    //        return GraphLookup.TryGetValue(GraphId, out Graph);
    //    }

    //    #endregion

    //    #region NumberOfGraphs(GraphFilter = null)

    //    /// <summary>
    //    /// Return the number of graphs matching the
    //    /// optional graph filter delegate.
    //    /// </summary>
    //    /// <param name="GraphFilter">An optional graph filter.</param>
    //    public UInt64 NumberOfGraphs(GraphFilter<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                             TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                             TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                             TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> GraphFilter = null)
    //    {

    //        if (GraphFilter == null)
    //            return (UInt64) GraphLookup.Count;

    //        else
    //            return (UInt64) (from   Graph
    //                             in     GraphLookup.Values
    //                             where  GraphFilter(Graph)
    //                             select Graph).Count();

    //    }

    //    #endregion


    //    #region RemovePropertyGraph(GraphId)

    //    /// <summary>
    //    /// Removes the graph identified by the given GraphId.
    //    /// </summary>
    //    /// <param name="GraphId">The unique identifier of the graph to remove.</param>
    //    /// <returns>True on success, false otherwise.</returns>
    //    public Boolean RemovePropertyGraph(TIdVertex GraphId)
    //    {
    //        return GraphLookup.Remove(GraphId);
    //    }

    //    #endregion


    //    #region IEnumerable<IGenericPropertyGraph<...>> Members

    //    /// <summary>
    //    /// Return a graph enumerator.
    //    /// </summary>
    //    public IEnumerator<IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
    //                                             TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
    //                                             TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
    //                                             TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> GetEnumerator()
    //    {
    //        return GraphLookup.Values.GetEnumerator();
    //    }

    //    /// <summary>
    //    /// Return a graph enumerator.
    //    /// </summary>
    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return GraphLookup.Values.GetEnumerator();
    //    }

    //    #endregion

    //}

    //#endregion

}
