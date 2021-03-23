/*
 * Copyright 2018 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using System;
using System.Collections.Generic;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Configuration;

namespace Amazon.Extensions.Configuration.SystemsManager
{
    /// <inheritdoc />
    /// <summary>
    /// Represents AWS Systems Manager Parameter Store variables as an <see cref="IConfigurationSource" />.
    /// </summary>
    public class SystemsManagerConfigurationSource : IConfigurationSource
    {
        public SystemsManagerConfigurationSource()
        {
            Filters = new List<ParameterStringFilter>();
            ParameterNames = new List<string>();
        }

        /// <summary>
        /// A Path used to filter parameters.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// <see cref="AWSOptions"/> used to create an AWS Systems Manager Client />.
        /// </summary>
        public AWSOptions AwsOptions { get; set; }

        /// <summary>
        /// Determines if loading configuration data from AWS Systems Manager Parameter Store is optional.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Parameters will be reloaded from the AWS Systems Manager Parameter Store after the specified time frame
        /// </summary>
        public TimeSpan? ReloadAfter { get; set; }

        /// <summary>
        /// Prepends the supplied Prefix to all result keys
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Will be called if an uncaught exception occurs in <see cref="SystemsManagerConfigurationProvider"/>.Load.
        /// </summary>
        public Action<SystemsManagerExceptionContext> OnLoadException { get; set; }

        /// <summary>
        /// Implementation of <see cref="IParameterProcessor"/> used to process <see cref="Parameter"/> results. Defaults to <see cref="DefaultParameterProcessor"/>.
        /// </summary>
        public IParameterProcessor ParameterProcessor { get; set; }

        /// <summary> 
        /// Filters to limit the request results.
        /// You can't filter using the parameter name.
        /// </summary>
        public List<ParameterStringFilter> Filters { get; }

        /// <summary> 
        /// Paramter names (including full path) to fetch values for.
        /// This is mutually exclusive with @Path. If a single ParameterName is specified, only values for specified names are fetched and "Path" is ignored
        /// </summary>
        public List<string> ParameterNames { get; }

        /// <summary> 
        /// paramter name (including full path) to fetch a value for.
        /// This is mutually exclusive with @Path. If a single ParameterName is specified, only values for specified names are fetched and "Path" is ignored
        /// </summary>
        public void AddParameterName(string parameterName) {
            ParameterNames.Add(parameterName);
        }

        /// <inheritdoc />
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SystemsManagerConfigurationProvider(this);
        }
    }
}
