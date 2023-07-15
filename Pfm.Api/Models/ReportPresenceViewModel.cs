using System.Collections.Generic;
using Ispm.Core.Helpers;

namespace Pfm.Api.ViewModels
{
    public class ReportPresenceExcelMonthlyViewModel
    {
        public string Unit { get; set; }
        public string Periode { get; set; }
        public List<ReportPresenceUserExcelMonthlyViewModel> Lists { get; set; }

    }
    public class ReportPresenceUserExcelMonthlyViewModel
    {
        public int No { get; set; }
        public string IdUser { get; set; }
        public string ProfileFile { get; set; }
        public string Nip { get; set; }
        public string Nama { get; set; }
        public string Unit { get; set; }
        public int TepatWaktu { get; set; }
        public int Terlambat { get; set; }
        public int TidakHadir { get; set; }
        public double TotalJamKerja { get; set; }
        public int TotalHariKerja { get; set; }

    }
    public class ReportPresenceExcelDailyViewModel
    {
        public string Unit { get; set; }
        public string Periode { get; set; }
        public List<ReportPresenceUserExcelDailyViewModel> Lists { get; set; }
    }
    public class ReportPresenceUserExcelDailyViewModel
    {
        public int No { get; set; }
        public string Nip { get; set; }
        public string Nama { get; set; }
        public string Tanggal { get; set; }
        public string JamMasuk { get; set; }
        public string JamPulang { get; set; }
        public string StatusAbsen { get; set; }
        public double TotalJamKerja { get; set; }
        public string LokasiAbsen { get; set; }
        public string Keterangan { get; set; }
    }
}

