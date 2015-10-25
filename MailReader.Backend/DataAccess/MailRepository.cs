using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
			using (var connection = CreateConnection(false))
			{
				var mdfPath = "MailReader.mdf";
				var ldfPath = "MailReader_log.ldf";

				string rawScript = ReadScript("CreateDatabase");
				string preparedScript = rawScript
					.Replace("%MDF_PATH%", HttpContext.Current.Server.MapPath("~/" + mdfPath))
					.Replace("%LDF_PATH%", HttpContext.Current.Server.MapPath("~/" + ldfPath));
				SqlCommand cmd = connection.CreateCommand();
				cmd.CommandText = preparedScript;
				cmd.ExecuteNonQuery();

				string tableScript = ReadScript("CreateMailsTable");
				SqlCommand tableCmd = connection.CreateCommand();
				tableCmd.CommandText = tableScript;
				tableCmd.ExecuteNonQuery();
			}
		}

		private static SqlConnection CreateConnection(bool useAppDatabase = true)
		{
			SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder();
			csBuilder.DataSource = "(local)";
			csBuilder.IntegratedSecurity = true;
			csBuilder.UserID = "sa";
			csBuilder.Password = "1qaz@WSX";
			if (useAppDatabase) csBuilder.InitialCatalog = "MailReader";
			
			var connection = new SqlConnection(csBuilder.ConnectionString);
			connection.Open();
			return connection;
		}

		public IList<MailDetails> GetAllMails()
		{
			var result = new List<MailDetails>();
			using (var connection = CreateConnection())
			{
				string rawScript = ReadScript("GetAllMails");

				SqlCommand cmd = connection.CreateCommand();
				cmd.CommandText = rawScript;

				SqlDataReader sqlReader = cmd.ExecuteReader(CommandBehavior.Default);

				if (sqlReader.HasRows)
				{
					while (sqlReader.Read())
					{
						result.Add(new MailDetails
						{
							Id = (uint)sqlReader.GetInt32(0),
							Subject = sqlReader.GetString(1),
							Body = sqlReader.GetString(2),
							From = sqlReader.GetString(3)
						});
					}
				}
			}
			return result;
		}

		public IList<MailDetails> GetMails(IEnumerable<uint> ids)
		{
			var result = new List<MailDetails>();
			using (var connection = CreateConnection())
			{
				SqlCommand cmd = connection.CreateCommand();
				string rawScript = String.Format("SELECT Id, Subject, Body, [From] FROM Mails WHERE Id IN ({0})", 
					AddParametersArray(ref cmd, ids.Select(x => (int)x).OfType<object>(), "Id"));
				
				cmd.CommandText = rawScript;
				
				SqlDataReader sqlReader = cmd.ExecuteReader(CommandBehavior.Default);

				if (sqlReader.HasRows)
				{
					while (sqlReader.Read())
					{
						result.Add(new MailDetails
						{
							Id = (uint)sqlReader.GetInt32(0),
							Subject = sqlReader.GetString(1),
							Body = sqlReader.GetString(2),
							From = sqlReader.GetString(3)
						});
					}
				}
			}
			return result;
		}

		public MailDetails GetMail(uint id)
		{
			using (var connection = CreateConnection())
			{
				var selectCmd = connection.CreateCommand();
				selectCmd.CommandText = "SELECT Id, Subject, Body, [From] FROM Mails WHERE Id=@id";
				selectCmd.Parameters.Add("@id", SqlDbType.Int);
				selectCmd.Parameters["@id"].Value = id;

				var reader = selectCmd.ExecuteReader();
				if (reader.HasRows)
				{
					reader.Read();
					return new MailDetails
					{
						Id = id,
						Subject = reader.GetString(1),
						Body = reader.GetString(2),
						From = reader.GetString(3)
					};
				}
				else
				{
					return null;
				}
			}
		} 

		public void SaveMail(MailDetails mail)
		{
			using (var connection = CreateConnection())
			{
				string rawScript = ReadScript("InsertIntoMails");
				string preparedScript = rawScript
					.Replace("%Id%", mail.Id.ToString())
					.Replace("%From%", Encode(mail.From))
					.Replace("%Subject%", Encode(mail.Subject))
					.Replace("%Body%", Encode(mail.Body));

				SqlCommand cmd = connection.CreateCommand();
				cmd.CommandText = preparedScript;
				cmd.ExecuteNonQuery();
			}
		}

		public void DeleteMail(uint uid)
		{
			using (var connection = CreateConnection())
			{
				var script = ReadScript("DeleteMail");
				var cmd = connection.CreateCommand();
				cmd.CommandText = script;
				cmd.Parameters.Add("@id", SqlDbType.Int);
				cmd.Parameters["@id"].Value = uid;

				cmd.ExecuteNonQuery();
			}
		}

		private static string ReadScript(string scriptName)
		{
			var dllPath = Assembly.GetExecutingAssembly().CodeBase.Substring(8);
			var dllFolderPath = Path.GetDirectoryName(dllPath);
			var scriptPath = Path.Combine(dllFolderPath, "DataAccess\\Scripts\\" + scriptName + ".sql");
			FileInfo scriptFile = new FileInfo(scriptPath);
			string rawScript = scriptFile.OpenText().ReadToEnd();
			return rawScript;
		}

		private static string Encode(string input)
		{
			return input
				.Replace("'", "''");
			//.Replace(Environment.NewLine, "' + CHAR(13) + '");
		}

		private static string AddParametersArray(ref SqlCommand cmd, IEnumerable<object> values, string name)
		{
			var parameters = new List<string>();
			int i = 0;
			foreach (var value in values)
			{
				string paramName = String.Format("@{0}{1}", name, i);
				cmd.Parameters.AddWithValue(paramName, value);
				parameters.Add(paramName);
				i++;
			}

			return string.Join(", ", parameters);
		}
	}
}
