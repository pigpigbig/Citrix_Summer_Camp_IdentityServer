using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yuan.MVC.Demo.Models;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Text;

namespace Yuan.MVC.Demo.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //触发身份认证
        [Authorize]
        public IActionResult Login()
        {
            ViewData["Message"] = "Secure page.";
            //System.Console.WriteLine("hello");
            return View();
        }

        
        //触发身份认证
        [Authorize]
        public IActionResult tech()
        {
            ViewData["Message"] = "Secure page.";
            return View();
        }


        [Authorize]
        public IActionResult Privacy()
        {
            ViewData["Message"] = "Secure page.";
            return View();
        }

        [Authorize]
        public IActionResult Products()
        {
            ViewData["Message"] = "Secure page.";
            return View();
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 测试请求API资源(api1)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> getApi()
        {
            var client = new HttpClient();
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (string.IsNullOrEmpty(accessToken))
            {
                return Json(new { msg = "accesstoken 获取失败" });
            }

            // client.SetBearerToken(accesstoken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //var httpResponse = await client.GetAsync("http://localhost:5003/api/identity/GetUserClaims");
            //var result = await httpResponse.Content.ReadAsStringAsync();
            //if (!httpResponse.IsSuccessStatusCode)
            //{
            //    return Json(new { msg = "请求 api1 失败。", error = result });
            //}
            //return Json(new
            //{
            //    msg = "成功",
            //    data = JsonConvert.DeserializeObject(result)
            //});
            var content = await client.GetStringAsync("http://localhost:5003/api/identity/GetUserClaims");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }
    }

}