using System;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000144 RID: 324
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class XmlFormatExtensionAttribute : Attribute
	{
		// Token: 0x06000A28 RID: 2600 RVA: 0x00047BB5 File Offset: 0x00046BB5
		public XmlFormatExtensionAttribute()
		{
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00047BC0 File Offset: 0x00046BC0
		public XmlFormatExtensionAttribute(string elementName, string ns, Type extensionPoint1)
			: this(elementName, ns, new Type[] { extensionPoint1 })
		{
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00047BE4 File Offset: 0x00046BE4
		public XmlFormatExtensionAttribute(string elementName, string ns, Type extensionPoint1, Type extensionPoint2)
			: this(elementName, ns, new Type[] { extensionPoint1, extensionPoint2 })
		{
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00047C0C File Offset: 0x00046C0C
		public XmlFormatExtensionAttribute(string elementName, string ns, Type extensionPoint1, Type extensionPoint2, Type extensionPoint3)
			: this(elementName, ns, new Type[] { extensionPoint1, extensionPoint2, extensionPoint3 })
		{
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00047C38 File Offset: 0x00046C38
		public XmlFormatExtensionAttribute(string elementName, string ns, Type extensionPoint1, Type extensionPoint2, Type extensionPoint3, Type extensionPoint4)
			: this(elementName, ns, new Type[] { extensionPoint1, extensionPoint2, extensionPoint3, extensionPoint4 })
		{
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00047C68 File Offset: 0x00046C68
		public XmlFormatExtensionAttribute(string elementName, string ns, Type[] extensionPoints)
		{
			this.name = elementName;
			this.ns = ns;
			this.types = extensionPoints;
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x00047C85 File Offset: 0x00046C85
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x00047C9C File Offset: 0x00046C9C
		public Type[] ExtensionPoints
		{
			get
			{
				if (this.types != null)
				{
					return this.types;
				}
				return new Type[0];
			}
			set
			{
				this.types = value;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x00047CA5 File Offset: 0x00046CA5
		// (set) Token: 0x06000A31 RID: 2609 RVA: 0x00047CBB File Offset: 0x00046CBB
		public string Namespace
		{
			get
			{
				if (this.ns != null)
				{
					return this.ns;
				}
				return string.Empty;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00047CC4 File Offset: 0x00046CC4
		// (set) Token: 0x06000A33 RID: 2611 RVA: 0x00047CDA File Offset: 0x00046CDA
		public string ElementName
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x04000644 RID: 1604
		private Type[] types;

		// Token: 0x04000645 RID: 1605
		private string name;

		// Token: 0x04000646 RID: 1606
		private string ns;
	}
}
