using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x0200063B RID: 1595
	[ComVisible(true)]
	[Serializable]
	public sealed class SecurityPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
	{
		// Token: 0x060039EF RID: 14831 RVA: 0x000C38F5 File Offset: 0x000C28F5
		public SecurityPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.SetUnrestricted(true);
				return;
			}
			if (state == PermissionState.None)
			{
				this.SetUnrestricted(false);
				this.Reset();
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x000C3929 File Offset: 0x000C2929
		public SecurityPermission(SecurityPermissionFlag flag)
		{
			this.VerifyAccess(flag);
			this.SetUnrestricted(false);
			this.m_flags = flag;
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x000C3946 File Offset: 0x000C2946
		private void SetUnrestricted(bool unrestricted)
		{
			if (unrestricted)
			{
				this.m_flags = SecurityPermissionFlag.AllFlags;
			}
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x000C3956 File Offset: 0x000C2956
		private void Reset()
		{
			this.m_flags = SecurityPermissionFlag.NoFlags;
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x000C396F File Offset: 0x000C296F
		// (set) Token: 0x060039F3 RID: 14835 RVA: 0x000C395F File Offset: 0x000C295F
		public SecurityPermissionFlag Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				this.VerifyAccess(value);
				this.m_flags = value;
			}
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x000C3978 File Offset: 0x000C2978
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_flags == SecurityPermissionFlag.NoFlags;
			}
			SecurityPermission securityPermission = target as SecurityPermission;
			if (securityPermission != null)
			{
				return (this.m_flags & ~securityPermission.m_flags) == SecurityPermissionFlag.NoFlags;
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x000C39E0 File Offset: 0x000C29E0
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (!base.VerifyType(target))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			SecurityPermission securityPermission = (SecurityPermission)target;
			if (securityPermission.IsUnrestricted() || this.IsUnrestricted())
			{
				return new SecurityPermission(PermissionState.Unrestricted);
			}
			SecurityPermissionFlag securityPermissionFlag = this.m_flags | securityPermission.m_flags;
			return new SecurityPermission(securityPermissionFlag);
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x000C3A64 File Offset: 0x000C2A64
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (!base.VerifyType(target))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			SecurityPermission securityPermission = (SecurityPermission)target;
			SecurityPermissionFlag securityPermissionFlag;
			if (securityPermission.IsUnrestricted())
			{
				if (this.IsUnrestricted())
				{
					return new SecurityPermission(PermissionState.Unrestricted);
				}
				securityPermissionFlag = this.m_flags;
			}
			else if (this.IsUnrestricted())
			{
				securityPermissionFlag = securityPermission.m_flags;
			}
			else
			{
				securityPermissionFlag = this.m_flags & securityPermission.m_flags;
			}
			if (securityPermissionFlag == SecurityPermissionFlag.NoFlags)
			{
				return null;
			}
			return new SecurityPermission(securityPermissionFlag);
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x000C3B02 File Offset: 0x000C2B02
		public override IPermission Copy()
		{
			if (this.IsUnrestricted())
			{
				return new SecurityPermission(PermissionState.Unrestricted);
			}
			return new SecurityPermission(this.m_flags);
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x000C3B1E File Offset: 0x000C2B1E
		public bool IsUnrestricted()
		{
			return this.m_flags == SecurityPermissionFlag.AllFlags;
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x000C3B30 File Offset: 0x000C2B30
		private void VerifyAccess(SecurityPermissionFlag type)
		{
			if ((type & ~(SecurityPermissionFlag.Assertion | SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.SkipVerification | SecurityPermissionFlag.Execution | SecurityPermissionFlag.ControlThread | SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy | SecurityPermissionFlag.SerializationFormatter | SecurityPermissionFlag.ControlDomainPolicy | SecurityPermissionFlag.ControlPrincipal | SecurityPermissionFlag.ControlAppDomain | SecurityPermissionFlag.RemotingConfiguration | SecurityPermissionFlag.Infrastructure | SecurityPermissionFlag.BindingRedirects)) != SecurityPermissionFlag.NoFlags)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[] { (int)type }));
			}
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x000C3B74 File Offset: 0x000C2B74
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = CodeAccessPermission.CreatePermissionElement(this, "System.Security.Permissions.SecurityPermission");
			if (!this.IsUnrestricted())
			{
				securityElement.AddAttribute("Flags", XMLUtil.BitFieldEnumToString(typeof(SecurityPermissionFlag), this.m_flags));
			}
			else
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x000C3BD0 File Offset: 0x000C2BD0
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.ValidateElement(esd, this);
			if (XMLUtil.IsUnrestricted(esd))
			{
				this.m_flags = SecurityPermissionFlag.AllFlags;
				return;
			}
			this.Reset();
			this.SetUnrestricted(false);
			string text = esd.Attribute("Flags");
			if (text != null)
			{
				this.m_flags = (SecurityPermissionFlag)Enum.Parse(typeof(SecurityPermissionFlag), text);
			}
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x000C3C2F File Offset: 0x000C2C2F
		int IBuiltInPermission.GetTokenIndex()
		{
			return SecurityPermission.GetTokenIndex();
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x000C3C36 File Offset: 0x000C2C36
		internal static int GetTokenIndex()
		{
			return 6;
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x000C3C39 File Offset: 0x000C2C39
		[SecurityPermission(SecurityAction.LinkDemand, SkipVerification = true)]
		internal static void MethodWithSkipVerificationLinkDemand()
		{
		}

		// Token: 0x04001DF4 RID: 7668
		private const string _strHeaderAssertion = "Assertion";

		// Token: 0x04001DF5 RID: 7669
		private const string _strHeaderUnmanagedCode = "UnmanagedCode";

		// Token: 0x04001DF6 RID: 7670
		private const string _strHeaderExecution = "Execution";

		// Token: 0x04001DF7 RID: 7671
		private const string _strHeaderSkipVerification = "SkipVerification";

		// Token: 0x04001DF8 RID: 7672
		private const string _strHeaderControlThread = "ControlThread";

		// Token: 0x04001DF9 RID: 7673
		private const string _strHeaderControlEvidence = "ControlEvidence";

		// Token: 0x04001DFA RID: 7674
		private const string _strHeaderControlPolicy = "ControlPolicy";

		// Token: 0x04001DFB RID: 7675
		private const string _strHeaderSerializationFormatter = "SerializationFormatter";

		// Token: 0x04001DFC RID: 7676
		private const string _strHeaderControlDomainPolicy = "ControlDomainPolicy";

		// Token: 0x04001DFD RID: 7677
		private const string _strHeaderControlPrincipal = "ControlPrincipal";

		// Token: 0x04001DFE RID: 7678
		private const string _strHeaderControlAppDomain = "ControlAppDomain";

		// Token: 0x04001DFF RID: 7679
		private SecurityPermissionFlag m_flags;
	}
}
