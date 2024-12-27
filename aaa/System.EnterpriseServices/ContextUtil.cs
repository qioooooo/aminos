using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;
using System.Transactions;

namespace System.EnterpriseServices
{
	// Token: 0x02000048 RID: 72
	public sealed class ContextUtil
	{
		// Token: 0x06000162 RID: 354 RVA: 0x00005DEF File Offset: 0x00004DEF
		private ContextUtil()
		{
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00005DF8 File Offset: 0x00004DF8
		internal static object ObjectContext
		{
			get
			{
				Platform.Assert(Platform.MTS, "ContextUtil.ObjectContext");
				IObjectContext objectContext = null;
				int objectContext2 = Util.GetObjectContext(out objectContext);
				if (objectContext2 == 0)
				{
					return objectContext;
				}
				if (objectContext2 == Util.E_NOINTERFACE || objectContext2 == Util.CONTEXT_E_NOCONTEXT)
				{
					throw new COMException(Resource.FormatString("Err_NoContext"), Util.CONTEXT_E_NOCONTEXT);
				}
				Marshal.ThrowExceptionForHR(objectContext2);
				return null;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00005E50 File Offset: 0x00004E50
		internal static object SafeObjectContext
		{
			get
			{
				Platform.Assert(Platform.MTS, "ContextUtil.ObjectContext");
				IObjectContext objectContext = null;
				int objectContext2 = Util.GetObjectContext(out objectContext);
				if (objectContext2 == 0)
				{
					return objectContext;
				}
				if (objectContext2 != Util.E_NOINTERFACE && objectContext2 != Util.CONTEXT_E_NOCONTEXT)
				{
					Marshal.ThrowExceptionForHR(objectContext2);
				}
				return null;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005E92 File Offset: 0x00004E92
		public static void EnableCommit()
		{
			Platform.Assert(Platform.MTS, "ContextUtil.EnableCommit");
			ContextThunk.EnableCommit();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005EA8 File Offset: 0x00004EA8
		public static void DisableCommit()
		{
			Platform.Assert(Platform.MTS, "ContextUtil.DisableCommit");
			ContextThunk.DisableCommit();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005EBE File Offset: 0x00004EBE
		public static void SetComplete()
		{
			Platform.Assert(Platform.MTS, "ContextUtil.SetComplete");
			ContextThunk.SetComplete();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005ED4 File Offset: 0x00004ED4
		public static void SetAbort()
		{
			Platform.Assert(Platform.MTS, "ContextUtil.SetAbort");
			ContextThunk.SetAbort();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005EEA File Offset: 0x00004EEA
		public static bool IsCallerInRole(string role)
		{
			Platform.Assert(Platform.MTS, "ContextUtil.IsCallerInRole");
			return ((IObjectContext)ContextUtil.ObjectContext).IsCallerInRole(role);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00005F0B File Offset: 0x00004F0B
		public static bool IsInTransaction
		{
			get
			{
				return ContextThunk.IsInTransaction();
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00005F14 File Offset: 0x00004F14
		public static bool IsSecurityEnabled
		{
			get
			{
				Platform.Assert(Platform.MTS, "ContextUtil.IsSecurityEnabled");
				bool flag;
				try
				{
					flag = ((IObjectContext)ContextUtil.ObjectContext).IsSecurityEnabled();
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00005F58 File Offset: 0x00004F58
		public static object Transaction
		{
			get
			{
				Platform.Assert(Platform.W2K, "ContextUtil.Transaction");
				return ContextThunk.GetTransaction();
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00005F70 File Offset: 0x00004F70
		public static Transaction SystemTransaction
		{
			get
			{
				Platform.Assert(Platform.W2K, "ContextUtil.SystemTransaction");
				object obj = null;
				TxInfo txInfo = new TxInfo();
				if (!ContextThunk.GetTransactionProxyOrTransaction(ref obj, txInfo))
				{
					return null;
				}
				if (txInfo.isDtcTransaction)
				{
					return TransactionInterop.GetTransactionFromDtcTransaction((IDtcTransaction)obj);
				}
				if (obj == null)
				{
					TransactionProxy transactionProxy = new TransactionProxy((DtcIsolationLevel)txInfo.IsolationLevel, txInfo.timeout);
					Guid guid = ContextThunk.RegisterTransactionProxy(transactionProxy);
					transactionProxy.SetOwnerGuid(guid);
					return transactionProxy.SystemTransaction;
				}
				TransactionProxy transactionProxy2 = obj as TransactionProxy;
				if (transactionProxy2 != null)
				{
					return transactionProxy2.SystemTransaction;
				}
				IDtcTransaction dtcTransaction = ContextThunk.GetTransaction() as IDtcTransaction;
				Transaction transactionFromDtcTransaction = TransactionInterop.GetTransactionFromDtcTransaction(dtcTransaction);
				Marshal.ReleaseComObject(obj);
				return transactionFromDtcTransaction;
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006014 File Offset: 0x00005014
		public static bool IsDefaultContext()
		{
			return ContextThunk.IsDefaultContext();
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000601B File Offset: 0x0000501B
		public static Guid TransactionId
		{
			get
			{
				Platform.Assert(Platform.W2K, "ContextUtil.TransactionId");
				return ContextThunk.GetTransactionId();
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00006031 File Offset: 0x00005031
		public static Guid ContextId
		{
			get
			{
				Platform.Assert(Platform.W2K, "ContextUtil.ContextId");
				return ((IObjectContextInfo)ContextUtil.ObjectContext).GetContextId();
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00006051 File Offset: 0x00005051
		public static Guid ActivityId
		{
			get
			{
				Platform.Assert(Platform.W2K, "ContextUtil.ActivityId");
				return ((IObjectContextInfo)ContextUtil.ObjectContext).GetActivityId();
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00006071 File Offset: 0x00005071
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00006087 File Offset: 0x00005087
		public static TransactionVote MyTransactionVote
		{
			get
			{
				Platform.Assert(Platform.W2K, "ContextUtil.MyTransactionVote");
				return (TransactionVote)ContextThunk.GetMyTransactionVote();
			}
			set
			{
				Platform.Assert(Platform.W2K, "ContextUtil.MyTransactionVote");
				ContextThunk.SetMyTransactionVote((int)value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000609E File Offset: 0x0000509E
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000060B4 File Offset: 0x000050B4
		public static bool DeactivateOnReturn
		{
			get
			{
				Platform.Assert(Platform.W2K, "ContextUtil.DeactivateOnReturn");
				return ContextThunk.GetDeactivateOnReturn();
			}
			set
			{
				Platform.Assert(Platform.W2K, "ContextUtil.DeactivateOnReturn");
				ContextThunk.SetDeactivateOnReturn(value);
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000060CB File Offset: 0x000050CB
		public static object GetNamedProperty(string name)
		{
			Platform.Assert(Platform.W2K, "ContextUtil.GetNamedProperty");
			return ((IGetContextProperties)ContextUtil.ObjectContext).GetProperty(name);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000060EC File Offset: 0x000050EC
		public static void SetNamedProperty(string name, object value)
		{
			Platform.Assert(Platform.W2K, "ContextUtil.SetNamedProperty");
			IContextProperties contextProperties = (IContextProperties)ContextUtil.ObjectContext;
			contextProperties.SetProperty(name, value);
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000611B File Offset: 0x0000511B
		public static Guid PartitionId
		{
			get
			{
				Platform.Assert(Platform.Whistler, "ContextUtil.PartitionId");
				return ((IObjectContextInfo2)ContextUtil.ObjectContext).GetPartitionId();
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000613B File Offset: 0x0000513B
		public static Guid ApplicationId
		{
			get
			{
				Platform.Assert(Platform.Whistler, "ContextUtil.ApplicationId");
				return ((IObjectContextInfo2)ContextUtil.ObjectContext).GetApplicationId();
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000615B File Offset: 0x0000515B
		public static Guid ApplicationInstanceId
		{
			get
			{
				Platform.Assert(Platform.Whistler, "ContextUtil.ApplicationInstanceId");
				return ((IObjectContextInfo2)ContextUtil.ObjectContext).GetApplicationInstanceId();
			}
		}

		// Token: 0x04000096 RID: 150
		internal static readonly Guid GUID_TransactionProperty = new Guid("ecabaeb1-7f19-11d2-978e-0000f8757e2a");

		// Token: 0x04000097 RID: 151
		internal static readonly Guid GUID_JitActivationPolicy = new Guid("ecabaeb2-7f19-11d2-978e-0000f8757e2a");
	}
}
