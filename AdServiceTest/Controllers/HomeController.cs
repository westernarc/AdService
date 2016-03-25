using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization; 

namespace AdServiceTest.Controllers
{
    public class HomeController : Controller
    {
        public string GetData() { 
            WCFAdDataService.AdDataServiceClient adsc = new WCFAdDataService.AdDataServiceClient();
            WCFAdDataService.Ad[] adData = adsc.GetAdDataByDateRange(new DateTime(2011, 1, 1), new DateTime(2011, 4, 1));
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(adData);
            return data;
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
