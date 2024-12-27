using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000771 RID: 1905
	[Serializable]
	public class PerformanceCounterPermissionEntry
	{
		// Token: 0x06003A96 RID: 14998 RVA: 0x000F9418 File Offset: 0x000F8418
		public PerformanceCounterPermissionEntry(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if ((permissionAccess & (PerformanceCounterPermissionAccess)(-8)) != PerformanceCounterPermissionAccess.None)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "permissionAccess", permissionAccess }));
			}
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "MachineName", machineName }));
			}
			this.permissionAccess = permissionAccess;
			this.machineName = machineName;
			this.categoryName = categoryName;
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x000F94B7 File Offset: 0x000F84B7
		internal PerformanceCounterPermissionEntry(ResourcePermissionBaseEntry baseEntry)
		{
			this.permissionAccess = (PerformanceCounterPermissionAccess)baseEntry.PermissionAccess;
			this.machineName = baseEntry.PermissionAccessPath[0];
			this.categoryName = baseEntry.PermissionAccessPath[1];
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06003A98 RID: 15000 RVA: 0x000F94E7 File Offset: 0x000F84E7
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06003A99 RID: 15001 RVA: 0x000F94EF File Offset: 0x000F84EF
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06003A9A RID: 15002 RVA: 0x000F94F7 File Offset: 0x000F84F7
		public PerformanceCounterPermissionAccess PermissionAccess
		{
			get
			{
				return this.permissionAccess;
			}
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x000F9500 File Offset: 0x000F8500
		internal ResourcePermissionBaseEntry GetBaseEntry()
		{
			return new ResourcePermissionBaseEntry((int)this.PermissionAccess, new string[] { this.MachineName, this.CategoryName });
		}

		// Token: 0x0400334E RID: 13134
		private string categoryName;

		// Token: 0x0400334F RID: 13135
		private string machineName;

		// Token: 0x04003350 RID: 13136
		private PerformanceCounterPermissionAccess permissionAccess;
	}
}
