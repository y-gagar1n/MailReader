using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using MailReader.Backend.DataAccess;
using MailReader.Backend.Models;
using S22.Imap;

namespace MailReader.Backend.Services
{
	public class MailFetcher
	{
		private readonly ImapClient _client;
		private MailRepository _repo;

		public MailFetcher()
		{
			var credProvider = new MailCredentialsProvider();
			_client = new ImapClient("imap.gmail.com", 993, credProvider.GetLogin(), credProvider.GetPassword(), AuthMethod.Login, true);
			_repo = new MailRepository();
		}

		public IEnumerable<MailDetails> FetchRecentMailsPreview()
		{
			var msgs = FetchAllMessages();

			return msgs;
		}

		public MailDetails GetMail(uint mailUid)
		{
			MailDetails cachedMail = _repo.GetMail(mailUid);
			if (cachedMail != null)
			{
				return cachedMail;
			}
			else
			{
				var msg = _client.GetMessage(mailUid, false);
				return new MailDetails
				{
					Body = PrepareBody(msg.Body),
					From = msg.From.DisplayName,
					Subject = msg.Subject,
					Id = mailUid
				};
			}
		}

		public string PrepareBody(string input)
		{
			var lines = input.Split(new string[] {Environment.NewLine}, StringSplitOptions.None).ToList();
			var htmlLines = lines.Select(l => "<p>" + l + "</p>");
			return string.Join("", htmlLines);
		}
		
		private IEnumerable<MailDetails> FetchAllMessages()
		{
			IEnumerable<uint> uids = _client
				.Search(SearchCondition.All())
				.Reverse()
				.Take(20)
				.ToList();

			var cachedMails = _repo.GetMails(uids);

			var absentUids = uids.Except(cachedMails.Select(x => x.Id)).ToList();

			var absentMails = _client.GetMessages(absentUids)
				.Zip(absentUids, (msg, id) => new MailDetails
				{
					Id = id,
					Subject = msg.Subject,
					Body = msg.Body,
					From = msg.From.DisplayName
				}).ToList();

			foreach (var newMessage in absentMails)
			{
				_repo.SaveMail(newMessage);
			}

			return cachedMails.Concat(absentMails).OrderByDescending(x => x.Id);

		}

		public void DeleteMail(uint uid)
		{
			_client.DeleteMessage(uid);
			_repo.DeleteMail(uid);
		}
	}
}
