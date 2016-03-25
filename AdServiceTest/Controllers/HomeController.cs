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
        public static DateTime endDate = new DateTime(2011, 1, 2);//new DateTime(2011, 4, 1)

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
                if ((float)adData[i].NumPages >= 0.5f)
                {
                    filteredData.Add(adData[i]);
                }
            }

            //Convert to datatable: All ads
            FlatAd[] filteredAds = new FlatAd[adData.Length];
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
