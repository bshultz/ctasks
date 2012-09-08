using System.ComponentModel.DataAnnotations;
namespace Calendar.Utility
{
    public class BindableAppointmentSlotDuration
    {
        public BindableAppointmentSlotDuration(AppointmentSlotDuration value)
        {
            Duration = value;
            DisplayName = value.GetAttributeValue<DisplayAttribute, string>(displayNameAttribute => displayNameAttribute.Name);
            Order = value.GetAttributeValue<DisplayAttribute, int>(displayNameAttribute => displayNameAttribute.Order);
        }

        public AppointmentSlotDuration Duration { get; private set; }
        public string DisplayName { get; private set; }

        public int Order { get; private set; }
    }
}
