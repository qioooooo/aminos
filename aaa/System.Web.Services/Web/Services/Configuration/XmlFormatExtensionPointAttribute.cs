using System;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000145 RID: 325
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class XmlFormatExtensionPointAttribute : Attribute
	{
		// Token: 0x06000A34 RID: 2612 RVA: 0x00047CE3 File Offset: 0x00046CE3
		public XmlFormatExtensionPointAttribute(string memberName)
		{
			this.name = memberName;
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00047CF9 File Offset: 0x00046CF9
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x00047D0F File Offset: 0x00046D0F
		public string MemberName
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

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00047D18 File Offset: 0x00046D18
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x00047D20 File Offset: 0x00046D20
		public bool AllowElements
		{
			get
			{
				return this.allowElements;
			}
			set
			{
				this.allowElements = value;
			}
		}

		// Token: 0x04000647 RID: 1607
		private string name;

		// Token: 0x04000648 RID: 1608
		private bool allowElements = true;
	}
}
