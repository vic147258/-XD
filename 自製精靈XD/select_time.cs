using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自製精靈XD
{
    public partial class select_time : Form
    {

        public int thetime = 0;
        public bool isOk = false;

        public select_time()
        {
            InitializeComponent();
            button1.Click += new EventHandler(button1_Click);
            button2.Click += new EventHandler(button2_Click);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
        }

        public select_time(int old_tome)
        {
            InitializeComponent();
            button1.Click += new EventHandler(button1_Click);
            button2.Click += new EventHandler(button2_Click);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
            textBox1.Text = old_tome.ToString();
        }

        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    thetime = int.Parse(textBox1.Text);
                }
                catch
                {
                    MessageBox.Show("輸入不正確");
                    return;
                }
                isOk = true;
                this.Close();
            }
        }

        void button2_Click(object sender, EventArgs e)
        {
            isOk = false;
            this.Close();
        }

        void button1_Click(object sender, EventArgs e)
        {
            try
            {
                thetime = int.Parse(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("輸入不正確");
                return;
            }
            isOk = true;
            this.Close();
        }

        public int get_time()
        {
            return thetime;
        }

        public bool get_ans()
        {
            return isOk;
        }
    }
}
