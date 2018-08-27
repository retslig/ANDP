using System;

namespace Common.Lib.Domain.Common.Models
{
    public class LanguageResource
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CultureCode { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
