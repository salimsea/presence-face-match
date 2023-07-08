using System;
namespace Pfm.Api.ViewModels
{
    public class PresensiViewModel
    {
        public int IdPresensi { get; set; }
        public string? Nama { get; set; }
        public string? Tanggal { get; set; }
        public string? JamHadir { get; set; }
        public string? JamKeluar { get; set; }
        public string? FotoHadir { get; set; }
        public string? FotoKeluar { get; set; }
    }
}

