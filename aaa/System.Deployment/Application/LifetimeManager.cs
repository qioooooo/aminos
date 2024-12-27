using System;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x02000062 RID: 98
	internal static class LifetimeManager
	{
		// Token: 0x06000301 RID: 769 RVA: 0x00011490 File Offset: 0x00010490
		static LifetimeManager()
		{
			TimeSpan timeSpan = new TimeSpan(0, 0, 10, 0);
			LifetimeManager._periodicTimer = new Timer(new TimerCallback(LifetimeManager.PeriodicTimerCallback), null, timeSpan, timeSpan);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000114D0 File Offset: 0x000104D0
		public static void StartOperation()
		{
			lock (LifetimeManager._periodicTimer)
			{
				LifetimeManager.CheckAlive();
				LifetimeManager._operationsInProgress++;
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00011514 File Offset: 0x00010514
		public static void EndOperation()
		{
			lock (LifetimeManager._periodicTimer)
			{
				LifetimeManager.CheckAlive();
				LifetimeManager._operationsInProgress--;
				LifetimeManager._lifetimeExtended = true;
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00011560 File Offset: 0x00010560
		public static void ExtendLifetime()
		{
			lock (LifetimeManager._periodicTimer)
			{
				LifetimeManager.CheckAlive();
				LifetimeManager._lifetimeExtended = true;
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000115A0 File Offset: 0x000105A0
		public static bool WaitForEnd()
		{
			LifetimeManager._lifetimeEndedEvent.WaitOne();
			return LifetimeManager._immediate;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000115B4 File Offset: 0x000105B4
		public static void EndImmediately()
		{
			lock (LifetimeManager._periodicTimer)
			{
				if (LifetimeManager._operationsInProgress != 0)
				{
					Logger.StartCurrentThreadLogging();
					Logger.AddPhaseInformation(Resources.GetString("Life_OperationsInProgress"), new object[] { LifetimeManager._operationsInProgress });
					Logger.EndCurrentThreadLogging();
				}
				LifetimeManager._lifetimeEndedEvent.Set();
				LifetimeManager._lifetimeEnded = true;
				LifetimeManager._immediate = true;
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00011634 File Offset: 0x00010634
		private static void CheckAlive()
		{
			if (LifetimeManager._lifetimeEnded)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_LifetimeEnded"));
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00011650 File Offset: 0x00010650
		private static void PeriodicTimerCallback(object state)
		{
			lock (LifetimeManager._periodicTimer)
			{
				if (LifetimeManager._operationsInProgress == 0 && !LifetimeManager._lifetimeExtended)
				{
					LifetimeManager._lifetimeEndedEvent.Set();
					LifetimeManager._lifetimeEnded = true;
				}
				else
				{
					LifetimeManager._lifetimeExtended = false;
				}
			}
		}

		// Token: 0x0400024D RID: 589
		private static ManualResetEvent _lifetimeEndedEvent = new ManualResetEvent(false);

		// Token: 0x0400024E RID: 590
		private static Timer _periodicTimer;

		// Token: 0x0400024F RID: 591
		private static int _operationsInProgress;

		// Token: 0x04000250 RID: 592
		private static bool _lifetimeExtended;

		// Token: 0x04000251 RID: 593
		private static bool _lifetimeEnded;

		// Token: 0x04000252 RID: 594
		private static bool _immediate;
	}
}
