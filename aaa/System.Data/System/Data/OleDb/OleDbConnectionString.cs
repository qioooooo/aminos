using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Data.OleDb
{
	// Token: 0x02000219 RID: 537
	internal sealed class OleDbConnectionString : DbConnectionOptions
	{
		// Token: 0x06001EB9 RID: 7865 RVA: 0x002589DC File Offset: 0x00257DDC
		internal OleDbConnectionString(string connectionString, bool validate)
			: base(connectionString)
		{
			string text = base["prompt"];
			this.PossiblePrompt = (!ADP.IsEmpty(text) && string.Compare(text, "noprompt", StringComparison.OrdinalIgnoreCase) != 0) || !ADP.IsEmpty(base["window handle"]);
			if (!base.IsEmpty)
			{
				string text2 = null;
				if (!validate)
				{
					int num = 0;
					string text3 = null;
					this._expandedConnectionString = base.ExpandDataDirectories(ref text3, ref num);
					if (!ADP.IsEmpty(text3))
					{
						text3 = ADP.GetFullPath(text3);
					}
					if (text3 != null)
					{
						text2 = OleDbConnectionString.LoadStringFromStorage(text3);
						if (!ADP.IsEmpty(text2))
						{
							this._expandedConnectionString = string.Concat(new object[]
							{
								this._expandedConnectionString.Substring(0, num),
								text2,
								';',
								this._expandedConnectionString.Substring(num)
							});
						}
					}
				}
				if (validate || ADP.IsEmpty(text2))
				{
					this.ActualConnectionString = this.ValidateConnectionString(connectionString);
				}
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x00258AD4 File Offset: 0x00257ED4
		internal int ConnectTimeout
		{
			get
			{
				return base.ConvertValueToInt32("connect timeout", 15);
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x00258AF0 File Offset: 0x00257EF0
		internal string DataSource
		{
			get
			{
				return base.ConvertValueToString("data source", ADP.StrEmpty);
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x00258B10 File Offset: 0x00257F10
		internal string InitialCatalog
		{
			get
			{
				return base.ConvertValueToString("initial catalog", ADP.StrEmpty);
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x00258B30 File Offset: 0x00257F30
		internal string Provider
		{
			get
			{
				return base["provider"];
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x00258B48 File Offset: 0x00257F48
		internal int OleDbServices
		{
			get
			{
				return this._oledbServices;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x00258B5C File Offset: 0x00257F5C
		// (set) Token: 0x06001EC0 RID: 7872 RVA: 0x00258B70 File Offset: 0x00257F70
		internal SchemaSupport[] SchemaSupport
		{
			get
			{
				return this._schemaSupport;
			}
			set
			{
				this._schemaSupport = value;
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x00258B84 File Offset: 0x00257F84
		protected internal override PermissionSet CreatePermissionSet()
		{
			PermissionSet permissionSet;
			if (this.PossiblePrompt)
			{
				permissionSet = new NamedPermissionSet("FullTrust");
			}
			else
			{
				permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new OleDbPermission(this));
			}
			return permissionSet;
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x00258BBC File Offset: 0x00257FBC
		protected internal override string Expand()
		{
			if (this._expandedConnectionString != null)
			{
				return this._expandedConnectionString;
			}
			return base.Expand();
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x00258BE0 File Offset: 0x00257FE0
		internal int GetSqlSupport(OleDbConnection connection)
		{
			int num = this._sqlSupport;
			if (!this._hasSqlSupport)
			{
				object dataSourcePropertyValue = connection.GetDataSourcePropertyValue(OleDbPropertySetGuid.DataSourceInfo, 109);
				if (dataSourcePropertyValue is int)
				{
					num = (int)dataSourcePropertyValue;
				}
				this._sqlSupport = num;
				this._hasSqlSupport = true;
			}
			return num;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x00258C28 File Offset: 0x00258028
		internal bool GetSupportIRow(OleDbConnection connection, OleDbCommand command)
		{
			bool flag = this._supportIRow;
			if (!this._hasSupportIRow)
			{
				object propertyValue = command.GetPropertyValue(OleDbPropertySetGuid.Rowset, 263);
				flag = !(propertyValue is OleDbPropertyStatus);
				this._supportIRow = flag;
				this._hasSupportIRow = true;
			}
			return flag;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x00258C74 File Offset: 0x00258074
		internal bool GetSupportMultipleResults(OleDbConnection connection)
		{
			bool flag = this._supportMultipleResults;
			if (!this._hasSupportMultipleResults)
			{
				object dataSourcePropertyValue = connection.GetDataSourcePropertyValue(OleDbPropertySetGuid.DataSourceInfo, 196);
				if (dataSourcePropertyValue is int)
				{
					flag = 0 != (int)dataSourcePropertyValue;
				}
				this._supportMultipleResults = flag;
				this._hasSupportMultipleResults = true;
			}
			return flag;
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x00258CC8 File Offset: 0x002580C8
		private static int UdlPoolSize
		{
			get
			{
				int num = OleDbConnectionString.UDL._PoolSize;
				if (!OleDbConnectionString.UDL._PoolSizeInit)
				{
					object obj = ADP.LocalMachineRegistryValue("SOFTWARE\\Microsoft\\DataAccess\\Udl Pooling", "Cache Size");
					if (obj is int)
					{
						num = (int)obj;
						num = ((0 < num) ? num : 0);
						OleDbConnectionString.UDL._PoolSize = num;
					}
					OleDbConnectionString.UDL._PoolSizeInit = true;
				}
				return num;
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x00258D1C File Offset: 0x0025811C
		private static string LoadStringFromStorage(string udlfilename)
		{
			string text = null;
			Dictionary<string, string> dictionary = OleDbConnectionString.UDL._Pool;
			if (dictionary == null || !dictionary.TryGetValue(udlfilename, out text))
			{
				text = OleDbConnectionString.LoadStringFromFileStorage(udlfilename);
				if (text != null && 0 < OleDbConnectionString.UdlPoolSize)
				{
					if (dictionary == null)
					{
						dictionary = new Dictionary<string, string>();
						dictionary[udlfilename] = text;
						lock (OleDbConnectionString.UDL._PoolLock)
						{
							if (OleDbConnectionString.UDL._Pool != null)
							{
								dictionary = OleDbConnectionString.UDL._Pool;
							}
							else
							{
								OleDbConnectionString.UDL._Pool = dictionary;
								dictionary = null;
							}
						}
					}
					if (dictionary != null)
					{
						lock (dictionary)
						{
							dictionary[udlfilename] = text;
						}
					}
				}
			}
			return text;
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x00258DE8 File Offset: 0x002581E8
		private static string LoadStringFromFileStorage(string udlfilename)
		{
			string text = null;
			Exception ex = null;
			try
			{
				int num = ADP.CharSize * "\ufeff[oledb]\r\n; Everything after this line is an OLE DB initstring\r\n".Length;
				using (FileStream fileStream = new FileStream(udlfilename, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					long length = fileStream.Length;
					if (length < (long)num || 0L != length % (long)ADP.CharSize)
					{
						ex = ADP.InvalidUDL();
					}
					else
					{
						byte[] array = new byte[num];
						int num2 = fileStream.Read(array, 0, array.Length);
						if (num2 < num)
						{
							ex = ADP.InvalidUDL();
						}
						else if (Encoding.Unicode.GetString(array, 0, num) != "\ufeff[oledb]\r\n; Everything after this line is an OLE DB initstring\r\n")
						{
							ex = ADP.InvalidUDL();
						}
						else
						{
							array = new byte[length - (long)num];
							num2 = fileStream.Read(array, 0, array.Length);
							text = Encoding.Unicode.GetString(array, 0, num2);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				if (!ADP.IsCatchableExceptionType(ex2))
				{
					throw;
				}
				throw ADP.UdlFileError(ex2);
			}
			if (ex != null)
			{
				throw ex;
			}
			return text.Trim();
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00258F08 File Offset: 0x00258308
		private string ValidateConnectionString(string connectionString)
		{
			if (base.ConvertValueToBoolean("asynchronous processing", false))
			{
				throw ODB.AsynchronousNotSupported();
			}
			int num = base.ConvertValueToInt32("connect timeout", 0);
			if (num < 0)
			{
				throw ADP.InvalidConnectTimeoutValue();
			}
			string text = base.ConvertValueToString("data provider", null);
			if (text != null)
			{
				text = text.Trim();
				if (0 < text.Length)
				{
					OleDbConnectionString.ValidateProvider(text);
				}
			}
			text = base.ConvertValueToString("remote provider", null);
			if (text != null)
			{
				text = text.Trim();
				if (0 < text.Length)
				{
					OleDbConnectionString.ValidateProvider(text);
				}
			}
			text = base.ConvertValueToString("provider", ADP.StrEmpty).Trim();
			OleDbConnectionString.ValidateProvider(text);
			this._oledbServices = -13;
			if (!base.ContainsKey("ole db services") || ADP.IsEmpty(base["ole db services"]))
			{
				string text2 = (string)ADP.ClassesRootRegistryValue(text + "\\CLSID", string.Empty);
				if (text2 != null && 0 < text2.Length)
				{
					Guid guid = new Guid(text2);
					if (ODB.CLSID_MSDASQL == guid)
					{
						throw ODB.MSDASQLNotSupported();
					}
					object obj = ADP.ClassesRootRegistryValue("CLSID\\{" + guid.ToString("D", CultureInfo.InvariantCulture) + "}", "OLEDB_SERVICES");
					if (obj != null)
					{
						try
						{
							this._oledbServices = (int)obj;
						}
						catch (InvalidCastException ex)
						{
							ADP.TraceExceptionWithoutRethrow(ex);
						}
						this._oledbServices &= -13;
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("ole db services");
						stringBuilder.Append("=");
						stringBuilder.Append(this._oledbServices.ToString(CultureInfo.InvariantCulture));
						stringBuilder.Append(";");
						stringBuilder.Append(connectionString);
						connectionString = stringBuilder.ToString();
					}
				}
			}
			else
			{
				this._oledbServices = base.ConvertValueToInt32("ole db services", -13);
			}
			return connectionString;
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x00259104 File Offset: 0x00258504
		internal static bool IsMSDASQL(string progid)
		{
			return "msdasql" == progid || progid.StartsWith("msdasql.", StringComparison.Ordinal) || "microsoft ole db provider for odbc drivers" == progid;
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0025913C File Offset: 0x0025853C
		private static void ValidateProvider(string progid)
		{
			if (ADP.IsEmpty(progid))
			{
				throw ODB.NoProviderSpecified();
			}
			if (255 <= progid.Length)
			{
				throw ODB.InvalidProviderSpecified();
			}
			progid = progid.ToLower(CultureInfo.InvariantCulture);
			if (OleDbConnectionString.IsMSDASQL(progid))
			{
				throw ODB.MSDASQLNotSupported();
			}
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x00259188 File Offset: 0x00258588
		internal static void ReleaseObjectPool()
		{
			OleDbConnectionString.UDL._PoolSizeInit = false;
			OleDbConnectionString.UDL._Pool = null;
		}

		// Token: 0x04001274 RID: 4724
		internal readonly bool PossiblePrompt;

		// Token: 0x04001275 RID: 4725
		internal readonly string ActualConnectionString;

		// Token: 0x04001276 RID: 4726
		private readonly string _expandedConnectionString;

		// Token: 0x04001277 RID: 4727
		internal SchemaSupport[] _schemaSupport;

		// Token: 0x04001278 RID: 4728
		internal int _sqlSupport;

		// Token: 0x04001279 RID: 4729
		internal bool _supportMultipleResults;

		// Token: 0x0400127A RID: 4730
		internal bool _supportIRow;

		// Token: 0x0400127B RID: 4731
		internal bool _hasSqlSupport;

		// Token: 0x0400127C RID: 4732
		internal bool _hasSupportMultipleResults;

		// Token: 0x0400127D RID: 4733
		internal bool _hasSupportIRow;

		// Token: 0x0400127E RID: 4734
		private int _oledbServices;

		// Token: 0x0400127F RID: 4735
		internal UnsafeNativeMethods.IUnknownQueryInterface DangerousDataSourceIUnknownQueryInterface;

		// Token: 0x04001280 RID: 4736
		internal UnsafeNativeMethods.IDBInitializeInitialize DangerousIDBInitializeInitialize;

		// Token: 0x04001281 RID: 4737
		internal UnsafeNativeMethods.IDBCreateSessionCreateSession DangerousIDBCreateSessionCreateSession;

		// Token: 0x04001282 RID: 4738
		internal UnsafeNativeMethods.IDBCreateCommandCreateCommand DangerousIDBCreateCommandCreateCommand;

		// Token: 0x04001283 RID: 4739
		internal bool HaveQueriedForCreateCommand;

		// Token: 0x0200021A RID: 538
		private static class UDL
		{
			// Token: 0x04001284 RID: 4740
			internal const string Header = "\ufeff[oledb]\r\n; Everything after this line is an OLE DB initstring\r\n";

			// Token: 0x04001285 RID: 4741
			internal const string Location = "SOFTWARE\\Microsoft\\DataAccess\\Udl Pooling";

			// Token: 0x04001286 RID: 4742
			internal const string Pooling = "Cache Size";

			// Token: 0x04001287 RID: 4743
			internal static volatile bool _PoolSizeInit;

			// Token: 0x04001288 RID: 4744
			internal static int _PoolSize;

			// Token: 0x04001289 RID: 4745
			internal static volatile Dictionary<string, string> _Pool;

			// Token: 0x0400128A RID: 4746
			internal static object _PoolLock = new object();
		}
	}
}
