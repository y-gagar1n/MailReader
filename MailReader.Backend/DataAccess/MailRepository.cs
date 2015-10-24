using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public void GetMails()
		{
			using (var connection = new SqlConnection())
			{
				
			}
		}
	}
}
