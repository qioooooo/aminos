using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x0200004B RID: 75
	internal abstract class VolatileEnlistmentState : EnlistmentState
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0002EF70 File Offset: 0x0002E370
		internal static VolatileEnlistmentActive _VolatileEnlistmentActive
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentActive == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentActive == null)
						{
							VolatileEnlistmentActive volatileEnlistmentActive = new VolatileEnlistmentActive();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentActive = volatileEnlistmentActive;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentActive;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0002EFD4 File Offset: 0x0002E3D4
		protected static VolatileEnlistmentPreparing _VolatileEnlistmentPreparing
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentPreparing == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentPreparing == null)
						{
							VolatileEnlistmentPreparing volatileEnlistmentPreparing = new VolatileEnlistmentPreparing();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentPreparing = volatileEnlistmentPreparing;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentPreparing;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0002F038 File Offset: 0x0002E438
		protected static VolatileEnlistmentPrepared _VolatileEnlistmentPrepared
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentPrepared == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentPrepared == null)
						{
							VolatileEnlistmentPrepared volatileEnlistmentPrepared = new VolatileEnlistmentPrepared();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentPrepared = volatileEnlistmentPrepared;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentPrepared;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0002F09C File Offset: 0x0002E49C
		protected static VolatileEnlistmentSPC _VolatileEnlistmentSPC
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentSPC == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentSPC == null)
						{
							VolatileEnlistmentSPC volatileEnlistmentSPC = new VolatileEnlistmentSPC();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentSPC = volatileEnlistmentSPC;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentSPC;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0002F100 File Offset: 0x0002E500
		protected static VolatileEnlistmentPreparingAborting _VolatileEnlistmentPreparingAborting
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentPreparingAborting == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentPreparingAborting == null)
						{
							VolatileEnlistmentPreparingAborting volatileEnlistmentPreparingAborting = new VolatileEnlistmentPreparingAborting();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentPreparingAborting = volatileEnlistmentPreparingAborting;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentPreparingAborting;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0002F164 File Offset: 0x0002E564
		protected static VolatileEnlistmentAborting _VolatileEnlistmentAborting
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentAborting == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentAborting == null)
						{
							VolatileEnlistmentAborting volatileEnlistmentAborting = new VolatileEnlistmentAborting();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentAborting = volatileEnlistmentAborting;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentAborting;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0002F1C8 File Offset: 0x0002E5C8
		protected static VolatileEnlistmentCommitting _VolatileEnlistmentCommitting
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentCommitting == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentCommitting == null)
						{
							VolatileEnlistmentCommitting volatileEnlistmentCommitting = new VolatileEnlistmentCommitting();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentCommitting = volatileEnlistmentCommitting;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentCommitting;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0002F22C File Offset: 0x0002E62C
		protected static VolatileEnlistmentInDoubt _VolatileEnlistmentInDoubt
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentInDoubt == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentInDoubt == null)
						{
							VolatileEnlistmentInDoubt volatileEnlistmentInDoubt = new VolatileEnlistmentInDoubt();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentInDoubt = volatileEnlistmentInDoubt;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentInDoubt;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0002F290 File Offset: 0x0002E690
		protected static VolatileEnlistmentEnded _VolatileEnlistmentEnded
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentEnded == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentEnded == null)
						{
							VolatileEnlistmentEnded volatileEnlistmentEnded = new VolatileEnlistmentEnded();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentEnded = volatileEnlistmentEnded;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentEnded;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0002F2F4 File Offset: 0x0002E6F4
		protected static VolatileEnlistmentDone _VolatileEnlistmentDone
		{
			get
			{
				if (VolatileEnlistmentState._volatileEnlistmentDone == null)
				{
					lock (VolatileEnlistmentState.ClassSyncObject)
					{
						if (VolatileEnlistmentState._volatileEnlistmentDone == null)
						{
							VolatileEnlistmentDone volatileEnlistmentDone = new VolatileEnlistmentDone();
							Thread.MemoryBarrier();
							VolatileEnlistmentState._volatileEnlistmentDone = volatileEnlistmentDone;
						}
					}
				}
				return VolatileEnlistmentState._volatileEnlistmentDone;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0002F358 File Offset: 0x0002E758
		private static object ClassSyncObject
		{
			get
			{
				if (VolatileEnlistmentState.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref VolatileEnlistmentState.classSyncObject, obj, null);
				}
				return VolatileEnlistmentState.classSyncObject;
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0002F384 File Offset: 0x0002E784
		internal override byte[] RecoveryInformation(InternalEnlistment enlistment)
		{
			throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("VolEnlistNoRecoveryInfo"), null);
		}

		// Token: 0x040000FE RID: 254
		private static VolatileEnlistmentActive _volatileEnlistmentActive;

		// Token: 0x040000FF RID: 255
		private static VolatileEnlistmentPreparing _volatileEnlistmentPreparing;

		// Token: 0x04000100 RID: 256
		private static VolatileEnlistmentPrepared _volatileEnlistmentPrepared;

		// Token: 0x04000101 RID: 257
		private static VolatileEnlistmentSPC _volatileEnlistmentSPC;

		// Token: 0x04000102 RID: 258
		private static VolatileEnlistmentPreparingAborting _volatileEnlistmentPreparingAborting;

		// Token: 0x04000103 RID: 259
		private static VolatileEnlistmentAborting _volatileEnlistmentAborting;

		// Token: 0x04000104 RID: 260
		private static VolatileEnlistmentCommitting _volatileEnlistmentCommitting;

		// Token: 0x04000105 RID: 261
		private static VolatileEnlistmentInDoubt _volatileEnlistmentInDoubt;

		// Token: 0x04000106 RID: 262
		private static VolatileEnlistmentEnded _volatileEnlistmentEnded;

		// Token: 0x04000107 RID: 263
		private static VolatileEnlistmentDone _volatileEnlistmentDone;

		// Token: 0x04000108 RID: 264
		private static object classSyncObject;
	}
}
