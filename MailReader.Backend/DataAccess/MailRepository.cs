using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailReader.Backend.Models;

namespace MailReader.Backend.DataAccess
{
	class MailRepository
	{
		static MailRepository()
		{
			CreateDatabaseIfNotExists();
		}

		private static void CreateDatabaseIfNotExists()
		{
			using (var connection = CreateConnection())
			{
				var runner = new SqlScriptRunner();
				runner.CreateDatabaseIfNotExists(connection, "MailReader.mdf", "MailReader_log.ldf");
			}
		}

		private static SqlConnection CreateConnection()
		{
			SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder();
			csBuilder.DataSource = "(local)";
			csBuilder.IntegratedSecurity = true;
			csBuilder.UserID = "sa";
			csBuilder.Password = "1qaz@WSX";
			
			var connection = new SqlConnection(csBuilder.ConnectionString);
			connection.Open();
			return connection;
		}

		public IEnumerable<MailDetails> GetMails()
		{
			var runner = new SqlScriptRunner();
			using (var connection = CreateConnection())
			{
				return runner.GetSavedMails(connection).ToList();
			}
		}

		public void SaveMail(MailDetails mail)
		{
			var runner = new SqlScriptRunner();
			using (var connection = CreateConnection())
			{
				runner.InsertIntoMails(connection, mail);
			}
		}
	}
}
