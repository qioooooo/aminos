using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Security
{
	// Token: 0x02000669 RID: 1641
	[Serializable]
	internal sealed class PermissionListSet
	{
		// Token: 0x06003BC1 RID: 15297 RVA: 0x000CCF75 File Offset: 0x000CBF75
		internal PermissionListSet()
		{
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x000CCF7D File Offset: 0x000CBF7D
		private void EnsureTriplesListCreated()
		{
			if (this.m_permSetTriples == null)
			{
				this.m_permSetTriples = new ArrayList();
				if (this.m_firstPermSetTriple != null)
				{
					this.m_permSetTriples.Add(this.m_firstPermSetTriple);
					this.m_firstPermSetTriple = null;
				}
			}
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x000CCFB3 File Offset: 0x000CBFB3
		internal void UpdateDomainPLS(PermissionListSet adPLS)
		{
			if (adPLS != null && adPLS.m_firstPermSetTriple != null)
			{
				this.UpdateDomainPLS(adPLS.m_firstPermSetTriple.GrantSet, adPLS.m_firstPermSetTriple.RefusedSet);
			}
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x000CCFDC File Offset: 0x000CBFDC
		internal void UpdateDomainPLS(PermissionSet grantSet, PermissionSet deniedSet)
		{
			if (this.m_firstPermSetTriple == null)
			{
				this.m_firstPermSetTriple = new PermissionSetTriple();
			}
			this.m_firstPermSetTriple.UpdateGrant(grantSet);
			this.m_firstPermSetTriple.UpdateRefused(deniedSet);
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x000CD009 File Offset: 0x000CC009
		private void Terminate(PermissionSetTriple currentTriple)
		{
			this.UpdateTripleListAndCreateNewTriple(currentTriple, null);
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x000CD013 File Offset: 0x000CC013
		private void Terminate(PermissionSetTriple currentTriple, PermissionListSet pls)
		{
			this.UpdateZoneAndOrigin(pls);
			this.UpdatePermissions(currentTriple, pls);
			this.UpdateTripleListAndCreateNewTriple(currentTriple, null);
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x000CD02D File Offset: 0x000CC02D
		private bool Update(PermissionSetTriple currentTriple, PermissionListSet pls)
		{
			this.UpdateZoneAndOrigin(pls);
			return this.UpdatePermissions(currentTriple, pls);
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x000CD040 File Offset: 0x000CC040
		private bool Update(PermissionSetTriple currentTriple, FrameSecurityDescriptor fsd)
		{
			FrameSecurityDescriptorWithResolver frameSecurityDescriptorWithResolver = fsd as FrameSecurityDescriptorWithResolver;
			if (frameSecurityDescriptorWithResolver != null)
			{
				return this.Update2(currentTriple, frameSecurityDescriptorWithResolver);
			}
			bool flag = this.Update2(currentTriple, fsd, false);
			if (!flag)
			{
				flag = this.Update2(currentTriple, fsd, true);
			}
			return flag;
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x000CD078 File Offset: 0x000CC078
		[SecurityCritical]
		private bool Update2(PermissionSetTriple currentTriple, FrameSecurityDescriptorWithResolver fsdWithResolver)
		{
			DynamicResolver resolver = fsdWithResolver.Resolver;
			CompressedStack securityContext = resolver.GetSecurityContext();
			securityContext.CompleteConstruction(null);
			return this.Update(currentTriple, securityContext.PLS);
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x000CD0A8 File Offset: 0x000CC0A8
		private bool Update2(PermissionSetTriple currentTriple, FrameSecurityDescriptor fsd, bool fDeclarative)
		{
			PermissionSet denials = fsd.GetDenials(fDeclarative);
			if (denials != null)
			{
				currentTriple.UpdateRefused(denials);
			}
			PermissionSet permitOnly = fsd.GetPermitOnly(fDeclarative);
			if (permitOnly != null)
			{
				currentTriple.UpdateGrant(permitOnly);
			}
			if (fsd.GetAssertAllPossible())
			{
				if (currentTriple.GrantSet == null)
				{
					currentTriple.GrantSet = PermissionSet.s_fullTrust;
				}
				this.UpdateTripleListAndCreateNewTriple(currentTriple, this.m_permSetTriples);
				currentTriple.GrantSet = PermissionSet.s_fullTrust;
				currentTriple.UpdateAssert(fsd.GetAssertions(fDeclarative));
				return true;
			}
			PermissionSet assertions = fsd.GetAssertions(fDeclarative);
			if (assertions != null)
			{
				if (assertions.IsUnrestricted())
				{
					if (currentTriple.GrantSet == null)
					{
						currentTriple.GrantSet = PermissionSet.s_fullTrust;
					}
					this.UpdateTripleListAndCreateNewTriple(currentTriple, this.m_permSetTriples);
					currentTriple.GrantSet = PermissionSet.s_fullTrust;
					currentTriple.UpdateAssert(assertions);
					return true;
				}
				PermissionSetTriple permissionSetTriple = currentTriple.UpdateAssert(assertions);
				if (permissionSetTriple != null)
				{
					this.EnsureTriplesListCreated();
					this.m_permSetTriples.Add(permissionSetTriple);
				}
			}
			return false;
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x000CD184 File Offset: 0x000CC184
		private void Update(PermissionSetTriple currentTriple, PermissionSet in_g, PermissionSet in_r)
		{
			ZoneIdentityPermission zoneIdentityPermission;
			UrlIdentityPermission urlIdentityPermission;
			currentTriple.UpdateGrant(in_g, out zoneIdentityPermission, out urlIdentityPermission);
			currentTriple.UpdateRefused(in_r);
			this.AppendZoneOrigin(zoneIdentityPermission, urlIdentityPermission);
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x000CD1AB File Offset: 0x000CC1AB
		private void Update(PermissionSet in_g)
		{
			if (this.m_firstPermSetTriple == null)
			{
				this.m_firstPermSetTriple = new PermissionSetTriple();
			}
			this.Update(this.m_firstPermSetTriple, in_g, null);
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x000CD1D0 File Offset: 0x000CC1D0
		private void UpdateZoneAndOrigin(PermissionListSet pls)
		{
			if (pls != null)
			{
				if (this.m_zoneList == null && pls.m_zoneList != null && pls.m_zoneList.Count > 0)
				{
					this.m_zoneList = new ArrayList();
				}
				PermissionListSet.UpdateArrayList(this.m_zoneList, pls.m_zoneList);
				if (this.m_originList == null && pls.m_originList != null && pls.m_originList.Count > 0)
				{
					this.m_originList = new ArrayList();
				}
				PermissionListSet.UpdateArrayList(this.m_originList, pls.m_originList);
			}
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x000CD254 File Offset: 0x000CC254
		private bool UpdatePermissions(PermissionSetTriple currentTriple, PermissionListSet pls)
		{
			if (pls != null)
			{
				if (pls.m_permSetTriples != null)
				{
					this.UpdateTripleListAndCreateNewTriple(currentTriple, pls.m_permSetTriples);
				}
				else
				{
					PermissionSetTriple firstPermSetTriple = pls.m_firstPermSetTriple;
					PermissionSetTriple permissionSetTriple;
					if (currentTriple.Update(firstPermSetTriple, out permissionSetTriple))
					{
						return true;
					}
					if (permissionSetTriple != null)
					{
						this.EnsureTriplesListCreated();
						this.m_permSetTriples.Add(permissionSetTriple);
					}
				}
			}
			else
			{
				this.UpdateTripleListAndCreateNewTriple(currentTriple, null);
			}
			return false;
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x000CD2B0 File Offset: 0x000CC2B0
		private void UpdateTripleListAndCreateNewTriple(PermissionSetTriple currentTriple, ArrayList tripleList)
		{
			if (!currentTriple.IsEmpty())
			{
				if (this.m_firstPermSetTriple == null && this.m_permSetTriples == null)
				{
					this.m_firstPermSetTriple = new PermissionSetTriple(currentTriple);
				}
				else
				{
					this.EnsureTriplesListCreated();
					this.m_permSetTriples.Add(new PermissionSetTriple(currentTriple));
				}
				currentTriple.Reset();
			}
			if (tripleList != null)
			{
				this.EnsureTriplesListCreated();
				this.m_permSetTriples.AddRange(tripleList);
			}
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x000CD318 File Offset: 0x000CC318
		private static void UpdateArrayList(ArrayList current, ArrayList newList)
		{
			if (newList == null)
			{
				return;
			}
			for (int i = 0; i < newList.Count; i++)
			{
				if (!current.Contains(newList[i]))
				{
					current.Add(newList[i]);
				}
			}
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x000CD358 File Offset: 0x000CC358
		private void AppendZoneOrigin(ZoneIdentityPermission z, UrlIdentityPermission u)
		{
			if (z != null)
			{
				if (this.m_zoneList == null)
				{
					this.m_zoneList = new ArrayList();
				}
				this.m_zoneList.Add(z.SecurityZone);
			}
			if (u != null)
			{
				if (this.m_originList == null)
				{
					this.m_originList = new ArrayList();
				}
				this.m_originList.Add(u.Url);
			}
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x000CD3BC File Offset: 0x000CC3BC
		[ComVisible(true)]
		internal static PermissionListSet CreateCompressedState(CompressedStack cs, CompressedStack innerCS)
		{
			bool flag = false;
			if (cs.CompressedStackHandle == null)
			{
				return null;
			}
			PermissionListSet permissionListSet = new PermissionListSet();
			PermissionSetTriple permissionSetTriple = new PermissionSetTriple();
			int dcscount = CompressedStack.GetDCSCount(cs.CompressedStackHandle);
			int num = dcscount - 1;
			while (num >= 0 && !flag)
			{
				DomainCompressedStack domainCompressedStack = CompressedStack.GetDomainCompressedStack(cs.CompressedStackHandle, num);
				if (domainCompressedStack != null)
				{
					if (domainCompressedStack.PLS == null)
					{
						throw new SecurityException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Security_Generic"), new object[0]));
					}
					permissionListSet.UpdateZoneAndOrigin(domainCompressedStack.PLS);
					permissionListSet.Update(permissionSetTriple, domainCompressedStack.PLS);
					flag = domainCompressedStack.ConstructionHalted;
				}
				num--;
			}
			if (!flag)
			{
				PermissionListSet permissionListSet2 = null;
				if (innerCS != null)
				{
					innerCS.CompleteConstruction(null);
					permissionListSet2 = innerCS.PLS;
				}
				permissionListSet.Terminate(permissionSetTriple, permissionListSet2);
			}
			else
			{
				permissionListSet.Terminate(permissionSetTriple);
			}
			return permissionListSet;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x000CD490 File Offset: 0x000CC490
		internal static PermissionListSet CreateCompressedState(IntPtr unmanagedDCS, out bool bHaltConstruction)
		{
			PermissionListSet permissionListSet = new PermissionListSet();
			PermissionSetTriple permissionSetTriple = new PermissionSetTriple();
			int descCount = DomainCompressedStack.GetDescCount(unmanagedDCS);
			bHaltConstruction = false;
			int num = 0;
			while (num < descCount && !bHaltConstruction)
			{
				PermissionSet permissionSet;
				PermissionSet permissionSet2;
				Assembly assembly;
				FrameSecurityDescriptor frameSecurityDescriptor;
				if (DomainCompressedStack.GetDescriptorInfo(unmanagedDCS, num, out permissionSet, out permissionSet2, out assembly, out frameSecurityDescriptor))
				{
					bHaltConstruction = permissionListSet.Update(permissionSetTriple, frameSecurityDescriptor);
				}
				else
				{
					permissionListSet.Update(permissionSetTriple, permissionSet, permissionSet2);
				}
				num++;
			}
			if (!bHaltConstruction && !DomainCompressedStack.IgnoreDomain(unmanagedDCS))
			{
				PermissionSet permissionSet;
				PermissionSet permissionSet2;
				DomainCompressedStack.GetDomainPermissionSets(unmanagedDCS, out permissionSet, out permissionSet2);
				permissionListSet.Update(permissionSetTriple, permissionSet, permissionSet2);
			}
			permissionListSet.Terminate(permissionSetTriple);
			return permissionListSet;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x000CD518 File Offset: 0x000CC518
		internal static PermissionListSet CreateCompressedState_HG()
		{
			PermissionListSet permissionListSet = new PermissionListSet();
			CompressedStack.GetHomogeneousPLS(permissionListSet);
			return permissionListSet;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x000CD534 File Offset: 0x000CC534
		internal bool CheckDemandNoThrow(CodeAccessPermission demand)
		{
			PermissionToken permissionToken = null;
			if (demand != null)
			{
				permissionToken = PermissionToken.GetToken(demand);
			}
			return this.m_firstPermSetTriple.CheckDemandNoThrow(demand, permissionToken);
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x000CD55A File Offset: 0x000CC55A
		internal bool CheckSetDemandNoThrow(PermissionSet pSet)
		{
			return this.m_firstPermSetTriple.CheckSetDemandNoThrow(pSet);
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x000CD568 File Offset: 0x000CC568
		internal bool CheckDemand(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			bool flag = true;
			if (this.m_permSetTriples != null)
			{
				for (int i = 0; i < this.m_permSetTriples.Count; i++)
				{
					if (!flag)
					{
						break;
					}
					PermissionSetTriple permissionSetTriple = (PermissionSetTriple)this.m_permSetTriples[i];
					flag = permissionSetTriple.CheckDemand(demand, permToken, rmh);
				}
			}
			else if (this.m_firstPermSetTriple != null)
			{
				flag = this.m_firstPermSetTriple.CheckDemand(demand, permToken, rmh);
			}
			return flag;
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x000CD5D0 File Offset: 0x000CC5D0
		internal bool CheckSetDemand(PermissionSet pset, RuntimeMethodHandle rmh)
		{
			PermissionSet permissionSet;
			this.CheckSetDemandWithModification(pset, out permissionSet, rmh);
			return false;
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x000CD5EC File Offset: 0x000CC5EC
		[SecurityCritical]
		internal bool CheckSetDemandWithModification(PermissionSet pset, out PermissionSet alteredDemandSet, RuntimeMethodHandle rmh)
		{
			bool flag = true;
			PermissionSet permissionSet = pset;
			alteredDemandSet = null;
			if (this.m_permSetTriples != null)
			{
				for (int i = 0; i < this.m_permSetTriples.Count; i++)
				{
					if (!flag)
					{
						break;
					}
					PermissionSetTriple permissionSetTriple = (PermissionSetTriple)this.m_permSetTriples[i];
					flag = permissionSetTriple.CheckSetDemand(permissionSet, out alteredDemandSet, rmh);
					if (alteredDemandSet != null)
					{
						permissionSet = alteredDemandSet;
					}
				}
			}
			else if (this.m_firstPermSetTriple != null)
			{
				flag = this.m_firstPermSetTriple.CheckSetDemand(permissionSet, out alteredDemandSet, rmh);
			}
			return flag;
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x000CD660 File Offset: 0x000CC660
		private bool CheckFlags(int flags)
		{
			bool flag = true;
			if (this.m_permSetTriples != null)
			{
				int num = 0;
				while (num < this.m_permSetTriples.Count && flag)
				{
					if (flags == 0)
					{
						break;
					}
					flag &= ((PermissionSetTriple)this.m_permSetTriples[num]).CheckFlags(ref flags);
					num++;
				}
			}
			else if (this.m_firstPermSetTriple != null)
			{
				flag = this.m_firstPermSetTriple.CheckFlags(ref flags);
			}
			return flag;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x000CD6C8 File Offset: 0x000CC6C8
		internal void DemandFlagsOrGrantSet(int flags, PermissionSet grantSet)
		{
			if (this.CheckFlags(flags))
			{
				return;
			}
			this.CheckSetDemand(grantSet, default(RuntimeMethodHandle));
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x000CD6F0 File Offset: 0x000CC6F0
		internal void GetZoneAndOrigin(ArrayList zoneList, ArrayList originList, PermissionToken zoneToken, PermissionToken originToken)
		{
			if (this.m_zoneList != null)
			{
				zoneList.AddRange(this.m_zoneList);
			}
			if (this.m_originList != null)
			{
				originList.AddRange(this.m_originList);
			}
		}

		// Token: 0x04001EB0 RID: 7856
		private PermissionSetTriple m_firstPermSetTriple;

		// Token: 0x04001EB1 RID: 7857
		private ArrayList m_permSetTriples;

		// Token: 0x04001EB2 RID: 7858
		private ArrayList m_zoneList;

		// Token: 0x04001EB3 RID: 7859
		private ArrayList m_originList;
	}
}
