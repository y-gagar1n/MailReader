using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

		private string ReadScript(string scriptName)
		{
			var dllPath = Assembly.GetExecutingAssembly().CodeBase.Substring(8);
			var dllFolderPath = Path.GetDirectoryName(dllPath);
			var scriptPath = Path.Combine(dllFolderPath, "DataAccess\\Scripts\\" + scriptName + ".sql");
			FileInfo scriptFile = new FileInfo(scriptPath);
			string rawScript = scriptFile.OpenText().ReadToEnd();
			return rawScript;
		}
	}
}
