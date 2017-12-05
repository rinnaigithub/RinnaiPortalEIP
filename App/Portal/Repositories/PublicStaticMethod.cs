using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Portal.Repositories
{
    public static class PublicStaticMethod
    {
        public static string To10CharString(this DateTime? dt)
        {
            return string.Format("{0:yyyy/MM/dd}", (DateTime)dt);
        }

        public static string To10CharString(this DateTime dt)
        {
            return string.Format("{0:yyyy/MM/dd}", dt);
        }

        /// <summary>
        /// 取得WebConfig的AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        /// <summary>
        /// 取得所有Controller以及旗下的Action名稱
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetControllerNames()
        {
            Dictionary<string, List<string>> controllerList = new Dictionary<string, List<string>>();
            GetSubClasses<Controller>().ForEach(
                type => controllerList[type.Name.Replace("Controller", string.Empty)] = new List<string>());

            foreach (var con in controllerList)
                con.Value.AddRange(ActionNames(con.Key));
            return controllerList;
        }

        private static List<string> ActionNames(string controllerName)
        {
            var types =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where typeof(IController).IsAssignableFrom(t) &&
                        string.Equals(controllerName + "Controller", t.Name, StringComparison.OrdinalIgnoreCase)
                select t;

            var controllerType = types.FirstOrDefault();

            if (controllerType == null)
            {
                return Enumerable.Empty<string>().ToList();
            }
            return new ReflectedControllerDescriptor(controllerType)
                .GetCanonicalActions().Select(x => x.ActionName)
                .Distinct().ToList();
        }

        /// <summary>
        /// 取得Enum根據value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value)
        {
            var name = Enum.GetName(typeof(T), value);
            return name.ToEnum<T>();
        }

        public static T ToEnum<T>(this string value)
        {
            try
            {
                var t = (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return (T)Enum.Parse(typeof(T), "NotSet", true);
            }
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}