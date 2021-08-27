using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

namespace 自製精靈XD
{
    public partial class Form1 : Form
    {
        String Process_list;
        String br = Convert.ToString(13) + Convert.ToString(10);
        bool is_callparameter = false;
        bool is_break = false;
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        //起始物件設定
        String this_ppid = "autokeyyy";
        tools tt = new tools();


        public Form1()
        {
            InitializeComponent();
            page_loadddd();


            //臨時區
            //bt_add_mouse_rc.Enabled = false;

        }

        public Form1(String[] aa)
        {
            InitializeComponent();
            page_loadddd();

            
            is_callparameter = true;

            read_to_list(aa[0]);

            ThreadPool.QueueUserWorkItem(new WaitCallback(main_run));

            //System.Threading.Thread.Sleep(3000);

            this.WindowState = FormWindowState.Minimized;


        }

        #region 共用的起始設定

        void page_loadddd()
        {
            #region 各種按鈕的事件設定

            //原生區
            bt_execution.Click += new EventHandler(bt_execution_Click);
            bt_exit.Click += new EventHandler(bt_exit_Click);
            bt_mouse_test.Click += new EventHandler(bt_mouse_test_Click);
            bt_load.Click += new EventHandler(bt_load_Click);
            bt_save.Click += new EventHandler(bt_save_Click);

            //新增區
            bt_add_mouse_move.Click += new EventHandler(bt_add_mouse_move_Click);
            bt_add_mouse_lc.Click += new EventHandler(bt_add_mouse_lc_Click);
            bt_add_mouse_rc.Click += new EventHandler(bt_add_mouse_rc_Click);
            bt_add_key.Click += new EventHandler(bt_add_key_Click);
            bt_add_time.Click += new EventHandler(bt_add_time_Click);

            //編輯區
            bt_edit_up.Click += new EventHandler(bt_edit_up_Click);
            bt_edit_down.Click += new EventHandler(bt_edit_down_Click);
            bt_delete.Click += new EventHandler(bt_delete_Click);

            //其他
            listBox1.KeyDown += new KeyEventHandler(listBox1_KeyDown);
            listBox1.DoubleClick += new EventHandler(listBox1_DoubleClick);

            this.SizeChanged += new EventHandler(Form1_SizeChanged);

            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(main_form_DragEnter);
            this.DragDrop += new DragEventHandler(main_form_DragDrop);


            //偵測全域按鈕
            WindwosHook.KeyboardHook.Enabled = true;

            WindwosHook.KeyboardHook.GlobalKeyDown += new EventHandler<WindwosHook.KeyboardHook.KeyEventArgs>(KeyboardHook_GlobalKeyDown);

            #endregion

            #region 登錄檔

            text_count.Text = tt.get_reg(this_ppid, "repeat");
            text_reciprocal.Text = tt.get_reg(this_ppid, "reciprocal");
            checkBox_Follow.Checked = tt.get_reg(this_ppid, "follow") == "1" ? true : false;
            checkBox_app.Checked = tt.get_reg(this_ppid, "check_app") == "1" ? true : false;

            if (text_count.Text == "") text_count.Text = "1";
            if (text_reciprocal.Text == "") text_reciprocal.Text = "3";


            #endregion

        }

        #endregion


        #region 讀寫檔案

        void main_form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        void main_form_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            String aaasd = "";
            for (i = 0; i < s.Length; i++)
                aaasd += s[i] + "****"; //全部檔案
            //MessageBox.Show(aaasd);
            if (s[0].Split('.')[s[0].Split('.').Length - 1] == "txt")
                read_to_list(s[0]);
            else
                MessageBox.Show("請丟入txr檔案");
        }

        //讀取
        void bt_load_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "文字檔|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                read_to_list(openFileDialog1.FileName);
            }
        }

        //寫入
        void bt_save_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "腳本";
            saveFileDialog1.Filter = "文字檔|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName);
                for (int i = 0; i < listBox1.Items.Count;i++ )
                    sw.WriteLine(listBox1.Items[i].ToString());
                sw.Close();
                MessageBox.Show("存檔完成");
            }
        }

        //寫進列表中
        void read_to_list(String file_path)
        {
            string line;
            int counter = 0;
            System.IO.StreamReader file = new System.IO.StreamReader(file_path);

            DialogResult anss = DialogResult.Yes;

            if (listBox1.Items.Count != 0)
                anss = MessageBox.Show("是否清除原有資料?", "警告", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (anss == DialogResult.Yes)
                listBox1.Items.Clear();

            if (anss != DialogResult.Cancel)
            {
                while ((line = file.ReadLine()) != null)
                {
                    listBox1.Items.Add(line);
                    counter++;
                }
                file.Close();
            }
        }

        #endregion

        #region 編輯排程的那些按鈕

        // 上移項目
        void bt_edit_up_Click(object sender, EventArgs e)
        {
            int temp_index = 0;
            if (listBox1.SelectedIndex >= 0)
            {
                if (listBox1.SelectedIndex > 0)
                {
                    temp_index = listBox1.SelectedIndex;
                    listBox1.Items.Insert(listBox1.SelectedIndex - 1, listBox1.Items[listBox1.SelectedIndex]);
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    listBox1.SelectedIndex = temp_index - 1;
                }
            }
        }

        // 下移項目
        void bt_edit_down_Click(object sender, EventArgs e)
        {
            int temp_index = 0;
            if (listBox1.SelectedIndex >= 0)
            {
                if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                {
                    temp_index = listBox1.SelectedIndex;
                    listBox1.Items.Insert(listBox1.SelectedIndex + 2, listBox1.Items[listBox1.SelectedIndex]);
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    listBox1.SelectedIndex = temp_index + 1;
                }
            }
        }

        // 刪除選取的項目
        void bt_delete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        // 列表 - 鍵盤控制_KeyDown
        void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //刪除
            if (e.KeyCode == Keys.Delete)
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
        }

        // 列表 - 滑鼠點兩下事件
        void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {

                string theAction = listBox1.Items[listBox1.SelectedIndex].ToString();
                //滑鼠動作
                if (theAction.IndexOf(',') != -1)
                {
                    select_mouse_point aaa = new select_mouse_point(0, 0);
                    aaa.ShowDialog();
                    if (aaa.get_ans())
                    {
                        int temp_a = listBox1.SelectedIndex;
                        listBox1.Items.Insert(listBox1.SelectedIndex, aaa.get_x().ToString() + "," + aaa.get_y().ToString());
                        listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                        listBox1.SelectedIndex = temp_a;
                    }
                }

                //鍵盤
                if (theAction.IndexOf("鍵盤") != -1)
                {
                    select_key aaa = new select_key();
                    aaa.ShowDialog();
                    if (aaa.get_ans())
                    {
                        if (listBox1.SelectedIndex >= 0)
                        {
                            int temp_a = listBox1.SelectedIndex;
                            listBox1.Items.Insert(listBox1.SelectedIndex, "鍵盤:" + aaa.get_key().ToString());
                            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                            listBox1.SelectedIndex = temp_a;
                        }
                        else
                            listBox1.Items.Add("鍵盤:" + aaa.get_key().ToString());
                    }
                }

                //等待時間
                if (theAction.IndexOf("等待") != -1)
                {
                    select_time aaa = new select_time(int.Parse(theAction.Replace("等待:", "")));
                    aaa.ShowDialog();
                    if (aaa.get_ans())
                    {
                        if (listBox1.SelectedIndex >= 0)
                        {
                            int temp_a = listBox1.SelectedIndex;
                            listBox1.Items.Insert(listBox1.SelectedIndex + 1, "等待:" + aaa.get_time().ToString());
                            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                            listBox1.SelectedIndex = temp_a;
                        }
                        else
                            listBox1.Items.Add("等待:" + aaa.get_time().ToString());
                    }
                }

            }
        }

        #endregion

        #region 新增排程的那些按鈕區

        /// <summary>
        /// 新增滑鼠做標
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_add_mouse_move_Click(object sender, EventArgs e)
        {
            select_mouse_point aaa = new select_mouse_point(0, 0);
            aaa.ShowDialog();
            if (aaa.get_ans())
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    listBox1.Items.Insert(listBox1.SelectedIndex + 1, aaa.get_x().ToString() + "," + aaa.get_y().ToString());
                    listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                }
                else
                    listBox1.Items.Add(aaa.get_x().ToString() + "," + aaa.get_y().ToString());
            }
        }

        /// <summary>
        /// 滑鼠左鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_add_mouse_lc_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.Insert(listBox1.SelectedIndex + 1, "Left_Click");
                listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
            }
            else
                listBox1.Items.Add("Left_Click");
        }

        /// <summary>
        /// 滑鼠右鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_add_mouse_rc_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.Insert(listBox1.SelectedIndex + 1, "Right_Click");
                listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
            }
            else
                listBox1.Items.Add("Right_Click");
        }


        /// <summary>
        /// 鍵盤事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_add_key_Click(object sender, EventArgs e)
        {
            select_key aaa = new select_key();
            aaa.ShowDialog();
            if (aaa.get_ans())
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    listBox1.Items.Insert(listBox1.SelectedIndex + 1, "鍵盤:" + aaa.get_key().ToString());
                    listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                }
                else
                    listBox1.Items.Add("鍵盤:" + aaa.get_key().ToString());
            }
        }

        /// <summary>
        /// 新增時間
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_add_time_Click(object sender, EventArgs e)
        {
            select_time aaa = new select_time();
            aaa.ShowDialog();
            if (aaa.get_ans())
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    listBox1.Items.Insert(listBox1.SelectedIndex + 1, "等待:" + aaa.get_time().ToString());
                    listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                }
                else
                    listBox1.Items.Add("等待:" + aaa.get_time().ToString());
            }
        }





        #endregion

        #region 原生區(測試、執行、結束)

        /// <summary>
        /// 測試滑鼠
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_mouse_test_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string[] point_a = listBox1.Items[listBox1.SelectedIndex].ToString().Split(',');
                My_Mouse.moveto(int.Parse(point_a[0]), int.Parse(point_a[1]));
            }
        }

        /// <summary>
        /// 結束應用程式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 執行按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt_execution_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(main_run));
        }

        #endregion


        #region 程式主體+副程式

        /// <summary>
        /// 主程式
        /// </summary>
        /// <param name="arg"></param>
        private void main_run(object arg)
        {
            is_break = false;
            int the_count = int.Parse(text_count.Text);

            //設定登入檔
            tt.set_reg(this_ppid, "repeat", text_count.Text);
            tt.set_reg(this_ppid, "reciprocal", text_reciprocal.Text);
            tt.set_reg(this_ppid, "follow", checkBox_Follow.Checked ? "1" : "0");
            tt.set_reg(this_ppid, "check_app", checkBox_app.Checked ? "1" : "0");


            Main_form lv = new Main_form(Update_view);
            Main_form ls = new Main_form(Update_state);
            Main_list ll = new Main_list(Update_listindex);
            Main_formb bt = new Main_formb(button_view);

            try { Double testint = double.Parse(textBox_speedrate.Text); }
            catch { textBox_speedrate.Text = "1"; }


            for (int i = int.Parse(text_reciprocal.Text); i > 0; i--)
            {
                Invoke(ls, i.ToString() + "秒後開始");
                System.Threading.Thread.Sleep(1000);
            }

            for (int i = 1; i <= the_count; i++)
            {
                if (checkBox_count.Checked) //無限判斷
                    i = 0;

                for (int j = 0; j < listBox1.Items.Count; j++)
                {
                    if (is_okrun()) //是否強制中斷
                    {
                        if (checkBox_Follow.Checked) //是否讓左邊跟著跑
                            Invoke(ll, j);
                        Invoke(lv, i.ToString().PadLeft(text_count.Text.Length, '0') + "/" + the_count.ToString());
                        Check_Action(listBox1.Items[j].ToString());
                        try
                        {
                            Invoke(bt, false);
                        }
                        catch
                        {
                            Application.Exit();
                        }
                    }
                    else
                    {
                        Invoke(bt, true);
                        Invoke(lv, i.ToString().PadLeft(text_count.Text.Length, '0') + "/" + the_count.ToString() + " (已中止)");
                        i = int.MaxValue - 3;  //強制離開
                        break;
                    }
                }
            }

            Invoke(bt, true);
            Invoke(ls, "結束");

            if (is_callparameter)
                Application.Exit();
        }

        /// <summary>
        /// 驗證動作
        /// </summary>
        void Check_Action(string theAction)
        {
            //滑鼠動作
            if (theAction.IndexOf(',') != -1)
            {
                string[] point_a = theAction.Split(',');
                My_Mouse.moveto(int.Parse(point_a[0]), int.Parse(point_a[1]));
            }

            //鍵盤
            if (theAction.IndexOf("鍵盤") != -1)
            {
                try
                {
                    SendKeys.SendWait(change_key_code(theAction));
                    return;
                }
                catch (Exception e)
                {
                    MessageBox.Show("按鍵：" + theAction.Replace("鍵盤:", "") + " 無法作為排程\n" + e.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //等待時間
            if (theAction.IndexOf("等待") != -1)
            {
                System.Threading.Thread.Sleep((int)(int.Parse(theAction.Replace("等待:", "").Replace("\t", "")) * double.Parse(textBox_speedrate.Text)));
            }

            //左鍵
            if (theAction.IndexOf("Left_Click") != -1)
            {
                My_Mouse.LeftClick(Cursor.Position.X, Cursor.Position.Y);
            }

            //右鍵
            if (theAction.IndexOf("Right_Click") != -1)
            {
                My_Mouse.RightClick(Cursor.Position.X, Cursor.Position.Y);
            }

        }



        #endregion







        #region 其他副程式 (偵測指定程式、強制停止(還需要修改)、全域偵測按鍵)



        Boolean check_gameison()
        {
            //MessageBox.Show(System.Diagnostics.Process.GetProcesses().Length.ToString());
            Process_list = "";

            foreach (Process p in Process.GetProcesses())
            {
                //if (p.ProcessName.IndexOf("C") >= 0)
                Process_list += p.ProcessName + " ";
            }

            //MessageBox.Show(Process_list);
            if (Process_list.IndexOf(textBox_appname.Text) != -1)   //
            {
                return true;
            }
            else
            {
                MessageBox.Show("程式未執行");
                return false;
            }
        }


        Boolean is_okrun()
        {
            //if (Cursor.Position.X >= 10)
            if (!is_break)
            {
                if (checkBox_app.Checked)
                    return check_gameison();
                else
                    return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 全域按鈕偵測
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyboardHook_GlobalKeyDown(object sender, WindwosHook.KeyboardHook.KeyEventArgs e)
        {
            if (e.Keys == Keys.Capital || e.Keys == Keys.F12)
            {
                switch (e.Keys)
                {
                    case Keys.Capital:
                        is_break = true;
                        break;
                    case Keys.F12:
                        is_break = true;
                        break;
                }
            }
            else
            {
                Console.WriteLine(e.Keys.ToString());
                Console.WriteLine(e.VirtualKeyCode.ToString());
            }

        }



        /// <summary>
        /// 把文字轉換成可以認識的key code
        /// </summary>
        /// <returns></returns>
        String change_key_code(String tempaadd)
        {
            String tempsss = tempaadd.Replace("鍵盤:", "").Replace("\t", "");


            if (tempsss.IndexOf("Space") != -1)
                return " ";

            if (tempsss.IndexOf("Return") != -1)
                return "{Enter}";

            if (tempsss.IndexOf("Shift") != -1)
                return "+";

            if (tempsss.IndexOf("Control") != -1)
                return "^";

            if (tempsss.IndexOf("Menu") != -1)
                return "%";

            if (tempsss.IndexOf("Back") != -1)
                return "{BKSP}";

            //if (tempsss.IndexOf("Capital") != -1)
            //    return "{CAPSLOCK}";  //反正按了也是停止 不如讓他出錯

            if (tempsss.IndexOf("Insert") != -1)
                return "{INS}";

            if (tempsss.IndexOf("Delete") != -1)
                return "{DEL}";

            if (tempsss.IndexOf("Home") != -1)
                return "{HOME}";

            if (tempsss.IndexOf("End") != -1)
                return "{END}";

            if (tempsss.IndexOf("PageUp") != -1)
                return "{PGUP}";

            if (tempsss.IndexOf("Next") != -1)
                return "{PGDN}";

            if (tempsss.IndexOf("F") != -1 && tempsss.Length > 1)
                return "{" + tempsss + "}";


            if (tempsss.IndexOf("Add") != -1)
                return "{ADD}";

            if (tempsss.IndexOf("Subtract") != -1)
                return "{SUBTRACT}";

            if (tempsss.IndexOf("Multiply") != -1)
                return "{MULTIPLY}";

            if (tempsss.IndexOf("Divide") != -1)
                return "{DIVIDE}";


            if (tempsss.IndexOf("Scroll") != -1)
                return "{SCROLLLOCK}";

            if (tempsss.IndexOf("Oemtilde") != -1)
                return "`";

            if (tempsss.IndexOf("Tab") != -1)
                return "{TAB}";




            if (tempsss.IndexOf("Up") != -1)
                return "{UP}";

            if (tempsss.IndexOf("Down") != -1)
                return "{DOWN}";

            if (tempsss.IndexOf("Left") != -1)
                return "{LEFT}";

            if (tempsss.IndexOf("Right") != -1)
                return "{RIGHT}";




            if (tempsss.Length > 1)  //上方數字健
            {
                return "{" + tempsss.Replace("D", "") + "}";
            }

            // //剩下的
            return "" + tempsss.ToLower() + "";
        }



        void Form1_SizeChanged(object sender, EventArgs e)
        {
            listBox1.Height = this.Size.Height-60;
        }



        delegate void Main_form(String sss);
        private void Update_state(String sss)
        {
            label_state.Text = sss;
            label_state.Update();
        }
        private void Update_view(String sss)
        {
            label_view.Text = sss;
            label_view.Update();
        }

        delegate void Main_list(int iii);
        private void Update_listindex(int iii)
        {
            listBox1.SelectedIndex = iii;
            listBox1.Update();
        }

        delegate void Main_formb(Boolean sss);
        private void button_view(Boolean sss)
        {
            bt_edit_up.Enabled = sss;
            bt_edit_down.Enabled = sss;
            bt_delete.Enabled = sss;
            bt_mouse_test.Enabled = sss;

            bt_add_mouse_move.Enabled = sss;
            bt_add_time.Enabled = sss;
            bt_add_key.Enabled = sss;
            bt_add_mouse_lc.Enabled = sss;
            bt_add_mouse_rc.Enabled = sss;

            bt_save.Enabled = sss;
            bt_load.Enabled = sss;

            checkBox_app.Enabled = sss;
            textBox_appname.Enabled = sss;
            checkBox_count.Enabled = sss;
            checkBox_Follow.Enabled = sss;
            text_count.Enabled = sss;
            text_reciprocal.Enabled = sss;
            textBox_speedrate.Enabled = sss;

            bt_execution.Enabled = sss;

        }





        #endregion

    }
}
