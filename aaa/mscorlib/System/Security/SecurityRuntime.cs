using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Security
{
	// Token: 0x0200067E RID: 1662
	internal class SecurityRuntime
	{
		// Token: 0x06003CB2 RID: 15538 RVA: 0x000D0A01 File Offset: 0x000CFA01
		private SecurityRuntime()
		{
		}

		// Token: 0x06003CB3 RID: 15539
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FrameSecurityDescriptor GetSecurityObjectForFrame(ref StackCrawlMark stackMark, bool create);

		// Token: 0x06003CB4 RID: 15540 RVA: 0x000D0A0C File Offset: 0x000CFA0C
		private static int OverridesHelper(FrameSecurityDescriptor secDesc)
		{
			int num = SecurityRuntime.OverridesHelper2(secDesc, false);
			return num + SecurityRuntime.OverridesHelper2(secDesc, true);
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x000D0A2C File Offset: 0x000CFA2C
		private static int OverridesHelper2(FrameSecurityDescriptor secDesc, bool fDeclarative)
		{
			int num = 0;
			PermissionSet permissionSet = secDesc.GetPermitOnly(fDeclarative);
			if (permissionSet != null)
			{
				num++;
			}
			permissionSet = secDesc.GetDenials(fDeclarative);
			if (permissionSet != null)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x000D0A5C File Offset: 0x000CFA5C
		internal static MethodInfo GetMethodInfo(RuntimeMethodHandle rmh)
		{
			if (rmh.IsNullHandle())
			{
				return null;
			}
			PermissionSet.s_fullTrust.Assert();
			RuntimeTypeHandle declaringType = rmh.GetDeclaringType();
			return RuntimeType.GetMethodBase(declaringType, rmh) as MethodInfo;
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x000D0A92 File Offset: 0x000CFA92
		private static bool FrameDescSetHelper(FrameSecurityDescriptor secDesc, PermissionSet demandSet, out PermissionSet alteredDemandSet, RuntimeMethodHandle rmh)
		{
			return secDesc.CheckSetDemand(demandSet, out alteredDemandSet, rmh);
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x000D0A9D File Offset: 0x000CFA9D
		private static bool FrameDescHelper(FrameSecurityDescriptor secDesc, IPermission demandIn, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			return secDesc.CheckDemand((CodeAccessPermission)demandIn, permToken, rmh);
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x000D0AB0 File Offset: 0x000CFAB0
		[SecurityCritical]
		private static bool CheckDynamicMethodSetHelper(DynamicResolver dynamicResolver, PermissionSet demandSet, out PermissionSet alteredDemandSet, RuntimeMethodHandle rmh)
		{
			CompressedStack securityContext = dynamicResolver.GetSecurityContext();
			bool flag;
			try
			{
				flag = securityContext.CheckSetDemandWithModificationNoHalt(demandSet, out alteredDemandSet, rmh);
			}
			catch (SecurityException ex)
			{
				throw new SecurityException(Environment.GetResourceString("Security_AnonymouslyHostedDynamicMethodCheckFailed"), ex);
			}
			return flag;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x000D0AF4 File Offset: 0x000CFAF4
		[SecurityCritical]
		private static bool CheckDynamicMethodHelper(DynamicResolver dynamicResolver, IPermission demandIn, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			CompressedStack securityContext = dynamicResolver.GetSecurityContext();
			bool flag;
			try
			{
				flag = securityContext.CheckDemandNoHalt((CodeAccessPermission)demandIn, permToken, rmh);
			}
			catch (SecurityException ex)
			{
				throw new SecurityException(Environment.GetResourceString("Security_AnonymouslyHostedDynamicMethodCheckFailed"), ex);
			}
			return flag;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x000D0B3C File Offset: 0x000CFB3C
		internal static void Assert(PermissionSet permSet, ref StackCrawlMark stackMark)
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
				frameSecurityDescriptor.SetAssert(permSet);
			}
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x000D0B98 File Offset: 0x000CFB98
		internal static void AssertAllPossible(ref StackCrawlMark stackMark)
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
				if (securityObjectForFrame.GetAssertAllPossible())
				{
					throw new SecurityException(Environment.GetResourceString("Security_MustRevertOverride"));
				}
				securityObjectForFrame.SetAssertAllPossible();
			}
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x000D0BE8 File Offset: 0x000CFBE8
		internal static void Deny(PermissionSet permSet, ref StackCrawlMark stackMark)
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
				securityObjectForFrame.SetDeny(permSet);
			}
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x000D0C38 File Offset: 0x000CFC38
		internal static void PermitOnly(PermissionSet permSet, ref StackCrawlMark stackMark)
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
				securityObjectForFrame.SetPermitOnly(permSet);
			}
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x000D0C88 File Offset: 0x000CFC88
		internal static void RevertAssert(ref StackCrawlMark stackMark)
		{
			FrameSecurityDescriptor securityObjectForFrame = SecurityRuntime.GetSecurityObjectForFrame(ref stackMark, false);
			if (securityObjectForFrame != null)
			{
				securityObjectForFrame.RevertAssert();
				return;
			}
			if (SecurityManager._IsSecurityOn())
			{
				throw new ExecutionEngineException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
			}
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x000D0CC0 File Offset: 0x000CFCC0
		internal static void RevertDeny(ref StackCrawlMark stackMark)
		{
			FrameSecurityDescriptor securityObjectForFrame = SecurityRuntime.GetSecurityObjectForFrame(ref stackMark, false);
			if (securityObjectForFrame != null)
			{
				securityObjectForFrame.RevertDeny();
				return;
			}
			if (SecurityManager._IsSecurityOn())
			{
				throw new ExecutionEngineException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
			}
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x000D0CF8 File Offset: 0x000CFCF8
		internal static void RevertPermitOnly(ref StackCrawlMark stackMark)
		{
			FrameSecurityDescriptor securityObjectForFrame = SecurityRuntime.GetSecurityObjectForFrame(ref stackMark, false);
			if (securityObjectForFrame != null)
			{
				securityObjectForFrame.RevertPermitOnly();
				return;
			}
			if (SecurityManager._IsSecurityOn())
			{
				throw new ExecutionEngineException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
			}
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x000D0D30 File Offset: 0x000CFD30
		internal static void RevertAll(ref StackCrawlMark stackMark)
		{
			FrameSecurityDescriptor securityObjectForFrame = SecurityRuntime.GetSecurityObjectForFrame(ref stackMark, false);
			if (securityObjectForFrame != null)
			{
				securityObjectForFrame.RevertAll();
				return;
			}
			if (SecurityManager._IsSecurityOn())
			{
				throw new ExecutionEngineException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
			}
		}

		// Token: 0x04001F03 RID: 7939
		internal const bool StackContinue = true;

		// Token: 0x04001F04 RID: 7940
		internal const bool StackHalt = false;
	}
}
