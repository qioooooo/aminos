using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000457 RID: 1111
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PersistChildrenAttribute : Attribute
	{
		// Token: 0x060034C2 RID: 13506 RVA: 0x000E4A79 File Offset: 0x000E3A79
		public PersistChildrenAttribute(bool persist)
		{
			this._persist = persist;
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000E4A88 File Offset: 0x000E3A88
		public PersistChildrenAttribute(bool persist, bool usesCustomPersistence)
			: this(persist)
		{
			this._usesCustomPersistence = usesCustomPersistence;
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x060034C4 RID: 13508 RVA: 0x000E4A98 File Offset: 0x000E3A98
		public bool Persist
		{
			get
			{
				return this._persist;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x060034C5 RID: 13509 RVA: 0x000E4AA0 File Offset: 0x000E3AA0
		public bool UsesCustomPersistence
		{
			get
			{
				return !this._persist && this._usesCustomPersistence;
			}
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000E4AB4 File Offset: 0x000E3AB4
		public override int GetHashCode()
		{
			return this.Persist.GetHashCode();
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000E4ACF File Offset: 0x000E3ACF
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is PersistChildrenAttribute && ((PersistChildrenAttribute)obj).Persist == this._persist);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000E4AF7 File Offset: 0x000E3AF7
		public override bool IsDefaultAttribute()
		{
			return this.Equals(PersistChildrenAttribute.Default);
		}

		// Token: 0x040024EC RID: 9452
		public static readonly PersistChildrenAttribute Yes = new PersistChildrenAttribute(true);

		// Token: 0x040024ED RID: 9453
		public static readonly PersistChildrenAttribute No = new PersistChildrenAttribute(false);

		// Token: 0x040024EE RID: 9454
		public static readonly PersistChildrenAttribute Default = PersistChildrenAttribute.Yes;

		// Token: 0x040024EF RID: 9455
		private bool _persist;

		// Token: 0x040024F0 RID: 9456
		private bool _usesCustomPersistence;
	}
}
