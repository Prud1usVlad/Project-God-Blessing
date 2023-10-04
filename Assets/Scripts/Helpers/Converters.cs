using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class Converters
    {
        public static string IntToUiString(int num, int decimals = 2)
        {
            var abs = Mathf.Abs(num);

            if (abs < 1000)
                return Math.Round((double)num, decimals).ToString();
            else if (abs < 1000 * 1000)
                return Math.Round(num / 1000d, decimals).ToString() + "K";
            else if (abs < 1000 * 1000 * 1000)
                return Math.Round(num / (1000d * 1000d), decimals).ToString() + "M";
            else return "none";
        } 
    }
}
