﻿using System;
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

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHistory()
        {
            var history = _lotsManager.GetAllLots();
            var response = history
                .Skip(890)
                .Take(300)
                .OrderBy(d=>d.Date)
                .Select<LottoStatisticsAnalyzer.Lot, object>(h =>
            {
                return new { date = h.Date.ToString("yyyy-MM-dd"), drop = h.Drops[0].Value };
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}