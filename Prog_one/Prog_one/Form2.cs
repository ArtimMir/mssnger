using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MaterialSkin.Controls;
using System.Drawing;

namespace Prog_one
{

    public partial class Form2 : MaterialForm
    {
        static string nickname;
        private const string host = "192.168.56.1";
        private const int port = 1234;

        //Socket socket;
        public static string Nickname { get; set; }
        public static List<string> Nicknames = new List<string>();
        public static Socket socket;

        public Form2(Socket socket1)
        {
            InitializeComponent();
            socket = socket1;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            nickname = Nickname;
            Thread CheckNick = new Thread(CheckNewNicknames);
            CheckNick.Start();
            
            //CheckNewNicknames();

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                richTextBox1.Text += DateTime.Now + "\n" + nickname + ": " + textBox1.Text + "\n";
                SendMessage(socket, textBox1.Text, nickname);
                textBox1.Text = "";
                //try
                //{


                //    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(host), port);

                //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //    // подключаемся к удаленному хосту
                //    socket.Connect(ipPoint);

                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
            //SendMessage(textBox1.Text);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            base.OnFormClosing(e);
            Environment.Exit(0);
        }
        static void SendMessage(Socket socket, string message, string nickname)
        {
            //MessageBox.Show("Все окей!");
            byte[] data = Encoding.Unicode.GetBytes(nickname + ": " + message);
            socket.Send(data);
        }
        public void CheckNewNicknames()
        {
            //Form2 frm2 = new Form2(socket);
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
                    string message = builder.ToString();
                    //получаем кол-во пользователей и сами ники
                    if ((message.IndexOf("countNicknames") > -1))
                    {
                        string nick = message.Replace("countNicknames", "");
                        int count = Int32.Parse(nick);
                        for(int i = 0; i < count; i++)
                        {
                            try
                            {
                                data = new byte[64]; // буфер для получаемых данных
                                builder = new StringBuilder();
                                bytes = 0;
                                do
                                {
                                    bytes = socket.Receive(data, data.Length, 0);
                                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                                }
                                while (socket.Available > 0);
                                message = builder.ToString();

                                if ((message.IndexOf(Nickname) > -1))
                                {
                                    Console.WriteLine(message.IndexOf(Nickname));
                                }
                                else if(!Nicknames.Contains(message.Replace("nickname", "")))
                                {
                                    //Nicknames.Clear();
                                    string nickN = message.Replace("nickname", "");
                                    Nicknames.Add(nickN);
                                    LoadData(nickN);
                                    //listBox1.Invoke(new Action(() => listBox1.Items.Add(nickN)));
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        
        // метод выводит данные в listView1
        private void LoadData(string Nick)
        {
            // очищаем listView1
            //listView1.Items.Clear();

            // создаем список изображений для строк listView1
            ImageList imageList = new ImageList();

            // устанавливаем размер изображений
            imageList.ImageSize = new Size(50, 50);
            //Создание объекта для генерации чисел
            Random rnd = new Random();

            //Получить случайное число (в диапазоне от 0 до 10)
            int value = rnd.Next(1, 28);
            // заполняем список изображениями
            imageList.Images.Add(new Bitmap("C:/Users/ACER/source/repos/Prog_one/Prog_one/Resources/icons/icons" + value + ".png"));
            

            // создадим пустое изображение (просто белая заливка)
            Bitmap emptyImage = new Bitmap(50, 50);

            // получим объект Graphics для редактирования изображения
            using (Graphics gr = Graphics.FromImage(emptyImage))
            {
                // выполним заливку изображения emptyImage белым цветом
                gr.Clear(Color.White);
            }

            // и добавим его в список
            imageList.Images.Add(emptyImage);

            // устанавливаем в listView1 список изображений imageList
            //listView1.SmallImageList = imageList;
            listView1.Invoke(new Action(() => listView1.SmallImageList = imageList));
            // массив имен, которые будем выводить в listView1
            string[] firstNames = { Nick };

            // добавляем строки в listView1
            for (int i = 0; i < firstNames.Length; i++)
            {
                // создадим объект ListViewItem (строку) для listView1
                ListViewItem listViewItem = new ListViewItem(new string[] { "", firstNames[i] });

                // индекс изображения из imageList для данной строки listViewItem
                listViewItem.ImageIndex = i;

                // добавляем созданный элемент listViewItem (строку) в listView1
                //listView1.Items.Add(listViewItem);
                listView1.Invoke(new Action(() => listView1.Items.Add(listViewItem)));
                Thread.Sleep(100);
                listView1.Items[0].Focused = true;
                listView1.Items[0].Selected = true;
            }
        }

        
        private void ListView1_ItemActivate(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the ListView.ItemActivate event.");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItemText = listView1.SelectedItems.ToString();
            MessageBox.Show("Selected: " + selectedItemText);
        }

        //public void AddName(string Value)
        //{
        //    label1.Text += Value;
        //}

    }
}
