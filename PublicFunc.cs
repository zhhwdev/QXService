using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Xml;

namespace WEB_Backgroundservice
{
    public abstract class PublicFunc
    {
        public static ArrayList GetAlltheDay()
        {
            return null;
        }

        #region 实况数据转换

        // 环境实况数据转换
        public static List<WHQXT_HJ_SK> DsToList_HJSK(DataSet ds)
        {
            List<WHQXT_HJ_SK> hjList = new List<WHQXT_HJ_SK>();

            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Rows.Count < 1)
                return null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                WHQXT_HJ_SK hj = new WHQXT_HJ_SK();

                hj.ID = int.Parse(dr["ID"].ToString());
                hj.STID = dr["STID"].ToString();
                hj.DT = DateTime.Parse(dr["DT"].ToString());
                hj.S1P = double.Parse(dr["S1P"].ToString());
                hj.N1P = double.Parse(dr["N1P"].ToString());
                hj.S3P = double.Parse(dr["S3P"].ToString());
                hj.N3P = double.Parse(dr["N3P"].ToString());
                hj.S6P = double.Parse(dr["S6P"].ToString());
                hj.N6P = double.Parse(dr["N6P"].ToString());
                hj.S12P = double.Parse(dr["S12P"].ToString());
                hj.N12P = double.Parse(dr["N12P"].ToString());
                hj.S24P = double.Parse(dr["S24P"].ToString());
                hj.N24P = double.Parse(dr["N24P"].ToString());

                hjList.Add(hj);
            }
            return hjList;
        }

        // 交通实况数据转换
        public static List<WHQXT_JT_SK> DsToList_JTSK(DataSet ds)
        {
            List<WHQXT_JT_SK> jtList = new List<WHQXT_JT_SK>();
            
            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Rows.Count < 1)
                return null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                WHQXT_JT_SK jt = new WHQXT_JT_SK();

                jt.ID = int.Parse(dr["ID"].ToString());
                jt.JID = dr["JID"].ToString();
                jt.STID = dr["STID"].ToString();
                jt.DT = DateTime.Parse(dr["DT"].ToString());
                jt.R1P = double.Parse(dr["R1P"].ToString());
                jt.H1P = double.Parse(dr["H1P"].ToString());
                jt.H1PMAX = double.Parse(dr["H1PMAX"].ToString());
                jt.H1PMIN = double.Parse(dr["H1PMIN"].ToString());
                jt.S1P = double.Parse(dr["S1P"].ToString());
                jt.N1P = double.Parse(dr["N1P"].ToString());
                jt.W1P = double.Parse(dr["W1P"].ToString());
                jt.W1PMAX = double.Parse(dr["W1PMAX"].ToString());
                jt.FX1P = double.Parse(dr["FX1P"].ToString());
                jt.FS1P = double.Parse(dr["FS1P"].ToString());

                jt.R3P = double.Parse(dr["R3P"].ToString());
                jt.H3P = double.Parse(dr["H3P"].ToString());
                jt.H3PMAX = double.Parse(dr["H3PMAX"].ToString());
                jt.H3PMIN = double.Parse(dr["H3PMIN"].ToString());
                jt.S3P = double.Parse(dr["S3P"].ToString());
                jt.N3P = double.Parse(dr["N3P"].ToString());
                jt.W3P = double.Parse(dr["W3P"].ToString());
                jt.W3PMAX = double.Parse(dr["W3PMAX"].ToString());
                jt.FX3P = double.Parse(dr["FX3P"].ToString());
                jt.FS3P = double.Parse(dr["FS3P"].ToString());

                jt.R6P = double.Parse(dr["R6P"].ToString());
                jt.H6P = double.Parse(dr["H6P"].ToString());
                jt.H6PMAX = double.Parse(dr["H6PMAX"].ToString());
                jt.H6PMIN = double.Parse(dr["H6PMIN"].ToString());
                jt.S6P = double.Parse(dr["S6P"].ToString());
                jt.N6P = double.Parse(dr["N6P"].ToString());
                jt.W6P = double.Parse(dr["W6P"].ToString());
                jt.W6PMAX = double.Parse(dr["W6PMAX"].ToString());
                jt.FX6P = double.Parse(dr["FX6P"].ToString());
                jt.FS6P = double.Parse(dr["FS6P"].ToString());

                jt.R12P = double.Parse(dr["R12P"].ToString());
                jt.H12P = double.Parse(dr["H12P"].ToString());
                jt.H12PMAX = double.Parse(dr["H12PMAX"].ToString());
                jt.H12PMIN = double.Parse(dr["H12PMIN"].ToString());
                jt.S12P = double.Parse(dr["S12P"].ToString());
                jt.N12P = double.Parse(dr["N12P"].ToString());
                jt.W12P = double.Parse(dr["W12P"].ToString());
                jt.W12PMAX = double.Parse(dr["W12PMAX"].ToString());
                jt.FX12P = double.Parse(dr["FX12P"].ToString());
                jt.FS12P = double.Parse(dr["FS12P"].ToString());

                jt.R24P = double.Parse(dr["R24P"].ToString());
                jt.H24P = double.Parse(dr["H24P"].ToString());
                jt.H24PMAX = double.Parse(dr["H24PMAX"].ToString());
                jt.H24PMIN = double.Parse(dr["H24PMIN"].ToString());
                jt.S24P = double.Parse(dr["S24P"].ToString());
                jt.N24P = double.Parse(dr["N24P"].ToString());
                jt.W24P = double.Parse(dr["W24P"].ToString());
                jt.W24PMAX = double.Parse(dr["W24PMAX"].ToString());
                jt.FX24P = double.Parse(dr["FX24P"].ToString());
                jt.FS24P = double.Parse(dr["FS24P"].ToString());

                jtList.Add(jt);
            }
            return jtList;
        }

        public static List<ZS_SK_INFO> DsToList_ZSSK(DataSet ds)
        {
            List<ZS_SK_INFO> zsList = new List<ZS_SK_INFO>();

            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Rows.Count < 1)
                return null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ZS_SK_INFO zs = new ZS_SK_INFO();
                zs.ID = int.Parse(dr["ID"].ToString());
                zs.name = dr["NAME"].ToString();
                zs.DT = dr["DT"].ToString();
                zs.r1p = double.Parse(dr["R1P"].ToString());
                zs.r3p = double.Parse(dr["R3P"].ToString());
                zs.r6p = double.Parse(dr["R6P"].ToString());
                zs.r12p = double.Parse(dr["R12P"].ToString());
                zs.r24p = double.Parse(dr["R24P"].ToString());

                zsList.Add(zs);
            }
            return zsList;
        }

        /// <summary>
        /// 热岛实况数据转换
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<WHQXT_RD_SK> DsToList_RDSK(DataSet ds)
        {
            List<WHQXT_RD_SK> rdList = new List<WHQXT_RD_SK>();

            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Rows.Count < 1)
                return null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                WHQXT_RD_SK rd = new WHQXT_RD_SK();
                rd.ID = int.Parse(dr["ID"].ToString());
                rd.STID = dr["STID"].ToString();
                rd.DT = DateTime.Parse(dr["DT"].ToString());
                rd.H1P = double.Parse(dr["H1P"].ToString());
                rd.H1PMAX = double.Parse(dr["H1PMAX"].ToString());
                rd.H1PMIN = double.Parse(dr["H1PMIN"].ToString());
                rd.H3P = double.Parse(dr["H3P"].ToString());
                rd.H3PMAX = double.Parse(dr["H3PMAX"].ToString());
                rd.H3PMIN = double.Parse(dr["H3PMIN"].ToString());
                rd.H6P = double.Parse(dr["H6P"].ToString());
                rd.H6PMAX = double.Parse(dr["H6PMAX"].ToString());
                rd.H6PMIN = double.Parse(dr["H6PMIN"].ToString());
                rd.H12P = double.Parse(dr["H12P"].ToString());
                rd.H12PMAX = double.Parse(dr["H12PMAX"].ToString());
                rd.H12PMIN = double.Parse(dr["H12PMIN"].ToString());
                rd.H24P = double.Parse(dr["H24P"].ToString());
                rd.H24PMAX = double.Parse(dr["H24PMAX"].ToString());
                rd.H24PMIN = double.Parse(dr["H24PMIN"].ToString());

                rdList.Add(rd);
            }
            return rdList;
        }

        public static List<DZ_SK_INFO> DsToList_DZSK(DataSet ds)
        {
            List<DZ_SK_INFO> dzList = new List<DZ_SK_INFO>();

          
            return dzList;
        }

        public static List<SQ_SK_INFO> DsToList_SQSK(DataSet ds)
        {
            List<SQ_SK_INFO> dzList = new List<SQ_SK_INFO>();

            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Rows.Count < 1)
                return null;

            return dzList;
        }

        public static List<WHQXT_KSTJ_SK> DsToList_KSTJSK(DataSet ds)
        {
            List<WHQXT_KSTJ_SK> dzList = new List<WHQXT_KSTJ_SK>();

            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Rows.Count < 1)
                return null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                WHQXT_KSTJ_SK kstj = new WHQXT_KSTJ_SK();
                kstj.ID = int.Parse(dr["ID"].ToString());
                kstj.QID = dr["QID"].ToString();
                kstj.DT = DateTime.Parse(dr["DT"].ToString());

                kstj.S1P = double.Parse(dr["S1P"].ToString());
                kstj.S3P = double.Parse(dr["S3P"].ToString());
                kstj.S6P = double.Parse(dr["S6P"].ToString());
                kstj.S12P = double.Parse(dr["S12P"].ToString());
                kstj.S24P = double.Parse(dr["S24P"].ToString());
            }
            return dzList;
        }
        #endregion

        /// <summary>
        ///  获取级别
        /// </summary>
        /// <param name="val"></param>
        /// <param name="r1p"></param>
        /// <param name="r2p"></param>
        /// <param name="r3p"></param>
        /// <param name="r4p"></param>
        /// <returns></returns>
        public static string GetLevel(double val, string r1p, string r2p, string r3p, string r4p)
        {
            string g1p = "";
            if (val >= Convert.ToDouble(r1p))
            {
                g1p = "1级";
            }
            if (val < Convert.ToDouble(r1p) && val >= Convert.ToDouble(r2p))
            {
                g1p = "2级";
            }
            if (val < Convert.ToDouble(r2p) && val >= Convert.ToDouble(r3p))
            {
                g1p = "3级";
            }
            if (val < Convert.ToDouble(r3p) && val >= Convert.ToDouble(r4p))
            {
                g1p = "4级";
            }
            if (Convert.ToDouble(r4p) > val)
            {
                g1p = "5级";
            }

            return g1p;
        }

        public static string GetLevel(double val, string r1p, string r2p, string r3p, string r4p, out int level)
        {
            string g1p = "";

            level = 5;
            if (val >= Convert.ToDouble(r1p))
            {
                g1p = "1级";
                level = 1;
            }
            if (val < Convert.ToDouble(r1p) && val >= Convert.ToDouble(r2p))
            {
                g1p = "2级";
                level = 2;
            }
            if (val < Convert.ToDouble(r2p) && val >= Convert.ToDouble(r3p))
            {
                g1p = "3级";
                level = 3;
            }
            if (val < Convert.ToDouble(r3p) && val >= Convert.ToDouble(r4p))
            {
                g1p = "4级";
                level = 4;
            }
            if (Convert.ToDouble(r4p) > val)
            {
                g1p = "5级";
                level = 5;
            }

            return g1p;
        }

        public static string GetLevel(double val, string h1, string h2, string h3)
        {
            if (val >= double.Parse(h1))
                return "1级";
            else if (val < double.Parse(h1) && val >= double.Parse(h2))
                return "2级";
            else if (val < double.Parse(h2) && val >= double.Parse(h3))
                return "3级";
            else
                return "4级";
        }

        public static string GetLevel(double val, string r1, string r2, string r3, string r4, out bool bOver)
        {
            string g1p = "";

            bOver = false;
            if (val >= Convert.ToDouble(r1))
                g1p = "1级";
            if (val < Convert.ToDouble(r1) && val >= Convert.ToDouble(r2))
                g1p = "2级";
            if (val < Convert.ToDouble(r2) && val >= Convert.ToDouble(r3))
                g1p = "3级";
            if (val < Convert.ToDouble(r3) && val >= Convert.ToDouble(r4))
                g1p = "4级";
            if (Convert.ToDouble(r4) > val)
            {
                g1p = "5级";
                bOver = true;
            }

            return g1p;
        }

        public static string GetLevel(double val, string r1, string r2, string r3, out bool bOver)
        {
            string g1p = "";

            bOver = false;
            if (val >= Convert.ToDouble(r1))
                g1p = "1级";
            if (val < Convert.ToDouble(r1) && val >= Convert.ToDouble(r2))
                g1p = "2级";
            if (val < Convert.ToDouble(r2) && val >= Convert.ToDouble(r3))
                g1p = "3级";
            if (val < Convert.ToDouble(r3))
            {
                g1p = "4级";
                bOver = true;
            }

            return g1p;
        }

        public static string GetValue(string strVal)
        {
            if (strVal == "999")
                return "0";

            return strVal;
        }

        public static void GetTime(DateTime DateIn, out DateTime dt1, out DateTime dt2)
        {
            string strPath = HttpContext.Current.Server.MapPath("App_data") + "\\YBTime.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(strPath);
            XmlNode xn = doc.SelectSingleNode("yb_time");
            XmlNodeList xnl = xn.ChildNodes;
            string strDt = DateIn.ToString("yyyy-MM-dd");

            XmlElement node = (XmlElement)xnl[0];
            string t1 = node.GetAttribute("time1");
            string t2 = node.GetAttribute("time2");

            DateTime d1 = DateTime.Parse(strDt + " " + t1 +":00:00");                   //  1 点的
            DateTime d2 = DateTime.Parse(strDt + " " + t2 + ":00:00");                  //  13 点

            if (DateTime.Compare(DateIn, d1) < 0)         // 早于
            {
                dt1 = d2.AddDays(-1);
                dt2 = d1.AddDays(-1);
            }
            else if (DateTime.Compare(DateIn, d1) >= 0 && DateTime.Compare(DateIn, d2) < 0)
            {
                dt1 = d1;
                dt2 = d2.AddDays(-1);
            }
            else
            {
                dt1 = d2;
                dt2 = d1;
            }
        }

        public static string GetWLevel(string strVal, out bool bAdd)
        {
            double dVal = double.Parse(strVal) * 1000;
            double wn1 = 500;
            double wn2 = 200;
            double wn3 = 50;
            string gw1;

            bAdd = true;
            if (dVal >= wn1)
            {
                gw1 = "4级";
                bAdd = false;
            }
            else if (dVal < wn1 && dVal >= wn2)
            {
                gw1 = "3级";
            }
            else if (dVal < wn2 && dVal >= wn3)
            {
                gw1 = "2级";
            }
            else
            {
                gw1 = "1级";
            }

            return gw1;
        }

        public static string GetMLevel(string strN, string strS, string strPM, out bool bAdd)
        {
            double dN = double.Parse(strN) * 1000;
            double dS = double.Parse(strS);
            double dPM = double.Parse(strPM);
            string level = "";

            bAdd = true;

            　/*黄色：1）能见度小于3000米且相对湿度小于80%的霾。
　　          （2）能见度小于3000米且相对湿度大于等于80%，PM2.5浓度大于115微克/立方米且小于等于150微克/立方米。
　　          （3）能见度小于5000米，PM2.5浓度大于150微克/立方米且小于等于250微克/立方米。*/
            if (dN<3000 && dS<80)
            {
                level = "3级";
            }
            else if (dN<3000 && dS>=80 && dPM>115 && dPM<=150)
            {
                level = "3级";
            }
            else if(dN < 5000 && dPM>150 && dPM <= 250)
            {
                level = "3级";
            }
            
            /*橙色：
　　          1）能见度小于2000米且相对湿度小于80%的霾。
　　      （2）能见度小于2000米且相对湿度大于等于80%，PM2.5浓度大于150微克/立方米且小于等于250微克/立方米。
　　      （3）能见度小于5000米，PM2.5浓度大于250微克/立方米且小于等于500微克/立方米。*/
            if (dN < 2000 && dS < 80)
            {
                level = "2级";
            }
            else if (dN < 2000 && dS >= 80 && dPM > 150 && dPM <= 250)
            {
                level = "2级";
            }
            else if (dN < 5000 && dPM > 250 && dPM <= 500)
            {
                level = "2级";
            }


            /*红色：1）能见度小于1000米且相对湿度小于80%的霾。
　　          （2）能见度小于1000米且相对湿度大于等于80%，PM2.5浓度大于250微克/立方米且小于等于500微克/立方米。
　　          （3）能见度小于5000米，PM2.5浓度大于500微克/立方米。*/
            if (dN < 1000 && dS < 80)
            {
                level = "1级";
            }
            else if (dN < 1000 && dS >= 80 && dPM > 250 && dPM <= 500)
            {
                level = "1级";
            }
            else if (dN < 5000  && dPM > 500)
            {
                level = "1级";
            }

            else
            {
                bAdd = false;
            }

            return level;
        }
    }
}