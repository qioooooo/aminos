using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Util;
using System.Threading;

namespace System
{
	// Token: 0x02000112 RID: 274
	internal sealed class SharedStatics
	{
		// Token: 0x06000FFA RID: 4090 RVA: 0x0002D974 File Offset: 0x0002C974
		private SharedStatics()
		{
			this._Remoting_Identity_IDGuid = null;
			this._Remoting_Identity_IDSeqNum = 64;
			this._maker = null;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0002D994 File Offset: 0x0002C994
		public static string Remoting_Identity_IDGuid
		{
			get
			{
				if (SharedStatics._sharedStatics._Remoting_Identity_IDGuid == null)
				{
					bool flag = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.ReliableEnter(SharedStatics._sharedStatics, ref flag);
						if (SharedStatics._sharedStatics._Remoting_Identity_IDGuid == null)
						{
							SharedStatics._sharedStatics._Remoting_Identity_IDGuid = Guid.NewGuid().ToString().Replace('-', '_');
						}
					}
					finally
					{
						if (flag)
						{
							Monitor.Exit(SharedStatics._sharedStatics);
						}
					}
				}
				return SharedStatics._sharedStatics._Remoting_Identity_IDGuid;
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0002DA1C File Offset: 0x0002CA1C
		public static Tokenizer.StringMaker GetSharedStringMaker()
		{
			Tokenizer.StringMaker stringMaker = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(SharedStatics._sharedStatics, ref flag);
				if (SharedStatics._sharedStatics._maker != null)
				{
					stringMaker = SharedStatics._sharedStatics._maker;
					SharedStatics._sharedStatics._maker = null;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(SharedStatics._sharedStatics);
				}
			}
			if (stringMaker == null)
			{
				stringMaker = new Tokenizer.StringMaker();
			}
			return stringMaker;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0002DA8C File Offset: 0x0002CA8C
		public static void ReleaseSharedStringMaker(ref Tokenizer.StringMaker maker)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(SharedStatics._sharedStatics, ref flag);
				SharedStatics._sharedStatics._maker = maker;
				maker = null;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(SharedStatics._sharedStatics);
				}
			}
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0002DADC File Offset: 0x0002CADC
		internal static int Remoting_Identity_GetNextSeqNum()
		{
			return Interlocked.Increment(ref SharedStatics._sharedStatics._Remoting_Identity_IDSeqNum);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0002DAED File Offset: 0x0002CAED
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static long AddMemoryFailPointReservation(long size)
		{
			return Interlocked.Add(ref SharedStatics._sharedStatics._memFailPointReservedMemory, size);
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x0002DAFF File Offset: 0x0002CAFF
		internal static ulong MemoryFailPointReservedMemory
		{
			get
			{
				return (ulong)SharedStatics._sharedStatics._memFailPointReservedMemory;
			}
		}

		// Token: 0x04000527 RID: 1319
		internal static SharedStatics _sharedStatics;

		// Token: 0x04000528 RID: 1320
		private string _Remoting_Identity_IDGuid;

		// Token: 0x04000529 RID: 1321
		private Tokenizer.StringMaker _maker;

		// Token: 0x0400052A RID: 1322
		private int _Remoting_Identity_IDSeqNum;

		// Token: 0x0400052B RID: 1323
		private long _memFailPointReservedMemory;
	}
}
