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
         
        public void Checkin(byte[] face, Action<TbPegawai> successAction, Action<string> failAction)
        {
            try
            {
                var apiBasePath = "https://faceapi.regulaforensics.com";

                var dataPegawais = this.GetPegawais();
                var countPegawai = dataPegawais.Count();
                var msgFace = "";
                TbPegawai tbPegawai = new();
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
                        tbPegawai = item;
                        break;
                    }
                    else
                    {
                        msgFace = "Wajah tidak terdaftar!";
                    }
                }
                if(msgFace != "") {
                    using var conn = PostgreSqlConnection;
                    using var tx = conn.BeginTransaction();
                    var cekPresensi = conn.GetList<TbPresensi>().Where(x => x.Tanggal.Date == DateTime.Now.Date)
                                                                .Where(x => x.IdPegawai == tbPegawai.IdPegawai)
                                                                .FirstOrDefault();

                    if(cekPresensi != null)
                    {
                        ///sudah absen hadir dan sudah absen pulang
                        if(cekPresensi.JamKeluar != null)
                        {
                            successAction(null);
                            failAction("Anda sudah melakukan absensi hadir & pulang, tidak dapat absen kembali !");
                        }
                        ///sudah absen hadir dan BELUM absen pulang
                        else
                        {
                            var tbPengaturan = conn.GetList<TbPengaturan>().FirstOrDefault();
                            var maxTimeOut = tbPengaturan.JamKeluar - TimeSpan.FromMinutes(tbPengaturan.ToleransiJamKeluarMenit);
                            if (DateTime.Now.TimeOfDay <= maxTimeOut)
                            {
                                successAction(null);
                                failAction("Absen pulang belum pada waktu nya!");
                            } else {
                                cekPresensi.JamKeluar = DateTime.Now.TimeOfDay;
                                conn.Update(cekPresensi);
                            }
                        }
                    } else {
                        TbPresensi tbPresensi = new()
                        {
                            IdPegawai = tbPegawai.IdPegawai,
                            Tanggal = DateTime.Now,
                            JamHadir = DateTime.Now.TimeOfDay,
                            JamKeluar = null,
                        };
                        conn.Insert(tbPresensi);
                    }
                   

                    successAction(tbPegawai);
                    failAction("");
                } else {
                    successAction(null);
                    failAction(msgFace);
                }

            }
            catch (System.Exception ex)
            {
                successAction(null);
                failAction("");
                throw;
            }
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