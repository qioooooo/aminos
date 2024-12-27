using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200053C RID: 1340
	internal class SpnDictionary : StringDictionary
	{
		// Token: 0x060028E5 RID: 10469 RVA: 0x000AA1B3 File Offset: 0x000A91B3
		internal SpnDictionary()
		{
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060028E6 RID: 10470 RVA: 0x000AA1CB File Offset: 0x000A91CB
		public override int Count
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable.Count;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060028E7 RID: 10471 RVA: 0x000AA1E2 File Offset: 0x000A91E2
		public override bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000AA1E8 File Offset: 0x000A91E8
		internal string InternalGet(string canonicalKey)
		{
			int num = 0;
			string text = null;
			lock (this.m_SyncTable.SyncRoot)
			{
				foreach (object obj in this.m_SyncTable.Keys)
				{
					string text2 = (string)obj;
					if (text2 != null && text2.Length > num && string.Compare(text2, 0, canonicalKey, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						num = text2.Length;
						text = text2;
					}
				}
			}
			if (text == null)
			{
				return null;
			}
			return (string)this.m_SyncTable[text];
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000AA2B4 File Offset: 0x000A92B4
		internal void InternalSet(string canonicalKey, string spn)
		{
			this.m_SyncTable[canonicalKey] = spn;
		}

		// Token: 0x1700085C RID: 2140
		public override string this[string key]
		{
			get
			{
				key = SpnDictionary.GetCanonicalKey(key);
				return this.InternalGet(key);
			}
			set
			{
				key = SpnDictionary.GetCanonicalKey(key);
				this.InternalSet(key, value);
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x000AA2E6 File Offset: 0x000A92E6
		public override ICollection Keys
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable.Keys;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060028ED RID: 10477 RVA: 0x000AA2FD File Offset: 0x000A92FD
		public override object SyncRoot
		{
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x000AA30F File Offset: 0x000A930F
		public override ICollection Values
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable.Values;
			}
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000AA326 File Offset: 0x000A9326
		public override void Add(string key, string value)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			this.m_SyncTable.Add(key, value);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000AA33D File Offset: 0x000A933D
		public override void Clear()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			this.m_SyncTable.Clear();
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000AA354 File Offset: 0x000A9354
		public override bool ContainsKey(string key)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			return this.m_SyncTable.ContainsKey(key);
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000AA36A File Offset: 0x000A936A
		public override bool ContainsValue(string value)
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			return this.m_SyncTable.ContainsValue(value);
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000AA382 File Offset: 0x000A9382
		public override void CopyTo(Array array, int index)
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			this.m_SyncTable.CopyTo(array, index);
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000AA39B File Offset: 0x000A939B
		public override IEnumerator GetEnumerator()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			return this.m_SyncTable.GetEnumerator();
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000AA3B2 File Offset: 0x000A93B2
		public override void Remove(string key)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			this.m_SyncTable.Remove(key);
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x000AA3C8 File Offset: 0x000A93C8
		private static string GetCanonicalKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			try
			{
				Uri uri = new Uri(key);
				key = uri.GetParts(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.SafeUnescaped);
				new WebPermission(NetworkAccess.Connect, new Uri(key)).Demand();
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(SR.GetString("net_mustbeuri", new object[] { "key" }), "key", ex);
			}
			return key;
		}

		// Token: 0x040027C9 RID: 10185
		private Hashtable m_SyncTable = Hashtable.Synchronized(new Hashtable());
	}
}
