using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SRN_Client
{
    public partial class Form1 : Form
    {
        private SerialPort arduSerialPort = new SerialPort();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpWebRequest register_request = (HttpWebRequest)WebRequest.Create("http://working.anapp.kr:3000/send ");
            int postData = 1;
            // postData = "drop database;";//JsonConvert.SerializeObject(u);
            var data = Encoding.ASCII.GetBytes(postData.ToString());
            register_request.Method = "POST";
            register_request.ContentType = "application/x-www-form-urlencoded";

            register_request.ContentLength = data.Length;
            using (var stream = register_request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)register_request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            MessageBox.Show(responseString);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arduSerialPort.PortName = "COM3";    //아두이노가 연결된 시리얼 포트 번호 지정
            arduSerialPort.BaudRate = 9600;       //시리얼 통신 속도 지정
            arduSerialPort.Open();                //포트 오픈
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string input = arduSerialPort.ReadLine();
            textBox1.Text = input + "\n";
        }
    }
}
