
using System.Collections.Generic;

namespace ANDP.Lib.Domain.Models
{
    public class ItemActionType
    {
        public ItemType ItemType { get; set; }
        public List<ActionType> ActionTypes { get; set; }
    }
}
