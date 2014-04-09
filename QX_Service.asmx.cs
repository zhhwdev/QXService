using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Data;
using NPOI.XWPF.UserModel;
using System.IO;

namespace WEB_Backgroundservice
{
    /// <summary>
    /// QX_Service 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class QX_Service : System.Web.Services.WebService
    {
        #region 时间范围查询
        /// <summary>
        ///  渍水系统查询
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <param name="tm_start"></param>
        /// <param name="tm_end"></param>
        /// <returns></returns>
        [WebMethod]
        public List<INFO> queryRecordZS(int index, string strStart, string strEnd)
        {
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";
            List<INFO> zsList = new List<INFO>();

            if (index == 0)
            {
                strSql += "SELECT sum(TBL1.R1P) AS VALUE, TBL2.NAME AS NAME, TBL1.ZID AS ZID";
                strSql += " FROM scott.whqxt_zs_sk  TBL1,scott.whqxt_zsd  TBL2";
                strSql += " WHERE TBL1.ZID = TBL2.ZID AND ";
                strSql += " (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss'))";
                strSql += " GROUP BY TBL2.NAME,TBL1.ZID order by to_number(TBL1.ZID)";
            }
            else if (index == 1)
            {
                strSql += "SELECT sum(TBL1.R6F) AS VALUE, TBL2.NAME AS NAME, TBL1.ZID AS ZID";
                strSql += " FROM scott.whqxt_zs_yb  TBL1,scott.whqxt_zsd  TBL2";
                strSql += " WHERE TBL1.ZID = TBL2.ZID AND ";
                strSql += " (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss'))";
                strSql += " GROUP BY TBL2.NAME,TBL1.ZID order by to_number(TBL1.ZID)";
            }
            else
                return zsList;

            sbSql.AppendFormat(strSql, strStart, strEnd);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return zsList;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                INFO zs = new INFO();

                zs.id = dr["ZID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.r1p = dr["VALUE"].ToString();
                zsList.Add(zs);
            }

            return zsList;
        }

        /// <summary>
        /// 热岛数据查询（有问题，感觉数据和以前的XML不对应（name））
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        [WebMethod]
        public List<RD_INFO> queryRecordRD(int index, string strStart, string strEnd)
        {
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";
            List<RD_INFO> rdList = new List<RD_INFO>();
            DateTime dt1 = DateTime.Parse(strStart);
            DateTime dt2 = DateTime.Parse(strEnd);

            TimeSpan ts = dt2 - dt1;
            long nHours = Convert.ToInt64(ts.TotalHours);

            if (index == 0)
            {
                strSql += "SELECT sum(TBL1.H1P) AS VALUE, TBL1.STID AS STID,TBL2.NAME AS NAME FROM scott.Whqxt_Rd_Sk  TBL1,scott.Whqxt_St  TBL2 ";
                strSql += " WHERE TBL1.STID=TBL2.STID AND (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += " GROUP BY TBL2.NAME,TBL1.STID order by TBL1.STID";
            }
            else if (index == 1)
            {
                strSql += "SELECT sum(TBL1.H6F) AS VALUE, TBL1.STID AS STID,TBL2.NAME AS NAME FROM scott.Whqxt_Rd_Yb  TBL1,scott.Whqxt_St  TBL2 ";
                strSql += " WHERE TBL1.STID=TBL2.STID AND (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += " GROUP BY TBL2.NAME,TBL1.STID order by TBL1.STID";
            }
            else
                return rdList;

            sbSql.AppendFormat(strSql, strStart, strEnd);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return rdList;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RD_INFO zs = new RD_INFO();

                zs.id = dr["STID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.h1 = (Convert.ToDouble(dr["VALUE"].ToString()) / nHours).ToString("f2");
                rdList.Add(zs);
            }

            return rdList;
        }

        /// <summary>
        /// 交通数据查询（完成了实况）
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        [WebMethod]
        public List<JT_INFO> queryRecordJT(int nIndex, string strStart, string strEnd)
        {
            StringBuilder sbSql = new StringBuilder();
            List<JT_INFO> dzList = new List<JT_INFO>();
            string strSql = "";
            DateTime dt1 = DateTime.Parse(strStart);
            DateTime dt2 = DateTime.Parse(strEnd);

            TimeSpan ts = dt2 - dt1;
            long nHours = Convert.ToInt64(ts.TotalHours);

            if (nIndex == 0)
            {
                strSql += "SELECT sum(TBL1.R1P) AS VALUE1,sum(TBL1.H1P) AS VALUE2,sum(TBL1.FX1P) AS VALUE3, sum(TBL1.FS1P) AS VALUE4, sum(TBL1.S1P) AS VALUE5,";
                strSql += " sum(TBL1.N1P) AS VALUE6,  sum(TBL1.W1P) AS VALUE7, ";
                strSql += " TBL3.NAME AS NAME, TBL3.STID AS STID ";
                strSql += " FROM scott.whqxt_jt_sk  TBL1,scott.whqxt_jtzd  TBL2, scott.whqxt_st  TBL3";
                strSql += " WHERE TBL1.JID = TBL2.JID AND TBL2.STID=TBL3.STID AND ";
                strSql += " (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += " GROUP BY TBL3.NAME,TBL3.STID order by TBL3.STID";
            }
            else if (nIndex == 1)
            {
                strSql += "SELECT sum(TBL1.R6F) AS VALUE1,sum(TBL1.H6F) AS VALUE2,sum(TBL1.FX1F) AS VALUE3, sum(TBL1.FS6F) AS VALUE4, sum(TBL1.S6F) AS VALUE5,";
                strSql += " sum(TBL1.N6F) AS VALUE6,  sum(TBL1.W6F) AS VALUE7, ";
                strSql += " TBL3.NAME AS NAME, TBL3.STID AS STID ";
                strSql += " FROM scott.whqxt_jt_yb  TBL1,scott.whqxt_jtzd  TBL2, scott.whqxt_st  TBL3";
                strSql += " WHERE TBL1.JID = TBL2.JID AND TBL2.STID=TBL3.STID AND ";
                strSql += " (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += " GROUP BY TBL3.NAME,TBL3.STID order by TBL3.STID";
            }
            else
                return dzList;

            sbSql.AppendFormat(strSql,  strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return dzList;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                JT_INFO zs = new JT_INFO();

                zs.id = dr["STID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.r1 = dr["VALUE1"].ToString();
                zs.h1 = (Convert.ToDouble(dr["VALUE2"].ToString()) / nHours).ToString("f2");
                zs.fx1 = (Convert.ToDouble(dr["VALUE3"].ToString()) / nHours).ToString("f2");
                zs.fs1 = (Convert.ToDouble(dr["VALUE4"].ToString()) / nHours).ToString("f2");
                zs.s1 = (Convert.ToDouble(dr["VALUE5"].ToString()) / nHours).ToString("f2");
                zs.n1 = (Convert.ToDouble(dr["VALUE6"].ToString()) / nHours).ToString("f2");
                zs.w1 = (Convert.ToDouble(dr["VALUE7"].ToString()) / nHours).ToString("f2");
                dzList.Add(zs);
            }
            return dzList;
        }

        /// <summary>
        /// 地质数据查询(完成了实况)
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        [WebMethod]
        public List<INFO> queryRecordDZ(int nIndex, string strStart, string strEnd)
        {
            StringBuilder sbSql = new StringBuilder();
            List<INFO> dzList = new List<INFO>();
            string strSql = "";

            if (nIndex == 1)
            {
                strSql += "SELECT sum(TBL1.R1P) AS VALUE, TBL2.NAME AS NAME, TBL1.DID AS DID ";
                strSql += " FROM scott.whqxt_dz_sk  TBL1,scott.whqxt_dzzhd  TBL2 ";
                strSql += " WHERE TBL1.DID = TBL2.DID AND ";
                strSql += " (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += " GROUP BY TBL2.NAME,TBL1.DID order by to_number(TBL1.DID)";
            }
            else if (nIndex == 2)
            {
                strSql += "SELECT sum(TBL1.R6F) AS VALUE, TBL2.NAME AS NAME, TBL1.DID AS DID ";
                strSql += " FROM scott.whqxt_dz_yb  TBL1,scott.whqxt_dzzhd  TBL2 ";
                strSql += " WHERE TBL1.DID = TBL2.DID AND ";
                strSql += " (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += " GROUP BY TBL2.NAME,TBL1.DID order by to_number(TBL1.DID)";
            }
            else
                return dzList;

            sbSql.AppendFormat(strSql, strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return dzList;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                INFO zs = new INFO();

                zs.id = dr["DID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.r1p = dr["VALUE"].ToString();
                dzList.Add(zs);
            }
            return dzList;
        }
        
        /// <summary>
        /// 部分完成(实况数据表每个时段差了一个数据)
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        [WebMethod]
        public List<HJ_INFO> queryRecordHJ(int nIndex, string strStart, string strEnd)
        {
            StringBuilder sbSql = new StringBuilder();
            List<HJ_INFO> dzList = new List<HJ_INFO>();
            string strSql = "";
            DateTime dt1 = DateTime.Parse(strStart);
            DateTime dt2 = DateTime.Parse(strEnd);

            TimeSpan ts = dt2 - dt1;
            long nHours = Convert.ToInt64(ts.TotalHours);

            if (nIndex == 0)
            {
                strSql += "SELECT sum(TBL1.N1P) AS VALUE1,sum(TBL1.S1P) AS VALUE2,sum(TBL1.PM1P) AS VALUE3,  TBL2.NAME AS NAME, TBL1.STID AS STID ";
                strSql += "FROM scott.whqxt_hj_sk  TBL1,scott.whqxt_ST  TBL2 ";
                strSql += "WHERE TBL1.STID = TBL2.STID AND ";
                strSql += "(DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += "GROUP BY TBL2.NAME,TBL1.STID ORDER BY STID ";
            }
            else if (nIndex == 1)
            {
                strSql += "SELECT sum(TBL1.N6F) AS VALUE1,sum(TBL1.S6F) AS VALUE2, TBL2.NAME AS NAME, TBL1.STID AS STID ";
                strSql += "FROM scott.whqxt_hj_yb  TBL1,scott.whqxt_ST  TBL2 ";
                strSql += "WHERE TBL1.STID = TBL2.STID AND ";
                strSql += "(DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += "GROUP BY TBL2.NAME,TBL1.STID ORDER BY STID ";
            }
            else
                return dzList;

            sbSql.AppendFormat(strSql, strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return dzList;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HJ_INFO zs = new HJ_INFO();

                zs.id = dr["STID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.n = (Convert.ToDouble(dr["VALUE1"].ToString()) / nHours).ToString("f2");
                zs.s = (Convert.ToDouble(dr["VALUE2"].ToString()) / nHours).ToString("f2");
                zs.p = (Convert.ToDouble(dr["VALUE3"].ToString()) / nHours).ToString("f2");
                dzList.Add(zs);
            }
            return dzList;
        }

        [WebMethod]
        public List<SQ_INFO> queryRecordSQ(int nIndex, string strStart, string strEnd)
        {
            StringBuilder sbSql = new StringBuilder();
            List<SQ_INFO> dzList = new List<SQ_INFO>();
            string strSql = "";
            DateTime dt1 = DateTime.Parse(strStart);
            DateTime dt2 = DateTime.Parse(strEnd);

            TimeSpan ts = dt2 - dt1;
            long nHours = Convert.ToInt64(ts.TotalHours);

            if (nIndex == 0)                // 实况
            {
                strSql += "SELECT sum(TBL1.R1P) AS VALUE1,sum(TBL1.H1P) AS VALUE2,sum(TBL1.FX1P) AS VALUE3,sum(TBL1.FS1P) AS VALUE4,sum(TBL1.S1P) AS VALUE5, sum(TBL1.H1PMAX) AS VALUE6,sum(TBL1.H1PMIN) AS VALUE7,TBL2.NAME AS NAME, TBL1.SID AS SID";
                strSql += " FROM scott.whqxt_sq_sk  TBL1,scott.WHQXT_SQZD  TBL2 ";
                strSql += " WHERE TBL1.SID = TBL2.SID AND ";
                strSql += " (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += " GROUP BY TBL2.NAME,TBL1.SID ORDER BY to_number(SID)";
            }
            else if (nIndex == 1)           // 预报
            {
                strSql += "SELECT sum(TBL1.R6F) AS VALUE1,sum(TBL1.H6F) AS VALUE2,sum(TBL1.FX6F) AS VALUE3,sum(TBL1.FS6F) AS VALUE4,sum(TBL1.S6F) AS VALUE5, sum(TBL1.H1FMAX) AS VALUE6,sum(TBL1.H1FMIN) AS VALUE7,TBL2.NAME AS NAME, TBL1.SID AS SID";
                strSql += " FROM scott.whqxt_sq_yb  TBL1,scott.WHQXT_SQZD  TBL2 ";
                strSql += " WHERE TBL1.SID = TBL2.SID AND ";
                strSql += " (DT BETWEEN to_date('{0}','yyyy-mm-dd hh24:mi:ss') AND to_date('{1}','yyyy-mm-dd hh24:mi:ss')) ";
                strSql += " GROUP BY TBL2.NAME,TBL1.SID ORDER BY to_number(SID)";
            }
            else
                return dzList;

            sbSql.AppendFormat(strSql, strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return dzList;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SQ_INFO zs = new SQ_INFO();

                zs.id = dr["SID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.r1 = dr["VALUE1"].ToString();
                zs.h1 = (Convert.ToDouble(dr["VALUE2"].ToString()) / nHours).ToString("f2");
                zs.fx1 = (Convert.ToDouble(dr["VALUE3"].ToString()) / nHours).ToString("f2");
                zs.fs1 = (Convert.ToDouble(dr["VALUE4"].ToString()) / nHours).ToString("f2");
                zs.s1 = (Convert.ToDouble(dr["VALUE5"].ToString()) / nHours).ToString("f2");
                zs.h1max = (Convert.ToDouble(dr["VALUE6"].ToString()) / nHours).ToString("f2");
                zs.h1min = (Convert.ToDouble(dr["VALUE7"].ToString()) / nHours).ToString("f2");
                dzList.Add(zs);
            }
            return dzList;
        }


        #endregion

        #region 实况数据查询
 
        /// <summary>
        /// 获取渍水实况          --- 完成
        /// </summary>
        /// <param name="strDate">YYYY-MM-DD HH24-MI-SS</param>
        /// <returns></returns>
        [WebMethod]
        public List<ZS_SK_INFO> GetInfozsSK(string strDate)
        {
            List<ZS_SK_INFO> zsList = new List<ZS_SK_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            strSql += "SELECT TBL1.ZID,TBL1.DT, TBL2.NAME AS NAME, TBL2.X AS X,TBL2.Y AS Y, R1P,R3P,R6P,R12P,R24P,TBL3.* ";
            strSql += " FROM scott.WHQXT_ZS_SK TBL1, scott.WHQXT_ZSD  TBL2,scott.whqxt_cvalue_zs TBL3 ";
            strSql += " WHERE TBL1.ZID=TBL2.ZID and DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss') AND TBL2.ZID=TBL3.ZID ";

            sbSql.AppendFormat(strSql, strDate);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                DateTime dt = DateTime.Parse(strDate);
                StringBuilder sql = new StringBuilder();

                dt.AddHours(-1);
                sql.AppendFormat(strSql, dt.ToString("yyyy-MM-dd HH:mM:ss"));
                ds = DbHelperOra.Query(sql.ToString());

                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return zsList;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ZS_SK_INFO zs = new ZS_SK_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                string R1_1 = dr["R1_1"].ToString();
                string R1_2 = dr["R1_2"].ToString();
                string R1_3 = dr["R1_3"].ToString();
                string R1_4 = dr["R1_4"].ToString();
                string R3_1 = dr["R3_1"].ToString();
                string R3_2 = dr["R3_2"].ToString();
                string R3_3 = dr["R3_3"].ToString();
                string R3_4 = dr["R3_4"].ToString();
                string R6_1 = dr["R6_1"].ToString();
                string R6_2 = dr["R6_2"].ToString();
                string R6_3 = dr["R6_3"].ToString();
                string R6_4 = dr["R6_4"].ToString();
                string R12_1 = dr["R12_1"].ToString();
                string R12_2 = dr["R12_2"].ToString();
                string R12_3 = dr["R12_3"].ToString();
                string R12_4 = dr["R12_4"].ToString();
                string R24_1 = dr["R24_1"].ToString();
                string R24_2 = dr["R24_2"].ToString();
                string R24_3 = dr["R24_3"].ToString();
                string R24_4 = dr["R24_4"].ToString();


                zs.ID = long.Parse(dr["ZID"].ToString());
                zs.name = dr["NAME"].ToString();
                zs.date = dt.ToString("yyyy-MM-dd");
                zs.time = dt.ToString("HH:mm");
                zs.px = dr["X"].ToString();
                zs.py = dr["Y"].ToString();
                zs.r1p = double.Parse(dr["R1P"].ToString());
                zs.r3p = double.Parse(dr["R3P"].ToString());
                zs.r6p = double.Parse(dr["R6P"].ToString());
                zs.r12p = double.Parse(dr["R12P"].ToString());
                zs.r24p = double.Parse(dr["R24P"].ToString());

                zs.g1p = PublicFunc.GetLevel(zs.r1p, R1_1, R1_2, R1_3, R1_4);
                zs.g3p = PublicFunc.GetLevel(zs.r3p, R3_1, R3_2, R3_3, R3_4);
                zs.g6p = PublicFunc.GetLevel(zs.r6p, R6_1, R6_2, R6_3, R6_4);
                zs.g12p = PublicFunc.GetLevel(zs.r12p, R12_1, R12_2, R12_3, R12_4);
                zs.g24p = PublicFunc.GetLevel(zs.r24p, R24_1, R24_2, R24_3, R24_4);

                zsList.Add(zs);
            }

            return zsList;
        }

        /// <summary>
        /// 环境实况
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<HJ_INFO> GetInfohjSK(string strDate)
        {
            List<HJ_INFO> hjInfo = new List<HJ_INFO>();
            string strSql = "";

            // 获取关键数据
            StringBuilder sbSql = new StringBuilder();
            strSql = "SELECT TBL1.*,TBL2.NAME AS NAME, TBL2.X AS X,TBL2.Y AS Y";
            strSql += " FROM scott.WHQXT_HJ_SK TBL1, scott.whqxt_ST  TBL2 ";
            strSql += " WHERE TBL1.STID=TBL2.STID AND DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss') ";

            sbSql.AppendFormat(strSql, strDate);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                DateTime dt = DateTime.Parse(strDate);
                StringBuilder sql = new StringBuilder();

                dt.AddHours(-1);
                sql.AppendFormat(strSql, dt.ToString("yyyy-MM-dd HH:mM:ss"));
                ds = DbHelperOra.Query(sql.ToString());

                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return hjInfo;
            }

            foreach (DataRow d_r in ds.Tables[0].Rows)
            {
                HJ_INFO hj = new HJ_INFO();
                DateTime dt = DateTime.Parse(d_r["DT"].ToString());
                bool bVal;
                hj.id = d_r["STID"].ToString();
                hj.name = d_r["NAME"].ToString();
                hj.date = dt.ToString("yyyy-MM-dd");
                hj.time = dt.ToString("HH:mm");
                hj.px = d_r["X"].ToString();
                hj.py = d_r["Y"].ToString();

                hj.s1 = d_r["S1P"].ToString();
                hj.n1 = d_r["N1P"].ToString();
                hj.pm1 = d_r["PM1P"].ToString();

                hj.s3 = d_r["S3P"].ToString();
                hj.n3 = d_r["N3P"].ToString();
                hj.pm3 = d_r["PM3P"].ToString();

                hj.s6 = d_r["S6P"].ToString();
                hj.n6 = d_r["N6P"].ToString();
                hj.pm6 = d_r["PM6P"].ToString();

                hj.s12 = d_r["S12P"].ToString();
                hj.n12 = d_r["N12P"].ToString();
                hj.pm12 = d_r["PM12P"].ToString();

                hj.s24 = d_r["S24P"].ToString();
                hj.n24 = d_r["N24P"].ToString();
                hj.pm24 = d_r["PM24P"].ToString();

                hj.gw1 = PublicFunc.GetWLevel(hj.n1, out bVal);
                hj.gw3 = PublicFunc.GetWLevel(hj.n3, out bVal);
                hj.gw6 = PublicFunc.GetWLevel(hj.n6, out bVal);
                hj.gw12 = PublicFunc.GetWLevel(hj.n12, out bVal);
                hj.gw24 = PublicFunc.GetWLevel(hj.n24, out bVal);

                hj.gm1 = PublicFunc.GetMLevel(hj.n1, hj.s1, hj.pm1, out bVal);
                hj.gm1 = PublicFunc.GetMLevel(hj.n3, hj.s3, hj.pm3, out bVal);
                hj.gm1 = PublicFunc.GetMLevel(hj.n6, hj.s6, hj.pm6, out bVal);
                hj.gm1 = PublicFunc.GetMLevel(hj.n12, hj.s12, hj.pm12, out bVal);
                hj.gm1 = PublicFunc.GetMLevel(hj.n24, hj.s24, hj.pm24, out bVal);

                hjInfo.Add(hj);
            }

            return hjInfo;
        }

        /// <summary>
        ///  获取交通实况数据
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strHour"></param>
        /// <returns></returns>
        [WebMethod]
        public List<JT_INFO> GetInfojtSK(string strDate)
        {
            List<JT_INFO> zsList = new List<JT_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            strSql += "SELECT TBL1.*,TBL3.NAME AS NAME, TBL3.X AS X,TBL3.Y AS Y, TBL3.STID AS STID";
            strSql += " FROM scott.WHQXT_JT_SK TBL1, scott.WHQXT_JTZD  TBL2, scott.WHQXT_ST TBL3";
            strSql += " WHERE TBL1.JID=TBL2.JID and TBL2.STID=TBL3.STID AND TBL2.STID=TBL3.STID AND ";
            strSql += " DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss')";

            sbSql.AppendFormat(strSql, strDate);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                DateTime dt = DateTime.Parse(strDate);
                StringBuilder sql = new StringBuilder();

                dt.AddHours(-1);
                sql.AppendFormat(strSql, dt.ToString("yyyy-MM-dd HH:mM:ss"));
                ds = DbHelperOra.Query(sql.ToString());

                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return zsList;
            }

            // 社区临界值要单独查询
            strSql = "SELECT * FROM scott.WHQXT_CVALUE_JT";
            DataSet cvDs = DbHelperOra.Query(strSql);

            if (cvDs.Tables.Count < 1 || cvDs.Tables[0].Rows.Count < 1)
                return zsList;

            DataRow dr_cv = cvDs.Tables[0].Rows[0];
            string R1_1 = dr_cv["R1_1"].ToString();
            string R1_2 = dr_cv["R1_2"].ToString();
            string R1_3 = dr_cv["R1_3"].ToString();
            string R1_4 = dr_cv["R1_4"].ToString();
            string R3_1 = dr_cv["R3_1"].ToString();
            string R3_2 = dr_cv["R3_2"].ToString();
            string R3_3 = dr_cv["R3_3"].ToString();
            string R3_4 = dr_cv["R3_4"].ToString();
            string R6_1 = dr_cv["R6_1"].ToString();
            string R6_2 = dr_cv["R6_2"].ToString();
            string R6_3 = dr_cv["R6_3"].ToString();
            string R6_4 = dr_cv["R6_4"].ToString();
            string R12_1 = dr_cv["R12_1"].ToString();
            string R12_2 = dr_cv["R12_2"].ToString();
            string R12_3 = dr_cv["R12_3"].ToString();
            string R12_4 = dr_cv["R12_4"].ToString();
            string R24_1 = dr_cv["R24_1"].ToString();
            string R24_2 = dr_cv["R24_2"].ToString();
            string R24_3 = dr_cv["R24_3"].ToString();
            string R24_4 = dr_cv["R24_4"].ToString();
            string R48_1 = dr_cv["R48_1"].ToString();
            string R48_2 = dr_cv["R48_2"].ToString();
            string R48_3 = dr_cv["R48_3"].ToString();
            string R48_4 = dr_cv["R48_4"].ToString();

            string h1 = dr_cv["H_1"].ToString();
            string h2 = dr_cv["H_2"].ToString();
            string h3 = dr_cv["H_3"].ToString();
            /*
            string N1_1 = dr_cv["N1_1"].ToString();
            string N1_2 = dr_cv["N1_2"].ToString();
            string N1_3 = dr_cv["N1_3"].ToString();
            string N1_4 = dr_cv["N1_4"].ToString();
            string N3_1 = dr_cv["N3_1"].ToString();
            string N3_2 = dr_cv["N3_2"].ToString();
            string N3_3 = dr_cv["N3_3"].ToString();
            string N3_4 = dr_cv["N3_4"].ToString();
            string N6_1 = dr_cv["N6_1"].ToString();
            string N6_2 = dr_cv["N6_2"].ToString();
            string N6_3 = dr_cv["N6_3"].ToString();
            string N6_4 = dr_cv["N6_4"].ToString();
            string N12_1 = dr_cv["N12_1"].ToString();
            string N12_2 = dr_cv["N12_2"].ToString();
            string N12_3 = dr_cv["N12_3"].ToString();
            string N12_4 = dr_cv["N12_4"].ToString();
            string N24_1 = dr_cv["N24_1"].ToString();
            string N24_2 = dr_cv["N24_2"].ToString();
            string N24_3 = dr_cv["N24_3"].ToString();
            string N24_4 = dr_cv["N24_4"].ToString();
            string N48_1 = dr_cv["N48_1"].ToString();
            string N48_2 = dr_cv["N48_2"].ToString();
            string N48_3 = dr_cv["N48_3"].ToString();
            string N48_4 = dr_cv["N48_4"].ToString();
            */
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                JT_INFO zs = new JT_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                zs.id = dr["STID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.date = dt.ToString("yyyy-MM-dd");
                zs.time = dt.ToString("HH:mm");
                zs.px = dr["X"].ToString();
                zs.py = dr["Y"].ToString();

                zs.r1 = dr["R1P"].ToString();
                zs.h1 = dr["H1P"].ToString();
                zs.s1 = dr["S1P"].ToString();
                zs.fx1 = dr["FX1P"].ToString();
                zs.fs1 = dr["FS1P"].ToString();
                zs.n1 = dr["N1P"].ToString();
                zs.w1 = dr["W1P"].ToString();

                zs.r3 = dr["R3P"].ToString();
                zs.h3 = dr["H3P"].ToString();
                zs.s3 = dr["S3P"].ToString();
                zs.fx3 = dr["FX3P"].ToString();
                zs.fs3 = dr["FS3P"].ToString();
                zs.n3 = dr["N3P"].ToString();
                zs.w3 = dr["W3P"].ToString();

                zs.r6 = dr["R6P"].ToString();
                zs.h6 = dr["H6P"].ToString();
                zs.s6 = dr["S6P"].ToString();
                zs.fx6 = dr["FX6P"].ToString();
                zs.fs6 = dr["FS6P"].ToString();
                zs.n6 = dr["N6P"].ToString();
                zs.w6 = dr["W6P"].ToString();

                zs.r12 = dr["R12P"].ToString();
                zs.h12 = dr["H12P"].ToString();
                zs.s12 = dr["S12P"].ToString();
                zs.fx12 = dr["FX12P"].ToString();
                zs.fs12 = dr["FS12P"].ToString();
                zs.n12 = dr["N12P"].ToString();
                zs.w12 = dr["W12P"].ToString();

                zs.r24 = dr["R24P"].ToString();
                zs.h24 = dr["H24P"].ToString();
                zs.s24 = dr["S24P"].ToString();
                zs.fx24 = dr["FX24P"].ToString();
                zs.fs24 = dr["FS24P"].ToString();
                zs.n24 = dr["N24P"].ToString();
                zs.w24 = dr["W24P"].ToString();

                zs.gr1 = PublicFunc.GetLevel(double.Parse(zs.r1), R1_1, R1_2, R1_3, R1_4);
                zs.gr3 = PublicFunc.GetLevel(double.Parse(zs.r3), R3_1, R3_2, R3_3, R3_4);
                zs.gr6 = PublicFunc.GetLevel(double.Parse(zs.r6), R6_1, R6_2, R6_3, R6_4);
                zs.gr12 = PublicFunc.GetLevel(double.Parse(zs.r12), R12_1, R12_2, R12_3, R12_4);
                zs.gr24 = PublicFunc.GetLevel(double.Parse(zs.r24), R24_1, R24_2, R24_3, R24_4);
                /*zs.gn1 = PublicFunc.GetLevel(double.Parse(zs.n1), R1_1, R1_2, R1_3, R1_4);
                zs.gn3 = PublicFunc.GetLevel(double.Parse(zs.n3), R3_1, R3_2, R3_3, R3_4);
                zs.gn6 = PublicFunc.GetLevel(double.Parse(zs.n6), R6_1, R6_2, R6_3, R6_4);
                zs.gn12 = PublicFunc.GetLevel(double.Parse(zs.n12), R12_1, R12_2, R12_3, R12_4);
                zs.gn24 = PublicFunc.GetLevel(double.Parse(zs.n24), R24_1, R24_2, R24_3, R24_4);*/
                zs.gt1 = PublicFunc.GetLevel(double.Parse(zs.h1), h1, h2, h3);
                zs.gt3 = PublicFunc.GetLevel(double.Parse(zs.h3), h1, h2, h3);
                zs.gt6 = PublicFunc.GetLevel(double.Parse(zs.h6), h1, h2, h3);
                zs.gt12 = PublicFunc.GetLevel(double.Parse(zs.h12), h1, h2, h3);
                zs.gt24 = PublicFunc.GetLevel(double.Parse(zs.h24), h1, h2, h3);

                zsList.Add(zs);
            }

            return zsList;
        }

        /// <summary>
        /// 获取热岛实况数据
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strHour"></param>
        /// <returns></returns>
        [WebMethod]
        public List<RD_INFO> GetInfordSK(string strDate)
        {
            List<RD_INFO> rdList = new List<RD_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "SELECT TBL1.*,TBL2.NAME,TBL2.X,TBL2.Y FROM scott.WHQXT_RD_SK TBL1,scott.WHQXT_ST TBL2 ";
            strSql += " WHERE TBL1.STID=TBL2.STID AND ";
            strSql += " DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss')";

            sbSql.AppendFormat(strSql, strDate);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                DateTime dt = DateTime.Parse(strDate);
                StringBuilder sql = new StringBuilder();

                dt.AddHours(-1);
                sql.AppendFormat(strSql, dt.ToString("yyyy-MM-dd HH:mM:ss"));
                ds = DbHelperOra.Query(sql.ToString());

                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return rdList;
            }

            strSql = "SELECT * FROM scott.WHQXT_CVALUE_RD";
            DataSet cv_dr = DbHelperOra.Query(strSql);
            if (cv_dr.Tables.Count < 1 || cv_dr.Tables[0].Rows.Count < 1)
                return rdList;

            // 临界值
            string h1 = cv_dr.Tables[0].Rows[0]["H_1"].ToString();
            string h2 = cv_dr.Tables[0].Rows[0]["H_2"].ToString();
            string h3 = cv_dr.Tables[0].Rows[0]["H_3"].ToString();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RD_INFO rd = new RD_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                rd.id = dr["STID"].ToString();
                rd.name = dr["NAME"].ToString();
                rd.date = dt.ToString("yyyy-MM-dd");
                rd.time = dt.ToString("HH:mm");
                rd.px = dr["X"].ToString();
                rd.py = dr["Y"].ToString();
                rd.h1   = dr["H1P"].ToString();
                rd.h1max = dr["H1PMAX"].ToString();
                rd.h1min = dr["H1PMIN"].ToString();
                rd.h3   = dr["H3P"].ToString();
                rd.h3max = dr["H3PMAX"].ToString();
                rd.h3min = dr["H3PMIN"].ToString();
                rd.h6   = dr["H6P"].ToString();
                rd.h6max = dr["H6PMAX"].ToString();
                rd.h6min = dr["H6PMIN"].ToString();
                rd.h12  = dr["H12P"].ToString();
                rd.h12max = dr["H12PMAX"].ToString();
                rd.h12min = dr["H12PMIN"].ToString();
                rd.h24  = dr["H24P"].ToString();
                rd.h24max = dr["H24PMAX"].ToString();
                rd.h24min = dr["H24PMIN"].ToString();

                rd.g1 = PublicFunc.GetLevel(double.Parse(rd.h1), h1, h2, h3);
                rd.g3 = PublicFunc.GetLevel(double.Parse(rd.h3), h1, h2, h3);
                rd.g6 = PublicFunc.GetLevel(double.Parse(rd.h6), h1, h2, h3);
                rd.g12 = PublicFunc.GetLevel(double.Parse(rd.h12), h1, h2, h3);
                rd.g24 = PublicFunc.GetLevel(double.Parse(rd.h24), h1, h2, h3);

                rdList.Add(rd);
            }

            return rdList;
        }

        /// <summary>
        /// 地质数据获取      ----------      完成
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strHour"></param>
        /// <returns></returns>
        [WebMethod]
        public List<DZ_SK_INFO> GetInfodzSK(string strDate)
        {
            List<DZ_SK_INFO> zsList = new List<DZ_SK_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            strSql += "SELECT TBL1.DID,TBL1.DT, TBL2.NAME AS NAME, TBL2.X AS X,TBL2.Y AS Y, R1P,R3P,R6P,R12P,R24P,TBL3.* ";
            strSql += " FROM scott.WHQXT_DZ_SK TBL1, scott.WHQXT_DZZHD  TBL2,scott.whqxt_cvalue_dz TBL3 ";
            strSql += " WHERE TBL1.DID=TBL2.DID and DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss') AND TBL1.DID=TBL3.DID ";

            sbSql.AppendFormat(strSql, strDate);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                DateTime dt = DateTime.Parse(strDate);
                StringBuilder sql = new StringBuilder();

                dt.AddHours(-1);
                sql.AppendFormat(strSql, dt.ToString("yyyy-MM-dd HH:mM:ss"));
                ds = DbHelperOra.Query(sql.ToString());

                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return zsList;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DZ_SK_INFO zs = new DZ_SK_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                string R1_1 = dr["R1_1"].ToString();
                string R1_2 = dr["R1_2"].ToString();
                string R1_3 = dr["R1_3"].ToString();
                string R1_4 = dr["R1_4"].ToString();
                string R3_1 = dr["R3_1"].ToString();
                string R3_2 = dr["R3_2"].ToString();
                string R3_3 = dr["R3_3"].ToString();
                string R3_4 = dr["R3_4"].ToString();
                string R6_1 = dr["R6_1"].ToString();
                string R6_2 = dr["R6_2"].ToString();
                string R6_3 = dr["R6_3"].ToString();
                string R6_4 = dr["R6_4"].ToString();
                string R12_1 = dr["R12_1"].ToString();
                string R12_2 = dr["R12_2"].ToString();
                string R12_3 = dr["R12_3"].ToString();
                string R12_4 = dr["R12_4"].ToString();
                string R24_1 = dr["R24_1"].ToString();
                string R24_2 = dr["R24_2"].ToString();
                string R24_3 = dr["R24_3"].ToString();
                string R24_4 = dr["R24_4"].ToString();


                zs.id = dr["DID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.date = dt.ToString("yyyy-MM-dd");
                zs.time = dt.ToString("HH:mm");
                zs.px = dr["X"].ToString();
                zs.py = dr["Y"].ToString();
                zs.r1p = double.Parse(dr["R1P"].ToString());
                zs.r3p = double.Parse(dr["R3P"].ToString());
                zs.r6p = double.Parse(dr["R6P"].ToString());
                zs.r12p = double.Parse(dr["R12P"].ToString());
                zs.r24p = double.Parse(dr["R24P"].ToString());

                zs.g1p = PublicFunc.GetLevel(zs.r1p, R1_1, R1_2, R1_3, R1_4);
                zs.g3p = PublicFunc.GetLevel(zs.r3p, R3_1, R3_2, R3_3, R3_4);
                zs.g6p = PublicFunc.GetLevel(zs.r6p, R6_1, R6_2, R6_3, R6_4);
                zs.g12p = PublicFunc.GetLevel(zs.r12p, R12_1, R12_2, R12_3, R12_4);
                zs.g24p = PublicFunc.GetLevel(zs.r24p, R24_1, R24_2, R24_3, R24_4);

                zsList.Add(zs);
            }

            return zsList;
        }

        /// <summary>
        /// 获取社区实况数据  -----------
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strHour"></param>
        /// <returns></returns>
        [WebMethod]
        public List<SQ_INFO> GetInfosqSK(string strDate)
        {
            List<SQ_INFO> zsList = new List<SQ_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            strSql += "SELECT TBL1.*,TBL2.NAME AS NAME, TBL2.X AS X,TBL2.Y AS Y";
            strSql += " FROM scott.WHQXT_SQ_SK TBL1, scott.WHQXT_SQZD  TBL2";
            strSql += " WHERE TBL1.SID=TBL2.SID and DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss')";

            sbSql.AppendFormat(strSql, strDate);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                DateTime dt = DateTime.Parse(strDate);
                StringBuilder sql = new StringBuilder();

                dt.AddHours(-1);
                sql.AppendFormat(strSql, dt.ToString("yyyy-MM-dd HH:mM:ss"));
                ds = DbHelperOra.Query(sql.ToString());

                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return zsList;
            }

            // 社区临界值要单独查询
            strSql = "SELECT * FROM scott.WHQXT_CVALUE_SQ";
            DataSet cvDs = DbHelperOra.Query(strSql);

            if (cvDs.Tables.Count < 1 || cvDs.Tables[0].Rows.Count < 1)
                return zsList;

            DataRow dr_cv = cvDs.Tables[0].Rows[0];
            string R1_1 = dr_cv["R1_1"].ToString();
            string R1_2 = dr_cv["R1_2"].ToString();
            string R1_3 = dr_cv["R1_3"].ToString();
            string R1_4 = dr_cv["R1_4"].ToString();
            string R3_1 = dr_cv["R3_1"].ToString();
            string R3_2 = dr_cv["R3_2"].ToString();
            string R3_3 = dr_cv["R3_3"].ToString();
            string R3_4 = dr_cv["R3_4"].ToString();
            string R6_1 = dr_cv["R6_1"].ToString();
            string R6_2 = dr_cv["R6_2"].ToString();
            string R6_3 = dr_cv["R6_3"].ToString();
            string R6_4 = dr_cv["R6_4"].ToString();
            string R12_1 = dr_cv["R12_1"].ToString();
            string R12_2 = dr_cv["R12_2"].ToString();
            string R12_3 = dr_cv["R12_3"].ToString();
            string R12_4 = dr_cv["R12_4"].ToString();
            string R24_1 = dr_cv["R24_1"].ToString();
            string R24_2 = dr_cv["R24_2"].ToString();
            string R24_3 = dr_cv["R24_3"].ToString();
            string R24_4 = dr_cv["R24_4"].ToString();
            string R48_1 = dr_cv["R48_1"].ToString();
            string R48_2 = dr_cv["R48_2"].ToString();
            string R48_3 = dr_cv["R48_3"].ToString();
            string R48_4 = dr_cv["R48_4"].ToString();
            string h1 = dr_cv["H_1"].ToString();
            string h2 = dr_cv["H_2"].ToString();
            string h3 = dr_cv["H_3"].ToString();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SQ_INFO zs = new SQ_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());
            
                zs.id = dr["SID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.date = dt.ToString("yyyy-MM-dd");
                zs.time = dt.ToString("HH:mm");
                zs.px  = dr["X"].ToString();
                zs.py = dr["Y"].ToString();

                zs.r1 = dr["R1P"].ToString();
                zs.h1 = dr["H1P"].ToString();
                zs.s1 = dr["S1P"].ToString();
                zs.fx1 = dr["FX1P"].ToString();
                zs.fs1 = dr["FS1P"].ToString();
                zs.h1min = dr["H1PMIN"].ToString();
                zs.h1max = dr["H1PMAX"].ToString();
                zs.hcode1 = dr["WPCODE1P"].ToString();

                zs.r3 = dr["R3P"].ToString();
                zs.h3 = dr["H3P"].ToString();
                zs.s3 = dr["S3P"].ToString();
                zs.fx3 = dr["FX3P"].ToString();
                zs.fs3 = dr["FS3P"].ToString();
                zs.h3min = dr["H3PMIN"].ToString();
                zs.h3max = dr["H3PMAX"].ToString();
                zs.hcode3 = dr["WPCODE3P"].ToString();

                zs.r6 = dr["R6P"].ToString();
                zs.h6 = dr["H6P"].ToString();
                zs.s6 = dr["S6P"].ToString();
                zs.fx6 = dr["FX6P"].ToString();
                zs.fs6 = dr["FS6P"].ToString();
                zs.h6min = dr["H6PMIN"].ToString();
                zs.h6max = dr["H6PMAX"].ToString();
                zs.hcode6 = dr["WPCODE6P"].ToString();

                zs.r12 = dr["R12P"].ToString();
                zs.h12 = dr["H12P"].ToString();
                zs.s12 = dr["S12P"].ToString();
                zs.fx12 = dr["FX12P"].ToString();
                zs.fs12 = dr["FS12P"].ToString();
                zs.h12min = dr["H12PMIN"].ToString();
                zs.h12max = dr["H12PMAX"].ToString();
                zs.hcode12 = dr["WPCODE12P"].ToString();

                zs.r24 = dr["R24P"].ToString();
                zs.h24 = dr["H24P"].ToString();
                zs.s24 = dr["S24P"].ToString();
                zs.fx24 = dr["FX24P"].ToString();
                zs.fs24 = dr["FS24P"].ToString();
                zs.h24min = dr["H24PMIN"].ToString();
                zs.h24max = dr["H24PMAX"].ToString();
                zs.hcode24 = dr["WPCODE24P"].ToString();

                zs.r48 = dr["R48P"].ToString();
                zs.h48 = dr["H48P"].ToString();
                zs.s48 = dr["S48P"].ToString();
                zs.fx48 = dr["FX48P"].ToString();
                zs.fs48 = dr["FS48P"].ToString();
                zs.h48min = dr["H48PMIN"].ToString();
                zs.h48max = dr["H48PMAX"].ToString();
                zs.hcode48 = dr["WPCODE48P"].ToString();

                zs.g1 = PublicFunc.GetLevel(double.Parse(zs.r1), R1_1, R1_2, R1_3, R1_4);
                zs.g3 = PublicFunc.GetLevel(double.Parse(zs.r3), R3_1, R3_2, R3_3, R3_4);
                zs.g6 = PublicFunc.GetLevel(double.Parse(zs.r6), R6_1, R6_2, R6_3, R6_4);
                zs.g12 = PublicFunc.GetLevel(double.Parse(zs.r12), R12_1, R12_2, R12_3, R12_4);
                zs.g24 = PublicFunc.GetLevel(double.Parse(zs.r24), R24_1, R24_2, R24_3, R24_4);
                zs.g48 = PublicFunc.GetLevel(double.Parse(zs.r48), R48_1, R48_2, R48_3, R48_4);
                zs.gh1 = PublicFunc.GetLevel(double.Parse(zs.h1), h1, h2, h3);
                zs.gh3 = PublicFunc.GetLevel(double.Parse(zs.h3), h1, h2, h3);
                zs.gh6 = PublicFunc.GetLevel(double.Parse(zs.h6), h1, h2, h3);
                zs.gh12 = PublicFunc.GetLevel(double.Parse(zs.h12), h1, h2, h3);
                zs.gh24 = PublicFunc.GetLevel(double.Parse(zs.h24), h1, h2, h3);
                zs.gh48 = PublicFunc.GetLevel(double.Parse(zs.h48), h1, h2, h3);

                zsList.Add(zs);
            }

            return zsList;
        }

        #endregion

        #region 预报数据查询

        /// <summary>
        /// 渍水预报数据          ------------------      完成
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        [WebMethod]
        public List<ZS_YB_INFO> GetInfozsYB(string strDate)
        {
            List<ZS_YB_INFO> zsList = new List<ZS_YB_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";
            DateTime DateIn = DateTime.Parse(strDate);
            DateTime dt1, dt2;

            PublicFunc.GetTime(DateIn, out dt1, out dt2);

            strSql += "SELECT TBL1.*, TBL2.NAME AS NAME, TBL2.X AS X,TBL2.Y AS Y, TBL3.* ";
            strSql += " FROM scott.WHQXT_ZS_YB TBL1, scott.WHQXT_ZSD  TBL2,scott.whqxt_cvalue_zs TBL3 ";
            strSql += " WHERE TBL1.ZID=TBL2.ZID and DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss') AND TBL2.ZID=TBL3.ZID ";

            sbSql.AppendFormat(strSql, dt1.ToString("yyyy-MM-dd HH:mm:ss"));

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendFormat(strSql, dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                ds = DbHelperOra.Query(sbSql.ToString());
                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return zsList;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ZS_YB_INFO zs = new ZS_YB_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                string R3_1 = dr["R3_1"].ToString();
                string R3_2 = dr["R3_2"].ToString();
                string R3_3 = dr["R3_3"].ToString();
                string R3_4 = dr["R3_4"].ToString();
                string R6_1 = dr["R6_1"].ToString();
                string R6_2 = dr["R6_2"].ToString();
                string R6_3 = dr["R6_3"].ToString();
                string R6_4 = dr["R6_4"].ToString();
                string R12_1 = dr["R12_1"].ToString();
                string R12_2 = dr["R12_2"].ToString();
                string R12_3 = dr["R12_3"].ToString();
                string R12_4 = dr["R12_4"].ToString();
                string R24_1 = dr["R24_1"].ToString();
                string R24_2 = dr["R24_2"].ToString();
                string R24_3 = dr["R24_3"].ToString();
                string R24_4 = dr["R24_4"].ToString();
                string R48_1 = dr["R24_1"].ToString();
                string R48_2 = dr["R24_2"].ToString();
                string R48_3 = dr["R24_3"].ToString();
                string R48_4 = dr["R24_4"].ToString();
                string R72_1 = dr["R24_1"].ToString();
                string R72_2 = dr["R24_2"].ToString();
                string R72_3 = dr["R24_3"].ToString();
                string R72_4 = dr["R24_4"].ToString();

                zs.id = long.Parse(dr["ZID"].ToString());
                zs.name = dr["NAME"].ToString();
                zs.date = dt.ToString("yyyy-MM-dd");
                zs.time = dt.ToString("HH:mm");
                zs.px = dr["X"].ToString();
                zs.py = dr["Y"].ToString();

                zs.r6f = double.Parse(dr["R6F"].ToString());
                zs.r12f = double.Parse(dr["R12F"].ToString());
                zs.r24f = double.Parse(dr["R24F"].ToString());
                zs.r48f = double.Parse(dr["R48F"].ToString());
                zs.r72f = double.Parse(dr["R72F"].ToString());

                zs.g6f = PublicFunc.GetLevel(zs.r6f, R6_1, R6_2, R6_3, R6_4);
                zs.g12f = PublicFunc.GetLevel(zs.r12f, R12_1, R12_2, R12_3, R12_4);
                zs.g24f = PublicFunc.GetLevel(zs.r24f, R24_1, R24_2, R24_3, R24_4);
                zs.g48f = PublicFunc.GetLevel(zs.r48f, R48_1, R48_2, R48_3, R48_4);
                zs.g72f = PublicFunc.GetLevel(zs.r72f, R72_1, R72_2, R72_3, R72_4);

                zsList.Add(zs);
            }

            return zsList;
        }

        /// <summary>
        /// 地质灾害预报数据查询              ------------------          完成
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<DZ_YB_INFO> GetInfodzYB(string strDate)
        {
            List<DZ_YB_INFO> zsList = new List<DZ_YB_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";
            DateTime DateIn = DateTime.Parse(strDate);
            DateTime dt1,dt2;

            PublicFunc.GetTime(DateIn, out dt1, out dt2);

           
            strSql += "SELECT TBL1.*, TBL2.NAME AS NAME, TBL2.X AS X,TBL2.Y AS Y, TBL3.* ";
            strSql += " FROM scott.WHQXT_DZ_YB TBL1, scott.WHQXT_DZZHD  TBL2,scott.whqxt_cvalue_dz TBL3 ";
            strSql += " WHERE TBL1.DID=TBL2.DID and DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss') AND TBL2.DID=TBL3.DID ";

            sbSql.AppendFormat(strSql, dt1.ToString("yyyy-MM-dd HH:mm:ss"));

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendFormat(strSql, dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                ds = DbHelperOra.Query(sbSql.ToString());
                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)                
                    return zsList;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DZ_YB_INFO zs = new DZ_YB_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                string R6_1 = dr["R6_1"].ToString();
                string R6_2 = dr["R6_2"].ToString();
                string R6_3 = dr["R6_3"].ToString();
                string R6_4 = dr["R6_4"].ToString();
                string R12_1 = dr["R12_1"].ToString();
                string R12_2 = dr["R12_2"].ToString();
                string R12_3 = dr["R12_3"].ToString();
                string R12_4 = dr["R12_4"].ToString();
                string R24_1 = dr["R24_1"].ToString();
                string R24_2 = dr["R24_2"].ToString();
                string R24_3 = dr["R24_3"].ToString();
                string R24_4 = dr["R24_4"].ToString();
                string R48_1 = dr["R48_1"].ToString();
                string R48_2 = dr["R48_2"].ToString();
                string R48_3 = dr["R48_3"].ToString();
                string R48_4 = dr["R48_4"].ToString();
                string R72_1 = dr["R72_1"].ToString();
                string R72_2 = dr["R72_2"].ToString();
                string R72_3 = dr["R72_3"].ToString();
                string R72_4 = dr["R72_4"].ToString();

                zs.id = dr["DID"].ToString();
                zs.name = dr["NAME"].ToString();

                zs.date = dt.ToString("yyyy-MM-dd");
                zs.time = dt.ToString("HH:mm");
                zs.px = dr["X"].ToString();
                zs.py = dr["Y"].ToString();
                zs.r6f = dr["R6F"].ToString();
                zs.r12f = dr["R12F"].ToString();
                zs.r24f = dr["R24F"].ToString();
                zs.r48f = dr["R48F"].ToString();
                zs.r72f = dr["R72F"].ToString();

                zs.g6f = PublicFunc.GetLevel(double.Parse(zs.r6f), R6_1, R6_2, R6_3, R6_4);
                zs.g12f = PublicFunc.GetLevel(double.Parse(zs.r12f), R12_1, R12_2, R12_3, R12_4);
                zs.g24f = PublicFunc.GetLevel(double.Parse(zs.r24f), R24_1, R24_2, R24_3, R24_4);
                zs.g48f = PublicFunc.GetLevel(double.Parse(zs.r48f), R48_1, R48_2, R48_3, R48_4);
                zs.g72f = PublicFunc.GetLevel(double.Parse(zs.r72f), R72_1, R72_2, R72_3, R72_4);

                zsList.Add(zs);
            }

            return zsList;
        }

        [WebMethod]
        public List<HJ_INFO> GetInfohjYB(string strDate)
        {
            List<HJ_INFO> hjInfo = new List<HJ_INFO>();
            string strSql = "";
            DateTime DateIn = DateTime.Parse(strDate);
            DateTime dt1, dt2;

            PublicFunc.GetTime(DateIn, out dt1, out dt2);
            // 获取关键数据
            StringBuilder sbSql = new StringBuilder();
            strSql = "SELECT TBL1.*,TBL2.NAME AS NAME, TBL2.X AS X,TBL2.Y AS Y";
            strSql += " FROM scott.WHQXT_HJ_YB TBL1, scott.whqxt_ST  TBL2 ";
            strSql += " WHERE TBL1.STID=TBL2.STID AND DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss') ";

            sbSql.AppendFormat(strSql, dt1.ToString("yyyy-MM-dd HH:mm:ss"));
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendFormat(strSql, dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                ds = DbHelperOra.Query(sbSql.ToString());
                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return hjInfo;
            }

            foreach (DataRow d_r in ds.Tables[0].Rows)
            {
                HJ_INFO hj = new HJ_INFO();
                DateTime dt = DateTime.Parse(d_r["DT"].ToString());

                hj.id = d_r["STID"].ToString();
                hj.name = d_r["NAME"].ToString();
                hj.date = dt.ToString("yyyy-MM-dd");
                hj.time = dt.ToString("HH:mm");
                hj.px = d_r["X"].ToString();
                hj.py = d_r["Y"].ToString();

                hj.s1 = d_r["S1F"].ToString();
                hj.n1 = d_r["N1F"].ToString();

                hj.s3 = d_r["S3F"].ToString();
                hj.n3 = d_r["N3F"].ToString();

                hj.s6 = d_r["S6F"].ToString();
                hj.n6 = d_r["N6F"].ToString();

                hj.s12 = d_r["S12F"].ToString();
                hj.n12 = d_r["N12F"].ToString();

                hj.s24 = d_r["S24F"].ToString();
                hj.n24 = d_r["N24F"].ToString();

                hjInfo.Add(hj);
            }

            return hjInfo;
        }

        [WebMethod]
        public List<JT_INFO> GetInfojtYB(string strDate)
        {
            List<JT_INFO> zsList = new List<JT_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";
            DateTime DateIn = DateTime.Parse(strDate);
            DateTime dt1, dt2;

            PublicFunc.GetTime(DateIn, out dt1, out dt2);

            strSql += "SELECT TBL1.*,TBL3.NAME AS NAME, TBL3.X AS X,TBL3.Y AS Y, TBL3.STID AS STID";
            strSql += " FROM scott.WHQXT_JT_YB TBL1, scott.WHQXT_JTZD  TBL2, scott.WHQXT_ST TBL3";
            strSql += " WHERE TBL1.JID=TBL2.JID and TBL2.STID=TBL3.STID AND TBL2.STID=TBL3.STID AND ";
            strSql += " DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss')";

            sbSql.AppendFormat(strSql, dt1.ToString("yyyy-MM-dd HH:mm:ss"));

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendFormat(strSql, dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                ds = DbHelperOra.Query(sbSql.ToString());
                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return zsList;
            }

            // 社区临界值要单独查询
            strSql = "SELECT * FROM scott.WHQXT_CVALUE_JT";
            DataSet cvDs = DbHelperOra.Query(strSql);

            if (cvDs.Tables.Count < 1 || cvDs.Tables[0].Rows.Count < 1)
                return zsList;

            DataRow dr_cv = cvDs.Tables[0].Rows[0];
            string R6_1 = dr_cv["R6_1"].ToString();
            string R6_2 = dr_cv["R6_2"].ToString();
            string R6_3 = dr_cv["R6_3"].ToString();
            string R6_4 = dr_cv["R6_4"].ToString();
            string R12_1 = dr_cv["R12_1"].ToString();
            string R12_2 = dr_cv["R12_2"].ToString();
            string R12_3 = dr_cv["R12_3"].ToString();
            string R12_4 = dr_cv["R12_4"].ToString();
            string R24_1 = dr_cv["R24_1"].ToString();
            string R24_2 = dr_cv["R24_2"].ToString();
            string R24_3 = dr_cv["R24_3"].ToString();
            string R24_4 = dr_cv["R24_4"].ToString();
            string R48_1 = dr_cv["R48_1"].ToString();
            string R48_2 = dr_cv["R48_2"].ToString();
            string R48_3 = dr_cv["R48_3"].ToString();
            string R48_4 = dr_cv["R48_4"].ToString();
            string R72_1 = dr_cv["R72_1"].ToString();
            string R72_2 = dr_cv["R72_2"].ToString();
            string R72_3 = dr_cv["R72_3"].ToString();
            string R72_4 = dr_cv["R72_4"].ToString();

            string h1 = dr_cv["H_1"].ToString();
            string h2 = dr_cv["H_2"].ToString();
            string h3 = dr_cv["H_3"].ToString();

            string n1 = dr_cv["N_1"].ToString();
            string n2 = dr_cv["N_2"].ToString();
            string n3 = dr_cv["N_3"].ToString();
            string n4 = dr_cv["N_4"].ToString();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                JT_INFO zs = new JT_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                zs.id = dr["STID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.date = dt.ToString("yyyy-MM-dd");
                zs.time = dt.ToString("HH:mm");
                zs.px = dr["X"].ToString();
                zs.py = dr["Y"].ToString();

                zs.r6 = dr["R6F"].ToString();
                zs.h6 = dr["H6F"].ToString();
                zs.s6 = dr["S6F"].ToString();
                zs.fx6 = dr["FX6F"].ToString();
                zs.fs6 = dr["FS6F"].ToString();
                zs.n6 = dr["N6F"].ToString();
                zs.w6 = dr["W6F"].ToString();

                zs.r12 = dr["R12F"].ToString();
                zs.h12 = dr["H12F"].ToString();
                zs.s12 = dr["S12F"].ToString();
                zs.fx12 = dr["FX12F"].ToString();
                zs.fs12 = dr["FS12F"].ToString();
                zs.n12 = dr["N12F"].ToString();
                zs.w12 = dr["W12F"].ToString();

                zs.r24 = dr["R24F"].ToString();
                zs.h24 = dr["H24F"].ToString();
                zs.s24 = dr["S24F"].ToString();
                zs.fx24 = dr["FX24F"].ToString();
                zs.fs24 = dr["FS24F"].ToString();
                zs.n24 = dr["N24F"].ToString();
                zs.w24 = dr["W24F"].ToString();

                zs.r48 = dr["R48F"].ToString();
                zs.h48 = dr["H48F"].ToString();
                zs.s48 = dr["S48F"].ToString();
                zs.fx48 = dr["FX48F"].ToString();
                zs.fs48 = dr["FS48F"].ToString();
                zs.n48 = dr["N48F"].ToString();
                zs.w48 = dr["W48F"].ToString();

                zs.r72 = dr["R72F"].ToString();
                zs.h72 = dr["H72F"].ToString();
                zs.s72 = dr["S72F"].ToString();
                zs.fx72 = dr["FX72F"].ToString();
                zs.fs72 = dr["FS72F"].ToString();
                zs.n72 = dr["N72F"].ToString();
                zs.w72 = dr["W72F"].ToString();

                zs.gr6 = PublicFunc.GetLevel(double.Parse(zs.r6), R6_1, R6_2, R6_3, R6_4);
                zs.gr12 = PublicFunc.GetLevel(double.Parse(zs.r12), R12_1, R12_2, R12_3, R12_4);
                zs.gr24 = PublicFunc.GetLevel(double.Parse(zs.r24), R24_1, R24_2, R24_3, R24_4);
                zs.gr48 = PublicFunc.GetLevel(double.Parse(zs.r48), R48_1, R48_2, R48_3, R48_4);
                zs.gr72 = PublicFunc.GetLevel(double.Parse(zs.r72), R72_1, R72_2, R72_3, R72_4);

                zs.gn6 = PublicFunc.GetLevel(double.Parse(zs.n6), n1, n2, n3, n4);
                zs.gn12 = PublicFunc.GetLevel(double.Parse(zs.n12), n1, n2, n3, n4);
                zs.gn24 = PublicFunc.GetLevel(double.Parse(zs.n24), n1, n2, n3, n4);
                zs.gn48 = PublicFunc.GetLevel(double.Parse(zs.n48), n1, n2, n3, n4);
                zs.gn72 = PublicFunc.GetLevel(double.Parse(zs.n72), n1, n2, n3, n4);

                zs.gt6 = PublicFunc.GetLevel(double.Parse(zs.h6), h1, h2, h3);
                zs.gt12 = PublicFunc.GetLevel(double.Parse(zs.h12), h1, h2, h3);
                zs.gt24 = PublicFunc.GetLevel(double.Parse(zs.h24), h1, h2, h3);
                zs.gt48 = PublicFunc.GetLevel(double.Parse(zs.h48), h1, h2, h3);
                zs.gt72 = PublicFunc.GetLevel(double.Parse(zs.h72), h1, h2, h3);

                zsList.Add(zs);
            }

            return zsList;
        }

        [WebMethod]
        public List<RD_INFO> GetInfordYB(string strDate)
        {
            List<RD_INFO> rdList = new List<RD_INFO>();
            StringBuilder sbSql = new StringBuilder();
            DateTime DateIn = DateTime.Parse(strDate);
            DateTime dt1, dt2;

            PublicFunc.GetTime(DateIn, out dt1, out dt2);

            string strSql = "SELECT TBL1.*,TBL2.NAME AS NAME,TBL2.X AS X,TBL2.Y AS Y FROM scott.WHQXT_RD_YB TBL1,scott.WHQXT_ST TBL2 ";
            strSql += " WHERE TBL1.STID=TBL2.STID AND ";
            strSql += " DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss')";

            sbSql.AppendFormat(strSql, dt1.ToString("yyyy-MM-dd HH:mm:ss"));
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendFormat(strSql, dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                ds = DbHelperOra.Query(sbSql.ToString());
                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return rdList;
            }

            strSql = "SELECT * FROM scott.WHQXT_CVALUE_RD";
            DataSet cv_dr = DbHelperOra.Query(strSql);
            if (cv_dr.Tables.Count < 1 || cv_dr.Tables[0].Rows.Count < 1)
                return rdList;

            // 临界值
            string h1 = cv_dr.Tables[0].Rows[0]["H_1"].ToString();
            string h2 = cv_dr.Tables[0].Rows[0]["H_2"].ToString();
            string h3 = cv_dr.Tables[0].Rows[0]["H_3"].ToString();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RD_INFO rd = new RD_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                rd.id = dr["STID"].ToString();
                rd.name = dr["NAME"].ToString();
                rd.date = dt.ToString("yyyy-MM-dd");
                rd.time = dt.ToString("HH:mm");
                rd.px = dr["X"].ToString();
                rd.py = dr["Y"].ToString();
                rd.h6 = dr["H6F"].ToString();
                rd.h6max = dr["H6FMAX"].ToString();
                rd.h6min = dr["H6FMIN"].ToString();
                rd.h12 = dr["H12F"].ToString();
                rd.h12max = dr["H12FMAX"].ToString();
                rd.h12min = dr["H12FMIN"].ToString();
                rd.h24 = dr["H24F"].ToString();
                rd.h24max = dr["H24FMAX"].ToString();
                rd.h24min = dr["H24FMIN"].ToString();
                rd.h48 = dr["H48F"].ToString();
                rd.h48max = dr["H48FMAX"].ToString();
                rd.h48min = dr["H48FMIN"].ToString();
                rd.h72 = dr["H72F"].ToString();
                rd.h72max = dr["H72FMAX"].ToString();
                rd.h72min = dr["H72FMIN"].ToString();

                rd.g6 = PublicFunc.GetLevel(double.Parse(rd.h6), h1, h2, h3);
                rd.g12 = PublicFunc.GetLevel(double.Parse(rd.h12), h1, h2, h3);
                rd.g24 = PublicFunc.GetLevel(double.Parse(rd.h24), h1, h2, h3);
                rd.g48 = PublicFunc.GetLevel(double.Parse(rd.h48), h1, h2, h3);
                rd.g72 = PublicFunc.GetLevel(double.Parse(rd.h72), h1, h2, h3);

                rdList.Add(rd);
            }

            return rdList;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        [WebMethod]
        public List<SQ_INFO> GetInfosqYB(string strDate)
        {
            List<SQ_INFO> zsList = new List<SQ_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";
            DateTime DateIn = DateTime.Parse(strDate);
            DateTime dt1, dt2;

            PublicFunc.GetTime(DateIn, out dt1, out dt2);

            strSql += "SELECT TBL1.*,TBL2.NAME AS NAME, TBL2.X AS X,TBL2.Y AS Y";
            strSql += " FROM scott.WHQXT_SQ_YB TBL1, scott.WHQXT_SQZD  TBL2 ";
            strSql += " WHERE TBL1.SID=TBL2.SID and DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss')";

            sbSql.AppendFormat(strSql, dt1.ToString("yyyy-MM-dd HH:mm:ss"));

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendFormat(strSql, dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                ds = DbHelperOra.Query(sbSql.ToString());
                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    return zsList;
            }

            // 社区临界值要单独查询
            strSql = "SELECT * FROM scott.WHQXT_CVALUE_SQ";
            DataSet cvDs = DbHelperOra.Query(strSql);

            if (cvDs.Tables.Count < 1 || cvDs.Tables[0].Rows.Count < 1)
                return zsList;

            DataRow dr_cv = cvDs.Tables[0].Rows[0];
            string R6_1 = dr_cv["R6_1"].ToString();
            string R6_2 = dr_cv["R6_2"].ToString();
            string R6_3 = dr_cv["R6_3"].ToString();
            string R6_4 = dr_cv["R6_4"].ToString();
            string R12_1 = dr_cv["R12_1"].ToString();
            string R12_2 = dr_cv["R12_2"].ToString();
            string R12_3 = dr_cv["R12_3"].ToString();
            string R12_4 = dr_cv["R12_4"].ToString();
            string R24_1 = dr_cv["R24_1"].ToString();
            string R24_2 = dr_cv["R24_2"].ToString();
            string R24_3 = dr_cv["R24_3"].ToString();
            string R24_4 = dr_cv["R24_4"].ToString();
            string R48_1 = dr_cv["R48_1"].ToString();
            string R48_2 = dr_cv["R48_2"].ToString();
            string R48_3 = dr_cv["R48_3"].ToString();
            string R48_4 = dr_cv["R48_4"].ToString();
            string R72_1 = dr_cv["R72_1"].ToString();
            string R72_2 = dr_cv["R72_2"].ToString();
            string R72_3 = dr_cv["R72_3"].ToString();
            string R72_4 = dr_cv["R72_4"].ToString();
            string h1 = dr_cv["H_1"].ToString();
            string h2 = dr_cv["H_2"].ToString();
            string h3 = dr_cv["H_3"].ToString();


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SQ_INFO zs = new SQ_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                zs.id = dr["SID"].ToString();
                zs.name = dr["NAME"].ToString();
                zs.date = dt.ToString("yyyy-MM-dd");
                zs.time = dt.ToString("HH:mm");
                zs.px = dr["X"].ToString();
                zs.py = dr["Y"].ToString();

                zs.r6 = dr["R6F"].ToString();
                zs.h6 = dr["H6F"].ToString();
                zs.s6 = dr["S6F"].ToString();
                zs.fx6 = dr["FX6F"].ToString();
                zs.fs6 = dr["FS6F"].ToString();
                zs.h6min = dr["H6FMIN"].ToString();
                zs.h6max = dr["H6FMAX"].ToString();
                zs.hcode6 = dr["WPCODE6F"].ToString();

                zs.r12= dr["R12F"].ToString();
                zs.h12 = dr["H12F"].ToString();
                zs.s12 = dr["S12F"].ToString();
                zs.fx12 = dr["FX12F"].ToString();
                zs.fs12 = dr["FS12F"].ToString();
                zs.h12min = dr["H12FMIN"].ToString();
                zs.h12max = dr["H12FMAX"].ToString();
                zs.hcode12 = dr["WPCODE12F"].ToString();

                zs.r24 = dr["R24F"].ToString();
                zs.h24 = dr["H24F"].ToString();
                zs.s24 = dr["S24F"].ToString();
                zs.fx24 = dr["FX24F"].ToString();
                zs.fs24 = dr["FS24F"].ToString();
                zs.h24min = dr["H24FMIN"].ToString();
                zs.h24max = dr["H24FMAX"].ToString();
                zs.hcode24 = dr["WPCODE24F"].ToString();

                zs.r48 = dr["R48F"].ToString();
                zs.h48 = dr["H48F"].ToString();
                zs.s48 = dr["S48F"].ToString();
                zs.fx48 = dr["FX48F"].ToString();
                zs.fs48 = dr["FS48F"].ToString();
                zs.h48min = dr["H48FMIN"].ToString();
                zs.h48max = dr["H48FMAX"].ToString();
                zs.hcode48 = dr["WPCODE48F"].ToString();

                zs.r72 = dr["R72F"].ToString();
                zs.h72 = dr["H72F"].ToString();
                zs.s72 = dr["S72F"].ToString();
                zs.fx72 = dr["FX72F"].ToString();
                zs.fs72 = dr["FS72F"].ToString();
                zs.h6min = dr["H72FMIN"].ToString();
                zs.h6max = dr["H72FMAX"].ToString();
                zs.hcode72 = dr["WPCODE72F"].ToString();

                zs.d1 = dr["DAYTIME1F"].ToString();
                zs.n1 = dr["NIGHT1F"].ToString();
                zs.d2 = dr["DAYTIME2F"].ToString();
                zs.n2 = dr["NIGHT2F"].ToString();
                zs.d3 = dr["DAYTIME3F"].ToString();
                zs.n3 = dr["NIGHT3F"].ToString();

                zs.g6 = PublicFunc.GetLevel(double.Parse(zs.r6), R6_1, R6_2, R6_3, R6_4);
                zs.g12 = PublicFunc.GetLevel(double.Parse(zs.r12), R12_1, R12_2, R12_3, R12_4);
                zs.g24 = PublicFunc.GetLevel(double.Parse(zs.r24), R24_1, R24_2, R24_3, R24_4);
                zs.g48 = PublicFunc.GetLevel(double.Parse(zs.r48), R48_1, R48_2, R48_3, R48_4);
                zs.g72 = PublicFunc.GetLevel(double.Parse(zs.r72), R72_1, R72_2, R72_3, R72_4);

                zs.gh6 = PublicFunc.GetLevel(double.Parse(zs.h6), h1, h2, h3);
                zs.gh12 = PublicFunc.GetLevel(double.Parse(zs.h12), h1, h2, h3);
                zs.gh24 = PublicFunc.GetLevel(double.Parse(zs.h24), h1, h2, h3);
                zs.gh48 = PublicFunc.GetLevel(double.Parse(zs.h48), h1, h2, h3);
                zs.gh72 = PublicFunc.GetLevel(double.Parse(zs.h72), h1, h2, h3);

                zsList.Add(zs);
            }

            return zsList;
        }

        [WebMethod]
        public List<KW_INFO> GetInfokqwr(string strDate)
        {
            List<KW_INFO> kw = new List<KW_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";
            DateTime dt1, dt2;
            DateTime date = DateTime.Parse(strDate);

            PublicFunc.GetTime(date, out dt1, out dt2);

            strSql = "SELECT * FROM scott.whqxt_kstj_sk TBL1, scott.whqxt_ksqy TBL2 WHERE TBL1.QID = TBL2.QID AND";
            strSql += " DT = TO_DATE('{0}','YYYY-MM-DD HH24:mi:ss')";
            sbSql.AppendFormat(strSql, dt1.ToString("yyyy-MM-dd HH:mm:ss"));

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendFormat(strSql, dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                ds = DbHelperOra.Query(sbSql.ToString());
                if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)     
                    return kw;
            }
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                DateTime dt = DateTime.Parse(dr["DT"].ToString());
                KW_INFO info = new KW_INFO();

                info.id = dr["QID"].ToString();
                info.name = dr["NAME"].ToString();
                info.date = dt.ToString("yyyy-MM-dd");
                info.time = dt.ToString("HH:mm");
                info.wd = dr["S1P"].ToString();

                 if (info.wd == "1")
                {
                    info.pj = "好";
                    info.ms = "非常有利于空气污染物稀释、扩散和清除";
                }
                if (info.wd == "2")
                {
                    info.pj = "较好";
                    info.ms = "较有利于空气污染物稀释、扩散和清除";
                }
                if (info.wd == "3")
                {
                    info.pj = "一般";
                    info.ms = "对空气污染物稀释、扩散和清除无明显影响";
                }
                if (info.wd == "4")
                {
                    info.pj = "较差";
                    info.ms = "不利于空气污染物稀释、扩散和清除";
                }
                if (info.wd == "5")
                {
                    info.pj = "差";
                    info.ms = "很不利于空气污染物稀释、扩散和清除";
                }

                kw.Add(info);
            }

            return kw;
        }
        #endregion

        #region 统计数据

        /// <summary>
        /// 渍水数据统计                  ----                完成
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strName"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        [WebMethod]
        public List<INFO> GetChartZS(int index, string strName, string strDate)
        {
            List<INFO> infos = new List<INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            if (index == 0)
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_zs_sk  TBL1,scott.whqxt_zsd  TBL2";
                strSql += " WHERE TBL1.ZID = TBL2.ZID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else if (index == 1)
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME, TBL1.ZID AS ZID";
                strSql += " FROM scott.whqxt_zs_yb  TBL1,scott.whqxt_zsd  TBL2";
                strSql += " WHERE TBL1.ZID = TBL2.ZID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else
                return infos;

            sbSql.AppendFormat(strSql, strDate, strName);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return infos;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                INFO info = new INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                info.id = dr["ZID"].ToString();
                info.date = dt.ToString("yyyy-MM-dd");
                info.time = dt.ToString("HH:mm");
                info.name = dr["NAME"].ToString();

                if (index == 0)
                {
                    info.r1p = dr["R1P"].ToString();
                    info.r3p = dr["R3P"].ToString();
                    info.r6p = dr["R6P"].ToString();
                    info.r12p = dr["R12P"].ToString();
                    info.r24p = dr["R24P"].ToString();
                }
                else
                {
                    info.r1p = dr["R1F"].ToString();
                    info.r3p = dr["R3F"].ToString();
                    info.r6p = dr["R6F"].ToString();
                    info.r12p = dr["R12F"].ToString();
                    info.r24p = dr["R24F"].ToString();
                }
                infos.Add(info);
            }

            return infos;
        }

        [WebMethod]
        public List<INFO> GetChartDZ(int index, string strName, string strDate)
        {
            List<INFO> infos = new List<INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            if (index == 0)
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_dz_sk  TBL1,scott.whqxt_dzzhd  TBL2";
                strSql += " WHERE TBL1.DID = TBL2.DID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else if (index == 1)
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_dz_yb  TBL1,scott.whqxt_dzzhd  TBL2";
                strSql += " WHERE TBL1.DID = TBL2.DID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else
                return infos;

            sbSql.AppendFormat(strSql, strDate, strName);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return infos;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                INFO info = new INFO();

                info.id = dr["DID"].ToString();
                info.date = dr["DT"].ToString();
                info.name = dr["NAME"].ToString();

                if (index == 0)
                {
                    info.r1p = dr["R1P"].ToString();
                    info.r3p = dr["R3P"].ToString();
                    info.r6p = dr["R6P"].ToString();
                    info.r12p = dr["R12P"].ToString();
                    info.r24p = dr["R24P"].ToString();
                }
                else
                {
                    info.r1p = dr["R1F"].ToString();
                    info.r3p = dr["R3F"].ToString();
                    info.r6p = dr["R6F"].ToString();
                    info.r12p = dr["R12F"].ToString();
                    info.r24p = dr["R24F"].ToString();
                }
                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 社区统计                    -------------------             完成
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strName"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        [WebMethod]
        public List<SQ_INFO> GetChartSQ(int index, string strName, string strDate)
        {
            List<SQ_INFO> sqInfos = new List<SQ_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            if (index == 0)                 // 实况
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_sq_sk  TBL1,scott.whqxt_sqzd  TBL2";
                strSql += " WHERE TBL1.SID = TBL2.SID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else if (index == 1)            // 预报
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_sq_yb  TBL1,scott.whqxt_sqzd  TBL2";
                strSql += " WHERE TBL1.SID = TBL2.SID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else
                return sqInfos;

            sbSql.AppendFormat(strSql, strDate, strName);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return sqInfos;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SQ_INFO info = new SQ_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());
                info.id = dr["SID"].ToString();
                info.date = dt.ToString("yyyy-MM-dd");
                info.time = dt.ToString("HH:mm");
                info.name = dr["NAME"].ToString();

                if (index == 0)
                {
                    info.h1 = dr["H1P"].ToString();
                    info.h3 = dr["H3P"].ToString();
                    info.h6 = dr["H6P"].ToString();
                    info.h12 = dr["H12P"].ToString();
                    info.h24 = dr["H24P"].ToString();

                    info.r1 = dr["R1P"].ToString();
                    info.r3 = dr["R3P"].ToString();
                    info.r6 = dr["R6P"].ToString();
                    info.r12 = dr["R12P"].ToString();
                    info.r24 = dr["R24P"].ToString();
                }
                else
                {
                    info.h1 = dr["H1F"].ToString();
                    info.h3 = dr["H3F"].ToString();
                    info.h6 = dr["H6F"].ToString();
                    info.h12 = dr["H12F"].ToString();
                    info.h24 = dr["H24F"].ToString();

                    info.r1 = dr["R1F"].ToString();
                    info.r3 = dr["R3F"].ToString();
                    info.r6 = dr["R6F"].ToString();
                    info.r12 = dr["R12F"].ToString();
                    info.r24 = dr["R24F"].ToString();
                }
                sqInfos.Add(info);
            }
            return sqInfos;
        }

        /// <summary>
        /// 环境统计                   ---------------                  完成
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strName"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        [WebMethod]
        public List<HJ_INFO> GetChartHJ(int index, string strName, string strDate)
        {
            List<HJ_INFO> hjInfos = new List<HJ_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            if (index == 0)
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_hj_sk  TBL1,scott.whqxt_st  TBL2";
                strSql += " WHERE TBL1.STID = TBL2.STID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else if (index == 1)
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_hj_yb  TBL1,scott.whqxt_st  TBL2";
                strSql += " WHERE TBL1.STID = TBL2.STID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else
                return hjInfos;

            sbSql.AppendFormat(strSql, strDate, strName);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return hjInfos;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HJ_INFO info = new HJ_INFO();

                info.id = dr["STID"].ToString();
                info.date = dr["DT"].ToString();
                info.name = dr["NAME"].ToString();

                if (index == 0)
                {
                    info.s1 = dr["S1P"].ToString();
                    info.n1 = dr["N1P"].ToString();
                    info.pm1 = dr["PM1P"].ToString();

                    info.s3 = dr["S3P"].ToString();
                    info.n3 = dr["N3P"].ToString();
                    info.pm3 = dr["PM3P"].ToString();

                    info.s6 = dr["S6P"].ToString();
                    info.n6 = dr["N6P"].ToString();
                    info.p6 = dr["PM6P"].ToString();

                    info.s12 = dr["S12P"].ToString();
                    info.n12 = dr["N12P"].ToString();
                    info.pm12 = dr["PM12P"].ToString();

                    info.s24 = dr["S24P"].ToString();
                    info.n24 = dr["N24P"].ToString();
                    info.pm24 = dr["PM24P"].ToString();

                }
                else
                {
                    info.s1 = dr["S1F"].ToString();
                    info.n1 = dr["N1F"].ToString();
                    //info.p1 = dr["P1P"].ToString();

                    info.s3 = dr["S3F"].ToString();
                    info.n3 = dr["N3F"].ToString();
                    //info.p3 = dr["P3P"].ToString();

                    info.s6 = dr["S6F"].ToString();
                    info.n6 = dr["N6F"].ToString();
                    //info.p6 = dr["P6P"].ToString();

                    info.s12 = dr["S12F"].ToString();
                    info.n12 = dr["N12F"].ToString();
                    //info.p12 = dr["P12P"].ToString();

                    info.s24 = dr["S24F"].ToString();
                    info.n24 = dr["N24F"].ToString();
                    //info.p24 = dr["P24P"].ToString();
                }
                hjInfos.Add(info);
            }

            return hjInfos;
        }

        /// <summary>
        /// 交通统计                   ----------------                 完成
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strName"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        [WebMethod]
        public List<JT_INFO> GetChartJT(int index, string strName, string strDate)
        {
            List<JT_INFO> jtInfos = new List<JT_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            if (index == 0)
            {
                strSql += "SELECT TBL1.*, TBL3.NAME AS NAME, TBL3.STID";
                strSql += " FROM scott.whqxt_jt_sk  TBL1,scott.whqxt_jtzd  TBL2,scott.whqxt_st  TBL3";
                strSql += " WHERE TBL1.JID = TBL2.JID AND TBL2.STID=TBL3.STID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL3.NAME='{1}'";
            }
            else if (index == 1)
            {
                strSql += "SELECT TBL1.*, TBL3.NAME AS NAME, TBL3.STID";
                strSql += " FROM scott.whqxt_jt_yb  TBL1,scott.whqxt_jtzd  TBL2,scott.whqxt_st  TBL3";
                strSql += " WHERE TBL1.JID = TBL2.JID AND TBL2.STID=TBL3.STID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL3.NAME='{1}'";
            }
            else
                return jtInfos;

            sbSql.AppendFormat(strSql, strDate, strName);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return jtInfos;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                JT_INFO info = new JT_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                info.id = dr["STID"].ToString();
                info.date = dt.ToString("yyyy-MM-dd");
                info.time = dt.ToString("HH:mm");
                info.name = dr["NAME"].ToString();

                if (index == 0)
                {
                    info.h1 = dr["H1P"].ToString();
                    info.h3 = dr["H3P"].ToString();
                    info.h6 = dr["H6P"].ToString();
                    info.h12 = dr["H12P"].ToString();
                    info.h24 = dr["H24P"].ToString();

                    info.r1 = dr["R1P"].ToString();
                    info.r3 = dr["R3P"].ToString();
                    info.r6 = dr["R6P"].ToString();
                    info.r12 = dr["R12P"].ToString();
                    info.r24 = dr["R24P"].ToString();

                    info.n1 = dr["N1P"].ToString();
                    info.n3 = dr["N3P"].ToString();
                    info.n6 = dr["N6P"].ToString();
                    info.n12 = dr["N12P"].ToString();
                    info.n24 = dr["N24P"].ToString();
                }
                else
                {
                    info.h1 = dr["H1F"].ToString();
                    info.h3 = dr["H3F"].ToString();
                    info.h6 = dr["H6F"].ToString();
                    info.h12 = dr["H12F"].ToString();
                    info.h24 = dr["H24F"].ToString();

                    info.r1 = dr["R1F"].ToString();
                    info.r3 = dr["R3F"].ToString();
                    info.r6 = dr["R6F"].ToString();
                    info.r12 = dr["R12F"].ToString();
                    info.r24 = dr["R24F"].ToString();

                    info.n1 = dr["N1F"].ToString();
                    info.n3 = dr["N3F"].ToString();
                    info.n6 = dr["N6F"].ToString();
                    info.n12 = dr["N12F"].ToString();
                    info.n24 = dr["N24F"].ToString();
                }
                jtInfos.Add(info);
            }
            return jtInfos;
        }

        /// <summary>
        /// 热岛统计功能完成
        /// </summary>
        /// <param name="index"></param>
        /// <param name="strName"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        [WebMethod]
        public List<RD_INFO> GetChartRD(int index, string strName, string strDate)
        {
            List<RD_INFO> rdInfos = new List<RD_INFO>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            if (index == 0)
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_rd_sk  TBL1,scott.whqxt_st  TBL2";
                strSql += " WHERE TBL1.STID = TBL2.STID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else if (index == 1)
            {
                strSql += "SELECT TBL1.*, TBL2.NAME AS NAME";
                strSql += " FROM scott.whqxt_rd_yb  TBL1,scott.whqxt_st  TBL2";
                strSql += " WHERE TBL1.STID = TBL2.STID AND ";
                strSql += " to_date('{0}','yyyy-mm-dd hh24:mi:ss') = DT AND TBL2.NAME='{1}'";
            }
            else
                return rdInfos;

            sbSql.AppendFormat(strSql, strDate, strName);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return rdInfos;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RD_INFO info = new RD_INFO();
                DateTime dt = DateTime.Parse(dr["DT"].ToString());

                info.id = dr["STID"].ToString();
                info.date = dt.ToString("yyyy-MM-dd");
                info.time = dt.ToString("HH:mm");
                info.name = dr["NAME"].ToString();

                if (index == 0)
                {
                    info.h1 = dr["H1P"].ToString();
                    info.h3 = dr["H3P"].ToString();
                    info.h6 = dr["H6P"].ToString();
                    info.h12 = dr["H12P"].ToString();
                    info.h24 = dr["H24P"].ToString();
                }
                else
                {
                    info.h1 = dr["H1F"].ToString();
                    info.h3 = dr["H3F"].ToString();
                    info.h6 = dr["H6F"].ToString();
                    info.h12 = dr["H12F"].ToString();
                    info.h24 = dr["H24F"].ToString();
                }
                rdInfos.Add(info);
            }
            return rdInfos;
        }

        #endregion

        /// <summary>
        /// 获取交通信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<JTXX_INFO> GetJTXX()
        {
            List<JTXX_INFO> xxInfo = new List<JTXX_INFO>();
            string strSql = "SELECT NAME,X,Y,TYPE FROM scott.WHQXT_JTXX";
            DataSet ds = DbHelperOra.Query(strSql);

            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                JTXX_INFO xx = new JTXX_INFO();

                xx.name = dr["NAME"].ToString();
                xx.x = dr["X"].ToString();
                xx.y = dr["Y"].ToString();
                xx.type = dr["TYPE"].ToString();

                xxInfo.Add(xx);
            }

            return xxInfo;
        }

        /// <summary>
        /// 渍水查询
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [WebMethod]
        public List<ZS_QUR_STRU> GetZSInfoByTime(int nIndex, string strStart, string strEnd, int time)
        {
            List<ZS_QUR_STRU> list  = new List<ZS_QUR_STRU>();
            StringBuilder sbSql     = new StringBuilder();
            string strSql           = "";
            string strTableName     = "";
            string strFld           = "";

            if (nIndex == 0)
            {
                strTableName = "whqxt_zs_sk";
                strFld       = "R" + time.ToString() + "P"; 
            }
            else if (nIndex == 1)
            {
                strTableName = "whqxt_zs_yb";
                strFld       = "R" + time.ToString() + "F"; 
            }
            else
                return list;

            string strLevelFld1     = "R" + time.ToString() + "_1";
            string strLevelFld2     = "R" + time.ToString() + "_2";
            string strLevelFld3     = "R" + time.ToString() + "_3";
            string strLevelFld4     = "R" + time.ToString() + "_4";

            strSql += "SELECT TBL1.ZID AS ZID,TBL1.DT,{0},{1},{2},{3},{4},TBL2.NAME AS NAME ";
            strSql += " FROM   scott.{5} TBL1,scott.whqxt_zsd TBL2,scott.whqxt_cvalue_zs TBL3";
            strSql += " WHERE TBL1.ZID = TBL2.ZID AND TBL3.ZID=TBL1.ZID AND ";
            strSql += "(DT BETWEEN TO_DATE('{6}','yyyy-mm-dd hh24:mi:ss') AND TO_DATE('{7}','yyyy-mm-dd hh24:mi:ss')) ";
            sbSql.AppendFormat(strSql, strFld, strLevelFld1, strLevelFld2, strLevelFld3, strLevelFld4,
                strTableName, strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1)
                return list;

            DataTable dt = ds.Tables[0];
            DataTable dtDate = dt.DefaultView.ToTable(true, "DT");              // 获取所有的时间(Distinct)

            foreach (DataRow dr in dtDate.Rows)
            {
                ZS_QUR_STRU zs = new ZS_QUR_STRU();
                DataRow[] drs = dt.Select("DT='"+ DateTime.Parse(dr["DT"].ToString()) + "'");
                DateTime date = DateTime.Parse(dr["DT"].ToString());

                zs.ValList = new List<ZS_VAL>();
                zs.date = date.ToString("yyyy-MM-dd");
                zs.time = date.ToString("HH:mm");

                foreach (DataRow dr1 in drs)
                {
                    ZS_VAL val = new ZS_VAL();
                    int nLevel = 1;

                    string R1 = dr1[strLevelFld1].ToString();
                    string R2 = dr1[strLevelFld2].ToString();
                    string R3 = dr1[strLevelFld3].ToString();
                    string R4 = dr1[strLevelFld4].ToString();
                    val.id = dr1["ZID"].ToString();
                    val.name = dr1["NAME"].ToString();
                    val.value = dr1[strFld].ToString();
                    val.level = PublicFunc.GetLevel(double.Parse(val.value), R1, R2, R3, R4,out nLevel);
                    if (nLevel > 4)
                        continue;

                    zs.ValList.Add(val);
                }

                list.Add(zs);
            }

            return list;
        }

        [WebMethod]
        public List<JT_QUR_STRU> GetJTInfoByTime(int nIndex, string strStart, string strEnd, int time)
        {
            List<JT_QUR_STRU> list  = new List<JT_QUR_STRU>();
            StringBuilder sbSql     = new StringBuilder();
            string strSql           = "";

            string strTableName     = "";
            string strFld1          = "";
            string strFld2          = "";
            string strFld3          = "";

            if (nIndex == 0)
            {
                strTableName        = "WHQXT_JT_SK";
                strFld1             = "R" + time.ToString() + "P";
                strFld2             = "H" + time.ToString() + "P";
                strFld3             = "N" + time.ToString() + "P";
            }
            else if (nIndex == 1)
            {
                strTableName = "WHQXT_JT_YB";
                strFld1             = "R" + time.ToString() + "H";
                strFld2             = "H" + time.ToString() + "H";
                strFld3             = "N" + time.ToString() + "H";
            }
            else
                return list;

            // 获取临界值
            // 社区临界值要单独查询
            strSql = "SELECT * FROM scott.WHQXT_CVALUE_JT";
            DataSet cvDs = DbHelperOra.Query(strSql);

            if (cvDs.Tables.Count < 1 || cvDs.Tables[0].Rows.Count < 1)
                return list;

            DataRow dr_cv = cvDs.Tables[0].Rows[0];
            string R1_1 = dr_cv["R1_1"].ToString();
            string R1_2 = dr_cv["R1_2"].ToString();
            string R1_3 = dr_cv["R1_3"].ToString();
            string R1_4 = dr_cv["R1_4"].ToString();
            string R3_1 = dr_cv["R3_1"].ToString();
            string R3_2 = dr_cv["R3_2"].ToString();
            string R3_3 = dr_cv["R3_3"].ToString();
            string R3_4 = dr_cv["R3_4"].ToString();
            string R6_1 = dr_cv["R6_1"].ToString();
            string R6_2 = dr_cv["R6_2"].ToString();
            string R6_3 = dr_cv["R6_3"].ToString();
            string R6_4 = dr_cv["R6_4"].ToString();
            string R12_1 = dr_cv["R12_1"].ToString();
            string R12_2 = dr_cv["R12_2"].ToString();
            string R12_3 = dr_cv["R12_3"].ToString();
            string R12_4 = dr_cv["R12_4"].ToString();
            string R24_1 = dr_cv["R24_1"].ToString();
            string R24_2 = dr_cv["R24_2"].ToString();
            string R24_3 = dr_cv["R24_3"].ToString();
            string R24_4 = dr_cv["R24_4"].ToString();
            string R48_1 = dr_cv["R48_1"].ToString();
            string R48_2 = dr_cv["R48_2"].ToString();
            string R48_3 = dr_cv["R48_3"].ToString();
            string R48_4 = dr_cv["R48_4"].ToString();
            string R72_1 = dr_cv["R72_1"].ToString();
            string R72_2 = dr_cv["R72_2"].ToString();
            string R72_3 = dr_cv["R72_3"].ToString();
            string R72_4 = dr_cv["R72_4"].ToString();

            string h1 = dr_cv["H_1"].ToString();
            string h2 = dr_cv["H_2"].ToString();
            string h3 = dr_cv["H_3"].ToString();

            string n1 = dr_cv["N_1"].ToString();
            string n2 = dr_cv["N_2"].ToString();
            string n3 = dr_cv["N_3"].ToString();
            string n4 = dr_cv["N_4"].ToString();


            strSql = "SELECT TBL1.JID AS JID,TBL1.DT,{0},{1},{2},TBL3.NAME AS NAME ";
            strSql += " FROM   scott.{3} TBL1,scott.WHQXT_JTZD TBL2,scott.whqxt_st  TBL3";
            strSql += " WHERE TBL1.JID = TBL2.JID AND TBL2.STID=TBL3.STID AND ";
            strSql += "(DT BETWEEN TO_DATE('{4}','yyyy-mm-dd hh24:mi:ss') AND TO_DATE('{5}','yyyy-mm-dd hh24:mi:ss')) ";
            sbSql.AppendFormat(strSql, strFld1, strFld2, strFld3, strTableName, strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count<1 || ds.Tables[0].Rows.Count<1)
                return list;

            DataTable dt = ds.Tables[0];
            DataTable dtDate = dt.DefaultView.ToTable(true, "DT");              // 获取所有的时间(Distinct)

            foreach (DataRow dr in dtDate.Rows)
            {
                JT_QUR_STRU zs = new JT_QUR_STRU();
                DataRow[] drs = dt.Select("DT='"+ DateTime.Parse(dr["DT"].ToString()) + "'");
                DateTime date = DateTime.Parse(dr["DT"].ToString());

                zs.ValList = new List<JT_VAL>();
                zs.date = date.ToString("yyyy-MM-dd");
                zs.time = date.ToString("HH:mm");

                foreach (DataRow dr1 in drs)
                {
                    JT_VAL val = new JT_VAL();
                    bool bR, bH, bN;

                    val.id = dr1["JID"].ToString();
                    val.name = dr1["NAME"].ToString();
                    val.rval = dr1[strFld1].ToString();
                    val.hval = dr1[strFld2].ToString();
                    val.nval = dr1[strFld3].ToString();

                    switch (time)
                    {
                        case 1:
                            {
                                val.rlevel = PublicFunc.GetLevel(double.Parse(val.rval), R1_1, R1_2, R1_3, R1_4, out bR);
                            }break;
                        case 3:
                            {
                                val.rlevel = PublicFunc.GetLevel(double.Parse(val.rval), R1_1, R1_2, R1_3, R1_4, out bR);
                            } break;
                        case 6:
                            {
                                val.rlevel = PublicFunc.GetLevel(double.Parse(val.rval), R1_1, R1_2, R1_3, R1_4, out bR);
                            } break;
                        case 12:
                            {
                                val.rlevel = PublicFunc.GetLevel(double.Parse(val.rval), R1_1, R1_2, R1_3, R1_4, out bR);
                            } break;
                        case 24:
                            {
                                val.rlevel = PublicFunc.GetLevel(double.Parse(val.rval), R1_1, R1_2, R1_3, R1_4, out bR);
                            } break;
                        case 48:
                            {
                                val.rlevel = PublicFunc.GetLevel(double.Parse(val.rval), R1_1, R1_2, R1_3, R1_4, out bR);
                            } break;
                        case 72:
                            {
                                val.rlevel = PublicFunc.GetLevel(double.Parse(val.rval), R1_1, R1_2, R1_3, R1_4, out bR);
                            } break;
                        default:
                            return list;
                    }

                    val.hlevel = PublicFunc.GetLevel(double.Parse(val.hval), h1, h2, h3, out bH);
                    val.nlevel = PublicFunc.GetLevel(double.Parse(val.nval), n1, n2, n3, n4, out bN);

                    if (bH&bR&bN)
                        continue;

                    zs.ValList.Add(val);
                }

                list.Add(zs);
            }

            return list;
        }

        [WebMethod]
        public List<DZ_QUR_STRU> GetDZInfoByTime(int nIndex, string strStart, string strEnd, int time)
        {
            List<DZ_QUR_STRU> list = new List<DZ_QUR_STRU>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";
            string strTableName = "";
            string strFld = "";

            if (nIndex == 0)
            {
                strTableName = "whqxt_dz_sk";
                strFld = "R" + time.ToString() + "P";
            }
            else if (nIndex == 1)
            {
                strTableName = "whqxt_dz_yb";
                strFld = "R" + time.ToString() + "F";
            }
            else
                return list;

            string strLevelFld1 = "R" + time.ToString() + "_1";
            string strLevelFld2 = "R" + time.ToString() + "_2";
            string strLevelFld3 = "R" + time.ToString() + "_3";
            string strLevelFld4 = "R" + time.ToString() + "_4";

            strSql += "SELECT TBL1.DID AS DID,TBL1.DT,{0},{1},{2},{3},{4},TBL2.NAME AS NAME ";
            strSql += " FROM   scott.{5} TBL1,scott.whqxt_dzzhd TBL2,scott.whqxt_cvalue_dz TBL3";
            strSql += " WHERE TBL1.DID = TBL2.DID AND TBL3.DID=TBL1.DID AND ";
            strSql += "(DT BETWEEN TO_DATE('{6}','yyyy-mm-dd hh24:mi:ss') AND TO_DATE('{7}','yyyy-mm-dd hh24:mi:ss')) ";
            sbSql.AppendFormat(strSql, strFld, strLevelFld1, strLevelFld2, strLevelFld3, strLevelFld4,
                strTableName, strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return list;

            DataTable dt = ds.Tables[0];
            DataTable dtDate = dt.DefaultView.ToTable(true, "DT");              // 获取所有的时间(Distinct)

            foreach (DataRow dr in dtDate.Rows)
            {
                DZ_QUR_STRU zs = new DZ_QUR_STRU();
                DataRow[] drs = dt.Select("DT='" + DateTime.Parse(dr["DT"].ToString()) + "'");
                DateTime date = DateTime.Parse(dr["DT"].ToString());

                zs.ValList = new List<DZ_VAL>();
                zs.date = date.ToString("yyyy-MM-dd");
                zs.time = date.ToString("HH:mm");

                foreach (DataRow dr1 in drs)
                {
                    DZ_VAL val = new DZ_VAL();
                    int nLevel = 1;

                    string R1 = dr1[strLevelFld1].ToString();
                    string R2 = dr1[strLevelFld2].ToString();
                    string R3 = dr1[strLevelFld3].ToString();
                    string R4 = dr1[strLevelFld4].ToString();
                    val.id = dr1["DID"].ToString();
                    val.name = dr1["NAME"].ToString();
                    val.hval = dr1[strFld].ToString();
                    val.hlevel = PublicFunc.GetLevel(double.Parse(val.hval), R1, R2, R3, R4, out nLevel);
                    if (nLevel > 4)
                        continue;

                    zs.ValList.Add(val);
                }

                list.Add(zs);
            }

            return list;
        }

        [WebMethod]
        public List<RD_QUR_STRU> GetRDInfoByTime(int nIndex, string strStart, string strEnd, int time)
        {
            List<RD_QUR_STRU> list = new List<RD_QUR_STRU>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            string strTableName = "";
            string strFld1 = "";
            string strFld2 = "";

            if (nIndex == 0)
            {
                strTableName = "WHQXT_RD_SK";
                strFld1 = "H" + time.ToString() + "PMAX";
                strFld2 = "H" + time.ToString() + "PMIN";
            }
            else if (nIndex == 1)
            {
                strTableName = "WHQXT_RD_YB";
                strFld1 = "H" + time.ToString() + "FMAX";
                strFld2 = "H" + time.ToString() + "FMIN";
            }
            else
                return list;

            // 获取临界值
            // 社区临界值要单独查询
            strSql = "SELECT * FROM scott.WHQXT_CVALUE_RD";
            DataSet cvDs = DbHelperOra.Query(strSql);

            if (cvDs.Tables.Count < 1 || cvDs.Tables[0].Rows.Count < 1)
                return list;

            DataRow dr_cv = cvDs.Tables[0].Rows[0];
            string h1 = dr_cv["H_1"].ToString();
            string h2 = dr_cv["H_2"].ToString();
            string h3 = dr_cv["H_3"].ToString();

            strSql = "SELECT TBL1.STID AS STID,TBL1.DT,{0},{1},TBL2.NAME AS NAME ";
            strSql += " FROM   scott.{2} TBL1,scott.whqxt_st  TBL2";
            strSql += " WHERE TBL1.STID = TBL2.STID AND ";
            strSql += "(DT BETWEEN TO_DATE('{3}','yyyy-mm-dd hh24:mi:ss') AND TO_DATE('{4}','yyyy-mm-dd hh24:mi:ss')) ";
            sbSql.AppendFormat(strSql, strFld1, strFld2, strTableName, strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return list;

            DataTable dt = ds.Tables[0];
            DataTable dtDate = dt.DefaultView.ToTable(true, "DT");              // 获取所有的时间(Distinct)

            foreach (DataRow dr in dtDate.Rows)
            {
                RD_QUR_STRU zs = new RD_QUR_STRU();
                DataRow[] drs = dt.Select("DT='" + DateTime.Parse(dr["DT"].ToString()) + "'");
                DateTime date = DateTime.Parse(dr["DT"].ToString());

                zs.ValList = new List<RD_VAL>();
                zs.date = date.ToString("yyyy-MM-dd");
                zs.time = date.ToString("HH:mm");

                foreach (DataRow dr1 in drs)
                {
                    RD_VAL val = new RD_VAL();

                    val.id = dr1["STID"].ToString();
                    val.name = dr1["NAME"].ToString();
                    val.hmax = dr1[strFld1].ToString();
                    val.hmin = dr1[strFld2].ToString();
                    val.hlevel = PublicFunc.GetLevel(double.Parse(val.hmax), h1, h2, h3);

                    zs.ValList.Add(val);
                }

                list.Add(zs);
            }

            return list;
        }

        [WebMethod]
        public List<HJ_QUR_STRU> GetHJInfoByTime(int nIndex, string strStart, string strEnd, int time, int nType)
        {
            List<HJ_QUR_STRU> list = new List<HJ_QUR_STRU>();
            StringBuilder sbSql = new StringBuilder();
            string strSql = "";

            string strTableName = "";
            string strFld1 = "";
            string strFld2 = "";
            string strFld3 = "";

            if (nIndex == 0)
            {
                strTableName = "WHQXT_HJ_SK";
                strFld1 = "S" + time.ToString() + "P";
                strFld2 = "N" + time.ToString() + "P";
                strFld3 = "PM" + time.ToString() + "P";
            }
            else if (nIndex == 1)
            {
                strTableName = "WHQXT_HJ_YB";
                strFld1 = "S" + time.ToString() + "F";
                strFld2 = "N" + time.ToString() + "F";
                strFld3 = "PM" + time.ToString() + "F";
            }
            else
                return list;

            strSql = "SELECT TBL1.STID AS STID,TBL1.DT,{0},{1},{2},TBL2.NAME AS NAME ";
            strSql += " FROM   scott.{3} TBL1,scott.whqxt_st  TBL2";
            strSql += " WHERE TBL1.STID = TBL2.STID AND ";
            strSql += "(DT BETWEEN TO_DATE('{4}','yyyy-mm-dd hh24:mi:ss') AND TO_DATE('{5}','yyyy-mm-dd hh24:mi:ss')) ";
            sbSql.AppendFormat(strSql, strFld1, strFld2, strFld3, strTableName, strStart, strEnd);

            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return list;

            DataTable dt = ds.Tables[0];
            DataTable dtDate = dt.DefaultView.ToTable(true, "DT");              // 获取所有的时间(Distinct)

            foreach (DataRow dr in dtDate.Rows)
            {
                HJ_QUR_STRU zs = new HJ_QUR_STRU();
                DataRow[] drs = dt.Select("DT='" + DateTime.Parse(dr["DT"].ToString()) + "'");
                DateTime date = DateTime.Parse(dr["DT"].ToString());

                zs.ValList = new List<HJ_VAL>();
                zs.date = date.ToString("yyyy-MM-dd");
                zs.time = date.ToString("HH:mm");

                foreach (DataRow dr1 in drs)
                {
                    HJ_VAL val = new HJ_VAL();
                    bool bAdd = true;

                    val.id = dr1["STID"].ToString();
                    val.name = dr1["NAME"].ToString();
                    val.sval = dr1[strFld1].ToString();
                    val.nval = dr1[strFld2].ToString();
                    val.pmval = dr1[strFld3].ToString();

                    if (nType == 0)         // 雾
                    {
                        val.level = PublicFunc.GetWLevel(val.nval, out bAdd);
                    }
                    else
                    {
                        val.level = PublicFunc.GetMLevel(val.nval, val.sval, val.pmval, out bAdd);
                    }
                    
                    if (!bAdd)
                        continue;

                    zs.ValList.Add(val);
                }

                list.Add(zs);
            }

            return list;
        }
     
        // 泵站查询  (面积(B1)*单位面积降雨量*百分比)/(排水量(speed))   --- 获取单个孔排水的时间
        [WebMethod]
        public List<ZS_BZ_INFO> GetInfozsBZ()
        {
            return null;
        }

        [WebMethod]
        public List<ZS_QUR_STRU> GetAllInfoZS(string time)
        {
            List<ZS_QUR_STRU> list = new List<ZS_QUR_STRU>();
            string strSql       = "";
            StringBuilder sbSql = new StringBuilder();
            string strFld       = "";
            string strLevelFld1 = "";
            string strLevelFld2 = "";
            string strLevelFld3 = "";
            string strLevelFld4 = "";

            if ((time!= null)&&(time != ""))
            {
                strFld       = "R" + time + "P";
                strLevelFld1 = "R" + time + "_1";
                strLevelFld2 = "R" + time + "_2";
                strLevelFld3 = "R" + time + "_3";
                strLevelFld4 = "R" + time + "_4";
            }
            else
            {
                strLevelFld1 = "R1_1";
                strLevelFld2 = "R1_2";
                strLevelFld3 = "R1_3";
                strLevelFld4 = "R1_4";
                strFld = "R1P";
            }

            strSql += "SELECT TBL1.ZID AS ZID,TBL1.DT,{0},{1},{2},{3},{4},TBL2.NAME AS NAME ";
            strSql += " FROM   scott.whqxt_zs_sk TBL1,scott.whqxt_zsd TBL2,scott.whqxt_cvalue_zs TBL3";
            strSql += " WHERE TBL1.ZID = TBL2.ZID AND TBL3.ZID=TBL1.ZID ";

            sbSql.AppendFormat(strSql, strFld, strLevelFld1, strLevelFld2, strLevelFld3, strLevelFld4);
            DataSet ds = DbHelperOra.Query(sbSql.ToString());
            if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return list;

            DataTable dt = ds.Tables[0];
            DataTable dtDate = dt.DefaultView.ToTable(true, "DT");              // 获取所有的时间(Distinct)

            foreach (DataRow dr in dtDate.Rows)
            {
                ZS_QUR_STRU zs = new ZS_QUR_STRU();
                DataRow[] drs = dt.Select("DT='" + DateTime.Parse(dr["DT"].ToString()) + "'");
                DateTime date = DateTime.Parse(dr["DT"].ToString());

                zs.ValList = new List<ZS_VAL>();
                zs.date = date.ToString("yyyy-MM-dd");
                zs.time = date.ToString("HH:mm");

                foreach (DataRow dr1 in drs)
                {
                    ZS_VAL val = new ZS_VAL();
                    int nLevel = 1;

                    string R1 = dr1[strLevelFld1].ToString();
                    string R2 = dr1[strLevelFld2].ToString();
                    string R3 = dr1[strLevelFld3].ToString();
                    string R4 = dr1[strLevelFld4].ToString();
                    val.id = dr1["ZID"].ToString();
                    val.name = dr1["NAME"].ToString();
                    val.value = dr1[strFld].ToString();
                    val.level = PublicFunc.GetLevel(double.Parse(val.value), R1, R2, R3, R4, out nLevel);
                    if (nLevel > 4)
                        continue;

                    zs.ValList.Add(val);
                }

                list.Add(zs);
            }
            return list;
        }

        [WebMethod]
        // 导出Word
        public void ExportWord()
        {
            string strPath = HttpContext.Current.Server.MapPath("App_data") + "\\sample\\sample1.docx";

            using (FileStream stream = File.OpenRead(strPath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                foreach (var para in doc.Paragraphs)
                {
                    string text = para.ParagraphText; //获得文本
                    var runs = para.Runs;
                    string styleid = para.Style;
 
                    for (int i = 0; i < runs.Count; i++)
                    {
                         var run = runs[i];
                         text = run.ToString(); //获得run的文本
                    }
                }
            }
        }
    }
}
