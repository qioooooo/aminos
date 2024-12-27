using System;
using System.Collections;
using System.Security.Permissions;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000B2 RID: 178
	public sealed class ClerkMonitor : IEnumerable
	{
		// Token: 0x06000440 RID: 1088 RVA: 0x0000D5D4 File Offset: 0x0000C5D4
		public ClerkMonitor()
		{
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			securityPermission.Assert();
			this._monitor = new CrmMonitor();
			this._version = 0;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000D60C File Offset: 0x0000C60C
		public void Populate()
		{
			this._clerks = (_IMonitorClerks)this._monitor.GetClerks();
			this._version++;
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000D632 File Offset: 0x0000C632
		public int Count
		{
			get
			{
				if (this._clerks == null)
				{
					return 0;
				}
				return this._clerks.Count();
			}
		}

		// Token: 0x1700009E RID: 158
		public ClerkInfo this[int index]
		{
			get
			{
				if (this._clerks == null)
				{
					return null;
				}
				return new ClerkInfo(index, this._monitor, this._clerks);
			}
		}

		// Token: 0x1700009F RID: 159
		public ClerkInfo this[string index]
		{
			get
			{
				if (this._clerks == null)
				{
					return null;
				}
				return new ClerkInfo(index, this._monitor, this._clerks);
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000D68A File Offset: 0x0000C68A
		public IEnumerator GetEnumerator()
		{
			return new ClerkMonitorEnumerator(this);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000D694 File Offset: 0x0000C694
		~ClerkMonitor()
		{
			this._monitor.Release();
		}

		// Token: 0x040001E9 RID: 489
		internal CrmMonitor _monitor;

		// Token: 0x040001EA RID: 490
		internal _IMonitorClerks _clerks;

		// Token: 0x040001EB RID: 491
		internal int _version;
	}
}
