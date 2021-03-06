//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Discipline
    {
        public Discipline()
        {
            this.Tests = new HashSet<Test>();
        }
    
        public int DisciplineID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public int FacultyID { get; set; }
        public int Course { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
}
