using System;
using System.ComponentModel;
using System.Web.Caching;

namespace System.Web.UI
{
	// Token: 0x020003D8 RID: 984
	internal class DataSourceCache : IStateManager
	{
		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06002FE3 RID: 12259 RVA: 0x000D4744 File Offset: 0x000D3744
		// (set) Token: 0x06002FE4 RID: 12260 RVA: 0x000D476D File Offset: 0x000D376D
		public virtual int Duration
		{
			get
			{
				object obj = this.ViewState["Duration"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("DataSourceCache_InvalidDuration"));
				}
				this.ViewState["Duration"] = value;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x000D47A0 File Offset: 0x000D37A0
		// (set) Token: 0x06002FE6 RID: 12262 RVA: 0x000D47C9 File Offset: 0x000D37C9
		public virtual bool Enabled
		{
			get
			{
				object obj = this.ViewState["Enabled"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["Enabled"] = value;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x000D47E4 File Offset: 0x000D37E4
		// (set) Token: 0x06002FE8 RID: 12264 RVA: 0x000D480D File Offset: 0x000D380D
		public virtual DataSourceCacheExpiry ExpirationPolicy
		{
			get
			{
				object obj = this.ViewState["ExpirationPolicy"];
				if (obj != null)
				{
					return (DataSourceCacheExpiry)obj;
				}
				return DataSourceCacheExpiry.Absolute;
			}
			set
			{
				if (value < DataSourceCacheExpiry.Absolute || value > DataSourceCacheExpiry.Sliding)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("DataSourceCache_InvalidExpiryPolicy"));
				}
				this.ViewState["ExpirationPolicy"] = value;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x000D4840 File Offset: 0x000D3840
		// (set) Token: 0x06002FEA RID: 12266 RVA: 0x000D486D File Offset: 0x000D386D
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		[WebSysDescription("DataSourceCache_KeyDependency")]
		public virtual string KeyDependency
		{
			get
			{
				object obj = this.ViewState["KeyDependency"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["KeyDependency"] = value;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06002FEB RID: 12267 RVA: 0x000D4880 File Offset: 0x000D3880
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		protected StateBag ViewState
		{
			get
			{
				if (this._viewState == null)
				{
					this._viewState = new StateBag();
					if (this._tracking)
					{
						this._viewState.TrackViewState();
					}
				}
				return this._viewState;
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000D48AE File Offset: 0x000D38AE
		public void Invalidate(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (!this.Enabled)
			{
				throw new InvalidOperationException(SR.GetString("DataSourceCache_CacheMustBeEnabled"));
			}
			HttpRuntime.CacheInternal.Remove(key);
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x000D48E7 File Offset: 0x000D38E7
		public object LoadDataFromCache(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (!this.Enabled)
			{
				throw new InvalidOperationException(SR.GetString("DataSourceCache_CacheMustBeEnabled"));
			}
			return HttpRuntime.CacheInternal.Get(key);
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x000D491F File Offset: 0x000D391F
		protected virtual void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				((IStateManager)this.ViewState).LoadViewState(savedState);
			}
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x000D4930 File Offset: 0x000D3930
		public void SaveDataToCache(string key, object data)
		{
			this.SaveDataToCache(key, data, null);
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000D493B File Offset: 0x000D393B
		public void SaveDataToCache(string key, object data, CacheDependency dependency)
		{
			this.SaveDataToCacheInternal(key, data, dependency);
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000D4948 File Offset: 0x000D3948
		protected virtual void SaveDataToCacheInternal(string key, object data, CacheDependency dependency)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (!this.Enabled)
			{
				throw new InvalidOperationException(SR.GetString("DataSourceCache_CacheMustBeEnabled"));
			}
			DateTime dateTime = Cache.NoAbsoluteExpiration;
			TimeSpan timeSpan = Cache.NoSlidingExpiration;
			switch (this.ExpirationPolicy)
			{
			case DataSourceCacheExpiry.Absolute:
				dateTime = DateTime.UtcNow.AddSeconds((double)((this.Duration == 0) ? int.MaxValue : this.Duration));
				break;
			case DataSourceCacheExpiry.Sliding:
				timeSpan = TimeSpan.FromSeconds((double)this.Duration);
				break;
			}
			AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
			if (this.KeyDependency.Length > 0)
			{
				string[] array = new string[] { this.KeyDependency };
				aggregateCacheDependency.Add(new CacheDependency[]
				{
					new CacheDependency(null, array)
				});
			}
			if (dependency != null)
			{
				aggregateCacheDependency.Add(new CacheDependency[] { dependency });
			}
			HttpRuntime.CacheInternal.UtcInsert(key, data, aggregateCacheDependency, dateTime, timeSpan);
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000D4A46 File Offset: 0x000D3A46
		protected virtual object SaveViewState()
		{
			if (this._viewState == null)
			{
				return null;
			}
			return ((IStateManager)this._viewState).SaveViewState();
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000D4A5D File Offset: 0x000D3A5D
		protected void TrackViewState()
		{
			this._tracking = true;
			if (this._viewState != null)
			{
				this._viewState.TrackViewState();
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x000D4A79 File Offset: 0x000D3A79
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this._tracking;
			}
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000D4A81 File Offset: 0x000D3A81
		void IStateManager.LoadViewState(object savedState)
		{
			this.LoadViewState(savedState);
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000D4A8A File Offset: 0x000D3A8A
		object IStateManager.SaveViewState()
		{
			return this.SaveViewState();
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000D4A92 File Offset: 0x000D3A92
		void IStateManager.TrackViewState()
		{
			this.TrackViewState();
		}

		// Token: 0x040021F8 RID: 8696
		public const int Infinite = 0;

		// Token: 0x040021F9 RID: 8697
		private bool _tracking;

		// Token: 0x040021FA RID: 8698
		private StateBag _viewState;
	}
}
