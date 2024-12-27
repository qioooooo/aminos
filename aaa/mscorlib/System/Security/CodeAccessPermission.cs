using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;
using System.Threading;

namespace System.Security
{
	// Token: 0x02000610 RID: 1552
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, ControlEvidence = true, ControlPolicy = true)]
	[Serializable]
	public abstract class CodeAccessPermission : IPermission, ISecurityEncodable, IStackWalk
	{
		// Token: 0x06003859 RID: 14425 RVA: 0x000BEBAC File Offset: 0x000BDBAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertAssert()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertAssert(ref stackCrawlMark);
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000BEBC4 File Offset: 0x000BDBC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertDeny()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertDeny(ref stackCrawlMark);
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x000BEBDC File Offset: 0x000BDBDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertPermitOnly()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertPermitOnly(ref stackCrawlMark);
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x000BEBF4 File Offset: 0x000BDBF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertAll()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertAll(ref stackCrawlMark);
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000BEC0C File Offset: 0x000BDC0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Demand()
		{
			if (!this.CheckDemand(null))
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCallersCaller;
				CodeAccessSecurityEngine.Check(this, ref stackCrawlMark);
			}
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x000BEC2C File Offset: 0x000BDC2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void DemandInternal(PermissionType permissionType)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCallersCaller;
			CodeAccessSecurityEngine.SpecialDemand(permissionType, ref stackCrawlMark);
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x000BEC44 File Offset: 0x000BDC44
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Assert()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			CodeAccessSecurityEngine.Assert(this, ref stackCrawlMark);
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x000BEC5C File Offset: 0x000BDC5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void AssertAllPossible()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.AssertAllPossible(ref stackCrawlMark);
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x000BEC74 File Offset: 0x000BDC74
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Deny()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			CodeAccessSecurityEngine.Deny(this, ref stackCrawlMark);
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x000BEC8C File Offset: 0x000BDC8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void PermitOnly()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			CodeAccessSecurityEngine.PermitOnly(this, ref stackCrawlMark);
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x000BECA3 File Offset: 0x000BDCA3
		public virtual IPermission Union(IPermission other)
		{
			if (other == null)
			{
				return this.Copy();
			}
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SecurityPermissionUnion"));
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x000BECC0 File Offset: 0x000BDCC0
		internal static SecurityElement CreatePermissionElement(IPermission perm, string permname)
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			XMLUtil.AddClassAttribute(securityElement, perm.GetType(), permname);
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x000BECF8 File Offset: 0x000BDCF8
		internal static void ValidateElement(SecurityElement elem, IPermission perm)
		{
			if (elem == null)
			{
				throw new ArgumentNullException("elem");
			}
			if (!XMLUtil.IsPermissionElement(perm, elem))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotAPermissionElement"));
			}
			string text = elem.Attribute("version");
			if (text != null && !text.Equals("1"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidXMLBadVersion"));
			}
		}

		// Token: 0x06003866 RID: 14438
		public abstract SecurityElement ToXml();

		// Token: 0x06003867 RID: 14439
		public abstract void FromXml(SecurityElement elem);

		// Token: 0x06003868 RID: 14440 RVA: 0x000BED58 File Offset: 0x000BDD58
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x000BED65 File Offset: 0x000BDD65
		internal bool VerifyType(IPermission perm)
		{
			return perm != null && perm.GetType() == base.GetType();
		}

		// Token: 0x0600386A RID: 14442
		public abstract IPermission Copy();

		// Token: 0x0600386B RID: 14443
		public abstract IPermission Intersect(IPermission target);

		// Token: 0x0600386C RID: 14444
		public abstract bool IsSubsetOf(IPermission target);

		// Token: 0x0600386D RID: 14445 RVA: 0x000BED7C File Offset: 0x000BDD7C
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			IPermission permission = obj as IPermission;
			if (obj != null && permission == null)
			{
				return false;
			}
			try
			{
				if (!this.IsSubsetOf(permission))
				{
					return false;
				}
				if (permission != null && !permission.IsSubsetOf(this))
				{
					return false;
				}
			}
			catch (ArgumentException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x000BEDD0 File Offset: 0x000BDDD0
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x000BEDD8 File Offset: 0x000BDDD8
		internal bool CheckDemand(CodeAccessPermission grant)
		{
			return this.IsSubsetOf(grant);
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x000BEDE1 File Offset: 0x000BDDE1
		internal bool CheckPermitOnly(CodeAccessPermission permitted)
		{
			return this.IsSubsetOf(permitted);
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x000BEDEC File Offset: 0x000BDDEC
		internal bool CheckDeny(CodeAccessPermission denied)
		{
			IPermission permission = this.Intersect(denied);
			return permission == null || permission.IsSubsetOf(null);
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x000BEE0D File Offset: 0x000BDE0D
		internal bool CheckAssert(CodeAccessPermission asserted)
		{
			return this.IsSubsetOf(asserted);
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x000BEE16 File Offset: 0x000BDE16
		internal bool CanUnrestrictedOverride()
		{
			return CodeAccessPermission.CanUnrestrictedOverride(this);
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x000BEE1E File Offset: 0x000BDE1E
		internal static bool CanUnrestrictedOverride(IPermission ip)
		{
			return CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() || ip is IUnrestrictedPermission;
		}
	}
}
