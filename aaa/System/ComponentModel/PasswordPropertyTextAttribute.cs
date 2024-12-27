using System;

namespace System.ComponentModel
{
	// Token: 0x02000125 RID: 293
	[AttributeUsage(AttributeTargets.All)]
	public sealed class PasswordPropertyTextAttribute : Attribute
	{
		// Token: 0x0600095C RID: 2396 RVA: 0x0001F676 File Offset: 0x0001E676
		public PasswordPropertyTextAttribute()
			: this(false)
		{
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001F67F File Offset: 0x0001E67F
		public PasswordPropertyTextAttribute(bool password)
		{
			this._password = password;
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0001F68E File Offset: 0x0001E68E
		public bool Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001F696 File Offset: 0x0001E696
		public override bool Equals(object o)
		{
			return o is PasswordPropertyTextAttribute && ((PasswordPropertyTextAttribute)o).Password == this._password;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001F6B5 File Offset: 0x0001E6B5
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001F6BD File Offset: 0x0001E6BD
		public override bool IsDefaultAttribute()
		{
			return this.Equals(PasswordPropertyTextAttribute.Default);
		}

		// Token: 0x04000A08 RID: 2568
		public static readonly PasswordPropertyTextAttribute Yes = new PasswordPropertyTextAttribute(true);

		// Token: 0x04000A09 RID: 2569
		public static readonly PasswordPropertyTextAttribute No = new PasswordPropertyTextAttribute(false);

		// Token: 0x04000A0A RID: 2570
		public static readonly PasswordPropertyTextAttribute Default = PasswordPropertyTextAttribute.No;

		// Token: 0x04000A0B RID: 2571
		private bool _password;
	}
}
