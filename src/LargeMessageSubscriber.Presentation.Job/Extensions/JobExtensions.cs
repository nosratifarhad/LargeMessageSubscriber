using LargeMessageSubscriber.Integration.Options;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace LargeMessageSubscriber.Presentation.Job.Extensions
{
    public static class JobExtensions
    {
        public static void AddJobServices(this IServiceCollection services)
        {
            services.AddMassTransitServices();
        }

        private static void AddMassTransitServices(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
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
