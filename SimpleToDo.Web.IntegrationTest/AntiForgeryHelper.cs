using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleToDo.Web.IntegrationTest
{
    public static class AntiForgeryHelper
    {
        public static string ExtractAntiForgeryToken(string htmlResponseText)
        {
            if (htmlResponseText == null) throw new ArgumentException("htmlResponseText");

            Match match = Regex.Match(
                htmlResponseText,
                @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }

        public static async Task<string> ExtractAntiForgeryTokenAsync(HttpClient client)
        {
            var response = await client.GetAsync("/ToDoList/Create");
            var htmlResponseText = await response.Content.ReadAsStringAsync();

            return await Task.FromResult(ExtractAntiForgeryToken(htmlResponseText));
        }
    }
}