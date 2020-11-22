using System;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using MSXML2;
using System.Windows.Forms;
using ADODB;
using System.Data;
using System.Runtime.InteropServices;
using fuzhu;
using System.Data.SqlClient;

namespace LKU8.shoukuan
{
    class shoukuanapi
    {

           public static string InsertProduct(DataTable dthead,DataTable dtbody)
           {

              
               
               U8EnvContext envContext = new U8EnvContext();
               envContext.U8Login = canshu.u8Login;
               //envContext.U8Login = u8Login;
               //第三步：设置API地址标识(Url)
               //当前API：添加新单据的地址标识为：U8API/MaterialOut/Add
               U8ApiAddress myApiAddress = new U8ApiAddress("U8API/ARClose/SaveVouch");

               //第四步：构造APIBroker
               U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

               //第五步：API参数赋值

               ////给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：11
               //broker.AssignNormalValue("sVouchType", "11");
               ////该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
               //string vouid = "";
               //broker.AssignNormalValue("VouchId", vouid);

               ////该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数


               MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocumentClass();
               broker.AssignNormalValue("domMsg", domMsg);

               ////给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
               //broker.AssignNormalValue("bCheck", true);

               ////给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
               //broker.AssignNormalValue("bBeforCheckStock", true);


               ////给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
               //broker.AssignNormalValue("bIsRedVouch", false);

               ////给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
               //string sadd = "";
               //broker.AssignNormalValue("sAddedState", sadd);

               ////给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
               //broker.AssignNormalValue("bReMote", false);

               //打开adodb连接
               Connection oCon = new ConnectionClass();
               oCon.Open(envContext.U8Login.UfDbName);
               MSXML2.IXMLDOMDocument2 domhead = new DOMDocument();
               MSXML2.IXMLDOMDocument2 dombody = new DOMDocument();
               //表头赋值

                
               //表头DOM赋值
               string sqlt = "select cast(null as nvarchar(2)) as editprop,* from AR_CloseR where 1=2 ";
               Recordset rs = new Recordset();

               rs.Open(sqlt, oCon, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly);
               rs.Save(domhead, PersistFormatEnum.adPersistXML);
               //插入表头信息
               IXMLDOMElement eleHead = (IXMLDOMElement)domhead.selectSingleNode("//rs:data");
               IXMLDOMElement ele = (IXMLDOMElement)domhead.createElement("z:row");
               eleHead.appendChild(ele);

               try
               {
                 


               ele.setAttribute("editprop", "A");//A代表增加，M代表修改

               ele.setAttribute("dVouchDate", DateTime.Now.ToShortDateString());
               ele.setAttribute("cdwcode","0999"); 
                   //ele.setAttribute("id", rdid);
                   ele.setAttribute("ccode", "100");
                   ele.setAttribute("VT_ID", "65");
                   ele.setAttribute("ddate", DateTime.Now.ToShortDateString()); //出库日期


                   ele.setAttribute("dnmaketime", DateTime.Now.ToString()); //制单日期
                   ele.setAttribute("cmemo", "下料生成"); //备注
                   ele.setAttribute("cmaker", canshu.u8Login.cUserName); //制单人


                   broker.AssignNormalValue("DomHead", domhead);
               }
               catch (Exception ex)
               {
                   MessageBox.Show(ex.Message, "错误发生", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   return "0";
               }

               

               //表体赋值
               string sqlw = "select cast(null as nvarchar(2)) as editprop,* from recordoutsq where 1=2 ";
               //Recordset rs = new Recordset();
               rs.Close();
               rs.Open(sqlw, oCon, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly);
               rs.Save(dombody, PersistFormatEnum.adPersistXML);
               //插入表体尾信息
               //dombody.rowcount = 
               IXMLDOMElement eleBody = (IXMLDOMElement)dombody.selectSingleNode("//rs:data");
               for (int i = 0;i<dtbody.Rows.Count;i++)
                {
       
               ele = (IXMLDOMElement)dombody.createElement("z:row");
               eleBody.appendChild(ele);

              // 表尾赋值

               //string cinvcode = "01019002063";
               //int autoid = 1000;
               //decimal sl = 4m;
try
               {
               ele.setAttribute("editprop", "A");
               //ele.setAttribute("autoid", autoid);
               ele.setAttribute("cinvcode", DbHelper.GetDbString(dtbody.Rows[i]["存货编码"]));
               if (string.IsNullOrEmpty(DbHelper.GetDbString(dtbody.Rows[i]["lotno"])) != true)
                   ele.setAttribute("cBatch", DbHelper.GetDbString(dtbody.Rows[i]["lotno"]));

               ele.setAttribute("iquantity", DbHelper.GetDbString(dtbody.Rows[i]["本次下料"]));
               //MessageBox.Show(dtbody.Rows[i]["本次下料"].ToString());
               ele.setAttribute("iMPoIds", DbHelper.GetDbInt(dtbody.Rows[i]["allocateid"]));
               ele.setAttribute("cmocode", DbHelper.GetDbString(dthead.Rows[0]["染料单号"]));
               ele.setAttribute("inquantity",DbHelper.GetDbdecimal(dtbody.Rows[i]["未下数量"])); //未下应下数量
               //生产订单信息
               ele.setAttribute("invcode", DbHelper.GetDbString(dthead.Rows[0]["色号"]));
               ele.setAttribute("imoseq", DbHelper.GetDbString(dthead.Rows[0]["sortseq"]));
               ele.setAttribute("iopseq", DbHelper.GetDbString(dtbody.Rows[i]["opseq"]));
               ele.setAttribute("copdesc", DbHelper.GetDbString(dtbody.Rows[i]["工序"]));
               ele.setAttribute("iExpiratDateCalcu",0);
               ele.setAttribute("irowno",i+1);
               }
catch (Exception ex)
{
    MessageBox.Show(ex.Message, "错误发生", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return "0";
}
                }

               broker.AssignNormalValue("DomBody", dombody);

               //测试结果
               //domhead.save(@"d:\2xml");
               //dombody.save(@"d:\3.xm.l");

               //第六步：调用API
               if (!broker.Invoke())
               {
                   //错误处理
                   Exception apiEx = broker.GetException();
                   if (apiEx != null)
                   {
                       if (apiEx is MomSysException)
                       {
                           MomSysException sysEx = apiEx as MomSysException;
                           MessageBox.Show("系统异常：" + sysEx.Message);
                           //todo:异常处理
                       }
                       else if (apiEx is MomBizException)
                       {
                           MomBizException bizEx = apiEx as MomBizException;
                           MessageBox.Show("API异常：" + bizEx.Message);
                           //todo:异常处理
                       }
                       //异常原因
                       String exReason = broker.GetExceptionString();
                       if (exReason.Length != 0)
                       {
                           MessageBox.Show("异常原因：" + exReason);
                       }
                   }
                   //结束本次调用，释放API资源
                   broker.Release();
                   return "0";
               }

               //第七步：获取返回结果

               //获取返回值
               //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
               System.Boolean result = Convert.ToBoolean(broker.GetReturnValue());

               if (result == false)
               {
                   //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                   System.String errMsgRet = broker.GetResult("errMsg") as System.String;
                   if (errMsgRet != null)
                       MessageBox.Show(errMsgRet.ToString(), "错误提示1！");
                   return "2";
                   //MessageBox.Show(result.ToString(), "导入成功提示");
               }
               ////获取out/inout参数值

               ////获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
               //System.String errMsgRet = broker.GetResult("errMsg") as System.String;
               //if (errMsgRet != null)
               //    MessageBox.Show(errMsgRet.ToString(), "错误提示1！");

               ////获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
               System.String VouchIdRet = broker.GetResult("VouchId") as System.String;

               ////获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
               //MSXML2.IXMLDOMDocument2 domMsgRet = (broker.GetResult("domMsg")) as MSXML2.IXMLDOMDocument2;
               //if (domMsgRet != null)
               //    MessageBox.Show(domMsgRet.ToString(), "错误提示2！");
               //结束本次调用，释放API资源
               broker.Release();
               return VouchIdRet;
           }
    }
}
