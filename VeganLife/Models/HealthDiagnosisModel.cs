namespace VeganLife.Models
{
    public class HealthDiagnosisModel
    {
        public Color StatusColor { get; set; }
        public string Classify { get; set; } = string.Empty;
        public string HealthDiagnosis { get; set; } = string.Empty;
        public DateTime DateDiagnosis {  get; set; }
        public string Note { get; set; } = string.Empty;
        public string Documents { get; set; } = string.Empty;
        public string ChartLink { get; set; } = string.Empty;
    }
}
