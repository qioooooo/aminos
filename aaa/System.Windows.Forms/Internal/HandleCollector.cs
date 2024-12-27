using System;
using System.Threading;

namespace System.Internal
{
	// Token: 0x02000032 RID: 50
	internal sealed class HandleCollector
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000154 RID: 340 RVA: 0x00005718 File Offset: 0x00004718
		// (remove) Token: 0x06000155 RID: 341 RVA: 0x0000572F File Offset: 0x0000472F
		internal static event HandleChangeEventHandler HandleAdded;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000156 RID: 342 RVA: 0x00005746 File Offset: 0x00004746
		// (remove) Token: 0x06000157 RID: 343 RVA: 0x0000575D File Offset: 0x0000475D
		internal static event HandleChangeEventHandler HandleRemoved;

		// Token: 0x06000158 RID: 344 RVA: 0x00005774 File Offset: 0x00004774
		internal static IntPtr Add(IntPtr handle, int type)
		{
			HandleCollector.handleTypes[type - 1].Add(handle);
			return handle;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005788 File Offset: 0x00004788
		internal static void SuspendCollect()
		{
			lock (HandleCollector.internalSyncObject)
			{
				HandleCollector.suspendCount++;
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000057C8 File Offset: 0x000047C8
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

		// Token: 0x0600015B RID: 347 RVA: 0x00005864 File Offset: 0x00004864
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

		// Token: 0x0600015C RID: 348 RVA: 0x000058FC File Offset: 0x000048FC
		internal static IntPtr Remove(IntPtr handle, int type)
		{
			return HandleCollector.handleTypes[type - 1].Remove(handle);
		}

		// Token: 0x04000B27 RID: 2855
		private static HandleCollector.HandleType[] handleTypes;

		// Token: 0x04000B28 RID: 2856
		private static int handleTypeCount;

		// Token: 0x04000B29 RID: 2857
		private static int suspendCount;

		// Token: 0x04000B2C RID: 2860
		private static object internalSyncObject = new object();

		// Token: 0x02000033 RID: 51
		private class HandleType
		{
			// Token: 0x0600015F RID: 351 RVA: 0x00005921 File Offset: 0x00004921
			internal HandleType(string name, int expense, int initialThreshHold)
			{
				this.name = name;
				this.initialThreshHold = initialThreshHold;
				this.threshHold = initialThreshHold;
				this.deltaPercent = 100 - expense;
			}

			// Token: 0x06000160 RID: 352 RVA: 0x00005948 File Offset: 0x00004948
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

			// Token: 0x06000161 RID: 353 RVA: 0x00005A00 File Offset: 0x00004A00
			internal int GetHandleCount()
			{
				int num;
				lock (this)
				{
					num = this.handleCount;
				}
				return num;
			}

			// Token: 0x06000162 RID: 354 RVA: 0x00005A38 File Offset: 0x00004A38
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

			// Token: 0x06000163 RID: 355 RVA: 0x00005AB4 File Offset: 0x00004AB4
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

			// Token: 0x04000B2D RID: 2861
			internal readonly string name;

			// Token: 0x04000B2E RID: 2862
			private int initialThreshHold;

			// Token: 0x04000B2F RID: 2863
			private int threshHold;

			// Token: 0x04000B30 RID: 2864
			private int handleCount;

			// Token: 0x04000B31 RID: 2865
			private readonly int deltaPercent;
		}
	}
}
