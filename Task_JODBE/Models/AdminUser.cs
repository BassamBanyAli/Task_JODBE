//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Task_JODBE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdminUser
    {
        public int AdminID { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
