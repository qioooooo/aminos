using System;

namespace System.Collections.Generic
{
	// Token: 0x02000292 RID: 658
	[Serializable]
	internal class GenericArraySortHelper<T> : IArraySortHelper<T> where T : IComparable<T>
	{
		// Token: 0x06001A4B RID: 6731 RVA: 0x000454CC File Offset: 0x000444CC
		public void Sort(T[] keys, int index, int length, IComparer<T> comparer)
		{
			try
			{
				if (comparer == null || comparer == Comparer<T>.Default)
				{
					GenericArraySortHelper<T>.QuickSort(keys, index, index + (length - 1));
				}
				else
				{
					ArraySortHelper<T>.QuickSort(keys, index, index + (length - 1), comparer);
				}
			}
			catch (IndexOutOfRangeException)
			{
				string text = "Arg_BogusIComparer";
				object[] array = new object[3];
				array[0] = default(T);
				array[1] = typeof(T).Name;
				throw new ArgumentException(Environment.GetResourceString(text, array));
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_IComparerFailed"), ex);
			}
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x00045570 File Offset: 0x00044570
		public int BinarySearch(T[] array, int index, int length, T value, IComparer<T> comparer)
		{
			int num;
			try
			{
				if (comparer == null || comparer == Comparer<T>.Default)
				{
					num = GenericArraySortHelper<T>.BinarySearch(array, index, length, value);
				}
				else
				{
					num = ArraySortHelper<T>.InternalBinarySearch(array, index, length, value, comparer);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_IComparerFailed"), ex);
			}
			return num;
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x000455CC File Offset: 0x000445CC
		private static int BinarySearch(T[] array, int index, int length, T value)
		{
			int i = index;
			int num = index + length - 1;
			while (i <= num)
			{
				int num2 = i + (num - i >> 1);
				int num3;
				if (array[num2] == null)
				{
					num3 = ((value == null) ? 0 : (-1));
				}
				else
				{
					num3 = array[num2].CompareTo(value);
				}
				if (num3 == 0)
				{
					return num2;
				}
				if (num3 < 0)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return ~i;
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x00045638 File Offset: 0x00044638
		private static void SwapIfGreaterWithItems(T[] keys, int a, int b)
		{
			if (a != b && keys[a] != null && keys[a].CompareTo(keys[b]) > 0)
			{
				T t = keys[a];
				keys[a] = keys[b];
				keys[b] = t;
			}
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x00045694 File Offset: 0x00044694
		private static void QuickSort(T[] keys, int left, int right)
		{
			do
			{
				int num = left;
				int num2 = right;
				int num3 = num + (num2 - num >> 1);
				GenericArraySortHelper<T>.SwapIfGreaterWithItems(keys, num, num3);
				GenericArraySortHelper<T>.SwapIfGreaterWithItems(keys, num, num2);
				GenericArraySortHelper<T>.SwapIfGreaterWithItems(keys, num3, num2);
				T t = keys[num3];
				do
				{
					if (t == null)
					{
						while (keys[num2] != null)
						{
							num2--;
						}
					}
					else
					{
						while (t.CompareTo(keys[num]) > 0)
						{
							num++;
						}
						while (t.CompareTo(keys[num2]) < 0)
						{
							num2--;
						}
					}
					if (num > num2)
					{
						break;
					}
					if (num < num2)
					{
						T t2 = keys[num];
						keys[num] = keys[num2];
						keys[num2] = t2;
					}
					num++;
					num2--;
				}
				while (num <= num2);
				if (num2 - left <= right - num)
				{
					if (left < num2)
					{
						GenericArraySortHelper<T>.QuickSort(keys, left, num2);
					}
					left = num;
				}
				else
				{
					if (num < right)
					{
						GenericArraySortHelper<T>.QuickSort(keys, num, right);
					}
					right = num2;
				}
			}
			while (left < right);
		}
	}
}
