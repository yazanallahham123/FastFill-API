using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string TypeId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Notes { get; set; }
        public string ImageURL { get; set; }
        public int UserId { get; set; }
        public string Price { get; set; }
        public string Liters { get; set; }
        public string Material { get; set; }
        public string Address { get; set; }
    }
}
