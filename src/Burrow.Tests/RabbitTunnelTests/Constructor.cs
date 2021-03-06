﻿using System;
using NSubstitute;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Burrow.Tests.RabbitTunnelTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void Can_construct_with_provided_route_finder_and_durable_connection()
        {
            // Arrange
            var routeFinder = Substitute.For<IRouteFinder>();
            var durableConnection = Substitute.For<IDurableConnection>();
            ////durableConnection.ConnectionFactory.Returns(Substitute.For<ConnectionFactory>());

            // Action
            var tunnel = new RabbitTunnel(routeFinder, durableConnection);

            // Assert
            Assert.IsNotNull(tunnel);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_if_provide_null_IConsumerManager()
        {
            // Action
            new RabbitTunnel(null, null, null, null, null, null, false);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_if_provide_null_IRabbitWatcher()
        {
            // Action
            new RabbitTunnel(Substitute.For<IConsumerManager>(), null, null, null, null, null, false);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_if_provide_null_IRouteFinder()
        {
            // Action
            new RabbitTunnel(Substitute.For<IConsumerManager>(), Substitute.For<IRabbitWatcher>(), null, Substitute.For<IDurableConnection>(), Substitute.For<ISerializer>(), Substitute.For<ICorrelationIdGenerator>(), false);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_if_provide_null_IDurableConnection()
        {
            // Action
            new RabbitTunnel(Substitute.For<IConsumerManager>(), Substitute.For<IRabbitWatcher>(), Substitute.For<IRouteFinder>(), null, Substitute.For<ISerializer>(), Substitute.For<ICorrelationIdGenerator>(), false);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_if_provide_null_ISerializer()
        {
            // Action
            new RabbitTunnel(Substitute.For<IConsumerManager>(), Substitute.For<IRabbitWatcher>(), Substitute.For<IRouteFinder>(), Substitute.For<IDurableConnection>(), null, Substitute.For<ICorrelationIdGenerator>(), false);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_if_provide_null_ICorrelationIdGenerator()
        {
            // Action
            new RabbitTunnel(Substitute.For<IConsumerManager>(), Substitute.For<IRabbitWatcher>(), Substitute.For<IRouteFinder>(), Substitute.For<IDurableConnection>(), Substitute.For<ISerializer>(), null, false);
        }
    }
}
// ReSharper restore InconsistentNaming