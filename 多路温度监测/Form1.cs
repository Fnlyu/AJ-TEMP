namespace 多路温度监测;

using System;
using System.IO.Ports;

public partial class Form1 : Form
{
    myOneNet _mqtt1 = new myOneNet();
    private SerialPort _port1; // 将 SerialPort 定义为类的字段
    private bool _isOpen = false; // 串口状态

    public Form1()
    {
        InitializeComponent();
    }

    private void comboBox1_DropDown(object sender, EventArgs e)
    {
        comboBox1.Items.Clear();
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {
            comboBox1.Items.Add(port);
        }
    }

    private void serialconn_Click(object sender, EventArgs e)
    {
        if (!_isOpen)
        {
            // 打开串口
            if (comboBox1.SelectedItem != null)
            {
                try
                {
                    _port1 = new SerialPort(comboBox1.SelectedItem.ToString(), 9600, Parity.None, 8, StopBits.One);
                    _port1.Open();
                    _isOpen = true;
                    serialconn.Text = "断开";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接失败：" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("请先选择串口");
            }
        }
        else
        {
            // 关闭串口
            try
            {
                if (_port1.IsOpen)
                {
                    _port1.Close();
                }
                _isOpen = false;
                serialconn.Text = "连接";
            }
            catch (Exception ex)
            {
                MessageBox.Show("断开失败：" + ex.Message);
            }
        }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        if (_isOpen)
        {
            try
            {
                // 读取数据
                string data = _port1.ReadExisting();
                string key = "";
                comreceive.Text = data;
                // 处理数据
                string[] values = data.Split(';');
                if (values.Length >= 4)
                {
                    dd1.Text = values[0]+ "°C";
                    _mqtt1.上传数据("Temperature1", values[0]);
                    dd2.Text = values[1]+ "°C";
                    _mqtt1.上传数据("Temperature2", values[1]);
                    dd3.Text = values[2]+ "°C";
                    _mqtt1.上传数据("Temperature3", values[2]);
                    if (values[3] == "1")
                    {
                        KEY.Text = "开";
                        key = "ON";
                        _mqtt1.上传数据("KEY", "1");
                    }
                    else if(values[3] == "0")
                    {
                        KEY.Text = "关";
                        key = "OFF";
                        _mqtt1.上传数据("KEY", "0");
                    }
                    
                    db.Insert_data(values[0]+ "°C", values[1]+ "°C", values[2]+ "°C", key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败：" + ex.Message);
            }
        }
    }


    private void Form1_Load(object sender, EventArgs e)
    {
        _mqtt1.Init();
        
    }
}