using System;
namespace Pfm.Api.ViewModels
{
    public class UserViewModel
    {
        public int IdUser { get; set; }
        public string? Nama { get; set; }
        public string? Email { get; set; }
    }
    public class PegawaiViewModel
    {
        public int IdPegawai { get; set; }
        public string? UrlFile { get; set; }
        public string? Nama { get; set; }
        public string? Nip { get; set; }
        public string CreatedBy { get; set; }
    }
}

