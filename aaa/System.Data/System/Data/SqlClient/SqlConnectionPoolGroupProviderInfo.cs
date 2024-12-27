using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Security;

namespace System.Data.SqlClient
{
	// Token: 0x020002D2 RID: 722
	internal sealed class SqlConnectionPoolGroupProviderInfo : DbConnectionPoolGroupProviderInfo
	{
		// Token: 0x0600250B RID: 9483 RVA: 0x0027A3EC File Offset: 0x002797EC
		internal SqlConnectionPoolGroupProviderInfo(SqlConnectionString connectionOptions)
		{
			this._failoverPartner = connectionOptions.FailoverPartner;
			if (ADP.IsEmpty(this._failoverPartner))
			{
				this._failoverPartner = null;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x0027A420 File Offset: 0x00279820
		internal string FailoverPartner
		{
			get
			{
				return this._failoverPartner;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x0027A434 File Offset: 0x00279834
		internal bool UseFailoverPartner
		{
			get
			{
				return this._useFailoverPartner;
			}
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x0027A448 File Offset: 0x00279848
		internal void AliasCheck(string server)
		{
			if (this._alias != server)
			{
				lock (this)
				{
					if (this._alias == null)
					{
						this._alias = server;
					}
					else if (this._alias != server)
					{
						Bid.Trace("<sc.SqlConnectionPoolGroupProviderInfo|INFO> alias change detected. Clearing PoolGroup\n");
						base.PoolGroup.Clear();
						this._alias = server;
					}
				}
			}
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x0027A4CC File Offset: 0x002798CC
		private PermissionSet CreateFailoverPermission(SqlConnectionString userConnectionOptions, string actualFailoverPartner)
		{
			string text;
			if (userConnectionOptions["failover partner"] == null)
			{
				text = "data source";
			}
			else
			{
				text = "failover partner";
			}
			string text2 = userConnectionOptions.ExpandKeyword(text, actualFailoverPartner);
			return new SqlConnectionString(text2).CreatePermissionSet();
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x0027A508 File Offset: 0x00279908
		internal void FailoverCheck(SqlInternalConnection connection, bool actualUseFailoverPartner, SqlConnectionString userConnectionOptions, string actualFailoverPartner)
		{
			if (this.UseFailoverPartner != actualUseFailoverPartner)
			{
				Bid.Trace("<sc.SqlConnectionPoolGroupProviderInfo|INFO> Failover detected. failover partner='%ls'. Clearing PoolGroup\n", actualFailoverPartner);
				base.PoolGroup.Clear();
				this._useFailoverPartner = actualUseFailoverPartner;
			}
			if (!this._useFailoverPartner && this._failoverPartner != actualFailoverPartner)
			{
				PermissionSet permissionSet = this.CreateFailoverPermission(userConnectionOptions, actualFailoverPartner);
				lock (this)
				{
					if (this._failoverPartner != actualFailoverPartner)
					{
						this._failoverPartner = actualFailoverPartner;
						this._failoverPermissionSet = permissionSet;
					}
				}
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x0027A5A8 File Offset: 0x002799A8
		internal void FailoverPermissionDemand()
		{
			if (this._useFailoverPartner)
			{
				PermissionSet failoverPermissionSet = this._failoverPermissionSet;
				if (failoverPermissionSet != null)
				{
					failoverPermissionSet.Demand();
				}
			}
		}

		// Token: 0x04001785 RID: 6021
		private string _alias;

		// Token: 0x04001786 RID: 6022
		private PermissionSet _failoverPermissionSet;

		// Token: 0x04001787 RID: 6023
		private string _failoverPartner;

		// Token: 0x04001788 RID: 6024
		private bool _useFailoverPartner;
	}
}
