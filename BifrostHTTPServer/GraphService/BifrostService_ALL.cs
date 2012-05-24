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

using System.Reflection;

using de.ahzf.Hermod.HTTP;

#endregion

namespace de.ahzf.Vanaheimr.Bifrost.HTTP.Server
{

    /// <summary>
    /// Any content representation.
    /// </summary>
    public class BifrostService_ALL : ABifrostService
    {

        #region Constructor(s)

        #region BifrostService_ALL()

        /// <summary>
        /// ALL content representation.
        /// </summary>
        public BifrostService_ALL()
            : base(HTTPContentType.ALL)
        { }

        #endregion

        #region BifrostService_ALL(IHTTPConnection)

        /// <summary>
        /// ALL content representation.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        public BifrostService_ALL(IHTTPConnection IHTTPConnection)
            : base(IHTTPConnection, HTTPContentType.ALL)
        {
            this.CallingAssembly = Assembly.GetExecutingAssembly();
        }

        #endregion

        #endregion

    }

}
