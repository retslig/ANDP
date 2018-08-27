using System;
namespace ANDP.Lib.Domain.Models
{
    public class EngineSchedule
    {
        public int Id { get; set; } // Id (Primary key)
        public bool Active { get; set; } // Sunday
        public string Name { get; set; } // Name
        public bool Sunday { get; set; } // Sunday
        public TimeSpan SundayStartTime { get; set; } // SundayStartTime
        public TimeSpan SundayEndtime { get; set; } // SundayEndtime
        public bool Monday { get; set; } // Monday
        public TimeSpan MondayStartTime { get; set; } // MondayStartTime
        public TimeSpan MondayEndtime { get; set; } // MondayEndtime
        public bool Tuesday { get; set; } // Tuesday
        public TimeSpan TuesdayStartTime { get; set; } // TuesdayStartTime
        public TimeSpan TuesdayEndtime { get; set; } // TuesdayEndtime
        public bool Wednesday { get; set; } // Wednesday
        public TimeSpan WednesdayStartTime { get; set; } // WednesdayStartTime
        public TimeSpan WednesdayEndtime { get; set; } // WednesdayEndtime
        public bool Thursday { get; set; } // Thursday
        public TimeSpan ThursdayStartTime { get; set; } // ThursdayStartTime
        public TimeSpan ThursdayEndtime { get; set; } // ThursdayEndtime
        public bool Friday { get; set; } // Friday
        public TimeSpan FridayStartTime { get; set; } // FridayStartTime
        public TimeSpan FridayEndtime { get; set; } // FridayEndtime
        public bool Saturday { get; set; } // Saturday
        public TimeSpan SaturdayStartTime { get; set; } // SaturdayStartTime
        public TimeSpan SaturdayEndtime { get; set; } // SaturdayEndtime
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }
}
