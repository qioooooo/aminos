using System;
using Microsoft.Win32;

namespace System.Drawing.Internal
{
	// Token: 0x020000EC RID: 236
	internal class SystemColorTracker
	{
		// Token: 0x06000D48 RID: 3400 RVA: 0x00027895 File Offset: 0x00026895
		private SystemColorTracker()
		{
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000278A0 File Offset: 0x000268A0
		internal static void Add(ISystemColorTracker obj)
		{
			lock (typeof(SystemColorTracker))
			{
				if (SystemColorTracker.list.Length == SystemColorTracker.count)
				{
					SystemColorTracker.GarbageCollectList();
				}
				if (!SystemColorTracker.addedTracker)
				{
					SystemColorTracker.addedTracker = true;
					SystemEvents.UserPreferenceChanged += SystemColorTracker.OnUserPreferenceChanged;
				}
				int num = SystemColorTracker.count;
				SystemColorTracker.count++;
				if (SystemColorTracker.list[num] == null)
				{
					SystemColorTracker.list[num] = new WeakReference(obj);
				}
				else
				{
					SystemColorTracker.list[num].Target = obj;
				}
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00027940 File Offset: 0x00026940
		private static void CleanOutBrokenLinks()
		{
			int num = SystemColorTracker.list.Length - 1;
			int num2 = 0;
			int num3 = SystemColorTracker.list.Length;
			for (;;)
			{
				if (num2 < num3)
				{
					if (SystemColorTracker.list[num2].Target != null)
					{
						num2++;
						continue;
					}
				}
				while (num >= 0 && SystemColorTracker.list[num].Target == null)
				{
					num--;
				}
				if (num2 >= num)
				{
					break;
				}
				WeakReference weakReference = SystemColorTracker.list[num2];
				SystemColorTracker.list[num2] = SystemColorTracker.list[num];
				SystemColorTracker.list[num] = weakReference;
				num2++;
				num--;
			}
			SystemColorTracker.count = num2;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x000279C4 File Offset: 0x000269C4
		private static void GarbageCollectList()
		{
			SystemColorTracker.CleanOutBrokenLinks();
			if ((float)SystemColorTracker.count / (float)SystemColorTracker.list.Length > SystemColorTracker.EXPAND_THRESHOLD)
			{
				WeakReference[] array = new WeakReference[SystemColorTracker.list.Length * SystemColorTracker.EXPAND_FACTOR];
				SystemColorTracker.list.CopyTo(array, 0);
				SystemColorTracker.list = array;
				int num = SystemColorTracker.list.Length;
				int warning_SIZE = SystemColorTracker.WARNING_SIZE;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00027A20 File Offset: 0x00026A20
		private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Color)
			{
				for (int i = 0; i < SystemColorTracker.count; i++)
				{
					ISystemColorTracker systemColorTracker = (ISystemColorTracker)SystemColorTracker.list[i].Target;
					if (systemColorTracker != null)
					{
						systemColorTracker.OnSystemColorChanged();
					}
				}
			}
		}

		// Token: 0x04000B3E RID: 2878
		private static int INITIAL_SIZE = 200;

		// Token: 0x04000B3F RID: 2879
		private static int WARNING_SIZE = 100000;

		// Token: 0x04000B40 RID: 2880
		private static float EXPAND_THRESHOLD = 0.75f;

		// Token: 0x04000B41 RID: 2881
		private static int EXPAND_FACTOR = 2;

		// Token: 0x04000B42 RID: 2882
		private static WeakReference[] list = new WeakReference[SystemColorTracker.INITIAL_SIZE];

		// Token: 0x04000B43 RID: 2883
		private static int count = 0;

		// Token: 0x04000B44 RID: 2884
		private static bool addedTracker;
	}
}
