//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Terelelli.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectUsers
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual Projects Projects { get; set; }
    }
}
