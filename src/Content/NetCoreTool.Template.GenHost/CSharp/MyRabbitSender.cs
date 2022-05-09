#if (MessagingRabbitMqClient)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;

namespace Company.GenericHost.CS
{
    public class MyRabbitSender : IHostedService
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "steeltoe_message_queue";
        private RabbitTemplate template;
        private Timer timer;

        public MyRabbitSender(IServiceProvider services)
        {
            template = services.GetRabbitTemplate();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(Sender, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            return Task.CompletedTask;
        }

        private void Sender(object state)
        {
            template.ConvertAndSend(RECEIVE_AND_CONVERT_QUEUE, "foo");
        }
    }
}
#endif
