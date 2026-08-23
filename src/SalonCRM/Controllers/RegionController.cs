using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalonCRM.Web;
using SalonCRM.Manager;

namespace SalonCRM.Controllers
{
    [Authorize]
    public class RegionController : Controller
    {
        // GET: Region
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppProvinceList()
        {
            var list = (from _ in CommonManager.GetProvinces()
                        select new
                        {
                            key = _.Code,
                            Name = _.Name,
                        }).ToList();
            return Json(list);
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppCityList(string province)
        {
            var list = (from _ in CommonManager.GetCitys(province)
                        select new
                        {
                            key = _.Code,
                            Name = _.Name,
                        }).ToList();
            return Json(list);
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppRegionList(string city)
        {
            var list = (from _ in CommonManager.GetRegions(city)
                        select new
                        {
                            key = _.Code,
                            Name = _.Name,
                        }).ToList();
            return Json(list);
        }
    }
}