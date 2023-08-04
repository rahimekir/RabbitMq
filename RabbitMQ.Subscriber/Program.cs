using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://kfsdoqic:jn0RezxxesWFhHedSXKfrxlcieC0PsC_@fish.rmq.cloudamqp.com/kfsdoqic");

            // factory üzerinden bağlantı açma
            using var connection = factory.CreateConnection();

            // bağlantıya  rabbitmq'ya kanal üzerinden bağlanma
            var channel = connection.CreateModel();  // kanal oluşturma

            //ister kullan ister sil. silersek subscriberı ayağa kaldırdığımızda yoksa hata verir.
            //publisher bu kuyruğu oluşturuyorsa gerek yok. 
            // channel.QueueDeclare("hello-queue", true, false, false);

            // nesnemizi oluşturduk.
            var subscriber = new EventingBasicConsumer(channel);

            //subscriberın dinleyeceği kuyrulu belirtme işlemi
            //BasicConsume(string queue, bool autoAck)

            //autoAck=> true : Mesaj doğru da işlense yanlış da işlense kuyruktan mesajı siler.
            //autoAck=> false :Rabbit mq kuyruktan silmez. mesajı doğru bir sekilde işlenirse haber verip sildirir.
            channel.BasicConsume("hello-queue", true, subscriber);


            //Artık event üzerinden mesaj dinlenebilir.
            // bu event (Received) , rabbit mq subscribera mesaj gönderdiğinde fırlayacak.
            subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());  // mesajı alma 
                Console.WriteLine("Gelen mesaj : " + message);
            };

            Console.ReadLine();
        }


    }
}