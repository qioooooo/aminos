using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008FC RID: 2300
	public sealed class CryptoKeyAuditRule : AuditRule
	{
		// Token: 0x0600539E RID: 21406 RVA: 0x001301F3 File Offset: 0x0012F1F3
		public CryptoKeyAuditRule(IdentityReference identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags)
			: this(identity, CryptoKeyAuditRule.AccessMaskFromRights(cryptoKeyRights), false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x00130206 File Offset: 0x0012F206
		public CryptoKeyAuditRule(string identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags)
			: this(new NTAccount(identity), CryptoKeyAuditRule.AccessMaskFromRights(cryptoKeyRights), false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x0013021E File Offset: 0x0012F21E
		private CryptoKeyAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x060053A1 RID: 21409 RVA: 0x0013022F File Offset: 0x0012F22F
		public CryptoKeyRights CryptoKeyRights
		{
			get
			{
				return CryptoKeyAuditRule.RightsFromAccessMask(base.AccessMask);
			}
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x0013023C File Offset: 0x0012F23C
		private static int AccessMaskFromRights(CryptoKeyRights cryptoKeyRights)
		{
			return (int)cryptoKeyRights;
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x0013023F File Offset: 0x0012F23F
		internal static CryptoKeyRights RightsFromAccessMask(int accessMask)
		{
			return (CryptoKeyRights)accessMask;
		}
	}
}
