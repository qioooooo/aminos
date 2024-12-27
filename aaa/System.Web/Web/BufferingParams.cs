using System;

namespace System.Web
{
	// Token: 0x02000095 RID: 149
	internal static class BufferingParams
	{
		// Token: 0x060007C4 RID: 1988 RVA: 0x00022CC8 File Offset: 0x00021CC8
		static BufferingParams()
		{
			Version version = Environment.OSVersion.Version;
			if (version.Major == 6 && version.Minor > 0)
			{
				BufferingParams.INTEGRATED_MODE_BUFFER_SIZE = 16384 - 4 * IntPtr.Size - 112;
				return;
			}
			BufferingParams.INTEGRATED_MODE_BUFFER_SIZE = 16384 - 4 * IntPtr.Size;
		}

		// Token: 0x04001166 RID: 4454
		internal const int OUTPUT_BUFFER_SIZE = 31744;

		// Token: 0x04001167 RID: 4455
		internal const int MAX_FREE_BYTES_TO_CACHE = 4096;

		// Token: 0x04001168 RID: 4456
		internal const int MAX_FREE_OUTPUT_BUFFERS = 64;

		// Token: 0x04001169 RID: 4457
		internal const int CHAR_BUFFER_SIZE = 1024;

		// Token: 0x0400116A RID: 4458
		internal const int MAX_FREE_CHAR_BUFFERS = 64;

		// Token: 0x0400116B RID: 4459
		internal const int MAX_BYTES_TO_COPY = 128;

		// Token: 0x0400116C RID: 4460
		internal const int MAX_RESOURCE_BYTES_TO_COPY = 4096;

		// Token: 0x0400116D RID: 4461
		internal static readonly int INTEGRATED_MODE_BUFFER_SIZE;
	}
}
