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
    
    public partial class User
    {
        public User()
        {
            this.Disciplines = new HashSet<Discipline>();
            this.TestResults = new HashSet<TestResult>();
            this.TestPoints = new HashSet<TestPoint>();
        }
    
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Group { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public int FacultyID { get; set; }
        public string Avatar { get; set; }
        public int LanguageID { get; set; }
    
        public virtual ICollection<Discipline> Disciplines { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public virtual ICollection<TestPoint> TestPoints { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual Language Language { get; set; }
    }
}
