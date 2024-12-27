using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

namespace System.Data.Sql
{
	// Token: 0x02000280 RID: 640
	public sealed class SqlDataSourceEnumerator : DbDataSourceEnumerator
	{
		// Token: 0x060021A2 RID: 8610 RVA: 0x00269768 File Offset: 0x00268B68
		private SqlDataSourceEnumerator()
		{
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x0026977C File Offset: 0x00268B7C
		public static SqlDataSourceEnumerator Instance
		{
			get
			{
				return SqlDataSourceEnumerator.SingletonInstance;
			}
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00269790 File Offset: 0x00268B90
		public override DataTable GetDataSources()
		{
			new NamedPermissionSet("FullTrust").Demand();
			char[] array = null;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 1024;
			int num2 = 0;
			array = new char[num];
			bool flag = true;
			bool flag2 = false;
			IntPtr intPtr = ADP.PtrZero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.timeoutTime = TdsParserStaticMethods.GetTimeoutSeconds(30);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					intPtr = SNINativeMethodWrapper.SNIServerEnumOpen();
				}
				if (ADP.PtrZero != intPtr)
				{
					while (flag && !TdsParserStaticMethods.TimeoutHasExpired(this.timeoutTime))
					{
						num2 = SNINativeMethodWrapper.SNIServerEnumRead(intPtr, array, num, ref flag);
						if (num2 > num)
						{
							flag2 = true;
							flag = false;
						}
						else if (0 < num2)
						{
							stringBuilder.Append(array, 0, num2);
						}
					}
				}
			}
			finally
			{
				if (ADP.PtrZero != intPtr)
				{
					SNINativeMethodWrapper.SNIServerEnumClose(intPtr);
				}
			}
			if (flag2)
			{
				Bid.Trace("<sc.SqlDataSourceEnumerator.GetDataSources|ERR> GetDataSources:SNIServerEnumRead returned bad length, requested %d, received %d", num, num2);
				throw ADP.ArgumentOutOfRange("readLength");
			}
			return SqlDataSourceEnumerator.ParseServerEnumString(stringBuilder.ToString());
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x002698A8 File Offset: 0x00268CA8
		private static DataTable ParseServerEnumString(string serverInstances)
		{
			DataTable dataTable = new DataTable("SqlDataSources");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("ServerName", typeof(string));
			dataTable.Columns.Add("InstanceName", typeof(string));
			dataTable.Columns.Add("IsClustered", typeof(string));
			dataTable.Columns.Add("Version", typeof(string));
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			char[] array = new char[1];
			foreach (string text5 in serverInstances.Split(array))
			{
				string text6 = text5;
				char[] array3 = new char[1];
				string text7 = text6.Trim(array3);
				if (text7.Length != 0)
				{
					foreach (string text8 in text7.Split(new char[] { ';' }))
					{
						if (text == null)
						{
							foreach (string text9 in text8.Split(new char[] { '\\' }))
							{
								if (text == null)
								{
									text = text9;
								}
								else
								{
									text2 = text9;
								}
							}
						}
						else if (text3 == null)
						{
							text3 = text8.Substring(SqlDataSourceEnumerator._clusterLength);
						}
						else
						{
							text4 = text8.Substring(SqlDataSourceEnumerator._versionLength);
						}
					}
					string text10 = "ServerName='" + text + "'";
					if (!ADP.IsEmpty(text2))
					{
						text10 = text10 + " AND InstanceName='" + text2 + "'";
					}
					if (dataTable.Select(text10).Length == 0)
					{
						DataRow dataRow = dataTable.NewRow();
						dataRow[0] = text;
						dataRow[1] = text2;
						dataRow[2] = text3;
						dataRow[3] = text4;
						dataTable.Rows.Add(dataRow);
					}
					text = null;
					text2 = null;
					text3 = null;
					text4 = null;
				}
			}
			foreach (object obj in dataTable.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				dataColumn.ReadOnly = true;
			}
			return dataTable;
		}

		// Token: 0x04001601 RID: 5633
		internal const string ServerName = "ServerName";

		// Token: 0x04001602 RID: 5634
		internal const string InstanceName = "InstanceName";

		// Token: 0x04001603 RID: 5635
		internal const string IsClustered = "IsClustered";

		// Token: 0x04001604 RID: 5636
		internal const string Version = "Version";

		// Token: 0x04001605 RID: 5637
		private const int timeoutSeconds = 30;

		// Token: 0x04001606 RID: 5638
		private static readonly SqlDataSourceEnumerator SingletonInstance = new SqlDataSourceEnumerator();

		// Token: 0x04001607 RID: 5639
		private long timeoutTime;

		// Token: 0x04001608 RID: 5640
		private static string _Version = "Version:";

		// Token: 0x04001609 RID: 5641
		private static string _Cluster = "Clustered:";

		// Token: 0x0400160A RID: 5642
		private static int _clusterLength = SqlDataSourceEnumerator._Cluster.Length;

		// Token: 0x0400160B RID: 5643
		private static int _versionLength = SqlDataSourceEnumerator._Version.Length;
	}
}
