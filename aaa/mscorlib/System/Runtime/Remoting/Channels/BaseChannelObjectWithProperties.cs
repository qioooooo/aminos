using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006DB RID: 1755
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public abstract class BaseChannelObjectWithProperties : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003F16 RID: 16150 RVA: 0x000D8325 File Offset: 0x000D7325
		public virtual IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return this;
			}
		}

		// Token: 0x17000AAF RID: 2735
		public virtual object this[object key]
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06003F19 RID: 16153 RVA: 0x000D8332 File Offset: 0x000D7332
		public virtual ICollection Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06003F1A RID: 16154 RVA: 0x000D8338 File Offset: 0x000D7338
		public virtual ICollection Values
		{
			get
			{
				ICollection keys = this.Keys;
				if (keys == null)
				{
					return null;
				}
				ArrayList arrayList = new ArrayList();
				foreach (object obj in keys)
				{
					arrayList.Add(this[obj]);
				}
				return arrayList;
			}
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x000D83A4 File Offset: 0x000D73A4
		public virtual bool Contains(object key)
		{
			if (key == null)
			{
				return false;
			}
			ICollection keys = this.Keys;
			if (keys == null)
			{
				return false;
			}
			string text = key as string;
			foreach (object obj in keys)
			{
				if (text != null)
				{
					string text2 = obj as string;
					if (text2 != null)
					{
						if (string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) == 0)
						{
							return true;
						}
						continue;
					}
				}
				if (key.Equals(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06003F1C RID: 16156 RVA: 0x000D843C File Offset: 0x000D743C
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06003F1D RID: 16157 RVA: 0x000D843F File Offset: 0x000D743F
		public virtual bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x000D8442 File Offset: 0x000D7442
		public virtual void Add(object key, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x000D8449 File Offset: 0x000D7449
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x000D8450 File Offset: 0x000D7450
		public virtual void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x000D8457 File Offset: 0x000D7457
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x000D845F File Offset: 0x000D745F
		public virtual void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x000D8468 File Offset: 0x000D7468
		public virtual int Count
		{
			get
			{
				ICollection keys = this.Keys;
				if (keys == null)
				{
					return 0;
				}
				return keys.Count;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06003F24 RID: 16164 RVA: 0x000D8487 File Offset: 0x000D7487
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06003F25 RID: 16165 RVA: 0x000D848A File Offset: 0x000D748A
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x000D848D File Offset: 0x000D748D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}
	}
}
