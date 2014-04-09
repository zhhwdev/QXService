using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace WEB_Backgroundservice
{
    /// <summary>
    /// 环境实况数据结构 ------------------------ 一切皆为字符串
    /// </summary>
    public class HJDataSK
    {
        public string date;
        public string time;
        public string jid;
        public string name;
        public string px;
        public string py;
        public string s1p;
        public string n1p;
        public string p1p;
        public string s3p;
        public string n3p;
        public string p3p;
        public string s6p;
        public string n6p;
        public string p6p;
        public string s12p;
        public string n12p;
        public string p12p;
        public string s24p;
        public string n24p;
        public string p24p;
    }

    public class HJDataRead
    {
        public HJDataRead(){}

        /// <summary>
        /// 读取实况环境数据
        /// </summary>
        /// <param name="strPath"></param>
        public List<HJDataSK> ReadHJData_SK(string strPath, int startTime, int endTime)
        {
            List<HJDataSK> list = new List<HJDataSK>();
            XmlDocument doc = new XmlDocument();
            doc.Load(strPath);
            XmlNode xn = doc.SelectSingleNode("points");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xnf in xnl)
            {
                XmlElement xe = (XmlElement)xnf;
                HJDataSK info = new HJDataSK();
                string hour = xe.GetAttribute("time");
                int ix = hour.LastIndexOf(":");
                double sj = Convert.ToDouble(hour.Substring(0, ix));

                if (sj >= startTime && sj <= endTime)
                {
                    info.jid = xe.GetAttribute("jid");
                    info.date = xe.GetAttribute("date");
                    info.time = xe.GetAttribute("time");
                    info.name = xe.GetAttribute("name");
                    info.px = xe.GetAttribute("px");
                    info.py = xe.GetAttribute("py");
                    info.s1p = xe.GetAttribute("s1p");
                    info.n1p = xe.GetAttribute("n1p");
                    info.p1p = xe.GetAttribute("p1p");
                    info.s3p = xe.GetAttribute("s3p");
                    info.n3p = xe.GetAttribute("n3p");
                    info.p3p = xe.GetAttribute("p3p");
                    info.s6p = xe.GetAttribute("s6p");
                    info.n6p = xe.GetAttribute("n6p");
                    info.p6p = xe.GetAttribute("p6p");
                    info.s12p = xe.GetAttribute("s12p");
                    info.n12p = xe.GetAttribute("n12p");
                    info.p12p = xe.GetAttribute("p12p");
                    info.s24p = xe.GetAttribute("s24p");
                    info.n1p = xe.GetAttribute("n24p");
                    info.p24p = xe.GetAttribute("p24p");
                    list.Add(info);
                }
            }
            return list;
        }
    }
}