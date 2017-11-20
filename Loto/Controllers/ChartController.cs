using System;
using System.Linq;
using System.Web.Mvc;
using LottoStatisticsAnalyzer.Managers;

namespace Loto.Controllers
{
    public class ChartController : Controller
    {
        private LotsManager _lotsManager;
        public ChartController()
        {
            _lotsManager = new LotsManager();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetHistory(int index)
        {
            var history = _lotsManager.GetAllLots(index);
            var response = history
                .OrderBy(d=>d.Date)
                .Select<LottoStatisticsAnalyzer.Lot, object>(h =>
            {
                return new { date = h.Date.ToString("yyyy-MM-dd"), drop = h.Drops[index].Value, diff = h.Drops[index].Diff };
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}