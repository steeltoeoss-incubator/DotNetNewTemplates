using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#if (HostingCloudFoundryOption)
using Steeltoe.Extensions.Configuration.CloudFoundry;
#endif
#if (ConfigurationRandomValueOption)
using Steeltoe.Extensions.Configuration.RandomValue;
#endif
#if (ConfigurationPlaceholderOption)
using Steeltoe.Extensions.Configuration.Placeholder;
#endif
#if (ConfigurationCloudConfigOption)
using Steeltoe.Extensions.Configuration.ConfigServer;
#endif
#if (AnyMessagingRabbitMq)
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;
#endif
#if (ConnectorRabbitMqOption && !Steeltoe2)
using Steeltoe.Connector.RabbitMQ;
#endif

namespace Company.GenericHost.CS
{
    class Program
    {
#if (AnyMessagingRabbitMq)        
        public const string RECEIVE_AND_CONVERT_QUEUE = "steeltoe_message_queue";
#endif        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((host, config) =>
                {
#if (HostingCloudFoundryOption)                    
                    config.AddCloudFoundry();
#endif
#if (ConfigurationRandomValueOption)
                    config.AddRandomValueSource();
#endif
#if (ConfigurationCloudConfigOption)                    
                    config.AddConfigServer();
#endif
#if (ConfigurationPlaceholderOption)                    
                    config.AddPlaceholderResolver();
#endif
                })
                .ConfigureServices((hostContext, services) =>
                {
#if (AnyMessagingRabbitMq)
                    // Add Steeltoe Rabbit services using JSON serialization
                    // to use .NET default serialization, pass "false"
                    services.AddRabbitServices(true);
                    // Add Steeltoe RabbitAdmin services to get queues declared
                    services.AddRabbitAdmin();
                    // Add a queue to the message container that the rabbit admin will discover and declare at startup
                    services.AddRabbitQueue(new Queue(RECEIVE_AND_CONVERT_QUEUE));
#endif
#if (MessagingRabbitMqClient)
                    // Add Steeltoe RabbitTemplate for sending/receiving
                    services.AddRabbitTemplate();
                    // Add a background service to send messages
                    services.AddSingleton<IHostedService, MyRabbitSender>();
#endif
#if (MessagingRabbitMqListener)
                    // Add singleton that will process incoming messages
                    services.AddSingleton<MyRabbitListener>();
                    // Tell steeltoe about singleton so it can wire up queues with methods to process queues
                    services.AddRabbitListeners<MyRabbitListener>();
#endif
#if (ConnectorRabbitMqOption)
                    services.AddRabbitMQConnection(hostContext.Configuration);
#endif
                });
    }
}
