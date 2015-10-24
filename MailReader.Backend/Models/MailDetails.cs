using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReader.Backend.Models
{
	public class MailDetails
	{
		public string Body { get; set; }
		public string Subject { get; set; }
		public string From { get; set; }
		public uint Id { get; set; }
	}
}
