using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x02000929 RID: 2345
	[ComVisible(false)]
	public abstract class IdentityReference
	{
		// Token: 0x06005515 RID: 21781 RVA: 0x001354AC File Offset: 0x001344AC
		internal IdentityReference()
		{
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06005516 RID: 21782
		public abstract string Value { get; }

		// Token: 0x06005517 RID: 21783
		public abstract bool IsValidTargetType(Type targetType);

		// Token: 0x06005518 RID: 21784
		public abstract IdentityReference Translate(Type targetType);

		// Token: 0x06005519 RID: 21785
		public abstract override bool Equals(object o);

		// Token: 0x0600551A RID: 21786
		public abstract override int GetHashCode();

		// Token: 0x0600551B RID: 21787
		public abstract override string ToString();

		// Token: 0x0600551C RID: 21788 RVA: 0x001354B4 File Offset: 0x001344B4
		public static bool operator ==(IdentityReference left, IdentityReference right)
		{
			return (left == null && right == null) || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x0600551D RID: 21789 RVA: 0x001354DC File Offset: 0x001344DC
		public static bool operator !=(IdentityReference left, IdentityReference right)
		{
			return !(left == right);
		}
	}
}
