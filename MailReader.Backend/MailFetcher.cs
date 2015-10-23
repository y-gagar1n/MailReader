using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailReader.Backend.Models;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace MailReader.Backend
{
	public class MailFetcher
	{
		public IEnumerable<MailPreview> FetchRecentMailsPreview()
		{
			var msgs = new List<string> {"1", "2", "3"};

			var i = 0;
			return msgs.Select(x => new MailPreview { Header = "header" + i++, From = "anonymous@gmail.com", Body = "Hi! "});
		}

		/// <summary>
		/// Example showing:
		///  - how to fetch all messages from a POP3 server
		/// </summary>
		/// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
		/// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
		/// <param name="useSsl">Whether or not to use SSL to connect to server</param>
		/// <param name="username">Username of the user on the server</param>
		/// <param name="password">Password of the user on the server</param>
		/// <returns>All Messages on the POP3 server</returns>
		private List<Message> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
		{
			// The client disconnects from the server when being disposed
			using (Pop3Client client = new Pop3Client())
			{
				// Connect to the server
				client.Connect(hostname, port, useSsl);

				// Authenticate ourselves towards the server
				client.Authenticate(username, password);

				// Get the number of messages in the inbox
				int messageCount = client.GetMessageCount();

				// We want to download all messages
				List<Message> allMessages = new List<Message>(messageCount);

				// Messages are numbered in the interval: [1, messageCount]
				// Ergo: message numbers are 1-based.
				// Most servers give the latest message the highest number
				for (int i = messageCount; i > 0; i--)
				{
					allMessages.Add(client.GetMessage(i));
				}

				// Now return the fetched messages
				return allMessages;
			}
		}
	}
}
