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
	    private static MailFetcher _fetcher;

	    static MailController()
	    {
		    _fetcher = new MailFetcher();
	    }

	    [System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/mail")]
		public IEnumerable<MailPreview> GetMails()
	    {
		    return _fetcher.FetchRecentMailsPreview();
	    }

		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/mail/{uid}")]
		public MailBody GetMails(uint uid)
		{
			return _fetcher.GetMail(uid);
		}
	}
}
