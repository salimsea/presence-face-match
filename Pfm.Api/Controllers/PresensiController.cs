using System.Text;
using Microsoft.AspNetCore.Mvc;
using Pfm.Api.Helpers;
using Pfm.Api.ViewModels;
using Pfm.Core.Entities;
using Pfm.Core.Helpers;
using Pfm.Core.Interfaces;

namespace Pfm.Api.Controllers;

[Route("api/[controller]")]
public class PresensiController : Controller
{
    private readonly IPresensi presence;

    public PresensiController(IPresensi presence)
    {
        this.presence = presence;
    }

    protected TbUser UserInfo()
    {
        TbUser ret = (TbUser)HttpContext.Items["User"]!;
        return ret;
    }

    #region PEGAWAI MODUL
    [Authorize]
    [HttpGet("GetPegawais", Name = "GetPegawais")]
    public IActionResult GetPegawais()
    {
        string err = string.Empty;
        List<PegawaiViewModel> ret = new();
        var result = presence.GetPegawais();
        if (!result.Any())
        {
            err = "data not found";
            goto GotoReturn;
        }
        ret = result.Select(x => new PegawaiViewModel()
        {
            IdPegawai = x.IdPegawai,
            Nama = x.Nama,
            Nip = x.Nip,
            UrlFile = $"{AppSetting.BaseUrl}{AppSetting.UrlFileUser}/{x.Foto}",
            Status = x.Status,
            CreatedBy = x.User.Nama
        }
        ).OrderByDescending(x => x.IdPegawai).ToList();
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<List<PegawaiViewModel>>()
        {
            Data = ret,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    [Authorize]
    [HttpGet("GetPegawai", Name = "GetPegawai")]
    public IActionResult GetPegawai(int idPegawai)
    {
        string err = string.Empty;
        PegawaiViewModel ret = new();
        var result = presence.GetPegawai(idPegawai);
        if (result == null)
        {
            err = "data not found";
            goto GotoReturn;
        }
        ret.IdPegawai = result.IdPegawai;
        ret.Nama = result.Nama;
        ret.Nip = result.Nip;
        ret.Status = result.Status;
        ret.UrlFile = $"{AppSetting.BaseUrl}{AppSetting.UrlFileUser}/{result.Foto}";
        ret.CreatedBy = result.User.Nama;
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<PegawaiViewModel>()
        {
            Data = ret,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    [Authorize]
    [HttpPost("AddPegawai", Name = "AddPegawai")]
    public IActionResult AddPegawai(
        string nip,
        string nama,
        int status,
        IFormFile foto
    )
    {
        string err = string.Empty;

        TbPegawai tbPegawai = new TbPegawai
        {
            Nip = nip,
            Nama = nama,
            Foto = null,
            Status = status,
            CreatedBy = 1
        };
        string pathFile = $"{AppSetting.PathFileUser}";
        if (foto != null && foto.Length >= 0)
            tbPegawai.Foto = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
        presence.AddPegawai(tbPegawai,
        successAction =>
        {
            if (successAction != null)
            {
                if (foto != null && foto.Length >= 0)
                {
                    FileHelper.SaveImage(pathFile, tbPegawai.Foto, foto, false);
                }
            }
        },
        failAction => { err = failAction; });
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<string>()
        {
            Data = null,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    [Authorize]
    [HttpPost("EditPegawai", Name = "EditPegawai")]
    public IActionResult EditPegawai(
        int idPegawai,
        string nip,
        string nama,
        int status,
        IFormFile foto
    )
    {
        string err = string.Empty;
        var result = presence.GetPegawai(idPegawai);
        if (result == null)
        {
            err = "data not found";
            goto GotoReturn;
        }
        TbPegawai tbPegawai = new TbPegawai
        {
            IdPegawai = idPegawai,
            Nip = nip,
            Nama = nama,
            Foto = result.Foto,
            Status = status,
            CreatedBy = 1
        };
        string pathFile = $"{AppSetting.PathFileUser}";
        string fileOld = "";
        if (foto != null && foto.Length >= 0)
        {
            fileOld = $"{AppSetting.PathFileUser}/{result.Foto}";
            tbPegawai.Foto = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
        }
        presence.EditPegawai(tbPegawai,
        successAction =>
        {
            if (successAction != null)
            {
                if (foto != null && foto.Length >= 0)
                {
                    FileHelper.DeleteFile(fileOld);
                    FileHelper.SaveImage(pathFile, tbPegawai.Foto, foto, false);
                }
            }
        },
        failAction => { err = failAction; });
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<string>()
        {
            Data = null,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }

    [Authorize]
    [HttpDelete("DeletePegawai", Name = "DeletePegawai")]
    public IActionResult DeletePegawai(int idPegawai)
    {
        string err = string.Empty;
        var data = presence.GetPegawai(idPegawai);
        if (data == null)
        {
            err = "data not found";
            goto GotoReturn;
        }
        string fileOld = $"{AppSetting.PathFileUser}/{data.Foto}";
        presence.DeletePegawai(idPegawai,
        successAction =>
        {
            if (successAction != null)
            {
                FileHelper.DeleteFile(fileOld);
            }
        },
        failAction => { err = failAction; });
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<string>()
        {
            Data = null,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }

    #endregion

    #region PRESENSI MODUL
    [HttpPost("Checkin", Name = "UsersCheckin")]
    public IActionResult Checkin(string bitmap_face)
    {
        var fileName = $"{Guid.NewGuid()}_presensi.png";
        var path = $"{Path.Combine(AppSetting.PathFileUser)}/{fileName}";
        string err = "";
        byte[] png_bytes;
        PegawaiViewModel pegawaiView = new();
        try
        {
            png_bytes = Convert.FromBase64String(bitmap_face);
        }
        catch (System.Exception ex)
        {
            err = ex.Message;
            goto GotoReturn;
        }

        TbPegawai tbPegawai = new();
        presence.Checkin(png_bytes, fileName,
                successAction =>
                {
                    if (successAction != null)
                    {
                        tbPegawai = successAction;
                        System.IO.File.WriteAllBytes(path, png_bytes);
                    }
                },
                failAction => { err = failAction; }
            );

        if (string.IsNullOrEmpty(err))
        {
            pegawaiView = new();
            pegawaiView.IdPegawai = tbPegawai.IdPegawai;
            pegawaiView.Nama = tbPegawai.Nama;
            pegawaiView.Nip = tbPegawai.Nip;
            pegawaiView.UrlFile = $"{AppSetting.BaseUrl}{AppSetting.UrlFileUser}/{tbPegawai.Foto}";
            pegawaiView.PresensiHariIni = new();
            var presensi = presence.GetPresensis().Where(x => x.IdPegawai == tbPegawai.IdPegawai && x.Tanggal.Date == DateTime.Now.Date).FirstOrDefault();
            if (presensi != null)
            {
                pegawaiView.PresensiHariIni.JamHadir = presensi.JamHadir.ToString(@"hh\:mm");
                pegawaiView.PresensiHariIni.JamKeluar = presensi.JamKeluar == null ? null : ((TimeSpan)presensi.JamKeluar).ToString(@"hh\:mm");
            }
        }

        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<PegawaiViewModel>()
        {
            Data = pegawaiView,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    [HttpPost("CheckinManual", Name = "CheckinManual")]
    public IActionResult CheckinManual(List<int> idPegawai, string tanggal)
    {
        string err = "";
        List<TbPresensi> tbPresensis = new();
        var pengaturan = presence.GetPengaturan();
        foreach (var item in idPegawai)
        {
            var pegawai = presence.GetPegawai(item);
            if (pegawai == null) { err = "pegawai tidak ada!"; goto GotoReturn; }
            DateTime dtTanggal = DateTimeHelper.StrToDateTime(tanggal);
            var cek = presence.GetPresensis().Where(x => x.IdPegawai == item && x.Tanggal == dtTanggal).FirstOrDefault();
            if (cek != null) { err = $"nama pegawai [{cek.Pegawai.Nama}] sudah melakukan absen pada tanggal [{tanggal}]!"; goto GotoReturn; }
            tbPresensis.Add(new()
            {
                IdPegawai = item,
                FotoHadir = "no-image.png",
                FotoKeluar = "no-image.png",
                JamHadir = pengaturan.WaktuHadir,
                JamKeluar = pengaturan.WaktuKeluar,
                WaktuHadir = pengaturan.WaktuHadir,
                WaktuKeluar = pengaturan.WaktuKeluar,
                Tanggal = dtTanggal
            });
        }
        presence.CheckinManual(tbPresensis, successAction => { }, failAction => { err = failAction; });

        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<PegawaiViewModel>()
        {
            Data = null,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    [Authorize]
    [HttpGet("GetPresensis", Name = "GetPresensis")]
    public IActionResult GetPresensis(string tanggalAwal, string tanggalAkhir)
    {
        string err = string.Empty;
        List<PresensiViewModel> ret = new();
        var presensis = presence.GetPresensis();
        if (!string.IsNullOrEmpty(tanggalAwal))
            presensis = presensis.Where(x => x.Tanggal >= DateTimeHelper.SetDateTime(tanggalAwal) && x.Tanggal <= DateTimeHelper.StrToDateTime(tanggalAkhir)).ToList();

        ret = presensis.Select(x => new PresensiViewModel()
        {
            IdPresensi = x.IdPresensi,
            JamHadir = x.JamHadir.ToString(@"hh\:mm"),
            JamKeluar = x.JamKeluar == null ? null : ((TimeSpan)x.JamKeluar).ToString(@"hh\:mm"),
            FotoHadir = $"{AppSetting.BaseUrl}{AppSetting.UrlFileUser}/{x.FotoHadir}",
            FotoKeluar = $"{AppSetting.BaseUrl}{AppSetting.UrlFileUser}/{x.FotoKeluar}",
            Nama = x.Pegawai?.Nama,
            Tanggal = x.Tanggal.ToString("dd-MM-yyyy")
        }).ToList();
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<List<PresensiViewModel>>()
        {
            Data = ret,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    #endregion

    #region PENGATURAN MODUL
    [HttpPost("SetPengaturan", Name = "SetPengaturan")]
    public IActionResult SetPengaturan(string waktuHadir, string waktuKeluar, string hightlight)
    {
        string err = "";
        TbPengaturan tbPengaturan = new()
        {
            IdPengaturan = 1,
            WaktuHadir = DateTimeHelper.StrToTime(waktuHadir),
            WaktuKeluar = DateTimeHelper.StrToTime(waktuKeluar),
            Hightlight = hightlight,
        };
        presence.SetPengaturan(tbPengaturan, successAction => { }, failAction => { err = failAction; });
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<PegawaiViewModel>()
        {
            Data = null,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    [HttpGet("GetPengaturan", Name = "GetPengaturan")]
    public IActionResult GetPengaturan()
    {
        string err = "";
        TbPengaturan ret = presence.GetPengaturan();
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<TbPengaturan>()
        {
            Data = ret,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    #endregion

    [Authorize]
    [HttpGet("DownloadLaporanPresensi", Name = "DownloadLaporanPresensi")]
    public ActionResult DownloadLaporanPresensi(string tanggalAwal, string tanggalAkhir)
    {
        var result = presence.GetPresensis();
        if (!string.IsNullOrEmpty(tanggalAwal))
            result = result.Where(x => x.Tanggal >= DateTimeHelper.SetDateTime(tanggalAwal) && x.Tanggal <= DateTimeHelper.StrToDateTime(tanggalAkhir)).ToList();

        if (result == null)
            return RedirectToAction("Error", "Webs", new { ErrorMessage = "data tidak ada" });
        ReportPresenceExcelDailyViewModel ret = new()
        {
            Unit = "Semua",
            Periode = string.IsNullOrEmpty(tanggalAwal) ? "Semua" : $"{DateTimeHelper.DateToLongString("id", DateTimeHelper.SetDateTime(tanggalAwal))} s.d. {DateTimeHelper.DateToLongString("id", DateTimeHelper.SetDateTime(tanggalAkhir))}",
            Lists = new()
        };
        int no = 0;
        foreach (var user in result)
        {
            ReportPresenceUserExcelDailyViewModel model = new()
            {
                No = ++no,
                Nama = user.Pegawai.Nama,
                Tanggal = DateTimeHelper.DateToLongString("id", user.Tanggal),
                JamMasuk = user.JamHadir.ToString(@"hh\:mm"),
                JamPulang = user.JamKeluar != null ? ((TimeSpan)user.JamKeluar).ToString(@"hh\:mm") : null
            };
            ret.Lists.Add(model);
        }
        var filename = ExcelHelper.GenerateReportPresenceDaily(UserInfo(), ret);

        var bytes = System.IO.File.ReadAllBytes(filename);
        System.IO.File.Delete(filename);
        string fileDownload = $"laporan_kehadiran_pegawai.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownload);
    }

}
