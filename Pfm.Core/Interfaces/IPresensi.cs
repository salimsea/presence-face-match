using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pfm.Core.Entities;
using Pfm.Core.Models;

namespace Pfm.Core.Interfaces
{
    public interface IPresensi
    {
        void Login(string email, string password, Action<JwtTokenModel> successAction, Action<string> failAction);

        void SetPengaturan(TbPengaturan tbPengaturan, Action<string> successAction, Action<string> failAction);
        TbPengaturan GetPengaturan();
        void Checkin(byte[] face, string fileName, Action<TbPegawai> successAction, Action<string> failAction);
        void CheckinManual(List<TbPresensi> tbPresensi, Action<string> successAction, Action<string> failAction);
        IEnumerable<TbPresensi> GetPresensis();

        IEnumerable<TbUser> GetUsers();
        TbUser GetUser(int idUser);

        IEnumerable<TbPegawai> GetPegawais();
        TbPegawai GetPegawai(int idPegawai);
        void AddPegawai(TbPegawai tbPegawai, Action<string> successAction, Action<string> failAction);
        void EditPegawai(TbPegawai tbPegawai, Action<string> successAction, Action<string> failAction);
        void DeletePegawai(int idPegawai, Action<string> successAction, Action<string> failAction);
    }
}