using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000139 RID: 313
	[ConfigurationCollection(typeof(SoapExtensionTypeElement))]
	public sealed class SoapExtensionTypeElementCollection : ConfigurationElementCollection
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x00045D26 File Offset: 0x00044D26
		public void Add(SoapExtensionTypeElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.BaseAdd(element);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00045D3D File Offset: 0x00044D3D
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00045D45 File Offset: 0x00044D45
		public bool ContainsKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return base.BaseGet(key) != null;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00045D62 File Offset: 0x00044D62
		protected override ConfigurationElement CreateNewElement()
		{
			return new SoapExtensionTypeElement();
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00045D69 File Offset: 0x00044D69
		public void CopyTo(SoapExtensionTypeElement[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00045D81 File Offset: 0x00044D81
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00045D92 File Offset: 0x00044D92
		public int IndexOf(SoapExtensionTypeElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return base.BaseIndexOf(element);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00045DA9 File Offset: 0x00044DA9
		public void Remove(SoapExtensionTypeElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00045DC6 File Offset: 0x00044DC6
		public void RemoveAt(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			base.BaseRemove(key);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00045DDD File Offset: 0x00044DDD
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x1700027C RID: 636
		public SoapExtensionTypeElement this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				SoapExtensionTypeElement soapExtensionTypeElement = (SoapExtensionTypeElement)base.BaseGet(key);
				if (soapExtensionTypeElement == null)
				{
					throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Res.GetString("ConfigKeyNotFoundInElementCollection"), new object[] { key.ToString() }));
				}
				return soapExtensionTypeElement;
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

		// Token: 0x1700027D RID: 637
		public SoapExtensionTypeElement this[int index]
		{
			get
			{
				return (SoapExtensionTypeElement)base.BaseGet(index);
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
