using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200040B RID: 1035
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class IDReferencePropertyAttribute : Attribute
	{
		// Token: 0x060032A0 RID: 12960 RVA: 0x000DD5E6 File Offset: 0x000DC5E6
		public IDReferencePropertyAttribute()
			: this(typeof(Control))
		{
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x000DD5F8 File Offset: 0x000DC5F8
		public IDReferencePropertyAttribute(Type referencedControlType)
		{
			this._referencedControlType = referencedControlType;
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x000DD607 File Offset: 0x000DC607
		public Type ReferencedControlType
		{
			get
			{
				return this._referencedControlType;
			}
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x000DD60F File Offset: 0x000DC60F
		public override int GetHashCode()
		{
			if (this.ReferencedControlType == null)
			{
				return 0;
			}
			return this.ReferencedControlType.GetHashCode();
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x000DD628 File Offset: 0x000DC628
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			IDReferencePropertyAttribute idreferencePropertyAttribute = obj as IDReferencePropertyAttribute;
			return idreferencePropertyAttribute != null && this.ReferencedControlType == idreferencePropertyAttribute.ReferencedControlType;
		}

		// Token: 0x040023CE RID: 9166
		private Type _referencedControlType;
	}
}
