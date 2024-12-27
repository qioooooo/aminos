using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace System.Web.Services.Configuration
{
	// Token: 0x0200013C RID: 316
	[ConfigurationCollection(typeof(TypeElement))]
	public sealed class TypeElementCollection : ConfigurationElementCollection
	{
		// Token: 0x060009B9 RID: 2489 RVA: 0x00046008 File Offset: 0x00045008
		public void Add(TypeElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.BaseAdd(element);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0004601F File Offset: 0x0004501F
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00046027 File Offset: 0x00045027
		public bool ContainsKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return base.BaseGet(key) != null;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00046044 File Offset: 0x00045044
		protected override ConfigurationElement CreateNewElement()
		{
			return new TypeElement();
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0004604B File Offset: 0x0004504B
		public void CopyTo(TypeElement[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x00046064 File Offset: 0x00045064
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			TypeElement typeElement = (TypeElement)element;
			return typeElement.Type;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0004608C File Offset: 0x0004508C
		public int IndexOf(TypeElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return base.BaseIndexOf(element);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000460A3 File Offset: 0x000450A3
		public void Remove(TypeElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000460C0 File Offset: 0x000450C0
		public void RemoveAt(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			base.BaseRemove(key);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x000460D7 File Offset: 0x000450D7
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x17000280 RID: 640
		public TypeElement this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				TypeElement typeElement = (TypeElement)base.BaseGet(key);
				if (typeElement == null)
				{
					throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Res.GetString("ConfigKeyNotFoundInElementCollection"), new object[] { key.ToString() }));
				}
				return typeElement;
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

		// Token: 0x17000281 RID: 641
		public TypeElement this[int index]
		{
			get
			{
				return (TypeElement)base.BaseGet(index);
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
