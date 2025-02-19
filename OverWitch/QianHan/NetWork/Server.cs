using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameServer
{
    /// <summary>
    /// 用于处理服务器的基本类
    /// </summary>
    public class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public void StartServer(string ipAddress, int port)
        {
            tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            tcpListener.Start();
            Console.WriteLine("服务器已启动，等待客户端连接");
            listenThread = new Thread(ListenForClients);
            listenThread.Start();
        }

        private void ListenForClients()
        {
            //无限循环
            while (true)
            {
                //等待客户端连接
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                Console.WriteLine("客户端已连接");
                //处理客户端请求
                Thread clientThread = new Thread(() => HandleClient(tcpClient));
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient tcpClient)
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRead;
                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("收到消息: " + message);

                    // 向客户端发送回应
                    byte[] response = Encoding.ASCII.GetBytes("服务器回应: " + message);
                    stream.Write(response, 0, response.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("客户端处理异常: " + ex.Message);
                    break;
                }
            }

            tcpClient.Close();
        }
    }

}