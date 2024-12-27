using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Transactions;

namespace System.Data.Odbc
{
	// Token: 0x020001DD RID: 477
	internal sealed class OdbcConnectionOpen : DbConnectionInternal
	{
		// Token: 0x06001AB3 RID: 6835 RVA: 0x002440B0 File Offset: 0x002434B0
		internal OdbcConnectionOpen(OdbcConnection outerConnection, OdbcConnectionString connectionOptions)
		{
			OdbcEnvironmentHandle globalEnvironmentHandle = OdbcEnvironment.GetGlobalEnvironmentHandle();
			outerConnection.ConnectionHandle = new OdbcConnectionHandle(outerConnection, connectionOptions, globalEnvironmentHandle);
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x002440D8 File Offset: 0x002434D8
		internal OdbcConnection OuterConnection
		{
			get
			{
				OdbcConnection odbcConnection = (OdbcConnection)base.Owner;
				if (odbcConnection == null)
				{
					throw ADP.InvalidOperation("internal connection without an outer connection?");
				}
				return odbcConnection;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x00244100 File Offset: 0x00243500
		public override string ServerVersion
		{
			get
			{
				return this.OuterConnection.Open_GetServerVersion();
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00244118 File Offset: 0x00243518
		protected override void Activate(Transaction transaction)
		{
			OdbcConnection.ExecutePermission.Demand();
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00244130 File Offset: 0x00243530
		public override DbTransaction BeginTransaction(IsolationLevel isolevel)
		{
			return this.BeginOdbcTransaction(isolevel);
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x00244144 File Offset: 0x00243544
		internal OdbcTransaction BeginOdbcTransaction(IsolationLevel isolevel)
		{
			return this.OuterConnection.Open_BeginTransaction(isolevel);
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00244160 File Offset: 0x00243560
		public override void ChangeDatabase(string value)
		{
			this.OuterConnection.Open_ChangeDatabase(value);
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x0024417C File Offset: 0x0024357C
		protected override DbReferenceCollection CreateReferenceCollection()
		{
			return new OdbcReferenceCollection();
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00244190 File Offset: 0x00243590
		protected override void Deactivate()
		{
			base.NotifyWeakReference(0);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x002441A4 File Offset: 0x002435A4
		public override void EnlistTransaction(Transaction transaction)
		{
			this.OuterConnection.Open_EnlistTransaction(transaction);
		}
	}
}
