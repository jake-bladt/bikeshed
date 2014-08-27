using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

using MediaRadarExercise.MediaRadar.AdService;
using MediaRadarExercise.MediaRadar;

namespace MediaRadarExercise.Controllers
{
    public class AdDataController : Controller
    {
        [OutputCache(Duration=600,VaryByParam="*")]
        public ActionResult Index(int startMonth, int endMonth, int pageSize = 20, int skipCount = 0, string sortBy = "brandname")
        {
            var allAds = GetAds(startMonth, endMonth);

            var ads = (from ad in allAds
                      orderby ad.FieldValue(sortBy) 
                      select ad).Skip(skipCount).Take(pageSize);
            var ret = new { count = allAds.Count(), ads = ads };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration=600, VaryByParam = "*")]
        public ActionResult BigCovers(int startMonth, int endMonth, int pageSize = 20, int skipCount = 0, string sortBy = "brandname")
        {
            var allAds = GetAds(startMonth, endMonth);

            var bigCoverAds = (from ad in allAds
                               orderby ad.FieldValue(sortBy)
                               where ad.Position == "Cover" && ad.NumPages >= (decimal)0.5
                               select ad);
            var ads = bigCoverAds.Skip(skipCount).Take(pageSize);
            var ret = new { count = bigCoverAds.Count(), ads = ads };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration=600, VaryByParam = "*")]
        public ActionResult TotalPageCoverage(int startMonth, int endMonth)
        {
            var allAds = GetAds(startMonth, endMonth);

            var aggregates = (from ad in allAds
                      group ad by ad.Brand.BrandName into g
                      select new { 
                        BrandName = g.Key,
                        TotalCoverage = g.Sum(a => a.NumPages)
                      });
            var ads = (from agg in aggregates
                      orderby agg.TotalCoverage descending, agg.BrandName ascending
                      select agg).Take(5);

            return Json(ads, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration=600, VaryByParam = "*")]
        public ActionResult BigAds(int startMonth, int endMonth)
        {
            var allAds = GetAds(startMonth, endMonth);

            var aggregates = (from ad in allAds
                              group ad by ad.Brand.BrandName into g
                              select new
                              {
                                  BrandName = g.Key,
                                  LargestAd = g.Max(a => a.NumPages)
                              });
            var ads = (from agg in aggregates
                       orderby agg.LargestAd descending, agg.BrandName ascending
                       select agg).Take(5);

            return Json(ads, JsonRequestBehavior.AllowGet);
        }

        private static DateTime DateFromMonth(int month)
        {
            int year = month / 100;
            int monthNum = month % 100;
            return new DateTime(year, monthNum, 1);
        }

        private static Ad[] GetAds(int startMonth, int endMonth)
        {
            string key = String.Concat("adData.", startMonth, endMonth);
            var cache = System.Web.HttpContext.Current.Cache;
            if (null == cache[key]) RegenerateCache(startMonth, endMonth, cache, key);
            return  (Ad[])cache[key];
        }

        private static void RegenerateCache(int startMonth, int endMonth, Cache cache, string dataKey)
        {
            // Request from MediaRadar.
            var client = new MediaRadar.AdService.AdDataServiceClient();
            var startDate = DateFromMonth(startMonth);
            var endDate = DateFromMonth(endMonth);
            var ads = client.GetAdDataByDateRange(startDate, endDate);
            
            // Store Service Data
            cache.Insert(dataKey, ads, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
        }
        
    }
}
