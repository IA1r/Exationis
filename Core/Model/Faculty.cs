//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Faculty
    {
        public Faculty()
        {
            this.Disciplines = new HashSet<Discipline>();
            this.Users = new HashSet<User>();
        }
    
        public int FacultyID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Discipline> Disciplines { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}