using System;
using System.Net.Sockets;
using System.Text;
namespace GameCliented
{
    /// <summary>
    /// ���ڴ���ͻ��˵Ļ�����
    /// </summary>
    public class GameClient
    {
        private TcpClient tcpClient;
        private NetworkStream stream;

        public void ConnectToServer(string ipAddress, int port)
        {
            tcpClient = new TcpClient(ipAddress, port);
            stream = tcpClient.GetStream();
            Console.WriteLine("�����ӵ�������!");

            // �������շ�������Ϣ���߳�
            System.Threading.Thread receiveThread = new System.Threading.Thread(ReceiveMessages);
            receiveThread.Start();

            // �������������Ϣ
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
                Console.WriteLine("�յ���������Ϣ: " + message);
            }
        }
    }

}