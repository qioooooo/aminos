using System;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000E9 RID: 233
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class Binding : NamedItem
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x0001D44E File Offset: 0x0001C44E
		internal void SetParent(ServiceDescription parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x0001D457 File Offset: 0x0001C457
		public ServiceDescription ServiceDescription
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0001D45F File Offset: 0x0001C45F
		[XmlIgnore]
		public override ServiceDescriptionFormatExtensionCollection Extensions
		{
			get
			{
				if (this.extensions == null)
				{
					this.extensions = new ServiceDescriptionFormatExtensionCollection(this);
				}
				return this.extensions;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x0001D47B File Offset: 0x0001C47B
		[XmlElement("operation")]
		public OperationBindingCollection Operations
		{
			get
			{
				if (this.operations == null)
				{
					this.operations = new OperationBindingCollection(this);
				}
				return this.operations;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001D497 File Offset: 0x0001C497
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x0001D4AD File Offset: 0x0001C4AD
		[XmlAttribute("type")]
		public XmlQualifiedName Type
		{
			get
			{
				if (this.type == null)
				{
					return XmlQualifiedName.Empty;
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x04000464 RID: 1124
		private ServiceDescriptionFormatExtensionCollection extensions;

		// Token: 0x04000465 RID: 1125
		private OperationBindingCollection operations;

		// Token: 0x04000466 RID: 1126
		private XmlQualifiedName type = XmlQualifiedName.Empty;

		// Token: 0x04000467 RID: 1127
		private ServiceDescription parent;
	}
}
