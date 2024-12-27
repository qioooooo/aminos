using System;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x02000668 RID: 1640
	[Serializable]
	internal sealed class PermissionSetTriple
	{
		// Token: 0x06003BAE RID: 15278 RVA: 0x000CCB9C File Offset: 0x000CBB9C
		internal PermissionSetTriple()
		{
			this.Reset();
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x000CCBAA File Offset: 0x000CBBAA
		internal PermissionSetTriple(PermissionSetTriple triple)
		{
			this.AssertSet = triple.AssertSet;
			this.GrantSet = triple.GrantSet;
			this.RefusedSet = triple.RefusedSet;
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x000CCBD6 File Offset: 0x000CBBD6
		internal void Reset()
		{
			this.AssertSet = null;
			this.GrantSet = null;
			this.RefusedSet = null;
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x000CCBED File Offset: 0x000CBBED
		internal bool IsEmpty()
		{
			return this.AssertSet == null && this.GrantSet == null && this.RefusedSet == null;
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06003BB2 RID: 15282 RVA: 0x000CCC0A File Offset: 0x000CBC0A
		private PermissionToken ZoneToken
		{
			get
			{
				if (PermissionSetTriple.s_zoneToken == null)
				{
					PermissionSetTriple.s_zoneToken = PermissionToken.GetToken(typeof(ZoneIdentityPermission));
				}
				return PermissionSetTriple.s_zoneToken;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06003BB3 RID: 15283 RVA: 0x000CCC2C File Offset: 0x000CBC2C
		private PermissionToken UrlToken
		{
			get
			{
				if (PermissionSetTriple.s_urlToken == null)
				{
					PermissionSetTriple.s_urlToken = PermissionToken.GetToken(typeof(UrlIdentityPermission));
				}
				return PermissionSetTriple.s_urlToken;
			}
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x000CCC50 File Offset: 0x000CBC50
		internal bool Update(PermissionSetTriple psTriple, out PermissionSetTriple retTriple)
		{
			retTriple = null;
			retTriple = this.UpdateAssert(psTriple.AssertSet);
			if (psTriple.AssertSet != null && psTriple.AssertSet.IsUnrestricted())
			{
				return true;
			}
			this.UpdateGrant(psTriple.GrantSet);
			this.UpdateRefused(psTriple.RefusedSet);
			return false;
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x000CCCA0 File Offset: 0x000CBCA0
		internal PermissionSetTriple UpdateAssert(PermissionSet in_a)
		{
			PermissionSetTriple permissionSetTriple = null;
			if (in_a != null)
			{
				if (in_a.IsSubsetOf(this.AssertSet))
				{
					return null;
				}
				PermissionSet permissionSet;
				if (this.GrantSet != null)
				{
					permissionSet = in_a.Intersect(this.GrantSet);
				}
				else
				{
					this.GrantSet = new PermissionSet(true);
					permissionSet = in_a.Copy();
				}
				bool flag = false;
				if (this.RefusedSet != null)
				{
					permissionSet = PermissionSet.RemoveRefusedPermissionSet(permissionSet, this.RefusedSet, out flag);
				}
				if (!flag)
				{
					flag = PermissionSet.IsIntersectingAssertedPermissions(permissionSet, this.AssertSet);
				}
				if (flag)
				{
					permissionSetTriple = new PermissionSetTriple(this);
					this.Reset();
					this.GrantSet = permissionSetTriple.GrantSet.Copy();
				}
				if (this.AssertSet == null)
				{
					this.AssertSet = permissionSet;
				}
				else
				{
					this.AssertSet.InplaceUnion(permissionSet);
				}
			}
			return permissionSetTriple;
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x000CCD58 File Offset: 0x000CBD58
		internal void UpdateGrant(PermissionSet in_g, out ZoneIdentityPermission z, out UrlIdentityPermission u)
		{
			z = null;
			u = null;
			if (in_g != null)
			{
				if (this.GrantSet == null)
				{
					this.GrantSet = in_g.Copy();
				}
				else
				{
					this.GrantSet.InplaceIntersect(in_g);
				}
				z = (ZoneIdentityPermission)in_g.GetPermission(this.ZoneToken);
				u = (UrlIdentityPermission)in_g.GetPermission(this.UrlToken);
			}
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x000CCDB6 File Offset: 0x000CBDB6
		internal void UpdateGrant(PermissionSet in_g)
		{
			if (in_g != null)
			{
				if (this.GrantSet == null)
				{
					this.GrantSet = in_g.Copy();
					return;
				}
				this.GrantSet.InplaceIntersect(in_g);
			}
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x000CCDDC File Offset: 0x000CBDDC
		internal void UpdateRefused(PermissionSet in_r)
		{
			if (in_r != null)
			{
				if (this.RefusedSet == null)
				{
					this.RefusedSet = in_r.Copy();
					return;
				}
				this.RefusedSet.InplaceUnion(in_r);
			}
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x000CCE04 File Offset: 0x000CBE04
		private static bool CheckAssert(PermissionSet pSet, CodeAccessPermission demand, PermissionToken permToken)
		{
			if (pSet != null)
			{
				pSet.CheckDecoded(demand, permToken);
				CodeAccessPermission codeAccessPermission = (CodeAccessPermission)pSet.GetPermission(demand);
				try
				{
					if ((pSet.IsUnrestricted() && demand.CanUnrestrictedOverride()) || demand.CheckAssert(codeAccessPermission))
					{
						return false;
					}
				}
				catch (ArgumentException)
				{
				}
				return true;
			}
			return true;
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x000CCE60 File Offset: 0x000CBE60
		private static bool CheckAssert(PermissionSet assertPset, PermissionSet demandSet, out PermissionSet newDemandSet)
		{
			newDemandSet = null;
			if (assertPset != null)
			{
				assertPset.CheckDecoded(demandSet);
				if (demandSet.CheckAssertion(assertPset))
				{
					return false;
				}
				PermissionSet.RemoveAssertedPermissionSet(demandSet, assertPset, out newDemandSet);
			}
			return true;
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x000CCE83 File Offset: 0x000CBE83
		internal bool CheckDemand(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			if (!PermissionSetTriple.CheckAssert(this.AssertSet, demand, permToken))
			{
				return false;
			}
			CodeAccessSecurityEngine.CheckHelper(this.GrantSet, this.RefusedSet, demand, permToken, rmh, null, SecurityAction.Demand, true);
			return true;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x000CCEAF File Offset: 0x000CBEAF
		internal bool CheckSetDemand(PermissionSet demandSet, out PermissionSet alteredDemandset, RuntimeMethodHandle rmh)
		{
			alteredDemandset = null;
			if (!PermissionSetTriple.CheckAssert(this.AssertSet, demandSet, out alteredDemandset))
			{
				return false;
			}
			if (alteredDemandset != null)
			{
				demandSet = alteredDemandset;
			}
			CodeAccessSecurityEngine.CheckSetHelper(this.GrantSet, this.RefusedSet, demandSet, rmh, null, SecurityAction.Demand, true);
			return true;
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x000CCEE5 File Offset: 0x000CBEE5
		internal bool CheckDemandNoThrow(CodeAccessPermission demand, PermissionToken permToken)
		{
			return CodeAccessSecurityEngine.CheckHelper(this.GrantSet, this.RefusedSet, demand, permToken, PermissionSetTriple.s_emptyRMH, null, SecurityAction.Demand, false);
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x000CCF02 File Offset: 0x000CBF02
		internal bool CheckSetDemandNoThrow(PermissionSet demandSet)
		{
			return CodeAccessSecurityEngine.CheckSetHelper(this.GrantSet, this.RefusedSet, demandSet, PermissionSetTriple.s_emptyRMH, null, SecurityAction.Demand, false);
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x000CCF20 File Offset: 0x000CBF20
		internal bool CheckFlags(ref int flags)
		{
			if (this.AssertSet != null)
			{
				int specialFlags = SecurityManager.GetSpecialFlags(this.AssertSet, null);
				if ((flags & specialFlags) != 0)
				{
					flags &= ~specialFlags;
				}
			}
			return (SecurityManager.GetSpecialFlags(this.GrantSet, this.RefusedSet) & flags) == flags;
		}

		// Token: 0x04001EAA RID: 7850
		private static RuntimeMethodHandle s_emptyRMH = new RuntimeMethodHandle(null);

		// Token: 0x04001EAB RID: 7851
		private static PermissionToken s_zoneToken;

		// Token: 0x04001EAC RID: 7852
		private static PermissionToken s_urlToken;

		// Token: 0x04001EAD RID: 7853
		internal PermissionSet AssertSet;

		// Token: 0x04001EAE RID: 7854
		internal PermissionSet GrantSet;

		// Token: 0x04001EAF RID: 7855
		internal PermissionSet RefusedSet;
	}
}
