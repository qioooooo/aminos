using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000081 RID: 129
	internal sealed class TempEnvironment
	{
		// Token: 0x060006F0 RID: 1776 RVA: 0x00070388 File Offset: 0x0006F788
		private TempEnvironment()
		{
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0007039C File Offset: 0x0006F79C
		private static void Initialize()
		{
			lock (TempEnvironment.locked)
			{
				if (!TempEnvironment.isInitialized)
				{
					bool flag = false;
					OCI.MODE mode = OCI.MODE.OCI_THREADED | OCI.MODE.OCI_OBJECT;
					OCI.DetermineClientVersion();
					TempEnvironment.environmentHandle = new OciEnvironmentHandle(mode, flag);
					TempEnvironment.availableErrorHandle = new OciErrorHandle(TempEnvironment.environmentHandle);
					TempEnvironment.isInitialized = true;
				}
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00070410 File Offset: 0x0006F810
		internal static OciErrorHandle GetErrorHandle()
		{
			if (!TempEnvironment.isInitialized)
			{
				TempEnvironment.Initialize();
			}
			return TempEnvironment.availableErrorHandle;
		}

		// Token: 0x040004ED RID: 1261
		private static OciEnvironmentHandle environmentHandle;

		// Token: 0x040004EE RID: 1262
		private static OciErrorHandle availableErrorHandle;

		// Token: 0x040004EF RID: 1263
		private static volatile bool isInitialized;

		// Token: 0x040004F0 RID: 1264
		private static object locked = new object();
	}
}
