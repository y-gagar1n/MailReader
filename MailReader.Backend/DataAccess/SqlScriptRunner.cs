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
			var dllPath = Assembly.GetExecutingAssembly().CodeBase.Substring(8);
			var dllFolderPath = Path.GetDirectoryName(dllPath);
			var scriptPath = Path.Combine(dllFolderPath, "DataAccess\\Scripts\\CreateDatabase.sql");
            FileInfo scriptFile = new FileInfo(scriptPath);
			string rawScript = scriptFile.OpenText().ReadToEnd();
			string preparedScript = rawScript
				.Replace("%MDF_PATH%", HttpContext.Current.Server.MapPath("~/" + mdfPath))
				.Replace("%LDF_PATH%", HttpContext.Current.Server.MapPath("~/" + ldfPath));
            var cmd = connection.CreateCommand();
			cmd.CommandText = preparedScript;
			cmd.ExecuteNonQuery();
		}
	}
}
