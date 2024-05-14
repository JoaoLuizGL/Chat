using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Envia
{
    public partial class Form1 : Form
    {
        Socket socketenviar = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
        IPEndPoint enderecoip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9060);

        Socket socketreceber = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
        EndPoint endereco = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9070);
        byte[] data = new byte[1024];
        int qtdbytes;
        Thread mythread;
        public Form1()
        {
            InitializeComponent();
            socketreceber.Bind(endereco);
            mythread = new Thread(new ThreadStart(this.meuProcesso));
            mythread.Start();
        }

        private void meuProcesso()
        {
            while (true)
            {
                qtdbytes = socketreceber.ReceiveFrom(data, ref endereco);
                listBox1.Invoke((Action)delegate ()
                {
                    listBox1.Items.Add(Encoding.ASCII.GetString(data, 0, qtdbytes));
                });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("Jonas escreveu: " + textBox1.Text);
            socketenviar.SendTo(Encoding.ASCII.GetBytes("Jonas escreveu: " + textBox1.Text), enderecoip);
            textBox1.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            socketenviar.Close();
            mythread.Abort();
        }
    }
}
