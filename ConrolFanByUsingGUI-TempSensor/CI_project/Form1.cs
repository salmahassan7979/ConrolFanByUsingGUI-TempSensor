using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
namespace CI_project
{
    public partial class Form1 : Form
    {
        SerialPort port = null;
        String data_rx = " ";
        int data_tx = 50;
        int num;
        bool flag_rx = false;
        public Form1()
        {
            InitializeComponent();
            refresh_com();
            label1.Text = "Disconnected";
            label1.ForeColor = Color.Red;
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            refresh_com();
        }
        private void refresh_com() {
            comboBox1.DataSource = SerialPort.GetPortNames();
        }
        private void connect() {
            port = new SerialPort(comboBox1.SelectedItem.ToString());
            port.DataReceived += new SerialDataReceivedEventHandler(data_rx_handler);
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            try
            {
                if (!port.IsOpen) {
                    port.Open();
                    label1.Text = "Connected";
                    label1.ForeColor = Color.Green;

                }
            }
            catch (Exception e) {
            }
        }
        private void disconnect() {
            try
            {
                if (!port.IsOpen)
                {
                    port.Close();
                    label1.Text = "Disconnected";
                    label1.ForeColor = Color.Red;

                }
            }
            catch (Exception e)
            {
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            disconnect();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            disconnect();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            send();
        }
        private void send() {
            try
            {
                port.Write("@"+textBox1.Text+";");
                data_tx = Int32.Parse(textBox1.Text);
                    
            }
            catch (Exception e) {

            }
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                send();
            }
        }
        private void data_rx_handler(object sender, SerialDataReceivedEventArgs e) {
            SerialPort port1 = (SerialPort)sender;
            String temp = port1.ReadExisting();
            int idx_end = temp.IndexOf(';');
            if ((idx_end >= 0) && flag_rx) {
                flag_rx = false;
                try
                {
                    data_rx += temp.Substring(0, idx_end);
                     num = Int32.Parse(data_rx);
                    if (num > 150 || num < 0)
                    {
                        data_rx = "Temperature out of range";
                    }
                }
                catch (Exception m)
                {
                      data_rx = m.Message;
                }
            }
            if (flag_rx)
            {
                data_rx += temp;
            }
            int idx_start = temp.IndexOf('@');
            if (idx_start>=0) {
                
                flag_rx = true;
                data_rx = "";
                if (temp.Length > (idx_start + 1)) {
                    data_rx += temp.Substring((idx_start + 1), (temp.Length - 1));
                    int idx = data_rx.IndexOf(';');
                    if (idx >= 0) {
                        data_rx = data_rx.Substring(0, idx);
                            }
                }
            }
           
            
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = data_rx;
            
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            textBox2.Text = data_rx;
            if (num > data_tx)
            {
                label2.Text = "Fan ON";
                label2.ForeColor = Color.Green;
            }
            else {
                label2.Text = "Fan OFF";
                label2.ForeColor = Color.Red;
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }
    }
}
