using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006F8 RID: 1784
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public sealed class LifetimeServices
	{
		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06003FD1 RID: 16337 RVA: 0x000DA488 File Offset: 0x000D9488
		private static object LifetimeSyncObject
		{
			get
			{
				if (LifetimeServices.s_LifetimeSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref LifetimeServices.s_LifetimeSyncObject, obj, null);
				}
				return LifetimeServices.s_LifetimeSyncObject;
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x000DA4B4 File Offset: 0x000D94B4
		// (set) Token: 0x06003FD3 RID: 16339 RVA: 0x000DA4BC File Offset: 0x000D94BC
		public static TimeSpan LeaseTime
		{
			get
			{
				return LifetimeServices.m_leaseTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				lock (LifetimeServices.LifetimeSyncObject)
				{
					if (LifetimeServices.isLeaseTime)
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_SetOnce"), new object[] { "LeaseTime" }));
					}
					LifetimeServices.m_leaseTime = value;
					LifetimeServices.isLeaseTime = true;
				}
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06003FD4 RID: 16340 RVA: 0x000DA52C File Offset: 0x000D952C
		// (set) Token: 0x06003FD5 RID: 16341 RVA: 0x000DA534 File Offset: 0x000D9534
		public static TimeSpan RenewOnCallTime
		{
			get
			{
				return LifetimeServices.m_renewOnCallTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				lock (LifetimeServices.LifetimeSyncObject)
				{
					if (LifetimeServices.isRenewOnCallTime)
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_SetOnce"), new object[] { "RenewOnCallTime" }));
					}
					LifetimeServices.m_renewOnCallTime = value;
					LifetimeServices.isRenewOnCallTime = true;
				}
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06003FD6 RID: 16342 RVA: 0x000DA5A4 File Offset: 0x000D95A4
		// (set) Token: 0x06003FD7 RID: 16343 RVA: 0x000DA5AC File Offset: 0x000D95AC
		public static TimeSpan SponsorshipTimeout
		{
			get
			{
				return LifetimeServices.m_sponsorshipTimeout;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				lock (LifetimeServices.LifetimeSyncObject)
				{
					if (LifetimeServices.isSponsorshipTimeout)
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_SetOnce"), new object[] { "SponsorshipTimeout" }));
					}
					LifetimeServices.m_sponsorshipTimeout = value;
					LifetimeServices.isSponsorshipTimeout = true;
				}
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x000DA61C File Offset: 0x000D961C
		// (set) Token: 0x06003FD9 RID: 16345 RVA: 0x000DA624 File Offset: 0x000D9624
		public static TimeSpan LeaseManagerPollTime
		{
			get
			{
				return LifetimeServices.m_pollTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				lock (LifetimeServices.LifetimeSyncObject)
				{
					LifetimeServices.m_pollTime = value;
					if (LeaseManager.IsInitialized())
					{
						LeaseManager.GetLeaseManager().ChangePollTime(LifetimeServices.m_pollTime);
					}
				}
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x000DA674 File Offset: 0x000D9674
		internal static ILease GetLeaseInitial(MarshalByRefObject obj)
		{
			LeaseManager leaseManager = LeaseManager.GetLeaseManager(LifetimeServices.LeaseManagerPollTime);
			ILease lease = leaseManager.GetLease(obj);
			if (lease == null)
			{
				lease = LifetimeServices.CreateLease(obj);
			}
			return lease;
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x000DA6A4 File Offset: 0x000D96A4
		internal static ILease GetLease(MarshalByRefObject obj)
		{
			LeaseManager leaseManager = LeaseManager.GetLeaseManager(LifetimeServices.LeaseManagerPollTime);
			return leaseManager.GetLease(obj);
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x000DA6C7 File Offset: 0x000D96C7
		internal static ILease CreateLease(MarshalByRefObject obj)
		{
			return LifetimeServices.CreateLease(LifetimeServices.LeaseTime, LifetimeServices.RenewOnCallTime, LifetimeServices.SponsorshipTimeout, obj);
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x000DA6DE File Offset: 0x000D96DE
		internal static ILease CreateLease(TimeSpan leaseTime, TimeSpan renewOnCallTime, TimeSpan sponsorshipTimeout, MarshalByRefObject obj)
		{
			LeaseManager.GetLeaseManager(LifetimeServices.LeaseManagerPollTime);
			return new Lease(leaseTime, renewOnCallTime, sponsorshipTimeout, obj);
		}

		// Token: 0x04002017 RID: 8215
		private static bool isLeaseTime = false;

		// Token: 0x04002018 RID: 8216
		private static bool isRenewOnCallTime = false;

		// Token: 0x04002019 RID: 8217
		private static bool isSponsorshipTimeout = false;

		// Token: 0x0400201A RID: 8218
		private static TimeSpan m_leaseTime = TimeSpan.FromMinutes(5.0);

		// Token: 0x0400201B RID: 8219
		private static TimeSpan m_renewOnCallTime = TimeSpan.FromMinutes(2.0);

		// Token: 0x0400201C RID: 8220
		private static TimeSpan m_sponsorshipTimeout = TimeSpan.FromMinutes(2.0);

		// Token: 0x0400201D RID: 8221
		private static TimeSpan m_pollTime = TimeSpan.FromMilliseconds(10000.0);

		// Token: 0x0400201E RID: 8222
		private static object s_LifetimeSyncObject = null;
	}
}
