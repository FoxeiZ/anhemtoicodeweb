using System;
using System.Collections.Generic;
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

        public static Tuple<int, int> PaginatorCalc<T>(IEnumerable<T> obj, int item_count, int page_request)
        {
            int maxPage = Math.Max(1, obj.Count() / 10);
            if (page_request > maxPage)
            {
                page_request = maxPage;
            }
            return Tuple.Create(page_request, maxPage);
        }
    }
}