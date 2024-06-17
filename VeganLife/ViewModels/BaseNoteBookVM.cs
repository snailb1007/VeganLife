using System.Text;

namespace VeganLife.ViewModels
{
    public class BaseNoteBookVM : BaseViewModel
    {
        protected ParallelQuery<VitaminModel> SearchFoodByName(ParallelQuery<VitaminModel> vitamins, string name)
        {
            // Normalize input name to support UTF-8 and improve search accuracy
            var normalizedNames = NormalizeStringAndSplit(name);
            return vitamins.Where(item => normalizedNames
                .Any(normalizedName => NormalizeString(item.Id).Contains(normalizedName)
                    || NormalizeString(item.VietnameseName).Contains(normalizedName)));

            string NormalizeString(string input)
            {
                return input.Normalize(NormalizationForm.FormKD).ToLower().Trim();
            }

            IEnumerable<string> NormalizeStringAndSplit(string input)
            {
                return input.Normalize(NormalizationForm.FormKD)
                    .ToLower()
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s));
            }
        }
    }
}