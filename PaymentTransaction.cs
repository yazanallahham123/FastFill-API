using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API
{
    public class PaymentTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int CompanyId { get; set;}
        public int FuelTypeId { get; set; }
        public double Amount { get; set; }
        public double Fastfill { get; set; }
        public bool Status { get; set; }
        public bool? Cleared { get; set; }
        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
        public int DailyId { get; set; }
        public int? CompanyPumpId { get; set; }

        public virtual CompanyPump? CompanyPump { get; set; }

    }
}
