using System;

namespace ANDP.Lib.Domain.Models
{
    public class InterLataPic
    {
        public ActionType ActionType { get; set; }
        public string Cic { get; set; }
        public string PicFreezeCode { get; set; }
        public DateTime PicEffectiveDate { get; set; }
    }
}
