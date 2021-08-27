using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace 自製精靈XD
{
    public partial class tools
    {
        /// <summary>
        /// 取得註冊檔的值
        /// </summary>
        /// <param name="dir">機碼</param>
        /// <param name="reg_name">字串值名稱</param>
        /// <returns></returns>
        public String get_reg(String dir, String reg_name)
        {
            return System.Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\Software\\" + dir, reg_name, ""));
        }

        /// <summary>
        /// 設定註冊檔的值
        /// </summary>
        /// <param name="dir">機碼</param>
        /// <param name="reg_name">字串值名稱</param>
        /// <param name="the_value">值</param>
        public void set_reg(String dir, String reg_name, String the_value)
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\" + dir, reg_name, the_value, RegistryValueKind.String);
        }


        /// <summary>
        /// 取得MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public String getMD5(String input)
        {
            MD5 md5 = MD5.Create();//建立一個MD5

            byte[] source = Encoding.Default.GetBytes(input);//將字串轉為Byte[]

            byte[] crypto = md5.ComputeHash(source);//進行MD5加密

            return Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串

        }


    }
}