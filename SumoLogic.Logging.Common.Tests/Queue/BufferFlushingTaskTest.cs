﻿/**
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

namespace SumoLogic.Logging.Common.Tests.Aggregation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using SumoLogic.Logging.Common.Queue;
    using Xunit;

    /// <summary>
    /// Test for Http buffer flushing task.
    /// </summary>
    public class BufferFlushingTaskTest
    {
        /// <summary>
        /// Test if the buffer flush.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unit test")]
        [Fact]
        public async Task FlushBySizeTest()
        {
            var buffer = new BufferWithFifoEviction<string>(1000, new StringLengthCostAssigner());
            var bufferFlushingTask = new DummyBufferFlushingTask(buffer, TimeSpan.MaxValue, 3, "No-Name");

            await bufferFlushingTask.Run();
            Assert.Equal(0, bufferFlushingTask.SentOut.Count);

            buffer.Add("msg1");
            buffer.Add("msg2");
            await bufferFlushingTask.Run();
            Assert.Equal(0, bufferFlushingTask.SentOut.Count);

            buffer.Add("msg3");
            await bufferFlushingTask.Run();
            Assert.Equal(1, bufferFlushingTask.SentOut.Count);

            Assert.Equal(3, bufferFlushingTask.SentOut[0].Count);
            Assert.Equal(new List<string>() { "msg1", "msg2", "msg3" }, bufferFlushingTask.SentOut[0]);
        }

        /// <summary>
        /// Test if the buffer flush.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unit test")]
        [Fact]
        public async Task FlushWithNPlusOneElementsTest()
        {
            var buffer = new BufferWithFifoEviction<string>(1000, new StringLengthCostAssigner());
            var bufferFlushingTask = new DummyBufferFlushingTask(buffer, TimeSpan.MaxValue, 3, "No-Name");

            buffer.Add("msg1");
            buffer.Add("msg2");
            buffer.Add("msg3");
            buffer.Add("msg4");
            await bufferFlushingTask.Run();
            Assert.Equal(4, bufferFlushingTask.SentOut[0].Count);
        }

        /// <summary>
        /// Test if the buffer flush when the add more message than capacity buffer.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unit test")]
        [Fact]
        public async Task FlushWhenBufferCapacityIsSmallTest()
        {
            var buffer = new BufferWithFifoEviction<string>(12, new StringLengthCostAssigner());
            var bufferFlushingTask = new DummyBufferFlushingTask(buffer, TimeSpan.MaxValue, 3, "No-Name");
            buffer.Add("msg1");
            buffer.Add("msg2");
            buffer.Add("msg3");
            buffer.Add("msg4");
            await bufferFlushingTask.Run();
            Assert.Equal(3, bufferFlushingTask.SentOut[0].Count);
            Assert.Equal(new List<string>() { "msg2", "msg3", "msg4" }, bufferFlushingTask.SentOut[0]);
        }      
    }
}
