﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TaskBoardEntities7 : DbContext
    {
        public TaskBoardEntities7()
            : base("name=TaskBoardEntities7")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Panels> Panels { get; set; }
        public virtual DbSet<ProjectUsers> ProjectUsers { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<TaskTimes> TaskTimes { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<TaskCompletionTimes> TaskCompletionTimes { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
    }
}
