using System;
using System.Globalization;

namespace System.Security.Permissions
{
	// Token: 0x020002C2 RID: 706
	[Serializable]
	public sealed class StorePermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06001816 RID: 6166 RVA: 0x00053170 File Offset: 0x00052170
		public StorePermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.m_flags = StorePermissionFlags.AllFlags;
				return;
			}
			if (state == PermissionState.None)
			{
				this.m_flags = StorePermissionFlags.NoFlags;
				return;
			}
			throw new ArgumentException(SR.GetString("Argument_InvalidPermissionState"));
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x000531A2 File Offset: 0x000521A2
		public StorePermission(StorePermissionFlags flag)
		{
			StorePermission.VerifyFlags(flag);
			this.m_flags = flag;
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001819 RID: 6169 RVA: 0x000531C6 File Offset: 0x000521C6
		// (set) Token: 0x06001818 RID: 6168 RVA: 0x000531B7 File Offset: 0x000521B7
		public StorePermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				StorePermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x000531CE File Offset: 0x000521CE
		public bool IsUnrestricted()
		{
			return this.m_flags == StorePermissionFlags.AllFlags;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x000531E0 File Offset: 0x000521E0
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			IPermission permission;
			try
			{
				StorePermission storePermission = (StorePermission)target;
				StorePermissionFlags storePermissionFlags = this.m_flags | storePermission.m_flags;
				if (storePermissionFlags == StorePermissionFlags.NoFlags)
				{
					permission = null;
				}
				else
				{
					permission = new StorePermission(storePermissionFlags);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return permission;
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x00053260 File Offset: 0x00052260
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_flags == StorePermissionFlags.NoFlags;
			}
			bool flag;
			try
			{
				StorePermission storePermission = (StorePermission)target;
				StorePermissionFlags flags = this.m_flags;
				StorePermissionFlags flags2 = storePermission.m_flags;
				flag = (flags & flags2) == flags;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return flag;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x000532E0 File Offset: 0x000522E0
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			IPermission permission;
			try
			{
				StorePermission storePermission = (StorePermission)target;
				StorePermissionFlags storePermissionFlags = storePermission.m_flags & this.m_flags;
				if (storePermissionFlags == StorePermissionFlags.NoFlags)
				{
					permission = null;
				}
				else
				{
					permission = new StorePermission(storePermissionFlags);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return permission;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0005335C File Offset: 0x0005235C
		public override IPermission Copy()
		{
			if (this.Flags == StorePermissionFlags.NoFlags)
			{
				return null;
			}
			return new StorePermission(this.m_flags);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00053374 File Offset: 0x00052374
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (!this.IsUnrestricted())
			{
				securityElement.AddAttribute("Flags", this.m_flags.ToString());
			}
			else
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00053410 File Offset: 0x00052410
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			string text = securityElement.Attribute("class");
			if (text == null || text.IndexOf(base.GetType().FullName, StringComparison.Ordinal) == -1)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidClassAttribute"), "securityElement");
			}
			string text2 = securityElement.Attribute("Unrestricted");
			if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_flags = StorePermissionFlags.AllFlags;
				return;
			}
			this.m_flags = StorePermissionFlags.NoFlags;
			string text3 = securityElement.Attribute("Flags");
			if (text3 != null)
			{
				StorePermissionFlags storePermissionFlags = (StorePermissionFlags)Enum.Parse(typeof(StorePermissionFlags), text3);
				StorePermission.VerifyFlags(storePermissionFlags);
				this.m_flags = storePermissionFlags;
			}
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x000534C8 File Offset: 0x000524C8
		internal static void VerifyFlags(StorePermissionFlags flags)
		{
			if ((flags & ~(StorePermissionFlags.CreateStore | StorePermissionFlags.DeleteStore | StorePermissionFlags.EnumerateStores | StorePermissionFlags.OpenStore | StorePermissionFlags.AddToStore | StorePermissionFlags.RemoveFromStore | StorePermissionFlags.EnumerateCertificates)) != StorePermissionFlags.NoFlags)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { (int)flags }));
			}
		}

		// Token: 0x04001621 RID: 5665
		private StorePermissionFlags m_flags;
	}
}
