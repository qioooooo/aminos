using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x02000134 RID: 308
	[Serializable]
	public sealed class CompressedStack : ISerializable
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060011A7 RID: 4519 RVA: 0x00032073 File Offset: 0x00031073
		// (set) Token: 0x060011A8 RID: 4520 RVA: 0x0003207B File Offset: 0x0003107B
		internal bool CanSkipEvaluation
		{
			get
			{
				return this.m_canSkipEvaluation;
			}
			private set
			{
				this.m_canSkipEvaluation = value;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00032084 File Offset: 0x00031084
		internal PermissionListSet PLS
		{
			get
			{
				return this.m_pls;
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0003208C File Offset: 0x0003108C
		internal CompressedStack(SafeCompressedStackHandle csHandle)
		{
			this.m_csHandle = csHandle;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0003209B File Offset: 0x0003109B
		private CompressedStack(SafeCompressedStackHandle csHandle, PermissionListSet pls)
		{
			this.m_csHandle = csHandle;
			this.m_pls = pls;
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x000320B1 File Offset: 0x000310B1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.CompleteConstruction(null);
			info.AddValue("PLS", this.m_pls);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x000320D9 File Offset: 0x000310D9
		private CompressedStack(SerializationInfo info, StreamingContext context)
		{
			this.m_pls = (PermissionListSet)info.GetValue("PLS", typeof(PermissionListSet));
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x00032101 File Offset: 0x00031101
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x00032109 File Offset: 0x00031109
		internal SafeCompressedStackHandle CompressedStackHandle
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.m_csHandle;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			private set
			{
				this.m_csHandle = value;
			}
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00032114 File Offset: 0x00031114
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000")]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static CompressedStack GetCompressedStack()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return CompressedStack.GetCompressedStack(ref stackCrawlMark);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0003212C File Offset: 0x0003112C
		internal static CompressedStack GetCompressedStack(ref StackCrawlMark stackMark)
		{
			CompressedStack compressedStack = null;
			CompressedStack compressedStack2;
			if (CodeAccessSecurityEngine.QuickCheckForAllDemands())
			{
				compressedStack2 = new CompressedStack(null);
				compressedStack2.CanSkipEvaluation = true;
			}
			else if (CodeAccessSecurityEngine.AllDomainsHomogeneousWithNoStackModifiers())
			{
				compressedStack2 = new CompressedStack(CompressedStack.GetDelayedCompressedStack(ref stackMark, false));
				compressedStack2.m_pls = PermissionListSet.CreateCompressedState_HG();
			}
			else
			{
				compressedStack2 = new CompressedStack(null);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					compressedStack2.CompressedStackHandle = CompressedStack.GetDelayedCompressedStack(ref stackMark, true);
					if (compressedStack2.CompressedStackHandle != null && CompressedStack.IsImmediateCompletionCandidate(compressedStack2.CompressedStackHandle, out compressedStack))
					{
						try
						{
							compressedStack2.CompleteConstruction(compressedStack);
						}
						finally
						{
							CompressedStack.DestroyDCSList(compressedStack2.CompressedStackHandle);
						}
					}
				}
			}
			return compressedStack2;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x000321D8 File Offset: 0x000311D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static CompressedStack Capture()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return CompressedStack.GetCompressedStack(ref stackCrawlMark);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x000321F0 File Offset: 0x000311F0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void Run(CompressedStack compressedStack, ContextCallback callback, object state)
		{
			if (compressedStack == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_NamedParamNull"), "compressedStack");
			}
			if (CompressedStack.cleanupCode == null)
			{
				CompressedStack.tryCode = new RuntimeHelpers.TryCode(CompressedStack.runTryCode);
				CompressedStack.cleanupCode = new RuntimeHelpers.CleanupCode(CompressedStack.runFinallyCode);
			}
			CompressedStack.CompressedStackRunData compressedStackRunData = new CompressedStack.CompressedStackRunData(compressedStack, callback, state);
			RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(CompressedStack.tryCode, CompressedStack.cleanupCode, compressedStackRunData);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00032258 File Offset: 0x00031258
		internal static void runTryCode(object userData)
		{
			CompressedStack.CompressedStackRunData compressedStackRunData = (CompressedStack.CompressedStackRunData)userData;
			compressedStackRunData.cssw = CompressedStack.SetCompressedStack(compressedStackRunData.cs, CompressedStack.GetCompressedStackThread());
			compressedStackRunData.callBack(compressedStackRunData.state);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00032294 File Offset: 0x00031294
		[PrePrepareMethod]
		internal static void runFinallyCode(object userData, bool exceptionThrown)
		{
			CompressedStack.CompressedStackRunData compressedStackRunData = (CompressedStack.CompressedStackRunData)userData;
			compressedStackRunData.cssw.Undo();
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x000322B4 File Offset: 0x000312B4
		internal static CompressedStackSwitcher SetCompressedStack(CompressedStack cs, CompressedStack prevCS)
		{
			CompressedStackSwitcher compressedStackSwitcher = default(CompressedStackSwitcher);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					CompressedStack.SetCompressedStackThread(cs);
					compressedStackSwitcher.prev_CS = prevCS;
					compressedStackSwitcher.curr_CS = cs;
					compressedStackSwitcher.prev_ADStack = CompressedStack.SetAppDomainStack(cs);
				}
			}
			catch
			{
				compressedStackSwitcher.UndoNoThrow();
				throw;
			}
			return compressedStackSwitcher;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00032324 File Offset: 0x00031324
		[ComVisible(false)]
		public CompressedStack CreateCopy()
		{
			return new CompressedStack(this.m_csHandle, this.m_pls);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00032337 File Offset: 0x00031337
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static IntPtr SetAppDomainStack(CompressedStack cs)
		{
			return Thread.CurrentThread.SetAppDomainStack((cs == null) ? null : cs.CompressedStackHandle);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0003234F File Offset: 0x0003134F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static void RestoreAppDomainStack(IntPtr appDomainStack)
		{
			Thread.CurrentThread.RestoreAppDomainStack(appDomainStack);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0003235C File Offset: 0x0003135C
		internal static CompressedStack GetCompressedStackThread()
		{
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (executionContextNoCreate != null && executionContextNoCreate.SecurityContext != null)
			{
				return executionContextNoCreate.SecurityContext.CompressedStack;
			}
			return null;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0003238C File Offset: 0x0003138C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static void SetCompressedStackThread(CompressedStack cs)
		{
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			if (executionContext.SecurityContext != null)
			{
				executionContext.SecurityContext.CompressedStack = cs;
				return;
			}
			if (cs != null)
			{
				executionContext.SecurityContext = new SecurityContext
				{
					CompressedStack = cs
				};
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x000323D0 File Offset: 0x000313D0
		internal bool CheckDemand(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			this.CompleteConstruction(null);
			if (this.PLS == null)
			{
				return false;
			}
			this.PLS.CheckDemand(demand, permToken, rmh);
			return false;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x000323F3 File Offset: 0x000313F3
		internal bool CheckDemandNoHalt(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			this.CompleteConstruction(null);
			return this.PLS == null || this.PLS.CheckDemand(demand, permToken, rmh);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00032414 File Offset: 0x00031414
		internal bool CheckSetDemand(PermissionSet pset, RuntimeMethodHandle rmh)
		{
			this.CompleteConstruction(null);
			return this.PLS != null && this.PLS.CheckSetDemand(pset, rmh);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00032434 File Offset: 0x00031434
		internal bool CheckSetDemandWithModificationNoHalt(PermissionSet pset, out PermissionSet alteredDemandSet, RuntimeMethodHandle rmh)
		{
			alteredDemandSet = null;
			this.CompleteConstruction(null);
			return this.PLS == null || this.PLS.CheckSetDemandWithModification(pset, out alteredDemandSet, rmh);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00032458 File Offset: 0x00031458
		internal void DemandFlagsOrGrantSet(int flags, PermissionSet grantSet)
		{
			this.CompleteConstruction(null);
			if (this.PLS == null)
			{
				return;
			}
			this.PLS.DemandFlagsOrGrantSet(flags, grantSet);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00032477 File Offset: 0x00031477
		internal void GetZoneAndOrigin(ArrayList zoneList, ArrayList originList, PermissionToken zoneToken, PermissionToken originToken)
		{
			this.CompleteConstruction(null);
			if (this.PLS != null)
			{
				this.PLS.GetZoneAndOrigin(zoneList, originList, zoneToken, originToken);
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00032498 File Offset: 0x00031498
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal void CompleteConstruction(CompressedStack innerCS)
		{
			if (this.PLS != null)
			{
				return;
			}
			PermissionListSet permissionListSet = PermissionListSet.CreateCompressedState(this, innerCS);
			lock (this)
			{
				if (this.PLS == null)
				{
					this.m_pls = permissionListSet;
				}
			}
		}

		// Token: 0x060011C3 RID: 4547
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern SafeCompressedStackHandle GetDelayedCompressedStack(ref StackCrawlMark stackMark, bool walkStack);

		// Token: 0x060011C4 RID: 4548
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DestroyDelayedCompressedStack(IntPtr unmanagedCompressedStack);

		// Token: 0x060011C5 RID: 4549
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DestroyDCSList(SafeCompressedStackHandle compressedStack);

		// Token: 0x060011C6 RID: 4550
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetDCSCount(SafeCompressedStackHandle compressedStack);

		// Token: 0x060011C7 RID: 4551
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsImmediateCompletionCandidate(SafeCompressedStackHandle compressedStack, out CompressedStack innerCS);

		// Token: 0x060011C8 RID: 4552
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern DomainCompressedStack GetDomainCompressedStack(SafeCompressedStackHandle compressedStack, int index);

		// Token: 0x060011C9 RID: 4553
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetHomogeneousPLS(PermissionListSet hgPLS);

		// Token: 0x040005DB RID: 1499
		private PermissionListSet m_pls;

		// Token: 0x040005DC RID: 1500
		private SafeCompressedStackHandle m_csHandle;

		// Token: 0x040005DD RID: 1501
		private bool m_canSkipEvaluation;

		// Token: 0x040005DE RID: 1502
		internal static RuntimeHelpers.TryCode tryCode;

		// Token: 0x040005DF RID: 1503
		internal static RuntimeHelpers.CleanupCode cleanupCode;

		// Token: 0x02000135 RID: 309
		internal class CompressedStackRunData
		{
			// Token: 0x060011CA RID: 4554 RVA: 0x000324E8 File Offset: 0x000314E8
			internal CompressedStackRunData(CompressedStack cs, ContextCallback cb, object state)
			{
				this.cs = cs;
				this.callBack = cb;
				this.state = state;
				this.cssw = default(CompressedStackSwitcher);
			}

			// Token: 0x040005E0 RID: 1504
			internal CompressedStack cs;

			// Token: 0x040005E1 RID: 1505
			internal ContextCallback callBack;

			// Token: 0x040005E2 RID: 1506
			internal object state;

			// Token: 0x040005E3 RID: 1507
			internal CompressedStackSwitcher cssw;
		}
	}
}
