using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000F0 RID: 240
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class Message : NamedItem
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x0001D805 File Offset: 0x0001C805
		internal void SetParent(ServiceDescription parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001D80E File Offset: 0x0001C80E
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

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001D82A File Offset: 0x0001C82A
		public ServiceDescription ServiceDescription
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001D832 File Offset: 0x0001C832
		[XmlElement("part")]
		public MessagePartCollection Parts
		{
			get
			{
				if (this.parts == null)
				{
					this.parts = new MessagePartCollection(this);
				}
				return this.parts;
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001D850 File Offset: 0x0001C850
		public MessagePart[] FindPartsByName(string[] partNames)
		{
			MessagePart[] array = new MessagePart[partNames.Length];
			for (int i = 0; i < partNames.Length; i++)
			{
				array[i] = this.FindPartByName(partNames[i]);
			}
			return array;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001D884 File Offset: 0x0001C884
		public MessagePart FindPartByName(string partName)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				MessagePart messagePart = this.parts[i];
				if (messagePart.Name == partName)
				{
					return messagePart;
				}
			}
			throw new ArgumentException(Res.GetString("MissingMessagePartForMessageFromNamespace3", new object[]
			{
				partName,
				base.Name,
				this.ServiceDescription.TargetNamespace
			}), "partName");
		}

		// Token: 0x04000475 RID: 1141
		private MessagePartCollection parts;

		// Token: 0x04000476 RID: 1142
		private ServiceDescription parent;

		// Token: 0x04000477 RID: 1143
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
