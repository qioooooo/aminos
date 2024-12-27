using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Web.Caching;

namespace System.Web.SessionState
{
	// Token: 0x0200035D RID: 861
	internal sealed class InProcSessionStateStore : SessionStateStoreProviderBase
	{
		// Token: 0x060029EB RID: 10731 RVA: 0x000BAF58 File Offset: 0x000B9F58
		public void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
		{
			PerfCounters.DecrementCounter(AppPerfCounter.SESSIONS_ACTIVE);
			InProcSessionState inProcSessionState = (InProcSessionState)value;
			if ((inProcSessionState._flags & 2) != 0 || (inProcSessionState._flags & 1) != 0)
			{
				return;
			}
			switch (reason)
			{
			case CacheItemRemovedReason.Removed:
				PerfCounters.IncrementCounter(AppPerfCounter.SESSIONS_ABANDONED);
				break;
			case CacheItemRemovedReason.Expired:
				PerfCounters.IncrementCounter(AppPerfCounter.SESSIONS_TIMED_OUT);
				break;
			}
			if (this._expireCallback != null)
			{
				string text = key.Substring(InProcSessionStateStore.CACHEKEYPREFIXLENGTH);
				this._expireCallback(text, SessionStateUtility.CreateLegitStoreData(null, inProcSessionState._sessionItems, inProcSessionState._staticObjects, inProcSessionState._timeout));
			}
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000BAFE4 File Offset: 0x000B9FE4
		private string CreateSessionStateCacheKey(string id)
		{
			return "j" + id;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x000BAFF1 File Offset: 0x000B9FF1
		public override void Initialize(string name, NameValueCollection config)
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "InProc Session State Provider";
			}
			base.Initialize(name, config);
			this._callback = new CacheItemRemovedCallback(this.OnCacheItemRemoved);
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x000BB01C File Offset: 0x000BA01C
		public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
		{
			this._expireCallback = expireCallback;
			return true;
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000BB026 File Offset: 0x000BA026
		public override void Dispose()
		{
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x000BB028 File Offset: 0x000BA028
		public override void InitializeRequest(HttpContext context)
		{
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x000BB02C File Offset: 0x000BA02C
		private SessionStateStoreData DoGet(HttpContext context, string id, bool exclusive, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			string text = this.CreateSessionStateCacheKey(id);
			locked = false;
			lockId = null;
			lockAge = TimeSpan.Zero;
			actionFlags = SessionStateActions.None;
			SessionIDManager.CheckIdLength(id, true);
			InProcSessionState inProcSessionState = (InProcSessionState)HttpRuntime.CacheInternal.Get(text);
			if (inProcSessionState == null)
			{
				return null;
			}
			int flags = inProcSessionState._flags;
			if ((flags & 1) != 0 && flags == Interlocked.CompareExchange(ref inProcSessionState._flags, flags & -2, flags))
			{
				actionFlags = SessionStateActions.InitializeItem;
			}
			bool flag;
			if (exclusive)
			{
				flag = true;
				if (!inProcSessionState._locked)
				{
					inProcSessionState._spinLock.AcquireWriterLock();
					try
					{
						if (!inProcSessionState._locked)
						{
							flag = false;
							inProcSessionState._locked = true;
							inProcSessionState._utcLockDate = DateTime.UtcNow;
							inProcSessionState._lockCookie++;
						}
						lockId = inProcSessionState._lockCookie;
						goto IL_00FE;
					}
					finally
					{
						inProcSessionState._spinLock.ReleaseWriterLock();
					}
				}
				lockId = inProcSessionState._lockCookie;
			}
			else
			{
				inProcSessionState._spinLock.AcquireReaderLock();
				try
				{
					flag = inProcSessionState._locked;
					lockId = inProcSessionState._lockCookie;
				}
				finally
				{
					inProcSessionState._spinLock.ReleaseReaderLock();
				}
			}
			IL_00FE:
			if (flag)
			{
				locked = true;
				lockAge = DateTime.UtcNow - inProcSessionState._utcLockDate;
				return null;
			}
			return SessionStateUtility.CreateLegitStoreData(context, inProcSessionState._sessionItems, inProcSessionState._staticObjects, inProcSessionState._timeout);
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x000BB190 File Offset: 0x000BA190
		public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			return this.DoGet(context, id, false, out locked, out lockAge, out lockId, out actionFlags);
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x000BB1A2 File Offset: 0x000BA1A2
		public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			return this.DoGet(context, id, true, out locked, out lockAge, out lockId, out actionFlags);
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x000BB1B4 File Offset: 0x000BA1B4
		public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
		{
			string text = this.CreateSessionStateCacheKey(id);
			int num = (int)lockId;
			SessionIDManager.CheckIdLength(id, true);
			InProcSessionState inProcSessionState = (InProcSessionState)HttpRuntime.CacheInternal.Get(text);
			if (inProcSessionState == null)
			{
				return;
			}
			if (inProcSessionState._locked)
			{
				inProcSessionState._spinLock.AcquireWriterLock();
				try
				{
					if (inProcSessionState._locked && num == inProcSessionState._lockCookie)
					{
						inProcSessionState._locked = false;
					}
				}
				finally
				{
					inProcSessionState._spinLock.ReleaseWriterLock();
				}
			}
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x000BB238 File Offset: 0x000BA238
		public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
		{
			string text = this.CreateSessionStateCacheKey(id);
			bool flag = true;
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			int num = InProcSessionStateStore.NewLockCookie;
			ISessionStateItemCollection sessionStateItemCollection = null;
			HttpStaticObjectsCollection httpStaticObjectsCollection = null;
			SessionIDManager.CheckIdLength(id, true);
			if (item.Items.Count > 0)
			{
				sessionStateItemCollection = item.Items;
			}
			if (!item.StaticObjects.NeverAccessed)
			{
				httpStaticObjectsCollection = item.StaticObjects;
			}
			if (!newItem)
			{
				InProcSessionState inProcSessionState = (InProcSessionState)cacheInternal.Get(text);
				int num2 = (int)lockId;
				if (inProcSessionState == null)
				{
					return;
				}
				inProcSessionState._spinLock.AcquireWriterLock();
				try
				{
					if (!inProcSessionState._locked || inProcSessionState._lockCookie != num2)
					{
						return;
					}
					if (inProcSessionState._timeout == item.Timeout)
					{
						inProcSessionState.Copy(sessionStateItemCollection, httpStaticObjectsCollection, item.Timeout, false, DateTime.MinValue, num2, inProcSessionState._flags);
						flag = false;
					}
					else
					{
						inProcSessionState._flags |= 2;
						num = num2;
						inProcSessionState._lockCookie = 0;
					}
				}
				finally
				{
					inProcSessionState._spinLock.ReleaseWriterLock();
				}
			}
			if (flag)
			{
				InProcSessionState inProcSessionState2 = new InProcSessionState(sessionStateItemCollection, httpStaticObjectsCollection, item.Timeout, false, DateTime.MinValue, num, 0);
				try
				{
				}
				finally
				{
					cacheInternal.UtcInsert(text, inProcSessionState2, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, inProcSessionState2._timeout, 0), CacheItemPriority.NotRemovable, this._callback);
					PerfCounters.IncrementCounter(AppPerfCounter.SESSIONS_TOTAL);
					PerfCounters.IncrementCounter(AppPerfCounter.SESSIONS_ACTIVE);
				}
			}
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x000BB3A8 File Offset: 0x000BA3A8
		public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
		{
			string text = this.CreateSessionStateCacheKey(id);
			SessionIDManager.CheckIdLength(id, true);
			InProcSessionState inProcSessionState = new InProcSessionState(null, null, timeout, false, DateTime.MinValue, InProcSessionStateStore.NewLockCookie, 1);
			try
			{
			}
			finally
			{
				if (HttpRuntime.CacheInternal.UtcAdd(text, inProcSessionState, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, timeout, 0), CacheItemPriority.NotRemovable, this._callback) == null)
				{
					PerfCounters.IncrementCounter(AppPerfCounter.SESSIONS_TOTAL);
					PerfCounters.IncrementCounter(AppPerfCounter.SESSIONS_ACTIVE);
				}
			}
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x000BB420 File Offset: 0x000BA420
		public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
		{
			string text = this.CreateSessionStateCacheKey(id);
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			int num = (int)lockId;
			SessionIDManager.CheckIdLength(id, true);
			InProcSessionState inProcSessionState = (InProcSessionState)cacheInternal.Get(text);
			if (inProcSessionState == null)
			{
				return;
			}
			inProcSessionState._spinLock.AcquireWriterLock();
			try
			{
				if (!inProcSessionState._locked || inProcSessionState._lockCookie != num)
				{
					return;
				}
				inProcSessionState._lockCookie = 0;
			}
			finally
			{
				inProcSessionState._spinLock.ReleaseWriterLock();
			}
			cacheInternal.Remove(text);
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x000BB4A8 File Offset: 0x000BA4A8
		public override void ResetItemTimeout(HttpContext context, string id)
		{
			string text = this.CreateSessionStateCacheKey(id);
			SessionIDManager.CheckIdLength(id, true);
			HttpRuntime.CacheInternal.Get(text);
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x000BB4D1 File Offset: 0x000BA4D1
		public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
		{
			return SessionStateUtility.CreateLegitStoreData(context, null, null, timeout);
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x000BB4DC File Offset: 0x000BA4DC
		public override void EndRequest(HttpContext context)
		{
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x000BB4DE File Offset: 0x000BA4DE
		[Conditional("DBG")]
		internal static void TraceSessionStats()
		{
		}

		// Token: 0x04001F24 RID: 7972
		internal static readonly int CACHEKEYPREFIXLENGTH = "j".Length;

		// Token: 0x04001F25 RID: 7973
		internal static readonly int NewLockCookie = 1;

		// Token: 0x04001F26 RID: 7974
		private CacheItemRemovedCallback _callback;

		// Token: 0x04001F27 RID: 7975
		private SessionStateItemExpireCallback _expireCallback;
	}
}
