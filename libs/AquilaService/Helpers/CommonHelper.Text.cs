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
            StringBuilder sBuilder = new();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Matches, without consuming any characters,
        /// immediately between a character matched by \w and a character not matched by \w (in either order).
        /// It cannot be used to separate non words from words.
        /// And then replace them.
        /// </summary>
        /// <param name="input">input string</param>
        /// <param name="word">replaced word</param>
        /// <param name="replacement">replacement word</param>
        /// <returns>string has been edited</returns>
        public static string ReplaceWordFromWordV1(string input, string word, string replacement)
        {
            string before = (word.StartsWith('(') || word.StartsWith('（')) ? @"\B" : @"\b";
            string after = (word.EndsWith(')') || word.EndsWith('）')) ? @"\B" : @"\b";
            string pattern = before + Regex.Escape(word) + after + @"(?!\.\w)";

            return Regex.Replace(input, pattern, replacement);
        }

        /// <summary>
        /// Replace word from word v2
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="word">word</param>
        /// <param name="replacement">replacement</param>
        /// <returns>replaced string</returns>
        public static string ReplaceWordFromWordV2(string input, string word, string replacement)
        {
            // Escape the word to be used in regex
            string escapedWord = Regex.Escape(word);

            // Define the regex pattern with word boundaries
            // Ensure that quotes are properly handled as part of the word
            string pattern = $@"(?<!\w){escapedWord}(?!\w)";

            // Perform the replacement
            return Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Matches, without consuming any characters,
        /// immediately between a character matched by \w and a character not matched by \w (in either order).
        /// It cannot be used to separate non words from words.
        /// And then replace them.
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="word">word</param>
        /// <param name="replacement">replacment</param>
        /// <returns>replaced word</returns>
        public static string ReplaceWordFromWordV3(string input, string word, string replacement)
        {
            // Determine the correct word boundary pattern
            string before = (word.StartsWith('(') || word.StartsWith('（')) ? @"\b" : @"\b";
            string after = (word.EndsWith(')') || word.EndsWith('）')) ? @"\b" : @"\b";

            // Escape the word for regex and construct the pattern
            string pattern = before + Regex.Escape(word) + after;

            // Perform the replacement using regex
            return Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Get information from JWT
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>A dictionary contains information from JWT</returns>
        //public static Dictionary<string, string>? GetInfoFromToken(string? token)
        //{
        //    if (token is null)
        //        return null;

        //    if (token.StartsWith("Bearer"))
        //        token = token.Substring("Bearer ".Length).Trim();

        //    var handler = new JwtSecurityTokenHandler();

        //    JwtSecurityToken jwt = handler.ReadJwtToken(token);

        //    var claims = new Dictionary<string, string>();

        //    foreach (var claim in jwt.Claims)
        //        claims.Add(claim.Type, claim.Value);
        //    return claims;
        //}

        /// <summary>
        /// Convert = to ==
        /// Uy Tin by Dieu
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns>a string has been edited</returns>
        public static string ReplaceEQToEQEQ(string input)
        {
            string pattern = @"(?<!<|>|=|!)=(?!=|>|<)";
            string replacement = "==";
            return Regex.Replace(input, pattern, replacement);
        }

        /// <summary>
        /// Replace Space Line Inside String
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns>string has been edited</returns>
        public static string ReplaceSpaceLineInsideString(string input)
        {
            // cut "\n" in testcase
            string pattern = @"(\n+)\s*";
            string replacement = "\n";
            string result = input;

            if (!string.IsNullOrEmpty(input))
                result = Regex.Replace(input, pattern, replacement);

            return result;
        }

        /// <summary>
        /// Insert bracket for minus expression
        /// </summary>
        /// <param name="expression">input expression</param>
        /// <returns>a string after insert</returns>
        public static string InsertBracketForMinusExpression(string expression)
        {
            string pattern = @"(-[\w\d]+)"; // Mẫu để tìm các giá trị âm
            string replacement = "($1)"; // Chuỗi thay thế với ngoặc

            string output = Regex.Replace(expression, pattern, replacement);

            return output;
        }

        /// <summary>
        /// Get Operator
        /// </summary>
        /// <param name="ope">input operator</param>
        /// <returns>output operator after splitting</returns>
        public static (string?, string?, string?) GetOperator(string? ope)
        {
            var operators = new List<string> { "<=", "!=", ">=", "==", "=", ">", "<", "~" };

            if (ope is not null)
            {
                var o = operators.FirstOrDefault(x => ope.Contains(x));
                if (o is null)
                    return (null, null, null);

                var split = ope.Split(o);
                var oIdx = ope.IndexOf(o);
                return (ope[..oIdx].Trim(), o.Trim(), ope[(oIdx + o.Length)..].Trim());
            }

            return (null, null, null);
        }

        /// <summary>
        /// Generates all combinations of a specific size from a list of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="numbers">The list of elements from which to generate combinations.</param>
        /// <param name="k">The size of each combination to generate.</param>
        /// <param name="startIndex">The starting index in the list for generating combinations.</param>
        /// <param name="currentCombination">The current combination being constructed.</param>
        /// <param name="combinations">The list that stores all the generated combinations.</param>
        public static void GenerateCombinations<T>(List<T> numbers, int k, int startIndex,
            List<T> currentCombination, List<List<T>> combinations)
        {
            if (currentCombination.Count == k)
            {
                combinations.Add(new List<T>(currentCombination));
                return;
            }

            for (int i = startIndex; i < numbers.Count; i++)
            {
                currentCombination.Add(numbers[i]);

                GenerateCombinations(numbers, k, i + 1, currentCombination, combinations);

                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        /// <summary>
        /// Retrieves all combinations of a specific size from a list of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="numbers">The list of elements from which to generate combinations.</param>
        /// <param name="k">The size of each combination to generate.</param>
        /// <returns>A list containing all possible combinations of size <paramref name="k"/> from the input list.</returns>
        public static List<List<T>> GetCombinations<T>(List<T> numbers, int k)
        {
            List<List<T>> combinations = new List<List<T>>();
            List<T> currentCombination = new List<T>();

            GenerateCombinations(numbers, k, 0, currentCombination, combinations);

            return combinations;
        }
    }
}