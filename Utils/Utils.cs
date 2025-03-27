using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace anhemtoicodeweb
{
    public class Utils
    {
        public static string NormalizeDiacriticalCharacters(string value)
        {
            if (value == null)
            {
                return "";
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


        public static String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
        public static string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown") || ipAddress.Length > 45)
                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }
    }
}