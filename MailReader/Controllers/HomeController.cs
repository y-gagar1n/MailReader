using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MailReader.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";

			return View();
		}

		public ActionResult MailList()
		{
			ViewBag.Title = "MailList";

			return View();
		}

		public ActionResult MailDetails()
		{
			ViewBag.Title = "MailDetails";

			return View();
		}
	}
}
