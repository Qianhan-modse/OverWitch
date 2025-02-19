using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using Servered;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        public Server server;
        public Program()
        {
            server = new Server();
        }

        static void Main(string[] args)
        {
            Server server = new Server();
            server.StartServer();
        }
    }
}
namespace Servered
{
    /// <summary>
    /// 用于处理服务器的基本类
    /// </summary>
    public class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private Thread inputThread;  // 用于处理控制台输入的线程
        private BanManager banManager;

        public Server()
        {
            banManager = new BanManager();
        }

        public void StartServer()
        {
            string ipAddress = string.Empty;
            Console.WriteLine("请输入服务器的IP地址：");
            ipAddress = Console.ReadLine();

            // 弹出确认提示
            Console.WriteLine($"您输入的IP地址是: {ipAddress}，是否确定使用此地址作为服务器地址？ (Y/N)");
            string userResponse = Console.ReadLine().ToLower();
            if (userResponse == "y" || userResponse == "yes" || userResponse == "true")
            {
                // 如果玩家确认，开始启动服务器
                Console.WriteLine("确认地址，启动服务器...");
                StartListening(ipAddress, 30660); // 端口可以根据需求修改
                StartConsoleInputThread(); // 启动控制台输入线程
            }
            else
            {
                // 如果玩家取消，退出服务器启动
                Console.WriteLine("服务器启动已取消。");
            }
        }

        private void StartListening(string ipAddress, int port)
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start();
                Console.WriteLine("服务器已启动，等待客户端连接...");
                listenThread = new Thread(ListenForClients);
                listenThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"服务器启动失败: {ex.Message}");
            }
        }

        private void ListenForClients()
        {
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                string clientIp = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();

                // 检查客户端的 IP 是否被封禁
                if (banManager.GetBannedIps().Contains(clientIp))
                {
                    Console.WriteLine($"拒绝连接，IP {clientIp} 被封禁！");
                    tcpClient.Close();
                    continue;
                }

                // 检查玩家是否被封禁（假设玩家名称从客户端传输）
                string playerName = "playerNameFromClient"; // 需要通过某种方式获取玩家名称
                if (banManager.GetBannedPlayers().Contains(playerName))
                {
                    Console.WriteLine($"拒绝连接，玩家 {playerName} 被封禁！");
                    tcpClient.Close();
                    continue;
                }

                Console.WriteLine("客户端已连接");
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

        // 启动控制台输入线程
        private void StartConsoleInputThread()
        {
            inputThread = new Thread(HandleConsoleInput);
            inputThread.Start();
        }

        private void HandleConsoleInput()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    string[] commandArgs = input.Split(' ');

                    if (commandArgs[0] == "ban")
                    {
                        if (commandArgs.Length == 3)
                        {
                            string playerName = commandArgs[1];
                            string banTime = commandArgs[2];
                            BanPlayer(playerName, banTime);
                        }
                        else
                        {
                            Console.WriteLine("使用格式: ban {player} {time}");
                        }
                    }
                    else if (commandArgs[0] == "banip")
                    {
                        if (commandArgs.Length == 3)
                        {
                            string ip = commandArgs[1];
                            string banTime = commandArgs[2];
                            BanIp(ip, banTime);
                        }
                        else
                        {
                            Console.WriteLine("使用格式: banip {ip} {time}");
                        }
                    }
                    else if (commandArgs[0] == "unban")
                    {
                        if (commandArgs.Length == 2)
                        {
                            string playerName = commandArgs[1];
                            UnbanPlayer(playerName);
                        }
                        else
                        {
                            Console.WriteLine("使用格式: unban {player}");
                        }
                    }
                    else if (commandArgs[0] == "unbanip")
                    {
                        if (commandArgs.Length == 2)
                        {
                            string ip = commandArgs[1];
                            UnbanIp(ip);
                        }
                        else
                        {
                            Console.WriteLine("使用格式: unbanip {ip}");
                        }
                    }
                    else if (commandArgs[0] == "gc")
                    {
                        // 当输入 "gc" 时触发垃圾回收
                        Console.WriteLine("正在进行垃圾回收...");
                        System.GC.Collect();  // 触发垃圾回收
                        System.GC.WaitForPendingFinalizers();//等待最终清理线程执行完毕
                        System.GC.Collect();//再次执行GC回收
                        Console.WriteLine("垃圾回收已完成");
                    }
                    else if (commandArgs[0] == "stop")
                    {
                        StopServer();
                    }
                    else
                    {
                        Console.WriteLine("无效命令！");
                    }
                }
            }
        }

        private void BanPlayer(string playerName, string banTime)
        {
            if (banTime == "infinity")
            {
                banManager.BanPlayer(playerName);
                Console.WriteLine($"玩家 {playerName} 被永久封禁！");
            }
            else
            {
                if (int.TryParse(banTime, out int banDuration))
                {
                    banManager.BanPlayer(playerName);
                    Console.WriteLine($"玩家 {playerName} 被封禁 {banDuration} 分钟！");
                    Thread.Sleep(banDuration * 60000);
                    banManager.UnbanPlayer(playerName);
                    Console.WriteLine($"玩家 {playerName} 封禁时间到，解除封禁。");
                }
                else
                {
                    Console.WriteLine("无效的封禁时间！");
                }
            }
        }

        private void BanIp(string ip, string banTime)
        {
            if (banTime == "infinity")
            {
                banManager.BanIp(ip);
                Console.WriteLine($"IP {ip} 被永久封禁！");
            }
            else
            {
                if (int.TryParse(banTime, out int banDuration))
                {
                    banManager.BanIp(ip);
                    Console.WriteLine($"IP {ip} 被封禁 {banDuration} 分钟！");
                    Thread.Sleep(banDuration * 60000);
                    banManager.UnbanIp(ip);
                    Console.WriteLine($"IP {ip} 封禁时间到，解除封禁。");
                }
                else
                {
                    Console.WriteLine("无效的封禁时间！");
                }
            }
        }

        private void UnbanPlayer(string playerName)
        {
            banManager.UnbanPlayer(playerName);
            Console.WriteLine($"玩家 {playerName} 已解除封禁。");
        }

        private void UnbanIp(string ip)
        {
            banManager.UnbanIp(ip);
            Console.WriteLine($"IP {ip} 已解除封禁。");
        }

        private void StopServer()
        {
            try
            {
                Console.WriteLine("服务器正在停止...");
                tcpListener.Stop();//先停止监听新连接
                listenThread?.Abort();//停止监听线程
                inputThread?.Abort();//停止控制台输入线程
                Console.WriteLine("服务器已停止！");
            }
            catch (SocketException ex)
            {
                // 捕获并处理 SocketException 异常
                Console.WriteLine($"停止服务器时发生错误: {ex.Message}");
            }
            catch (Exception ex)
            {
                // 捕获其他类型的异常
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }

    }

    public class BanManager
    {
        private string banDirectory = "config-Ban";
        private string playerBanFile = "config-Ban/players.txt";
        private string ipBanFile = "config-Ban/ips.txt";

        public BanManager()
        {
            // 确保目录和文件存在
            if (!Directory.Exists(banDirectory))
            {
                Directory.CreateDirectory(banDirectory);
            }

            if (!File.Exists(playerBanFile))
            {
                File.Create(playerBanFile).Close();
            }

            if (!File.Exists(ipBanFile))
            {
                File.Create(ipBanFile).Close();
            }
        }

        // 获取被封禁的玩家列表
        public List<string> GetBannedPlayers()
        {
            return File.ReadAllLines(playerBanFile).ToList();
        }

        // 获取被封禁的IP列表
        public List<string> GetBannedIps()
        {
            return File.ReadAllLines(ipBanFile).ToList();
        }

        // 将玩家添加到封禁列表
        public void BanPlayer(string playerName)
        {
            List<string> bannedPlayers = GetBannedPlayers();
            if (!bannedPlayers.Contains(playerName))
            {
                bannedPlayers.Add(playerName);
                File.WriteAllLines(playerBanFile, bannedPlayers.ToArray()); // 转换为 string[] 类型
            }
        }

        public void BanIp(string ip)
        {
            List<string> bannedIps = GetBannedIps();
            if (!bannedIps.Contains(ip))
            {
                bannedIps.Add(ip);
                File.WriteAllLines(ipBanFile, bannedIps.ToArray()); // 转换为 string[] 类型
            }
        }

        public void UnbanPlayer(string playerName)
        {
            List<string> bannedPlayers = GetBannedPlayers();
            if (bannedPlayers.Contains(playerName))
            {
                bannedPlayers.Remove(playerName);
                File.WriteAllLines(playerBanFile, bannedPlayers.ToArray()); // 转换为 string[] 类型
            }
        }

        public void UnbanIp(string ip)
        {
            List<string> bannedIps = GetBannedIps();
            if (bannedIps.Contains(ip))
            {
                bannedIps.Remove(ip);
                File.WriteAllLines(ipBanFile, bannedIps.ToArray()); // 转换为 string[] 类型
            }
        }
    }

}
