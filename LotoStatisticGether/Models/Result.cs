using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotoStatisticGether.Models
{
    public class Result
    {
        private readonly string[] _lot;
        private readonly string _date;

        public Result(string[] lot, string date)
        {
            _lot = lot;
            _date = date;
        }

        public string[] Lot
        {
            get
            {
                return _lot;
            }
        }

        public string Date
        {
            get
            {
                return _date;
            }
        }
    }
}
