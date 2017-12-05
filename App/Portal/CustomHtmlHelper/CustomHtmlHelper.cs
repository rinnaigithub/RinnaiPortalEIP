using Portal.Models.AccountModels;
using PortalDataEntities.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.CustomHtmlHelper
{
    public static class CustomHtmlHelper
    {
        private class BreadcrumbModel
        {
            public string MenuID { get; set; }
            public string MenuName { get; set; }
            public string MenuPath { get { return "#"; } }
        }

        public static MvcHtmlString Breadcrumb(this HtmlHelper helper, string controllerName, string actionName)
        {
            PORTALDB PorDB = new PORTALDB();
            string htmlStr = string.Empty;
            List<BreadcrumbModel> breadcrumb = new List<BreadcrumbModel>();
            string path = controllerName + "/" + actionName;
            var controller = PorDB.PTFunction.Where(o => o.FN_LINK == string.Concat(controllerName, "/")).FirstOrDefault();
            var action = PorDB.PTFunction.Where(o => o.FN_LINK == path).FirstOrDefault();

            if (action != null)
            {
                breadcrumb.Add(new BreadcrumbModel() { MenuID = action.MAP_MUID });
                for (; ; )
                {
                    var parentID = PorDB.PTMenu.Where(o => o.MUID == action.MAP_MUID).First().MUPID;
                    if (string.IsNullOrEmpty(parentID))
                    {
                        breadcrumb = breadcrumb.OrderBy(o => o.MenuID).ToList();
                        break;
                    }
                    breadcrumb.Add(new BreadcrumbModel() { MenuID = parentID });
                    action.MAP_MUID = parentID;
                }

                foreach (var b in breadcrumb)
                {
                    b.MenuName = PorDB.PTMenu.Where(o => o.MUID == b.MenuID).First().MU_NM;
                    htmlStr += string.Format(@"<a href=""{0}"" class=""current"">{1}</a>", b.MenuPath, b.MenuName);
                }
            }

            /*
            <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> Home</a>
            <a href="#" class="current">Buttons &amp; icons</a>
            */
            return MvcHtmlString.Create(htmlStr);
        }

        /// <summary>
        /// 製作無限下層的左側選單
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static MvcHtmlString SubMenuHandler(this HtmlHelper helper, MenuLinkModel menu)
        {
            string htmlStr = string.Empty;
            if (menu.SubMenu.Count > 0)
            {
                htmlStr = @"<li class=""submenu coustom-submenu"">";
                //htmlStr += string.Format(@"<a href=""#""><i class=""{0}""></i> <span>{1}</span> <span class=""label label-success"">{2}</span></a>", menu.MenuIcon, menu.MenuName, menu.SubMenu.Count);
                htmlStr += string.Format(@"<a href=""#""><i class=""{0}""></i> <span>{1}</span></a>", menu.MenuIcon, menu.MenuName);
                htmlStr += "<ul>";
                foreach (var sub in menu.SubMenu)
                {
                    if (sub.Enabled && sub.MenuEnabled)
                        htmlStr += chkHasSubMenu(sub);
                }
                htmlStr += "</ul>";
                htmlStr += "</li>";
            }
            else
            {
                htmlStr = string.Format(@"<li><a href=""/{0}""><i class=""{1}""></i> <span>{2}</span></a> </li>", menu.MenuPath, menu.MenuIcon, menu.MenuName);
            }
            return MvcHtmlString.Create(htmlStr);
        }

        private static string chkHasSubMenu(MenuLinkModel mu)
        {
            string htmlStr = string.Empty;
            if (mu.SubMenu.Count > 0)
            {
                htmlStr += @"<li class=""submenu coustom-submenu"">";
                htmlStr += string.Format(@"<a href=""#""><i class=""{0}""></i> <span>{1}</span> <span class=""label label-success"">{2}</span></a>", mu.MenuIcon, mu.MenuName, mu.SubMenu.Count);
                htmlStr += "<ul>";
                foreach (var sub in mu.SubMenu)
                {
                    htmlStr += string.Format(@"<li><a href=""/{0}""><i class=""{1}""></i> <span>{2}</span></a> </li>", sub.MenuPath, sub.MenuIcon, sub.MenuName);
                }
                htmlStr += "</ul>";
                htmlStr += "</li>";
            }
            else
            {
                htmlStr = string.Format(@"<li><a href=""/{0}""><i class=""{1}""></i> <span>{2}</span></a> </li>", mu.MenuPath, mu.MenuIcon, mu.MenuName);
            }
            return htmlStr;
        }
    }
} 