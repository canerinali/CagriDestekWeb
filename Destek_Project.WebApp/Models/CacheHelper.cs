using Destek.BLL;
using Destek.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Destek_Project.WebApp.Models
{
    public class CacheHelper
    {
        public static List<Brans> GetCategoriesFromCache()
        {
            var result = WebCache.Get("brans-cache");

            if (result == null)
            {
                BransManager bransManager = new BransManager();
                result = bransManager.List();

                WebCache.Set("brans-cache", result, 20, true);
            }

            return result;
        }
        public static List<Message> GetPostFromCache()
        {
            var result = WebCache.Get("message-cache");

            if (result == null)
            {
                MessageManager messageManager = new MessageManager();
                result = messageManager.List();

                WebCache.Set("message-cache", result, 20, true);
            }

            return result;
        }
        public static void RemoveCategoriesFromCache()
        {
            Remove("brans-cache");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}
