using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace SimpleToDo.Web.IntegrationTest.Helper
{
    public static class AntiForgeryHelper
    {
        private static SetCookieHeaderValue _antiforgeryCookie;
        private static string _antiforgeryToken;

        public static Regex AntiforgeryFormFieldRegex = new Regex(
            @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

        public static async Task<string> EnsureAntiforgeryTokenAsync(HttpClient client)
        {
            if (_antiforgeryToken != null)
                return _antiforgeryToken;

            var response = await client.GetAsync("/ToDoList/Create");
            response.EnsureSuccessStatusCode();
            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                _antiforgeryCookie = SetCookieHeaderValue.ParseList(values.ToList())
                    .SingleOrDefault(
                        c => c.Name.StartsWith(
                            ".AspNetCore.AntiForgery.",
                            StringComparison.InvariantCultureIgnoreCase));
            }

            Assert.NotNull(_antiforgeryCookie);

            client.DefaultRequestHeaders.Add(
                "Cookie",
                new CookieHeaderValue(_antiforgeryCookie.Name, _antiforgeryCookie.Value)
                    .ToString());

            var responseHtml = await response.Content.ReadAsStringAsync();
            var match = AntiforgeryFormFieldRegex.Match(responseHtml);
            _antiforgeryToken = match.Success ? match.Groups[1].Captures[0].Value : null;

            Assert.NotNull(_antiforgeryToken);

            return _antiforgeryToken;
        }
    }
}