using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000695 RID: 1685
	internal sealed class XmlHierarchyData : IHierarchyData, ICustomTypeDescriptor
	{
		// Token: 0x06005283 RID: 21123 RVA: 0x0014D204 File Offset: 0x0014C204
		internal XmlHierarchyData(XmlHierarchicalEnumerable parent, XmlNode item)
		{
			this._parent = parent;
			this._item = item;
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x0014D21A File Offset: 0x0014C21A
		private string CreateRecursivePath(XmlNode node)
		{
			if (node.ParentNode == null)
			{
				return string.Empty;
			}
			return this.CreateRecursivePath(node.ParentNode) + this.FindNodePosition(node);
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x0014D244 File Offset: 0x0014C244
		private string FindNodePosition(XmlNode node)
		{
			XmlNodeList childNodes = node.ParentNode.ChildNodes;
			int num = 0;
			for (int i = 0; i < childNodes.Count; i++)
			{
				if (childNodes[i].NodeType == XmlNodeType.Element)
				{
					num++;
				}
				if (childNodes[i] == node)
				{
					return "/*[position()=" + Convert.ToString(num, CultureInfo.InvariantCulture) + "]";
				}
			}
			throw new ArgumentException(SR.GetString("XmlHierarchyData_CouldNotFindNode"));
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x0014D2B7 File Offset: 0x0014C2B7
		public override string ToString()
		{
			return this._item.Name;
		}

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06005287 RID: 21127 RVA: 0x0014D2C4 File Offset: 0x0014C2C4
		bool IHierarchyData.HasChildren
		{
			get
			{
				return this._item.HasChildNodes;
			}
		}

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x06005288 RID: 21128 RVA: 0x0014D2D1 File Offset: 0x0014C2D1
		object IHierarchyData.Item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x06005289 RID: 21129 RVA: 0x0014D2DC File Offset: 0x0014C2DC
		string IHierarchyData.Path
		{
			get
			{
				if (this._path == null)
				{
					if (this._parent != null)
					{
						if (this._parent.Path == null)
						{
							this._parent.Path = this.CreateRecursivePath(this._item.ParentNode);
						}
						this._path = this._parent.Path + this.FindNodePosition(this._item);
					}
					else
					{
						this._path = this.CreateRecursivePath(this._item);
					}
				}
				return this._path;
			}
		}

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x0600528A RID: 21130 RVA: 0x0014D35E File Offset: 0x0014C35E
		string IHierarchyData.Type
		{
			get
			{
				return this._item.Name;
			}
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x0014D36B File Offset: 0x0014C36B
		IHierarchicalEnumerable IHierarchyData.GetChildren()
		{
			return new XmlHierarchicalEnumerable(this._item.ChildNodes);
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x0014D380 File Offset: 0x0014C380
		IHierarchyData IHierarchyData.GetParent()
		{
			XmlNode parentNode = this._item.ParentNode;
			if (parentNode == null)
			{
				return null;
			}
			return new XmlHierarchyData(null, parentNode);
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x0014D3A5 File Offset: 0x0014C3A5
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x0014D3AC File Offset: 0x0014C3AC
		string ICustomTypeDescriptor.GetClassName()
		{
			return base.GetType().Name;
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x0014D3B9 File Offset: 0x0014C3B9
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x0014D3BC File Offset: 0x0014C3BC
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x0014D3BF File Offset: 0x0014C3BF
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x0014D3C2 File Offset: 0x0014C3C2
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return new XmlHierarchyData.XmlHierarchyDataPropertyDescriptor("#Name");
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x0014D3CE File Offset: 0x0014C3CE
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x0014D3D1 File Offset: 0x0014C3D1
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return null;
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x0014D3D4 File Offset: 0x0014C3D4
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attrs)
		{
			return null;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x0014D3D7 File Offset: 0x0014C3D7
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x0014D3E0 File Offset: 0x0014C3E0
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attrFilter)
		{
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			list.Add(new XmlHierarchyData.XmlHierarchyDataPropertyDescriptor("#Name"));
			list.Add(new XmlHierarchyData.XmlHierarchyDataPropertyDescriptor("#Value"));
			list.Add(new XmlHierarchyData.XmlHierarchyDataPropertyDescriptor("#InnerText"));
			XmlAttributeCollection attributes = this._item.Attributes;
			if (attributes != null)
			{
				for (int i = 0; i < attributes.Count; i++)
				{
					list.Add(new XmlHierarchyData.XmlHierarchyDataPropertyDescriptor(attributes[i].Name));
				}
			}
			return new PropertyDescriptorCollection(list.ToArray());
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x0014D465 File Offset: 0x0014C465
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			if (pd is XmlHierarchyData.XmlHierarchyDataPropertyDescriptor)
			{
				return this;
			}
			return null;
		}

		// Token: 0x04002DFF RID: 11775
		private XmlNode _item;

		// Token: 0x04002E00 RID: 11776
		private XmlHierarchicalEnumerable _parent;

		// Token: 0x04002E01 RID: 11777
		private string _path;

		// Token: 0x02000696 RID: 1686
		private class XmlHierarchyDataPropertyDescriptor : PropertyDescriptor
		{
			// Token: 0x06005299 RID: 21145 RVA: 0x0014D472 File Offset: 0x0014C472
			public XmlHierarchyDataPropertyDescriptor(string name)
				: base(name, null)
			{
				this._name = name;
			}

			// Token: 0x17001503 RID: 5379
			// (get) Token: 0x0600529A RID: 21146 RVA: 0x0014D483 File Offset: 0x0014C483
			public override Type ComponentType
			{
				get
				{
					return typeof(XmlHierarchyData);
				}
			}

			// Token: 0x17001504 RID: 5380
			// (get) Token: 0x0600529B RID: 21147 RVA: 0x0014D48F File Offset: 0x0014C48F
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001505 RID: 5381
			// (get) Token: 0x0600529C RID: 21148 RVA: 0x0014D492 File Offset: 0x0014C492
			public override Type PropertyType
			{
				get
				{
					return typeof(string);
				}
			}

			// Token: 0x0600529D RID: 21149 RVA: 0x0014D49E File Offset: 0x0014C49E
			public override bool CanResetValue(object o)
			{
				return false;
			}

			// Token: 0x0600529E RID: 21150 RVA: 0x0014D4A4 File Offset: 0x0014C4A4
			public override object GetValue(object o)
			{
				XmlHierarchyData xmlHierarchyData = o as XmlHierarchyData;
				if (xmlHierarchyData != null)
				{
					string name;
					if ((name = this._name) != null)
					{
						if (name == "#Name")
						{
							return xmlHierarchyData._item.Name;
						}
						if (name == "#Value")
						{
							return xmlHierarchyData._item.Value;
						}
						if (name == "#InnerText")
						{
							return xmlHierarchyData._item.InnerText;
						}
					}
					XmlAttributeCollection attributes = xmlHierarchyData._item.Attributes;
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

			// Token: 0x0600529F RID: 21151 RVA: 0x0014D53D File Offset: 0x0014C53D
			public override void ResetValue(object o)
			{
			}

			// Token: 0x060052A0 RID: 21152 RVA: 0x0014D53F File Offset: 0x0014C53F
			public override void SetValue(object o, object value)
			{
			}

			// Token: 0x060052A1 RID: 21153 RVA: 0x0014D541 File Offset: 0x0014C541
			public override bool ShouldSerializeValue(object o)
			{
				return true;
			}

			// Token: 0x04002E02 RID: 11778
			private string _name;
		}
	}
}
