using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Pfm.Core.Interfaces;
using Pfm.Core.Entities;
using Pfm.Core.Models;
using Pfm.Api.ViewModels;

namespace Ispm.Api.Controllers
{
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly IPresence presence;
        public SecurityController(IPresence presence)
        {
            this.presence = presence;
        }
        protected TbUser UserInfo()
        {
            TbUser ret = (TbUser)HttpContext.Items["User"]!;
            return ret;
        }
        [HttpPost("Login", Name = "Login")]
        public IActionResult Login(string usernameOrEmail, string password)
        {
            string err = string.Empty;
            var token = string.Empty;
            var refreshToken = string.Empty;
            var ret = new JwtTokenModel();
            presence.Login(usernameOrEmail, password,
            successAction =>
            {
                if (successAction != null)
                {
                    ret = new JwtTokenModel
                    {
                        Token = successAction.Token,
                    };
                }
            },
            failAction => { err = failAction; });
            goto GotoReturn;
        GotoReturn:
            return Ok(new ResponseViewModel<JwtTokenModel>()
            {
                Data = ret.Token != null ? ret : null,
                IsSuccess = string.IsNullOrEmpty(err),
                ReturnMessage = err
            });
        }
    }
}