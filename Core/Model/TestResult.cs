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
    
    public partial class TestResult
    {
        public int UserID { get; set; }
        public int AnswerID { get; set; }
        public bool Result { get; set; }
        public int TestResultID { get; set; }
    
        public virtual Answer Answer { get; set; }
        public virtual User User { get; set; }
    }
}
