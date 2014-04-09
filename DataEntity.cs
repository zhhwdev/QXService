using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_Backgroundservice
{
    public class JTXX_INFO
    {
        public string name;
        public string x;
        public string y;
        public string type;
    }

    public class KW_INFO
    {
        public string id;
        public string date;
        public string time;
        public string name;
        public string px;
        public string py;
        public string wd;
        public string pj;
        public string ms;
    }

    public class INFO
    {
        public string id;
        public string name;
        public string date;
        public string time;
        public string r1p;
        public string r3p;
        public string r6p;
        public string r12p;
        public string r24p;
    }

    public class RD_INFO
    {
        public string id;
        public string date;
        public string time;
        public string name;
        public string px;
        public string py;
        public string h1;
        public string h1max;
        public string h1min;
        public string h3;
        public string h3max;
        public string h3min;
        public string h6;
        public string h6max;
        public string h6min;
        public string h12;
        public string h12max;
        public string h12min;
        public string h24;
        public string h24max;
        public string h24min;
        public string h48;
        public string h48max;
        public string h48min;
        public string h72;
        public string h72max;
        public string h72min;

        public string g1;
        public string g3;
        public string g6;
        public string g12;
        public string g24;
        public string g48;
        public string g72;
    }

    public class HJ_INFO
    {
        public string id;
        public string date;
        public string time;
        public string name;
        public string px;
        public string py;

        public string s1;
        public string n1;
        public string p1;
        public string pm1;

        public string s3;
        public string n3;
        public string p3;
        public string pm3;

        public string s6;
        public string n6;
        public string p6;
        public string pm6;

        public string s12;
        public string n12;
        public string p12;
        public string pm12;

        public string s24;
        public string n24;
        public string p24;
        public string pm24;

        public string gw1;
        public string gw3;
        public string gw6;
        public string gw12;
        public string gw24;

        public string gm1;
        public string gm3;
        public string gm6;
        public string gm12;
        public string gm24;

        public string n;
        public string s;
        public string p;
    }

    public class JT_INFO
    {
        public string id;
        public string date;
        public string time;
        public string name;
        public string px;
        public string py;

        public string r1;
        public string h1;
        public string fx1;
        public string fs1;
        public string s1;
        public string n1;
        public string w1;

        public string r3;
        public string h3;
        public string fx3;
        public string fs3;
        public string s3;
        public string n3;
        public string w3;

        public string r6;
        public string h6;
        public string fx6;
        public string fs6;
        public string s6;
        public string n6;
        public string w6;

        public string r12;
        public string h12;
        public string fx12;
        public string fs12;
        public string s12;
        public string n12;
        public string w12;

        public string r24;
        public string h24;
        public string fx24;
        public string fs24;
        public string s24;
        public string n24;
        public string w24;

        public string r48;
        public string h48;
        public string fx48;
        public string fs48;
        public string s48;
        public string n48;
        public string w48;

        public string r72;
        public string h72;
        public string fx72;
        public string fs72;
        public string s72;
        public string n72;
        public string w72;

        public string gr1;
        public string gt1;
        public string gn1;

        public string gr3;
        public string gt3;
        public string gn3;

        public string gr6;
        public string gt6;
        public string gn6;

        public string gr12;
        public string gt12;
        public string gn12;

        public string gr24;
        public string gt24;
        public string gn24;

        public string gr48;
        public string gt48;
        public string gn48;

        public string gr72;
        public string gt72;
        public string gn72;

    }

    public class SQ_INFO
    {
        public string id;
        public string date;
        public string time;
        public string name;
        public string px;
        public string py;

        public string r1;
        public string h1;
        public string fx1;
        public string fs1;
        public string s1;
        public string h1max;
        public string h1min;
        public string hcode1;

        public string r3;
        public string h3;
        public string fx3;
        public string fs3;
        public string s3;
        public string h3max;
        public string h3min;
        public string hcode3;

        public string r6;
        public string h6;
        public string fx6;
        public string fs6;
        public string s6;
        public string h6max;
        public string h6min;
        public string hcode6;

        public string r12;
        public string h12;
        public string fx12;
        public string fs12;
        public string s12;
        public string h12max;
        public string h12min;
        public string hcode12;

        public string r24;
        public string h24;
        public string fx24;
        public string fs24;
        public string s24;
        public string h24max;
        public string h24min;
        public string hcode24;

        public string r48;
        public string h48;
        public string fx48;
        public string fs48;
        public string s48;
        public string h48max;
        public string h48min;
        public string hcode48;

        public string r72;
        public string h72;
        public string fx72;
        public string fs72;
        public string s72;
        public string h72max;
        public string h72min;
        public string hcode72;

        public string d1;
        public string n1;

        public string d2;
        public string n2;

        public string d3;
        public string n3;

        public string g1;
        public string g3;
        public string g6;
        public string g12;
        public string g24;
        public string g48;
        public string g72;

        public string gh1;
        public string gh3;
        public string gh6;
        public string gh12;
        public string gh24;
        public string gh48;
        public string gh72;
    }


    #region 实况数据 || 预报数据
    /// <summary>
    /// 渍水实况
    /// </summary>
    public class ZS_SK_INFO
    {
        public long   ID;       // ID
        /*public string ZID;      // 渍水点ID
        public string BID;      // 泵站ID
        public string STID;     // 自动站ID*/
        public string DT;     // 观测时间
        public string date;
        public string time;
        public string name;     // 站点名称
        public string px;
        public string py;

        public double r1p;      // 小时降水量
        public string g1p;         // 级别    
        public double r3p;      // 过去3小时降水量
        public string g3p;         // 级别
        public double r6p;      // 过去6小时降水量
        public string g6p;         // 级别
        public double r12p;     // 过去12小时降水量
        public string g12p;        // 过去12小时降水量
        public double r24p;     // 过去24小时降水量
        public string g24p;         // 过去12小时降水量

        public string g1f;
        public string g3f;
        public string g6f;
        public string g12f;
        public string g24f;
    }

    /// <summary>
    /// 热岛实况
    /// </summary>
    public class WHQXT_RD_SK
    {
        public long ID;                 // ID
        public string STID;             // 自动站
        public DateTime DT;             // 观测时间
        public double H1P;              // 过去1小时气温
        public double H1PMAX;           // 过去1小时最高气温
        public double H1PMIN;           // 过去1小时最低气温
        public double H3P;              // 过去3小时气温 (后面类推)
        public double H3PMAX;
        public double H3PMIN;
        public double H6P;
        public double H6PMAX;
        public double H6PMIN;
        public double H12P;
        public double H12PMAX;
        public double H12PMIN;
        public double H24P;
        public double H24PMAX;
        public double H24PMIN;
    }
    /// <summary>
    /// 地址灾害实况				
    /// </summary>
    public class DZ_SK_INFO
    {
        public string id;
        public string name;
        public string date;
        public string time;
        public string px;
        public string py;
        public double r1p;
        public string g1p;              // 级别
        public double r3p;
        public string g3p;
        public double r6p;
        public string g6p;
        public double r12p;
        public string g12p;
        public double r24p;
        public string g24p;
    }
    public class DZ_YB_INFO
    {
        public string id;
        public string name;
        public string date;
        public string time;
        public string px;
        public string py;
        public string r1f;
        public string g1f;              // 级别
        public string r3f;
        public string g3f;
        public string r6f;
        public string g6f;
        public string r12f;
        public string g12f;
        public string r24f;
        public string g24f;
        public string r48f;
        public string g48f;
        public string r72f;
        public string g72f;
    }
    /// <summary>
    /// 社区气象实况				s
    /// </summary>
    public class SQ_SK_INFO
    {
        public long ID;                     // ID
        public string SID;                  // 社区站点
        public string STID;                 // 自动站ID
        public DateTime DT;                 // 观测时间
        public double R1P;                  // 过去1小时降水量
        public double H1P;                  // 过去1小时气温
        public double H1PMAX;               // 过去1小时最高气温
        public double H1PMIN;               // 过去1小时最低气温
        public double S1P;                  // 过去1小时相对湿度
        public string WPCODE1P;             // 过去1小时天气现象编码
        public double FX1P;                 // 过去1小时瞬时风向
        public double FS1P;                 // 过去1小时瞬时风速   (后面类推)
        public double R3P;
        public double H3P;
        public double H3PMAX;
        public double H3PMIN;
        public double S3P;
        public string WPCODE3P;
        public double FX3P;
        public double FS3P;
        public double R6P;
        public double H6P;
        public double H6PMAX;
        public double H6PMIN;
        public double S6P;
        public string WPCODE6P;
        public double FX6P;
        public double FS6P;
        public double R12P;
        public double H12P;
        public double H12PMAX;
        public double H12PMIN;
        public double S12P;
        public string WPCODE12P;
        public double FX12P;
        public double FS12P;
        public double R24P;
        public double H24P;
        public double H24PMAX;
        public double H24PMIN;
        public double S24P;
        public string WPCODE24P;
        public double FX24P;
        public double FS24P;
        public double R48P;
        public double H48P;
        public double H48PMAX;
        public double H48PMIN;
        public double S48P;
        public string WPCODE48P;
        public double FX48P;
        public double FS48P;
    }

    /// <summary>
    /// 交通实况
    /// </summary>
    public class WHQXT_JT_SK
    {
        public long ID;                     // ID
        public string JID;                  // 交通站点ID
        public string STID;                 // 自动站ID
        public DateTime DT;                 // 观测时间
        public double R1P;              // 过去1小时降水量
        public double H1P;              // 过去1小时气温
        public double H1PMAX;           // 过去1小时最高气温
        public double H1PMIN;           // 过去1小时最低气温
        public double S1P;              // 过去1小时相对湿度
        public double N1P;              // 过去1小时能见度
        public double W1P;              // 过去1小时地表温度
        public double W1PMAX;           // 过去1小时地表最高温度
        public double FX1P;             // 过去1小时瞬时风向
        public double FS1P;             // 过去1小时瞬时风速
        public double R3P;
        public double H3P;
        public double H3PMAX;
        public double H3PMIN;
        public double S3P;
        public double N3P;
        public double W3P;
        public double W3PMAX;
        public double FX3P;
        public double FS3P;
        public double R6P;
        public double H6P;
        public double H6PMAX;
        public double H6PMIN;
        public double S6P;
        public double N6P;
        public double W6P;
        public double W6PMAX;
        public double FX6P;
        public double FS6P;
        public double R12P;
        public double H12P;
        public double H12PMAX;
        public double H12PMIN;
        public double S12P;
        public double N12P;
        public double W12P;
        public double W12PMAX;
        public double FX12P;
        public double FS12P;
        public double R24P;
        public double H24P;
        public double H24PMAX;
        public double H24PMIN;
        public double S24P;
        public double N24P;
        public double W24P;
        public double W24PMAX;
        public double FX24P;
        public double FS24P;
    }

    /// <summary>
    /// 环境气象实况
    /// </summary>
    public class WHQXT_HJ_SK
    {
        public long ID;                 // ID   
        public string STID;             // 站点编号
        public DateTime DT;             // 观测时间
        public double S1P;              // 过去1小时相对湿度
        public double N1P;              // 过去1小时能见度   (后面类似)
        public double S3P;
        public double N3P;
        public double S6P;
        public double N6P;
        public double S12P;
        public double N12P;
        public double S24P;
        public double N24P;
    }

    /// <summary>
    /// 扩散条件实况
    /// </summary>
    public class WHQXT_KSTJ_SK
    {
        public long ID;             // ID
        public string QID;          // 区域ID
        public DateTime DT;         // 观测时间
        public double S1P;          //未来1小时扩散条件
        public double S3P;          // 未来3小时扩散条件
        public double S6P;          // 未来6小时扩散条件
        public double S12P;         // 未来12小时扩散条件
        public double S24P;         // 未来24小时扩散条件
    }

    /// <summary>
    /// 渍水点
    /// </summary>
    public class WHQXT_ZSD
    {
        public long ZID;
        public string NAME;
        public string BID;
        public double X;
        public double Y;
        public string STID;
        public string COMMENTS;
        public string REST;
    }

    /// <summary>
    /// 泵站
    /// </summary>
    public class WHQXT_ZSBZD
    {
        public string BID;                  // 泵站ID
        public string NAME;                 // 泵站名称
        public double X;                    // 经度
        public double Y;                    // 维度
        public double SPEED;                // 排水量
        public string COMMENTS;             // 备注
        public string REST;                 // 其他
    }

    /// <summary>
    /// 地址灾害点
    /// </summary>
    public class WHQXT_DZZHD
    {
        public long DID;                // 地址灾害点ID
        public string NAME;             // 地址灾害点名称
        public double X;                // 经度
        public double Y;                // 维度
        public string STID;             // 关联自动站
        public string COMMENTS;         // 备注
        public string REST;             // 其他
    }

    /// <summary>
    /// 社区站点
    /// </summary>
    public class WHQXT_SQZD
    {
        public long SID;                // 社区站点ID
        public string NAME;             // 社区站点名称
        public double X;
        public double Y;
        public string STID;
        public string COMMENTS;
        public string REST;

    }

    /// <summary>
    /// 交通站点
    /// </summary>
    public class WHQXT_JTZD
    {
        public long JID;                // 交通路段ID
        public string NAME;             // 交通路段名称

        //起始经度 起始维度 结束经度 结束维度
        public double X;
        public double Y;
        public double X2;
        public double Y2;
        public string STID;             // 关联自动站
        public string COMMENTS;
        public string REST;
    }

    /// <summary>
    /// 交通信息
    /// </summary>
    public class WHQXT_JTXX
    {
        public long XID;
        public string NAME;
        public double X;
        public double Y;
        public string TYPE;
        public string COMMENTS;
        public string REST;

    }

    /// <summary>
    /// 区域表
    /// </summary>
    public class WHQXT_KSQY
    {
        public long QID;            // 区域ID
        public string NAME;         // 区域名称
        public double X;            // 经度
        public double Y;            // 维度
        public string COMMENTS;
        public string REST;
    }
    #endregion

    #region 预报数据

    /// <summary>
    /// 渍水预报
    /// </summary>
    public class ZS_YB_INFO
    {
        public long id;
        /*public string ZID;        // 渍水点ID
        public string BID;          // 泵站ID
        public string STID;         // 自动站ID    */
        public string name;         // 站点名称
        public string DT;         // 观测时间
        public string date;
        public string time;
        public string px;
        public string py;
        public double r1f;          // 未来1小时降水量
        public string g1f;          // 级别    
        public double r3f;
        public string g3f;         // 级别    
        public double r6f;
        public string g6f;         // 级别    
        public double r12f;
        public string g12f;        // 级别    
        public string g24f;        // 级别    
        public double r24f;
        public double r48f;
        public string g48f;        // 级别    
        public double r72f;
        public string g72f;        // 级别    

    }

    /// <summary>
    /// 热岛预报
    /// </summary>
    public class WHQXT_RD_YB
    {
        public long ID;
        public string STID;         // 自动站ID
        public DateTime DT;         // 观测时间
        public double H1F;          // 未来1小时气温
        public double H1FMAX;       // 未来1小时最高气温
        public double H1FMIN;       // 未来1小时最低气温  （后面类推）
        public double H3F;
        public double H3FMAX;
        public double H3FMIN;
        public double H6F;
        public double H6FMAX;
        public double H6FMIN;
        public double H12F;
        public double H12FMAX;
        public double H12FMIN;
        public double H24F;
        public double H24FMAX;
        public double H24FMIN;
        public double H72F;
        public double H72FMAX;
        public double H72FMIN;
    }

    /// <summary>
    /// 地址灾害预报
    /// </summary>
    public class WHQXT_DZ_YB
    {
        public long ID;
        public string DID;          // 地质灾害点ID
        public string STID;         // 自动站ID
        public DateTime DT;         // 观测时间  
        public double R1F;          // 小时降水量
        public double R3F;          // 未来3小时降水量
        public double R6F;          // 未来6小时降水量
        public double R12F;
        public double R24F;
    }

    /// <summary>
    /// 社区气象预报
    /// </summary>
    public class WHQXT_SQ_YB
    {
        public long ID;                     // ID
        public string SID;                  // 社区站点
        public string STID;                 // 自动站ID
        public DateTime DT;                 // 观测时间
        public double R1F;                  // 未来1小时降水量
        public double H1F;                  // 未来1小时气温
        public double H1FMAX;               // 未来1小时最高气温
        public double H1FMIN;               // 未来1小时最低气温
        public double S1F;                  // 未来1小时相对湿度
        public string WFCODE1F;             // 未来1小时天气现象编码
        public double FX1F;                 // 未来1小时瞬时风向
        public double FS1F;                 // 未来1小时瞬时风速   (后面类推)
        public double R3F;
        public double H3F;
        public double H3FMAX;
        public double H3FMIN;
        public double S3F;
        public string WFCODE3F;
        public double FX3F;
        public double FS3F;
        public double R6F;
        public double H6F;
        public double H6FMAX;
        public double H6FMIN;
        public double S6F;
        public string WFCODE6F;
        public double FX6F;
        public double FS6F;
        public double R12F;
        public double H12F;
        public double H12FMAX;
        public double H12FMIN;
        public double S12F;
        public string WFCODE12F;
        public double FX12F;
        public double FS12F;
        public double R24F;
        public double H24F;
        public double H24FMAX;
        public double H24FMIN;
        public double S24F;
        public string WFCODE24F;
        public double FX24F;
        public double FS24F;
        public double R48F;
        public double H48F;
        public double H48FMAX;
        public double H48FMIN;
        public double S48F;
        public string WFCODE48F;
        public double FX48F;
        public double FS48F;
    }

    public class WHQXT_JT_YB
    {
        public long ID;                     // ID
        public string JID;                  // 交通站点ID
        public string STID;                 // 自动站ID
        public DateTime DT;                 // 观测时间
        public double R1F;              // 未来1小时降水量
        public double H1F;              // 未来1小时气温
        public double H1FMAX;           // 未来1小时最高气温
        public double H1FMIN;           // 未来1小时最低气温
        public double S1F;              // 未来1小时相对湿度
        public double N1F;              // 未来1小时能见度
        public double W1F;              // 未来1小时地表温度
        public double W1FMAX;           // 未来1小时地表最高温度
        public double FX1F;             // 未来1小时瞬时风向
        public double FS1F;             // 未来1小时瞬时风速
        public double R3F;
        public double H3F;
        public double H3FMAX;
        public double H3FMIN;
        public double S3F;
        public double N3F;
        public double W3F;
        public double W3FMAX;
        public double FX3F;
        public double FS3F;
        public double R6F;
        public double H6F;
        public double H6FMAX;
        public double H6FMIN;
        public double S6F;
        public double N6F;
        public double W6F;
        public double W6FMAX;
        public double FX6F;
        public double FS6F;
        public double R12F;
        public double H12F;
        public double H12FMAX;
        public double H12FMIN;
        public double S12F;
        public double N12F;
        public double W12F;
        public double W12FMAX;
        public double FX12F;
        public double FS12F;
        public double R24F;
        public double H24F;
        public double H24FMAX;
        public double H24FMIN;
        public double S24F;
        public double N24F;
        public double W24F;
        public double W24FMAX;
        public double FX24F;
        public double FS24F;
    }

    /// <summary>
    /// 环境气象预报
    /// </summary>
    public class WHQXT_HJ_YB
    {
        public long ID;                 // ID   
        public string STID;             // 站点编号
        public DateTime DT;             // 观测时间
        public double S1F;              // 未来1小时相对湿度
        public double N1F;              // 未来1小时能见度   (后面类似)
        public double S3F;
        public double N3F;
        public double S6F;
        public double N6F;
        public double S12F;
        public double N12F;
        public double S24F;
        public double N24F;
    }
#endregion

    #region 自动站
    /// <summary>
    /// 自动站
    /// </summary>
    public class WHQXT_ZDZ
    {
        public string STID;             // 站点编号
        public string NAME;             // 站名
        public double X;                // 经度
        public double Y;                // 纬度
        public string ASL;              // 海拔
        public string AREA;             // 区域
        public string LOCATION;         // 地址
        public string TYPE;             // 类型
    }
    #endregion

    #region  临界值
    /// <summary>
    /// 渍水站点临界值
    /// </summary>
    public class WHQXT_CVALUE_ZS
    {
        public string STID;
        public string NAME;
        public double R1_1;
        public double R1_2;
        public double R1_3;
        public double R1_4;
        public double R3_1;
        public double R3_2;
        public double R3_3;
        public double R3_4;
        public double R6_1;
        public double R6_2;
        public double R6_3;
        public double R6_4;
        public double R12_1;
        public double R12_2;
        public double R12_3;
        public double R12_4;
        public double R24_1;
        public double R24_2;
        public double R24_3;
        public double R24_4;
    }

    /// <summary>
    /// 地质灾害站点临界值
    /// </summary>
    public class WHQXT_CVALUE_DZ
    {
        public string STID;
        public string NAME;
        public double R1_1;
        public double R1_2;
        public double R1_3;
        public double R1_4;
        public double R3_1;
        public double R3_2;
        public double R3_3;
        public double R3_4;
        public double R6_1;
        public double R6_2;
        public double R6_3;
        public double R6_4;
        public double R12_1;
        public double R12_2;
        public double R12_3;
        public double R12_4;
        public double R24_1;
        public double R24_2;
        public double R24_3;
        public double R24_4;
    }

    /// <summary>
    /// 社区站点临界值
    /// </summary>
    public class WHQXT_CVALUE_SQ
    {
        public string STID;
        public string NAME;
        public double R1_1;
        public double R1_2;
        public double R1_3;
        public double R1_4;
        public double R3_1;
        public double R3_2;
        public double R3_3;
        public double R3_4;
        public double R6_1;
        public double R6_2;
        public double R6_3;
        public double R6_4;
        public double R12_1;
        public double R12_2;
        public double R12_3;
        public double R12_4;
        public double R24_1;
        public double R24_2;
        public double R24_3;
        public double R24_4;
    }
    #endregion

    //渍水查询数据结构
    public class ZS_VAL
    {
        public string id;
        public string name;
        public string level;
        public string value;
    }

    public class ZS_QUR_STRU
    {
        public string date;
        public string time;
        
        public List<ZS_VAL> ValList;
    }

    public class JT_VAL
    {
        public string id;
        public string name;
        public string hval;
        public string rval;
        public string nval;
        public string hlevel;
        public string rlevel;
        public string nlevel;

    }

    public class JT_QUR_STRU
    {
        public string date;
        public string time;

        public List<JT_VAL> ValList;
    }

    public class DZ_VAL
    {
        public string id;
        public string name;
        public string hval;             // 降雨量
        public string hlevel;           // 级别
    }

    public class DZ_QUR_STRU
    {
        public string date;
        public string time;

        public List<DZ_VAL> ValList;
    }

    public class RD_VAL
    {
        public string id;
        public string name;
        public string hmax;             // 最大温度
        public string hmin;             // 最低温度
        public string hlevel;           // 级别
    }
    public class RD_QUR_STRU
    {
        public string date;
        public string time;

        public List<RD_VAL> ValList;
    }

    public class HJ_VAL
    {
        public string id;
        public string name;
        public string nval;             // 雾
        public string sval;             // 霾
        public string pmval;            // PM值

        public string level;            // 级别
    }

    public class HJ_QUR_STRU
    {
        public string date;
        public string time;

        public List<HJ_VAL> ValList;
    }

#region
    public class ZS_BZ_INFO
    {
        public string id;
        public string date;
        public string time;
    }
#endregion
}