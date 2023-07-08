using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Pfm.Core.Entities
{
    [Table("tb_presensi", Schema = "public")]
    public class TbPresensi
    {
        [Key]
        [Column("id_presensi")]
        public int IdPresensi { get; set; }
        [Column("id_pegawai")]
        public int IdPegawai { get; set; }
        [Column("jam_hadir")]
        public TimeSpan JamHadir { get; set; }
        [Column("jam_keluar")]
        public TimeSpan? JamKeluar { get; set; }
        [Column("foto_hadir")]
        public string? FotoHadir { get; set; }
        [Column("foto_keluar")]
        public string? FotoKeluar { get; set; }
        [Column("tanggal")]
        public DateTime Tanggal { get; set; }
         [Column("waktu_hadir")]
        public TimeSpan WaktuHadir { get; set; }
        [Column("waktu_keluar")]
        public TimeSpan WaktuKeluar { get; set; }
        public TbPegawai? Pegawai { get; set; }

        public static TbPresensi SetVal(TbPresensi presensi, TbPegawai pegawai)
        {
            TbPresensi ret = presensi;
            ret.Pegawai = pegawai;
            return ret;
        }
    }
    
}
