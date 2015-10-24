using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReader.Backend.Services
{
	class MailCredentialsProvider
	{
		public string GetLogin()
		{
			return Environment.GetEnvironmentVariable("MAILREADER_LOGIN");
		}

		public string GetPassword()
		{
			return Environment.GetEnvironmentVariable("MAILREADER_PASSWORD");
		}
	}
}
