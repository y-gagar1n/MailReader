using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReader.Backend.Models
{
	public class MailPreviewCollection// : Collection<MailPreview>
	{
		public MailPreviewCollection(IList<MailPreview> list)
		{
			Messages = list;
		}

		public IList<MailPreview> Messages { get; set; } 
		public int Take { get; set; }
		public int Skip { get; set; }
	}
}
