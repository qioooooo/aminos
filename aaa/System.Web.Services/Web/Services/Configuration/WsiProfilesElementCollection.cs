using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000143 RID: 323
	[ConfigurationCollection(typeof(WsiProfilesElement))]
	public sealed class WsiProfilesElementCollection : ConfigurationElementCollection
	{
		// Token: 0x06000A18 RID: 2584 RVA: 0x000479A5 File Offset: 0x000469A5
		public void Add(WsiProfilesElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.BaseAdd(element);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x000479BC File Offset: 0x000469BC
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x000479C4 File Offset: 0x000469C4
		public bool ContainsKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return base.BaseGet(key) != null;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x000479E1 File Offset: 0x000469E1
		protected override ConfigurationElement CreateNewElement()
		{
			return new WsiProfilesElement();
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x000479E8 File Offset: 0x000469E8
		public void CopyTo(WsiProfilesElement[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00047A00 File Offset: 0x00046A00
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			WsiProfilesElement wsiProfilesElement = (WsiProfilesElement)element;
			return wsiProfilesElement.Name.ToString();
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00047A32 File Offset: 0x00046A32
		public int IndexOf(WsiProfilesElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return base.BaseIndexOf(element);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00047A49 File Offset: 0x00046A49
		public void Remove(WsiProfilesElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00047A66 File Offset: 0x00046A66
		public void RemoveAt(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			base.BaseRemove(key);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00047A7D File Offset: 0x00046A7D
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00047A88 File Offset: 0x00046A88
		internal void SetDefaults()
		{
			WsiProfilesElement wsiProfilesElement = new WsiProfilesElement(WsiProfiles.BasicProfile1_1);
			this.Add(wsiProfilesElement);
		}

		// Token: 0x170002A8 RID: 680
		public WsiProfilesElement this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				WsiProfilesElement wsiProfilesElement = (WsiProfilesElement)base.BaseGet(key);
				if (wsiProfilesElement == null)
				{
					throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Res.GetString("ConfigKeyNotFoundInElementCollection"), new object[] { key.ToString() }));
				}
				return wsiProfilesElement;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (this.GetElementKey(value).Equals(key))
				{
					if (base.BaseGet(key) != null)
					{
						base.BaseRemove(key);
					}
					this.Add(value);
					return;
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Res.GetString("ConfigKeysDoNotMatch"), new object[]
				{
					this.GetElementKey(value).ToString(),
					key.ToString()
				}));
			}
		}

		// Token: 0x170002A9 RID: 681
		public WsiProfilesElement this[int index]
		{
			get
			{
				return (WsiProfilesElement)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}
	}
}
