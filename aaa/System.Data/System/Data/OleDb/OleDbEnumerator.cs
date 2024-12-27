using System;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace System.Data.OleDb
{
	// Token: 0x02000224 RID: 548
	public sealed class OleDbEnumerator
	{
		// Token: 0x06001F89 RID: 8073 RVA: 0x0025E060 File Offset: 0x0025D460
		public DataTable GetElements()
		{
			OleDbConnection.ExecutePermission.Demand();
			DataTable dataTable = new DataTable("MSDAENUM");
			dataTable.Locale = CultureInfo.InvariantCulture;
			OleDbDataReader rootEnumerator = OleDbEnumerator.GetRootEnumerator();
			OleDbDataAdapter.FillDataTable(rootEnumerator, new DataTable[] { dataTable });
			return dataTable;
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0025E0A8 File Offset: 0x0025D4A8
		public static OleDbDataReader GetEnumerator(Type type)
		{
			OleDbConnection.ExecutePermission.Demand();
			return OleDbEnumerator.GetEnumeratorFromType(type);
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0025E0C8 File Offset: 0x0025D4C8
		internal static OleDbDataReader GetEnumeratorFromType(Type type)
		{
			object obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public, null, null, CultureInfo.InvariantCulture, null);
			return OleDbEnumerator.GetEnumeratorReader(obj);
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0025E0EC File Offset: 0x0025D4EC
		private static OleDbDataReader GetEnumeratorReader(object value)
		{
			NativeMethods.ISourcesRowset sourcesRowset = null;
			try
			{
				sourcesRowset = (NativeMethods.ISourcesRowset)value;
			}
			catch (InvalidCastException)
			{
				throw ODB.ISourcesRowsetNotSupported();
			}
			if (sourcesRowset == null)
			{
				throw ODB.ISourcesRowsetNotSupported();
			}
			value = null;
			int num = 0;
			IntPtr ptrZero = ADP.PtrZero;
			Bid.Trace("<oledb.ISourcesRowset.GetSourcesRowset|API|OLEDB> IID_IRowset\n");
			OleDbHResult sourcesRowset2 = sourcesRowset.GetSourcesRowset(ADP.PtrZero, ODB.IID_IRowset, num, ptrZero, out value);
			Bid.Trace("<oledb.ISourcesRowset.GetSourcesRowset|API|OLEDB|RET> %08X{HRESULT}\n", sourcesRowset2);
			Exception ex = OleDbConnection.ProcessResults(sourcesRowset2, null, null);
			if (ex != null)
			{
				throw ex;
			}
			OleDbDataReader oleDbDataReader = new OleDbDataReader(null, null, 0, CommandBehavior.Default);
			oleDbDataReader.InitializeIRowset(value, ChapterHandle.DB_NULL_HCHAPTER, ADP.RecordsUnaffected);
			oleDbDataReader.BuildMetaInfo();
			oleDbDataReader.HasRowsRead();
			return oleDbDataReader;
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0025E1A4 File Offset: 0x0025D5A4
		public static OleDbDataReader GetRootEnumerator()
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbEnumerator.GetRootEnumerator|API>\n");
			OleDbDataReader enumeratorFromType;
			try
			{
				Type typeFromProgID = Type.GetTypeFromProgID("MSDAENUM", true);
				enumeratorFromType = OleDbEnumerator.GetEnumeratorFromType(typeFromProgID);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return enumeratorFromType;
		}
	}
}
