using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MailReader
{
	public class GlobalContext
	{
		public string CurrentPath
		{
			get { return HttpContext.Current.Server.MapPath("."); }
		}
	}
}