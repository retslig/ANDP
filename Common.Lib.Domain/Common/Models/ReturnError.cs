using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lib.Models
{
    public class ReturnError
    {
        public bool DidError { get; set; }
        public string CustomErrorMessage { get; set; }
        public System.Exception Exception { get; set; }
    }
}
