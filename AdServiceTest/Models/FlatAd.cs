using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdServiceTest.Models
{
    public class FlatAd
    {
        //Flattens the Ad object (Removes seperate Brand level)
        private int _AdId;
        private int _BrandId;
        private string _BrandName;
        private float _NumPages;
        private string _Position;
        public int AdId
        {
            get { return _AdId; }
            set { _AdId = value; }
        }
        public int BrandId
        {
            get { return _BrandId; }
            set { _BrandId = value; }
        }
        public string BrandName
        {
            get { return _BrandName; }
            set { _BrandName = value; }
        }
        public float NumPages
        {
            get { return _NumPages; }
            set { _NumPages = value; }
        }
        public string Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        public FlatAd()
        {
            AdId = 0;
            BrandId = 0;
            BrandName = "";
            NumPages = 0;
            Position = "";
        }
        public FlatAd(WCFAdDataService.Ad ad)
        {
            AdId = ad.AdId;
            BrandId = ad.Brand.BrandId;
            BrandName = ad.Brand.BrandName;
            NumPages = (float)ad.NumPages;
            Position = ad.Position;
        }
    }
}