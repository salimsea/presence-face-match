using Microsoft.AspNetCore.Mvc;
using Pfm.Core.Interfaces;

namespace Pfm.Api.Controllers;

[Route("api/[controller]")]
public class WeatherForecastController : Controller
{
    private readonly IPresence presence;

    public WeatherForecastController(IPresence presence)
    {
        this.presence = presence;
    }

    [HttpPost("Checkin", Name = "UsersCheckin")]
    public string Checkin(IFormFile face)
    {
        string ret = "";
        using (var ms = new MemoryStream())
        {
            face.CopyTo(ms);
            var fileBytes = ms.ToArray();
            
            presence.Checkin(fileBytes, 
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
        }
       

        goto GotoReturn;
    GotoReturn:
        return ret;
    }
}
