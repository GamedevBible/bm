using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM
{
    public static class ValuesConverter
    {
        public static string LevelToPoints(int level)
        {
            switch(level)
            {
                case 1:
                    return "100";
                case 2:
                    return "200";
                case 3:
                    return "300";
                case 4:
                    return "500";
                case 5:
                    return "1 000";
                case 6:
                    return "2 000";
                case 7:
                    return "4 000";
                case 8:
                    return "8 000";
                case 9:
                    return "16 000";
                case 10:
                    return "32 000";
                case 11:
                    return "64 000";
                case 12:
                    return "125 000";
                case 13:
                    return "250 000";
                case 14:
                    return "500 000";
                case 15:
                    return "1 000 000";
                default:
                    return "0";
            }
        }

        public static string LevelToCheckPoints(int level)
        {
            if (level < 5)
                return "0";
            if (level >= 5 && level < 10)
                return "1000";
            if (level >= 10)
                return "32000";
            return "0";
        }
    }
}
