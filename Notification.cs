using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string? TypeId { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Notes { get; set; }
        public string? ImageURL { get; set; }
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
        public string? Price { get; set; }
        public string? Liters { get; set; }
        public string? Material { get; set; }
        public string? Address { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company? Company {get; set; }
        public bool? Cleared { get; set; }

    }
}
