using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 多路温度监测;

public class db
{
    private static SqlConnection conn;

    public static void Open_db()
    {
        try
        {
            var ConStr = "server=.\\SQLEXPRESS;database=Temperature;uid=sa;pwd=`sql2022";
            //string ConStr ="Data Source=.\\SQLEXPRESS; Initial Catalog=MonitorDB; User ID=sa; Password=`sql2016; Pooling=true" 

            conn = new SqlConnection(ConStr);
            conn.Open();
        }
        catch
        {
            MessageBox.Show("连接数据库失败！");
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
