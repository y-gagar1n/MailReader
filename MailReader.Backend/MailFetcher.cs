using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailReader.Backend.DataAccess;
using MailReader.Backend.Models;
using S22.Imap;

namespace MailReader.Backend
{
	public class MailFetcher
	{
		private readonly ImapClient _client;

		public MailFetcher()
		{
			_client = new ImapClient("", 0, "", "");
			var repo = new MailRepository();
		}

		public IEnumerable<MailPreview> FetchRecentMailsPreview()
		{
			var msgs = FetchAllMessages();

			return msgs.Select(x => new MailPreview { From = x.Item2.From.DisplayName, Header = x.Item2.Subject, Number = x.Item1 });
		}

		public MailBody GetMail(uint mailUid)
		{
			var msg = _client.GetMessage(mailUid, false);
			return new MailBody {Body = PrepareBody(msg.Body), From = msg.From.DisplayName, Header = msg.Subject};
		}

		public string PrepareBody(string input)
		{
			var lines = input.Split(new string[] {Environment.NewLine}, StringSplitOptions.None).ToList();
			var htmlLines = lines.Select(l => "<p>" + l + "</p>");
			return string.Join("", htmlLines);
		}
		
		private IEnumerable<Tuple<uint, MailMessage>> FetchAllMessages()
		{
			IEnumerable<uint> uids = _client.Search(SearchCondition.All()).Reverse().Take(20);
			IList<MailMessage> messages = _client.GetMessages(uids).ToList();
			
			return messages.Zip(uids, (msg, uid) => Tuple.Create(uid, msg));

		}
	}
}
