using System;
using System.Web.Mvc;

namespace Loto.Controllers
{
    public class ChartController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHistory()
        {
            var history = new object[] {
                new {date = "10/01/2013",drop =10 },
                new {date ="10/02/2013",drop = 1 },
                new {date = "10/03/2013",drop = 22 },
                new {date = "10/04/2013",drop =10 },
                new {date ="10/05/2013",drop = 21 },
                new {date = "10/06/2013",drop = 26 },
                new {date = "10/07/2013",drop =3 },
                new {date ="10/08/2013",drop = 21 },
                new {date = "10/09/2013",drop = 15 },
                };

            return Json(history, JsonRequestBehavior.AllowGet);
        }
    }
}