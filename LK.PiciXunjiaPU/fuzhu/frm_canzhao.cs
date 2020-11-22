using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace U8Soft.BomManage
{
    public partial class frm_canzhao : Form
    {

         DataTable dts;//数据源
         DataTable dtCzMc = new DataTable();  //数据源字段
         DataTable dtCzOp1 = new DataTable();
         DataTable dtCzOp2 = new DataTable();
        //public DataSet ds = new DataSet();
         string[] colArray;
         string CZname;

      

        public delegate void TransfDelegate(String[] value);
        public event TransfDelegate TransfEvent; 
        public frm_canzhao()
        {
            InitializeComponent();
        }


        #region 重载窗口
        /// <summary>
/// 参照窗体
/// </summary>
/// <param name="dt">参照数据源</param>
/// <param name="canZhaoTxt">参照，输入的文本</param>
/// <param name="canZhaoMing">参照名称</param>
/// <param name="renum">返回字段个数</param>
/// <param name="cflag"></param>
        public  frm_canzhao(DataTable dt,string canZhaoTxt,string canZhaoMing, string[] csArray)
        {
            

            InitializeComponent();
            this.Text = canZhaoMing + " 参照";
            CZname = canZhaoMing;
            dts = dt;
            dataGridView1.DataSource = dts;
            textBox1.Text = canZhaoTxt;
            colArray = csArray;//返回字段个数
        
            
           ///初始化查询条件
         dtCzMc.Columns.Add(new DataColumn("czmc", typeof(string)));//为dt_dry表内建立Column
         dtCzMc.Columns.Add(new DataColumn("cztype", typeof(string)));
           
            //string colname = dataGridView1.Columns[0].HeaderText;
            if (dt.Columns.Count>=1)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                 dtCzMc.Rows.Add(dataGridView1.Columns[i].HeaderText,  dts.Columns[i].DataType.ToString());//为dt_dry表内建立Column  

                }

                comboBox1.DataSource = dtCzMc;
                comboBox1.DisplayMember = "czmc";
                comboBox1.ValueMember = "cztype";
                comboBox1.Text =dtCzMc.Rows[0]["czmc"].ToString();


                this.dataGridView1.Columns[0].Width = 100;
                this.dataGridView1.Columns[1].Width = 300;
                //comboBox1.DisplayMember = "cztype";
                //comboBox1.ValueMember = "czmc";
                
            }
            //string coldata = dataGridView1.Columns[0]

            dtCzOp1.Columns.Add(new DataColumn("operater", typeof(string)));
            dtCzOp1.Rows.Add("like");
            dtCzOp1.Rows.Add("=");
            dtCzOp1.Rows.Add(">=");
            dtCzOp1.Rows.Add("<=");
            dtCzOp1.Rows.Add("<>");

            dtCzOp2.Columns.Add(new DataColumn("operater", typeof(string)));
            dtCzOp2.Rows.Add("=");
            dtCzOp2.Rows.Add(">=");
            dtCzOp2.Rows.Add("<=");
            dtCzOp2.Rows.Add("<>");

            comboBox2.DataSource = dtCzOp1;
            comboBox2.DisplayMember = "operater";
            comboBox2.Text = "like";
            cx(1);

        }
      

        private void frm_canzhao_Load(object sender, EventArgs e)
        {

            
        }
        #endregion


        #region 查询
        private void button1_Click(object sender, EventArgs e)
        {
            cx(2);
        }
        /// <summary>
        /// 全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DataView dv1 = dts.DefaultView;
            dv1.RowFilter = "1=1";
            dataGridView1.DataSource = dv1;
        }
        /// <summary>
        /// 执行过滤
        /// </summary>
        /// <param name="i">如果1，第一次查询。条件不全，全部显示。如果2，输入查询，提醒不全</param>
        private void cx(int i)
        {
            
            DataView dv1 = dts.DefaultView;
            string sText = textBox1.Text;




            if (comboBox1.Text == "" || comboBox2.Text == "" || textBox1.Text == "")
            {
                if (i == 1)
                {
                    dv1.RowFilter = "1=1";
                    dataGridView1.DataSource = dv1;
                }
                else
                {
                    MessageBox.Show("查询条件输入不完整！");
                    return;
                }
            }
            else
            {
                if (comboBox2.Text == "like")
                    sText = "%" + sText + "%";

                string sfilter = comboBox1.Text + " " + comboBox2.Text + " '" + sText + "'";
                dv1.RowFilter = sfilter;
                //dv1.RowFilter = "角色id like 1%";
                dataGridView1.DataSource = dv1;
            }
        }
        #endregion

        #region 换查询条件，换运算符组
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          //字符型的采用like，其他都不用。可以在查询是，sql里转换成string
            //MessageBox.Show(comboBox1.SelectedValue.ToString());
          if (comboBox1.SelectedValue.ToString() == "System.String")
          {
              comboBox2.DataSource = dtCzOp1;
              comboBox2.DisplayMember = "operater";
              //comboBox2.ValueMember = "operater";
          }
          else
          {
              comboBox2.DataSource = dtCzOp2;
              comboBox2.DisplayMember = "operater";
              //comboBox2.ValueMember = "operater";
          }
        }
        #endregion

        #region 双击返回数值
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (colArray[0] != "不返回")//传递数组，如果第一个是不返回，就不返回值。
            {
                int column = dataGridView1.CurrentCellAddress.X;
                int row = dataGridView1.CurrentCellAddress.Y;
                string[] sArray = new string[colArray.Length];//新的数组，返回值
                //默认，查询第一栏就是返回值。
                for (int i = 0; i < colArray.Length; i++)
                {
                    sArray[i] = dataGridView1.Rows[row].Cells[colArray[i]].Value.ToString();
                }

                TransfEvent(sArray);
                this.Close();
            }
        }
        #endregion


      
    }
}
