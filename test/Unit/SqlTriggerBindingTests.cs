// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Azure.WebJobs.Extensions.Sql.Tests.Common;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.Sql.Tests.Unit
{
    public class TriggerBindingTests
    {
        private static readonly Mock<IConfiguration> config = new Mock<IConfiguration>();
        private static readonly Mock<IHostIdProvider> hostIdProvider = new Mock<IHostIdProvider>();
        private static readonly Mock<ILoggerFactory> loggerFactory = new Mock<ILoggerFactory>();
        private static readonly Mock<ITriggeredFunctionExecutor> mockExecutor = new Mock<ITriggeredFunctionExecutor>();
        private static readonly Mock<ILogger> logger = new Mock<ILogger>();
        private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        [Fact]
        public void TestTriggerBindingProviderNullConfig()
        {
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerAttributeBindingProvider(null, hostIdProvider.Object, loggerFactory.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerAttributeBindingProvider(config.Object, null, loggerFactory.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerAttributeBindingProvider(config.Object, hostIdProvider.Object, null));
        }

        [Fact]
        public async void TestTriggerAttributeBindingProviderNullContext()
        {
            var configProvider = new SqlTriggerAttributeBindingProvider(config.Object, hostIdProvider.Object, loggerFactory.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => configProvider.TryCreateAsync(null));
        }

        [Fact]
        public void TestTriggerListenerNullConfig()
        {
            string connectionString = "testConnectionString";
            string tableName = "testTableName";
            string userFunctionId = "testUserFunctionId";

            Assert.Throws<ArgumentNullException>(() => new SqlTriggerListener<TestData>(null, tableName, userFunctionId, mockExecutor.Object, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerListener<TestData>(connectionString, null, userFunctionId, mockExecutor.Object, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerListener<TestData>(connectionString, tableName, null, mockExecutor.Object, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerListener<TestData>(connectionString, tableName, userFunctionId, null, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerListener<TestData>(connectionString, tableName, userFunctionId, mockExecutor.Object, null));
        }

        [Fact]
        public void TestTriggerBindingNullConfig()
        {
            string connectionString = "testConnectionString";
            string tableName = "testTableName";

            Assert.Throws<ArgumentNullException>(() => new SqlTriggerBinding<TestData>(null, connectionString, TriggerBindingFunctionTest.GetParam(), hostIdProvider.Object, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerBinding<TestData>(tableName, null, TriggerBindingFunctionTest.GetParam(), hostIdProvider.Object, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerBinding<TestData>(tableName, connectionString, null, hostIdProvider.Object, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerBinding<TestData>(tableName, connectionString, TriggerBindingFunctionTest.GetParam(), null, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SqlTriggerBinding<TestData>(tableName, connectionString, TriggerBindingFunctionTest.GetParam(), hostIdProvider.Object, null));
        }

        [Fact]
        public async void TestTriggerBindingProviderTryCreateAsync()
        {
            var triggerBindingProviderContext = new TriggerBindingProviderContext(TriggerBindingFunctionTest.GetParam(), cancellationTokenSource.Token);
            var triggerAttributeBindingProvider = new SqlTriggerAttributeBindingProvider(config.Object, hostIdProvider.Object, loggerFactory.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => triggerAttributeBindingProvider.TryCreateAsync(triggerBindingProviderContext));
        }
        private static class TriggerBindingFunctionTest
        {
            public static void InvalidParameterType(
            [SqlTrigger("[dbo].[Employees]", ConnectionStringSetting = "SqlConnectionString")]
            IEnumerable<SqlChange<TestData>> changes,
            ILogger logger)
            {
                logger.LogInformation(changes.ToString());
            }
            public static ParameterInfo GetParam()
            {
                MethodInfo methodInfo = typeof(TriggerBindingFunctionTest).GetMethod("InvalidParameterType", BindingFlags.Public | BindingFlags.Static);
                ParameterInfo[] parameters = methodInfo.GetParameters();
                return parameters[^2];
            }
        }
    }
}