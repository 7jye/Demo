﻿using System;   

namespace Student.Domain.Entities
{
   public class StudentInfo: BaseEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
