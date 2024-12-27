using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x0200059A RID: 1434
	[ComVisible(true)]
	[Serializable]
	public enum DriveType
	{
		// Token: 0x04001BD5 RID: 7125
		Unknown,
		// Token: 0x04001BD6 RID: 7126
		NoRootDirectory,
		// Token: 0x04001BD7 RID: 7127
		Removable,
		// Token: 0x04001BD8 RID: 7128
		Fixed,
		// Token: 0x04001BD9 RID: 7129
		Network,
		// Token: 0x04001BDA RID: 7130
		CDRom,
		// Token: 0x04001BDB RID: 7131
		Ram
	}
}
