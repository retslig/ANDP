using System;
using System.Collections.Generic;

namespace ANDP.Lib.Domain.Models
{
    public class Feature
    {
        public ActionType ActionType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime DeactivationDate { get; set; }
        public int Quantity{ get; set; }
        public List<Attribute> Attributes{ get; set; }
    }
}
