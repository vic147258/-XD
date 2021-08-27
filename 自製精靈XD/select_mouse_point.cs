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
    public partial class select_mouse_point : Form
    {

        public int select_X = 0, select_Y = 0;
        public bool isOk = false;

        public select_mouse_point()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(select_mouse_point_KeyDown);
        }

        public select_mouse_point(int X , int Y)
        {
            InitializeComponent();
            select_X = X;
            select_Y = Y;
            this.KeyDown += new KeyEventHandler(select_mouse_point_KeyDown);
        }

        void select_mouse_point_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                select_X = Cursor.Position.X;
                select_Y = Cursor.Position.Y;
                //label1.Text = "X: " + select_X + "\nY: " + select_Y;
                isOk = true;
                this.Close();
            }
            if (e.KeyCode == Keys.Escape)
            {
                isOk = false;
                this.Close();
            }
        }

        public int get_x()
        {
            return select_X;
        }

        public int get_y()
        {
            return select_Y;
        }

        public bool get_ans()
        {
            return isOk;
        }
    }
}
