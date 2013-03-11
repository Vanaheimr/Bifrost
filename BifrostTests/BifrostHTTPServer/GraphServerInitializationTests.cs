﻿/*
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
using System.Collections.Generic;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Hermod.Datastructures;

using NUnit.Framework;
using eu.Vanaheimr.Bifrost.HTTP.Server;
using eu.Vanaheimr.Illias.Commons.Votes;
using eu.Vanaheimr.Balder.InMemory;

#endregion

namespace de.ahzf.Bifrost.UnitTests.GraphServerTests
{

    /// <summary>
    /// GraphServer initialization unit tests.
    /// </summary>
    [TestFixture]
    public class GraphServerInitializationTests : InitGraphServer
    {

        #region GraphServerConstructorTest()

        [Test]
        public void GraphServerConstructorTest()
        {

            using (var GraphServer = new BifrostHTTPServer(new IPPort(8080), (id, descr, init) => GraphFactory.CreateGenericPropertyGraph_WithStringIds(id, descr, () => new VetoVote(), init)) {
                                         ServerName = "GraphServer v0.1"
                                     } as IBifrostHTTPServer)
            {

                GraphServer.CreateNewGraph("123");

                //Assert.IsNotNull(GraphServer);
                //Assert.IsNotNull(GraphServer.ServerName);
                //Assert.AreEqual("GraphServer v0.1", GraphServer.ServerName);

                //var graphs = GraphServer.ToList();
                //Assert.IsNotNull(graphs);
                //Assert.AreEqual(1, graphs.Count);

                //var graph1 = graphs[0];
                //Assert.IsNotNull(graph1);
                //Assert.IsNotNull(graph1.IdKey);
                //Assert.IsNotNull(graph1.RevIdKey);
                //Assert.AreEqual(123UL, graph1.Id);

            }

        }

        #endregion

        #region AddPropertyGraphTest()

        [Test]
        public void AddPropertyGraphTest()
        {

            using (var GraphServer = new BifrostHTTPServer(new IPPort(8080), (id, descr, init) => GraphFactory.CreateGenericPropertyGraph_WithStringIds(id, descr, () => new VetoVote(), init)) {
                                         ServerName = "GraphServer v0.1"
                                     } as IBifrostHTTPServer)
            {

                GraphServer.CreateNewGraph("123");

                //var graph = GraphServer.CreateNewGraph("256");
                //Assert.IsNotNull(graph);
                //Assert.IsNotNull(graph.IdKey);
                //Assert.IsNotNull(graph.RevIdKey);
                //Assert.AreEqual(256UL, graph.Id);

                //var graphs = GraphServer.ToList();
                //Assert.IsNotNull(graphs);
                //Assert.AreEqual(2, graphs.Count);

                //var graphIds = (from _graph in graphs select _graph.Id).ToList();
                //Assert.IsTrue(graphIds.Contains("123"));
                //Assert.IsTrue(graphIds.Contains("256"));

            }

        }

        #endregion

        #region NewPropertyGraphTest()

        [Test]
        public void NewPropertyGraphTest()
        {

            using (var GraphServer = new BifrostHTTPServer(new IPPort(8080), (id, descr, init) => GraphFactory.CreateGenericPropertyGraph_WithStringIds(id, descr, () => new VetoVote(), init)) {
                                         ServerName = "GraphServer v0.1"
                                     } as IBifrostHTTPServer)
            {

                GraphServer.CreateNewGraph("123");

                //var graph = GraphServer.CreateNewGraph("512", "demo graph", g => g.SetProperty("hello", "world!"));
                //Assert.IsNotNull(graph);
                //Assert.IsNotNull(graph.IdKey);
                //Assert.IsNotNull(graph.RevIdKey);
                //Assert.AreEqual(512UL, graph.Id);
                //Assert.IsTrue(graph.ContainsKey("hello"));
                //Assert.IsTrue(graph.ContainsValue("world!"));
                //Assert.IsTrue(graph.Contains("hello", "world!"));

                //var graphs = GraphServer.ToList();
                //Assert.IsNotNull(graphs);
                //Assert.AreEqual(2, graphs.Count);

                //var graphIds = (from _graph in graphs select _graph.Id).ToList();
                //Assert.IsTrue(graphIds.Contains("123"));
                //Assert.IsTrue(graphIds.Contains("512"));

            }

        }

        #endregion

        //#region GraphIdAndHashCodeTest()

        //[Test]
        //public void GraphIdAndHashCodeTest()
        //{

        //    var graph1 = new PropertyGraph(123, null);
        //    var graph2 = new PropertyGraph(256, null);
        //    var graph3 = new PropertyGraph(123, null);

        //    Assert.IsNotNull(graph1.Id);
        //    Assert.IsNotNull(graph2.Id);
        //    Assert.IsNotNull(graph3.Id);

        //    Assert.IsNotNull(graph1.GetHashCode());
        //    Assert.IsNotNull(graph2.GetHashCode());
        //    Assert.IsNotNull(graph3.GetHashCode());

        //    Assert.AreEqual(graph1.Id, graph1.GetHashCode());
        //    Assert.AreEqual(graph2.Id, graph2.GetHashCode());
        //    Assert.AreEqual(graph3.Id, graph3.GetHashCode());

        //    Assert.AreEqual(graph1.Id, graph3.Id);
        //    Assert.AreEqual(graph1.GetHashCode(), graph3.GetHashCode());

        //}

        //#endregion

    }

}
