﻿using System;
using Burrow.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// ReSharper disable InconsistentNaming
namespace Burrow.Tests.Internal.DurableConnectionTests
{
    [TestClass]
    public class MethodSharedConnectionShutdown : DurableConnectionTestHelper
    {
        [TestMethod]
        public void Should_be_called_if_App_is_closed_by_user()
        {
            // Arrange
            var retryPolicy = Substitute.For<IRetryPolicy>();
            var watcher = Substitute.For<IRabbitWatcher>();
            IConnection rmqConnection;
            var connectionFactory = CreateMockConnectionFactory<ConnectionFactory>("/", out rmqConnection);
            
            
            var durableConnection = new DurableConnection(retryPolicy, watcher, connectionFactory);
            durableConnection.Disconnected += () => { };
            durableConnection.Connect();

            // Action
            rmqConnection.ConnectionShutdown += Raise.Event<ConnectionShutdownEventHandler>(rmqConnection, new ShutdownEventArgs(ShutdownInitiator.Application, 0, "Connection disposed by application"));
            
            //Assert
            retryPolicy.DidNotReceive().WaitForNextRetry(Arg.Any<Action>());
        }

        [TestMethod]
        public void Should_try_reconnect_by_retryPolicy_if_Connection_Shutdown_event_was_fired()
        {
            // Arrange
            var retryPolicy = Substitute.For<IRetryPolicy>();
            var watcher = Substitute.For<IRabbitWatcher>();
            IConnection rmqConnection;
            var connectionFactory = CreateMockConnectionFactory<ManagedConnectionFactory>("/", out rmqConnection);


            var durableConnection = new DurableConnection(retryPolicy, watcher, connectionFactory);
            durableConnection.Disconnected += () => { };
            durableConnection.Connect();
            Assert.AreEqual(1, ManagedConnectionFactory.SharedConnections.Count);

            // Action
            rmqConnection.ConnectionShutdown += Raise.Event<ConnectionShutdownEventHandler>(rmqConnection, new ShutdownEventArgs(ShutdownInitiator.Application, 0, "Connection dropped for unknow reason ;)"));

            //Assert
            Assert.AreEqual(0, ManagedConnectionFactory.SharedConnections.Count);
            retryPolicy.Received().WaitForNextRetry(Arg.Any<Action>());
        }
    }
}
// ReSharper restore InconsistentNaming