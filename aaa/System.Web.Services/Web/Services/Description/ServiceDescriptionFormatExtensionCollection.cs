using System;
using System.Collections;
using System.Xml;

namespace System.Web.Services.Description
{
	// Token: 0x020000F4 RID: 244
	public sealed class ServiceDescriptionFormatExtensionCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x0600067C RID: 1660 RVA: 0x0001DDB5 File Offset: 0x0001CDB5
		public ServiceDescriptionFormatExtensionCollection(object parent)
			: base(parent)
		{
		}

		// Token: 0x170001EA RID: 490
		public object this[int index]
		{
			get
			{
				return base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001DDDB File Offset: 0x0001CDDB
		public int Add(object extension)
		{
			return base.List.Add(extension);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001DDE9 File Offset: 0x0001CDE9
		public void Insert(int index, object extension)
		{
			base.List.Insert(index, extension);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001DDF8 File Offset: 0x0001CDF8
		public int IndexOf(object extension)
		{
			return base.List.IndexOf(extension);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001DE06 File Offset: 0x0001CE06
		public bool Contains(object extension)
		{
			return base.List.Contains(extension);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001DE14 File Offset: 0x0001CE14
		public void Remove(object extension)
		{
			base.List.Remove(extension);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001DE22 File Offset: 0x0001CE22
		public void CopyTo(object[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001DE34 File Offset: 0x0001CE34
		public object Find(Type type)
		{
			for (int i = 0; i < base.List.Count; i++)
			{
				object obj = base.List[i];
				if (type.IsAssignableFrom(obj.GetType()))
				{
					((ServiceDescriptionFormatExtension)obj).Handled = true;
					return obj;
				}
			}
			return null;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001DE84 File Offset: 0x0001CE84
		public object[] FindAll(Type type)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < base.List.Count; i++)
			{
				object obj = base.List[i];
				if (type.IsAssignableFrom(obj.GetType()))
				{
					((ServiceDescriptionFormatExtension)obj).Handled = true;
					arrayList.Add(obj);
				}
			}
			return (object[])arrayList.ToArray(type);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001DEE8 File Offset: 0x0001CEE8
		public XmlElement Find(string name, string ns)
		{
			for (int i = 0; i < base.List.Count; i++)
			{
				XmlElement xmlElement = base.List[i] as XmlElement;
				if (xmlElement != null && xmlElement.LocalName == name && xmlElement.NamespaceURI == ns)
				{
					this.SetHandled(xmlElement);
					return xmlElement;
				}
			}
			return null;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001DF48 File Offset: 0x0001CF48
		public XmlElement[] FindAll(string name, string ns)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < base.List.Count; i++)
			{
				XmlElement xmlElement = base.List[i] as XmlElement;
				if (xmlElement != null && xmlElement.LocalName == name && xmlElement.NamespaceURI == ns)
				{
					this.SetHandled(xmlElement);
					arrayList.Add(xmlElement);
				}
			}
			return (XmlElement[])arrayList.ToArray(typeof(XmlElement));
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001DFC6 File Offset: 0x0001CFC6
		private void SetHandled(XmlElement element)
		{
			if (this.handledElements == null)
			{
				this.handledElements = new ArrayList();
			}
			if (!this.handledElements.Contains(element))
			{
				this.handledElements.Add(element);
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001DFF6 File Offset: 0x0001CFF6
		public bool IsHandled(object item)
		{
			if (item is XmlElement)
			{
				return this.IsHandled((XmlElement)item);
			}
			return ((ServiceDescriptionFormatExtension)item).Handled;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001E018 File Offset: 0x0001D018
		public bool IsRequired(object item)
		{
			if (item is XmlElement)
			{
				return this.IsRequired((XmlElement)item);
			}
			return ((ServiceDescriptionFormatExtension)item).Required;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001E03A File Offset: 0x0001D03A
		private bool IsHandled(XmlElement element)
		{
			return this.handledElements != null && this.handledElements.Contains(element);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001E054 File Offset: 0x0001D054
		private bool IsRequired(XmlElement element)
		{
			XmlAttribute xmlAttribute = element.Attributes["required", "http://schemas.xmlsoap.org/wsdl/"];
			if (xmlAttribute == null || xmlAttribute.Value == null)
			{
				xmlAttribute = element.Attributes["required"];
				if (xmlAttribute == null || xmlAttribute.Value == null)
				{
					return false;
				}
			}
			return XmlConvert.ToBoolean(xmlAttribute.Value);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001E0AB File Offset: 0x0001D0AB
		protected override void SetParent(object value, object parent)
		{
			if (value is ServiceDescriptionFormatExtension)
			{
				((ServiceDescriptionFormatExtension)value).SetParent(parent);
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001E0C1 File Offset: 0x0001D0C1
		protected override void OnValidate(object value)
		{
			if (!(value is XmlElement) && !(value is ServiceDescriptionFormatExtension))
			{
				throw new ArgumentException(Res.GetString("OnlyXmlElementsOrTypesDerivingFromServiceDescriptionFormatExtension0"), "value");
			}
			base.OnValidate(value);
		}

		// Token: 0x04000480 RID: 1152
		private ArrayList handledElements;
	}
}
