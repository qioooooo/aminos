using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003F4 RID: 1012
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FileLevelControlBuilderAttribute : Attribute
	{
		// Token: 0x06003205 RID: 12805 RVA: 0x000DC0B9 File Offset: 0x000DB0B9
		public FileLevelControlBuilderAttribute(Type builderType)
		{
			this.builderType = builderType;
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x000DC0C8 File Offset: 0x000DB0C8
		public Type BuilderType
		{
			get
			{
				return this.builderType;
			}
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x000DC0D0 File Offset: 0x000DB0D0
		public override int GetHashCode()
		{
			return this.builderType.GetHashCode();
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000DC0DD File Offset: 0x000DB0DD
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is FileLevelControlBuilderAttribute && ((FileLevelControlBuilderAttribute)obj).BuilderType == this.builderType);
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000DC105 File Offset: 0x000DB105
		public override bool IsDefaultAttribute()
		{
			return this.Equals(FileLevelControlBuilderAttribute.Default);
		}

		// Token: 0x040022EE RID: 8942
		public static readonly FileLevelControlBuilderAttribute Default = new FileLevelControlBuilderAttribute(null);

		// Token: 0x040022EF RID: 8943
		private Type builderType;
	}
}
