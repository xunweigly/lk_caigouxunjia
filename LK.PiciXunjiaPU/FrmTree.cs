using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using fuzhu;

namespace LKU8.shoukuan
{
    public partial class FrmTree : Form
    {
        private DataTable dtTree = null;

        private DataView dv = null;
        string cInvcode, cInvname, cInvstd, cInvaddcode,cInvnameEng;
        public FrmTree()
        {
            InitializeComponent();
        }
        public FrmTree(string cinvcode,string cinvname,string cinvstd)
        {
            InitializeComponent();
            cInvcode = cinvcode;
            cInvname = cinvname ;
            cInvstd = cinvstd;
            label1.Text = cInvcode;
            label3.Text = cInvname;
            label5.Text = cInvstd;
            //ExtensionMethods.DoubleBuffered(dataGridView1, true);
            //dataGridView1.AutoGenerateColumns = false;
        }

        public FrmTree(string cinvcode, string cinvname, string cinvstd, string cinvaddcode, string cinvnameEng)
        {
            InitializeComponent();
            cInvcode = cinvcode;
            cInvname = cinvname;
            cInvstd = cinvstd;
            cInvaddcode = cinvaddcode;
            cInvnameEng = cinvnameEng;
            label1.Text = cInvcode;
            label3.Text = cInvname;
            label5.Text = cInvstd;
            label6.Text = cinvaddcode;
            label9.Text = cinvnameEng;
            //ExtensionMethods.DoubleBuffered(dataGridView1, true);
            //dataGridView1.AutoGenerateColumns = false;
        }

        #region 加载
        private void FrmTree_Load(object sender, EventArgs e)
        {
            try
            {
                //zdy_zd_sp_xzqs](@dt1 datetime ,@dt2 datetime ,@lx varchar(20))

                //            string sql =string.Format(@"select a.ddate,a.cmaker,b.gys,a.fpxjrxm,a.cinvcode,a.cinvstd,a.cinvname,a.cqty1,a.cqty2,a.cqty3,b.xunjia1,b.xunjia2,b.xunjia3,b.bz1,b.bz2,b.bz3  from zdy_lk_xunjia a,zdy_lk_xunjias b 
                //               where a.id = b.id and a.id in (select top 5 id from zdy_lk_xunjia where  cinvcode = '{0}' or cinvaddcode = '{1}' or cinvcode = '{1}' or cinvaddcode = '{0}' order by id desc)",cInvcode,cInvaddcode);

                //201903013 更改为全部显示
                string sql = string.Format(@"select b.ddate 日期,a.cmaker 业务员,b.gys 供应商,a.fpxjrxm 采购员,a.cinvcode 存货编码,a.cinvaddcode CAS,a.cinvstd 规格型号,a.cinvname 存货名称,a.cqty1 数量,b.xunjia1 询价1,b.xunjia2 询价2,b.xunjia3 询价3,b.bz1 备注1,b.bz2 备注2,b.bz3 备注3  from zdy_lk_xunjia a,zdy_lk_xunjias b 
                           where a.id = b.id and (cinvcode = '{0}' or cinvaddcode = '{1}' or cinvcode = '{1}' or cinvaddcode = '{0}' )", cInvcode, cInvaddcode);


                DataTable dtmx = DbHelper.ExecuteTable(sql);
                gridControl1.DataSource = dtmx;

                //采购历史查询
                sql = string.Format(@"select a.dPODate 订单日期,a.cPOID 订单号,d.cVenName 供应商名称,c.cPersonName 采购员,b.cinvcode 存货编码,i.InvAddCode CAS
,i.InvName 存货名称,i.InvStd 规格型号,i.ComUnitName 单位,CONVERT(REAL, b.iQuantity) 数量,CONVERT(REAL,b.iTaxPrice) 含税单价,CONVERT(REAL,b.iPerTaxRate) 税率,CONVERT(REAL,b.iSum) 含税金额 from po_pomain a
inner join po_podetails b  ON a.poid = b.poid
LEFT JOIN dbo.Person c ON a.cPersonCode = c.cPersonCode
INNER JOIN vendor d ON a.cVenCode = d.cVenCode
INNER JOIN dbo.v_bas_inventory i ON b.cInvCode =i.invcode where (cinvcode = '{0}' or invaddcode = '{1}' or cinvcode = '{1}' or invaddcode = '{0}' )", cInvcode, cInvaddcode);


                DataTable  dtmx2 = DbHelper.ExecuteTable(sql);

                gridControl2.DataSource = dtmx2;

                //供应商列表查询
                sql = string.Format(@"SELECT cvencode 供应商编码,cvenname 供应商名称,cinvcode 存货编码,cinvaddcode CAS,cinvname 存货名称,
cinvnameeng 英文名,cinvstd 规格型号,cmemo 备注 FROM zdy_lk_veninvcode where ((cinvcode = '{0}'or cinvcode = '{1}') and isnull(cinvcode,'')<>'') or ((cinvaddcode = '{1}'  or cinvaddcode = '{0}') and isnull(cinvaddcode,'')<>'') or cinvnameeng ='{2}' ", cInvcode, cInvaddcode, cInvnameEng);


                DataTable dtmx3 = DbHelper.ExecuteTable(sql);

                gridControl3.DataSource = dtmx3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }

        }
        #endregion

    

   
    }
}
