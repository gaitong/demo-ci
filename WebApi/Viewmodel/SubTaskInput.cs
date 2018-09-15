using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Viewmodel
{
    public class SubTaskInput
    {
        public int MainTaskId { get; set; }
        public string Detail { get; set; }
    }
}