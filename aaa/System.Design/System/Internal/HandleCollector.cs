using System;
using System.Threading;

namespace System.Internal
{
	// Token: 0x02000009 RID: 9
	internal sealed class HandleCollector
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000D RID: 13 RVA: 0x0000229D File Offset: 0x0000129D
		// (remove) Token: 0x0600000E RID: 14 RVA: 0x000022B4 File Offset: 0x000012B4
		internal static event HandleChangeEventHandler HandleAdded;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600000F RID: 15 RVA: 0x000022CB File Offset: 0x000012CB
		// (remove) Token: 0x06000010 RID: 16 RVA: 0x000022E2 File Offset: 0x000012E2
		internal static event HandleChangeEventHandler HandleRemoved;

		// Token: 0x06000011 RID: 17 RVA: 0x000022F9 File Offset: 0x000012F9
		internal static IntPtr Add(IntPtr handle, int type)
		{
			HandleCollector.handleTypes[type - 1].Add(handle);
			return handle;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000230C File Offset: 0x0000130C
		internal static void SuspendCollect()
		{
			lock (HandleCollector.internalSyncObject)
			{
				HandleCollector.suspendCount++;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000234C File Offset: 0x0000134C
		internal static void ResumeCollect()
		{
			bool flag = false;
			lock (HandleCollector.internalSyncObject)
			{
				if (HandleCollector.suspendCount > 0)
				{
					HandleCollector.suspendCount--;
				}
				if (HandleCollector.suspendCount == 0)
				{
					for (int i = 0; i < HandleCollector.handleTypeCount; i++)
					{
						lock (HandleCollector.handleTypes[i])
						{
							if (HandleCollector.handleTypes[i].NeedCollection())
							{
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
			{
				GC.Collect();
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023E8 File Offset: 0x000013E8
		internal static int RegisterType(string typeName, int expense, int initialThreshold)
		{
			int num;
			lock (HandleCollector.internalSyncObject)
			{
				if (HandleCollector.handleTypeCount == 0 || HandleCollector.handleTypeCount == HandleCollector.handleTypes.Length)
				{
					HandleCollector.HandleType[] array = new HandleCollector.HandleType[HandleCollector.handleTypeCount + 10];
					if (HandleCollector.handleTypes != null)
					{
						Array.Copy(HandleCollector.handleTypes, 0, array, 0, HandleCollector.handleTypeCount);
					}
					HandleCollector.handleTypes = array;
				}
				HandleCollector.handleTypes[HandleCollector.handleTypeCount++] = new HandleCollector.HandleType(typeName, expense, initialThreshold);
				num = HandleCollector.handleTypeCount;
			}
			return num;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002480 File Offset: 0x00001480
		internal static IntPtr Remove(IntPtr handle, int type)
		{
			return HandleCollector.handleTypes[type - 1].Remove(handle);
		}

		// Token: 0x04000759 RID: 1881
		private static HandleCollector.HandleType[] handleTypes;

		// Token: 0x0400075A RID: 1882
		private static int handleTypeCount;

		// Token: 0x0400075B RID: 1883
		private static int suspendCount;

		// Token: 0x0400075E RID: 1886
		private static object internalSyncObject = new object();

		// Token: 0x0200000A RID: 10
		private class HandleType
		{
			// Token: 0x06000018 RID: 24 RVA: 0x000024A5 File Offset: 0x000014A5
			internal HandleType(string name, int expense, int initialThreshHold)
			{
				this.name = name;
				this.initialThreshHold = initialThreshHold;
				this.threshHold = initialThreshHold;
				this.deltaPercent = 100 - expense;
			}

			// Token: 0x06000019 RID: 25 RVA: 0x000024CC File Offset: 0x000014CC
			internal void Add(IntPtr handle)
			{
				if (handle == IntPtr.Zero)
				{
					return;
				}
				bool flag = false;
				int num = 0;
				lock (this)
				{
					this.handleCount++;
					flag = this.NeedCollection();
					num = this.handleCount;
				}
				lock (HandleCollector.internalSyncObject)
				{
					if (HandleCollector.HandleAdded != null)
					{
						HandleCollector.HandleAdded(this.name, handle, num);
					}
				}
				if (!flag)
				{
					return;
				}
				if (flag)
				{
					GC.Collect();
					int num2 = (100 - this.deltaPercent) / 4;
					Thread.Sleep(num2);
				}
			}

			// Token: 0x0600001A RID: 26 RVA: 0x00002584 File Offset: 0x00001584
			internal int GetHandleCount()
			{
				int num;
				lock (this)
				{
					num = this.handleCount;
				}
				return num;
			}

			// Token: 0x0600001B RID: 27 RVA: 0x000025BC File Offset: 0x000015BC
			internal bool NeedCollection()
			{
				if (HandleCollector.suspendCount > 0)
				{
					return false;
				}
				if (this.handleCount > this.threshHold)
				{
					this.threshHold = this.handleCount + this.handleCount * this.deltaPercent / 100;
					return true;
				}
				int num = 100 * this.threshHold / (100 + this.deltaPercent);
				if (num >= this.initialThreshHold && this.handleCount < (int)((float)num * 0.9f))
				{
					this.threshHold = num;
				}
				return false;
			}

			// Token: 0x0600001C RID: 28 RVA: 0x00002638 File Offset: 0x00001638
			internal IntPtr Remove(IntPtr handle)
			{
				if (handle == IntPtr.Zero)
				{
					return handle;
				}
				int num = 0;
				lock (this)
				{
					this.handleCount--;
					if (this.handleCount < 0)
					{
						this.handleCount = 0;
					}
					num = this.handleCount;
				}
				lock (HandleCollector.internalSyncObject)
				{
					if (HandleCollector.HandleRemoved != null)
					{
						HandleCollector.HandleRemoved(this.name, handle, num);
					}
				}
				return handle;
			}

			// Token: 0x0400075F RID: 1887
			internal readonly string name;

			// Token: 0x04000760 RID: 1888
			private int initialThreshHold;

			// Token: 0x04000761 RID: 1889
			private int threshHold;

			// Token: 0x04000762 RID: 1890
			private int handleCount;

			// Token: 0x04000763 RID: 1891
			private readonly int deltaPercent;
		}
	}
}
