using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Pfm.Core.Helpers
{
    public class DateTimeHelper
    {
        private static readonly DateTime CurrDate = DateTime.Now;
        private static readonly string[] DayNameIds = new string[7] { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jum'at", "Sabtu" };
        private static readonly string[] DayNameEns = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesay", "Thursday", "Friday", "Saturday" };
        private static readonly string[] DayNameArs = new string[7] { "Ahad", "Senin", "Selasa", "Rabu", "Kamis", "Jum'at", "Sabtu" };
        private static readonly string[] ShortDayNameIds = new string[7] { "Mng", "Sen", "Sel", "Rab", "Kam", "Jum", "Sab" };
        private static readonly string[] ShortDayNameEns = new string[7] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        private static readonly string[] ShortDayNameArs = new string[7] { "Ahd", "Sen", "Sel", "Rab", "Kam", "Jum", "Sab" };
        private static readonly string[] MonthNameIds = new string[12] { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "Nopember", "Desember" };
        private static readonly string[] MonthNameEns = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private static readonly string[] MonthNameArs = new string[12] { "Muharram", "Safar", "Rabi’ul Awal", "Rabi’ul Akhir", "Jumadil Awal", "Jumadil Akhir", "Rajab", "Sya’ban", "Ramadhan", "Syawal", "Dzulkaidah", "Dzulhijjah" };
        private static readonly string[] ShortMonthNameIds = new string[12] { "Jan", "Feb", "Mar", "Apr", "Mei", "Jun", "Jul", "Agu", "Sep", "Okt", "Nop", "Des" };
        private static readonly string[] ShortMonthNameEns = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        private static readonly string[] ShortMonthNameArs = new string[12] { "Muh", "Saf", "Raw", "Rak", "Juw", "Juk", "Raj", "Sya", "Ram", "Sya", "Dzk", "Dzh" };

        public static bool IsDateTime(string StrDate)
        {
            try
            {
                if (StrDate.Length == 10)
                {
                    var cek = DateTime.ParseExact(StrDate.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    var cek = DateTime.ParseExact(StrDate.Replace("/", "-"), "dd-MM-yyyy HH24:mm:ss", CultureInfo.InvariantCulture);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static DateTime SetDateTime(string StrDate)
        {
            DateTime currdate = DateTime.Now;
            if (!string.IsNullOrEmpty(StrDate))
            {
                var retDate = DateTimeHelper.StrToDateTime(StrDate, out string err);
                if (string.IsNullOrEmpty(err))
                    currdate = (DateTime)retDate;
            }
            return currdate;

        }
        public static DateTime StrToDateTime(string StrDate)
        {
            if (StrDate.Length == 10)
                return DateTime.ParseExact(StrDate.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            else
                return DateTime.ParseExact(StrDate.Replace("/", "-"), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
        public static DateTime? StrToDateTime(string StrDate, out string oMessage)
        {
            try
            {
                oMessage = string.Empty;
                return (DateTime)StrToDateTime(StrDate);
            }
            catch (Exception ex)
            {
                oMessage = ex.Message;
                return null;
            }
        }
        public static bool IsTime(string StrTime)
        {
            try
            {
                var cek = TimeSpan.ParseExact(StrTime, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static TimeSpan StrToTime(string StrTime)
        {
            if (StrTime.Length == 8)
                return TimeSpan.ParseExact(StrTime, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
            else
                return TimeSpan.ParseExact(StrTime, @"hh\:mm", CultureInfo.InvariantCulture);
        }
        public static TimeSpan? StrToTime(string StrTime, out string oMessage)
        {
            try
            {
                oMessage = string.Empty;
                return (TimeSpan)StrToTime(StrTime);
            }
            catch (Exception ex)
            {
                oMessage = ex.Message;
                return null;
            }
        }

        public static string DateToLongString(string LanguageCode, DateTime date)
        {
            string strTanggal = date.Day.ToString();
            switch (LanguageCode)
            {
                case "id":
                    strTanggal += "-" + MonthNameIds[date.Month - 1];
                    break;
                case "ar":
                    strTanggal += "-" + MonthNameArs[date.Month - 1];
                    break;
                default:
                    strTanggal += "-" + MonthNameEns[date.Month - 1];
                    break;
            }
            strTanggal += "-" + date.Year;
            return strTanggal;
        }
        public static string DateToShortString(string LanguageCode, DateTime date)
        {
            string strTanggal = date.Day.ToString();
            switch (LanguageCode)
            {
                case "id":
                    strTanggal += "-" + ShortMonthNameIds[date.Month - 1];
                    break;
                case "ar":
                    strTanggal += "-" + ShortMonthNameIds[date.Month - 1];
                    break;
                default:
                    strTanggal += "-" + ShortMonthNameIds[date.Month - 1];
                    break;
            }
            strTanggal += "-" + date.Year;
            return strTanggal;
        }
        public static DateTime ConvertMasehi2Hijriyah(DateTime Date, out string oMessage)
        {
            try
            {
                oMessage = string.Empty;
                int date = Date.Day;
                int month = Date.Month;
                int year = Date.Year;
                int jd;
                if (year > 1528 || (year == 1528 && month > 10) || (year == 1528 && month == 10 && date > 14))
                {
                    jd = (int)((1461 * (year + 4800 + (int)((month - 14) / 12))) / 4);
                    jd += (int)((367 * (month - 2 - 12 * ((int)((month - 14) / 12)))) / 12);
                    jd -= (int)((3 * ((int)((year + 4900 + (int)((month - 14) / 12)) / 100))) / 4);
                    jd += date - 32075;
                }
                else
                {
                    jd = 367 * year;
                    jd -= (int)((7 * (year + 5001 + (int)((month - 9) / 7))) / 4);
                    jd += (int)((275 * month) / 9);
                    jd += date + 1729777;
                }

                _ = jd % 7;
                int l = jd - 1948440 + 10632;
                int n = (int)((l - 1) / 10631);
                l = l - 10631 * n + 354;
                int z = ((int)((10985 - l) / 5316)) * ((int)((50 * l) / 17719)) + ((int)(l / 5670)) * ((int)((43 * l) / 15238));
                l = l - ((int)((30 - z) / 15)) * ((int)((17719 * z) / 50)) - ((int)(z / 16)) * ((int)((15238 * z) / 43)) + 29;
                int m = (int)((24 * l) / 709);
                int d = l - (int)((709 * m) / 24);
                int y = 30 * n + z - 30;
                return new DateTime(y, m, d);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        public static string GetShortMonthName(string LanguageCode, int Month)
        {
            switch (LanguageCode)
            {
                case "id":
                    return ShortMonthNameIds[Month];
                case "ar":
                    return ShortMonthNameArs[Month];
                default:
                    return ShortMonthNameEns[Month];
            }
        }
        public static string GetLongMonthName(string LanguageCode, int Month)
        {
            switch (LanguageCode)
            {
                case "id":
                    return MonthNameIds[Month];
                case "ar":
                    return MonthNameArs[Month];
                default:
                    return MonthNameEns[Month];
            }
        }
        public static string[] GetDayNames(string LanguageCode)
        {
            switch (LanguageCode)
            {
                case "id":
                    return DayNameIds;
                case "ar":
                    return DayNameArs;
                default:
                    return DayNameEns;
            }
        }
        public static string GetShortDayName(string LanguageCode, int Day)
        {
            switch (LanguageCode)
            {
                case "id":
                    return ShortDayNameIds[Day];
                case "ar":
                    return ShortDayNameArs[Day];
                default:
                    return ShortDayNameEns[Day];
            }
        }
        public static string GetLongDayName(string LanguageCode, int Day)
        {
            switch (LanguageCode)
            {
                case "id":
                    return DayNameIds[Day];
                case "ar":
                    return DayNameArs[Day];
                default:
                    return DayNameEns[Day];
            }
        }
        public static DateTime GetFirstDate(DateTime date)
        {
            if (date.DayOfWeek == 0) return date;

            DateTime result = date.AddDays(-1);
            while (result.DayOfWeek != 0)
                result = result.AddDays(-1);
            return result;
        }
        public static DateTime GetLastDate(DateTime date)
        {
            if ((int)date.DayOfWeek == 6) return date;
            DateTime result = date.AddDays(1);
            while ((int)result.DayOfWeek != 6)
                result = result.AddDays(1);
            return result;
        }
    }
}

