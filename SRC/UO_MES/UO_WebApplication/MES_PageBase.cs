using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.UI;

namespace UO_WebApplication
{
    // 供所有基类继承
    public class MES_PageBase : Page
    {
        protected StyleTemplateConfiguration styleConfig;
        protected string userStyle;			// 用户设置的风格
        protected string userTheme;			// 用户设置的主题
        protected IUserStyleStrategy userStrategy;		// 使用何种算法来获得用户自定义的信息

        protected override void InitializeCulture()
        {
            String selectedLanguage = Context.Profile["Language"] as string;
            if (selectedLanguage != null)
            {
                UICulture = selectedLanguage;
                Culture = selectedLanguage;

                Thread.CurrentThread.CurrentCulture =
                    CultureInfo.CreateSpecificCulture(selectedLanguage);
                Thread.CurrentThread.CurrentUICulture = new
                    CultureInfo(selectedLanguage);
            }
            base.InitializeCulture();
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // 这里会被缓存只在第一次时调用有用
            this.styleConfig = (StyleTemplateConfiguration)ConfigurationManager.GetSection("styleTemplates");

            // 当你实现了自己的 Strategy时只需要更改这里就可以了
            // 更好的办法是将Stragey的类型保存的Web.config中，
            // 然后使用反射来动态创建
            //userStrategy = new CookieStyleStrategy("userStyle");
            userStrategy = new ProfileStyleStrategy();

            // 获取用户风格
            userStyle = userStrategy.GetUserStyle();

            // 如果用户没有设置风格，使用默认风格
            if (String.IsNullOrEmpty(userStyle))
            {
                userStyle = styleConfig.DefaultStyle;
                userTheme = styleConfig.DefaultTheme;
            }
            else
            {
                // 根据用户设置的风格 获取 主题名称
                userTheme = styleConfig.GetTheme(userStyle);
            }
            this.Theme = userTheme;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            String csname = "OnSubmitScript";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the OnSubmit statement is already registered.
            if (!cs.IsOnSubmitStatementRegistered(cstype, csname))
            {
                String cstext = "return FormCheck_OnSubmit();";
                cs.RegisterOnSubmitStatement(cstype, csname, cstext);
            }
        }

        /// <summary>
        /// MES Service will save to Session, so it will remain between page refresh.
        /// Always use MESPageService to reduce service object number for a user
        /// </summary>
        protected UO_Service.Base.Service MESPageService
        {
            get { return Session["MESPageService"] as UO_Service.Base.Service; }
            set { Session["MESPageService"] = value; }
        }
    }
}