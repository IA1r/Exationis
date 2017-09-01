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
    
    public partial class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
            this.Images = new HashSet<Image>();
        }
    
        public int QuestionID { get; set; }
        public string Content { get; set; }
        public int TestID { get; set; }
        public double Points { get; set; }
    
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
