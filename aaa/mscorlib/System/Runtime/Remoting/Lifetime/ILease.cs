using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006EC RID: 1772
	[ComVisible(true)]
	public interface ILease
	{
		// Token: 0x06003F8D RID: 16269
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Register(ISponsor obj, TimeSpan renewalTime);

		// Token: 0x06003F8E RID: 16270
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Register(ISponsor obj);

		// Token: 0x06003F8F RID: 16271
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Unregister(ISponsor obj);

		// Token: 0x06003F90 RID: 16272
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		TimeSpan Renew(TimeSpan renewalTime);

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06003F91 RID: 16273
		// (set) Token: 0x06003F92 RID: 16274
		TimeSpan RenewOnCallTime
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06003F93 RID: 16275
		// (set) Token: 0x06003F94 RID: 16276
		TimeSpan SponsorshipTimeout
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06003F95 RID: 16277
		// (set) Token: 0x06003F96 RID: 16278
		TimeSpan InitialLeaseTime
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06003F97 RID: 16279
		TimeSpan CurrentLeaseTime
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06003F98 RID: 16280
		LeaseState CurrentState
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
