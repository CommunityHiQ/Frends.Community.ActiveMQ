using NUnit.Framework;
using System.Threading.Tasks;

namespace Frends.Community.ActiveMQ.Tests
{
    [TestFixture]
    public class ProduceTests
    {
        /*
         * Tests require ActiveMQ to be in use. Please run the following command before executing tests.
         * cd Frends.Community.ActiveMQ.Tests && docker-compose up -d
         * 
         * UI Admin login at http://localhost:8161/admin. User: admin, Pass: admin
         */

        private readonly string _connectionString = "activemq:tcp://admin:admin@localhost:61616";

        [Test]
        public async Task ProduceMessageToQueue()
        {
            var input = new ProduceInput
            {
                ConnectionString = _connectionString,
                Queue = "producequeue",
                Message = "This is test message"
            };

            var result = await ActiveMQTasks.Produce(input);
            Assert.IsTrue(result.Success);
        }
    }
}
