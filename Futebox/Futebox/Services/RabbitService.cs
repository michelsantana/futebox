using Futebox.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class RabbitService : IDisposable
    {

        public RabbitService()
        {
            CreateClient();
        }
        public List<Action<RabbitMessage>> WhenDefaults { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void WhenDefault(RabbitMessage message)
        {
            
        }

        private void CreateClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (s, e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());
                    Console.WriteLine(Environment.NewLine + "[Nova mensagem recebida] " + message);
                };

                channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);

                Console.WriteLine("Aguardando mensagens para processamento");
                Console.WriteLine("Pressione uma tecla para encerrar...");

                Console.ReadLine();
            }
        }
    }
}
