using System;

namespace System.Data.Design
{
	internal class ConnectionString
	{
		public ConnectionString(string providerName, string connectionString)
		{
			this.connectionString = connectionString;
			this.providerName = providerName;
		}

		public string ToFullString()
		{
			return this.connectionString.ToString();
		}

		private string providerName;

		private string connectionString;
	}
}
