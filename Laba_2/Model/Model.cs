﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IDomainObject
    {
        int Id { get; set; }
    }
    public class Student : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Speciality { get; set; }
    }
}
