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
	class SqlScriptRunner
	{
		public void CreateDatabaseIfNotExists(SqlConnection connection, string mdfPath, string ldfPath)
		{
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

		public void InsertIntoMails(SqlConnection connection, MailDetails mail)
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

		public IEnumerable<MailDetails> GetSavedMails(SqlConnection connection)
		{
			string rawScript = ReadScript("GetMails");

			SqlCommand cmd = connection.CreateCommand();
			cmd.CommandText = rawScript;
			
			SqlDataReader sqlReader = cmd.ExecuteReader(CommandBehavior.Default);

			if (sqlReader.HasRows)
			{
				while (sqlReader.Read())
				{
					yield return new MailDetails
					{
						Id = (uint) sqlReader.GetInt32(0),
						Subject = sqlReader.GetString(1),
						Body = sqlReader.GetString(2),
						From = sqlReader.GetString(3)
					};
				}
			}
		}

		private string ReadScript(string scriptName)
		{
			var dllPath = Assembly.GetExecutingAssembly().CodeBase.Substring(8);
			var dllFolderPath = Path.GetDirectoryName(dllPath);
			var scriptPath = Path.Combine(dllFolderPath, "DataAccess\\Scripts\\" + scriptName + ".sql");
			FileInfo scriptFile = new FileInfo(scriptPath);
			string rawScript = scriptFile.OpenText().ReadToEnd();
			return rawScript;
		}

		private string Encode(string input)
		{
			return input
				.Replace("'", "''");
			//.Replace(Environment.NewLine, "' + CHAR(13) + '");
		}
	}
}
