using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Util;
using System.Threading;

namespace System.Security
{
	// Token: 0x0200067D RID: 1661
	[ComVisible(true)]
	public static class SecurityManager
	{
		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06003C93 RID: 15507 RVA: 0x000D014F File Offset: 0x000CF14F
		internal static PolicyManager PolicyManager
		{
			get
			{
				return SecurityManager.polmgr;
			}
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x000D0158 File Offset: 0x000CF158
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool IsGranted(IPermission perm)
		{
			if (perm == null)
			{
				return true;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			PermissionSet permissionSet;
			PermissionSet permissionSet2;
			SecurityManager._GetGrantedPermissions(out permissionSet, out permissionSet2, ref stackCrawlMark);
			return permissionSet.Contains(perm) && (permissionSet2 == null || !permissionSet2.Contains(perm));
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x000D0194 File Offset: 0x000CF194
		private static bool CheckExecution()
		{
			if (SecurityManager.checkExecution == -1)
			{
				SecurityManager.checkExecution = (((SecurityManager.GetGlobalFlags() & 256) != 0) ? 0 : 1);
			}
			if (SecurityManager.checkExecution == 1)
			{
				if (SecurityManager.securityPermissionType == null)
				{
					SecurityManager.securityPermissionType = typeof(SecurityPermission);
					SecurityManager.executionSecurityPermission = new SecurityPermission(SecurityPermissionFlag.Execution);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x000D01EC File Offset: 0x000CF1EC
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, Name = "System.Windows.Forms", PublicKey = "0x00000000000000000400000000000000")]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GetZoneAndOrigin(out ArrayList zone, out ArrayList origin)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (SecurityManager._IsSecurityOn())
			{
				CodeAccessSecurityEngine.GetZoneAndOrigin(ref stackCrawlMark, out zone, out origin);
				return;
			}
			zone = null;
			origin = null;
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x000D0214 File Offset: 0x000CF214
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		public static PolicyLevel LoadPolicyLevelFromFile(string path, PolicyLevelType type)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!File.InternalExists(path))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PolicyFileDoesNotExist"));
			}
			string fullPath = Path.GetFullPath(path);
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.None);
			fileIOPermission.AddPathList(FileIOPermissionAccess.Read, fullPath);
			fileIOPermission.AddPathList(FileIOPermissionAccess.Write, fullPath);
			fileIOPermission.Demand();
			PolicyLevel policyLevel;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					policyLevel = SecurityManager.LoadPolicyLevelFromStringHelper(streamReader.ReadToEnd(), path, type);
				}
			}
			return policyLevel;
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x000D02C0 File Offset: 0x000CF2C0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		public static PolicyLevel LoadPolicyLevelFromString(string str, PolicyLevelType type)
		{
			return SecurityManager.LoadPolicyLevelFromStringHelper(str, null, type);
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x000D02CC File Offset: 0x000CF2CC
		private static PolicyLevel LoadPolicyLevelFromStringHelper(string str, string path, PolicyLevelType type)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			PolicyLevel policyLevel = new PolicyLevel(type, path);
			Parser parser = new Parser(str);
			SecurityElement topElement = parser.GetTopElement();
			if (topElement == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[] { "configuration" }));
			}
			SecurityElement securityElement = topElement.SearchForChildByTag("mscorlib");
			if (securityElement == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[] { "mscorlib" }));
			}
			SecurityElement securityElement2 = securityElement.SearchForChildByTag("security");
			if (securityElement2 == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[] { "security" }));
			}
			SecurityElement securityElement3 = securityElement2.SearchForChildByTag("policy");
			if (securityElement3 == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[] { "policy" }));
			}
			SecurityElement securityElement4 = securityElement3.SearchForChildByTag("PolicyLevel");
			if (securityElement4 != null)
			{
				policyLevel.FromXml(securityElement4);
				return policyLevel;
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[] { "PolicyLevel" }));
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x000D0430 File Offset: 0x000CF430
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		public static void SavePolicyLevel(PolicyLevel level)
		{
			PolicyManager.EncodeLevel(level);
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x000D0438 File Offset: 0x000CF438
		private static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied, out int securitySpecialFlags, bool checkExecutionPermission)
		{
			CodeAccessPermission.AssertAllPossible();
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidence, reqdPset, optPset, denyPset, out denied, checkExecutionPermission);
			securitySpecialFlags = SecurityManager.GetSpecialFlags(permissionSet, denied);
			return permissionSet;
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x000D0465 File Offset: 0x000CF465
		public static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied)
		{
			return SecurityManager.ResolvePolicy(evidence, reqdPset, optPset, denyPset, out denied, true);
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x000D0474 File Offset: 0x000CF474
		private static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied, bool checkExecutionPermission)
		{
			Exception ex = null;
			PermissionSet permissionSet;
			if (reqdPset == null)
			{
				permissionSet = optPset;
			}
			else
			{
				permissionSet = ((optPset == null) ? null : reqdPset.Union(optPset));
			}
			if (permissionSet != null && !permissionSet.IsUnrestricted() && SecurityManager.CheckExecution())
			{
				permissionSet.AddPermission(SecurityManager.executionSecurityPermission);
			}
			if (evidence == null)
			{
				evidence = new Evidence();
			}
			else
			{
				evidence = evidence.ShallowCopy();
			}
			evidence.AddHost(new PermissionRequestEvidence(reqdPset, optPset, denyPset));
			PermissionSet permissionSet2 = SecurityManager.polmgr.Resolve(evidence);
			if (permissionSet != null)
			{
				permissionSet2.InplaceIntersect(permissionSet);
			}
			if (checkExecutionPermission && SecurityManager.CheckExecution() && (!permissionSet2.Contains(SecurityManager.executionSecurityPermission) || (denyPset != null && denyPset.Contains(SecurityManager.executionSecurityPermission))))
			{
				throw new PolicyException(Environment.GetResourceString("Policy_NoExecutionPermission"), -2146233320, ex);
			}
			if (reqdPset != null && !reqdPset.IsSubsetOf(permissionSet2))
			{
				throw new PolicyException(Environment.GetResourceString("Policy_NoRequiredPermission"), -2146233321, ex);
			}
			if (denyPset != null)
			{
				denied = denyPset.Copy();
				permissionSet2.MergeDeniedSet(denied);
				if (denied.IsEmpty())
				{
					denied = null;
				}
			}
			else
			{
				denied = null;
			}
			permissionSet2.IgnoreTypeLoadFailures = true;
			return permissionSet2;
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x000D0583 File Offset: 0x000CF583
		public static PermissionSet ResolvePolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				evidence = new Evidence();
			}
			else
			{
				evidence = evidence.ShallowCopy();
			}
			evidence.AddHost(new PermissionRequestEvidence(null, null, null));
			return SecurityManager.polmgr.Resolve(evidence);
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x000D05B4 File Offset: 0x000CF5B4
		public static PermissionSet ResolvePolicy(Evidence[] evidences)
		{
			if (evidences == null || evidences.Length == 0)
			{
				Evidence[] array = new Evidence[1];
				evidences = array;
			}
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidences[0]);
			if (permissionSet == null)
			{
				return null;
			}
			for (int i = 1; i < evidences.Length; i++)
			{
				permissionSet = permissionSet.Intersect(SecurityManager.ResolvePolicy(evidences[i]));
				if (permissionSet == null || permissionSet.IsEmpty())
				{
					return permissionSet;
				}
			}
			return permissionSet;
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x000D060C File Offset: 0x000CF60C
		public static PermissionSet ResolveSystemPolicy(Evidence evidence)
		{
			if (PolicyManager.IsGacAssembly(evidence))
			{
				return new PermissionSet(PermissionState.Unrestricted);
			}
			return SecurityManager.polmgr.CodeGroupResolve(evidence, true);
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x000D0629 File Offset: 0x000CF629
		public static IEnumerator ResolvePolicyGroups(Evidence evidence)
		{
			return SecurityManager.polmgr.ResolveCodeGroups(evidence);
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x000D0636 File Offset: 0x000CF636
		public static IEnumerator PolicyHierarchy()
		{
			return SecurityManager.polmgr.PolicyHierarchy();
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x000D0642 File Offset: 0x000CF642
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		public static void SavePolicy()
		{
			SecurityManager.polmgr.Save();
			SecurityManager.SaveGlobalFlags();
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06003CA4 RID: 15524 RVA: 0x000D0653 File Offset: 0x000CF653
		// (set) Token: 0x06003CA5 RID: 15525 RVA: 0x000D066A File Offset: 0x000CF66A
		public static bool CheckExecutionRights
		{
			get
			{
				return (SecurityManager.GetGlobalFlags() & 256) != 256;
			}
			set
			{
				if (value)
				{
					SecurityManager.checkExecution = 1;
					SecurityManager.SetGlobalFlags(256, 0);
					return;
				}
				new SecurityPermission(SecurityPermissionFlag.ControlPolicy).Demand();
				SecurityManager.checkExecution = 0;
				SecurityManager.SetGlobalFlags(256, 256);
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x000D06A2 File Offset: 0x000CF6A2
		// (set) Token: 0x06003CA7 RID: 15527 RVA: 0x000D06A9 File Offset: 0x000CF6A9
		[Obsolete("Because security can no longer be turned off permanently, setting the SecurityEnabled property no longer has any effect. Reading the property will still indicate whether security has been turned off temporarily.")]
		public static bool SecurityEnabled
		{
			get
			{
				return SecurityManager._IsSecurityOn();
			}
			set
			{
			}
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x000D06AC File Offset: 0x000CF6AC
		internal static int GetSpecialFlags(PermissionSet grantSet, PermissionSet deniedSet)
		{
			if (grantSet != null && grantSet.IsUnrestricted() && (deniedSet == null || deniedSet.IsEmpty()))
			{
				return -1;
			}
			SecurityPermissionFlag securityPermissionFlag = SecurityPermissionFlag.NoFlags;
			ReflectionPermissionFlag reflectionPermissionFlag = ReflectionPermissionFlag.NoFlags;
			CodeAccessPermission[] array = new CodeAccessPermission[6];
			if (grantSet != null)
			{
				if (grantSet.IsUnrestricted())
				{
					securityPermissionFlag = SecurityPermissionFlag.AllFlags;
					reflectionPermissionFlag = ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess | ReflectionPermissionFlag.ReflectionEmit | ReflectionPermissionFlag.RestrictedMemberAccess;
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = SecurityManager.s_UnrestrictedSpecialPermissionMap[i];
					}
				}
				else
				{
					SecurityPermission securityPermission = grantSet.GetPermission(6) as SecurityPermission;
					if (securityPermission != null)
					{
						securityPermissionFlag = securityPermission.Flags;
					}
					ReflectionPermission reflectionPermission = grantSet.GetPermission(4) as ReflectionPermission;
					if (reflectionPermission != null)
					{
						reflectionPermissionFlag = reflectionPermission.Flags;
					}
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = grantSet.GetPermission(SecurityManager.s_BuiltInPermissionIndexMap[j][0]) as CodeAccessPermission;
					}
				}
			}
			if (deniedSet != null)
			{
				if (deniedSet.IsUnrestricted())
				{
					securityPermissionFlag = SecurityPermissionFlag.NoFlags;
					reflectionPermissionFlag = ReflectionPermissionFlag.NoFlags;
					for (int k = 0; k < SecurityManager.s_BuiltInPermissionIndexMap.Length; k++)
					{
						array[k] = null;
					}
				}
				else
				{
					SecurityPermission securityPermission = deniedSet.GetPermission(6) as SecurityPermission;
					if (securityPermission != null)
					{
						securityPermissionFlag &= ~securityPermission.Flags;
					}
					ReflectionPermission reflectionPermission = deniedSet.GetPermission(4) as ReflectionPermission;
					if (reflectionPermission != null)
					{
						reflectionPermissionFlag &= ~reflectionPermission.Flags;
					}
					for (int l = 0; l < SecurityManager.s_BuiltInPermissionIndexMap.Length; l++)
					{
						CodeAccessPermission codeAccessPermission = deniedSet.GetPermission(SecurityManager.s_BuiltInPermissionIndexMap[l][0]) as CodeAccessPermission;
						if (codeAccessPermission != null && !codeAccessPermission.IsSubsetOf(null))
						{
							array[l] = null;
						}
					}
				}
			}
			int num = SecurityManager.MapToSpecialFlags(securityPermissionFlag, reflectionPermissionFlag);
			if (num != -1)
			{
				for (int m = 0; m < array.Length; m++)
				{
					if (array[m] != null && ((IUnrestrictedPermission)array[m]).IsUnrestricted())
					{
						num |= 1 << SecurityManager.s_BuiltInPermissionIndexMap[m][1];
					}
				}
			}
			return num;
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x000D086C File Offset: 0x000CF86C
		private static int MapToSpecialFlags(SecurityPermissionFlag securityPermissionFlags, ReflectionPermissionFlag reflectionPermissionFlags)
		{
			int num = 0;
			if ((securityPermissionFlags & SecurityPermissionFlag.UnmanagedCode) == SecurityPermissionFlag.UnmanagedCode)
			{
				num |= 1;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.SkipVerification) == SecurityPermissionFlag.SkipVerification)
			{
				num |= 2;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.Assertion) == SecurityPermissionFlag.Assertion)
			{
				num |= 8;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.SerializationFormatter) == SecurityPermissionFlag.SerializationFormatter)
			{
				num |= 32;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.BindingRedirects) == SecurityPermissionFlag.BindingRedirects)
			{
				num |= 256;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.ControlEvidence) == SecurityPermissionFlag.ControlEvidence)
			{
				num |= 65536;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.ControlPrincipal) == SecurityPermissionFlag.ControlPrincipal)
			{
				num |= 131072;
			}
			if ((reflectionPermissionFlags & ReflectionPermissionFlag.RestrictedMemberAccess) == ReflectionPermissionFlag.RestrictedMemberAccess)
			{
				num |= 64;
			}
			if ((reflectionPermissionFlags & ReflectionPermissionFlag.MemberAccess) == ReflectionPermissionFlag.MemberAccess)
			{
				num |= 16;
			}
			return num;
		}

		// Token: 0x06003CAA RID: 15530
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _IsSameType(string strLeft, string strRight);

		// Token: 0x06003CAB RID: 15531
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _SetThreadSecurity(bool bThreadSecurity);

		// Token: 0x06003CAC RID: 15532
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _IsSecurityOn();

		// Token: 0x06003CAD RID: 15533
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetGlobalFlags();

		// Token: 0x06003CAE RID: 15534
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetGlobalFlags(int mask, int flags);

		// Token: 0x06003CAF RID: 15535
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SaveGlobalFlags();

		// Token: 0x06003CB0 RID: 15536
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetGrantedPermissions(out PermissionSet granted, out PermissionSet denied, ref StackCrawlMark stackmark);

		// Token: 0x04001EFC RID: 7932
		private const int CheckExecutionRightsDisabledFlag = 256;

		// Token: 0x04001EFD RID: 7933
		private static Type securityPermissionType = null;

		// Token: 0x04001EFE RID: 7934
		private static SecurityPermission executionSecurityPermission = null;

		// Token: 0x04001EFF RID: 7935
		private static int checkExecution = -1;

		// Token: 0x04001F00 RID: 7936
		private static PolicyManager polmgr = new PolicyManager();

		// Token: 0x04001F01 RID: 7937
		private static int[][] s_BuiltInPermissionIndexMap = new int[][]
		{
			new int[] { 0, 10 },
			new int[] { 1, 11 },
			new int[] { 2, 12 },
			new int[] { 4, 13 },
			new int[] { 6, 14 },
			new int[] { 7, 9 }
		};

		// Token: 0x04001F02 RID: 7938
		private static CodeAccessPermission[] s_UnrestrictedSpecialPermissionMap = new CodeAccessPermission[]
		{
			new EnvironmentPermission(PermissionState.Unrestricted),
			new FileDialogPermission(PermissionState.Unrestricted),
			new FileIOPermission(PermissionState.Unrestricted),
			new ReflectionPermission(PermissionState.Unrestricted),
			new SecurityPermission(PermissionState.Unrestricted),
			new UIPermission(PermissionState.Unrestricted)
		};
	}
}
