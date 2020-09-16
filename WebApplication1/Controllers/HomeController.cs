using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;
using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(Guid? invoiceId)
        {
            Guid invoiceRecordId;
            if (invoiceId.HasValue)
            {
                invoiceRecordId = (Guid)invoiceId;
            }
            else
            {
                invoiceRecordId = new Guid(ConfigurationManager.AppSettings.Get("invoiceId"));
                ViewBag.Message = "Enter Invoice guid as parameter. (Now it's read from Web.config)";
            }

            OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
            var service = (IOrganizationService)serviceProxy;
            try
            {
                Entity mainEntity = service.Retrieve("invoice", invoiceRecordId, new ColumnSet(true));
                ViewBag.Name = mainEntity.GetAttributeValue<string>("name");
                ViewBag.TotalAmount = mainEntity.GetAttributeValue<Money>("totalamount").Value;
            }
            catch
            {
                ViewBag.Message = "There is no Invoice with ID:" + invoiceRecordId;
            }

            return View();
        }

    }
}