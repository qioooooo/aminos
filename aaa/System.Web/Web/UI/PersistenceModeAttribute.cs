using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000459 RID: 1113
	[AttributeUsage(AttributeTargets.All)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PersistenceModeAttribute : Attribute
	{
		// Token: 0x060034CA RID: 13514 RVA: 0x000E4B26 File Offset: 0x000E3B26
		public PersistenceModeAttribute(PersistenceMode mode)
		{
			if (mode < PersistenceMode.Attribute || mode > PersistenceMode.EncodedInnerDefaultProperty)
			{
				throw new ArgumentOutOfRangeException("mode");
			}
			this.mode = mode;
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x060034CB RID: 13515 RVA: 0x000E4B48 File Offset: 0x000E3B48
		public PersistenceMode Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000E4B50 File Offset: 0x000E3B50
		public override int GetHashCode()
		{
			return this.Mode.GetHashCode();
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000E4B62 File Offset: 0x000E3B62
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is PersistenceModeAttribute && ((PersistenceModeAttribute)obj).Mode == this.mode);
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000E4B8A File Offset: 0x000E3B8A
		public override bool IsDefaultAttribute()
		{
			return this.Equals(PersistenceModeAttribute.Default);
		}

		// Token: 0x040024F6 RID: 9462
		public static readonly PersistenceModeAttribute Attribute = new PersistenceModeAttribute(PersistenceMode.Attribute);

		// Token: 0x040024F7 RID: 9463
		public static readonly PersistenceModeAttribute InnerProperty = new PersistenceModeAttribute(PersistenceMode.InnerProperty);

		// Token: 0x040024F8 RID: 9464
		public static readonly PersistenceModeAttribute InnerDefaultProperty = new PersistenceModeAttribute(PersistenceMode.InnerDefaultProperty);

		// Token: 0x040024F9 RID: 9465
		public static readonly PersistenceModeAttribute EncodedInnerDefaultProperty = new PersistenceModeAttribute(PersistenceMode.EncodedInnerDefaultProperty);

		// Token: 0x040024FA RID: 9466
		public static readonly PersistenceModeAttribute Default = PersistenceModeAttribute.Attribute;

		// Token: 0x040024FB RID: 9467
		private PersistenceMode mode;
	}
}
