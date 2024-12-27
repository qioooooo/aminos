using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000E8 RID: 232
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class OperationBinding : NamedItem
	{
		// Token: 0x06000632 RID: 1586 RVA: 0x0001D38B File Offset: 0x0001C38B
		internal void SetParent(Binding parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x0001D394 File Offset: 0x0001C394
		public Binding Binding
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001D39C File Offset: 0x0001C39C
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0001D3B8 File Offset: 0x0001C3B8
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x0001D3C0 File Offset: 0x0001C3C0
		[XmlElement("input")]
		public InputBinding Input
		{
			get
			{
				return this.input;
			}
			set
			{
				if (this.input != null)
				{
					this.input.SetParent(null);
				}
				this.input = value;
				if (this.input != null)
				{
					this.input.SetParent(this);
				}
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0001D3F1 File Offset: 0x0001C3F1
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x0001D3F9 File Offset: 0x0001C3F9
		[XmlElement("output")]
		public OutputBinding Output
		{
			get
			{
				return this.output;
			}
			set
			{
				if (this.output != null)
				{
					this.output.SetParent(null);
				}
				this.output = value;
				if (this.output != null)
				{
					this.output.SetParent(this);
				}
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001D42A File Offset: 0x0001C42A
		[XmlElement("fault")]
		public FaultBindingCollection Faults
		{
			get
			{
				if (this.faults == null)
				{
					this.faults = new FaultBindingCollection(this);
				}
				return this.faults;
			}
		}

		// Token: 0x0400045F RID: 1119
		private ServiceDescriptionFormatExtensionCollection extensions;

		// Token: 0x04000460 RID: 1120
		private FaultBindingCollection faults;

		// Token: 0x04000461 RID: 1121
		private InputBinding input;

		// Token: 0x04000462 RID: 1122
		private OutputBinding output;

		// Token: 0x04000463 RID: 1123
		private Binding parent;
	}
}
