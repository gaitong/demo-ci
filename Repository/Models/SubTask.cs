using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class SubTask
    {
        public int Id { get; set; }
        public int MainTaskId { get; set; } 
        public string Detail { get; set; }
        public bool Active { get; set; }
    }
}