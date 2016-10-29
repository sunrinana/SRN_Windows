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
        private int time = 0; //500ms마다 한번씩으로 카운트
        private SerialPort arduSerialPort = new SerialPort();
        public Form1()
        {
            InitializeComponent();
        }

        private void send_server()
        {
            /*
             * 서버에 POST 메세지로
             * 일 안할때만 no work
             * 뭐 이딴식으로 보낸다
             * 일할때는 보낼 필요도
             * 없을 듯 하다.
             */
            var request = (HttpWebRequest)WebRequest.Create("http://working.anapp.kr/send");
            var postData = "sleep";
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            Console.WriteLine("Send");
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //var response = (HttpWebResponse)register_request.GetResponse();
            //string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //MessageBox.Show(responseString);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arduSerialPort.PortName = "COM3";    //아두이노가 연결된 시리얼 포트 번호 지정
            arduSerialPort.BaudRate = 9600;       //시리얼 통신 속도 지정
            arduSerialPort.Open();                //포트 오픈
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
             * TODO : cnt는 0.5초마다 한번씩 작동,
             * 따라서 1초에 두번 읽어온다.
             * 그렇가면 if((cnt / 2) == 60)인 조건에서 일을 안 한다고 가정하고 전송한다
             * 
             */
             
            string input = arduSerialPort.ReadLine();
            //Console.WriteLine(input);
            if(input[0] == '0')
            {
                //움직임 없으면
                time+=1;
                Console.WriteLine("+++");
            }
            else
            {
                time = 0;
            }
            if(time == 30)
            {
                Console.WriteLine("SEX");
                send_server();
            }
            textBox1.Text = input + "\n";
        }
    }
}
