using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Util;
using System.Text;
using System.Threading;

namespace System.Security
{
	// Token: 0x0200066A RID: 1642
	internal class PolicyManager
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06003BDD RID: 15325 RVA: 0x000CD71C File Offset: 0x000CC71C
		private IList PolicyLevels
		{
			get
			{
				if (this.m_policyLevels == null)
				{
					ArrayList arrayList = new ArrayList();
					string locationFromType = PolicyLevel.GetLocationFromType(PolicyLevelType.Enterprise);
					arrayList.Add(new PolicyLevel(PolicyLevelType.Enterprise, locationFromType, ConfigId.EnterprisePolicyLevel));
					string locationFromType2 = PolicyLevel.GetLocationFromType(PolicyLevelType.Machine);
					arrayList.Add(new PolicyLevel(PolicyLevelType.Machine, locationFromType2, ConfigId.MachinePolicyLevel));
					if (Config.UserDirectory != null)
					{
						string locationFromType3 = PolicyLevel.GetLocationFromType(PolicyLevelType.User);
						arrayList.Add(new PolicyLevel(PolicyLevelType.User, locationFromType3, ConfigId.UserPolicyLevel));
					}
					Interlocked.CompareExchange(ref this.m_policyLevels, arrayList, null);
				}
				return this.m_policyLevels as ArrayList;
			}
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x000CD799 File Offset: 0x000CC799
		internal PolicyManager()
		{
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x000CD7A1 File Offset: 0x000CC7A1
		internal void AddLevel(PolicyLevel level)
		{
			this.PolicyLevels.Add(level);
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x000CD7B0 File Offset: 0x000CC7B0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		internal IEnumerator PolicyHierarchy()
		{
			return this.PolicyLevels.GetEnumerator();
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x000CD7C0 File Offset: 0x000CC7C0
		internal PermissionSet Resolve(Evidence evidence)
		{
			if (!PolicyManager.IsGacAssembly(evidence))
			{
				HostSecurityManager hostSecurityManager = AppDomain.CurrentDomain.HostSecurityManager;
				if ((hostSecurityManager.Flags & HostSecurityManagerOptions.HostResolvePolicy) == HostSecurityManagerOptions.HostResolvePolicy)
				{
					return hostSecurityManager.ResolvePolicy(evidence);
				}
			}
			return this.ResolveHelper(evidence);
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x000CD7FC File Offset: 0x000CC7FC
		internal PermissionSet ResolveHelper(Evidence evidence)
		{
			PermissionSet permissionSet;
			if (PolicyManager.IsGacAssembly(evidence))
			{
				permissionSet = new PermissionSet(PermissionState.Unrestricted);
			}
			else
			{
				ApplicationTrust applicationTrust = AppDomain.CurrentDomain.ApplicationTrust;
				if (applicationTrust != null)
				{
					if (PolicyManager.IsFullTrust(evidence, applicationTrust))
					{
						permissionSet = new PermissionSet(PermissionState.Unrestricted);
					}
					else
					{
						permissionSet = applicationTrust.DefaultGrantSet.PermissionSet;
					}
				}
				else
				{
					permissionSet = this.CodeGroupResolve(evidence, false);
				}
			}
			return permissionSet;
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x000CD854 File Offset: 0x000CC854
		internal PermissionSet CodeGroupResolve(Evidence evidence, bool systemPolicy)
		{
			PermissionSet permissionSet = null;
			IEnumerator enumerator = this.PolicyLevels.GetEnumerator();
			char[] array = PolicyManager.MakeEvidenceArray(evidence, false);
			int count = evidence.Count;
			bool flag = AppDomain.CurrentDomain.GetData("IgnoreSystemPolicy") != null;
			bool flag2 = false;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PolicyLevel policyLevel = (PolicyLevel)obj;
				if (systemPolicy)
				{
					if (policyLevel.Type == PolicyLevelType.AppDomain)
					{
						continue;
					}
				}
				else if (flag && policyLevel.Type != PolicyLevelType.AppDomain)
				{
					continue;
				}
				PolicyStatement policyStatement = policyLevel.Resolve(evidence, count, array);
				if (permissionSet == null)
				{
					permissionSet = policyStatement.PermissionSet;
				}
				else
				{
					permissionSet.InplaceIntersect(policyStatement.GetPermissionSetNoCopy());
				}
				if (permissionSet == null || permissionSet.FastIsEmpty())
				{
					break;
				}
				if ((policyStatement.Attributes & PolicyStatementAttribute.LevelFinal) == PolicyStatementAttribute.LevelFinal)
				{
					if (policyLevel.Type != PolicyLevelType.AppDomain)
					{
						flag2 = true;
						break;
					}
					break;
				}
			}
			if (permissionSet != null && flag2)
			{
				PolicyLevel policyLevel2 = null;
				for (int i = this.PolicyLevels.Count - 1; i >= 0; i--)
				{
					PolicyLevel policyLevel = (PolicyLevel)this.PolicyLevels[i];
					if (policyLevel.Type == PolicyLevelType.AppDomain)
					{
						policyLevel2 = policyLevel;
						break;
					}
				}
				if (policyLevel2 != null)
				{
					PolicyStatement policyStatement = policyLevel2.Resolve(evidence, count, array);
					permissionSet.InplaceIntersect(policyStatement.GetPermissionSetNoCopy());
				}
			}
			if (permissionSet == null)
			{
				permissionSet = new PermissionSet(PermissionState.None);
			}
			if (!CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() || !permissionSet.IsUnrestricted())
			{
				IEnumerator hostEnumerator = evidence.GetHostEnumerator();
				while (hostEnumerator.MoveNext())
				{
					object obj2 = hostEnumerator.Current;
					IIdentityPermissionFactory identityPermissionFactory = obj2 as IIdentityPermissionFactory;
					if (identityPermissionFactory != null)
					{
						IPermission permission = identityPermissionFactory.CreateIdentityPermission(evidence);
						if (permission != null)
						{
							permissionSet.AddPermission(permission);
						}
					}
				}
			}
			permissionSet.IgnoreTypeLoadFailures = true;
			return permissionSet;
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x000CD9DA File Offset: 0x000CC9DA
		internal static bool IsGacAssembly(Evidence evidence)
		{
			return new GacMembershipCondition().Check(evidence);
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x000CD9E8 File Offset: 0x000CC9E8
		private static bool IsFullTrust(Evidence evidence, ApplicationTrust appTrust)
		{
			if (appTrust == null)
			{
				return false;
			}
			StrongName[] fullTrustAssemblies = appTrust.FullTrustAssemblies;
			if (fullTrustAssemblies != null)
			{
				for (int i = 0; i < fullTrustAssemblies.Length; i++)
				{
					if (fullTrustAssemblies[i] != null)
					{
						StrongNameMembershipCondition strongNameMembershipCondition = new StrongNameMembershipCondition(fullTrustAssemblies[i].PublicKey, fullTrustAssemblies[i].Name, fullTrustAssemblies[i].Version);
						object obj = null;
						if (((IReportMatchMembershipCondition)strongNameMembershipCondition).Check(evidence, out obj))
						{
							IDelayEvaluatedEvidence delayEvaluatedEvidence = obj as IDelayEvaluatedEvidence;
							if (obj != null)
							{
								delayEvaluatedEvidence.MarkUsed();
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x000CDA58 File Offset: 0x000CCA58
		internal IEnumerator ResolveCodeGroups(Evidence evidence)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.PolicyLevels)
			{
				CodeGroup codeGroup = ((PolicyLevel)obj).ResolveMatchingCodeGroups(evidence);
				if (codeGroup != null)
				{
					arrayList.Add(codeGroup);
				}
			}
			return arrayList.GetEnumerator(0, arrayList.Count);
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x000CDAAB File Offset: 0x000CCAAB
		internal static PolicyStatement ResolveCodeGroup(CodeGroup codeGroup, Evidence evidence)
		{
			if (codeGroup.GetType().Assembly != typeof(UnionCodeGroup).Assembly)
			{
				evidence.MarkAllEvidenceAsUsed();
			}
			return codeGroup.Resolve(evidence);
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x000CDAD8 File Offset: 0x000CCAD8
		internal static bool CheckMembershipCondition(IMembershipCondition membershipCondition, Evidence evidence, out object usedEvidence)
		{
			IReportMatchMembershipCondition reportMatchMembershipCondition = membershipCondition as IReportMatchMembershipCondition;
			if (reportMatchMembershipCondition != null)
			{
				return reportMatchMembershipCondition.Check(evidence, out usedEvidence);
			}
			usedEvidence = null;
			evidence.MarkAllEvidenceAsUsed();
			return membershipCondition.Check(evidence);
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x000CDB08 File Offset: 0x000CCB08
		internal void Save()
		{
			this.EncodeLevel(Environment.GetResourceString("Policy_PL_Enterprise"));
			this.EncodeLevel(Environment.GetResourceString("Policy_PL_Machine"));
			this.EncodeLevel(Environment.GetResourceString("Policy_PL_User"));
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x000CDB3C File Offset: 0x000CCB3C
		private void EncodeLevel(string label)
		{
			for (int i = 0; i < this.PolicyLevels.Count; i++)
			{
				PolicyLevel policyLevel = (PolicyLevel)this.PolicyLevels[i];
				if (policyLevel.Label.Equals(label))
				{
					PolicyManager.EncodeLevel(policyLevel);
					return;
				}
			}
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x000CDB88 File Offset: 0x000CCB88
		internal static void EncodeLevel(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("configuration");
			SecurityElement securityElement2 = new SecurityElement("mscorlib");
			SecurityElement securityElement3 = new SecurityElement("security");
			SecurityElement securityElement4 = new SecurityElement("policy");
			securityElement.AddChild(securityElement2);
			securityElement2.AddChild(securityElement3);
			securityElement3.AddChild(securityElement4);
			securityElement4.AddChild(level.ToXml());
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				Encoding utf = Encoding.UTF8;
				SecurityElement securityElement5 = new SecurityElement("xml");
				securityElement5.m_type = SecurityElementType.Format;
				securityElement5.AddAttribute("version", "1.0");
				securityElement5.AddAttribute("encoding", utf.WebName);
				stringBuilder.Append(securityElement5.ToString());
				stringBuilder.Append(securityElement.ToString());
				byte[] bytes = utf.GetBytes(stringBuilder.ToString());
				if (level.Path == null || !Config.SaveDataByte(level.Path, bytes, 0, bytes.Length))
				{
					throw new PolicyException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_UnableToSave"), new object[] { level.Label }));
				}
			}
			catch (Exception ex)
			{
				if (ex is PolicyException)
				{
					throw ex;
				}
				throw new PolicyException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_UnableToSave"), new object[] { level.Label }), ex);
			}
			catch
			{
				throw new PolicyException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_UnableToSave"), new object[] { level.Label }));
			}
			Config.ResetCacheData(level.ConfigId);
			if (PolicyManager.CanUseQuickCache(level.RootCodeGroup))
			{
				Config.SetQuickCache(level.ConfigId, PolicyManager.GenerateQuickCache(level));
			}
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x000CDD54 File Offset: 0x000CCD54
		internal static bool CanUseQuickCache(CodeGroup group)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add(group);
			for (int i = 0; i < arrayList.Count; i++)
			{
				group = (CodeGroup)arrayList[i];
				IUnionSemanticCodeGroup unionSemanticCodeGroup = group as IUnionSemanticCodeGroup;
				if (unionSemanticCodeGroup == null)
				{
					return false;
				}
				if (!PolicyManager.TestPolicyStatement(group.PolicyStatement))
				{
					return false;
				}
				IMembershipCondition membershipCondition = group.MembershipCondition;
				if (membershipCondition != null && !(membershipCondition is IConstantMembershipCondition))
				{
					return false;
				}
				IList children = group.Children;
				if (children != null && children.Count > 0)
				{
					foreach (object obj in children)
					{
						arrayList.Add(obj);
					}
				}
			}
			return true;
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x000CDDF9 File Offset: 0x000CCDF9
		private static bool TestPolicyStatement(PolicyStatement policy)
		{
			return policy == null || (policy.Attributes & PolicyStatementAttribute.Exclusive) == PolicyStatementAttribute.Nothing;
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x000CDE0C File Offset: 0x000CCE0C
		private static QuickCacheEntryType GenerateQuickCache(PolicyLevel level)
		{
			QuickCacheEntryType[] array = new QuickCacheEntryType[]
			{
				QuickCacheEntryType.FullTrustZoneMyComputer,
				QuickCacheEntryType.FullTrustZoneIntranet,
				QuickCacheEntryType.FullTrustZoneInternet,
				QuickCacheEntryType.FullTrustZoneTrusted,
				QuickCacheEntryType.FullTrustZoneUntrusted
			};
			QuickCacheEntryType quickCacheEntryType = (QuickCacheEntryType)0;
			Evidence evidence = new Evidence();
			try
			{
				PermissionSet permissionSet = level.Resolve(evidence).PermissionSet;
				if (permissionSet.IsUnrestricted())
				{
					quickCacheEntryType |= QuickCacheEntryType.FullTrustAll;
				}
			}
			catch (PolicyException)
			{
			}
			Array values = Enum.GetValues(typeof(SecurityZone));
			for (int i = 0; i < values.Length; i++)
			{
				if ((SecurityZone)values.GetValue(i) != SecurityZone.NoZone)
				{
					Evidence evidence2 = new Evidence();
					evidence2.AddHost(new Zone((SecurityZone)values.GetValue(i)));
					try
					{
						PermissionSet permissionSet2 = level.Resolve(evidence2).PermissionSet;
						if (permissionSet2.IsUnrestricted())
						{
							quickCacheEntryType |= array[i];
						}
					}
					catch (PolicyException)
					{
					}
				}
			}
			return quickCacheEntryType;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x000CDF1C File Offset: 0x000CCF1C
		internal static char[] MakeEvidenceArray(Evidence evidence, bool verbose)
		{
			IEnumerator enumerator = evidence.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				IBuiltInEvidence builtInEvidence = obj as IBuiltInEvidence;
				if (builtInEvidence == null)
				{
					return null;
				}
				num += builtInEvidence.GetRequiredSize(verbose);
			}
			enumerator.Reset();
			char[] array = new char[num];
			int num2 = 0;
			while (enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				num2 = ((IBuiltInEvidence)obj2).OutputToBuffer(array, num2, verbose);
			}
			return array;
		}

		// Token: 0x04001EB4 RID: 7860
		private object m_policyLevels;
	}
}
