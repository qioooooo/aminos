using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace System.Security
{
	// Token: 0x02000659 RID: 1625
	internal class CodeAccessSecurityEngine
	{
		// Token: 0x06003AD6 RID: 15062
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SpecialDemand(PermissionType whatPermission, ref StackCrawlMark stackMark);

		// Token: 0x06003AD7 RID: 15063
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool DoesFullTrustMeanFullTrust();

		// Token: 0x06003AD8 RID: 15064 RVA: 0x000C838E File Offset: 0x000C738E
		[Conditional("_DEBUG")]
		private static void DEBUG_OUT(string str)
		{
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x000C8390 File Offset: 0x000C7390
		private CodeAccessSecurityEngine()
		{
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x000C83B4 File Offset: 0x000C73B4
		private static void ThrowSecurityException(Assembly asm, PermissionSet granted, PermissionSet refused, RuntimeMethodHandle rmh, SecurityAction action, object demand, IPermission permThatFailed)
		{
			AssemblyName assemblyName = null;
			Evidence evidence = null;
			if (asm != null)
			{
				PermissionSet.s_fullTrust.Assert();
				assemblyName = asm.GetName();
				if (asm != Assembly.GetExecutingAssembly())
				{
					evidence = asm.Evidence;
				}
			}
			throw SecurityException.MakeSecurityException(assemblyName, evidence, granted, refused, rmh, action, demand, permThatFailed);
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x000C83F8 File Offset: 0x000C73F8
		private static void ThrowSecurityException(object assemblyOrString, PermissionSet granted, PermissionSet refused, RuntimeMethodHandle rmh, SecurityAction action, object demand, IPermission permThatFailed)
		{
			if (assemblyOrString == null || assemblyOrString is Assembly)
			{
				CodeAccessSecurityEngine.ThrowSecurityException((Assembly)assemblyOrString, granted, refused, rmh, action, demand, permThatFailed);
				return;
			}
			AssemblyName assemblyName = new AssemblyName((string)assemblyOrString);
			throw SecurityException.MakeSecurityException(assemblyName, null, granted, refused, rmh, action, demand, permThatFailed);
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x000C8444 File Offset: 0x000C7444
		private static void LazyCheckSetHelper(PermissionSet demands, IntPtr asmSecDesc, RuntimeMethodHandle rmh, Assembly assembly, SecurityAction action)
		{
			if (demands.CanUnrestrictedOverride())
			{
				return;
			}
			PermissionSet permissionSet;
			PermissionSet permissionSet2;
			CodeAccessSecurityEngine._GetGrantedPermissionSet(asmSecDesc, out permissionSet, out permissionSet2);
			CodeAccessSecurityEngine.CheckSetHelper(permissionSet, permissionSet2, demands, rmh, assembly, action, true);
		}

		// Token: 0x06003ADE RID: 15070
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetGrantedPermissionSet(IntPtr secDesc, out PermissionSet grants, out PermissionSet refused);

		// Token: 0x06003ADF RID: 15071 RVA: 0x000C8472 File Offset: 0x000C7472
		internal static void CheckSetHelper(CompressedStack cs, PermissionSet grants, PermissionSet refused, PermissionSet demands, RuntimeMethodHandle rmh, Assembly asm, SecurityAction action)
		{
			if (cs != null)
			{
				cs.CheckSetDemand(demands, rmh);
				return;
			}
			CodeAccessSecurityEngine.CheckSetHelper(grants, refused, demands, rmh, asm, action, true);
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x000C8494 File Offset: 0x000C7494
		internal static bool CheckSetHelper(PermissionSet grants, PermissionSet refused, PermissionSet demands, RuntimeMethodHandle rmh, object assemblyOrString, SecurityAction action, bool throwException)
		{
			IPermission permission = null;
			if (grants != null)
			{
				grants.CheckDecoded(demands);
			}
			if (refused != null)
			{
				refused.CheckDecoded(demands);
			}
			bool flag = SecurityManager._SetThreadSecurity(false);
			try
			{
				if (!demands.CheckDemand(grants, out permission))
				{
					if (!throwException)
					{
						return false;
					}
					CodeAccessSecurityEngine.ThrowSecurityException(assemblyOrString, grants, refused, rmh, action, demands, permission);
				}
				if (!demands.CheckDeny(refused, out permission))
				{
					if (!throwException)
					{
						return false;
					}
					CodeAccessSecurityEngine.ThrowSecurityException(assemblyOrString, grants, refused, rmh, action, demands, permission);
				}
			}
			catch (SecurityException)
			{
				throw;
			}
			catch (Exception)
			{
				if (!throwException)
				{
					return false;
				}
				CodeAccessSecurityEngine.ThrowSecurityException(assemblyOrString, grants, refused, rmh, action, demands, permission);
			}
			catch
			{
				return false;
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

		// Token: 0x06003AE1 RID: 15073 RVA: 0x000C8570 File Offset: 0x000C7570
		internal static void CheckHelper(CompressedStack cs, PermissionSet grantedSet, PermissionSet refusedSet, CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh, Assembly asm, SecurityAction action)
		{
			if (cs != null)
			{
				cs.CheckDemand(demand, permToken, rmh);
				return;
			}
			CodeAccessSecurityEngine.CheckHelper(grantedSet, refusedSet, demand, permToken, rmh, asm, action, true);
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x000C8594 File Offset: 0x000C7594
		internal static bool CheckHelper(PermissionSet grantedSet, PermissionSet refusedSet, CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh, object assemblyOrString, SecurityAction action, bool throwException)
		{
			if (permToken == null)
			{
				permToken = PermissionToken.GetToken(demand);
			}
			if (grantedSet != null)
			{
				grantedSet.CheckDecoded(permToken.m_index);
			}
			if (refusedSet != null)
			{
				refusedSet.CheckDecoded(permToken.m_index);
			}
			bool flag = SecurityManager._SetThreadSecurity(false);
			try
			{
				if (grantedSet == null)
				{
					if (!throwException)
					{
						return false;
					}
					CodeAccessSecurityEngine.ThrowSecurityException(assemblyOrString, grantedSet, refusedSet, rmh, action, demand, demand);
				}
				else if (!grantedSet.IsUnrestricted() || !demand.CanUnrestrictedOverride())
				{
					CodeAccessPermission codeAccessPermission = (CodeAccessPermission)grantedSet.GetPermission(permToken);
					if (!demand.CheckDemand(codeAccessPermission))
					{
						if (!throwException)
						{
							return false;
						}
						CodeAccessSecurityEngine.ThrowSecurityException(assemblyOrString, grantedSet, refusedSet, rmh, action, demand, demand);
					}
				}
				if (refusedSet != null)
				{
					CodeAccessPermission codeAccessPermission2 = (CodeAccessPermission)refusedSet.GetPermission(permToken);
					if (codeAccessPermission2 != null && !codeAccessPermission2.CheckDeny(demand))
					{
						if (!throwException)
						{
							return false;
						}
						CodeAccessSecurityEngine.ThrowSecurityException(assemblyOrString, grantedSet, refusedSet, rmh, action, demand, demand);
					}
					if (refusedSet.IsUnrestricted() && demand.CanUnrestrictedOverride())
					{
						if (!throwException)
						{
							return false;
						}
						CodeAccessSecurityEngine.ThrowSecurityException(assemblyOrString, grantedSet, refusedSet, rmh, action, demand, demand);
					}
				}
			}
			catch (SecurityException)
			{
				throw;
			}
			catch (Exception)
			{
				if (!throwException)
				{
					return false;
				}
				CodeAccessSecurityEngine.ThrowSecurityException(assemblyOrString, grantedSet, refusedSet, rmh, action, demand, demand);
			}
			catch
			{
				return false;
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

		// Token: 0x06003AE3 RID: 15075 RVA: 0x000C86FC File Offset: 0x000C76FC
		private static void CheckGrantSetHelper(PermissionSet grantSet)
		{
			grantSet.CopyWithNoIdentityPermissions().Demand();
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x000C8709 File Offset: 0x000C7709
		internal static void ReflectionTargetDemandHelper(PermissionType permission, PermissionSet targetGrant)
		{
			CodeAccessSecurityEngine.ReflectionTargetDemandHelper((int)permission, targetGrant);
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x000C8714 File Offset: 0x000C7714
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ReflectionTargetDemandHelper(int permission, PermissionSet targetGrant)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			CompressedStack compressedStack = CompressedStack.GetCompressedStack(ref stackCrawlMark);
			CodeAccessSecurityEngine.ReflectionTargetDemandHelper(permission, targetGrant, compressedStack);
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x000C8733 File Offset: 0x000C7733
		private static void ReflectionTargetDemandHelper(int permission, PermissionSet targetGrant, Resolver accessContext)
		{
			CodeAccessSecurityEngine.ReflectionTargetDemandHelper(permission, targetGrant, accessContext.GetSecurityContext());
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x000C8744 File Offset: 0x000C7744
		private static void ReflectionTargetDemandHelper(int permission, PermissionSet targetGrant, CompressedStack securityContext)
		{
			PermissionSet permissionSet;
			if (targetGrant == null)
			{
				permissionSet = new PermissionSet(PermissionState.Unrestricted);
			}
			else
			{
				permissionSet = targetGrant.CopyWithNoIdentityPermissions();
				permissionSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
			}
			securityContext.DemandFlagsOrGrantSet(1 << permission, permissionSet);
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x000C8780 File Offset: 0x000C7780
		internal static void GetZoneAndOriginHelper(CompressedStack cs, PermissionSet grantSet, PermissionSet refusedSet, ArrayList zoneList, ArrayList originList)
		{
			if (cs != null)
			{
				cs.GetZoneAndOrigin(zoneList, originList, PermissionToken.GetToken(typeof(ZoneIdentityPermission)), PermissionToken.GetToken(typeof(UrlIdentityPermission)));
				return;
			}
			ZoneIdentityPermission zoneIdentityPermission = (ZoneIdentityPermission)grantSet.GetPermission(typeof(ZoneIdentityPermission));
			UrlIdentityPermission urlIdentityPermission = (UrlIdentityPermission)grantSet.GetPermission(typeof(UrlIdentityPermission));
			if (zoneIdentityPermission != null)
			{
				zoneList.Add(zoneIdentityPermission.SecurityZone);
			}
			if (urlIdentityPermission != null)
			{
				originList.Add(urlIdentityPermission.Url);
			}
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x000C880A File Offset: 0x000C780A
		internal static void GetZoneAndOrigin(ref StackCrawlMark mark, out ArrayList zone, out ArrayList origin)
		{
			zone = new ArrayList();
			origin = new ArrayList();
			CodeAccessSecurityEngine.GetZoneAndOriginInternal(zone, origin, ref mark);
		}

		// Token: 0x06003AEA RID: 15082
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetZoneAndOriginInternal(ArrayList zoneList, ArrayList originList, ref StackCrawlMark stackMark);

		// Token: 0x06003AEB RID: 15083 RVA: 0x000C8824 File Offset: 0x000C7824
		internal static void CheckAssembly(Assembly asm, CodeAccessPermission demand)
		{
			if (SecurityManager._IsSecurityOn())
			{
				PermissionSet permissionSet;
				PermissionSet permissionSet2;
				asm.nGetGrantSet(out permissionSet, out permissionSet2);
				CodeAccessSecurityEngine.CheckHelper(permissionSet, permissionSet2, demand, PermissionToken.GetToken(demand), RuntimeMethodHandle.EmptyHandle, asm, SecurityAction.Demand, true);
			}
		}

		// Token: 0x06003AEC RID: 15084
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Check(object demand, ref StackCrawlMark stackMark, bool isPermSet);

		// Token: 0x06003AED RID: 15085
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool QuickCheckForAllDemands();

		// Token: 0x06003AEE RID: 15086
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool AllDomainsHomogeneousWithNoStackModifiers();

		// Token: 0x06003AEF RID: 15087 RVA: 0x000C8859 File Offset: 0x000C7859
		internal static void Check(CodeAccessPermission cap, ref StackCrawlMark stackMark)
		{
			CodeAccessSecurityEngine.Check(cap, ref stackMark, false);
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x000C8863 File Offset: 0x000C7863
		internal static void Check(PermissionSet permSet, ref StackCrawlMark stackMark)
		{
			CodeAccessSecurityEngine.Check(permSet, ref stackMark, true);
		}

		// Token: 0x06003AF1 RID: 15089
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FrameSecurityDescriptor CheckNReturnSO(PermissionToken permToken, CodeAccessPermission demand, ref StackCrawlMark stackMark, int unrestrictedOverride, int create);

		// Token: 0x06003AF2 RID: 15090 RVA: 0x000C8870 File Offset: 0x000C7870
		internal static void Assert(CodeAccessPermission cap, ref StackCrawlMark stackMark)
		{
			FrameSecurityDescriptor frameSecurityDescriptor = CodeAccessSecurityEngine.CheckNReturnSO(CodeAccessSecurityEngine.AssertPermissionToken, CodeAccessSecurityEngine.AssertPermission, ref stackMark, 1, 1);
			if (frameSecurityDescriptor == null)
			{
				if (SecurityManager._IsSecurityOn())
				{
					throw new ExecutionEngineException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
				}
			}
			else
			{
				if (frameSecurityDescriptor.HasImperativeAsserts())
				{
					throw new SecurityException(Environment.GetResourceString("Security_MustRevertOverride"));
				}
				frameSecurityDescriptor.SetAssert(cap);
			}
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x000C88CC File Offset: 0x000C78CC
		internal static void Deny(CodeAccessPermission cap, ref StackCrawlMark stackMark)
		{
			FrameSecurityDescriptor securityObjectForFrame = SecurityRuntime.GetSecurityObjectForFrame(ref stackMark, true);
			if (securityObjectForFrame == null)
			{
				if (SecurityManager._IsSecurityOn())
				{
					throw new ExecutionEngineException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
				}
			}
			else
			{
				if (securityObjectForFrame.HasImperativeDenials())
				{
					throw new SecurityException(Environment.GetResourceString("Security_MustRevertOverride"));
				}
				securityObjectForFrame.SetDeny(cap);
			}
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x000C891C File Offset: 0x000C791C
		internal static void PermitOnly(CodeAccessPermission cap, ref StackCrawlMark stackMark)
		{
			FrameSecurityDescriptor securityObjectForFrame = SecurityRuntime.GetSecurityObjectForFrame(ref stackMark, true);
			if (securityObjectForFrame == null)
			{
				if (SecurityManager._IsSecurityOn())
				{
					throw new ExecutionEngineException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
				}
			}
			else
			{
				if (securityObjectForFrame.HasImperativeRestrictions())
				{
					throw new SecurityException(Environment.GetResourceString("Security_MustRevertOverride"));
				}
				securityObjectForFrame.SetPermitOnly(cap);
			}
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x000C896C File Offset: 0x000C796C
		private static PermissionListSet UpdateAppDomainPLS(PermissionListSet adPLS, PermissionSet grantedPerms, PermissionSet refusedPerms)
		{
			if (adPLS == null)
			{
				adPLS = new PermissionListSet();
				adPLS.UpdateDomainPLS(grantedPerms, refusedPerms);
				return adPLS;
			}
			PermissionListSet permissionListSet = new PermissionListSet();
			permissionListSet.UpdateDomainPLS(adPLS);
			permissionListSet.UpdateDomainPLS(grantedPerms, refusedPerms);
			return permissionListSet;
		}

		// Token: 0x04001E59 RID: 7769
		internal static SecurityPermission AssertPermission = new SecurityPermission(SecurityPermissionFlag.Assertion);

		// Token: 0x04001E5A RID: 7770
		internal static PermissionToken AssertPermissionToken = PermissionToken.GetToken(CodeAccessSecurityEngine.AssertPermission);
	}
}
