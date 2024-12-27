using System;

namespace System.Net
{
	// Token: 0x020004EA RID: 1258
	internal class BaseLoggingObject
	{
		// Token: 0x06002728 RID: 10024 RVA: 0x000A2384 File Offset: 0x000A1384
		internal BaseLoggingObject()
		{
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000A238C File Offset: 0x000A138C
		internal virtual void EnterFunc(string funcname)
		{
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000A238E File Offset: 0x000A138E
		internal virtual void LeaveFunc(string funcname)
		{
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000A2390 File Offset: 0x000A1390
		internal virtual void DumpArrayToConsole()
		{
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000A2392 File Offset: 0x000A1392
		internal virtual void PrintLine(string msg)
		{
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000A2394 File Offset: 0x000A1394
		internal virtual void DumpArray(bool shouldClose)
		{
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000A2396 File Offset: 0x000A1396
		internal virtual void DumpArrayToFile(bool shouldClose)
		{
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000A2398 File Offset: 0x000A1398
		internal virtual void Flush()
		{
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000A239A File Offset: 0x000A139A
		internal virtual void Flush(bool close)
		{
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000A239C File Offset: 0x000A139C
		internal virtual void LoggingMonitorTick()
		{
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000A239E File Offset: 0x000A139E
		internal virtual void Dump(byte[] buffer)
		{
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000A23A0 File Offset: 0x000A13A0
		internal virtual void Dump(byte[] buffer, int length)
		{
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000A23A2 File Offset: 0x000A13A2
		internal virtual void Dump(byte[] buffer, int offset, int length)
		{
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000A23A4 File Offset: 0x000A13A4
		internal virtual void Dump(IntPtr pBuffer, int offset, int length)
		{
		}
	}
}
