using System.Net;

namespace UserStorage.Replication
{
    public interface ITcpListener
    {
        IPEndPoint ClientEndPoint { get; }
    }
}