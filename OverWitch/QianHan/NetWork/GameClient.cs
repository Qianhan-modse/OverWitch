using System;
using System.Net.Sockets;
using System.Text;
namespace GameCliented
{
    /// <summary>
    /// 用于处理客户端的基本类
    /// </summary>
    public class GameClient
    {
        private TcpClient tcpClient;
        private NetworkStream stream;

        public void ConnectToServer(string ipAddress, int port)
        {
            tcpClient = new TcpClient(ipAddress, port);
            stream = tcpClient.GetStream();
            Console.WriteLine("已连接到服务器!");

            // 启动接收服务器消息的线程
            System.Threading.Thread receiveThread = new System.Threading.Thread(ReceiveMessages);
            receiveThread.Start();

            // 向服务器发送消息
            SendMessage("Hello, Server!");
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("收到服务器消息: " + message);
            }
        }
    }

}