﻿namespace NServiceBus.AcceptanceTests.BestPractices
{
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using EndpointTemplates;
    using Features;
    using NUnit.Framework;

    public class When_subscribing_to_command_bestpractices_disabled_on_endpoint : NServiceBusAcceptanceTest
    {
        [Test]
        public Task Should_allow_subscribing_to_commands()
        {
            return Scenario.Define<Context>()
                .WithEndpoint<Endpoint>(b => b.When((session, c) => session.Subscribe<MyCommand>()))
                .Done(c => c.EndpointsStarted)
                .Run();
        }

        [Test]
        public Task Should_allow_publishing_commands()
        {
            return Scenario.Define<Context>()
                .WithEndpoint<Endpoint>(b => b.When((session, c) => session.Publish(new MyCommand())))
                .Done(c => c.EndpointsStarted)
                .Run();
        }

        [Test]
        public Task Should_allow_sending_events()
        {
            return Scenario.Define<Context>()
                .WithEndpoint<Endpoint>(b => b.When((session, c) => session.Send(new MyEvent())))
                .Done(c => c.EndpointsStarted)
                .Run();
        }

        public class Context : ScenarioContext
        {
        }

        public class Endpoint : EndpointConfigurationBuilder
        {
            public Endpoint()
            {
                EndpointSetup<DefaultServer>(c => c.DisableFeature<BestPracticeEnforcement>())
                    .AddMapping<MyCommand>(typeof(Endpoint))
                    .AddMapping<MyEvent>(typeof(Endpoint));
            }

            public class Handler : IHandleMessages<MyEvent>, IHandleMessages<MyCommand>
            {
                public Task Handle(MyCommand message, IMessageHandlerContext context)
                {
                    return Task.FromResult(0);
                }

                public Task Handle(MyEvent message, IMessageHandlerContext context)
                {
                    return Task.FromResult(0);
                }
            }
        }

        public class MyCommand : ICommand
        {
        }

        public class MyEvent : IEvent
        {
        }
    }
}