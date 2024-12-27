using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200006F RID: 111
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public sealed class SoapHeaderAttribute : Attribute
	{
		// Token: 0x06000305 RID: 773 RVA: 0x0000E3B5 File Offset: 0x0000D3B5
		public SoapHeaderAttribute(string memberName)
		{
			this.memberName = memberName;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000E3D2 File Offset: 0x0000D3D2
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000E3E8 File Offset: 0x0000D3E8
		public string MemberName
		{
			get
			{
				if (this.memberName != null)
				{
					return this.memberName;
				}
				return string.Empty;
			}
			set
			{
				this.memberName = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000E3F1 File Offset: 0x0000D3F1
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000E3F9 File Offset: 0x0000D3F9
		public SoapHeaderDirection Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000E402 File Offset: 0x0000D402
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0000E40A File Offset: 0x0000D40A
		[Obsolete("This property will be removed from a future version. The presence of a particular header in a SOAP message is no longer enforced", false)]
		public bool Required
		{
			get
			{
				return this.required;
			}
			set
			{
				this.required = value;
			}
		}

		// Token: 0x04000330 RID: 816
		private string memberName;

		// Token: 0x04000331 RID: 817
		private SoapHeaderDirection direction = SoapHeaderDirection.In;

		// Token: 0x04000332 RID: 818
		private bool required = true;
	}
}
