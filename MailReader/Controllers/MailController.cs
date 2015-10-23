using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MailReader.Backend;
using MailReader.Backend.Models;
using MailReader.Models.Mail;

namespace MailReader.Controllers
{
    public class MailController : ApiController
    {
		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/mail")]
		public IEnumerable<MailPreview> GetMails()
	    {
		    var fetcher = new MailFetcher();
		    return fetcher.FetchRecentMailsPreview();
	    }

	    //public ActionResult Index()
	    //{
		   // return View();
	    //}


    }
}
