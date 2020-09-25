using System;
using System.Web.Mvc;
using TCAdmin.SDK.Web.MVC.Controllers;
using TCAdmin.Web.MVC;
using TCAdmin.SDK.Web;
using Service = TCAdmin.GameHosting.SDK.Objects.Service;

namespace Game
{
    [Authorize]
    [ExceptionHandler]
    public class MaintenanceController : BaseServiceController
    {
        [ParentAction("Service", "Home")]
        public ActionResult Index()
        {
            var model = new MaintenanceModel { Service = new Service(Service.GetSelectedService().ServiceId), ServiceId = Service.GetSelectedService().ServiceId };
            return View("Maintenance", model);
        }

        public ActionResult MaintenanceSwitch(bool switchy)
        {
            var serviceFromGame = TCAdmin.GameHosting.SDK.Objects.Service.GetSelectedService();

            if (switchy)
            {
                try {
                    serviceFromGame.Variables["_ServiceMaintenance::Maintenance"] = switchy;
                    serviceFromGame.Variables["_ServiceMaintenance::AdminUser"] = TCAdmin.SDK.Session.GetCurrentUser().UserName;
                    serviceFromGame.Variables["_ServiceMaintenance::StartTime"] = DateTime.UtcNow;
                    serviceFromGame.Save();
                }
                catch (Exception e)
                {
                    return JavaScript($"TCAdmin.Ajax.ShowBasicDialog('Error', '{Utility.EscapeJavaScriptString(e.Message  + "<br><br>Stacktrace:<br>" + e.StackTrace)}');$('body').css('cursor', 'default');");
                }
            }
            else
            {
                try
                {
                    serviceFromGame.Variables["_ServiceMaintenance::Maintenance"] = switchy;
                    serviceFromGame.Variables["_ServiceMaintenance::AdminUser"] = "";
                    serviceFromGame.Variables["_ServiceMaintenance::StartTime"] = "";
                    serviceFromGame.Save();
                }
                catch (Exception e)
                {
                    return JavaScript($"TCAdmin.Ajax.ShowBasicDialog('Error', '{Utility.EscapeJavaScriptString(e.Message  + "<br><br>Stacktrace:<br>" + e.StackTrace)}');$('body').css('cursor', 'default');");
                }
            }
            
            return new EmptyResult();
        }
    }
}