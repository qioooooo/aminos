using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000F2 RID: 242
	public class TopLevelName
	{
		// Token: 0x0600073E RID: 1854 RVA: 0x00025E7A File Offset: 0x00024E7A
		internal TopLevelName(int flag, LSA_UNICODE_STRING val, LARGE_INTEGER time)
		{
			this.status = (TopLevelNameStatus)flag;
			this.name = Marshal.PtrToStringUni(val.Buffer, (int)(val.Length / 2));
			this.time = time;
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00025EA9 File Offset: 0x00024EA9
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00025EB1 File Offset: 0x00024EB1
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x00025EB9 File Offset: 0x00024EB9
		public TopLevelNameStatus Status
		{
			get
			{
				return this.status;
			}
			set
			{
				if (value != TopLevelNameStatus.Enabled && value != TopLevelNameStatus.NewlyCreated && value != TopLevelNameStatus.AdminDisabled && value != TopLevelNameStatus.ConflictDisabled)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TopLevelNameStatus));
				}
				this.status = value;
			}
		}

		// Token: 0x040005D0 RID: 1488
		private string name;

		// Token: 0x040005D1 RID: 1489
		private TopLevelNameStatus status;

		// Token: 0x040005D2 RID: 1490
		internal LARGE_INTEGER time;
	}
}
