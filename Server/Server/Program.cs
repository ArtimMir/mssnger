using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{

    public class ClientObject
    {
        bool t = true;
        public static Socket client;
        public static List<Socket> connectedClients = new List<Socket>();
        public static List<string> Nickname = new List<string>();

        public ClientObject(Socket socketClient, List<Socket> connectedClient)
        {
            client = socketClient;
            connectedClients = connectedClient;
            //Nickname = nickName;
        }

        public void Nicknames()
        {
            string path = @"C:\Users\ACER\source\repos\Server\Server\Nicknames.txt";
            try
            {
                //Socket handler = client;
                // получаем ник
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[256]; // буфер для получаемых данных

                do
                {
                    bytes = client.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (client.Available > 0);

                

                //записываем ник

                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("nickname" + builder.ToString());
                }
                // отправляем кол-во пользователей
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    int count = File.ReadAllLines(path).Length;
                    string countNicknames = "countNicknames" + count;
                    foreach (Socket client in connectedClients)
                    {
                        data = Encoding.Unicode.GetBytes(countNicknames);
                        client.Send(data);
                        Thread.Sleep(500);
                    }
                }

                //string nickname = "nickname" + builder.ToString();
                //Nickname.Add(nickname); //добавляем ник в список ников
                //отправляем ники
                foreach (Socket client in connectedClients)
                {
                    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            data = Encoding.Unicode.GetBytes(line);
                            client.Send(data);
                            Thread.Sleep(1500);
                        }
                    }
                    //Console.WriteLine("Проверка отправки " + data);
                }
                Console.WriteLine("Клиент " + builder.ToString() + " присоединился к чату!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Проблемка 4?");
            }
        }

        public void Messages()
        {
            while (t == true)
            {
                try
                {
                    //Socket handler = client;
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = client.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (client.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    // отправляем ответ
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    client.Send(data);
                    // закрываем сокет
                    //handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //client.Shutdown(SocketShutdown.Receive);
                    client.Dispose();
                    client.Close();
                    Console.WriteLine("Пользователь отключился.");
                    t = false;
                }
                finally
                {
                    //client.Close();
                    //Console.WriteLine("Проблемка 3?");
                }
            }
        }

        public void Process()
        {
            Nicknames(); //получение имён

            Messages(); //получение сообщений
        }
}
   
    class Program
    {
        public static List<Socket> connectedClients = new List<Socket>();
        //public static List<string> nickName = new List<string>();
        static void Main(string[] args)
        {
            string path = @"C:\Users\ACER\source\repos\Server\Server\Nicknames.txt";
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write("");
            }
            // Устанавливаем для сокета локальную конечную точку
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 1234);

            // Создаем сокет TCP/IP
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                listener.Bind(ipEndPoint);
                listener.Listen(10);
                Console.WriteLine("Ожидаем подключение клиента.");

                // Начинаем слушать соединения
                while (true)
                {
                    Socket handler = listener.Accept();

                    connectedClients.Add(handler);

                    ClientObject clientObject = new ClientObject(handler, connectedClients);

                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Проблемка 1?");
            }
            
        }
    }
}