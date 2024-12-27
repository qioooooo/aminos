using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006F0 RID: 1776
	internal class Lease : MarshalByRefObject, ILease
	{
		// Token: 0x06003F9E RID: 16286 RVA: 0x000D95F8 File Offset: 0x000D85F8
		internal Lease(TimeSpan initialLeaseTime, TimeSpan renewOnCallTime, TimeSpan sponsorshipTimeout, MarshalByRefObject managedObject)
		{
			this.id = Lease.nextId++;
			this.renewOnCallTime = renewOnCallTime;
			this.sponsorshipTimeout = sponsorshipTimeout;
			this.initialLeaseTime = initialLeaseTime;
			this.managedObject = managedObject;
			this.leaseManager = LeaseManager.GetLeaseManager();
			this.sponsorTable = new Hashtable(10);
			this.state = LeaseState.Initial;
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x000D965C File Offset: 0x000D865C
		internal void ActivateLease()
		{
			this.leaseTime = DateTime.UtcNow.Add(this.initialLeaseTime);
			this.state = LeaseState.Active;
			this.leaseManager.ActivateLease(this);
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x000D9695 File Offset: 0x000D8695
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x000D9698 File Offset: 0x000D8698
		// (set) Token: 0x06003FA2 RID: 16290 RVA: 0x000D96A0 File Offset: 0x000D86A0
		public TimeSpan RenewOnCallTime
		{
			get
			{
				return this.renewOnCallTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				if (this.state == LeaseState.Initial)
				{
					this.renewOnCallTime = value;
					return;
				}
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_InitialStateRenewOnCall"), new object[] { this.state.ToString() }));
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x000D96F2 File Offset: 0x000D86F2
		// (set) Token: 0x06003FA4 RID: 16292 RVA: 0x000D96FC File Offset: 0x000D86FC
		public TimeSpan SponsorshipTimeout
		{
			get
			{
				return this.sponsorshipTimeout;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				if (this.state == LeaseState.Initial)
				{
					this.sponsorshipTimeout = value;
					return;
				}
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_InitialStateSponsorshipTimeout"), new object[] { this.state.ToString() }));
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06003FA5 RID: 16293 RVA: 0x000D974E File Offset: 0x000D874E
		// (set) Token: 0x06003FA6 RID: 16294 RVA: 0x000D9758 File Offset: 0x000D8758
		public TimeSpan InitialLeaseTime
		{
			get
			{
				return this.initialLeaseTime;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				if (this.state != LeaseState.Initial)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Lifetime_InitialStateInitialLeaseTime"), new object[] { this.state.ToString() }));
				}
				this.initialLeaseTime = value;
				if (TimeSpan.Zero.CompareTo(value) >= 0)
				{
					this.state = LeaseState.Null;
					return;
				}
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06003FA7 RID: 16295 RVA: 0x000D97C3 File Offset: 0x000D87C3
		public TimeSpan CurrentLeaseTime
		{
			get
			{
				return this.leaseTime.Subtract(DateTime.UtcNow);
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06003FA8 RID: 16296 RVA: 0x000D97D5 File Offset: 0x000D87D5
		public LeaseState CurrentState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x000D97DD File Offset: 0x000D87DD
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public void Register(ISponsor obj)
		{
			this.Register(obj, TimeSpan.Zero);
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x000D97EC File Offset: 0x000D87EC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public void Register(ISponsor obj, TimeSpan renewalTime)
		{
			lock (this)
			{
				if (this.state != LeaseState.Expired && !(this.sponsorshipTimeout == TimeSpan.Zero))
				{
					object sponsorId = this.GetSponsorId(obj);
					lock (this.sponsorTable)
					{
						if (renewalTime > TimeSpan.Zero)
						{
							this.AddTime(renewalTime);
						}
						if (!this.sponsorTable.ContainsKey(sponsorId))
						{
							this.sponsorTable[sponsorId] = new Lease.SponsorStateInfo(renewalTime, Lease.SponsorState.Initial);
						}
					}
				}
			}
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x000D9898 File Offset: 0x000D8898
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public void Unregister(ISponsor sponsor)
		{
			lock (this)
			{
				if (this.state != LeaseState.Expired)
				{
					object sponsorId = this.GetSponsorId(sponsor);
					lock (this.sponsorTable)
					{
						if (sponsorId != null)
						{
							this.leaseManager.DeleteSponsor(sponsorId);
							Lease.SponsorStateInfo sponsorStateInfo = (Lease.SponsorStateInfo)this.sponsorTable[sponsorId];
							this.sponsorTable.Remove(sponsorId);
						}
					}
				}
			}
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x000D9928 File Offset: 0x000D8928
		private object GetSponsorId(ISponsor obj)
		{
			object obj2 = null;
			if (obj != null)
			{
				if (RemotingServices.IsTransparentProxy(obj))
				{
					obj2 = RemotingServices.GetRealProxy(obj);
				}
				else
				{
					obj2 = obj;
				}
			}
			return obj2;
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x000D9950 File Offset: 0x000D8950
		private ISponsor GetSponsorFromId(object sponsorId)
		{
			RealProxy realProxy = sponsorId as RealProxy;
			object obj;
			if (realProxy != null)
			{
				obj = realProxy.GetTransparentProxy();
			}
			else
			{
				obj = sponsorId;
			}
			return (ISponsor)obj;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x000D997A File Offset: 0x000D897A
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public TimeSpan Renew(TimeSpan renewalTime)
		{
			return this.RenewInternal(renewalTime);
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x000D9984 File Offset: 0x000D8984
		internal TimeSpan RenewInternal(TimeSpan renewalTime)
		{
			TimeSpan timeSpan;
			lock (this)
			{
				if (this.state == LeaseState.Expired)
				{
					timeSpan = TimeSpan.Zero;
				}
				else
				{
					this.AddTime(renewalTime);
					timeSpan = this.leaseTime.Subtract(DateTime.UtcNow);
				}
			}
			return timeSpan;
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x000D99DC File Offset: 0x000D89DC
		internal void Remove()
		{
			if (this.state == LeaseState.Expired)
			{
				return;
			}
			this.state = LeaseState.Expired;
			this.leaseManager.DeleteLease(this);
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x000D99FC File Offset: 0x000D89FC
		internal void Cancel()
		{
			lock (this)
			{
				if (this.state != LeaseState.Expired)
				{
					this.Remove();
					RemotingServices.Disconnect(this.managedObject, false);
					RemotingServices.Disconnect(this);
				}
			}
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x000D9A50 File Offset: 0x000D8A50
		internal void RenewOnCall()
		{
			lock (this)
			{
				if (this.state != LeaseState.Initial && this.state != LeaseState.Expired)
				{
					this.AddTime(this.renewOnCallTime);
				}
			}
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x000D9AA0 File Offset: 0x000D8AA0
		internal void LeaseExpired(DateTime now)
		{
			lock (this)
			{
				if (this.state != LeaseState.Expired)
				{
					if (this.leaseTime.CompareTo(now) < 0)
					{
						this.ProcessNextSponsor();
					}
				}
			}
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x000D9AF0 File Offset: 0x000D8AF0
		internal void SponsorCall(ISponsor sponsor)
		{
			bool flag = false;
			if (this.state == LeaseState.Expired)
			{
				return;
			}
			lock (this.sponsorTable)
			{
				try
				{
					object sponsorId = this.GetSponsorId(sponsor);
					this.sponsorCallThread = Thread.CurrentThread.GetHashCode();
					Lease.AsyncRenewal asyncRenewal = new Lease.AsyncRenewal(sponsor.Renewal);
					Lease.SponsorStateInfo sponsorStateInfo = (Lease.SponsorStateInfo)this.sponsorTable[sponsorId];
					sponsorStateInfo.sponsorState = Lease.SponsorState.Waiting;
					asyncRenewal.BeginInvoke(this, new AsyncCallback(this.SponsorCallback), null);
					if (sponsorStateInfo.sponsorState == Lease.SponsorState.Waiting && this.state != LeaseState.Expired)
					{
						this.leaseManager.RegisterSponsorCall(this, sponsorId, this.sponsorshipTimeout);
					}
					this.sponsorCallThread = 0;
				}
				catch (Exception)
				{
					flag = true;
					this.sponsorCallThread = 0;
				}
			}
			if (flag)
			{
				this.Unregister(sponsor);
				this.ProcessNextSponsor();
			}
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x000D9BDC File Offset: 0x000D8BDC
		internal void SponsorTimeout(object sponsorId)
		{
			lock (this)
			{
				if (this.sponsorTable.ContainsKey(sponsorId))
				{
					lock (this.sponsorTable)
					{
						Lease.SponsorStateInfo sponsorStateInfo = (Lease.SponsorStateInfo)this.sponsorTable[sponsorId];
						if (sponsorStateInfo.sponsorState == Lease.SponsorState.Waiting)
						{
							this.Unregister(this.GetSponsorFromId(sponsorId));
							this.ProcessNextSponsor();
						}
					}
				}
			}
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x000D9C6C File Offset: 0x000D8C6C
		private void ProcessNextSponsor()
		{
			object obj = null;
			TimeSpan timeSpan = TimeSpan.Zero;
			lock (this.sponsorTable)
			{
				IDictionaryEnumerator enumerator = this.sponsorTable.GetEnumerator();
				while (enumerator.MoveNext())
				{
					object key = enumerator.Key;
					Lease.SponsorStateInfo sponsorStateInfo = (Lease.SponsorStateInfo)enumerator.Value;
					if (sponsorStateInfo.sponsorState == Lease.SponsorState.Initial && timeSpan == TimeSpan.Zero)
					{
						timeSpan = sponsorStateInfo.renewalTime;
						obj = key;
					}
					else if (sponsorStateInfo.renewalTime > timeSpan)
					{
						timeSpan = sponsorStateInfo.renewalTime;
						obj = key;
					}
				}
			}
			if (obj != null)
			{
				this.SponsorCall(this.GetSponsorFromId(obj));
				return;
			}
			this.Cancel();
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x000D9D28 File Offset: 0x000D8D28
		internal void SponsorCallback(object obj)
		{
			this.SponsorCallback((IAsyncResult)obj);
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x000D9D38 File Offset: 0x000D8D38
		internal void SponsorCallback(IAsyncResult iar)
		{
			if (this.state == LeaseState.Expired)
			{
				return;
			}
			int hashCode = Thread.CurrentThread.GetHashCode();
			if (hashCode == this.sponsorCallThread)
			{
				WaitCallback waitCallback = new WaitCallback(this.SponsorCallback);
				ThreadPool.QueueUserWorkItem(waitCallback, iar);
				return;
			}
			AsyncResult asyncResult = (AsyncResult)iar;
			Lease.AsyncRenewal asyncRenewal = (Lease.AsyncRenewal)asyncResult.AsyncDelegate;
			ISponsor sponsor = (ISponsor)asyncRenewal.Target;
			Lease.SponsorStateInfo sponsorStateInfo = null;
			if (!iar.IsCompleted)
			{
				this.Unregister(sponsor);
				this.ProcessNextSponsor();
				return;
			}
			bool flag = false;
			TimeSpan timeSpan = TimeSpan.Zero;
			try
			{
				timeSpan = asyncRenewal.EndInvoke(iar);
			}
			catch (Exception)
			{
				flag = true;
			}
			if (flag)
			{
				this.Unregister(sponsor);
				this.ProcessNextSponsor();
				return;
			}
			object sponsorId = this.GetSponsorId(sponsor);
			lock (this.sponsorTable)
			{
				if (this.sponsorTable.ContainsKey(sponsorId))
				{
					sponsorStateInfo = (Lease.SponsorStateInfo)this.sponsorTable[sponsorId];
					sponsorStateInfo.sponsorState = Lease.SponsorState.Completed;
					sponsorStateInfo.renewalTime = timeSpan;
				}
			}
			if (sponsorStateInfo == null)
			{
				this.ProcessNextSponsor();
				return;
			}
			if (sponsorStateInfo.renewalTime == TimeSpan.Zero)
			{
				this.Unregister(sponsor);
				this.ProcessNextSponsor();
				return;
			}
			this.RenewInternal(sponsorStateInfo.renewalTime);
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x000D9E94 File Offset: 0x000D8E94
		private void AddTime(TimeSpan renewalSpan)
		{
			if (this.state == LeaseState.Expired)
			{
				return;
			}
			DateTime dateTime = DateTime.UtcNow.Add(renewalSpan);
			if (this.leaseTime.CompareTo(dateTime) < 0)
			{
				this.leaseManager.ChangedLeaseTime(this, dateTime);
				this.leaseTime = dateTime;
				this.state = LeaseState.Active;
			}
		}

		// Token: 0x04001FF3 RID: 8179
		internal int id;

		// Token: 0x04001FF4 RID: 8180
		internal DateTime leaseTime;

		// Token: 0x04001FF5 RID: 8181
		internal TimeSpan initialLeaseTime;

		// Token: 0x04001FF6 RID: 8182
		internal TimeSpan renewOnCallTime;

		// Token: 0x04001FF7 RID: 8183
		internal TimeSpan sponsorshipTimeout;

		// Token: 0x04001FF8 RID: 8184
		internal bool isInfinite;

		// Token: 0x04001FF9 RID: 8185
		internal Hashtable sponsorTable;

		// Token: 0x04001FFA RID: 8186
		internal int sponsorCallThread;

		// Token: 0x04001FFB RID: 8187
		internal LeaseManager leaseManager;

		// Token: 0x04001FFC RID: 8188
		internal MarshalByRefObject managedObject;

		// Token: 0x04001FFD RID: 8189
		internal LeaseState state;

		// Token: 0x04001FFE RID: 8190
		internal static int nextId;

		// Token: 0x020006F1 RID: 1777
		// (Invoke) Token: 0x06003FBB RID: 16315
		internal delegate TimeSpan AsyncRenewal(ILease lease);

		// Token: 0x020006F2 RID: 1778
		[Serializable]
		internal enum SponsorState
		{
			// Token: 0x04002000 RID: 8192
			Initial,
			// Token: 0x04002001 RID: 8193
			Waiting,
			// Token: 0x04002002 RID: 8194
			Completed
		}

		// Token: 0x020006F3 RID: 1779
		internal sealed class SponsorStateInfo
		{
			// Token: 0x06003FBE RID: 16318 RVA: 0x000D9EE4 File Offset: 0x000D8EE4
			internal SponsorStateInfo(TimeSpan renewalTime, Lease.SponsorState sponsorState)
			{
				this.renewalTime = renewalTime;
				this.sponsorState = sponsorState;
			}

			// Token: 0x04002003 RID: 8195
			internal TimeSpan renewalTime;

			// Token: 0x04002004 RID: 8196
			internal Lease.SponsorState sponsorState;
		}
	}
}
