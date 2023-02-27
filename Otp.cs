using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API
{
    public class Otp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]

        public int Id { get; set; }
        public string? registerId { get; set; }
        public string? otpCode { get; set; }
        public bool? status { get; set; }
        public DateTime? Date { get; set; }
        public string? mobileNumber { get; set; }
    }
}
