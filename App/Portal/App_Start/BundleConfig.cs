using System.Web.Optimization;

namespace Portal
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // ----------------------------------------------------------------------------------------------
            // jQuery
            // ----------------------------------------------------------------------------------------------
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            #region .net

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/site.css"));

            #endregion .net

            #region mainCssJs
            // ----------------------------------------------------------------------------------------------
            // main css
            // matrix-style.css 裡的圖片路徑img目錄，只能放置在跟目錄下CssRewriteUrlTransform無法將相對路徑更換
            // ----------------------------------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/Content/mainCss").Include(
                 "~/Content/bootstrap.min.css",
                 "~/Content/bootstrap-responsive.min.css",
                 "~/Content/fullcalendar.css",
                 "~/Content/select2.min.css",
                 "~/Content/PagedList.css"
            )
            .Include("~/Content/matrix-style.css", new CssRewriteUrlTransform())
            .Include("~/Content/matrix-media.css", new CssRewriteUrlTransform())
            .Include("~/Content/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform())
            .Include("~/Content/jquery-confirm.css", new CssRewriteUrlTransform())
            .Include("~/Content/pnotify.custom.min.css", new CssRewriteUrlTransform())
            .Include("~/Content/colorpicker.css", new CssRewriteUrlTransform())
            .Include("~/Content/datepicker.css", new CssRewriteUrlTransform())
            .Include(@"~/Content/tablesorter-themes\blue\style.css", new CssRewriteUrlTransform())
            );
            // ----------------------------------------------------------------------------------------------
            // main js
            // ----------------------------------------------------------------------------------------------
            string[] mainJsAry = new string[]
            {
                "~/Scripts/jquery.ui.custom.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/matrix.js",
                "~/Scripts/select2.full.min.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery-confirm.min.js",
                "~/Scripts/pnotify.custom.min.js",
                "~/Scripts/bootstrap-clockpicker.min.js",
                "~/Scripts/bootstrap-datepicker.min.js",
                "~/Scripts/bootstrap-datepicker.zh-TW.js",
                "~/Scripts/jquery.tablesorter.min.js",
                "~/Scripts/PortalAlert.js"
            };
            bundles.Add(new ScriptBundle("~/bundles/mainJs").Include(mainJsAry));

            #endregion mainCssJs

            #region 登入的js，css 因為圖片連結相對位置 需特別給CssRewriteUrlTransform

            // ----------------------------------------------------------------------------------------------
            // logen js
            // ----------------------------------------------------------------------------------------------
            string[] loginJsAry = new string[]
            {
                "~/Scripts/jquery-confirm.min.js" ,
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.validate.unobtrusive.min.js"
            };
            bundles.Add(new ScriptBundle("~/bundles/loginJs").Include(loginJsAry));

            // ----------------------------------------------------------------------------------------------
            // login css
            // ----------------------------------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/Content/loginCss")
          .Include("~/Content/login-page.css", new CssRewriteUrlTransform())
          .Include("~/Content/jquery-confirm.css", new CssRewriteUrlTransform())
          .Include("~/Content/font-awesome-4.7.0/css/font-awesome.css", new CssRewriteUrlTransform())
          );

            #endregion 登入的js，css 因為圖片連結相對位置 需特別給CssRewriteUrlTransform
        }
    }
}