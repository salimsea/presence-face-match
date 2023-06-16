using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Pfm.Core.Entities
{
    [Table("tb_pengaturan", Schema = "public")]
    public class TbPengaturan
    {

        [Key]
        [Column("id_pengaturan")]
        public int IdPengaturan { get; set; }
        [Column("jam_hadir")]
        public TimeSpan JamHadir { get; set; }
        [Column("jam_keluar")]
        public TimeSpan JamKeluar { get; set; }
        [Column("toleransi_jam_hadir_menit")]
        public int ToleransiJamHadirMenit { get; set; }
        [Column("toleransi_jam_keluar_menit")]
        public int ToleransiJamKeluarMenit { get; set; }
        [Column("highlight")]
        public string? Highlight { get; set; }
       
    }
}
