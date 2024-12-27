using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003C0 RID: 960
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ControlBuilderAttribute : Attribute
	{
		// Token: 0x06002F0C RID: 12044 RVA: 0x000D23A5 File Offset: 0x000D13A5
		public ControlBuilderAttribute(Type builderType)
		{
			this.builderType = builderType;
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002F0D RID: 12045 RVA: 0x000D23B4 File Offset: 0x000D13B4
		public Type BuilderType
		{
			get
			{
				return this.builderType;
			}
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000D23BC File Offset: 0x000D13BC
		public override int GetHashCode()
		{
			if (this.BuilderType == null)
			{
				return 0;
			}
			return this.BuilderType.GetHashCode();
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000D23D3 File Offset: 0x000D13D3
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is ControlBuilderAttribute && ((ControlBuilderAttribute)obj).BuilderType == this.builderType);
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000D23FB File Offset: 0x000D13FB
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ControlBuilderAttribute.Default);
		}

		// Token: 0x040021C0 RID: 8640
		public static readonly ControlBuilderAttribute Default = new ControlBuilderAttribute(null);

		// Token: 0x040021C1 RID: 8641
		private Type builderType;
	}
}
