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
                .OrderBy(d => d.Date)
                .Select<LottoStatisticsAnalyzer.Lot, object>(h =>
            {
                return new { date = h.Date.ToString("yyyy-MM-dd"), drop = h.Drops[index].Value, diff = h.Drops[index].Diff };
            })
            .Skip(936);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDiffOnly(int position=0)
        {
            var history = _lotsManager.GetAllLots(position);
            var response = history
                .OrderByDescending(d => d.Date)
                .Select<LottoStatisticsAnalyzer.Lot, object>(h => //LottoStatisticsAnalyzer.Domain.Drop>(h =>
                {
                    //return new { date = h.Date.ToString("yyyy-MM-dd"), drop = h.Drops[0].Diff };
                    //return h.Drops[0].Diff;
                    return h.Drops.Select(d => d.Value).ToArray();
                    //return new { drop = h.Drops[0].Value, diff = h.Drops[0].Diff };
                });
                //.Skip(936);
            
            //.Take(100);

            var grouped = response.GroupBy(e => e).Select(g=> 
            {
                return new { Value = g.Key, Count = g.Count() };
            })
                .OrderBy(g => g.Count);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}