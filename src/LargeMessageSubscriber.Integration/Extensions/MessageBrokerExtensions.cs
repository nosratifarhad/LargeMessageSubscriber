using LargeMessageSubscriber.Integration.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace LargeMessageSubscriber.Integration.Extensions
{
    public static class MessageBrokerExtensions
    {
        public static void AddMessageBrokerServices(this IHostApplicationBuilder builder)
        {
            builder.AddMassTransitServices();
        }

        private static void AddMassTransitServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetExecutingAssembly());

                configure.UsingRabbitMq((context, cfg) =>
                {
                    var brokerOption = context.GetRequiredService<IOptions<BrokerOptions>>().Value;

                    if (brokerOption is null)
                    {
                        throw new ArgumentNullException(nameof(BrokerOptions));
                    }

                    cfg.Host(brokerOption.Host, hostConfigure =>
                    {
                        hostConfigure.Username(brokerOption.Username);
                        hostConfigure.Password(brokerOption.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
