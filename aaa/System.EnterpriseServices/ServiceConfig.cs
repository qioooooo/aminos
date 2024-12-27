using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;
using System.Transactions;

namespace System.EnterpriseServices
{
	// Token: 0x020000E8 RID: 232
	[ComVisible(false)]
	public sealed class ServiceConfig
	{
		// Token: 0x06000525 RID: 1317 RVA: 0x00011F4B File Offset: 0x00010F4B
		private void Init()
		{
			this.m_sct = new ServiceConfigThunk();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00011F58 File Offset: 0x00010F58
		public ServiceConfig()
		{
			Platform.Assert(Platform.Supports(PlatformFeature.SWC), "ServiceConfig");
			this.Init();
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00011F76 File Offset: 0x00010F76
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x00011F7E File Offset: 0x00010F7E
		public ThreadPoolOption ThreadPool
		{
			get
			{
				return this.m_thrpool;
			}
			set
			{
				this.m_sct.ThreadPool = (int)value;
				this.m_thrpool = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00011F93 File Offset: 0x00010F93
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x00011F9C File Offset: 0x00010F9C
		public InheritanceOption Inheritance
		{
			get
			{
				return this.m_inheritance;
			}
			set
			{
				this.m_sct.Inheritance = (int)value;
				this.m_inheritance = value;
				switch (value)
				{
				case InheritanceOption.Inherit:
					this.m_thrpool = ThreadPoolOption.Inherit;
					this.m_txn = TransactionOption.Supported;
					this.m_sync = SynchronizationOption.Supported;
					this.m_bIISIntrinsics = true;
					this.m_bComTIIntrinsics = true;
					this.m_sxs = SxsOption.Inherit;
					this.m_part = PartitionOption.Inherit;
					return;
				case InheritanceOption.Ignore:
					this.m_thrpool = ThreadPoolOption.None;
					this.m_txn = TransactionOption.Disabled;
					this.m_sync = SynchronizationOption.Disabled;
					this.m_bIISIntrinsics = false;
					this.m_bComTIIntrinsics = false;
					this.m_sxs = SxsOption.Ignore;
					this.m_part = PartitionOption.Ignore;
					return;
				default:
					throw new ArgumentException(Resource.FormatString("Err_value"));
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00012041 File Offset: 0x00011041
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x00012049 File Offset: 0x00011049
		public BindingOption Binding
		{
			get
			{
				return this.m_binding;
			}
			set
			{
				this.m_sct.Binding = (int)value;
				this.m_binding = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001205E File Offset: 0x0001105E
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x00012066 File Offset: 0x00011066
		public TransactionOption Transaction
		{
			get
			{
				return this.m_txn;
			}
			set
			{
				this.m_sct.Transaction = (int)value;
				this.m_txn = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001207B File Offset: 0x0001107B
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x00012083 File Offset: 0x00011083
		public TransactionIsolationLevel IsolationLevel
		{
			get
			{
				return this.m_txniso;
			}
			set
			{
				this.m_sct.TxIsolationLevel = (int)value;
				this.m_txniso = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00012098 File Offset: 0x00011098
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x000120A0 File Offset: 0x000110A0
		public int TransactionTimeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				this.m_sct.TxTimeout = value;
				this.m_timeout = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x000120B5 File Offset: 0x000110B5
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x000120BD File Offset: 0x000110BD
		public string TipUrl
		{
			get
			{
				return this.m_strTipUrl;
			}
			set
			{
				this.m_sct.TipUrl = value;
				this.m_strTipUrl = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x000120D2 File Offset: 0x000110D2
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x000120DA File Offset: 0x000110DA
		public string TransactionDescription
		{
			get
			{
				return this.m_strTxDesc;
			}
			set
			{
				this.m_sct.TxDesc = value;
				this.m_strTxDesc = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x000120EF File Offset: 0x000110EF
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x000120F7 File Offset: 0x000110F7
		public ITransaction BringYourOwnTransaction
		{
			get
			{
				return this.m_txnByot;
			}
			set
			{
				this.m_sct.Byot = value;
				this.m_txnByot = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001210C File Offset: 0x0001110C
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x0001213C File Offset: 0x0001113C
		public Transaction BringYourOwnSystemTransaction
		{
			get
			{
				if (this.m_txnByot != null)
				{
					return TransactionInterop.GetTransactionFromDtcTransaction(this.m_txnByot as IDtcTransaction);
				}
				if (this.m_txnProxyByot != null)
				{
					return this.m_txnProxyByot.SystemTransaction;
				}
				return null;
			}
			set
			{
				if (!this.m_sct.SupportsSysTxn)
				{
					this.m_txnByot = (ITransaction)TransactionInterop.GetDtcTransaction(value);
					this.m_sct.Byot = this.m_txnByot;
					this.m_txnProxyByot = null;
					return;
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_txnByot = null;
				this.m_txnProxyByot = new TransactionProxy(value);
				this.m_sct.ByotSysTxn = this.m_txnProxyByot;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x000121BA File Offset: 0x000111BA
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x000121C2 File Offset: 0x000111C2
		public SynchronizationOption Synchronization
		{
			get
			{
				return this.m_sync;
			}
			set
			{
				this.m_sct.Synchronization = (int)value;
				this.m_sync = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x000121D7 File Offset: 0x000111D7
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x000121DF File Offset: 0x000111DF
		public bool IISIntrinsicsEnabled
		{
			get
			{
				return this.m_bIISIntrinsics;
			}
			set
			{
				this.m_sct.IISIntrinsics = value;
				this.m_bIISIntrinsics = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x000121F4 File Offset: 0x000111F4
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x000121FC File Offset: 0x000111FC
		public bool COMTIIntrinsicsEnabled
		{
			get
			{
				return this.m_bComTIIntrinsics;
			}
			set
			{
				this.m_sct.COMTIIntrinsics = value;
				this.m_bComTIIntrinsics = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00012211 File Offset: 0x00011211
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x00012219 File Offset: 0x00011219
		public bool TrackingEnabled
		{
			get
			{
				return this.m_bTracker;
			}
			set
			{
				this.m_sct.Tracker = value;
				this.m_bTracker = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0001222E File Offset: 0x0001122E
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x00012236 File Offset: 0x00011236
		public string TrackingAppName
		{
			get
			{
				return this.m_strTrackerAppName;
			}
			set
			{
				this.m_sct.TrackerAppName = value;
				this.m_strTrackerAppName = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0001224B File Offset: 0x0001124B
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x00012253 File Offset: 0x00011253
		public string TrackingComponentName
		{
			get
			{
				return this.m_strTrackerCompName;
			}
			set
			{
				this.m_sct.TrackerCtxName = value;
				this.m_strTrackerCompName = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00012268 File Offset: 0x00011268
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x00012270 File Offset: 0x00011270
		public SxsOption SxsOption
		{
			get
			{
				return this.m_sxs;
			}
			set
			{
				this.m_sct.Sxs = (int)value;
				this.m_sxs = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00012285 File Offset: 0x00011285
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x0001228D File Offset: 0x0001128D
		public string SxsDirectory
		{
			get
			{
				return this.m_strSxsDirectory;
			}
			set
			{
				this.m_sct.SxsDirectory = value;
				this.m_strSxsDirectory = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x000122A2 File Offset: 0x000112A2
		// (set) Token: 0x0600054C RID: 1356 RVA: 0x000122AA File Offset: 0x000112AA
		public string SxsName
		{
			get
			{
				return this.m_strSxsName;
			}
			set
			{
				this.m_sct.SxsName = value;
				this.m_strSxsName = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x000122BF File Offset: 0x000112BF
		// (set) Token: 0x0600054E RID: 1358 RVA: 0x000122C7 File Offset: 0x000112C7
		public PartitionOption PartitionOption
		{
			get
			{
				return this.m_part;
			}
			set
			{
				this.m_sct.Partition = (int)value;
				this.m_part = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x000122DC File Offset: 0x000112DC
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x000122E4 File Offset: 0x000112E4
		public Guid PartitionId
		{
			get
			{
				return this.m_guidPart;
			}
			set
			{
				this.m_sct.PartitionId = value;
				this.m_guidPart = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x000122F9 File Offset: 0x000112F9
		internal ServiceConfigThunk SCT
		{
			get
			{
				return this.m_sct;
			}
		}

		// Token: 0x04000228 RID: 552
		private ServiceConfigThunk m_sct;

		// Token: 0x04000229 RID: 553
		private ThreadPoolOption m_thrpool;

		// Token: 0x0400022A RID: 554
		private InheritanceOption m_inheritance;

		// Token: 0x0400022B RID: 555
		private BindingOption m_binding;

		// Token: 0x0400022C RID: 556
		private TransactionOption m_txn;

		// Token: 0x0400022D RID: 557
		private TransactionIsolationLevel m_txniso;

		// Token: 0x0400022E RID: 558
		private int m_timeout;

		// Token: 0x0400022F RID: 559
		private string m_strTipUrl;

		// Token: 0x04000230 RID: 560
		private string m_strTxDesc;

		// Token: 0x04000231 RID: 561
		private ITransaction m_txnByot;

		// Token: 0x04000232 RID: 562
		private TransactionProxy m_txnProxyByot;

		// Token: 0x04000233 RID: 563
		private SynchronizationOption m_sync;

		// Token: 0x04000234 RID: 564
		private bool m_bIISIntrinsics;

		// Token: 0x04000235 RID: 565
		private bool m_bComTIIntrinsics;

		// Token: 0x04000236 RID: 566
		private bool m_bTracker;

		// Token: 0x04000237 RID: 567
		private string m_strTrackerAppName;

		// Token: 0x04000238 RID: 568
		private string m_strTrackerCompName;

		// Token: 0x04000239 RID: 569
		private SxsOption m_sxs;

		// Token: 0x0400023A RID: 570
		private string m_strSxsDirectory;

		// Token: 0x0400023B RID: 571
		private string m_strSxsName;

		// Token: 0x0400023C RID: 572
		private PartitionOption m_part;

		// Token: 0x0400023D RID: 573
		private Guid m_guidPart;
	}
}
