using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

using azureWebApp.Models;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace azureWebApp.Controllers
{
    public class HomeController : Controller
    {
        private string ClientId = System.Web.Configuration.WebConfigurationManager.AppSettings["Google.ClientID"];
        private string SecretKey = System.Web.Configuration.WebConfigurationManager.AppSettings["Google.SecretKey"];
        private string RedirectUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["Google.RedirectUrl"];
          

        /// <summary>    
        /// Returns login page if user is not logged in else return user profile    
        /// </summary>    
        /// <returns>return page</returns>  
        public async Task<ActionResult> Index()
        {
            string token = (string)Session["user"];
            if (string.IsNullOrEmpty(token))
            {
                return View();
            }
            else
            {
                return View("UserProfile", await GetuserProfile(token));
            }
        }

        /// <summary>  
        /// Hit Google API to get access code  
        /// </summary>  
        public void LoginUsingGoogle()
        {
            Response.Redirect($"https://accounts.google.com/o/oauth2/v2/auth?client_id={ClientId}&response_type=code&scope=openid%20email%20profile&redirect_uri={RedirectUrl}&state=abcdef&prompt=consent");
        }

        //[HttpGet]
        //public ActionResult SignOut()
        //{
        //    Session["user"] = null;
        //    return View("Index");
        //}

        /// <summary>  
        /// Listen response from Google API after user authorization  
        /// </summary>  
        /// <param name="code">access code returned from Google API</param>  
        /// <param name="state">A value passed by application to prevent Cross-site request forgery attack</param>  
        /// <param name="session_state">session state</param>  
        /// <returns></returns>  
        [HttpGet]
        public async Task<ActionResult> SaveGoogleUser(string code, string state, string session_state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return View("Error");
            }

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.googleapis.com")
            };
            var requestUrl = $"oauth2/v4/token?code={code}&client_id={ClientId}&client_secret={SecretKey}&redirect_uri={RedirectUrl}&grant_type=authorization_code";

            var dict = new Dictionary<string, string>
            {
                { "Content-Type", "application/x-www-form-urlencoded" }
            };
            var req = new HttpRequestMessage(HttpMethod.Post, requestUrl) { Content = new FormUrlEncodedContent(dict) };
            var response = await httpClient.SendAsync(req);
            var token = JsonConvert.DeserializeObject<GmailToken>(await response.Content.ReadAsStringAsync());
            Session["user"] = token.AccessToken;
            var obj = await GetuserProfile(token.AccessToken);

            //IdToken property stores user data in Base64Encoded form  
            //var data = Convert.FromBase64String(token.IdToken.Split('.')[1]);  
            //var base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);  

            return View("UserProfile", obj);
        }

        /// <summary>  
        /// To fetch User Profile by access token  
        /// </summary>  
        /// <param name="accesstoken">access token</param>  
        /// <returns>User Profile page</returns>  
        public async Task<UserProfile> GetuserProfile(string accesstoken)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.googleapis.com")
            };
            string url = $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={accesstoken}";
            var response = await httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<UserProfile>(await response.Content.ReadAsStringAsync());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Send an OpenID Connect sign-in request.
        /// Alternatively, you can just decorate the SignIn method with the [Authorize] attribute
        /// </summary>
        public void SignIn()
        {
            //if not already logged in
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }

        }

        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            //HttpContext.GetOwinContext().Authentication.SignOut(
            //    OpenIdConnectAuthenticationDefaults.AuthenticationType,
            //    CookieAuthenticationDefaults.AuthenticationType);

            HttpContext.GetOwinContext().Authentication.SignOut(new AuthenticationProperties { RedirectUri = "https://azurehclkantar.azurewebsites.net/" }, 
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

    }
}
