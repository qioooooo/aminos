using System;
using System.Threading;

namespace System.Management
{
	// Token: 0x02000107 RID: 263
	internal class ThreadDispatch
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0002762A File Offset: 0x0002662A
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00027632 File Offset: 0x00026632
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x0002763A File Offset: 0x0002663A
		public object Parameter
		{
			get
			{
				return this.threadParams;
			}
			set
			{
				this.threadParams = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00027643 File Offset: 0x00026643
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x0002764B File Offset: 0x0002664B
		public bool IsBackgroundThread
		{
			get
			{
				return this.backgroundThread;
			}
			set
			{
				this.backgroundThread = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00027654 File Offset: 0x00026654
		public object Result
		{
			get
			{
				return this.threadReturn;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0002765C File Offset: 0x0002665C
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x00027664 File Offset: 0x00026664
		public ApartmentState ApartmentType
		{
			get
			{
				return this.apartmentType;
			}
			set
			{
				this.apartmentType = value;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0002766D File Offset: 0x0002666D
		public ThreadDispatch(ThreadDispatch.ThreadWorkerMethodWithReturn workerMethod)
			: this()
		{
			this.InitializeThreadState(null, workerMethod, ApartmentState.MTA, false);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002767F File Offset: 0x0002667F
		public ThreadDispatch(ThreadDispatch.ThreadWorkerMethodWithReturnAndParam workerMethod)
			: this()
		{
			this.InitializeThreadState(null, workerMethod, ApartmentState.MTA, false);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00027691 File Offset: 0x00026691
		public ThreadDispatch(ThreadDispatch.ThreadWorkerMethodWithParam workerMethod)
			: this()
		{
			this.InitializeThreadState(null, workerMethod, ApartmentState.MTA, false);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000276A3 File Offset: 0x000266A3
		public ThreadDispatch(ThreadDispatch.ThreadWorkerMethod workerMethod)
			: this()
		{
			this.InitializeThreadState(null, workerMethod, ApartmentState.MTA, false);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000276B5 File Offset: 0x000266B5
		public void Start()
		{
			this.exception = null;
			this.DispatchThread();
			if (this.Exception != null)
			{
				throw this.Exception;
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000276D4 File Offset: 0x000266D4
		private ThreadDispatch()
		{
			this.thread = null;
			this.exception = null;
			this.threadParams = null;
			this.threadWorkerMethodWithReturn = null;
			this.threadWorkerMethodWithReturnAndParam = null;
			this.threadWorkerMethod = null;
			this.threadWorkerMethodWithParam = null;
			this.threadReturn = null;
			this.backgroundThread = false;
			this.apartmentType = ApartmentState.MTA;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002772D File Offset: 0x0002672D
		private void InitializeThreadState(object threadParams, ThreadDispatch.ThreadWorkerMethodWithReturn workerMethod, ApartmentState aptState, bool background)
		{
			this.threadParams = threadParams;
			this.threadWorkerMethodWithReturn = workerMethod;
			this.thread = new Thread(new ThreadStart(this.ThreadEntryPointMethodWithReturn));
			this.thread.SetApartmentState(aptState);
			this.backgroundThread = background;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00027768 File Offset: 0x00026768
		private void InitializeThreadState(object threadParams, ThreadDispatch.ThreadWorkerMethodWithReturnAndParam workerMethod, ApartmentState aptState, bool background)
		{
			this.threadParams = threadParams;
			this.threadWorkerMethodWithReturnAndParam = workerMethod;
			this.thread = new Thread(new ThreadStart(this.ThreadEntryPointMethodWithReturnAndParam));
			this.thread.SetApartmentState(aptState);
			this.backgroundThread = background;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x000277A3 File Offset: 0x000267A3
		private void InitializeThreadState(object threadParams, ThreadDispatch.ThreadWorkerMethod workerMethod, ApartmentState aptState, bool background)
		{
			this.threadParams = threadParams;
			this.threadWorkerMethod = workerMethod;
			this.thread = new Thread(new ThreadStart(this.ThreadEntryPoint));
			this.thread.SetApartmentState(aptState);
			this.backgroundThread = background;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000277DE File Offset: 0x000267DE
		private void InitializeThreadState(object threadParams, ThreadDispatch.ThreadWorkerMethodWithParam workerMethod, ApartmentState aptState, bool background)
		{
			this.threadParams = threadParams;
			this.threadWorkerMethodWithParam = workerMethod;
			this.thread = new Thread(new ThreadStart(this.ThreadEntryPointMethodWithParam));
			this.thread.SetApartmentState(aptState);
			this.backgroundThread = background;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00027819 File Offset: 0x00026819
		private void DispatchThread()
		{
			this.thread.Start();
			this.thread.Join();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00027834 File Offset: 0x00026834
		private void ThreadEntryPoint()
		{
			try
			{
				this.threadWorkerMethod();
			}
			catch (Exception ex)
			{
				this.exception = ex;
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00027868 File Offset: 0x00026868
		private void ThreadEntryPointMethodWithParam()
		{
			try
			{
				this.threadWorkerMethodWithParam(this.threadParams);
			}
			catch (Exception ex)
			{
				this.exception = ex;
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x000278A4 File Offset: 0x000268A4
		private void ThreadEntryPointMethodWithReturn()
		{
			try
			{
				this.threadReturn = this.threadWorkerMethodWithReturn();
			}
			catch (Exception ex)
			{
				this.exception = ex;
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000278E0 File Offset: 0x000268E0
		private void ThreadEntryPointMethodWithReturnAndParam()
		{
			try
			{
				this.threadReturn = this.threadWorkerMethodWithReturnAndParam(this.threadParams);
			}
			catch (Exception ex)
			{
				this.exception = ex;
			}
		}

		// Token: 0x0400052C RID: 1324
		private Thread thread;

		// Token: 0x0400052D RID: 1325
		private Exception exception;

		// Token: 0x0400052E RID: 1326
		private ThreadDispatch.ThreadWorkerMethodWithReturn threadWorkerMethodWithReturn;

		// Token: 0x0400052F RID: 1327
		private ThreadDispatch.ThreadWorkerMethodWithReturnAndParam threadWorkerMethodWithReturnAndParam;

		// Token: 0x04000530 RID: 1328
		private ThreadDispatch.ThreadWorkerMethod threadWorkerMethod;

		// Token: 0x04000531 RID: 1329
		private ThreadDispatch.ThreadWorkerMethodWithParam threadWorkerMethodWithParam;

		// Token: 0x04000532 RID: 1330
		private object threadReturn;

		// Token: 0x04000533 RID: 1331
		private object threadParams;

		// Token: 0x04000534 RID: 1332
		private bool backgroundThread;

		// Token: 0x04000535 RID: 1333
		private ApartmentState apartmentType;

		// Token: 0x02000108 RID: 264
		// (Invoke) Token: 0x06000677 RID: 1655
		public delegate object ThreadWorkerMethodWithReturn();

		// Token: 0x02000109 RID: 265
		// (Invoke) Token: 0x0600067B RID: 1659
		public delegate object ThreadWorkerMethodWithReturnAndParam(object param);

		// Token: 0x0200010A RID: 266
		// (Invoke) Token: 0x0600067F RID: 1663
		public delegate void ThreadWorkerMethod();

		// Token: 0x0200010B RID: 267
		// (Invoke) Token: 0x06000683 RID: 1667
		public delegate void ThreadWorkerMethodWithParam(object param);
	}
}
