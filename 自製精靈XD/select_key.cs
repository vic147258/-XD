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
    public partial class select_key : Form
    {

        public Keys theKey;
        public int theKeyvalue;
        public bool isOk = false;

        public select_key()
        {
            InitializeComponent();
            label1.Click += new EventHandler(label1_Click);
            this.Click += new EventHandler(label1_Click);
            this.KeyDown += new KeyEventHandler(select_key_KeyDown);
        }

        void select_key_KeyDown(object sender, KeyEventArgs e)
        {
            theKey = e.KeyCode;
            theKeyvalue = e.KeyValue;
            isOk = true;
            this.Close();
        }

        void label1_Click(object sender, EventArgs e)
        {
            isOk = false;
            this.Close();
        }





        public Keys get_key()
        {
            return theKey;
        }

        public int get_keyvalue()
        {
            return theKeyvalue;
        }


        public bool get_ans()
        {
            return isOk;
        }


    }
}
