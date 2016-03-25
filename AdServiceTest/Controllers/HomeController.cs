using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AdServiceTest.Controllers
{
    public class HomeController : Controller
    {
        struct FlatAd
        {
            public int AdId;
            public int BrandId;
            public string BrandName;
            public float NumPages;
            public string Position;
        }
        public static DateTime startDate = new DateTime(2011, 1, 1);
        public static DateTime endDate = new DateTime(2011, 4, 1);

        public string GetAllAdData()
        {
            WCFAdDataService.AdDataServiceClient adsc = new WCFAdDataService.AdDataServiceClient();
            WCFAdDataService.Ad[] adData = adsc.GetAdDataByDateRange(startDate, endDate);

            //Convert to datatable: All ads
            FlatAd[] allAds = new FlatAd[adData.Length];
            for (int i = 0; i < adData.Length; i++)
            {
                allAds[i].AdId = adData[i].AdId;
                allAds[i].BrandId = adData[i].Brand.BrandId;
                allAds[i].BrandName = adData[i].Brand.BrandName;
                allAds[i].NumPages = (float)adData[i].NumPages;
                allAds[i].Position = adData[i].Position;
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
                filteredAds[i].AdId = filteredData[i].AdId;
                filteredAds[i].BrandId = filteredData[i].Brand.BrandId;
                filteredAds[i].BrandName = filteredData[i].Brand.BrandName;
                filteredAds[i].NumPages = (float)filteredData[i].NumPages;
                filteredAds[i].Position = filteredData[i].Position;
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(filteredAds);

            return data;
        }
        private static int CompareAdsByCoverage(FlatAd x, FlatAd y)
        {
            return y.NumPages.CompareTo(x.NumPages);
        }
        public string GetTop5AdData()
        {
            WCFAdDataService.AdDataServiceClient adsc = new WCFAdDataService.AdDataServiceClient();
            WCFAdDataService.Ad[] adData = adsc.GetAdDataByDateRange(startDate, endDate);

            //Convert to datatable: All ads
            List<FlatAd> allAds = new List<FlatAd>();
            for (int i = 0; i < adData.Length; i++)
            {
                FlatAd newAd = new FlatAd();
                newAd.AdId = adData[i].AdId;
                newAd.BrandId = adData[i].Brand.BrandId;
                newAd.BrandName = adData[i].Brand.BrandName;
                newAd.NumPages = (float)adData[i].NumPages;
                newAd.Position = adData[i].Position;
                allAds.Add(newAd);
            }

            //Sort by ad coverage
            allAds.Sort(CompareAdsByCoverage);
            List<FlatAd> top5Ads = new List<FlatAd>();
            List<int> usedBrands = new List<int>();
            //Go through each all ad after it is sorted.  Add to this list until 5 ads
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

            //Convert to array: All ads
            FlatAd[] allAds = new FlatAd[adData.Length];
            for (int i = 0; i < adData.Length; i++)
            {
                allAds[i].AdId = adData[i].AdId;
                allAds[i].BrandId = adData[i].Brand.BrandId;
                allAds[i].BrandName = adData[i].Brand.BrandName;
                allAds[i].NumPages = (float)adData[i].NumPages;
                allAds[i].Position = adData[i].Position;
            }

            //Create list of brands
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
            
            //Create new list of FlatAds 
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
        
        public string[,,] GetData()
        {
            DateTime startDate = new DateTime(2011, 1, 1);
            DateTime endDate = new DateTime(2011, 2, 1);//new DateTime(2011, 4, 1)
            WCFAdDataService.AdDataServiceClient adsc = new WCFAdDataService.AdDataServiceClient();
            WCFAdDataService.Ad[] adData = adsc.GetAdDataByDateRange(startDate, endDate);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(adData);

            string[, ,] returnSet = new string[4, adData.Length, 5];

            //Convert to datatable: All ads
            DataTable allAds = new DataTable();
            allAds.Columns.Add("AdId", typeof(int));
            allAds.Columns.Add("BrandId", typeof(int));
            allAds.Columns.Add("BrandName", typeof(string));
            allAds.Columns.Add("NumPages", typeof(float));
            allAds.Columns.Add("Position", typeof(string));
            for (int i = 0; i < adData.Length; i++)
            {
                DataRow adRow = allAds.NewRow();
                adRow["AdId"] = adData[i].AdId;
                adRow["BrandId"] = adData[i].Brand.BrandId;
                adRow["BrandName"] = adData[i].Brand.BrandName;
                adRow["NumPages"] = adData[i].NumPages;
                adRow["Position"] = adData[i].Position;
                allAds.Rows.Add(adRow);
            }

            //Datatable: Cover position with 50%+ coverage
            DataTable highCoverAds = allAds.Clone();
            DataRow[] highCoverAdRows = allAds.Select("NumPages > 0.5", "BrandName DESC");
            foreach (DataRow dr in highCoverAdRows)
            {
                highCoverAds.ImportRow(dr);
            }

            //Datatable:  Top 5 ads by coverage amount, distinct by brand
            //select top 5 AdID from allAds group by brandid sort by numpages desc
            DataTable top5CoverAds = allAds.Clone();
            //DataRow top5CoverAdRows = allAds.Select();

            //Datatable:  Top 5 brands by page coverage amount.  Columns: Brand name, coverage
            DataTable top5Brands = new DataTable();
            top5Brands.Columns.Add("BrandId", typeof(int));
            top5Brands.Columns.Add("BrandName", typeof(string));
            top5Brands.Columns.Add("NumPages", typeof(float));


            //Convert back to string[][]
            int row = 0;
            foreach (DataRow dr in allAds.Rows)
            {
                returnSet[0, row, 0] = dr["AdId"].ToString();
                returnSet[0, row, 1] = dr["BrandId"].ToString();
                returnSet[0, row, 2] = dr["BrandName"].ToString();
                returnSet[0, row, 3] = dr["NumPages"].ToString();
                returnSet[0, row, 4] = dr["Position"].ToString();
                row++;
            }

            row = 0;
            foreach (DataRow dr in highCoverAds.Rows)
            {
                returnSet[1, row, 0] = dr["AdId"].ToString();
                returnSet[1, row, 1] = dr["BrandId"].ToString();
                returnSet[1, row, 2] = dr["BrandName"].ToString();
                returnSet[1, row, 3] = dr["NumPages"].ToString();
                returnSet[1, row, 4] = dr["Position"].ToString();
                row++;
            }

            row = 0;
            foreach (DataRow dr in top5CoverAds.Rows)
            {
                returnSet[2, row, 0] = dr["AdId"].ToString();
                returnSet[2, row, 1] = dr["BrandId"].ToString();
                returnSet[2, row, 2] = dr["BrandName"].ToString();
                returnSet[2, row, 3] = dr["NumPages"].ToString();
                returnSet[2, row, 4] = dr["Position"].ToString();
                row++;
            }

            row = 0;
            foreach (DataRow dr in top5Brands.Rows)
            {
                returnSet[3, row, 0] = dr["AdId"].ToString();
                returnSet[3, row, 1] = dr["BrandId"].ToString();
                returnSet[3, row, 2] = dr["BrandName"].ToString();
                returnSet[3, row, 3] = dr["NumPages"].ToString();
                returnSet[3, row, 4] = dr["Position"].ToString();
                row++;
            }

            return returnSet;
        }
        public DateTime GetDate()
        {
            return DateTime.Today;
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
