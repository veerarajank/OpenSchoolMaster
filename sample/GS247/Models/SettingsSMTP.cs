//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GS247.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SettingsSMTP
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public Nullable<int> ConnectionSecurity { get; set; }
        public Nullable<int> EnableSMTP { get; set; }
    }
}
