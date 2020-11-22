using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fuzhu;
using ADODB;
using MSXML2;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework.Parameter;
using System.Threading;
using System.Data.SqlClient;
using Process;

namespace LKU8.shoukuan
{
    public partial class UserControl1 : UserControl
    {


        DataTable dtXunjia,dtXunjiaPu;
        string sColname;
        int iks = 0;

        public UserControl1()
        {
            InitializeComponent();

            ExtensionMethods.DoubleBuffered(dataGridView1, true);
            ExtensionMethods.DoubleBuffered(dataGridView2, true);
        }

    

        #region 单元格显示按钮，参照档案
        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView2.Controls.Clear();//移除所有控件
            sColname = this.dataGridView2.Columns[e.ColumnIndex].Name.ToString();
            if (sColname == "cvenname" )
            //if (e.ColumnIndex.Equals(this.dataGridView2.Columns["dinvcode"].Index) || e.ColumnIndex.Equals(this.dataGridView2.Columns["Dopseq"].Index) || e.ColumnIndex.Equals(this.dataGridView2.Columns["Ddefine_23"].Index))
            {
                System.Windows.Forms.Button btn = new System.Windows.Forms.Button();//创建Buttonbtn   
                btn.Text = "...";//设置button文字   
                btn.Font = new System.Drawing.Font("Arial", 7);//设置文字格式   
                btn.Visible = true;//设置控件允许显示  
                btn.BackColor = dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;


                btn.Width = this.dataGridView2.GetCellDisplayRectangle(e.ColumnIndex,
                                e.RowIndex, true).Height;//获取单元格高并设置为btn的宽   
                btn.Height = this.dataGridView2.GetCellDisplayRectangle(e.ColumnIndex,
                                e.RowIndex, true).Height;//获取单元格高并设置为btn的高   

                btn.Click += new EventHandler(btn_Click);//为btn添加单击事件   

                this.dataGridView2.Controls.Add(btn);//dataGridView2中添加控件btn   

                btn.Location = new System.Drawing.Point(((this.dataGridView2.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Right) -
                        (btn.Width)), this.dataGridView2.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Y);//设置btn显示位置   
            }
        }



        void btn_Click(object sender, EventArgs e)
        {
            if (sColname == "gys")
            {
                try
                {

                    U8RefService.IServiceClass obj = new U8RefService.IServiceClass();
                    obj.RefID = "vendor_AA";
                    obj.Mode = U8RefService.RefModes.modeRefing;
                    //obj.FilterSQL = " bbomsub =1";
                    obj.FillText = dataGridView2.CurrentCell.Value.ToString();
                    obj.Web = false;
                    obj.MetaXML = "<Ref><RefSet   bMultiSel='0'  /></Ref>";
                    obj.RememberLastRst = false;
                    ADODB.Recordset retRstGrid = null, retRstClass = null;
                    string sErrMsg = "";
                    obj.GetPortalHwnd((int)this.Handle);

                    Object objLogin = canshu.u8Login;
                    if (obj.ShowRefSecond(ref objLogin, ref retRstClass, ref retRstGrid, ref sErrMsg) == false)
                    {
                        MessageBox.Show(sErrMsg);
                    }
                    else
                    {
                        if (retRstGrid != null)
                        {
                            dtXunjia.Rows[dataGridView1.CurrentCell.RowIndex]["cvenname"] = DbHelper.GetDbString(retRstGrid.Fields["cvenname"].Value);
                            //dataGridView2.CurrentCell.Value = DbHelper.GetDbString(retRstGrid.Fields["cvenname"].Value);
                            //dataGridView2.Rows[dataGridView2.CurrentCellAddress.Y].Cells["cinvname"].Value = DbHelper.GetDbString(retRstGrid.Fields["cinvname"].Value);
                            //dataGridView2.Rows[dataGridView2.CurrentCellAddress.Y].Cells["cinvstd"].Value = DbHelper.GetDbString(retRstGrid.Fields["cinvstd"].Value);
                            //this.textBox3.Text = DbHelper.GetDbString(retRstGrid.Fields["cdepcode"].Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("参照失败，原因：" + ex.Message);
                }


             
            }
            

        }

        private void dataGridView2_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.dataGridView2.Controls.Clear();//宽度调整时移除所有控件   
        }

        private void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            this.dataGridView2.Controls.Clear();//滚动条移动时移除所有控件   
        }
        #endregion


      
        #region 加载
      private void UserControl1_Load(object sender, EventArgs e)
        {
         
            dataGridView2.AutoGenerateColumns = false;
            dataGridView1.AutoGenerateColumns = false;

            Dgvfuzhu.BindReadDataGridViewStyle(this.Name, dataGridView1); // 初始化布局
            Dgvfuzhu.BindReadDataGridViewStyle(this.Name, dataGridView2); // 初始化布局
            string sql = @"select 0 as xz,[id]
      
      ,[xmbm]
      ,[cinvcode]
      ,[cinvaddcode]
      ,[cinvname],cEnglishname
      ,[cinvstd]
      ,[cqty1]
      ,[cqty2]
      ,[cqty3]
      ,[cqty4]
      ,[cqty5]
      ,[cmemo1]
      ,[czt]
      ,[cmaker]
      ,[ddate]
      ,[xunjiaid]
      ,[dmaketime]
      ,[dmodifytime]
      ,[bimportant]
      ,[burgent]
      ,[zdxjr]
      ,[fpxjr]
      ,[cztgbq]
      ,[dclosetime]
      ,[yid]
      ,[fpxjrxm]
      ,[cpersoncode]
      ,[cdefine1]
      ,[lx]
      ,[byanfa]
      ,[cyanfazt]
      ,[bself]
      ,[bsup]
      ,[bkucun]
      ,[cEnglishname] from zdy_lk_xunjia where czt = '已提交' and fpxjr = '" + canshu.userName+"'";
            dtXunjia = DbHelper.ExecuteTable(sql);
            dataGridView1.DataSource = dtXunjia;

            comboBox1.Text = "已提交";

            txtcgy.Text = canshu.userName;





            if (canshu.cQx == "1" || canshu.cQx == "2" || canshu.userName == "demo")
            {
                dataGridView1.Columns["fpxjr"].Visible = true;
            
            }
            
        }
        #endregion


        #region 写序号
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv != null)
            {
                Rectangle rect = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgv.RowHeadersWidth - 4, e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgv.RowHeadersDefaultCellStyle.Font, rect, dgv.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            }
        }

        #endregion


        #region 布局设置
        public void SaveBuju()
        {
            Dgvfuzhu.SaveDataGridViewStyle(this.Name, dataGridView1);
            Dgvfuzhu.SaveDataGridViewStyle(this.Name, dataGridView2);
            MessageBox.Show("布局保存成功！");
        }

        public void DelBuju()
        {
            Dgvfuzhu.deleteDataGridViewStyle(this.Name, dataGridView1);
            Dgvfuzhu.deleteDataGridViewStyle(this.Name, dataGridView2);
            //Dgvfuzhu.BindReadDataGridViewStyle(this.Name, dataGridView1);
            MessageBox.Show("请关掉界面重新打开，恢复初始布局！");
        }
        #endregion
  
  

        #region 增加
       public void Add()
        {
            try
            {
                dataGridView2.EndEdit();
                dataGridView2.AllowUserToAddRows = true;
                if (label6.Text == "")
                {
                    MessageBox.Show("没有选中询价行，无法增加");
                    return;
                
                }
                DataRow dr = dtXunjiaPu.NewRow();
                dr["ddate"] = DateTime.Now.ToString("yyyy-MM-dd");
                dr["id"] = label6.Text;
                 dtXunjiaPu.Rows.Add(dr);
                dataGridView2.AllowUserToAddRows = false;
                iks = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion

       #region 删除
       public void Del()
       {
           DialogResult result = CommonHelper.MsgQuestion("确认要删除选中行吗？");
           if (result == DialogResult.Yes)
           {
               try
               {
                    int i = dataGridView2.CurrentRow.Index;
                   if (i <0)
                   {
                   MessageBox.Show("没有选择删除行");
                    return;
                   }
                    
                          string sId = DbHelper.GetDbString(dataGridView2.Rows[i].Cells["autoid"].Value);
                          if (sId != "")
                          {
                              string sql = " delete from zdy_lk_xunjias where autoid=@Id ";
                              DbHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@Id", sId) });
                          }
                          else
                          {
                              dtXunjiaPu.Rows.RemoveAt(i);
                          }

                      


                   }
               
               catch (Exception ex)
               {
                   //DbHelper.RollbackAndCloseConnection(tran);
                   CommonHelper.MsgError("删除失败！原因：" + ex.Message);
               }
           CommonHelper.MsgInformation("删除完成！");
           dtXunjiaPu = DbHelper.ExecuteTable("select id,autoid,ddate,gys,xunjia1,xunjia2,xunjia3,bz1,bz2,bz3 from zdy_lk_xunjias where id ='" + label6.Text + "'");
           dataGridView2.DataSource = dtXunjiaPu;
           }
           
       }
       #endregion

       #region 保存
       public void Save2()
       {
           Save();
           MessageBox.Show("保存完成");
               return;
       
       }

       public void Save()
       {
           txtcinvcode.Focus();
           //dataGridView2.CommitEdit((DataGridViewDataErrorContexts)123);

           //MessageBox.Show(dataGridView2.Rows[1].Cells["xunjia3"].Value.ToString());
           //MessageBox.Show(dataGridView2.Rows[1].Cells["autoid"].Value.ToString());
           //return;
           try
           {
              
               for (int i = 0; i < dataGridView2.Rows.Count; i++)
               {
                

                       //没id，自动保存。有id，判断是否modifyed，如果更改了，update
                       string autoId = DbHelper.GetDbString(dataGridView2.Rows[i].Cells["autoid"].Value);
                    if (string.IsNullOrEmpty(autoId) || autoId=="")
                    {
                           string sql = @" Insert Into zdy_lk_xunjias(id,ddate,gys,xunjia1,xunjia2,xunjia3,bz1,bz2,bz3) 
                    values(@id,@ddate,@gys,@xunjia1,@xunjia2,@xunjia3,@bz1,@bz2,@bz3) select @@identity ";
                           object obj = DbHelper.GetSingle(sql, new SqlParameter[]{ 
                              new SqlParameter("@id", dataGridView2.Rows[i].Cells["id2"].Value), 
                             new SqlParameter("@ddate", dataGridView2.Rows[i].Cells["ddate2"].Value),
                             new SqlParameter("@gys", dataGridView2.Rows[i].Cells["gys"].Value),
                             new SqlParameter("@xunjia1", dataGridView2.Rows[i].Cells["xunjia1"].Value),
                             new SqlParameter("@xunjia2", dataGridView2.Rows[i].Cells["xunjia2"].Value),
                             new SqlParameter("@xunjia3", dataGridView2.Rows[i].Cells["xunjia3"].Value),
                             new SqlParameter("@bz1", dataGridView2.Rows[i].Cells["bz1"].Value),
                             new SqlParameter("@bz2",dataGridView2.Rows[i].Cells["bz2"].Value),
                             new SqlParameter("@bz3", dataGridView2.Rows[i].Cells["bz3"].Value)
                            });
                           //数据表赋值
                           dtXunjiaPu.Rows[i]["autoid"] = Convert.ToInt32(obj);
                           // 设置为非更改状态
                           
                       }
                       else
                       {
                           

                           string sql = @" update zdy_lk_xunjias
                        set gys = @gys,ddate =@ddate,xunjia1 = @xunjia1 ,xunjia2 = @xunjia2,xunjia3 =@xunjia3,bz1=@bz1,bz2= @bz2,bz3= @bz3
                         where autoid = @autoid  ";
                         DbHelper.ExecuteNonQuery (sql, new SqlParameter[]{ 
                             new SqlParameter("@gys", dataGridView2.Rows[i].Cells["gys"].Value), 
                             new SqlParameter("@ddate", dataGridView2.Rows[i].Cells["ddate2"].Value),
                             new SqlParameter("@xunjia1", dataGridView2.Rows[i].Cells["xunjia1"].Value),
                             new SqlParameter("@xunjia2", dataGridView2.Rows[i].Cells["xunjia2"].Value),
                             new SqlParameter("@xunjia3", dataGridView2.Rows[i].Cells["xunjia3"].Value),
                             new SqlParameter("@bz1", dataGridView2.Rows[i].Cells["bz1"].Value),
                             new SqlParameter("@bz2", dataGridView2.Rows[i].Cells["bz2"].Value),
                             new SqlParameter("@bz3", dataGridView2.Rows[i].Cells["bz3"].Value),
                             new SqlParameter("@autoid", autoId)
                            });

                       }
                   }
               
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
               return;
           }
          
       }
       #endregion

       #region 分配
       public void FenPei()
       {
           txtcinvcode.Focus();
           //dataGridView2.CommitEdit((DataGridViewDataErrorContexts)123);

           //MessageBox.Show(dataGridView2.Rows[1].Cells["xunjia3"].Value.ToString());
           //MessageBox.Show(dataGridView2.Rows[1].Cells["autoid"].Value.ToString());
           //return;
           try
           {
               SqlConnection conn = new SqlConnection(DbHelper.conStr);
               SqlCommand comm = new SqlCommand();
               SqlDataAdapter da = new SqlDataAdapter();
               SqlCommandBuilder cb = new SqlCommandBuilder();
               string sql = @"SELECT [id]
      
      ,[xmbm]
      ,[cinvcode]
      ,[cinvaddcode]
      ,[cinvname]
      ,[cinvstd]
      ,[cqty1]
      ,[cqty2]
      ,[cqty3]
      ,[cqty4]
      ,[cqty5]
      ,[cmemo1]
      ,[czt]
      ,[cmaker]
      ,[ddate]
      ,[xunjiaid]
      ,[dmaketime]
      ,[dmodifytime]
      ,[bimportant]
      ,[burgent]
      ,[zdxjr]
      ,[fpxjr]
      ,[cztgbq]
      ,[dclosetime]
      ,[yid]
      ,[fpxjrxm]
      ,[cpersoncode]
      ,[cdefine1]
      ,[lx]
      ,[byanfa]
      ,[cyanfazt]
      ,[bself]
      ,[bsup]
      ,[bkucun]
      ,[cEnglishname]   FROM zdy_lk_xunjia";
               comm = conn.CreateCommand();
               comm.CommandText = sql;
               da = new SqlDataAdapter(comm);
               cb = new SqlCommandBuilder(da);
               da.Update(dtXunjia);

               MessageBox.Show("保存成功！");
               Cx();
             

           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
               return;
           }

       }
       #endregion

       #region 提交
       public void Tijiao()
       {

           dataGridView2.Update();
           dataGridView2.EndEdit();

           try
           {

               for (int i = 0; i < dataGridView2.Rows.Count; i++)
               {
                  
                       //判断是否输入cas和数量，没输入的不允许进行提交
                       if (DbHelper.GetDbString(dataGridView2.Rows[i].Cells["gys"].Value) == "" && DbHelper.GetDbString(dataGridView2.Rows[i].Cells["xunjia1"].Value) == "")
                       {
                           MessageBox.Show("第"+i.ToString()+"行没有输入供应商和询价结果，无法提交");
                           continue;
                       }
                      //f分配采购员
                       //如果是已指定采购员的，直接到指定采购员
                       //如果是没指定的，进行分配，按行进行分配
                       //还是写触发器，同时写提醒语句。

                  

                       //没id，提示保存
                       string autoId = DbHelper.GetDbString(dataGridView2.Rows[i].Cells["autoid"].Value);
                       if (string.IsNullOrEmpty(autoId))
                       {
                           MessageBox.Show("第"+(i+1).ToString()+"没有保存，请先保存");
                           return;
                       }
               }
                     
                       

                          string cZt = dataGridView1.CurrentRow.Cells["czt"].Value.ToString();

                          if (cZt == "已提交")
                           {
                        string sql = @" update zdy_lk_xunjia
                        set czt='询价完成'
                         where id = @id  ";
                               DbHelper.ExecuteNonQuery(sql, new SqlParameter[]{ 
                                                           new SqlParameter("@id",label6.Text) });

                          string cXs= dataGridView1.CurrentRow.Cells["cmaker"].Value.ToString();
                              string cPersoncode  = DbHelper.ExecuteScalar("select cUser_Id  from ufsystem..UA_User where cUser_Name ='"+cXs+"'").ToString();
                              
                              string  sqlmsg = @"
  INSERT INTO UFSystem..ua_message (cmsgid,nmsgtype,cmsgtitle,cmsgcontent,csender,creceiver,dsend,nvalidday,bhasread,nurgent, account,[year],cmsgpara)
VALUES(newid(),313552,'询价完成提醒,id号'+@id,'询价完成提醒，id号'+@id,@sender,@rec,getdate(),4,0,0,'001','2016',null)";
                              DbHelper.ExecuteNonQuery(sqlmsg, new SqlParameter[]{ 
                                                           new SqlParameter("@id",label6.Text),
                              new SqlParameter("@sender","采购询价"),
                                new SqlParameter("@rec",cPersoncode),
                            });

                         
                          
                       
                  
               }
                       
              
               MessageBox.Show("提交完成");
               Cx();
           }
         
           catch (Exception ex)
           {
               CommonHelper.MsgInformation(ex.Message);
               return;
           }
           ////判断是否输入cas和数量
           //if (DbHelper.GetDbString(dataGridView1.Rows[e.RowIndex].Cells["cqty1"].Value) == "" && DbHelper.GetDbString(dataGridView1.Rows[e.RowIndex].Cells["cinvcode"].Value) == "")
           //{
           //    MessageBox.Show("当前行没有输入cas号和数量，无法保存");
           //    return;

           //}

           //UFLTMService connSrv = new UFLTMService();

           //connSrv.Start(canshu.u8Login.UFDataConnstringForNet);		//传递连接串初始化对象

           //connSrv.BeginTransaction();	//开始事务





           //AuditServiceProxy auditSvc = new AuditServiceProxy();

           ////构造Login的 CalledContext对象
           //CalledContext calledCtx = new CalledContext();
           //calledCtx.subId = "ST";
           //calledCtx.TaskID = canshu.u8Login.get_TaskId();
           //calledCtx.token = canshu.u8Login.userToken;
           ////业务对象标识
           //string bizObjectId = "UAPFORM.U8CUSTDEF_0018";
           ////业务事件标识  
           //string bizEventId = "U8CUSTDEF_0018.Commit";
           ////单据号
           //string voucherId = "10";
           //if (bizEventId == string.Empty || bizObjectId == string.Empty)
           //{
           //    MessageBox.Show("请选择选择业务对象或业务事件!");
           //    //return null;
           //}
           //string errMsg = "";
           ////bool a = auditSvc.IsFlowEnabled(bizObjectId, bizEventId, calledCtx, ref errMsg);
           ////if (a == true)
           ////    MessageBox.Show("提交成功");
           ////else
           ////    MessageBox.Show("提交失败，失败原因：" + errMsg);


           //errMsg = "";
           //bool bControled = true;

           //bool ret = auditSvc.SubmitApplicationMessage(bizObjectId, bizEventId, voucherId, calledCtx, ref bControled, ref errMsg);

           //if (ret == true && bControled)
           //    MessageBox.Show("提交成功");
           //else
           //    MessageBox.Show("提交失败，失败原因：" + errMsg);


           //connSrv.Commit();	//提交事务，回滚事务请根据自己代码的情况调用Rollback
           //connSrv.Finish();




           




       }
       #endregion

       #region 联查
       public void Liancha()
       {
           try
           {
               this.Validate();
               this.Update();

               string cInvcode = DbHelper.GetDbString(dataGridView1.CurrentRow.Cells["cinvcode"].Value);
               string cInvaddcode = DbHelper.GetDbString(dataGridView1.CurrentRow.Cells["cinvaddcode"].Value);
               string cInvname = DbHelper.GetDbString(dataGridView1.CurrentRow.Cells["cinvname"].Value);
               string cInvstd = DbHelper.GetDbString(dataGridView1.CurrentRow.Cells["cinvstd"].Value);
               string cInvnameEng =  DbHelper.GetDbString(dataGridView1.CurrentRow.Cells["cEnglishname"].Value);
               if (string.IsNullOrEmpty(cInvcode) == false || string.IsNullOrEmpty(cInvaddcode) == false)
               {

                   FrmTree frm = new FrmTree(cInvcode, cInvname, cInvstd, cInvaddcode, cInvnameEng);
                   frm.ShowDialog();
               }
               else
               {

                   MessageBox.Show("选中行存货编码为空，不能联查！");
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
               return;
           }
       }
       #endregion

       #region 查询
       public void Cx()
       {
           SearchCondition searchObj = new SearchCondition();
           //searchObj.AddCondition("cinvcode", txtcinvcode.Text, SqlOperator.Like);
           searchObj.AddCondition("cmaker", txtcCusname.Text, SqlOperator.Like);
           searchObj.AddCondition("fpxjr", txtcgy.Text, SqlOperator.Like);
           searchObj.AddCondition("ddate", dateTimePicker1.Value.ToString("yyyy-MM-dd"), SqlOperator.MoreThanOrEqual, dateTimePicker1.Checked == false);
           searchObj.AddCondition("ddate", dateTimePicker2.Value.ToString("yyyy-MM-dd"), SqlOperator.LessThanOrEqual, dateTimePicker2.Checked == false);
           //searchObj.AddCondition("czt", comboBox1.Text, SqlOperator.Equal);


           string conditionSql = searchObj.BuildConditionSql(2);

           if (comboBox1.Text == "询价完成")
           {
               conditionSql += " and czt in ('询价完成','关闭')";

           }
           else
           {
               conditionSql += string.Format(" and czt ='{0}' ",comboBox1.Text);
           }

           if (!string.IsNullOrEmpty(txtcinvcode.Text))
           {

               conditionSql += string.Format(" and (cinvcode like '%{0}%' or cinvaddcode like '%{0}%')", txtcinvcode.Text);
           }

           StringBuilder strb = new StringBuilder(@"SELECT 0 as xz,[id]
      
      ,[xmbm]
      ,[cinvcode]
      ,[cinvaddcode]
      ,[cinvname]
      ,[cinvstd]
      ,[cqty1]
      ,[cqty2]
      ,[cqty3]
      ,[cqty4]
      ,[cqty5]
      ,[cmemo1]
      ,[czt]
      ,[cmaker]
      ,[ddate]
      ,[xunjiaid]
      ,[dmaketime]
      ,[dmodifytime]
      ,[bimportant]
      ,[burgent]
      ,[zdxjr]
      ,[fpxjr]
      ,[cztgbq]
      ,[dclosetime]
      ,[yid]
      ,[fpxjrxm]
      ,[cpersoncode]
      ,[cdefine1]
      ,[lx]
      ,[byanfa]
      ,[cyanfazt]
      ,[bself]
      ,[bsup]
      ,[bkucun]
      ,[cEnglishname] from zdy_lk_xunjia where 1=1");
           strb.Append(conditionSql);

          
           dtXunjia= DbHelper.ExecuteTable(strb.ToString());
           dataGridView1.DataSource = dtXunjia;
       }
       #endregion

       #region 输入条件 参照
       private void button1_Click(object sender, EventArgs e)
       {
           try
           {

               U8RefService.IServiceClass obj = new U8RefService.IServiceClass();
               obj.RefID = "Inventory_AA";
               obj.Mode = U8RefService.RefModes.modeRefing;
               //obj.FilterSQL = " bbomsub =1";
               obj.FillText = txtcinvcode.Text;
               obj.Web = false;
               obj.MetaXML = "<Ref><RefSet   bMultiSel='0'  /></Ref>";
               obj.RememberLastRst = false;
               ADODB.Recordset retRstGrid = null, retRstClass = null;
               string sErrMsg = "";
               obj.GetPortalHwnd((int)this.Handle);

               Object objLogin = canshu.u8Login;
               if (obj.ShowRefSecond(ref objLogin, ref retRstClass, ref retRstGrid, ref sErrMsg) == false)
               {
                   MessageBox.Show(sErrMsg);
               }
               else
               {
                   if (retRstGrid != null)
                   {

                       this.txtcinvcode.Text = DbHelper.GetDbString(retRstGrid.Fields["cinvcode"].Value);
                   }
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show("参照失败，原因：" + ex.Message);
           }
       }
       #endregion

       #region 换行自动保存
       private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
       {
          
       }
       #endregion

     
       #region 点击显示明细
       private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
       {
           //if (e.RowIndex != -1)
           //{
           //    string id = DbHelper.GetDbString(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
           //    dataGridView2.DataSource = DbHelper.ExecuteTable("select ddate,gys,xunjia1,xunjia2,xunjia3,bz1,bz2,bz3 from zdy_lk_xunjias where id ='" + id.ToString() + "'");
           
           
           //}
       }
       #endregion

     
   
  #region 进入当前行，获得状态
       private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
       {
           if (e.ColumnIndex > -1)
           {
               label6.Text = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();

               dtXunjiaPu = DbHelper.ExecuteTable("select id,autoid,ddate,gys,xunjia1,xunjia2,xunjia3,bz1,bz2,bz3 from zdy_lk_xunjias where id ='" + label6.Text + "'");
               dataGridView2.DataSource = dtXunjiaPu;
           }


       }
  #endregion

        //定时刷新
       private void timer1_Tick(object sender, EventArgs e)
       {
           MessageBox.Show("44");
           Cx();
       }

       private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
       {
           dataGridView2.CommitEdit((DataGridViewDataErrorContexts)123);
           dataGridView2.BindingContext[dataGridView2.DataSource].EndCurrentEdit();
           iks++;
       }

       private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
       {

       }

        //设置颜色
       private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
       {
           if (e.RowIndex > -1)
           {
               string cImportant = this.dataGridView1.Rows[e.RowIndex].Cells["重要"].Value.ToString();
               string cUrgent = this.dataGridView1.Rows[e.RowIndex].Cells["紧急"].Value.ToString();
               if (cImportant == "True" || cUrgent == "True")
               {
                   dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
               }
           }
       }

       private void dataGridView2_Leave(object sender, EventArgs e)
       {
           dataGridView2.CommitEdit((DataGridViewDataErrorContexts)123);
           dataGridView2.BindingContext[dataGridView2.DataSource].EndCurrentEdit();
           
               Save();
          
          
       }

       #region 双击复制
       private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
       {
           if (dataGridView1 != null)
           {
               string sv = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
               Clipboard.SetData(DataFormats.Text, sv);
           }
       }
       #endregion

       private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
       {
           if (dataGridView2 != null)
           {
               string sv = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
               Clipboard.SetData(DataFormats.Text, sv);
           }
       }

       private void button2_Click(object sender, EventArgs e)
       {
           try
           {

               U8RefService.IServiceClass obj = new U8RefService.IServiceClass();
               obj.RefID = "U8CUSTDEF_0016_AA";
               obj.Mode = U8RefService.RefModes.modeRefing;
               //obj.FilterSQL = " bbomsub =1";
               //obj.FillText = dataGridView1.CurrentCell.Value.ToString();
               obj.Web = false;
               obj.MetaXML = "<Ref><RefSet   bMultiSel='0'  /></Ref>";
               obj.RememberLastRst = false;
               ADODB.Recordset retRstGrid = null, retRstClass = null;
               string sErrMsg = "";
               obj.GetPortalHwnd((int)this.Handle);

               Object objLogin = canshu.u8Login;
               if (obj.ShowRefSecond(ref objLogin, ref retRstClass, ref retRstGrid, ref sErrMsg) == false)
               {
                   MessageBox.Show(sErrMsg);
               }
               else
               {
                   if (retRstGrid != null)
                   {
                       txtcgy.Text = DbHelper.GetDbString(retRstGrid.Fields["cNo"].Value);

                   }
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show("参照失败，原因：" + ex.Message);
           }
       }

       private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
       {
           this.dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
       }

       private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
       {
           this.dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
       }



    }
}
