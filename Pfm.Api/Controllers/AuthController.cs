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
using Pfm.Core.Helpers;

namespace Ispm.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IPresence presence;
        public AuthController(IPresence presence)
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
    
        [Authorize]
        [HttpGet("GetInfoUser", Name = "GetInfoUser")]
        public IActionResult GetPegawais()
        {
            UserViewModel ret = new();
            ret.Nama = UserInfo().Nama;
            ret.Email = UserInfo().Email;
            ret.Nama = UserInfo().Nama;
            return Ok(new ResponseViewModel<UserViewModel>()
            {
                Data = ret,
                IsSuccess = string.IsNullOrEmpty(null),
                ReturnMessage = null
            });
        }
    }
}