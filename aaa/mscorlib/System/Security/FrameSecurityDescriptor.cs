using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security
{
	// Token: 0x0200065A RID: 1626
	[Serializable]
	internal class FrameSecurityDescriptor
	{
		// Token: 0x06003AF6 RID: 15094
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void IncrementOverridesCount();

		// Token: 0x06003AF7 RID: 15095
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DecrementOverridesCount();

		// Token: 0x06003AF8 RID: 15096
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void IncrementAssertCount();

		// Token: 0x06003AF9 RID: 15097
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DecrementAssertCount();

		// Token: 0x06003AFA RID: 15098 RVA: 0x000C89A3 File Offset: 0x000C79A3
		internal FrameSecurityDescriptor()
		{
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x000C89AC File Offset: 0x000C79AC
		private PermissionSet CreateSingletonSet(IPermission perm)
		{
			PermissionSet permissionSet = new PermissionSet(false);
			permissionSet.AddPermission(perm.Copy());
			return permissionSet;
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x000C89CE File Offset: 0x000C79CE
		internal bool HasImperativeAsserts()
		{
			return this.m_assertions != null;
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x000C89DC File Offset: 0x000C79DC
		internal bool HasImperativeDenials()
		{
			return this.m_denials != null;
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x000C89EA File Offset: 0x000C79EA
		internal bool HasImperativeRestrictions()
		{
			return this.m_restriction != null;
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x000C89F8 File Offset: 0x000C79F8
		internal void SetAssert(IPermission perm)
		{
			this.m_assertions = this.CreateSingletonSet(perm);
			FrameSecurityDescriptor.IncrementAssertCount();
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x000C8A0C File Offset: 0x000C7A0C
		internal void SetAssert(PermissionSet permSet)
		{
			this.m_assertions = permSet.Copy();
			this.m_AssertFT = this.m_AssertFT || (CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() && this.m_assertions.IsUnrestricted());
			FrameSecurityDescriptor.IncrementAssertCount();
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x000C8A45 File Offset: 0x000C7A45
		internal PermissionSet GetAssertions(bool fDeclarative)
		{
			if (!fDeclarative)
			{
				return this.m_assertions;
			}
			return this.m_DeclarativeAssertions;
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x000C8A57 File Offset: 0x000C7A57
		internal void SetAssertAllPossible()
		{
			this.m_assertAllPossible = true;
			FrameSecurityDescriptor.IncrementAssertCount();
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x000C8A65 File Offset: 0x000C7A65
		internal bool GetAssertAllPossible()
		{
			return this.m_assertAllPossible;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x000C8A6D File Offset: 0x000C7A6D
		internal void SetDeny(IPermission perm)
		{
			this.m_denials = this.CreateSingletonSet(perm);
			FrameSecurityDescriptor.IncrementOverridesCount();
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x000C8A81 File Offset: 0x000C7A81
		internal void SetDeny(PermissionSet permSet)
		{
			this.m_denials = permSet.Copy();
			FrameSecurityDescriptor.IncrementOverridesCount();
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000C8A94 File Offset: 0x000C7A94
		internal PermissionSet GetDenials(bool fDeclarative)
		{
			if (!fDeclarative)
			{
				return this.m_denials;
			}
			return this.m_DeclarativeDenials;
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x000C8AA6 File Offset: 0x000C7AA6
		internal void SetPermitOnly(IPermission perm)
		{
			this.m_restriction = this.CreateSingletonSet(perm);
			FrameSecurityDescriptor.IncrementOverridesCount();
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x000C8ABA File Offset: 0x000C7ABA
		internal void SetPermitOnly(PermissionSet permSet)
		{
			this.m_restriction = permSet.Copy();
			FrameSecurityDescriptor.IncrementOverridesCount();
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x000C8ACD File Offset: 0x000C7ACD
		internal PermissionSet GetPermitOnly(bool fDeclarative)
		{
			if (!fDeclarative)
			{
				return this.m_restriction;
			}
			return this.m_DeclarativeRestrictions;
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x000C8ADF File Offset: 0x000C7ADF
		internal void SetTokenHandles(SafeTokenHandle callerToken, SafeTokenHandle impToken)
		{
			this.m_callerToken = callerToken;
			this.m_impToken = impToken;
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x000C8AF0 File Offset: 0x000C7AF0
		internal void RevertAssert()
		{
			if (this.m_assertions != null)
			{
				this.m_assertions = null;
				FrameSecurityDescriptor.DecrementAssertCount();
			}
			if (this.m_DeclarativeAssertions != null)
			{
				this.m_AssertFT = CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() && this.m_DeclarativeAssertions.IsUnrestricted();
				return;
			}
			this.m_AssertFT = false;
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x000C8B3C File Offset: 0x000C7B3C
		internal void RevertAssertAllPossible()
		{
			if (this.m_assertAllPossible)
			{
				this.m_assertAllPossible = false;
				FrameSecurityDescriptor.DecrementAssertCount();
			}
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x000C8B52 File Offset: 0x000C7B52
		internal void RevertDeny()
		{
			if (this.HasImperativeDenials())
			{
				FrameSecurityDescriptor.DecrementOverridesCount();
				this.m_denials = null;
			}
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x000C8B68 File Offset: 0x000C7B68
		internal void RevertPermitOnly()
		{
			if (this.HasImperativeRestrictions())
			{
				FrameSecurityDescriptor.DecrementOverridesCount();
				this.m_restriction = null;
			}
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x000C8B7E File Offset: 0x000C7B7E
		internal void RevertAll()
		{
			this.RevertAssert();
			this.RevertAssertAllPossible();
			this.RevertDeny();
			this.RevertPermitOnly();
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x000C8B98 File Offset: 0x000C7B98
		internal bool CheckDemand(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			bool flag = this.CheckDemand2(demand, permToken, rmh, false);
			if (flag)
			{
				flag = this.CheckDemand2(demand, permToken, rmh, true);
			}
			return flag;
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x000C8BC0 File Offset: 0x000C7BC0
		internal bool CheckDemand2(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh, bool fDeclarative)
		{
			if (this.GetPermitOnly(fDeclarative) != null)
			{
				this.GetPermitOnly(fDeclarative).CheckDecoded(demand, permToken);
			}
			if (this.GetDenials(fDeclarative) != null)
			{
				this.GetDenials(fDeclarative).CheckDecoded(demand, permToken);
			}
			if (this.GetAssertions(fDeclarative) != null)
			{
				this.GetAssertions(fDeclarative).CheckDecoded(demand, permToken);
			}
			bool flag = SecurityManager._SetThreadSecurity(false);
			try
			{
				PermissionSet permissionSet = this.GetPermitOnly(fDeclarative);
				if (permissionSet != null)
				{
					CodeAccessPermission codeAccessPermission = (CodeAccessPermission)permissionSet.GetPermission(demand);
					if (codeAccessPermission == null)
					{
						if (!permissionSet.IsUnrestricted() || !demand.CanUnrestrictedOverride())
						{
							throw new SecurityException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Security_Generic"), new object[] { demand.GetType().AssemblyQualifiedName }), null, permissionSet, SecurityRuntime.GetMethodInfo(rmh), demand, demand);
						}
					}
					else
					{
						bool flag2 = true;
						try
						{
							flag2 = !demand.CheckPermitOnly(codeAccessPermission);
						}
						catch (ArgumentException)
						{
						}
						if (flag2)
						{
							throw new SecurityException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Security_Generic"), new object[] { demand.GetType().AssemblyQualifiedName }), null, permissionSet, SecurityRuntime.GetMethodInfo(rmh), demand, demand);
						}
					}
				}
				permissionSet = this.GetDenials(fDeclarative);
				if (permissionSet != null)
				{
					CodeAccessPermission codeAccessPermission2 = (CodeAccessPermission)permissionSet.GetPermission(demand);
					if (permissionSet.IsUnrestricted() && demand.CanUnrestrictedOverride())
					{
						throw new SecurityException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Security_Generic"), new object[] { demand.GetType().AssemblyQualifiedName }), permissionSet, null, SecurityRuntime.GetMethodInfo(rmh), demand, demand);
					}
					bool flag3 = true;
					try
					{
						flag3 = !demand.CheckDeny(codeAccessPermission2);
					}
					catch (ArgumentException)
					{
					}
					if (flag3)
					{
						throw new SecurityException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Security_Generic"), new object[] { demand.GetType().AssemblyQualifiedName }), permissionSet, null, SecurityRuntime.GetMethodInfo(rmh), demand, demand);
					}
				}
				if (this.GetAssertAllPossible())
				{
					return false;
				}
				permissionSet = this.GetAssertions(fDeclarative);
				if (permissionSet != null)
				{
					CodeAccessPermission codeAccessPermission3 = (CodeAccessPermission)permissionSet.GetPermission(demand);
					try
					{
						if ((permissionSet.IsUnrestricted() && demand.CanUnrestrictedOverride()) || demand.CheckAssert(codeAccessPermission3))
						{
							return false;
						}
					}
					catch (ArgumentException)
					{
					}
				}
			}
			finally
			{
				if (flag)
				{
					SecurityManager._SetThreadSecurity(true);
				}
			}
			return true;
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x000C8E60 File Offset: 0x000C7E60
		internal bool CheckSetDemand(PermissionSet demandSet, out PermissionSet alteredDemandSet, RuntimeMethodHandle rmh)
		{
			PermissionSet permissionSet = null;
			PermissionSet permissionSet2 = null;
			bool flag = this.CheckSetDemand2(demandSet, out permissionSet, rmh, false);
			if (permissionSet != null)
			{
				demandSet = permissionSet;
			}
			if (flag)
			{
				flag = this.CheckSetDemand2(demandSet, out permissionSet2, rmh, true);
			}
			if (permissionSet2 != null)
			{
				alteredDemandSet = permissionSet2;
			}
			else if (permissionSet != null)
			{
				alteredDemandSet = permissionSet;
			}
			else
			{
				alteredDemandSet = null;
			}
			return flag;
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x000C8EA8 File Offset: 0x000C7EA8
		internal bool CheckSetDemand2(PermissionSet demandSet, out PermissionSet alteredDemandSet, RuntimeMethodHandle rmh, bool fDeclarative)
		{
			alteredDemandSet = null;
			if (demandSet == null || demandSet.IsEmpty())
			{
				return false;
			}
			if (this.GetPermitOnly(fDeclarative) != null)
			{
				this.GetPermitOnly(fDeclarative).CheckDecoded(demandSet);
			}
			if (this.GetDenials(fDeclarative) != null)
			{
				this.GetDenials(fDeclarative).CheckDecoded(demandSet);
			}
			if (this.GetAssertions(fDeclarative) != null)
			{
				this.GetAssertions(fDeclarative).CheckDecoded(demandSet);
			}
			bool flag = SecurityManager._SetThreadSecurity(false);
			try
			{
				PermissionSet permissionSet = this.GetPermitOnly(fDeclarative);
				if (permissionSet != null)
				{
					IPermission permission = null;
					bool flag2 = true;
					try
					{
						flag2 = !demandSet.CheckPermitOnly(permissionSet, out permission);
					}
					catch (ArgumentException)
					{
					}
					if (flag2)
					{
						throw new SecurityException(Environment.GetResourceString("Security_GenericNoType"), null, permissionSet, SecurityRuntime.GetMethodInfo(rmh), demandSet, permission);
					}
				}
				permissionSet = this.GetDenials(fDeclarative);
				if (permissionSet != null)
				{
					IPermission permission2 = null;
					bool flag3 = true;
					try
					{
						flag3 = !demandSet.CheckDeny(permissionSet, out permission2);
					}
					catch (ArgumentException)
					{
					}
					if (flag3)
					{
						throw new SecurityException(Environment.GetResourceString("Security_GenericNoType"), permissionSet, null, SecurityRuntime.GetMethodInfo(rmh), demandSet, permission2);
					}
				}
				if (this.GetAssertAllPossible())
				{
					return false;
				}
				permissionSet = this.GetAssertions(fDeclarative);
				if (permissionSet != null)
				{
					if (demandSet.CheckAssertion(permissionSet))
					{
						return false;
					}
					if (!permissionSet.IsUnrestricted())
					{
						PermissionSet.RemoveAssertedPermissionSet(demandSet, permissionSet, out alteredDemandSet);
					}
				}
			}
			finally
			{
				if (flag)
				{
					SecurityManager._SetThreadSecurity(true);
				}
			}
			return true;
		}

		// Token: 0x04001E5B RID: 7771
		private PermissionSet m_assertions;

		// Token: 0x04001E5C RID: 7772
		private PermissionSet m_denials;

		// Token: 0x04001E5D RID: 7773
		private PermissionSet m_restriction;

		// Token: 0x04001E5E RID: 7774
		private PermissionSet m_DeclarativeAssertions;

		// Token: 0x04001E5F RID: 7775
		private PermissionSet m_DeclarativeDenials;

		// Token: 0x04001E60 RID: 7776
		private PermissionSet m_DeclarativeRestrictions;

		// Token: 0x04001E61 RID: 7777
		[NonSerialized]
		private SafeTokenHandle m_callerToken;

		// Token: 0x04001E62 RID: 7778
		[NonSerialized]
		private SafeTokenHandle m_impToken;

		// Token: 0x04001E63 RID: 7779
		private bool m_AssertFT;

		// Token: 0x04001E64 RID: 7780
		private bool m_assertAllPossible;

		// Token: 0x04001E65 RID: 7781
		private bool m_declSecComputed;
	}
}
