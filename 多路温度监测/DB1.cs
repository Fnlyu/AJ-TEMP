using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
// using MQTTnet.Client;
using MQTTnet;
// using MQTTnet.Server;

namespace mqtt_demo
{
    class myTcpServer
    {
        public string strOut = "";

        public string strIP = "127.0.0.1";
        public int port = 9050;

        //初始化要连接的ip和端口号
        //public  IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(strIP), 9050);

        //创建套接字
        public Socket newsock = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        public Socket client;

        public void Tcp_Send(string str)
        {
            if (client.Connected)
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                client.Send(data);
            }
        }

        public void Start()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(strIP), port);  //   IPAddress.Any, 9050);

            //套接字与ip,端口绑定
            newsock.Bind(ipep);

            //套接字收听接入连接
            newsock.Listen(10);

            Console.WriteLine("9050waiting for a client... ");

            //3.receive data
            //new Thread(Thread_Connect) { IsBackground = true }.Start();
            //当初始化一个线程，把Thread.IsBackground = true的时候，指示该线程为后台线程。后台线程将会随着主线程的退出而退出。
            new Thread(Thread_Receive) { IsBackground = true }.Start();

        }
        /*
         void Thread_Connect()
        {
            while (true)
            {
                try
                {
                    //将套接字与远程主机相连，此时服务器端应该已经启动Accept
                    if (!server.Connected) server.Connect(ipep);
                }
                catch (SocketException e)//连接异常处理
                {
                    server.Close();
                }
                Thread.Sleep(1000);
            }
        }*/

        void Thread_Receive()
        {
            int recv = 0;
            byte[] data = new byte[1024];

            //接收来自客户机的接入连接尝试，
            //返回一个新的套接字，在于客户机的通信中使用这个新的套接字
            client = newsock.Accept();//同步方法

            //客户机的ip和端口信息
            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("connected with {0} at port {1}", clientep.Address, clientep.Port);

            string welcom = "welcom to my test server";
            data = Encoding.ASCII.GetBytes(welcom);

            //发出欢迎信息
            client.Send(data, data.Length, SocketFlags.None);


            while (true)
            {

                //Recive在使用buffer的同时，重新设置了buffer的大小，因此重新设置为原大小
                data = new byte[1024]; 

                try { recv = client.Receive(data); }
                catch { }

                if (recv == 0)
                {
                    break;//客户端退出则服务器端退出                 
                }
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

                try { client.Send(data, recv, SocketFlags.None); }
                catch { }

                strOut = Encoding.ASCII.GetString(data, 0, recv);

                Thread.Sleep(10);
            }
        }

    }

    class myTcp
    {
        public string strOut = "";

        public string strIP = "127.0.0.1";

        //初始化要连接的ip和端口号
        //public  IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(strIP), 9050);

        //创建套接字
        public Socket client = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        public void Tcp_Send(string str)
        {
            if (client.Connected)
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                client.Send(data);
            }
        }

        public void Start()
        {
            //3.receive data
            //new Thread(Thread_Connect) { IsBackground = true }.Start();
            //当初始化一个线程，把Thread.IsBackground = true的时候，指示该线程为后台线程。后台线程将会随着主线程的退出而退出。
            new Thread(Thread_Receive) { IsBackground = true }.Start();

        }
        /*
         void Thread_Connect()
        {
            while (true)
            {
                try
                {
                    //将套接字与远程主机相连，此时服务器端应该已经启动Accept
                    if (!client.Connected) client.Connect(ipep);
                }
                catch (SocketException e)//连接异常处理
                {
                    client.Close();
                }
                Thread.Sleep(1000);
            }
        }*/

        void Thread_Receive()
        {
            int recv = 0;
            byte[] data = new byte[1024];
            while (true)
            {
                if (client.Connected)
                {
                    try { recv = client.Receive(data); }
                    catch { }

                    strOut = Encoding.ASCII.GetString(data, 0, recv);
                }
                Thread.Sleep(10);
            }
        }

    }



    class DB1
    {
        public static SqlConnection conn;
        public static void Open_db()
        {
            try
            {
                string ConStr = "server=.;database=test;uid=sa;pwd=`sql2016";
                conn = new SqlConnection(ConStr);
                conn.Open();
            }
            catch
            {
                //MessageBox.Show("连接数据库失败");
            }
        }

        public static void Insert_data(string str)
        {
            if (conn.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string strSQL = "INSERT INTO LOG(时间,信息) "
                              + "VALUES ('" + DateTime.Now.ToString() + "','" + str+  "')";

                cmd.CommandText = strSQL;  //插入数据SQL语句
                cmd.CommandType = CommandType.Text;
                int i = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private static void Insert_data(int id, double val)
        {
            if (conn.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string strSQL = "INSERT INTO data(ID , 数值, 时间) "
                                + "VALUES (" + id + ", " + val
                                + ", '" + DateTime.Now.ToString() + "')";

                cmd.CommandText = strSQL;  //插入数据SQL语句
                cmd.CommandType = CommandType.Text;
                int i = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static void Insert_data(int id, double val, DateTime dt)
        {
            if (conn.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string strSQL = "INSERT INTO data(ID , 数值, 时间) "
                                + "VALUES (" + id + ", " + val
                                + ", '" + dt.ToString() + "')";

                cmd.CommandText = strSQL;  //插入数据SQL语句
                cmd.CommandType = CommandType.Text;
                int i = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

    }

    class myUdp
    {

        //创建Socket
        public Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public string 目标板IP = "127.0.0.1";
        public int 目标板Port = 502;

        string str_ip = "127.0.0.1";
        public int local_port = 8089;    //Server Bind

        public string strOut = "";
        public string strOutHex = "";

        public static string ToHexString(byte[] bytes, int start, int len)
        {
            string strReturn = "";
            for (int i = start; i < (start + len); i++)
            {
                byte bt = bytes[i];
                strReturn += " " + bt.ToString("x2");
            }
            return strReturn;
        }

        public void Udp_Send_KG(bool KG)
        {
            EndPoint serverPoint = new IPEndPoint(IPAddress.Parse(目标板IP), 目标板Port);
            byte[] data;
            if(KG)
                data = new byte[] { 0X01, 0X01, 0X00, 0X00, 0X00, 0X08, 0X00, 0X0F, 0X00, 0X07, 0X00, 0X01, 0X01, 0X01 };
            else
                data = new byte[] { 0X01, 0X01, 0X00, 0X00, 0X00, 0X08, 0X00, 0X0F, 0X00, 0X07, 0X00, 0X01, 0X01, 0X00 };
            //ON - 0X01,0X03,0X00,0X00,0X00,0X08,0X00,0X0F,0X00,0X07,0X00,0X01,0X01,0X01};
           //OFF - 0X01,0X01,0X00,0X00,0X00,0X08,0X00,0X0F,0X00,0X07,0X00,0X01,0X01,0X00

            udpClient.SendTo(data, serverPoint);
        }

        public void Udp_Send(string str)
        {
            EndPoint serverPoint = new IPEndPoint(IPAddress.Parse(目标板IP), 目标板Port);
            //string message = Console.ReadLine();
            //string message = textBox1.Text;
            byte[] data = Encoding.UTF8.GetBytes(str);

            udpClient.SendTo(data, serverPoint);
        }

        public void Udp_Send(byte[] data)
        {
            EndPoint serverPoint = new IPEndPoint(IPAddress.Parse(目标板IP), 目标板Port);
            //string message = Console.ReadLine();
            //string message = textBox1.Text;
            //byte[] data = Encoding.UTF8.GetBytes(str);
            udpClient.SendTo(data, serverPoint);
        }

        public void Start_Receive()
        {
            //1.Socket creat
            //udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //2.Bind ip and port
            udpClient.Bind(new IPEndPoint(IPAddress.Parse(str_ip), local_port));

            //3.receive data
            new Thread(Thread_Receive) { IsBackground = true }.Start();
            //当初始化一个线程，把Thread.IsBackground = true的时候，指示该线程为后台线程。后台线程将会随着主线程的退出而退出。
        }

        void Thread_Receive()
        {
            while (true)
            {
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);   //远程来访对象：套接字IP+Port

                byte[] data = new byte[1024];
                int length = udpClient.ReceiveFrom(data, ref remoteEndPoint);//此方法把数据来源ip、port放到第二个参数中

                //string message = Encoding.UTF8.GetString(data, 0, length);
                //string message = Encoding.Unicode.GetString(data, 0, length);
                string message = Encoding.ASCII.GetString(data, 0, length);

                strOut = "从ip" + (remoteEndPoint as IPEndPoint).Address.ToString() + ":" + (remoteEndPoint as IPEndPoint).Port
                              + "Get" + message;

                strOutHex = ToHexString(data, 0, length);
                //remoteEndPoint.Address.ToString();
                //Console.WriteLine(strOut);

                Thread.Sleep(10);

                String str = "SET TIME " + DateTime.Now.ToString();
                //byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);

                //string转ASCII byte[]:
                //byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(str);
                //udpClient.SendTo(byteArray, remoteEndPoint);
            }
        }
    }

    class myOneNet
    {
        // private MqttFactory mqttFactory1 = null;
        public IMqttClient mqttClient1 = null;
        string broker = "mqtts.heclouds.com";
        public string username = "01GtlXme09";
        public string password = "version=2018-10-31&res=products%2F01GtlXme09%2Fdevices%2FDev1&et=1750207610&method=md5&sign=ZtJOEL1rMRZMNJki6cV0SQ%3D%3D";
        public string DEVID = "Dev1";

        public string txtCustId, txtTopic, subTopic1, subTopic2;
        public string txtmsg="";
        public string cmd = "";

        public void Init()
        {
            // mqttFactory1 = new MqttFactory();
            // Environment f1 = new Environment();
            txtCustId = DEVID;
            subTopic1 = "$sys/" + username + "/" + DEVID + "/cmd/request/+";
            subTopic2 = "$sys/" + username + "/" + DEVID + "/dp/post/json/+";
            txtTopic = "$sys/" + username + "/" + DEVID + "/dp/post/json";
            Connect();
            Thread.Sleep(2000);
            // if (mqttClient1.IsConnected)    //若为返回主题，则处理收到下发命令
            // {
            //     Subscribe(subTopic1);// 100毫秒后订阅主题1
            // }

        }

        public string OneJson(int id, string sensor, string val)
        {
            string t;
            t = "{\"id\":" + id + ",\"dp\": {\"" + sensor + "\": [{\"v\":" + val + "}]}}";
            return t;
        }

        private async void Connect()
        {
            try
            {
                // MqttFactory mqttFactory = new MqttFactory();
                // mqttClient1 = mqttFactory.CreateMqttClient();
                //连接 MQTT 服务器
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    //.WithTcpServer(txtUrl.Text, Convert.ToInt32(txtPort.Text))   // MQTT 服务器IP+端口
                    .WithTcpServer(broker, 1883)
                    .WithClientId(txtCustId)         // 客户端名称 DEV1
                    .WithCredentials(username, password) // Set username and password

                    .Build();
                mqttClient1.ApplicationMessageReceivedAsync += e2 =>
                {
                    //Console.WriteLine("Received application message.");
                    //Console.WriteLine(e2.ApplicationMessage.Topic);
                    //Console.WriteLine(Encoding.UTF8.GetString(e2.ApplicationMessage.Payload));
                    //Console.WriteLine("===================");
                    string topic = e2.ApplicationMessage.Topic;
                    string payload = Encoding.UTF8.GetString(e2.ApplicationMessage.Payload);

                    if (topic.IndexOf("cmd/request") > 0)    //若为返回主题，则处理收到下发命令
                    {
                        //if (MessageBox.Show(payload, "是否执行控制命令", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{MessageBox.Show("执行控制命令:" + payload);}
                        cmd = payload; 
                    }
                    txtmsg = "收到消息。主题：{" + topic + "} , 内容： {" + payload + "}.\r\n" ;
                    return Task.CompletedTask;
                };
                var response = await mqttClient1.ConnectAsync(mqttClientOptions, CancellationToken.None);
                txtmsg = "MQTT客户端已连接. IsConnected: [{mqttClient1.IsConnected}] resultCode:{response.ResultCode}\r\n";
            }
            catch
            {
                txtmsg = "连接失败.\r\n";
            }
        }

        // private async void Subscribe(string tp)     //订阅主题
        // {
        //     if (mqttClient1 == null) return;
        //     if (!mqttClient1.IsConnected) return;
        //
        //     //订阅名主题的消息
        //     var mqttSubscribeOptions = mqttFactory1.CreateSubscribeOptionsBuilder()
        //     .WithTopicFilter(
        //         f =>
        //         {
        //             f.WithTopic(tp);
        //             f.WithExactlyOnceQoS();     //即精准一次
        //         })
        //     .Build();
        //     await mqttClient1.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        //     txtmsg = "已订阅主题：" + tp + "\r\n" ;
        //
        // }

        public async void 上传数据(string Sensor, string value)
        {
            if (mqttClient1 == null) return;
            if (!mqttClient1.IsConnected) return;

            string t = OneJson(1, Sensor, value);
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(txtTopic)
                .WithPayload(t)
                .Build();

            //异步发布消息
            await mqttClient1.PublishAsync(applicationMessage, CancellationToken.None);
            txtmsg = "";
            //txtmsg = "消息已发送\r\n" + t;
        }


    }
}
