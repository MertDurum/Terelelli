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
    
    public partial class Tasks
    {
        public int TaskId { get; set; }
        public Nullable<int> PanelId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> TaskStartDate { get; set; }
        public string TaskDescription { get; set; }
        public string TaskNotes { get; set; }
        public Nullable<short> TaskDifficulty { get; set; }
    
        public virtual Panels Panels { get; set; }
    }
}