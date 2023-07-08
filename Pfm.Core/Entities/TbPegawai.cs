using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Pfm.Core.Entities
{
    [Table("tb_pegawai", Schema = "public")]
    public class TbPegawai
    {
        [Key]
        [Column("id_pegawai")]
        public int IdPegawai { get; set; }
        [Column("nama")]
        public string Nama { get; set; }
        [Column("nip")]
        public string Nip { get; set; }
        [Column("foto")]
        public string Foto { get; set; }
        [Column("created_by")]
        public int CreatedBy { get; set; }
        public TbUser User { get; set; }
        public static TbPegawai SetVal(TbPegawai pegawai, TbUser user)
        {
            TbPegawai ret = pegawai;
            ret.User = user;
            return ret;
        }

    }
}
