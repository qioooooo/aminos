using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008E5 RID: 2277
	public abstract class KnownAce : GenericAce
	{
		// Token: 0x060052F4 RID: 21236 RVA: 0x0012D074 File Offset: 0x0012C074
		internal KnownAce(AceType type, AceFlags flags, int accessMask, SecurityIdentifier securityIdentifier)
			: base(type, flags)
		{
			if (securityIdentifier == null)
			{
				throw new ArgumentNullException("securityIdentifier");
			}
			this.AccessMask = accessMask;
			this.SecurityIdentifier = securityIdentifier;
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x060052F5 RID: 21237 RVA: 0x0012D0A2 File Offset: 0x0012C0A2
		// (set) Token: 0x060052F6 RID: 21238 RVA: 0x0012D0AA File Offset: 0x0012C0AA
		public int AccessMask
		{
			get
			{
				return this._accessMask;
			}
			set
			{
				this._accessMask = value;
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x060052F7 RID: 21239 RVA: 0x0012D0B3 File Offset: 0x0012C0B3
		// (set) Token: 0x060052F8 RID: 21240 RVA: 0x0012D0BB File Offset: 0x0012C0BB
		public SecurityIdentifier SecurityIdentifier
		{
			get
			{
				return this._sid;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._sid = value;
			}
		}

		// Token: 0x04002AE4 RID: 10980
		internal const int AccessMaskLength = 4;

		// Token: 0x04002AE5 RID: 10981
		private int _accessMask;

		// Token: 0x04002AE6 RID: 10982
		private SecurityIdentifier _sid;
	}
}
