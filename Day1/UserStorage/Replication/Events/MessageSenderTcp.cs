using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace UserStorage.Replication.Events
{
    public class MessageSenderTcp : IMessageSender
    {
        public void SendMessage(IEnumerable<ISlave> slaves, Message message)
        {
            var formatter = new BinaryFormatter();
            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, message);
                data = memoryStream.ToArray();
            }

            foreach (var slave in slaves)
            {
                NetworkStream stream = null;
                try
                {
                    var client = new TcpClient();
                    client.Connect(slave.ClientEndPoint);
                    stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
                finally
                {
                    stream?.Close();
                }
            }
        }
    }
}