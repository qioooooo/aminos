using System;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x0200072C RID: 1836
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal static class Direct
	{
		// Token: 0x0400320A RID: 12810
		public const int FILE_ACTION_ADDED = 1;

		// Token: 0x0400320B RID: 12811
		public const int FILE_ACTION_REMOVED = 2;

		// Token: 0x0400320C RID: 12812
		public const int FILE_ACTION_MODIFIED = 3;

		// Token: 0x0400320D RID: 12813
		public const int FILE_ACTION_RENAMED_OLD_NAME = 4;

		// Token: 0x0400320E RID: 12814
		public const int FILE_ACTION_RENAMED_NEW_NAME = 5;

		// Token: 0x0400320F RID: 12815
		public const int FILE_NOTIFY_CHANGE_FILE_NAME = 1;

		// Token: 0x04003210 RID: 12816
		public const int FILE_NOTIFY_CHANGE_DIR_NAME = 2;

		// Token: 0x04003211 RID: 12817
		public const int FILE_NOTIFY_CHANGE_NAME = 3;

		// Token: 0x04003212 RID: 12818
		public const int FILE_NOTIFY_CHANGE_ATTRIBUTES = 4;

		// Token: 0x04003213 RID: 12819
		public const int FILE_NOTIFY_CHANGE_SIZE = 8;

		// Token: 0x04003214 RID: 12820
		public const int FILE_NOTIFY_CHANGE_LAST_WRITE = 16;

		// Token: 0x04003215 RID: 12821
		public const int FILE_NOTIFY_CHANGE_LAST_ACCESS = 32;

		// Token: 0x04003216 RID: 12822
		public const int FILE_NOTIFY_CHANGE_CREATION = 64;

		// Token: 0x04003217 RID: 12823
		public const int FILE_NOTIFY_CHANGE_SECURITY = 256;
	}
}
