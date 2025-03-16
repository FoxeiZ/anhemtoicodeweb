using System;
using System.Linq;
using System.Text;

namespace anhemtoicodeweb
{
    public class Utils
    {
        public static string NormalizeDiacriticalCharacters(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var normalised = value.Normalize(NormalizationForm.FormD).ToLower().ToCharArray();

            return new string(normalised.Where(c => (int)c <= 127).ToArray());
        }
    }
}