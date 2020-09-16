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
            var id = ConfigurationManager.AppSettings.Get("invoiceId");
            Guid entityGuid = new Guid(id);
            if (entityGuid!=null)
            {
                OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
                var service = (IOrganizationService)serviceProxy;
                try
                {
                    Entity mainEntity = service.Retrieve("invoice", entityGuid, new ColumnSet(true));
                    ViewBag.Name = mainEntity.GetAttributeValue<string>("name");
                    ViewBag.TotalAmount= mainEntity.GetAttributeValue<Money>("totalamount").Value;
                }
                catch
                {
                    ViewBag.Message = "There is no MainEntity with ID:"+invoiceId;
                }
                
            }
            else
            {
                ViewBag.Message = "Enter MainEntity guid as parameter";
            }
            
            return View();
        }

    }
}