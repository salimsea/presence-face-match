using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Pfm.Core.Entities
{
    [Table("tb_user", Schema = "public")]
    public class TbUser
    {

        [Key]
        [Column("id_user")]
        public int IdUser { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("nama")]
        public string Nama { get; set; }
       
    }
}
