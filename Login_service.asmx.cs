using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Data.OleDb;
//using System.Text.RegularExpressions;
//using MySql.Data.MySqlClient;
namespace WEB_Backgroundservice
{
    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class Login_service : System.Web.Services.WebService
    {
        [WebMethod]
        //根据某个时间段查询范围内记录信息
        public List<DatagridInfosk> queryRecord(int flag, string time1, string time2,double sj1,double sj2)
        {
            string path = HttpContext.Current.Server.MapPath("App_data");
            string dirname = path+@"\武汉气象平台xml数据存储demo\渍涝系统";
            if (flag == 0) { dirname += "\\泵站"; }
            else if (flag == 1) { dirname += "\\实况"; }
            else if (flag == 2) { dirname += "\\预报"; }
            int year1 = Convert.ToInt16(time1.Substring(0, 4));
            int month1 = Convert.ToInt16(time1.Substring(5, 2));
            int day1 = Convert.ToInt16(time1.Substring(8, 2));
            int year2 = Convert.ToInt16(time2.Substring(0, 4));
            int month2 = Convert.ToInt16(time2.Substring(5, 2));
            int day2 = Convert.ToInt16(time2.Substring(8, 2));
            int i, j, k;
            ArrayList arrfile = new ArrayList();
            ArrayList exitfile = new ArrayList();
            for (i = year1; i <= year2; i++)
            {
                if (year2 > year1)  //不同年
                {
                    if (i == year1)
                    {
                        for (j = month1; j <= 12; j++)
                        {
                            if (j == month1)
                            {
                                for (k = day1; k <= 31; k++)
                                {
                                    arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml");
                                }
                            }
                            else
                            {
                                for (k = 1; k <= 31; k++)
                                { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                            }
                        }

                    }
                    else if (i == year2)
                    {
                        for (j = 1; j <= month2; j++)
                        {
                            if (j == month2)
                            {
                                for (k = 1; k <= day2; k++)
                                { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                            }
                            else
                            {
                                for (k = 1; k <= 31; k++)
                                { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                            }
                        }
                    }
                    else
                    {
                        for (j = 1; j <= 12; j++)
                        {
                            for (k = 1; k <= 31; k++)
                            { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                        }
                    }
                }
                else if (year1 == year2)  //同年
                {
                    for (j = month1; j <= month2; j++)
                    {
                        if (month1 == month2)
                        {
                            for (k = day1; k <= day2; k++)
                            { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                        }
                        else
                        {
                            if (j == month1)
                            {
                                for (k = day1; k <= 31; k++)
                                { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                            }
                            else if (j == month2)
                            {
                                for (k = 1; k <= day2; k++)
                                { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                            }
                            else
                            {
                                for (k = 1; k <= 31; k++)
                                { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                            }
                        }
                    }
                }
            }

            List<DatagridInfosk> list = new List<DatagridInfosk>();
            for (i = 0; i < arrfile.Count; i++)
            {
                if (File.Exists(dirname + "\\" + arrfile[i]))
                {
                    List<DatagridInfosk> listtemp=new List<DatagridInfosk>();
                    exitfile.Add(dirname + "\\" + arrfile[i]);
                    if (arrfile.Count == 1)
                    {
                        listtemp = readXmlsj(flag, dirname + "\\" + arrfile[i],sj1,sj2);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            listtemp = readXmlsj(flag, dirname + "\\" + arrfile[i],sj1,23);
                        }
                        else if (i == arrfile.Count - 1)
                        {
                            listtemp = readXmlsj(flag, dirname + "\\" + arrfile[i], 0, sj2);
                        }
                        else 
                        {
                            listtemp = readXml(flag, dirname + "\\" + arrfile[i]);
                        }
                    }

                    for (int b = 0; b < listtemp.Count; b++)
                    {

                        DatagridInfosk info = listtemp[b];
                        list.Add(info);
                    }
                }
            }

         
            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
            strConnection += @"Data Source=" + path + "//attribute.mdb";


            OleDbConnection objConnection = new OleDbConnection(strConnection);  //建立连接  
            objConnection.Open();  //打开连接  
            OleDbCommand sqlcmd = new OleDbCommand("delete * from temptablesz", objConnection);  //sql语句  
            int deleteint = sqlcmd.ExecuteNonQuery();
            foreach (var info in list)
            {
                OleDbCommand sqlcmdinsert = new OleDbCommand("insert into temptablesz(zdname,r1p,zdid) values('"+
                    info.name+"',"+info.r1p+","+info.id+")", objConnection);
                int insertint = sqlcmdinsert.ExecuteNonQuery();
            }

            List<DatagridInfosk> result = new List<DatagridInfosk>();

            OleDbCommand sqlTJ = new OleDbCommand("select zdname,zdid,sum(r1p) from temptablesz group by zdname,zdid order by zdid", objConnection);
            OleDbDataReader dr = sqlTJ.ExecuteReader();
            while (dr.Read())
            {
                DatagridInfosk inforesult = new DatagridInfosk();
                inforesult.name = dr[0].ToString();
                inforesult.id = dr[1].ToString();
                inforesult.r1p = dr[2].ToString();
               
                result.Add(inforesult);
            }
            return result ;
        }

        public class DatagridInfosk
        {
            public string id;
            public string name;
            public string r1p;
            public string r3p;
            public string r6p;
            public string r12p;
            public string r24p;

        }

        public class INFO
        {
            public string date;
            public string time;
            public string name;
            public string px;
            public string py;
            public string r1p;
            public string r3p;
            public string r6p;
            public string r12p;
            public string r24p;

            public string g1p;
            public string g3p;
            public string g6p;
            public string g12p;
            public string g24p;


        }

        public class INFOYB
        {
            public string date;
            public string time;
            public string name;
            public string px;
            public string py;
            public string r1f;
            public string r3f;
            public string r6f;
            public string r12f;
            public string r24f;

            public string g1f;
            public string g3f;
            public string g6f;
            public string g12f;
            public string g24f;
        }

        public List<DatagridInfosk> readXmlsj(int index, string xmlfile,double sj1,double sj2)
        {
            List<DatagridInfosk> list = new List<DatagridInfosk>();
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlfile);
            XmlNode xn = doc.SelectSingleNode("points");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xnf in xnl)
            {
                XmlElement xe = (XmlElement)xnf;
                DatagridInfosk info = new DatagridInfosk();
                string hour= xe.GetAttribute("time");
                int ix = hour.LastIndexOf(":");
                double sj=Convert.ToDouble(hour.Substring(0,ix));
                if (sj >= sj1 && sj <= sj2)
                {
                    if (index == 1)
                    {
                        info.id = xe.GetAttribute("zid");
                   //     info.date = xe.GetAttribute("date");
                  //      info.time = xe.GetAttribute("time");
                        info.name = xe.GetAttribute("name");
                 //       info.px = xe.GetAttribute("px");
                 //       info.py = xe.GetAttribute("py");
                        info.r1p = xe.GetAttribute("r1p");
                        info.r3p = xe.GetAttribute("r3p");
                        info.r6p = xe.GetAttribute("r6p");
                        info.r12p = xe.GetAttribute("r12p");
                        info.r24p = xe.GetAttribute("r24p");
                        list.Add(info);
                    }
                    if (index == 2)
                    {
                        info.id = xe.GetAttribute("zid");
                        //info.date = xe.GetAttribute("date");
                        //info.time = xe.GetAttribute("time");
                        info.name = xe.GetAttribute("name");
                        //info.px = xe.GetAttribute("px");
                        //info.py = xe.GetAttribute("py");
                        info.r1p = xe.GetAttribute("r1f");
                        info.r3p = xe.GetAttribute("r3f");
                        info.r6p = xe.GetAttribute("r6f");
                        info.r12p = xe.GetAttribute("r12f");
                        info.r24p = xe.GetAttribute("r24f");
                        list.Add(info);
                    }
                }
            }
            return list;
        }

        public List<DatagridInfosk> readXml(int index, string xmlfile)
        {
            List<DatagridInfosk> list = new List<DatagridInfosk>();
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlfile);
            XmlNode xn = doc.SelectSingleNode("points");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xnf in xnl)
            {
                XmlElement xe = (XmlElement)xnf;
                DatagridInfosk info = new DatagridInfosk();
                if (index == 1)
                {
                    info.id = xe.GetAttribute("zid");
                    //info.date = xe.GetAttribute("date");
                    //info.time = xe.GetAttribute("time");
                    info.name = xe.GetAttribute("name");
                    //info.px = xe.GetAttribute("px");
                    //info.py = xe.GetAttribute("py");
                    info.r1p = xe.GetAttribute("r1p");
                    info.r3p = xe.GetAttribute("r3p");
                    info.r6p = xe.GetAttribute("r6p");
                    info.r12p = xe.GetAttribute("r12p");
                    info.r24p = xe.GetAttribute("r24p");
                    list.Add(info);
                }
                if (index == 2)
                {
                    info.id = xe.GetAttribute("zid");
                    //info.date = xe.GetAttribute("date");
                    //info.time = xe.GetAttribute("time");
                    info.name = xe.GetAttribute("name");
                    //info.px = xe.GetAttribute("px");
                    //info.py = xe.GetAttribute("py");
                    info.r1p = xe.GetAttribute("r1f");
                    info.r3p = xe.GetAttribute("r3f");
                    info.r6p = xe.GetAttribute("r6f");
                    info.r12p = xe.GetAttribute("r12f");
                    info.r24p = xe.GetAttribute("r24f");
                    list.Add(info);
                }
            }
            return list;
        }

        [WebMethod]
        public List<INFO> GetChartzs(int index, string cxmc, string datepath, string cxsj)
        {
            List<INFO> list = new List<INFO>();
            string path = HttpContext.Current.Server.MapPath("App_data");
            XmlDocument docinfo = new XmlDocument();
            if (index == 1)
            {
                docinfo.Load(path + @"\武汉气象平台xml数据存储demo\渍涝系统\实况\" + datepath);
            }
            if (index == 2)
            {
                docinfo.Load(path + @"\武汉气象平台xml数据存储demo\渍涝系统\预报\" + datepath);
            }
               XmlNode xninfo = docinfo.SelectSingleNode("points");
            XmlNodeList xnlinfo = xninfo.ChildNodes;
            foreach (XmlElement el in xnlinfo)
            {

                string date = el.Attributes["date"].InnerText;
                string time = el.Attributes["time"].InnerText;
                string name = el.Attributes["name"].InnerText;
              

                int sjindex = time.LastIndexOf(":");
                string sj = time.Substring(0, sjindex);
                if (sj == cxsj&&name==cxmc)
                {
                    INFO info = new INFO();
                    if (index == 1)
                    {
                        string r1p = el.Attributes["r1p"].InnerText;
                        string r3p = el.Attributes["r3p"].InnerText;
                        string r6p = el.Attributes["r6p"].InnerText;
                        string r12p = el.Attributes["r12p"].InnerText;
                        string r24p = el.Attributes["r24p"].InnerText;
                        info.r1p = r1p;
                        info.r3p = r3p;
                        info.r6p = r6p;
                        info.r12p = r12p;
                        info.r24p = r24p;
                        info.name = name;
                        info.date = date;
                        info.time = time;
                    }
                    if (index == 2)
                    {
                        string r1p = el.Attributes["r1f"].InnerText;
                        string r3p = el.Attributes["r3f"].InnerText;
                        string r6p = el.Attributes["r6f"].InnerText;
                        string r12p = el.Attributes["r12f"].InnerText;
                        string r24p = el.Attributes["r24f"].InnerText;
                        info.r1p = r1p;
                        info.r3p = r3p;
                        info.r6p = r6p;
                        info.r12p = r12p;
                        info.r24p = r24p;
                        info.name = name;
                        info.date = date;
                        info.time = time;
                    }
                    list.Add(info);
                }
            }


            return list;
        }


        [WebMethod]
        public List<INFO> ZLdata(string datepath,string cxsj)
        {
            List<INFO> list = new List<INFO>();

            string path = HttpContext.Current.Server.MapPath("App_data");
            XmlDocument doczsdj = new XmlDocument();
            doczsdj.Load(path + @"\武汉气象平台xml数据存储demo\渍涝系统\cvalue.xml");




            XmlDocument docinfo = new XmlDocument();
            docinfo.Load(path+@"\武汉气象平台xml数据存储demo\渍涝系统\实况\"+datepath);
            XmlNode xninfo = docinfo.SelectSingleNode("points");
            XmlNodeList xnlinfo = xninfo.ChildNodes;
            foreach (XmlElement el in xnlinfo)
            {
                
                string date = el.Attributes["date"].InnerText;
                string time = el.Attributes["time"].InnerText;
                string name = el.Attributes["name"].InnerText;
                string px = el.Attributes["px"].InnerText;
                string py = el.Attributes["py"].InnerText;
                string r1p = el.Attributes["r1p"].InnerText;
                string r3p = el.Attributes["r3p"].InnerText;
                string r6p = el.Attributes["r6p"].InnerText;
                string r12p = el.Attributes["r12p"].InnerText;
                string r24p = el.Attributes["r24p"].InnerText;

                int sjindex = time.LastIndexOf(":");
                string sj = time.Substring(0, sjindex);
                if(sj==cxsj)
                {
                    INFO info = new INFO();
                info.date = date;
                info.time = time;
                info.name = name;
                info.px = px;
                info.py = py;
                info.r1p = r1p;
                info.r3p = r3p;
                info.r6p = r6p;
                info.r12p = r12p;
                info.r24p = r24p;


                XmlNode xnzsdj = doczsdj.SelectSingleNode("points");
                XmlNodeList xnlzsdj = xnzsdj.ChildNodes;
                foreach (XmlElement elzsdj in xnlzsdj)
                {
                    string namezsdj = elzsdj.Attributes["name"].InnerText;

                    if (namezsdj == name)
                    {

                        double r1p1 = Convert.ToDouble(elzsdj.Attributes["r1p1"].InnerText);
                        double r1p2 = Convert.ToDouble(elzsdj.Attributes["r1p2"].InnerText);
                        double r1p3 = Convert.ToDouble(elzsdj.Attributes["r1p3"].InnerText);
                        double r1p4 = Convert.ToDouble(elzsdj.Attributes["r1p4"].InnerText);
                        string g1p = "";
                        if (Convert.ToDouble(r3p) >= r1p1)
                        {
                            g1p = "1级";
                        }
                        if (Convert.ToDouble(r1p) >= r1p2 && Convert.ToDouble(r1p) < r1p1)
                        {
                            g1p = "2级";
                        }
                        if (Convert.ToDouble(r1p) >= r1p3 && Convert.ToDouble(r1p) < r1p2)
                        {
                            g1p = "3级";
                        }
                        if (Convert.ToDouble(r1p) >= r1p4 && Convert.ToDouble(r1p) < r1p3)
                        {
                            g1p = "4级";
                        }
                        if (Convert.ToDouble(r1p) < r1p4)
                        {
                            g1p = "5级";
                        }
                        info.g1p = g1p;

                        double r3p1 = Convert.ToDouble(elzsdj.Attributes["r3p1"].InnerText);
                        double r3p2 = Convert.ToDouble(elzsdj.Attributes["r3p2"].InnerText);
                        double r3p3 = Convert.ToDouble(elzsdj.Attributes["r3p3"].InnerText);
                        double r3p4 = Convert.ToDouble(elzsdj.Attributes["r3p4"].InnerText);
                        string g3p = "";
                        if (Convert.ToDouble(r3p) >= r3p1)
                        {
                            g3p = "1级";
                        }
                        if (Convert.ToDouble(r3p) >= r3p2 && Convert.ToDouble(r3p) < r3p1)
                        {
                            g3p = "2级";
                        }
                        if (Convert.ToDouble(r3p) >= r3p3 && Convert.ToDouble(r3p) < r3p2)
                        {
                            g3p = "3级";
                        }
                        if (Convert.ToDouble(r3p) >= r3p4 && Convert.ToDouble(r3p) < r3p3)
                        {
                            g3p = "4级";
                        }
                        if (Convert.ToDouble(r3p) < r3p4)
                        {
                            g3p = "5级";
                        }
                        info.g3p = g3p;

                        double r6p1 = Convert.ToDouble(elzsdj.Attributes["r6p1"].InnerText);
                        double r6p2 = Convert.ToDouble(elzsdj.Attributes["r6p2"].InnerText);
                        double r6p3 = Convert.ToDouble(elzsdj.Attributes["r6p3"].InnerText);
                        double r6p4 = Convert.ToDouble(elzsdj.Attributes["r6p4"].InnerText);
                        string g6p = "";
                        if (Convert.ToDouble(r6p) >= r6p1)
                        {
                            g6p = "1级";
                        }
                        if (Convert.ToDouble(r6p) >= r6p2 && Convert.ToDouble(r6p) < r6p1)
                        {
                            g6p = "2级";
                        }
                        if (Convert.ToDouble(r6p) >= r6p3 && Convert.ToDouble(r6p) < r6p2)
                        {
                            g6p = "3级";
                        }
                        if (Convert.ToDouble(r6p) >= r6p4 && Convert.ToDouble(r6p) < r6p3)
                        {
                            g6p = "4级";
                        }
                        if (Convert.ToDouble(r6p) < r6p4)
                        {
                            g6p = "5级";
                        }
                        info.g6p = g6p;

                        double r12p1 = Convert.ToDouble(elzsdj.Attributes["r12p1"].InnerText);
                        double r12p2 = Convert.ToDouble(elzsdj.Attributes["r12p2"].InnerText);
                        double r12p3 = Convert.ToDouble(elzsdj.Attributes["r12p3"].InnerText);
                        double r12p4 = Convert.ToDouble(elzsdj.Attributes["r12p4"].InnerText);
                        string g12p = "";
                        if (Convert.ToDouble(r12p) >= r12p1)
                        {
                            g12p = "1级";
                        }
                        if (Convert.ToDouble(r12p) >= r12p2 && Convert.ToDouble(r12p) < r12p1)
                        {
                            g12p = "2级";
                        }
                        if (Convert.ToDouble(r12p) >= r12p3 && Convert.ToDouble(r12p) < r12p2)
                        {
                            g12p = "3级";
                        }
                        if (Convert.ToDouble(r12p) >= r12p4 && Convert.ToDouble(r12p) < r12p3)
                        {
                            g12p = "4级";
                        }
                        if (Convert.ToDouble(r12p) < r12p4)
                        {
                            g12p = "5级";
                        }
                        info.g12p = g12p;

                        double r24p1 = Convert.ToDouble(elzsdj.Attributes["r24p1"].InnerText);
                        double r24p2 = Convert.ToDouble(elzsdj.Attributes["r24p2"].InnerText);
                        double r24p3 = Convert.ToDouble(elzsdj.Attributes["r24p3"].InnerText);
                        double r24p4 = Convert.ToDouble(elzsdj.Attributes["r24p4"].InnerText);
                        string g24p = "";
                        if (Convert.ToDouble(r24p) >= r24p1)
                        {
                            g24p = "1级";
                        }
                        if (Convert.ToDouble(r24p) >= r24p2 && Convert.ToDouble(r24p) < r24p1)
                        {
                            g24p = "2级";
                        }
                        if (Convert.ToDouble(r24p) >= r24p3 && Convert.ToDouble(r24p) < r24p2)
                        {
                            g24p = "3级";
                        }
                        if (Convert.ToDouble(r24p) >= r24p4 && Convert.ToDouble(r24p) < r24p3)
                        {
                            g24p = "4级";
                        }
                        if (Convert.ToDouble(r24p) < r24p4)
                        {
                            g24p = "5级";
                        }
                        info.g24p = g24p;

                        break;
                    }
                }

                

                list.Add(info);
            }

            }
            return list;
        }

        [WebMethod]
        public List<INFOYB> YBdata(string datepath, string cxsj)
        {
            List<INFOYB> list = new List<INFOYB>();

            string path = HttpContext.Current.Server.MapPath("App_data");
            XmlDocument doczsdj = new XmlDocument();
            doczsdj.Load(path + @"\武汉气象平台xml数据存储demo\渍涝系统\cvalue.xml");




            XmlDocument docinfo = new XmlDocument();
            docinfo.Load(path + @"\武汉气象平台xml数据存储demo\渍涝系统\预报\"+datepath);
            XmlNode xninfo = docinfo.SelectSingleNode("points");
            XmlNodeList xnlinfo = xninfo.ChildNodes;
            foreach (XmlElement el in xnlinfo)
            {
                
               
                string date = el.Attributes["date"].InnerText;
                string time = el.Attributes["time"].InnerText;
                string name = el.Attributes["name"].InnerText;
                string px = el.Attributes["px"].InnerText;
                string py = el.Attributes["py"].InnerText;
                string r1f = el.Attributes["r1f"].InnerText;
                string r3f = el.Attributes["r3f"].InnerText;
                string r6f = el.Attributes["r6f"].InnerText;
                string r12f = el.Attributes["r12f"].InnerText;
                string r24f = el.Attributes["r24f"].InnerText;


                int sjindex = time.LastIndexOf(":");
                string sj = time.Substring(0, sjindex);
                if (sj == cxsj)
                {
                    INFOYB info = new INFOYB();
                    info.date = date;
                    info.time = time;
                    info.name = name;
                    info.px = px;
                    info.py = py;
                    info.r1f = r1f;
                    info.r3f = r3f;
                    info.r6f = r6f;
                    info.r12f = r12f;
                    info.r24f = r24f;


                    XmlNode xnzsdj = doczsdj.SelectSingleNode("points");
                    XmlNodeList xnlzsdj = xnzsdj.ChildNodes;
                    foreach (XmlElement elzsdj in xnlzsdj)
                    {
                        string namezsdj = elzsdj.Attributes["name"].InnerText;
                        if (namezsdj == name)
                        {

                            double r1p1 = Convert.ToDouble(elzsdj.Attributes["r1p1"].InnerText);
                            double r1p2 = Convert.ToDouble(elzsdj.Attributes["r1p2"].InnerText);
                            double r1p3 = Convert.ToDouble(elzsdj.Attributes["r1p3"].InnerText);
                            double r1p4 = Convert.ToDouble(elzsdj.Attributes["r1p4"].InnerText);
                            string g1p = "";
                            if (Convert.ToDouble(r3f) >= r1p1)
                            {
                                g1p = "1级";
                            }
                            if (Convert.ToDouble(r1f) >= r1p2 && Convert.ToDouble(r1f) < r1p1)
                            {
                                g1p = "2级";
                            }
                            if (Convert.ToDouble(r1f) >= r1p3 && Convert.ToDouble(r1f) < r1p2)
                            {
                                g1p = "3级";
                            }
                            if (Convert.ToDouble(r1f) >= r1p4 && Convert.ToDouble(r1f) < r1p3)
                            {
                                g1p = "4级";
                            }
                            if (Convert.ToDouble(r1f) < r1p4)
                            {
                                g1p = "5级";
                            }
                            info.g1f = g1p;

                            double r3p1 = Convert.ToDouble(elzsdj.Attributes["r3p1"].InnerText);
                            double r3p2 = Convert.ToDouble(elzsdj.Attributes["r3p2"].InnerText);
                            double r3p3 = Convert.ToDouble(elzsdj.Attributes["r3p3"].InnerText);
                            double r3p4 = Convert.ToDouble(elzsdj.Attributes["r3p4"].InnerText);
                            string g3p = "";
                            if (Convert.ToDouble(r3f) >= r3p1)
                            {
                                g3p = "1级";
                            }
                            if (Convert.ToDouble(r3f) >= r3p2 && Convert.ToDouble(r3f) < r3p1)
                            {
                                g3p = "2级";
                            }
                            if (Convert.ToDouble(r3f) >= r3p3 && Convert.ToDouble(r3f) < r3p2)
                            {
                                g3p = "3级";
                            }
                            if (Convert.ToDouble(r3f) >= r3p4 && Convert.ToDouble(r3f) < r3p3)
                            {
                                g3p = "4级";
                            }
                            if (Convert.ToDouble(r3f) < r3p4)
                            {
                                g3p = "5级";
                            }
                            info.g3f = g3p;

                            double r6p1 = Convert.ToDouble(elzsdj.Attributes["r6p1"].InnerText);
                            double r6p2 = Convert.ToDouble(elzsdj.Attributes["r6p2"].InnerText);
                            double r6p3 = Convert.ToDouble(elzsdj.Attributes["r6p3"].InnerText);
                            double r6p4 = Convert.ToDouble(elzsdj.Attributes["r6p4"].InnerText);
                            string g6p = "";
                            if (Convert.ToDouble(r6f) >= r6p1)
                            {
                                g6p = "1级";
                            }
                            if (Convert.ToDouble(r6f) >= r6p2 && Convert.ToDouble(r6f) < r6p1)
                            {
                                g6p = "2级";
                            }
                            if (Convert.ToDouble(r6f) >= r6p3 && Convert.ToDouble(r6f) < r6p2)
                            {
                                g6p = "3级";
                            }
                            if (Convert.ToDouble(r6f) >= r6p4 && Convert.ToDouble(r6f) < r6p3)
                            {
                                g6p = "4级";
                            }
                            if (Convert.ToDouble(r6f) < r6p4)
                            {
                                g6p = "5级";
                            }
                            info.g6f = g6p;

                            double r12p1 = Convert.ToDouble(elzsdj.Attributes["r12p1"].InnerText);
                            double r12p2 = Convert.ToDouble(elzsdj.Attributes["r12p2"].InnerText);
                            double r12p3 = Convert.ToDouble(elzsdj.Attributes["r12p3"].InnerText);
                            double r12p4 = Convert.ToDouble(elzsdj.Attributes["r12p4"].InnerText);
                            string g12p = "";
                            if (Convert.ToDouble(r12f) >= r12p1)
                            {
                                g12p = "1级";
                            }
                            if (Convert.ToDouble(r12f) >= r12p2 && Convert.ToDouble(r12f) < r12p1)
                            {
                                g12p = "2级";
                            }
                            if (Convert.ToDouble(r12f) >= r12p3 && Convert.ToDouble(r12f) < r12p2)
                            {
                                g12p = "3级";
                            }
                            if (Convert.ToDouble(r12f) >= r12p4 && Convert.ToDouble(r12f) < r12p3)
                            {
                                g12p = "4级";
                            }
                            if (Convert.ToDouble(r12f) < r12p4)
                            {
                                g12p = "5级";
                            }
                            info.g12f = g12p;

                            double r24p1 = Convert.ToDouble(elzsdj.Attributes["r24p1"].InnerText);
                            double r24p2 = Convert.ToDouble(elzsdj.Attributes["r24p2"].InnerText);
                            double r24p3 = Convert.ToDouble(elzsdj.Attributes["r24p3"].InnerText);
                            double r24p4 = Convert.ToDouble(elzsdj.Attributes["r24p4"].InnerText);
                            string g24p = "";
                            if (Convert.ToDouble(r24f) >= r24p1)
                            {
                                g24p = "1级";
                            }
                            if (Convert.ToDouble(r24f) >= r24p2 && Convert.ToDouble(r24f) < r24p1)
                            {
                                g24p = "2级";
                            }
                            if (Convert.ToDouble(r24f) >= r24p3 && Convert.ToDouble(r24f) < r24p2)
                            {
                                g24p = "3级";
                            }
                            if (Convert.ToDouble(r24f) >= r24p4 && Convert.ToDouble(r24f) < r24p3)
                            {
                                g24p = "4级";
                            }
                            if (Convert.ToDouble(r24f) < r24p4)
                            {
                                g24p = "5级";
                            }
                            info.g24f = g24p;

                            break;
                        }
                    }

                    list.Add(info);

                }
            }
            return list;
        }

        // 环境数据查询
        [WebMethod]
        public List<HJDataSK> HJQueryRecord(int flag, string time1, string time2, int sj1, int sj2)
        {
            string path = HttpContext.Current.Server.MapPath("App_data");
            string dirname = path + @"\武汉气象平台xml数据存储demo\环境气象";

            if (flag == 0) 
            {
                dirname += "\\空气污染"; 
            }
            else if (flag == 1) 
            { 
                dirname += "\\实况"; 
            }
            else if (flag == 2) 
            { 
                dirname += "\\预报"; 
            }

            DateTime dt1, dt2;
            
            int i, j, k;

            try
            {
                dt1 = DateTime.Parse(time1);
                dt2 = DateTime.Parse(time2);
            }
            catch (ArgumentException e)
            {
                Console.Write(e.Message);
                return null;
            }
            catch (FormatException e)
            {
                Console.Write(e.Message);
                return null;
            }

            int year1 = dt1.Year;
            int month1 = dt1.Month;
            int day1 = dt1.Day;
            int year2 = dt2.Year;
            int month2 = dt2.Month;
            int day2 = dt2.Day;
            ArrayList arrfile = new ArrayList();
            ArrayList exitfile = new ArrayList();
            for (i = year1; i <= year2; i++)
            {
                if (year2 > year1)                  // 处理跨年的情况
                {
                    if (i == year1)
                    {
                        for (j = month1; j <= 12; j++)
                        {
                            if (j == month1)                        // 第一个月特殊处理
                            {
                                for (k = day1; k <= 31; k++)
                                {
                                    arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml");
                                }
                            }
                            else
                            {
                                for (k = 1; k <= 31; k++)
                                { 
                                    arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); 
                                }
                            }
                        }

                    }
                    else if (i == year2)
                    {
                        for (j = 1; j <= month2; j++)
                        {
                            if (j == month2)
                            {
                                for (k = 1; k <= day2; k++)
                                { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                            }
                            else
                            {
                                for (k = 1; k <= 31; k++)
                                { arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); }
                            }
                        }
                    }
                    else
                    {
                        for (j = 1; j <= 12; j++)
                        {
                            for (k = 1; k <= 31; k++)
                            {
                                arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); 
                            }
                        }
                    }
                }
                else if (year1 == year2)  //同年
                {
                    for (j = month1; j <= month2; j++)
                    {
                        if (month1 == month2)                       // 处理同月的情况
                        {
                            for (k = day1; k <= day2; k++)
                            { 
                                arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); 
                            }
                        }
                        else                                        // 处理不同月的情况
                        {
                            if (j == month1)
                            {
                                for (k = day1; k <= 31; k++)
                                { 
                                    arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); 
                                }
                            }
                            else if (j == month2)
                            {
                                for (k = 1; k <= day2; k++)
                                { 
                                    arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); 
                                }
                            }
                            else
                            {
                                for (k = 1; k <= 31; k++)
                                { 
                                    arrfile.Add(i.ToString() + "年\\" + j.ToString() + "月\\" + k.ToString() + ".xml"); 
                                }
                            }
                        }
                    }
                }
            }

            List<HJDataSK> list = new List<HJDataSK>();
            HJDataRead hjRead = new HJDataRead();

            for (i = 0; i < arrfile.Count; i++)
            {
                if (File.Exists(dirname + "\\" + arrfile[i]))
                {
                    List<HJDataSK> listtemp = new List<HJDataSK>();
                    exitfile.Add(dirname + "\\" + arrfile[i]);
                    if (arrfile.Count == 1)
                    {
                        listtemp = hjRead.ReadHJData_SK(dirname + "\\" + arrfile[i], sj1, sj2);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            listtemp = hjRead.ReadHJData_SK(dirname + "\\" + arrfile[i], sj1, 23);
                        }
                        else if (i == arrfile.Count - 1)
                        {
                            listtemp = hjRead.ReadHJData_SK(dirname + "\\" + arrfile[i], 0, sj2);
                        }
                        else
                        {
                            listtemp = hjRead.ReadHJData_SK(dirname + "\\" + arrfile[i], 0, 24);
                        }
                    }

                    for (int b = 0; b < listtemp.Count; b++)
                    {

                        HJDataSK info = listtemp[b];
                        list.Add(info);
                    }
                }
            }

            return list;
            /*
            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
            strConnection += @"Data Source=" + path + "//attribute.mdb";


            OleDbConnection objConnection = new OleDbConnection(strConnection);  //建立连接  
            objConnection.Open();  //打开连接  
            OleDbCommand sqlcmd = new OleDbCommand("delete * from temptablesz", objConnection);  //sql语句  
            int deleteint = sqlcmd.ExecuteNonQuery();
            foreach (var info in list)
            {
                //OleDbCommand sqlcmdinsert = new OleDbCommand("insert into temptablesz(zdname,r1p,zdid) values('" +
                //    info.name + "'," + info.r1p + "," + info.id + ")", objConnection);
                //int insertint = sqlcmdinsert.ExecuteNonQuery();
            }

            List<DatagridInfosk> result = new List<DatagridInfosk>();

            OleDbCommand sqlTJ = new OleDbCommand("select zdname,zdid,sum(r1p) from temptablesz group by zdname,zdid order by zdid", objConnection);
            OleDbDataReader dr = sqlTJ.ExecuteReader();
            while (dr.Read())
            {
                DatagridInfosk inforesult = new DatagridInfosk();
                inforesult.name = dr[0].ToString();
                inforesult.id = dr[1].ToString();
                inforesult.r1p = dr[2].ToString();

                result.Add(inforesult);
            }
            //return result; 
             */
            
        }


    }
}
