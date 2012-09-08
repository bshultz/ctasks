
using System.ComponentModel.DataAnnotations;

namespace Calendar
{
    public enum AppointmentSlotDuration
    {
        [Display(Name = "1 hour", Order = 1)]
        SixtyMinutes = 1,
        [Display(Name = "30 minutes", Order = 2)]
        ThirtyMinutes = 2,
        [Display(Name = "20 minutes", Order = 3)]
        TwentyMinutes = 3,
        [Display(Name = "15 minutes", Order = 4)]
        FifteenMinutes = 4,
        [Display(Name = "10 minutes", Order = 5)]
        TenMinutes = 6,
        [Display(Name = "5 minutes", Order = 6)]
        FiveMinutes = 12,
    }
}
