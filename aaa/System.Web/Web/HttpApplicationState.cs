using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200004F RID: 79
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpApplicationState : NameObjectCollectionBase
	{
		// Token: 0x06000271 RID: 625 RVA: 0x0000C80B File Offset: 0x0000B80B
		internal HttpApplicationState()
			: this(null, null)
		{
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000C818 File Offset: 0x0000B818
		internal HttpApplicationState(HttpStaticObjectsCollection applicationStaticObjects, HttpStaticObjectsCollection sessionStaticObjects)
			: base(Misc.CaseInsensitiveInvariantKeyComparer)
		{
			this._applicationStaticObjects = applicationStaticObjects;
			if (this._applicationStaticObjects == null)
			{
				this._applicationStaticObjects = new HttpStaticObjectsCollection();
			}
			this._sessionStaticObjects = sessionStaticObjects;
			if (this._sessionStaticObjects == null)
			{
				this._sessionStaticObjects = new HttpStaticObjectsCollection();
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000C86F File Offset: 0x0000B86F
		internal HttpStaticObjectsCollection SessionStaticObjects
		{
			get
			{
				return this._sessionStaticObjects;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000C878 File Offset: 0x0000B878
		public override int Count
		{
			get
			{
				int num = 0;
				this._lock.AcquireRead();
				try
				{
					num = base.Count;
				}
				finally
				{
					this._lock.ReleaseRead();
				}
				return num;
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000C8B8 File Offset: 0x0000B8B8
		public void Add(string name, object value)
		{
			this._lock.AcquireWrite();
			try
			{
				base.BaseAdd(name, value);
			}
			finally
			{
				this._lock.ReleaseWrite();
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000C8F8 File Offset: 0x0000B8F8
		public void Set(string name, object value)
		{
			this._lock.AcquireWrite();
			try
			{
				base.BaseSet(name, value);
			}
			finally
			{
				this._lock.ReleaseWrite();
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000C938 File Offset: 0x0000B938
		public void Remove(string name)
		{
			this._lock.AcquireWrite();
			try
			{
				base.BaseRemove(name);
			}
			finally
			{
				this._lock.ReleaseWrite();
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000C978 File Offset: 0x0000B978
		public void RemoveAt(int index)
		{
			this._lock.AcquireWrite();
			try
			{
				base.BaseRemoveAt(index);
			}
			finally
			{
				this._lock.ReleaseWrite();
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000C9B8 File Offset: 0x0000B9B8
		public void Clear()
		{
			this._lock.AcquireWrite();
			try
			{
				base.BaseClear();
			}
			finally
			{
				this._lock.ReleaseWrite();
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000C9F4 File Offset: 0x0000B9F4
		public void RemoveAll()
		{
			this.Clear();
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000C9FC File Offset: 0x0000B9FC
		public object Get(string name)
		{
			object obj = null;
			this._lock.AcquireRead();
			try
			{
				obj = base.BaseGet(name);
			}
			finally
			{
				this._lock.ReleaseRead();
			}
			return obj;
		}

		// Token: 0x170000B4 RID: 180
		public object this[string name]
		{
			get
			{
				return this.Get(name);
			}
			set
			{
				this.Set(name, value);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000CA54 File Offset: 0x0000BA54
		public object Get(int index)
		{
			object obj = null;
			this._lock.AcquireRead();
			try
			{
				obj = base.BaseGet(index);
			}
			finally
			{
				this._lock.ReleaseRead();
			}
			return obj;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000CA98 File Offset: 0x0000BA98
		public string GetKey(int index)
		{
			string text = null;
			this._lock.AcquireRead();
			try
			{
				text = base.BaseGetKey(index);
			}
			finally
			{
				this._lock.ReleaseRead();
			}
			return text;
		}

		// Token: 0x170000B5 RID: 181
		public object this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000CAE8 File Offset: 0x0000BAE8
		public string[] AllKeys
		{
			get
			{
				string[] array = null;
				this._lock.AcquireRead();
				try
				{
					array = base.BaseGetAllKeys();
				}
				finally
				{
					this._lock.ReleaseRead();
				}
				return array;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000CB28 File Offset: 0x0000BB28
		public HttpApplicationState Contents
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000CB2B File Offset: 0x0000BB2B
		public HttpStaticObjectsCollection StaticObjects
		{
			get
			{
				return this._applicationStaticObjects;
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000CB33 File Offset: 0x0000BB33
		public void Lock()
		{
			this._lock.AcquireWrite();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000CB40 File Offset: 0x0000BB40
		public void UnLock()
		{
			this._lock.ReleaseWrite();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000CB4D File Offset: 0x0000BB4D
		internal void EnsureUnLock()
		{
			this._lock.EnsureReleaseWrite();
		}

		// Token: 0x04000E66 RID: 3686
		private HttpApplicationStateLock _lock = new HttpApplicationStateLock();

		// Token: 0x04000E67 RID: 3687
		private HttpStaticObjectsCollection _applicationStaticObjects;

		// Token: 0x04000E68 RID: 3688
		private HttpStaticObjectsCollection _sessionStaticObjects;
	}
}
