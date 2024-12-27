using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000147 RID: 327
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public static class Monitor
	{
		// Token: 0x06001240 RID: 4672
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Enter(object obj);

		// Token: 0x06001241 RID: 4673
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ReliableEnter(object obj, ref bool tookLock);

		// Token: 0x06001242 RID: 4674
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Exit(object obj);

		// Token: 0x06001243 RID: 4675 RVA: 0x00033283 File Offset: 0x00032283
		public static bool TryEnter(object obj)
		{
			return Monitor.TryEnterTimeout(obj, 0);
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0003328C File Offset: 0x0003228C
		public static bool TryEnter(object obj, int millisecondsTimeout)
		{
			return Monitor.TryEnterTimeout(obj, millisecondsTimeout);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00033298 File Offset: 0x00032298
		public static bool TryEnter(object obj, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return Monitor.TryEnterTimeout(obj, (int)num);
		}

		// Token: 0x06001246 RID: 4678
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TryEnterTimeout(object obj, int timeout);

		// Token: 0x06001247 RID: 4679
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ObjWait(bool exitContext, int millisecondsTimeout, object obj);

		// Token: 0x06001248 RID: 4680 RVA: 0x000332D9 File Offset: 0x000322D9
		public static bool Wait(object obj, int millisecondsTimeout, bool exitContext)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return Monitor.ObjWait(exitContext, millisecondsTimeout, obj);
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x000332F4 File Offset: 0x000322F4
		public static bool Wait(object obj, TimeSpan timeout, bool exitContext)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return Monitor.Wait(obj, (int)num, exitContext);
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00033336 File Offset: 0x00032336
		public static bool Wait(object obj, int millisecondsTimeout)
		{
			return Monitor.Wait(obj, millisecondsTimeout, false);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00033340 File Offset: 0x00032340
		public static bool Wait(object obj, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
			}
			return Monitor.Wait(obj, (int)num, false);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00033382 File Offset: 0x00032382
		public static bool Wait(object obj)
		{
			return Monitor.Wait(obj, -1, false);
		}

		// Token: 0x0600124D RID: 4685
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ObjPulse(object obj);

		// Token: 0x0600124E RID: 4686 RVA: 0x0003338C File Offset: 0x0003238C
		public static void Pulse(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Monitor.ObjPulse(obj);
		}

		// Token: 0x0600124F RID: 4687
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ObjPulseAll(object obj);

		// Token: 0x06001250 RID: 4688 RVA: 0x000333A2 File Offset: 0x000323A2
		public static void PulseAll(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Monitor.ObjPulseAll(obj);
		}
	}
}
