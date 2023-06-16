using System.Text;
using Microsoft.AspNetCore.Mvc;
using Pfm.Api.Helpers;
using Pfm.Api.ViewModels;
using Pfm.Core.Entities;
using Pfm.Core.Helpers;
using Pfm.Core.Interfaces;

namespace Pfm.Api.Controllers;

[Route("api/[controller]")]
public class PresenceController : Controller
{
    private readonly IPresence presence;

    public PresenceController(IPresence presence)
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
        if(result.Count() == 0) 
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
            CreatedBy = x.CreatedBy 
        }
        ).ToList();
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
        if(result == null) 
        {
            err = "data not found";
            goto GotoReturn;
        }
        ret.IdPegawai = result.IdPegawai;
        ret.Nama = result.Nama;
        ret.Nip = result.Nip;
        ret.UrlFile = $"{AppSetting.BaseUrl}{AppSetting.UrlFileUser}/{result.Foto}";
        ret.CreatedBy = result.CreatedBy;
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
        IFormFile foto
    )
    {
        string err = string.Empty;
        
        TbPegawai tbPegawai = new TbPegawai
        {
            Nip = nip,
            Nama = nama,
            Foto = null,
            CreatedBy = 1
        };
        string pathFile = $"{AppSetting.PathFileUser}";
        if (foto != null && foto.Length >= 0)
            tbPegawai.Foto = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
        presence.AddPegawai(tbPegawai,
        successAction => { 
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
        IFormFile foto
    )
    {
        string err = string.Empty;
        var result = presence.GetPegawai(idPegawai);
        if(result == null) 
        {
            err = "data not found";
            goto GotoReturn;
        }
        TbPegawai tbPegawai = new TbPegawai
        {
            IdPegawai = idPegawai,
            Nip = nip,
            Nama = nama,
            Foto = null,
            CreatedBy = 1
        };
        string pathFile = $"{AppSetting.PathFileUser}";
        string fileOld = "";
        if (foto != null && foto.Length >= 0) {
            fileOld = $"{AppSetting.PathFileUser}/{result.Foto}";
            tbPegawai.Foto = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
        }
        presence.EditPegawai(tbPegawai,
        successAction => {
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
        if(data == null)
        {
            err ="data not found";
            goto GotoReturn;
        }
        string fileOld = $"{AppSetting.PathFileUser}/{data.Foto}";
        presence.DeletePegawai(idPegawai,
        successAction => { 
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

    [HttpPost("Checkin", Name = "UsersCheckin")]
    public IActionResult Checkin(string bitmap_face)
    {
        var path = $"{Path.Combine(AppSetting.PathFileUser)}/{Guid.NewGuid()}_presensi.png";
        var png_bytes = Convert.FromBase64String(bitmap_face);
        
        string err = "";
        PegawaiViewModel pegawaiView = new();
        TbPegawai tbPegawai = new();
        presence.Checkin(png_bytes, 
                successAction => {
                    tbPegawai = successAction;
                    System.IO.File.WriteAllBytes(path, png_bytes);
                }, 
                failAction => {
                    if(!string.IsNullOrEmpty(failAction))
                    {
                        err = failAction; 
                    }
                }
            );

        if(err == "") 
        {
            pegawaiView.IdPegawai = tbPegawai.IdPegawai;
            pegawaiView.Nama = tbPegawai.Nama;
            pegawaiView.Nip = tbPegawai.Nip;
            pegawaiView.UrlFile = $"{AppSetting.BaseUrl}{AppSetting.UrlFileUser}/{tbPegawai.Foto}";
            pegawaiView.CreatedBy = tbPegawai.CreatedBy;
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
}
