using System.Data;
using System.Data.SqlClient;
using MQTTnet;
using MQTTnet.Client;
using System.Text;


namespace 多路温度监测;

public class db
{
    private static SqlConnection conn;

    public static void Open_db()
    {
        try
        {
            // var ConStr = "server=.\\SQLEXPRESS;database=Temperature;uid=sa;pwd=`sql2022";
            var ConStr = "Server=FBIX0024\\SQLEXPRESS;Database=Temperature;uid=sa;pwd=`sql2022";

            //string ConStr ="Data Source=.\\SQLEXPRESS; Initial Catalog=MonitorDB; User ID=sa; Password=`sql2016; Pooling=true" 

            conn = new SqlConnection(ConStr);
            conn.Open();
        }
        catch
        {
            MessageBox.Show("连接数据库失败！");
        }
    }

    public static void Insert_data(string str1, string str2, string str3, string str4)
    {
        if (conn.State == ConnectionState.Open)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            string table = "[Table_1]";
            string strSQL = "INSERT INTO " + table + " (Time,Temperature1,Temperature2,Temperature3,Switch) "
                            + "VALUES ('" + DateTime.Now.ToString() + "','" + str1 + "','" + str2 + "','" + str3 +
                            "','" + str4 + "')";

            cmd.CommandText = strSQL; //插入数据SQL语句
            cmd.CommandType = CommandType.Text;
            int i = Convert.ToInt32(cmd.ExecuteScalar());
        }
    }


    public static void Insert_备忘录(DateTime dt, string str)
    {
        if (conn.State == ConnectionState.Open)
        {
            var cmd = new SqlCommand();
            cmd.Connection = conn;
            var table = "[备忘录]";
            var strSQL = "INSERT INTO " + table + " (时间,信息) "
                         + "VALUES ('" + dt.ToString() + "','" + str + "')";
            cmd.CommandText = strSQL; //插入数据SQL语句
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteScalar();
        }
    }
}

class myOneNet
    {
        private MqttFactory mqttFactory1 = null;
        public IMqttClient mqttClient1 = null;
        string broker = "mqtts.heclouds.com";
        public string username = "01GtlXme09";

        public string password =
            "version=2018-10-31&res=products%2F01GtlXme09%2Fdevices%2FDev1&et=1750207610&method=md5&sign=ZtJOEL1rMRZMNJki6cV0SQ%3D%3D";

        public string DEVID = "Dev1";

        public string txtCustId, txtTopic, subTopic1, subTopic2;
        public string txtmsg = "";
        public string cmd = "";

        public void Init()
        {
            mqttFactory1 = new MqttFactory();
            // Environment f1 = new Environment();
            txtCustId = DEVID;
            subTopic1 = "$sys/" + username + "/" + DEVID + "/cmd/request/+";
            subTopic2 = "$sys/" + username + "/" + DEVID + "/dp/post/json/+";
            txtTopic = "$sys/" + username + "/" + DEVID + "/dp/post/json";
            Connect();
            Thread.Sleep(2000);
            if (mqttClient1.IsConnected) //若为返回主题，则处理收到下发命令
            {
                Subscribe(subTopic1); // 100毫秒后订阅主题1
            }

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
                MqttFactory mqttFactory = new MqttFactory();
                mqttClient1 = mqttFactory.CreateMqttClient();
                //连接 MQTT 服务器
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    //.WithTcpServer(txtUrl.Text, Convert.ToInt32(txtPort.Text))   // MQTT 服务器IP+端口
                    .WithTcpServer(broker, 1883)
                    .WithClientId(txtCustId) // 客户端名称 DEV1
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

                    if (topic.IndexOf("cmd/request") > 0) //若为返回主题，则处理收到下发命令
                    {
                        //if (MessageBox.Show(payload, "是否执行控制命令", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{MessageBox.Show("执行控制命令:" + payload);}
                        cmd = payload;
                    }

                    txtmsg = "收到消息。主题：{" + topic + "} , 内容： {" + payload + "}.\r\n";
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

        private async void Subscribe(string tp) //订阅主题
        {
            if (mqttClient1 == null) return;
            if (!mqttClient1.IsConnected) return;

            //订阅名主题的消息
            var mqttSubscribeOptions = mqttFactory1.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                    f =>
                    {
                        f.WithTopic(tp);
                        f.WithExactlyOnceQoS(); //即精准一次
                    })
                .Build();
            await mqttClient1.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            txtmsg = "已订阅主题：" + tp + "\r\n";

        }

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
