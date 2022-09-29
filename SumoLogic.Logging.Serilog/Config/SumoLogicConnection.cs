/**
 *    _____ _____ _____ _____    __    _____ _____ _____ _____
 *   |   __|  |  |     |     |  |  |  |     |   __|     |     |
 *   |__   |  |  | | | |  |  |  |  |__|  |  |  |  |-   -|   --|
 *   |_____|_____|_|_|_|_____|  |_____|_____|_____|_____|_____|
 *
 *                UNICORNS AT WARP SPEED SINCE 2010
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
namespace SumoLogic.Logging.Serilog.Config
{
    using System;

    /// <summary>
    /// SumoLogic connection properties.
    /// </summary>
    public class SumoLogicConnection
    {
        /// <summary>
        /// Gets or sets the SumoLogic server URL.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the client name value that is included in each request.
        /// This value is used for telemetry purposes to track usage of different clients.
        /// </summary>
        public string ClientName { get; set; } = "sumo-serilog-sender";

        /// <summary>
        /// Gets or sets the connection timeout.
        /// </summary>
        public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMilliseconds(60_000);

        /// <summary>
        /// Gets or sets the send message retry interval.
        /// </summary>
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromMilliseconds(10_000);

        /// <summary>
        /// Gets or sets the maximum interval between flushes.
        /// </summary>
        public TimeSpan MaxFlushInterval { get; set; } = TimeSpan.FromMilliseconds(10_000);

        /// <summary>
        /// Gets or sets how often the messages queue is checked for messages to send.
        /// </summary>
        public TimeSpan FlushingAccuracy { get; set; } = TimeSpan.FromMilliseconds(250);

        /// <summary>
        /// Gets or sets how many messages need to be in the queue before flushing.
        /// </summary>
        public long MessagesPerRequest { get; set; } = 100;

        /// <summary>
        /// Gets or sets the messages queue capacity, in bytes.
        /// </summary>
        /// <remarks>Messages are dropped When the queue capacity is exceeded.</remarks>
        public long MaxQueueSizeBytes { get; set; } = 1_000_000;
    }
}
