using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public interface IUserStyleStrategy
{
	void ResetUserStyle(string styleName);	// 重新设置用户风格
	string GetUserStyle();						// 获取用户风格
}

// 默认风格设置方法：使用Cookie记录
public class CookieStyleStrategy : IUserStyleStrategy {
	
	private string cookieName;			// cookie名称
	private HttpContext context;

	public CookieStyleStrategy(string cookieName){
		this.cookieName = cookieName;
		context = HttpContext.Current;
	}

	// 重新设置用户风格名称
	public void ResetUserStyle(string styleName) {
		HttpCookie cookie;
		if(context.Request.Cookies[cookieName]!=null)
			cookie = context.Request.Cookies[cookieName];
		else
			cookie = new HttpCookie(cookieName);

		cookie.Value = context.Server.UrlEncode(styleName);
		cookie.Expires = DateTime.Now.AddHours(2);	// 设置Cookie过期时间

		context.Response.Cookies.Add(cookie);

		// 因为风格(master page和theme)的动态设置只能在 PreInit 事件中
		// 而Button的Click事件在PreInit事件之后，所以需要Redirect才可以生效
		context.Response.Redirect(context.Request.Url.PathAndQuery);
	}

	// 获取用户自己设置的风格名称
	public string GetUserStyle() {		
		if (context.Request.Cookies[cookieName] != null) {
			string value = context.Request.Cookies[cookieName].Value;
			value = context.Server.UrlDecode(value);	// 避免出现中文乱码
			return value;
		} else
			return null;
	}
}

// Use profile to save style theme
public class ProfileStyleStrategy : IUserStyleStrategy
{
    private HttpContext context;
    public ProfileStyleStrategy()
    {
        context = HttpContext.Current;
	}

	public void ResetUserStyle(string styleName)
    {
        context.Profile["StyleTheme"] = styleName;
        // 因为风格(master page和theme)的动态设置只能在 PreInit 事件中
        // 而Button的Click事件在PreInit事件之后，所以需要Redirect才可以生效
        context.Response.Redirect(context.Request.Url.PathAndQuery);
    }

	public string GetUserStyle() {
        return context.Profile["StyleTheme"] as string;
	}
}