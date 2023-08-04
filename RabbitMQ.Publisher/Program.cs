using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://kfsdoqic:jn0RezxxesWFhHedSXKfrxlcieC0PsC_@fish.rmq.cloudamqp.com/kfsdoqic");

            // Factory üzerinden bağlantı açma
            using var connection = factory.CreateConnection();

            // Bu bağlantıya  rabbitmq'ya kanal üzerinden bağlanma
            var channel = connection.CreateModel();  // kanal oluşturma

            // mesajların gidebilmesi için kuyruk oluşturmak gerekiyor 
            //QueueDeclare(string queue,bool durable, bool exclusive ,bool autoDelete,IDictionary<string,object> arguments)

            //kuyruk ismi 

            //durable :false => rabbit mqda olusan kuyruklar  memoryde tutulur. rabbit mq restart atarsa tüm kuyruklar silinir 
            //durable :true => kuyruklar hiç bi zaman kaybolmaz. fiziksel olarak tutulur. (önerilen)

            //exclusive: true => bu kuyruğa sadece burada oluşturulan kanal üzerinden bağlanılabilir.
            //Subscriber tarafınan başka bir kanal üzerinden bağlanmak için false yapılmalıdır.

            //autoDelete :true => eğer kuyruğa bağlı olan son subscriber da bağlantısını koparırısa otomatik olarak kuyruk silinir. (önerilmez)
            //çünkü subscriber yanlışlıkla down olursa kuyruk silinir. kuyruk her zaman ayakta durmalı. 

            channel.QueueDeclare("hello-queue",true,false,false);

            string message = "hello queen ";  // bu stringi rabbit mq ya göndereceğiz
                                              //rabbit mq ya mesajları byte dizini olarak gönderiririz. bu sayede istediğimiz her şeyi gönderebiliriz (pdj , image,büyük dosyalar vs)
            var messageBody = Encoding.UTF8.GetBytes(message);

            //BasicPublish(string exchange, string routingKey, IBasicProporties basicProporties,ReadonlyMemory<byte>)
            //şuan exchange kullanmıyoruz 
            //routingKey kuyruğun ismi verilir. bu sayede bu roadmape e  göre gelen mesajı istenilen kuyruğa gönderebilsin.
            channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

            Console.WriteLine("Mesaj gönderilmiştir.");
            Console.ReadLine();
        }
    }
}
