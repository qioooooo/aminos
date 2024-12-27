using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006A8 RID: 1704
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ClientSponsor : MarshalByRefObject, ISponsor
	{
		// Token: 0x06003DC2 RID: 15810 RVA: 0x000D3F9C File Offset: 0x000D2F9C
		public ClientSponsor()
		{
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x000D3FC5 File Offset: 0x000D2FC5
		public ClientSponsor(TimeSpan renewalTime)
		{
			this.m_renewalTime = renewalTime;
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x000D3FF5 File Offset: 0x000D2FF5
		// (set) Token: 0x06003DC5 RID: 15813 RVA: 0x000D3FFD File Offset: 0x000D2FFD
		public TimeSpan RenewalTime
		{
			get
			{
				return this.m_renewalTime;
			}
			set
			{
				this.m_renewalTime = value;
			}
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x000D4008 File Offset: 0x000D3008
		public bool Register(MarshalByRefObject obj)
		{
			ILease lease = (ILease)obj.GetLifetimeService();
			if (lease == null)
			{
				return false;
			}
			lease.Register(this);
			lock (this.sponsorTable)
			{
				this.sponsorTable[obj] = lease;
			}
			return true;
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x000D4064 File Offset: 0x000D3064
		public void Unregister(MarshalByRefObject obj)
		{
			ILease lease = null;
			lock (this.sponsorTable)
			{
				lease = (ILease)this.sponsorTable[obj];
			}
			if (lease != null)
			{
				lease.Unregister(this);
			}
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x000D40B8 File Offset: 0x000D30B8
		public TimeSpan Renewal(ILease lease)
		{
			return this.m_renewalTime;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x000D40C0 File Offset: 0x000D30C0
		public void Close()
		{
			lock (this.sponsorTable)
			{
				IDictionaryEnumerator enumerator = this.sponsorTable.GetEnumerator();
				while (enumerator.MoveNext())
				{
					((ILease)enumerator.Value).Unregister(this);
				}
				this.sponsorTable.Clear();
			}
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x000D4128 File Offset: 0x000D3128
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x000D412C File Offset: 0x000D312C
		~ClientSponsor()
		{
		}

		// Token: 0x04001F57 RID: 8023
		private Hashtable sponsorTable = new Hashtable(10);

		// Token: 0x04001F58 RID: 8024
		private TimeSpan m_renewalTime = TimeSpan.FromMinutes(2.0);
	}
}
