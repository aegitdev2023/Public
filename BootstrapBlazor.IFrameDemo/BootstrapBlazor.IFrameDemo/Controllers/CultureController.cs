using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace BootstrapBlazor.IFrameDemo.Controllers
{
    [Route("[controller]/[action]")]
    public class CultureController : ControllerBase
    {
        public IActionResult SetCulture(string culture, string redirectUri)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                HttpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)));
            }

            return LocalRedirect(redirectUri);
        }

        public IActionResult ResetCulture(string redirectUri)
        {
            HttpContext.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);

            return LocalRedirect(redirectUri);
        }

        /// <summary>
        /// 根据key获取Cookie的Value值
        /// </summary>
        /// <returns></returns>
        public string GetCookid(string sCookieKeyName)
        {
            return HttpContext.Request.Cookies[sCookieKeyName].ToString();
        }

        /// <summary>
        /// 根据key删除Cookie
        /// </summary>
        public void DeleteCookie(string sCookieKeyName)
        {
            HttpContext.Response.Cookies.Delete(sCookieKeyName);
        }

    }
}
