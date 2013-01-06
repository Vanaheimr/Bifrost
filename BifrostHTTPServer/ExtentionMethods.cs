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

using de.ahzf.Vanaheimr.Hermod.Datastructures;
using de.ahzf.Vanaheimr.Blueprints;

#endregion

namespace de.ahzf.Vanaheimr.Bifrost.HTTP.Server
{

    /// <summary>
    /// Extention methods.
    /// </summary>
    public static class ExtentionsMethods
    {

        #region StartHTTPServer(this Graph, Autostart = false)

        /// <summary>
        /// Start a new TCP/HTTP/REST based graph server.
        /// </summary>
        /// <param name="Graph">A graph.</param>
        /// <param name="Autostart">Autostart the server.</param>
        public static IBifrostHTTPServer StartHTTPServer(this IGenericPropertyGraph<String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object> Graph,
                                                   Boolean Autostart = false)

        {

            var Server = new BifrostHTTPServer();
            Server.AddGraph(Graph);

            if (Autostart)
                Server.Start();

            return Server;

        }

        #endregion

        #region StartHTTPServer(this Graph, Port, Autostart = false)

        /// <summary>
        /// Start a new TCP/HTTP/REST based graph server.
        /// </summary>
        /// <param name="Graph">A graph.</param>
        /// <param name="Port">The listening port.</param>
        /// <param name="Autostart">Autostart the server.</param>
        public static IBifrostHTTPServer StartHTTPServer(this IGenericPropertyGraph<String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object> Graph,
                                                   IPPort Port,
                                                   Boolean Autostart = false)

        {

            var Server = new BifrostHTTPServer(Port, Autostart);
            Server.AddGraph(Graph);
            return Server;

        }

        #endregion

        #region StartHTTPServer(this Graph, IIPAddress, Port, Autostart = false)

        /// <summary>
        /// Start a new TCP/HTTP/REST based graph server.
        /// </summary>
        /// <param name="Graph">A graph.</param>
        /// <param name="IIPAddress">The listening IP address(es).</param>
        /// <param name="Port">The listening port.</param>
        /// <param name="Autostart">Autostart the server.</param>
        public static IBifrostHTTPServer StartHTTPServer(this IGenericPropertyGraph<String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object> Graph,
                                                   IIPAddress IIPAddress,
                                                   IPPort     Port,
                                                   Boolean    Autostart = false)

        {

            var Server = new BifrostHTTPServer(IIPAddress, Port, Autostart);
            Server.AddGraph(Graph);
            return Server;

        }

        #endregion

        #region StartHTTPServer(this Graph, IPSocket, Autostart = false)

        /// <summary>
        /// Start a new TCP/HTTP/REST based graph server.
        /// </summary>
        /// <param name="Graph">A graph.</param>
        /// <param name="IPSocket">The listening IPSocket.</param>
        /// <param name="Autostart">Autostart the server.</param>
        public static IBifrostHTTPServer StartHTTPServer(this IGenericPropertyGraph<String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object,
                                                                              String, Int64, String, String, Object> Graph,
                                                   IPSocket IPSocket,
                                                   Boolean Autostart = false)

        {

            var Server = new BifrostHTTPServer(IPSocket, Autostart);
            Server.AddGraph(Graph);
            return Server;

        }

        #endregion

    }

}