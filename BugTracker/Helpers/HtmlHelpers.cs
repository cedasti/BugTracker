using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.HtmlHelpers
{
    public static class HtmlHelpers
    {
        public static string Truncate(this HtmlHelper helper, string item, int length)
        {
            if (item.Length <= length)
            {
                return item;
            }
            else
            {
                return item.Substring(0, length) + "...";
            }
        }
    }
}
