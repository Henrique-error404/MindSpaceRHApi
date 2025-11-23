using System.ComponentModel.DataAnnotations;

namespace MindSpaceRhApi.Models
{
    public class WellnessMetric
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DepartmentId { get; set; }

        public Department? Department { get; set; }

        public double StressLevelAverage { get; set; }

        public DateTime ReferenceDate { get; set; }
    }
}