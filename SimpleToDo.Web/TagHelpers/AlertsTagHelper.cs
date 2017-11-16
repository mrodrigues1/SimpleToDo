using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using SimpleToDo.Model;

namespace SimpleToDo.Web.TagHelpers
{
    public class AlertsTagHelper : TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected ITempDataDictionary TempData => ViewContext.TempData;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            if (TempData["SimpleToDo.Alert"] == null)
                TempData["SimpleToDo.Alert"] = JsonConvert.SerializeObject(new HashSet<Alert>());


            var alerts = JsonConvert.DeserializeObject<ICollection<Alert>>(TempData["SimpleToDo.Alert"].ToString());

            var html = string.Empty;

            foreach (var alert in alerts)
            {
                html += $"<div class='alert {alert.Type}' id='inner-alert' role='alert'>" +
                        $"<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" +
                        $"{alert.Message}" +
                        $"</div>";
            }

            output.Content.SetHtmlContent(html);

            base.Process(context, output);
        }
    }
}