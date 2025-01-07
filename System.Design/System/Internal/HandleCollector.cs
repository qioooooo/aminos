using System;
using System.Threading;

namespace System.Internal
{
	internal sealed class HandleCollector
	{
		internal static event HandleChangeEventHandler HandleAdded;

		internal static event HandleChangeEventHandler HandleRemoved;

		internal static IntPtr Add(IntPtr handle, int type)
		{
			HandleCollector.handleTypes[type - 1].Add(handle);
			return handle;
		}

		internal static void SuspendCollect()
		{
			lock (HandleCollector.internalSyncObject)
			{
				HandleCollector.suspendCount++;
			}
		}

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

		internal static IntPtr Remove(IntPtr handle, int type)
		{
			return HandleCollector.handleTypes[type - 1].Remove(handle);
		}

		private static HandleCollector.HandleType[] handleTypes;

		private static int handleTypeCount;

		private static int suspendCount;

		private static object internalSyncObject = new object();

		private class HandleType
		{
			internal HandleType(string name, int expense, int initialThreshHold)
			{
				this.name = name;
				this.initialThreshHold = initialThreshHold;
				this.threshHold = initialThreshHold;
				this.deltaPercent = 100 - expense;
			}

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

			internal int GetHandleCount()
			{
				int num;
				lock (this)
				{
					num = this.handleCount;
				}
				return num;
			}

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

			internal readonly string name;

			private int initialThreshHold;

			private int threshHold;

			private int handleCount;

			private readonly int deltaPercent;
		}
	}
}
