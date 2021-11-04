using System.Collections.Generic;
using Steeltoe.NetCoreTool.Template.WebApi.Test.Models;
using Xunit.Abstractions;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test
{
    public class ConnectorRabbitMqOptionTest : ProjectOptionTest
    {
        public ConnectorRabbitMqOptionTest(ITestOutputHelper logger) : base("connector-rabbitmq",
            "Add a connector for RabbitMQ message brokers", logger)
        {
        }

        protected override void AssertPackageReferencesHook(ProjectOptions options, List<(string, string)> packages)
        {
            packages.Add(("RabbitMQ.Client", "5.1.*"));
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                packages.Add(("Steeltoe.CloudFoundry.ConnectorCore", "$(SteeltoeVersion)"));
            }
            else
            {
                packages.Add(("Steeltoe.Connector.ConnectorCore", "$(SteeltoeVersion)"));
            }
        }

        protected override void AssertStartupSnippetsHook(ProjectOptions options, List<string> snippets)
        {
            if (options.SteeltoeVersion < SteeltoeVersion.Steeltoe30)
            {
                snippets.Add("Steeltoe.CloudFoundry.Connector.RabbitMQ");
            }
            else
            {
                snippets.Add("Steeltoe.Connector.RabbitMQ");
            }

            snippets.Add("services.AddRabbitMQConnection");
        }
    }
}
