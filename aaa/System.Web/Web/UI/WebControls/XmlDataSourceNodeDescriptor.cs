using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200068F RID: 1679
	internal sealed class XmlDataSourceNodeDescriptor : ICustomTypeDescriptor, IXPathNavigable
	{
		// Token: 0x0600525C RID: 21084 RVA: 0x0014CBA9 File Offset: 0x0014BBA9
		public XmlDataSourceNodeDescriptor(XmlNode node)
		{
			this._node = node;
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x0014CBB8 File Offset: 0x0014BBB8
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x0014CBBF File Offset: 0x0014BBBF
		string ICustomTypeDescriptor.GetClassName()
		{
			return base.GetType().Name;
		}

		// Token: 0x0600525F RID: 21087 RVA: 0x0014CBCC File Offset: 0x0014BBCC
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x06005260 RID: 21088 RVA: 0x0014CBCF File Offset: 0x0014BBCF
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		// Token: 0x06005261 RID: 21089 RVA: 0x0014CBD2 File Offset: 0x0014BBD2
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x0014CBD5 File Offset: 0x0014BBD5
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x0014CBD8 File Offset: 0x0014BBD8
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x0014CBDB File Offset: 0x0014BBDB
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return null;
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x0014CBDE File Offset: 0x0014BBDE
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attrs)
		{
			return null;
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x0014CBE1 File Offset: 0x0014BBE1
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x0014CBEC File Offset: 0x0014BBEC
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attrFilter)
		{
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			XmlAttributeCollection attributes = this._node.Attributes;
			if (attributes != null)
			{
				for (int i = 0; i < attributes.Count; i++)
				{
					list.Add(new XmlDataSourceNodeDescriptor.XmlDataSourcePropertyDescriptor(attributes[i].Name));
				}
			}
			return new PropertyDescriptorCollection(list.ToArray());
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x0014CC41 File Offset: 0x0014BC41
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			if (pd is XmlDataSourceNodeDescriptor.XmlDataSourcePropertyDescriptor)
			{
				return this;
			}
			return null;
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x0014CC4E File Offset: 0x0014BC4E
		XPathNavigator IXPathNavigable.CreateNavigator()
		{
			return this._node.CreateNavigator();
		}

		// Token: 0x04002DF6 RID: 11766
		private XmlNode _node;

		// Token: 0x02000690 RID: 1680
		private class XmlDataSourcePropertyDescriptor : PropertyDescriptor
		{
			// Token: 0x0600526A RID: 21098 RVA: 0x0014CC5B File Offset: 0x0014BC5B
			public XmlDataSourcePropertyDescriptor(string name)
				: base(name, null)
			{
				this._name = name;
			}

			// Token: 0x170014F8 RID: 5368
			// (get) Token: 0x0600526B RID: 21099 RVA: 0x0014CC6C File Offset: 0x0014BC6C
			public override Type ComponentType
			{
				get
				{
					return typeof(XmlDataSourceNodeDescriptor);
				}
			}

			// Token: 0x170014F9 RID: 5369
			// (get) Token: 0x0600526C RID: 21100 RVA: 0x0014CC78 File Offset: 0x0014BC78
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170014FA RID: 5370
			// (get) Token: 0x0600526D RID: 21101 RVA: 0x0014CC7B File Offset: 0x0014BC7B
			public override Type PropertyType
			{
				get
				{
					return typeof(string);
				}
			}

			// Token: 0x0600526E RID: 21102 RVA: 0x0014CC87 File Offset: 0x0014BC87
			public override bool CanResetValue(object o)
			{
				return false;
			}

			// Token: 0x0600526F RID: 21103 RVA: 0x0014CC8C File Offset: 0x0014BC8C
			public override object GetValue(object o)
			{
				XmlDataSourceNodeDescriptor xmlDataSourceNodeDescriptor = o as XmlDataSourceNodeDescriptor;
				if (xmlDataSourceNodeDescriptor != null)
				{
					XmlAttributeCollection attributes = xmlDataSourceNodeDescriptor._node.Attributes;
					if (attributes != null)
					{
						XmlAttribute xmlAttribute = attributes[this._name];
						if (xmlAttribute != null)
						{
							return xmlAttribute.Value;
						}
					}
				}
				return string.Empty;
			}

			// Token: 0x06005270 RID: 21104 RVA: 0x0014CCCE File Offset: 0x0014BCCE
			public override void ResetValue(object o)
			{
			}

			// Token: 0x06005271 RID: 21105 RVA: 0x0014CCD0 File Offset: 0x0014BCD0
			public override void SetValue(object o, object value)
			{
			}

			// Token: 0x06005272 RID: 21106 RVA: 0x0014CCD2 File Offset: 0x0014BCD2
			public override bool ShouldSerializeValue(object o)
			{
				return true;
			}

			// Token: 0x04002DF7 RID: 11767
			private string _name;
		}
	}
}
