using System;

namespace Microsoft.JScript
{
	// Token: 0x02000013 RID: 19
	internal sealed class QuickSort
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00005FA1 File Offset: 0x00004FA1
		internal QuickSort(object obj, ScriptFunction compareFn)
		{
			this.compareFn = compareFn;
			this.obj = obj;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005FB8 File Offset: 0x00004FB8
		private int Compare(object x, object y)
		{
			if (x == null || x is Missing)
			{
				if (y == null || y is Missing)
				{
					return 0;
				}
				return 1;
			}
			else
			{
				if (y == null || y is Missing)
				{
					return -1;
				}
				if (this.compareFn == null)
				{
					return string.CompareOrdinal(Convert.ToString(x), Convert.ToString(y));
				}
				double num = Convert.ToNumber(this.compareFn.Call(new object[] { x, y }, null));
				if (num != num)
				{
					throw new JScriptException(JSError.NumberExpected);
				}
				return (int)Runtime.DoubleToInt64(num);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006040 File Offset: 0x00005040
		internal void SortObject(long left, long right)
		{
			if (right > left)
			{
				long num = left + (long)((double)(right - left) * MathObject.random());
				LateBinding.SwapValues(this.obj, (uint)num, (uint)right);
				object valueAtIndex = LateBinding.GetValueAtIndex(this.obj, (ulong)right);
				long num2 = left - 1L;
				long num3 = right;
				for (;;)
				{
					object obj = LateBinding.GetValueAtIndex(this.obj, (ulong)(num2 += 1L));
					if (num2 >= num3 || this.Compare(valueAtIndex, obj) < 0)
					{
						do
						{
							obj = LateBinding.GetValueAtIndex(this.obj, (ulong)(num3 -= 1L));
						}
						while (num3 > num2 && this.Compare(valueAtIndex, obj) <= 0);
						if (num2 >= num3)
						{
							break;
						}
						LateBinding.SwapValues(this.obj, (uint)num2, (uint)num3);
					}
				}
				LateBinding.SwapValues(this.obj, (uint)num2, (uint)right);
				this.SortObject(left, num2 - 1L);
				this.SortObject(num2 + 1L, right);
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006108 File Offset: 0x00005108
		internal void SortArray(int left, int right)
		{
			ArrayObject arrayObject = (ArrayObject)this.obj;
			if (right > left)
			{
				int num = left + (int)((double)(right - left) * MathObject.random());
				object obj = arrayObject.denseArray[num];
				arrayObject.denseArray[num] = arrayObject.denseArray[right];
				arrayObject.denseArray[right] = obj;
				int num2 = left - 1;
				int num3 = right;
				for (;;)
				{
					object obj2 = arrayObject.denseArray[++num2];
					if (num2 >= num3 || this.Compare(obj, obj2) < 0)
					{
						do
						{
							obj2 = arrayObject.denseArray[--num3];
						}
						while (num3 > num2 && this.Compare(obj, obj2) <= 0);
						if (num2 >= num3)
						{
							break;
						}
						QuickSort.Swap(arrayObject.denseArray, num2, num3);
					}
				}
				QuickSort.Swap(arrayObject.denseArray, num2, right);
				this.SortArray(left, num2 - 1);
				this.SortArray(num2 + 1, right);
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000061DC File Offset: 0x000051DC
		private static void Swap(object[] array, int i, int j)
		{
			object obj = array[i];
			array[i] = array[j];
			array[j] = obj;
		}

		// Token: 0x04000037 RID: 55
		internal ScriptFunction compareFn;

		// Token: 0x04000038 RID: 56
		internal object obj;
	}
}
