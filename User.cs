using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FastFill_API
{
    public partial class User
    {
        public User()
        {
            Wallets = new HashSet<Wallet>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public string MobileNumber { get; set; }
        public bool? Disabled { get; set; }
        public byte[] StoredSalt { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
