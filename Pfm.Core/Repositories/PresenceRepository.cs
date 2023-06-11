using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pfm.Core.Entities;
using Pfm.Core.Interfaces;
using Pfm.Core.Models;
using Pfm.Core.Helpers;
using Regula.FaceSDK.WebClient.Api;
using Regula.FaceSDK.WebClient.Model;
using Dapper;
using Ispm.Core.Helpers;

namespace Pfm.Core.Repositories
{
    public class PresenceRepository : DapperHelper, IPresence
    {
        public void Login(string email, string password, Action<JwtTokenModel> successAction, Action<string> failAction)
        {
            try
            {
                using var conn = PostgreSqlConnection;
                using var tx = conn.BeginTransaction();

                var tbUser = conn.GetList<TbUser>().Where(x => x.Email == email).FirstOrDefault();
                if (tbUser == null) { failAction($"email [{email}] tidak ada"); return; }
                if (CryptoHelper.DecryptString(tbUser.Password) != password) { failAction("password salah!"); return; }
                successAction(new JwtTokenModel
                {
                    Token = JwtMiddleware.GenerateJwtToken(tbUser)
                });
                failAction("");
                tx.Commit();
            }
            catch (Exception ex)
            {
                successAction(null);
                failAction(ErrorHelper.GetErrorMessage("Login", ex));
            }
        }
         
        public void Checkin(byte[] face, Action<string> successAction, Action<string> failAction)
        {
            try
            {
                var apiBasePath = "https://faceapi.regulaforensics.com";

                var dataPegawais = this.GetPegawais();
                var countPegawai = dataPegawais.Count();
                var msgFace = "";
                foreach (var item in dataPegawais)
                {
                    var pathFoto = Path.Combine(AppSetting.PathFileUser);
                    var face1 = File.ReadAllBytes($"{pathFoto}/{item.Foto}");
                    var face2 = face;

                    var sdk = new FaceSdk(apiBasePath);

                    var matchImage1 = new MatchImage(data: face1, type: ImageSource.LIVE);
                    var matchImage2 = new MatchImage(data: face2, type: ImageSource.LIVE);


                    var matchImages = new List<MatchImage> { matchImage1, matchImage2 };
                    var matchingRequest = new MatchRequest(
                        null, 
                        false, 
                        matchImages
                    );

                    var matchingResponse = sdk.MatchingApi.Match(matchingRequest);

                    decimal score = 0;
                    foreach (var comparison in matchingResponse.Results)
                    {
                        if(comparison.FirstIndex == 0)
                            score = comparison.Similarity * 100;
                    }

                    Console.WriteLine($"SCORE {score}");

                    if(score >= 075)
                    {
                        msgFace = $"{item.Nama} : STATUS FACE :  COCOK";
                        break;
                    }
                    else
                    {
                        msgFace = "STATUS FACE : TIDAK COCOK";
                    }
                }

                successAction(msgFace);
                failAction("");

               

            }
            catch (System.Exception ex)
            {
                successAction("ERROR");
                failAction("");
                throw;
            }
        }

        public void Checkout(byte[] face, Action<string> successAction, Action<string> failAction)
        {
            throw new NotImplementedException();
        }

        public void AddPegawai(TbPegawai tbPegawai, Action<string> successAction, Action<string> failAction)
        {
            try
            {
                using var conn = PostgreSqlConnection;
                using var tx = conn.BeginTransaction();
                conn.Insert(tbPegawai);
                successAction("success");
                failAction("");
                tx.Commit();
            }
            catch (System.Exception ex)
            {
                successAction(null);
                failAction(ErrorHelper.GetErrorMessage("AddPegawai", ex));
            }
        }

        public void DeletePegawai(int idPegawai, Action<string> successAction, Action<string> failAction)
        {
             try
            {
                using var conn = PostgreSqlConnection;
                using var tx = conn.BeginTransaction();
                var tbPegawaExist = conn.Get<TbPegawai>(idPegawai);
                conn.Delete(tbPegawaExist);
                successAction("success");
                failAction("");
                tx.Commit();
            }
            catch (System.Exception ex)
            {
                successAction(null);
                failAction(ErrorHelper.GetErrorMessage("DeletePegawai", ex));
            }
        }

        public void EditPegawai(TbPegawai tbPegawai, Action<string> successAction, Action<string> failAction)
        {
            try
            {
                using var conn = PostgreSqlConnection;
                using var tx = conn.BeginTransaction();
                conn.Update(tbPegawai);
                successAction("success");
                failAction("");
                tx.Commit();
            }
            catch (System.Exception ex)
            {
                successAction(null);
                failAction(ErrorHelper.GetErrorMessage("EditPegawai", ex));
            }
        }

        public TbPegawai GetPegawai(int idPegawai)
        {
            return GetPegawais().Where(x => x.IdPegawai == idPegawai).FirstOrDefault();
        }

        public IEnumerable<TbPegawai> GetPegawais()
        {
            using var conn = PostgreSqlConnection;
            return conn.GetList<TbPegawai>();
        }

        public IEnumerable<TbUser> GetUsers()
        {
            using var conn = PostgreSqlConnection;
            return conn.GetList<TbUser>();
        }

        public TbUser GetUser(int idUser)
        {
            return GetUsers().Where(x => x.IdUser == idUser).FirstOrDefault();
        }
    }
}