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

    [HttpGet("GetPegawais", Name = "GetPegawais")]
    public IActionResult GetPegawais()
    {
        string err = string.Empty;
        List<TbPegawai> ret = new();
        var result = presence.GetPegawais();
        if(result.Count() == 0) 
        {
            err = "data not found";
            goto GotoReturn;
        }
        ret = result.OrderByDescending(x => x.IdPegawai).ToList();
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<List<TbPegawai>>()
        {
            Data = ret,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
    [HttpGet("GetPegawai", Name = "GetPegawai")]
    public IActionResult GetPegawai(int idPegawai)
    {
        string err = string.Empty;
        TbPegawai ret = new();
        var result = presence.GetPegawai(idPegawai);
        if(result == null) 
        {
            err = "data not found";
            goto GotoReturn;
        }
        ret = result;
        goto GotoReturn;
    GotoReturn:
        return Ok(new ResponseViewModel<TbPegawai>()
        {
            Data = ret,
            IsSuccess = string.IsNullOrEmpty(err),
            ReturnMessage = err
        });
    }
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
        {
            tbPegawai.Foto = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
        }
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
    [HttpPost("EditPegawai", Name = "EditPegawai")]
    public IActionResult EditPegawai(
        int idPegawai,
        string nip,
        string nama,
        IFormFile foto
    )
    {
        string err = string.Empty;
        TbPegawai tbPegawai = new TbPegawai
        {
            IdPegawai = idPegawai,
            Nip = nip,
            Nama = nama,
            Foto = null,
            CreatedBy = 1
        };
        presence.EditPegawai(tbPegawai,
        successAction => { },
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

    [HttpDelete("DeletePegawai", Name = "DeletePegawai")]
    public IActionResult DeletePegawai(int idPegawai)
    {
        string err = string.Empty;
        var tbUser = presence.GetPegawai(idPegawai);
        if(tbUser == null)
        {
            err ="data not found";
            goto GotoReturn;
        }
        presence.DeletePegawai(idPegawai,
        successAction => { },
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
    public string Checkin(string bitmap_face)
    {
        var path = Path.Combine(AppSetting.PathFileUser);
        var pathxx = $"{path}/file-foto.png";

        // var png_b64 = bitmap_face.Substring(22); // extract only base64 part.
        var png_bytes = Convert.FromBase64String(bitmap_face);
 
        System.IO.File.WriteAllBytes(pathxx, png_bytes);
        Console.WriteLine("The data has been written to the file.");
        
        string ret = "";
         presence.Checkin(png_bytes, 
                successAction => { 
                    ret = successAction; 
                }, 
                failAction => {
                    if(!string.IsNullOrEmpty(failAction))
                    {
                        ret = failAction; 
                    }
                }
            );
       

        goto GotoReturn;
    GotoReturn:
        return ret;
    }
}
