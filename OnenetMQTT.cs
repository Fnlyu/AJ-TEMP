using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mqtt_demo
{
    public partial class OnenetMQTT : Form
    {
        private MqttFactory mqttFactory1 = null;
        private IMqttClient mqttClient1 = null;
        string broker = "mqtts.heclouds.com";
        public string username = "01GtlXme09";
        //string password = "version=2018-10-31&res=products%2FNG88K4a92D%2Fdevices%2FDEV1&et=2110970010&method=md5&sign=wvaJ5jyVR0EqLzG6GNe2fQ%3D%3D";
        public string password = "version=2018-10-31&res=products%2F01GtlXme09%2Fdevices%2FDev1&et=1750207610&method=md5&sign=ZtJOEL1rMRZMNJki6cV0SQ%3D%3D";
        public string DEVID = "Dev1";


        public void mqttInit()
        {
            txtCustId.Text = DEVID;
            subTopic1.Text = "$sys/" + username + "/" + DEVID + "/cmd/request/+";
            subTopic2.Text = "$sys/" + username + "/" + DEVID + "/dp/post/json/+";
            txtTopic.Text = "$sys/" + username + "/" + DEVID + "/dp/post/json";
        }

        public OnenetMQTT()
        {
            InitializeComponent();
        }

        public string OneJson(int id,string sensor,string val)
        {
            string t;
            t = "{\"id\":" + id + ",\"dp\": {\"" + sensor + "\": [{\"v\":" + val + "}]}}";
            return t;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btnConnect.Enabled = true;
            btnSend.Enabled = false;
            btnClose.Enabled = false;
            btn上传数据.Enabled = false;
            cbSensor.SelectedIndex = 0;
            mqttFactory1 = new MqttFactory();
            Environment f1 = new Environment();
            f1.Show();

        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                MqttFactory mqttFactory = new MqttFactory();
                mqttClient1 = mqttFactory.CreateMqttClient();
                //连接 MQTT 服务器
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    //.WithTcpServer(txtUrl.Text, Convert.ToInt32(txtPort.Text))   // MQTT 服务器IP+端口
                    .WithTcpServer(broker,1883)
                    .WithClientId(txtCustId.Text)         // 客户端名称 DEV1
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

                    txtmsg.Invoke(new Action(() =>
                    {
                        if (topic.IndexOf("cmd/request") > 0)    //若为返回主题，则处理收到下发命令
                        {
                            if (MessageBox.Show(payload, "是否执行控制命令", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                MessageBox.Show("执行控制命令:" + payload);
                            }
                        }
                        txtmsg.Text = "收到消息。主题：{" + topic + "} , 内容： {" + payload + "}.\r\n" + txtmsg.Text;
                    }));
                    return Task.CompletedTask;
                };
                var response = await mqttClient1.ConnectAsync(mqttClientOptions, CancellationToken.None);
                txtmsg.Invoke(new Action(() =>
                {
                    txtmsg.Text = "MQTT客户端已连接. IsConnected: [{mqttClient1.IsConnected}] resultCode:{response.ResultCode}\r\n" + txtmsg.Text;
                    btnConnect.Enabled = false;
                    btnSend.Enabled = true;
                    btnClose.Enabled = true;
                    btn上传数据.Enabled = true;
                    btnSend.Focus();
                }));
            }
            catch {
                txtmsg.Invoke(new Action(() =>
                {
                    txtmsg.Text = "连接失败.\r\n" + txtmsg.Text;
                    btnConnect.Enabled = true;
                    btnSend.Enabled = false;
                    btnClose.Enabled = false;
                }));
            }
        }

        private async void txtSend_Click(object sender, EventArgs e)
        {
            // 发布消息
            // 在主题上发布消息
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(txtTopic.Text)
                .WithPayload(txtSend.Text)
                .Build();

            //异步发布消息
            await mqttClient1.PublishAsync(applicationMessage, CancellationToken.None);
            txtmsg.Invoke(new Action(() =>
            {
                txtmsg.Text = "消息已发送\r\n"+ txtmsg.Text;
            }));
        }

        private async void btnClose_Click(object sender, EventArgs e)
        {
            //断开连接
            await mqttClient1.DisconnectAsync();
            txtmsg.Invoke(new Action(() =>
            {
                txtmsg.Text = "连接已断开\r\n" + txtmsg.Text;
                btnConnect.Enabled = true;
                btnSend.Enabled = false;
                btnClose.Enabled = false;
                btnConnect.Focus();
            }));
        }

        private async void btnSubscribe_Click(object sender, EventArgs e)
        {
            if (mqttClient1 == null) return;
            if (!mqttClient1.IsConnected) return;
            //订阅名主题的消息
            string tp="";
            if (sender == btnSubscribe2)
                tp = subTopic2.Text;
            else
                tp = subTopic1.Text;

            var mqttSubscribeOptions = mqttFactory1.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic(tp);
                    f.WithExactlyOnceQoS();     //即精准一次
                })
            .Build();
            await mqttClient1.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            txtmsg.Invoke(new Action(() =>
            {
                txtmsg.Text = "已订阅主题：" + tp + "\r\n" + txtmsg.Text;
            }));
        }

        public async void btn上传数据_Click(object sender, EventArgs e)
        {
            // 发布消息
            // 在主题上发布消息
            if(cbSensor.Text =="") return;

            string t = OneJson(1, cbSensor.Text, txtVal.Text);
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(txtTopic.Text)
                .WithPayload(t)
                .Build();

            //异步发布消息
            await mqttClient1.PublishAsync(applicationMessage, CancellationToken.None);
            txtmsg.Invoke(new Action(() =>
            {
                txtmsg.Text = "消息已发送\r\n" + t;
            }));
        }
    }
}
