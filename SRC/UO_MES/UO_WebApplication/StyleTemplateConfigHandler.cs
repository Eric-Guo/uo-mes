using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Collections.Generic;

public class StyleTemplateConfigHandler : IConfigurationSectionHandler
{
	public object Create(object parent, object configContext, XmlNode section) {
		return new StyleTemplateConfiguration(section);
	}
}

// 映射 styleTemplates 结点的实体类
public class StyleTemplateConfiguration {

	private XmlNode node;				// styleTemplates 结点
	private string defaultTheme;		// 默认的主题名称
	private string defaultStyle;		// 站点默认风格名称
	private HttpContext context;
	private Dictionary<string,string> styleDic; // Key: 风格名称；Value: 主题名称


	public StyleTemplateConfiguration(XmlNode node) {
		this.node = node;
		context = HttpContext.Current;
		styleDic = new Dictionary<string, string>();

		// 获取所有style结点的 name属性 和 theme属性
		XmlNodeList styleList = node.SelectNodes("style");
		foreach (XmlNode style in styleList) {
			styleDic[style.Attributes["name"].Value] = style.Attributes["theme"].Value;
		}

		// 获取 站点默认风格 名称
		defaultStyle = node.Attributes["default"].Value;

		// 根据 风格名称 获取主题
		defaultTheme = styleDic[defaultStyle];
	}

	// 获取所有风格名称
	public ICollection<String> StyleNames {
		get {
			return styleDic.Keys;
		}
	}

	// 根据风格名称获取主题名称
	public string GetTheme(string styleName) {
		return styleDic[styleName]; 
	}

	// 设置、获取 站点默认风格
	public string DefaultStyle{
		get {
			return defaultStyle;
		}
		set { // 更改Web.Config中的默认风格，一般为站长才可以使用
			XmlDocument doc = new XmlDocument();
			doc.Load(context.Server.MapPath(@"~/web.config"));
			XmlNode root = doc.DocumentElement;
			XmlNode styleTemp = root.SelectSingleNode("styleTemplates");

			styleTemp.Attributes["default"].Value = value;
			doc.Save(context.Server.MapPath(@"~/web.config"));
		}
	}

	// 获取默认主题名称
	public string DefaultTheme {
		get { return defaultTheme;	}
	}
}