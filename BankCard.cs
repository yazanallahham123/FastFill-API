using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FastFill_API
{
    public class BankCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BankName { get; set; }
        public string CardNumber { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual User User { get; set; }


    }
}
