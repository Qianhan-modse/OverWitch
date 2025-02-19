using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameServer
{
    /// <summary>
    /// ���ڴ���������Ļ�����
    /// </summary>
    public class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public void StartServer(string ipAddress, int port)
        {
            tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            tcpListener.Start();
            Console.WriteLine("���������������ȴ��ͻ�������");
            listenThread = new Thread(ListenForClients);
            listenThread.Start();
        }

        private void ListenForClients()
        {
            //����ѭ��
            while (true)
            {
                //�ȴ��ͻ�������
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                Console.WriteLine("�ͻ���������");
                //����ͻ�������
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
                    Console.WriteLine("�յ���Ϣ: " + message);

                    // ��ͻ��˷��ͻ�Ӧ
                    byte[] response = Encoding.ASCII.GetBytes("��������Ӧ: " + message);
                    stream.Write(response, 0, response.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("�ͻ��˴����쳣: " + ex.Message);
                    break;
                }
            }

            tcpClient.Close();
        }
    }

}