using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "Login",
            url: "dang-nhap-he-thong",
            defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            namespaces: new string[] { "DoAn.Web.Controllers" } // như này mơi đúng
           );
            routes.MapRoute(
             name: "Giam Sat He Thong",
             url: "quan-ly-giam-sat",
             defaults: new { controller = "GiamSat", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "DoAn.Web.Controllers " });
            routes.MapRoute( // cai default lúc nào cũng phải để cuôi cùng và k đc đổi. e đooir đi sao n chạy được. đã dèault còn đỏio gì
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           ); 
        }
    }
}
