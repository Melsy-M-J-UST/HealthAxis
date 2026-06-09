using HealthAxis.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class AppointmentStatusUpdateDto
    {
        public int AppointmentId { get; set; }
        [Required] public AppointmentStatusEnum Status { get; set; }
        [StringLength(225)] public string CancellationReason { get; set; }
    }
}
