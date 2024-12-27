using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200042F RID: 1071
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class NonVisualControlAttribute : Attribute
	{
		// Token: 0x0600335D RID: 13149 RVA: 0x000DEF1D File Offset: 0x000DDF1D
		public NonVisualControlAttribute()
			: this(true)
		{
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000DEF26 File Offset: 0x000DDF26
		public NonVisualControlAttribute(bool nonVisual)
		{
			this._nonVisual = nonVisual;
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x0600335F RID: 13151 RVA: 0x000DEF35 File Offset: 0x000DDF35
		public bool IsNonVisual
		{
			get
			{
				return this._nonVisual;
			}
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x000DEF40 File Offset: 0x000DDF40
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			NonVisualControlAttribute nonVisualControlAttribute = obj as NonVisualControlAttribute;
			return nonVisualControlAttribute != null && nonVisualControlAttribute.IsNonVisual == this.IsNonVisual;
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000DEF6D File Offset: 0x000DDF6D
		public override int GetHashCode()
		{
			return this._nonVisual.GetHashCode();
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000DEF7A File Offset: 0x000DDF7A
		public override bool IsDefaultAttribute()
		{
			return this.Equals(NonVisualControlAttribute.Default);
		}

		// Token: 0x040023F8 RID: 9208
		public static readonly NonVisualControlAttribute NonVisual = new NonVisualControlAttribute(true);

		// Token: 0x040023F9 RID: 9209
		public static readonly NonVisualControlAttribute Visual = new NonVisualControlAttribute(false);

		// Token: 0x040023FA RID: 9210
		public static readonly NonVisualControlAttribute Default = NonVisualControlAttribute.Visual;

		// Token: 0x040023FB RID: 9211
		private bool _nonVisual;
	}
}
