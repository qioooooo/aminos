using System;

namespace System.Data.Design
{
	internal class ManagedProviderNames
	{
		private ManagedProviderNames()
		{
		}

		public static string SqlClient
		{
			get
			{
				return "System.Data.SqlClient";
			}
		}
	}
}
