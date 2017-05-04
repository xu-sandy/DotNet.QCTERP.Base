using Qct.Infrastructure.Data.EntityInterface;

namespace Qct.Objects.Entities
{
    public class SysWebSetting : CompanyEntity
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        //   [DataMember]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        /// <summary>
        /// logo展现方式
        /// </summary>
        public short LogoDispWay { get; set; }
        /// <summary>
        /// 登陆Logo
        /// [长度：200]
        /// </summary>
        public string LoginLogo { get; set; }
        /// <summary>
        /// 顶部Logo
        /// [长度：200]
        /// </summary>
        public string TopLogo { get; set; }

        /// <summary>
        /// 底部Logo
        /// [长度：200]
        /// </summary>
        public string BottomLogo { get; set; }

        /// <summary>
        /// App640图标
        /// [长度：200]
        /// </summary>
        public string AppIcon640 { get; set; }
        /// <summary>
        /// App960图标
        /// [长度：200]
        /// </summary>
        public string AppIcon960 { get; set; }
        /// <summary>
        /// APP首页logo
        /// </summary>
        public string AppIndexIcon640 { get; set; }
        /// <summary>
        /// APP首页logo
        /// </summary>
        public string AppIndexIcon960 { get; set; }
        /// <summary>
        /// App首页背景图
        /// </summary>
        public string AppIndexbg640 { get; set; }
        /// <summary>
        /// App首页背景图
        /// </summary>
        public string AppIndexbg960 { get; set; }
        /// <summary>
        /// App首页文字
        /// [长度：200]
        /// </summary>
        public string AppCustomer640 { get; set; }
        /// <summary>
        /// App首页文字
        /// [长度：200]
        /// </summary>
        public string AppCustomer960 { get; set; }

        /// <summary>
        /// 系统Icon
        /// [长度：200]
        /// </summary>
        public string SysIcon { get; set; }

        /// <summary>
        /// 系统名称
        /// [长度：200]
        /// </summary>
        public string SysName { get; set; }

        /// <summary>
        /// 官网地址
        /// [长度：200]
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// 公司简称
        /// [长度：50]
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// 公司全称
        /// </summary>
        public string CompanyFullTitle { get; set; }
        /// <summary>
        /// 公司电话
        /// [长度：20]
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 页面Title
        /// [长度：50]
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// SMTP发件箱服务器
        /// [长度：100]
        /// </summary>
        public string SMTPServer { get; set; }

        /// <summary>
        /// SMTP端口
        /// </summary>
        public int? SMTPPort { get; set; }

        /// <summary>
        /// SMTP显示名称
        /// [长度：200]
        /// </summary>
        public string SMTPShowName { get; set; }

        /// <summary>
        /// SMTP SSL端口
        /// </summary>
        public int? SMTPSSLPort { get; set; }

        /// <summary>
        /// SMTP账号
        /// [长度：100]
        /// </summary>
        public string SMTPAccount { get; set; }

        /// <summary>
        /// SMTP密码
        /// [长度：50]
        /// </summary>
        public string SMTPPwd { get; set; }

        /// <summary>
        /// APP在环信上注册的对应应用URL
        /// [长度：200]
        /// </summary>
        public string EMReqUrlBase { get; set; }

        /// <summary>
        /// APP在环信上注册的对应应用Id
        /// [长度：200]
        /// </summary>
        public string EMAppId { get; set; }

        /// <summary>
        /// APP在环信上注册的对应应用Secret
        /// [长度：200]
        /// </summary>
        public string EMAppSecret { get; set; }

        /// <summary>
        /// APP在环信上对应的appkey的org_name部分
        /// [长度：200]
        /// </summary>
        public string EMAppOrg { get; set; }

        /// <summary>
        /// APP在环信上对应的appkey的app_name部分
        /// [长度：200]
        /// </summary>
        public string EMAppName { get; set; }

    }
}
