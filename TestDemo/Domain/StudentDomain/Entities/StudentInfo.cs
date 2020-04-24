using System;   

namespace Student.Domain.Entities
{
   public class StudentInfo: BaseEntity
    {

        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
