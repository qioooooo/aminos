using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000622 RID: 1570
	[ComVisible(true)]
	[Serializable]
	public sealed class IsolatedStorageFilePermission : IsolatedStoragePermission, IBuiltInPermission
	{
		// Token: 0x0600390F RID: 14607 RVA: 0x000C17D2 File Offset: 0x000C07D2
		public IsolatedStorageFilePermission(PermissionState state)
			: base(state)
		{
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x000C17DB File Offset: 0x000C07DB
		internal IsolatedStorageFilePermission(IsolatedStorageContainment UsageAllowed, long ExpirationDays, bool PermanentData)
			: base(UsageAllowed, ExpirationDays, PermanentData)
		{
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x000C17E8 File Offset: 0x000C07E8
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
			IsolatedStorageFilePermission isolatedStorageFilePermission = (IsolatedStorageFilePermission)target;
			if (base.IsUnrestricted() || isolatedStorageFilePermission.IsUnrestricted())
			{
				return new IsolatedStorageFilePermission(PermissionState.Unrestricted);
			}
			return new IsolatedStorageFilePermission(PermissionState.None)
			{
				m_userQuota = IsolatedStoragePermission.max(this.m_userQuota, isolatedStorageFilePermission.m_userQuota),
				m_machineQuota = IsolatedStoragePermission.max(this.m_machineQuota, isolatedStorageFilePermission.m_machineQuota),
				m_expirationDays = IsolatedStoragePermission.max(this.m_expirationDays, isolatedStorageFilePermission.m_expirationDays),
				m_permanentData = (this.m_permanentData || isolatedStorageFilePermission.m_permanentData),
				m_allowed = (IsolatedStorageContainment)IsolatedStoragePermission.max((long)this.m_allowed, (long)isolatedStorageFilePermission.m_allowed)
			};
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x000C18D4 File Offset: 0x000C08D4
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_userQuota == 0L && this.m_machineQuota == 0L && this.m_expirationDays == 0L && !this.m_permanentData && this.m_allowed == IsolatedStorageContainment.None;
			}
			bool flag;
			try
			{
				IsolatedStorageFilePermission isolatedStorageFilePermission = (IsolatedStorageFilePermission)target;
				if (isolatedStorageFilePermission.IsUnrestricted())
				{
					flag = true;
				}
				else
				{
					flag = isolatedStorageFilePermission.m_userQuota >= this.m_userQuota && isolatedStorageFilePermission.m_machineQuota >= this.m_machineQuota && isolatedStorageFilePermission.m_expirationDays >= this.m_expirationDays && (isolatedStorageFilePermission.m_permanentData || !this.m_permanentData) && isolatedStorageFilePermission.m_allowed >= this.m_allowed;
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return flag;
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x000C19C0 File Offset: 0x000C09C0
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
			IsolatedStorageFilePermission isolatedStorageFilePermission = (IsolatedStorageFilePermission)target;
			if (isolatedStorageFilePermission.IsUnrestricted())
			{
				return this.Copy();
			}
			if (base.IsUnrestricted())
			{
				return target.Copy();
			}
			IsolatedStorageFilePermission isolatedStorageFilePermission2 = new IsolatedStorageFilePermission(PermissionState.None);
			isolatedStorageFilePermission2.m_userQuota = IsolatedStoragePermission.min(this.m_userQuota, isolatedStorageFilePermission.m_userQuota);
			isolatedStorageFilePermission2.m_machineQuota = IsolatedStoragePermission.min(this.m_machineQuota, isolatedStorageFilePermission.m_machineQuota);
			isolatedStorageFilePermission2.m_expirationDays = IsolatedStoragePermission.min(this.m_expirationDays, isolatedStorageFilePermission.m_expirationDays);
			isolatedStorageFilePermission2.m_permanentData = this.m_permanentData && isolatedStorageFilePermission.m_permanentData;
			isolatedStorageFilePermission2.m_allowed = (IsolatedStorageContainment)IsolatedStoragePermission.min((long)this.m_allowed, (long)isolatedStorageFilePermission.m_allowed);
			if (isolatedStorageFilePermission2.m_userQuota == 0L && isolatedStorageFilePermission2.m_machineQuota == 0L && isolatedStorageFilePermission2.m_expirationDays == 0L && !isolatedStorageFilePermission2.m_permanentData && isolatedStorageFilePermission2.m_allowed == IsolatedStorageContainment.None)
			{
				return null;
			}
			return isolatedStorageFilePermission2;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x000C1AE0 File Offset: 0x000C0AE0
		public override IPermission Copy()
		{
			IsolatedStorageFilePermission isolatedStorageFilePermission = new IsolatedStorageFilePermission(PermissionState.Unrestricted);
			if (!base.IsUnrestricted())
			{
				isolatedStorageFilePermission.m_userQuota = this.m_userQuota;
				isolatedStorageFilePermission.m_machineQuota = this.m_machineQuota;
				isolatedStorageFilePermission.m_expirationDays = this.m_expirationDays;
				isolatedStorageFilePermission.m_permanentData = this.m_permanentData;
				isolatedStorageFilePermission.m_allowed = this.m_allowed;
			}
			return isolatedStorageFilePermission;
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x000C1B39 File Offset: 0x000C0B39
		int IBuiltInPermission.GetTokenIndex()
		{
			return IsolatedStorageFilePermission.GetTokenIndex();
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x000C1B40 File Offset: 0x000C0B40
		internal static int GetTokenIndex()
		{
			return 3;
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x000C1B43 File Offset: 0x000C0B43
		[ComVisible(false)]
		public override SecurityElement ToXml()
		{
			return base.ToXml("System.Security.Permissions.IsolatedStorageFilePermission");
		}
	}
}
