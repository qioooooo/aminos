using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000473 RID: 1139
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TemplateInstanceAttribute : Attribute
	{
		// Token: 0x060035B0 RID: 13744 RVA: 0x000E7E83 File Offset: 0x000E6E83
		public TemplateInstanceAttribute(TemplateInstance instances)
		{
			this._instances = instances;
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x060035B1 RID: 13745 RVA: 0x000E7E92 File Offset: 0x000E6E92
		public TemplateInstance Instances
		{
			get
			{
				return this._instances;
			}
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000E7E9C File Offset: 0x000E6E9C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			TemplateInstanceAttribute templateInstanceAttribute = obj as TemplateInstanceAttribute;
			return templateInstanceAttribute != null && templateInstanceAttribute.Instances == this.Instances;
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x000E7EC9 File Offset: 0x000E6EC9
		public override int GetHashCode()
		{
			return this._instances.GetHashCode();
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000E7EDB File Offset: 0x000E6EDB
		public override bool IsDefaultAttribute()
		{
			return this.Equals(TemplateInstanceAttribute.Default);
		}

		// Token: 0x04002548 RID: 9544
		public static readonly TemplateInstanceAttribute Multiple = new TemplateInstanceAttribute(TemplateInstance.Multiple);

		// Token: 0x04002549 RID: 9545
		public static readonly TemplateInstanceAttribute Single = new TemplateInstanceAttribute(TemplateInstance.Single);

		// Token: 0x0400254A RID: 9546
		public static readonly TemplateInstanceAttribute Default = TemplateInstanceAttribute.Multiple;

		// Token: 0x0400254B RID: 9547
		private TemplateInstance _instances;
	}
}
