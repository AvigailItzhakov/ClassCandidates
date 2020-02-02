using Class.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Class.Web
{
    public class LayoutPageAttribute: ActionFilterAttribute
    {

        private string _connectionString;
        public LayoutPageAttribute(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = (Controller)context.Controller;
            var db = new CandidateRepository(_connectionString);
            controller.ViewBag.PendingCount = db.GetPendingCount();
            controller.ViewBag.ConfirmedCount = db.GetConfirmedCount();
            controller.ViewBag.DeclinedCount = db.GetDeclinedCount();
            base.OnActionExecuting(context);
        }
       
    }

}
