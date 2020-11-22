﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFIDA.U8.Portal.Proxy.supports;
using UFIDA.U8.Portal.Proxy.editors;
using UFSoft.U8.Framework.Login.UI;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using fuzhu;


namespace LKU8.shoukuan
{
    public class xialiaologin : NetLoginable
    {
        //U8EnvContext envContext = new U8EnvContext();
        public override object CallFunction(string cMenuId, string cMenuName, string cAuthId, string cCmdLine)
        {

            //canshu.uLogin = LoginObject;
          
            //赋值给公共变量 constr
            canshu.userName = LoginObject.GetLoginInfo().UserId;
            //canshu.conStr = LoginObject.GetLoginInfo().ConnString;
            //canshu.conStr = canshu.conStr.Substring(canshu.conStr.IndexOf("data"), canshu.conStr.IndexOf(";Conn") + 2);
             string userToken = LoginObject.userToken;
             //string accid = LoginObject.GetLoginInfo().AccID;
             //string iyear = LoginObject.GetLoginInfo().iYear;
             //string ztdb = "//ZT" + accid + "//" + iyear;
             canshu.acc = LoginObject.GetLoginInfo().AccID;


             U8Login.clsLogin u8Login = new U8Login.clsLogin();
             String sSubId = "AS";
             String sAccID = LoginObject.GetLoginInfo().AccID;
             String sYear = LoginObject.GetLoginInfo().iYear;
             String sUserID = LoginObject.GetLoginInfo().UserId;
             String sPassword =LoginObject.GetLoginInfo().Password;
             String sDate = LoginObject.GetLoginInfo().operDate;
             String sServer = LoginObject.GetLoginInfo().AppServer;
             //String sSerial = "";
             if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer))
             //if (!uLogin.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer))
             {
                 MessageBox.Show("登陆失败，原因：" + u8Login.ShareString);
                 Marshal.FinalReleaseComObject(u8Login);
                 return "0";
             }

             canshu.conStr = u8Login.UFDataConnstringForNet;

             string sql = "select cSysUserName from UA_User where cSysUserName is not null and  cUser_Id='" + canshu.userName + "'";
             DataTable dt = DbHelper.ExecuteTable(sql);
             if (dt.Rows.Count > 0)
             {
                 canshu.cQx = DbHelper.GetDbString(dt.Rows[0]["cSysUserName"]);
             }
             else
             {
                 canshu.cQx = "0";

             }

             //U8Login.clsLogin u8Login = new U8Login.clsLoginClass();
             //u8Login.ConstructLogin(userToken);
             ////u8Login.UfDbPath = ztdb;

             canshu.u8Login = u8Login;


             //MessageBox.Show(canshu.conStr);
            INetUserControl mycontrol = new MyNetUserControl();
            mycontrol.Title = cMenuName;
            base.ShowEmbedControl(mycontrol, cMenuId, true);



            return null;
        }
        public override bool SubSysLogin()
        {
            
            return true;
        }


    }
}
