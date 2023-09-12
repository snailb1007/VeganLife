using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganLifeDataCenter.Models
{
    public class ReportModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public int PercentBreakfast { get; set; }
        public int PercentLunch { get; set; }
        public int PercentDinner { get; set; }
        public int PercentOther { get; set; }
        public int GoalKcal { get; set; }
        [Required]
        public DateTimeOffset? CreatedDate { get; set; }
        [DefaultValue(0)]
        public int Status { get; set; }
        [NotMapped]
        public required List<string> FoodNutriFactId { get; set; }
    }
}
