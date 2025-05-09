namespace 多路温度监测;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        d1 = new System.Windows.Forms.Label();
        d4 = new System.Windows.Forms.Label();
        d3 = new System.Windows.Forms.Label();
        d2 = new System.Windows.Forms.Label();
        comboBox1 = new System.Windows.Forms.ComboBox();
        serial1 = new System.Windows.Forms.Label();
        serialconn = new System.Windows.Forms.Button();
        dd1 = new System.Windows.Forms.Label();
        dd2 = new System.Windows.Forms.Label();
        dd3 = new System.Windows.Forms.Label();
        dd4 = new System.Windows.Forms.Label();
        timer1 = new System.Windows.Forms.Timer(components);
        comreceive = new System.Windows.Forms.TextBox();
        KEY = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // d1
        // 
        d1.Font = new System.Drawing.Font("OPPO Sans 4.0", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        d1.Location = new System.Drawing.Point(107, 160);
        d1.Name = "d1";
        d1.RightToLeft = System.Windows.Forms.RightToLeft.No;
        d1.Size = new System.Drawing.Size(123, 53);
        d1.TabIndex = 0;
        d1.Text = "设备1";
        // 
        // d4
        // 
        d4.AutoSize = true;
        d4.Font = new System.Drawing.Font("OPPO Sans 4.0", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        d4.Location = new System.Drawing.Point(107, 377);
        d4.Name = "d4";
        d4.RightToLeft = System.Windows.Forms.RightToLeft.No;
        d4.Size = new System.Drawing.Size(126, 53);
        d4.TabIndex = 1;
        d4.Text = "设备4";
        // 
        // d3
        // 
        d3.AutoSize = true;
        d3.Font = new System.Drawing.Font("OPPO Sans 4.0", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        d3.Location = new System.Drawing.Point(107, 305);
        d3.Name = "d3";
        d3.RightToLeft = System.Windows.Forms.RightToLeft.No;
        d3.Size = new System.Drawing.Size(123, 53);
        d3.TabIndex = 2;
        d3.Text = "设备3";
        // 
        // d2
        // 
        d2.AutoSize = true;
        d2.Font = new System.Drawing.Font("OPPO Sans 4.0", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        d2.Location = new System.Drawing.Point(107, 232);
        d2.Name = "d2";
        d2.RightToLeft = System.Windows.Forms.RightToLeft.No;
        d2.Size = new System.Drawing.Size(123, 53);
        d2.TabIndex = 3;
        d2.Text = "设备2";
        // 
        // comboBox1
        // 
        comboBox1.FormattingEnabled = true;
        comboBox1.Location = new System.Drawing.Point(177, 67);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new System.Drawing.Size(121, 28);
        comboBox1.TabIndex = 4;
        comboBox1.DropDown += comboBox1_DropDown;
        // 
        // serial1
        // 
        serial1.Font = new System.Drawing.Font("OPPO Sans", 11.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        serial1.Location = new System.Drawing.Point(71, 67);
        serial1.Name = "serial1";
        serial1.Size = new System.Drawing.Size(100, 23);
        serial1.TabIndex = 5;
        serial1.Text = "串口选择";
        // 
        // serialconn
        // 
        serialconn.AutoSize = true;
        serialconn.Font = new System.Drawing.Font("OPPO Sans", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        serialconn.Location = new System.Drawing.Point(328, 67);
        serialconn.Name = "serialconn";
        serialconn.Size = new System.Drawing.Size(72, 33);
        serialconn.TabIndex = 6;
        serialconn.Text = "连接";
        serialconn.UseVisualStyleBackColor = true;
        serialconn.Click += serialconn_Click;
        // 
        // dd1
        // 
        dd1.Font = new System.Drawing.Font("OPPO Sans", 23.999998F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        dd1.Location = new System.Drawing.Point(245, 160);
        dd1.Name = "dd1";
        dd1.Size = new System.Drawing.Size(177, 53);
        dd1.TabIndex = 7;
        dd1.Text = "####";
        // 
        // dd2
        // 
        dd2.Font = new System.Drawing.Font("OPPO Sans", 23.999998F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        dd2.Location = new System.Drawing.Point(245, 232);
        dd2.Name = "dd2";
        dd2.Size = new System.Drawing.Size(177, 53);
        dd2.TabIndex = 8;
        dd2.Text = "####";
        // 
        // dd3
        // 
        dd3.Font = new System.Drawing.Font("OPPO Sans", 23.999998F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        dd3.Location = new System.Drawing.Point(245, 305);
        dd3.Name = "dd3";
        dd3.Size = new System.Drawing.Size(177, 53);
        dd3.TabIndex = 9;
        dd3.Text = "####";
        // 
        // dd4
        // 
        dd4.Font = new System.Drawing.Font("OPPO Sans", 23.999998F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        dd4.Location = new System.Drawing.Point(245, 377);
        dd4.Name = "dd4";
        dd4.Size = new System.Drawing.Size(177, 53);
        dd4.TabIndex = 10;
        dd4.Text = "####";
        // 
        // timer1
        // 
        timer1.Enabled = true;
        timer1.Tick += timer1_Tick;
        // 
        // comreceive
        // 
        comreceive.Location = new System.Drawing.Point(132, 484);
        comreceive.Multiline = true;
        comreceive.Name = "comreceive";
        comreceive.Size = new System.Drawing.Size(544, 116);
        comreceive.TabIndex = 11;
        // 
        // KEY
        // 
        KEY.Font = new System.Drawing.Font("OPPO Sans", 23.999998F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
        KEY.Location = new System.Drawing.Point(545, 242);
        KEY.Name = "KEY";
        KEY.Size = new System.Drawing.Size(177, 53);
        KEY.TabIndex = 13;
        KEY.Text = "ON/OFF";
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(835, 647);
        Controls.Add(KEY);
        Controls.Add(comreceive);
        Controls.Add(dd4);
        Controls.Add(dd3);
        Controls.Add(dd2);
        Controls.Add(dd1);
        Controls.Add(serialconn);
        Controls.Add(serial1);
        Controls.Add(comboBox1);
        Controls.Add(d2);
        Controls.Add(d3);
        Controls.Add(d4);
        Controls.Add(d1);
        Text = "Form1";
        Load += Form1_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label KEY;

    private System.Windows.Forms.TextBox comreceive;

    private System.Windows.Forms.Label dd2;
    private System.Windows.Forms.Label dd3;
    private System.Windows.Forms.Label dd4;
    private System.Windows.Forms.Timer timer1;

    private System.Windows.Forms.Label dd1;

    private System.Windows.Forms.Button serialconn;

    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label serial1;

    private System.Windows.Forms.Label d4;
    private System.Windows.Forms.Label d3;
    private System.Windows.Forms.Label d2;

    private System.Windows.Forms.Label d1;

    #endregion
}