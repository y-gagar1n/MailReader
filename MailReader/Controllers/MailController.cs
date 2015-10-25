using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MailReader.Backend;
using MailReader.Backend.Models;
using MailReader.Backend.Services;
using MailReader.Models.Mail;

namespace MailReader.Controllers
{
    public class MailController : ApiController
    {
	    private static readonly MailFetcher _fetcher;

	    static MailController()
	    {
		    _fetcher = new MailFetcher();
	    }

	    [System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/mail")]
		public MailPreviewCollection GetMails(int take = 20, int skip = 0)
	    {
		    return new MailPreviewCollection(_fetcher.FetchRecentMailsPreview(take, skip).Select(x => new MailPreview()
		    {
			    From = x.From,
				Subject = x.Subject,
				Id = x.Id
		    }).ToList())
		    {
			    Skip = skip,
				Take = take
		    };
	    }

		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/mail/{uid}")]
		public MailDetails GetMails(uint uid)
		{
			return _fetcher.GetMail(uid);
		}

		[System.Web.Http.HttpDelete]
		[System.Web.Http.Route("api/mail/{uid}")]
		public void DeleteMail(uint uid)
		{
			_fetcher.DeleteMail(uid);
		}
	}
}
