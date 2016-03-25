using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AdServiceTest.Models;
namespace AdServiceTest.Controllers
{
    public class HomeController : Controller
    {
        //Start and end dates for ad data retrieval
        public static DateTime startDate = new DateTime(2011, 1, 1);
        public static DateTime endDate = new DateTime(2011, 4, 1);

        //Method for comparing Ads by NumPages; used for List.Sort()
        private static int CompareAdsByCoverage(FlatAd x, FlatAd y)
        {
            return y.NumPages.CompareTo(x.NumPages);
        }

        public string GetAllAdData()
        {
            WCFAdDataService.AdDataServiceClient adsc = new WCFAdDataService.AdDataServiceClient();
            WCFAdDataService.Ad[] adData = adsc.GetAdDataByDateRange(startDate, endDate);

            //Convert to datatable: All ads
            FlatAd[] allAds = new FlatAd[adData.Length];
            for (int i = 0; i < adData.Length; i++)
            {
                allAds[i] = new FlatAd(adData[i]);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(allAds);

            return data;
        }
        public string GetHighCoverAdData()
        {
            WCFAdDataService.AdDataServiceClient adsc = new WCFAdDataService.AdDataServiceClient();
            WCFAdDataService.Ad[] adData = adsc.GetAdDataByDateRange(startDate, endDate);

            List<WCFAdDataService.Ad> filteredData = new List<WCFAdDataService.Ad>();
            for (int i = 0; i < adData.Length; i++)
            {
                if ((float)adData[i].NumPages >= 0.5f && adData[i].Position == "Cover")
                {
                    filteredData.Add(adData[i]);
                }
            }

            //Convert to array: All ads
            FlatAd[] filteredAds = new FlatAd[filteredData.Count];
            for (int i = 0; i < filteredAds.Length; i++)
            {
                filteredAds[i] = new FlatAd(filteredData[i]);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(filteredAds);

            return data;
        }
        public string GetTop5AdData()
        {
            WCFAdDataService.AdDataServiceClient adsc = new WCFAdDataService.AdDataServiceClient();
            WCFAdDataService.Ad[] adData = adsc.GetAdDataByDateRange(startDate, endDate);

            //Convert to datatable: All ads
            List<FlatAd> allAds = new List<FlatAd>();
            for (int i = 0; i < adData.Length; i++)
            {
                FlatAd newAd = new FlatAd(adData[i]);
                allAds.Add(newAd);
            }

            //Sort by NumPages
            allAds.Sort(CompareAdsByCoverage);

            List<FlatAd> top5Ads = new List<FlatAd>();
            List<int> usedBrands = new List<int>();
            //Go through each ad after it is sorted by NumPages.  Add to Top 5 list until 5 ads
            //with distinct brand IDs fill the list
            foreach (FlatAd ad in allAds)
            {
                if (!usedBrands.Contains(ad.BrandId))
                {
                    top5Ads.Add(ad);
                    usedBrands.Add(ad.BrandId);
                }
                if (top5Ads.Count >= 5)
                {
                    break;
                }
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(top5Ads.ToArray());

            return data;
        }
        public string GetTop5BrandData()
        {
            WCFAdDataService.AdDataServiceClient adsc = new WCFAdDataService.AdDataServiceClient();
            WCFAdDataService.Ad[] adData = adsc.GetAdDataByDateRange(startDate, endDate);

            //Convert to array of flat ads
            FlatAd[] allAds = new FlatAd[adData.Length];
            for (int i = 0; i < adData.Length; i++)
            {
                allAds[i] = new FlatAd(adData[i]);
            }

            //Create list of brands and their NumPages sum
            Dictionary<string, float> brandList = new Dictionary<string, float>();
            for (int i = 0; i < allAds.Length; i++)
            {
                if (!brandList.Keys.Contains<string>(allAds[i].BrandName))
                {
                    brandList.Add(allAds[i].BrandName, 0f);
                }

                //Sum numpages value
                brandList[allAds[i].BrandName] += allAds[i].NumPages;
            }
            
            //Create new array of FlatAds from the dictionary sums
            //The datatable on the front page will only show the highest 5
            FlatAd[] filteredAds = new FlatAd[brandList.Count];
            int row = 0;
            foreach (KeyValuePair<string, float> kvp in brandList)
            {
                filteredAds[row].BrandName = kvp.Key;
                filteredAds[row].NumPages = kvp.Value;
                row++;
            }
            
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(filteredAds);

            return data;
        }
        
        public ActionResult Index()
        {
            return View();
        }

    }
}
