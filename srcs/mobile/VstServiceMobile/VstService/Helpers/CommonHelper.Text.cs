namespace AquilaService
{
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;
    using System.Text.RegularExpressions;

    public static class TextHelper
    {
        /// <summary>
        /// Check string whether following JSON format
        /// </summary>
        /// <param name="jsonString">string</param>
        /// <returns><list type="bullet">
        /// <item><term>True</term><description>Is JSON</description></item>
        /// <item><term>False</term><description>Otherwise</description></item>
        /// </list></returns>
        public static bool IsJson(string jsonString)
        {
            try
            {
                JsonDocument.Parse(jsonString);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Md5 password
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>hashed string</returns>
        public static string ToMd5Hash(this string input)
        {
            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}