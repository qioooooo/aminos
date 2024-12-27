using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006F5 RID: 1781
	internal class LeaseManager
	{
		// Token: 0x06003FC3 RID: 16323 RVA: 0x000D9F4C File Offset: 0x000D8F4C
		internal static bool IsInitialized()
		{
			DomainSpecificRemotingData remotingData = Thread.GetDomain().RemotingData;
			LeaseManager leaseManager = remotingData.LeaseManager;
			return leaseManager != null;
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x000D9F74 File Offset: 0x000D8F74
		internal static LeaseManager GetLeaseManager(TimeSpan pollTime)
		{
			DomainSpecificRemotingData remotingData = Thread.GetDomain().RemotingData;
			LeaseManager leaseManager = remotingData.LeaseManager;
			if (leaseManager == null)
			{
				lock (remotingData)
				{
					if (remotingData.LeaseManager == null)
					{
						remotingData.LeaseManager = new LeaseManager(pollTime);
					}
					leaseManager = remotingData.LeaseManager;
				}
			}
			return leaseManager;
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x000D9FD4 File Offset: 0x000D8FD4
		internal static LeaseManager GetLeaseManager()
		{
			DomainSpecificRemotingData remotingData = Thread.GetDomain().RemotingData;
			return remotingData.LeaseManager;
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x000D9FF4 File Offset: 0x000D8FF4
		private LeaseManager(TimeSpan pollTime)
		{
			this.pollTime = pollTime;
			this.leaseTimeAnalyzerDelegate = new TimerCallback(this.LeaseTimeAnalyzer);
			this.waitHandle = new AutoResetEvent(false);
			this.leaseTimer = new Timer(this.leaseTimeAnalyzerDelegate, null, -1, -1);
			this.leaseTimer.Change((int)pollTime.TotalMilliseconds, -1);
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x000DA07C File Offset: 0x000D907C
		internal void ChangePollTime(TimeSpan pollTime)
		{
			this.pollTime = pollTime;
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x000DA088 File Offset: 0x000D9088
		internal void ActivateLease(Lease lease)
		{
			lock (this.leaseToTimeTable)
			{
				this.leaseToTimeTable[lease] = lease.leaseTime;
			}
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x000DA0D4 File Offset: 0x000D90D4
		internal void DeleteLease(Lease lease)
		{
			lock (this.leaseToTimeTable)
			{
				this.leaseToTimeTable.Remove(lease);
			}
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x000DA114 File Offset: 0x000D9114
		[Conditional("_LOGGING")]
		internal void DumpLeases(Lease[] leases)
		{
			for (int i = 0; i < leases.Length; i++)
			{
			}
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x000DA130 File Offset: 0x000D9130
		internal ILease GetLease(MarshalByRefObject obj)
		{
			bool flag = true;
			Identity identity = MarshalByRefObject.GetIdentity(obj, out flag);
			if (identity == null)
			{
				return null;
			}
			return identity.Lease;
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x000DA154 File Offset: 0x000D9154
		internal void ChangedLeaseTime(Lease lease, DateTime newTime)
		{
			lock (this.leaseToTimeTable)
			{
				this.leaseToTimeTable[lease] = newTime;
			}
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x000DA19C File Offset: 0x000D919C
		internal void RegisterSponsorCall(Lease lease, object sponsorId, TimeSpan sponsorshipTimeOut)
		{
			lock (this.sponsorTable)
			{
				DateTime dateTime = DateTime.UtcNow.Add(sponsorshipTimeOut);
				this.sponsorTable[sponsorId] = new LeaseManager.SponsorInfo(lease, sponsorId, dateTime);
			}
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x000DA1F4 File Offset: 0x000D91F4
		internal void DeleteSponsor(object sponsorId)
		{
			lock (this.sponsorTable)
			{
				this.sponsorTable.Remove(sponsorId);
			}
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x000DA234 File Offset: 0x000D9234
		private void LeaseTimeAnalyzer(object state)
		{
			DateTime utcNow = DateTime.UtcNow;
			lock (this.leaseToTimeTable)
			{
				IDictionaryEnumerator enumerator = this.leaseToTimeTable.GetEnumerator();
				while (enumerator.MoveNext())
				{
					DateTime dateTime = (DateTime)enumerator.Value;
					Lease lease = (Lease)enumerator.Key;
					if (dateTime.CompareTo(utcNow) < 0)
					{
						this.tempObjects.Add(lease);
					}
				}
				for (int i = 0; i < this.tempObjects.Count; i++)
				{
					Lease lease2 = (Lease)this.tempObjects[i];
					this.leaseToTimeTable.Remove(lease2);
				}
			}
			for (int j = 0; j < this.tempObjects.Count; j++)
			{
				Lease lease3 = (Lease)this.tempObjects[j];
				if (lease3 != null)
				{
					lease3.LeaseExpired(utcNow);
				}
			}
			this.tempObjects.Clear();
			lock (this.sponsorTable)
			{
				IDictionaryEnumerator enumerator2 = this.sponsorTable.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					object key = enumerator2.Key;
					LeaseManager.SponsorInfo sponsorInfo = (LeaseManager.SponsorInfo)enumerator2.Value;
					if (sponsorInfo.sponsorWaitTime.CompareTo(utcNow) < 0)
					{
						this.tempObjects.Add(sponsorInfo);
					}
				}
				for (int k = 0; k < this.tempObjects.Count; k++)
				{
					LeaseManager.SponsorInfo sponsorInfo2 = (LeaseManager.SponsorInfo)this.tempObjects[k];
					this.sponsorTable.Remove(sponsorInfo2.sponsorId);
				}
			}
			for (int l = 0; l < this.tempObjects.Count; l++)
			{
				LeaseManager.SponsorInfo sponsorInfo3 = (LeaseManager.SponsorInfo)this.tempObjects[l];
				if (sponsorInfo3 != null && sponsorInfo3.lease != null)
				{
					sponsorInfo3.lease.SponsorTimeout(sponsorInfo3.sponsorId);
					this.tempObjects[l] = null;
				}
			}
			this.tempObjects.Clear();
			this.leaseTimer.Change((int)this.pollTime.TotalMilliseconds, -1);
		}

		// Token: 0x04002007 RID: 8199
		private Hashtable leaseToTimeTable = new Hashtable();

		// Token: 0x04002008 RID: 8200
		private Hashtable sponsorTable = new Hashtable();

		// Token: 0x04002009 RID: 8201
		private TimeSpan pollTime;

		// Token: 0x0400200A RID: 8202
		private AutoResetEvent waitHandle;

		// Token: 0x0400200B RID: 8203
		private TimerCallback leaseTimeAnalyzerDelegate;

		// Token: 0x0400200C RID: 8204
		private volatile Timer leaseTimer;

		// Token: 0x0400200D RID: 8205
		private ArrayList tempObjects = new ArrayList(10);

		// Token: 0x020006F6 RID: 1782
		internal class SponsorInfo
		{
			// Token: 0x06003FD0 RID: 16336 RVA: 0x000DA468 File Offset: 0x000D9468
			internal SponsorInfo(Lease lease, object sponsorId, DateTime sponsorWaitTime)
			{
				this.lease = lease;
				this.sponsorId = sponsorId;
				this.sponsorWaitTime = sponsorWaitTime;
			}

			// Token: 0x0400200E RID: 8206
			internal Lease lease;

			// Token: 0x0400200F RID: 8207
			internal object sponsorId;

			// Token: 0x04002010 RID: 8208
			internal DateTime sponsorWaitTime;
		}
	}
}
