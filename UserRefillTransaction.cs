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
    public class UserRefillTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public string transactionId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool status { get; set; }
        public int? RefillSourceId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual User User { get; set; }
    }
}
