using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200006D RID: 109
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class SoapHeaderMapping
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x0000D9BD File Offset: 0x0000C9BD
		internal SoapHeaderMapping()
		{
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000D9C5 File Offset: 0x0000C9C5
		public Type HeaderType
		{
			get
			{
				return this.headerType;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000D9CD File Offset: 0x0000C9CD
		public bool Repeats
		{
			get
			{
				return this.repeats;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000D9D5 File Offset: 0x0000C9D5
		public bool Custom
		{
			get
			{
				return this.custom;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000D9DD File Offset: 0x0000C9DD
		public SoapHeaderDirection Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000D9E5 File Offset: 0x0000C9E5
		public MemberInfo MemberInfo
		{
			get
			{
				return this.memberInfo;
			}
		}

		// Token: 0x04000327 RID: 807
		internal Type headerType;

		// Token: 0x04000328 RID: 808
		internal bool repeats;

		// Token: 0x04000329 RID: 809
		internal bool custom;

		// Token: 0x0400032A RID: 810
		internal SoapHeaderDirection direction;

		// Token: 0x0400032B RID: 811
		internal MemberInfo memberInfo;
	}
}
