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
        [Column("waktu_hadir")]
        public TimeSpan WaktuHadir { get; set; }
        [Column("waktu_keluar")]
        public TimeSpan WaktuKeluar { get; set; }
        [Column("toleransi_waktu_hadir")]
        public int ToleransiWaktuHadir { get; set; }
        [Column("toleransi_waktu_keluar")]
        public int ToleransiWaktuKeluar { get; set; }
        [Column("hightlight")]
        public string? Hightlight { get; set; }
       
    }
}
