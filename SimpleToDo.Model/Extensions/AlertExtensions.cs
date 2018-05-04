using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SimpleToDo.Model.Extensions
{
    public static class AlertExtensions
    {
        private const string AlertKey = "SimpleToDo.Alert";

        public static void AddAlertSuccess(this Controller controller, string message)
        {
            var alerts = GetAlerts(controller);

            alerts.Add(new Alert(message, "alert-success"));

            controller.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
        }

        public static void AddAlertInfo(this Controller controller, string message)
        {
            var alerts = GetAlerts(controller);

            alerts.Add(new Alert(message, "alert-info"));

            controller.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
        }

        public static void AddAlertWarning(this Controller controller, string message)
        {
            var alerts = GetAlerts(controller);

            alerts.Add(new Alert(message, "alert-warning"));

            controller.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
        }

        public static void AddAlertDanger(this Controller controller, string message)
        {
            var alerts = GetAlerts(controller);

            alerts.Add(new Alert(message, "alert-danger"));

            controller.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
        }

        private static ICollection<Alert> GetAlerts(Controller controller)
        {
            var alertsTemp = controller.TempData[AlertKey];

            if (alertsTemp == null)
                alertsTemp = JsonConvert.SerializeObject(new HashSet<Alert>());

            ICollection<Alert> alerts = JsonConvert.DeserializeObject<ICollection<Alert>>(alertsTemp.ToString());

            if (alerts == null)
            {
                alerts = new HashSet<Alert>();
            }
            return alerts;
        }
    }
}