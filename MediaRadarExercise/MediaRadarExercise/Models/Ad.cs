using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaRadarExercise.MediaRadar.AdService
{
    public partial class Ad
    {
        public object FieldValue(string fieldName)
        {
            object ret = null;
            switch (fieldName.ToLower())
            {
                case "adid":
                    ret = this.AdId;
                    break;
                case "brand":
                case "brandname":
                    ret = this.Brand.BrandName;
                    break;
                case "brandid":
                    ret = this.Brand.BrandId;
                    break;
                case "numpages":
                    ret = this.NumPages;
                    break;
                case "position":
                    ret = this.Position;
                    break;
            }
            return ret;
        }

    }
}