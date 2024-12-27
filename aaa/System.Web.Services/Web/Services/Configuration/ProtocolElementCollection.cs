using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000140 RID: 320
	[ConfigurationCollection(typeof(ProtocolElement))]
	public sealed class ProtocolElementCollection : ConfigurationElementCollection
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x000476AD File Offset: 0x000466AD
		public void Add(ProtocolElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.BaseAdd(element);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x000476C4 File Offset: 0x000466C4
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x000476CC File Offset: 0x000466CC
		public bool ContainsKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return base.BaseGet(key) != null;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x000476E9 File Offset: 0x000466E9
		protected override ConfigurationElement CreateNewElement()
		{
			return new ProtocolElement();
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x000476F0 File Offset: 0x000466F0
		public void CopyTo(ProtocolElement[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00047708 File Offset: 0x00046708
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ProtocolElement protocolElement = (ProtocolElement)element;
			return protocolElement.Name.ToString();
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0004773A File Offset: 0x0004673A
		public int IndexOf(ProtocolElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return base.BaseIndexOf(element);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00047751 File Offset: 0x00046751
		public void Remove(ProtocolElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0004776E File Offset: 0x0004676E
		public void RemoveAt(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			base.BaseRemove(key);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00047785 File Offset: 0x00046785
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00047790 File Offset: 0x00046790
		internal void SetDefaults()
		{
			ProtocolElement protocolElement = new ProtocolElement(WebServiceProtocols.HttpSoap12);
			ProtocolElement protocolElement2 = new ProtocolElement(WebServiceProtocols.HttpSoap);
			ProtocolElement protocolElement3 = new ProtocolElement(WebServiceProtocols.HttpPostLocalhost);
			ProtocolElement protocolElement4 = new ProtocolElement(WebServiceProtocols.Documentation);
			this.Add(protocolElement);
			this.Add(protocolElement2);
			this.Add(protocolElement3);
			this.Add(protocolElement4);
		}

		// Token: 0x170002A4 RID: 676
		public ProtocolElement this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				ProtocolElement protocolElement = (ProtocolElement)base.BaseGet(key);
				if (protocolElement == null)
				{
					throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Res.GetString("ConfigKeyNotFoundInElementCollection"), new object[] { key.ToString() }));
				}
				return protocolElement;
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

		// Token: 0x170002A5 RID: 677
		public ProtocolElement this[int index]
		{
			get
			{
				return (ProtocolElement)base.BaseGet(index);
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
