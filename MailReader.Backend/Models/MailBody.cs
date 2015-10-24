using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReader.Backend.Models
{
	public class MailBody
	{
		public string Body { get; set; }
		public string Header { get; set; }
		public string From { get; set; }
		public uint Number { get; set; }
	}
}
