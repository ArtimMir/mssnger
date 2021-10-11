using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Prog_one
{

    public partial class Form1 : MaterialForm
    {
        private const string host = "192.168.56.1"; 
        private const int port = 1234;
        public static Socket socket;
        public Form1(Socket socket1)
        {
            InitializeComponent();
            socket = socket1;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        { 
            //передаем все что нужно на новую форму
                
            if(materialSingleLineTextField2.Text != "") //если пользователь ввёл имя
            {
                SendName(socket, materialSingleLineTextField2.Text);
                this.Hide();
                Form2 newForm = new Form2(socket);
                Form2.Nickname = materialSingleLineTextField2.Text; //передаем во вторую форму nickname
                newForm.Show();

                //pictureBox1.Visible = true;
                //panel1.Visible = true;
                //panel2.Visible = true;
                //try
                //{
                //    //получаем адреса для запуска сокета
                //    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(host), port);
                //    //создаем сокет
                //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //    //подключаемся к удаленному хосту
                //    socket.Connect(ipPoint);
                //    SendName(socket, materialSingleLineTextField2.Text);

                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
                //nickname = materialSingleLineTextField2.Text;
                //base.OnFormClosing(e);
            }

        }
        static void SendName(Socket socket, string nickname)
        {
            byte[] data = Encoding.Unicode.GetBytes(nickname);
            socket.Send(data);
        }
        /*public static void ReceiveMessage(Socket socket)
        {
            Form2 frm2 = new Form2();
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);

                    string nicknames = builder.ToString();

                    //добавляем nickname в список
                    //Form2.Nicknames.Add(nicknames);

                    frm2.AddName(nicknames);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }*/
    }
}
