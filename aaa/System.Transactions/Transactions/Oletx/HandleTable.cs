using System;
using System.Collections.Generic;

namespace System.Transactions.Oletx
{
	// Token: 0x02000088 RID: 136
	internal static class HandleTable
	{
		// Token: 0x06000372 RID: 882 RVA: 0x000341D4 File Offset: 0x000335D4
		public static IntPtr AllocHandle(object target)
		{
			IntPtr intPtr;
			lock (HandleTable.syncRoot)
			{
				int num = HandleTable.FindAvailableHandle();
				HandleTable.handleTable.Add(num, target);
				intPtr = new IntPtr(num);
			}
			return intPtr;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0003422C File Offset: 0x0003362C
		public static bool FreeHandle(IntPtr handle)
		{
			bool flag;
			lock (HandleTable.syncRoot)
			{
				flag = HandleTable.handleTable.Remove(handle.ToInt32());
			}
			return flag;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00034280 File Offset: 0x00033680
		public static object FindHandle(IntPtr handle)
		{
			object obj3;
			lock (HandleTable.syncRoot)
			{
				object obj2;
				if (!HandleTable.handleTable.TryGetValue(handle.ToInt32(), out obj2))
				{
					obj3 = null;
				}
				else
				{
					obj3 = obj2;
				}
			}
			return obj3;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000342DC File Offset: 0x000336DC
		private static int FindAvailableHandle()
		{
			int num;
			do
			{
				num = ((++HandleTable.currentHandle != 0) ? HandleTable.currentHandle : (++HandleTable.currentHandle));
			}
			while (HandleTable.handleTable.ContainsKey(num));
			return num;
		}

		// Token: 0x040001CC RID: 460
		private static Dictionary<int, object> handleTable = new Dictionary<int, object>(256);

		// Token: 0x040001CD RID: 461
		private static object syncRoot = new object();

		// Token: 0x040001CE RID: 462
		private static int currentHandle;
	}
}
