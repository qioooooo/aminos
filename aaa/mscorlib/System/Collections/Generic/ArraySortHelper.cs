using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000291 RID: 657
	[TypeDependency("System.Collections.Generic.GenericArraySortHelper`1")]
	internal class ArraySortHelper<T> : IArraySortHelper<T>
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x000451E8 File Offset: 0x000441E8
		public static IArraySortHelper<T> Default
		{
			get
			{
				IArraySortHelper<T> arraySortHelper = ArraySortHelper<T>.defaultArraySortHelper;
				if (arraySortHelper == null)
				{
					arraySortHelper = ArraySortHelper<T>.CreateArraySortHelper();
				}
				return arraySortHelper;
			}
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00045208 File Offset: 0x00044208
		private static IArraySortHelper<T> CreateArraySortHelper()
		{
			if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
			{
				ArraySortHelper<T>.defaultArraySortHelper = (IArraySortHelper<T>)typeof(GenericArraySortHelper<string>).TypeHandle.Instantiate(new RuntimeTypeHandle[] { typeof(T).TypeHandle }).Allocate();
			}
			else
			{
				ArraySortHelper<T>.defaultArraySortHelper = new ArraySortHelper<T>();
			}
			return ArraySortHelper<T>.defaultArraySortHelper;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00045290 File Offset: 0x00044290
		public void Sort(T[] keys, int index, int length, IComparer<T> comparer)
		{
			try
			{
				if (comparer == null)
				{
					comparer = Comparer<T>.Default;
				}
				ArraySortHelper<T>.QuickSort(keys, index, index + (length - 1), comparer);
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_BogusIComparer", new object[]
				{
					null,
					typeof(T).Name,
					comparer
				}));
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_IComparerFailed"), ex);
			}
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x00045318 File Offset: 0x00044318
		public int BinarySearch(T[] array, int index, int length, T value, IComparer<T> comparer)
		{
			int num;
			try
			{
				if (comparer == null)
				{
					comparer = Comparer<T>.Default;
				}
				num = ArraySortHelper<T>.InternalBinarySearch(array, index, length, value, comparer);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_IComparerFailed"), ex);
			}
			return num;
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x00045364 File Offset: 0x00044364
		internal static int InternalBinarySearch(T[] array, int index, int length, T value, IComparer<T> comparer)
		{
			int i = index;
			int num = index + length - 1;
			while (i <= num)
			{
				int num2 = i + (num - i >> 1);
				int num3 = comparer.Compare(array[num2], value);
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

		// Token: 0x06001A48 RID: 6728 RVA: 0x000453AC File Offset: 0x000443AC
		private static void SwapIfGreaterWithItems(T[] keys, IComparer<T> comparer, int a, int b)
		{
			if (a != b && comparer.Compare(keys[a], keys[b]) > 0)
			{
				T t = keys[a];
				keys[a] = keys[b];
				keys[b] = t;
			}
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x000453F4 File Offset: 0x000443F4
		internal static void QuickSort(T[] keys, int left, int right, IComparer<T> comparer)
		{
			do
			{
				int num = left;
				int num2 = right;
				int num3 = num + (num2 - num >> 1);
				ArraySortHelper<T>.SwapIfGreaterWithItems(keys, comparer, num, num3);
				ArraySortHelper<T>.SwapIfGreaterWithItems(keys, comparer, num, num2);
				ArraySortHelper<T>.SwapIfGreaterWithItems(keys, comparer, num3, num2);
				T t = keys[num3];
				for (;;)
				{
					if (comparer.Compare(keys[num], t) >= 0)
					{
						while (comparer.Compare(t, keys[num2]) < 0)
						{
							num2--;
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
						if (num > num2)
						{
							break;
						}
					}
					else
					{
						num++;
					}
				}
				if (num2 - left <= right - num)
				{
					if (left < num2)
					{
						ArraySortHelper<T>.QuickSort(keys, left, num2, comparer);
					}
					left = num;
				}
				else
				{
					if (num < right)
					{
						ArraySortHelper<T>.QuickSort(keys, num, right, comparer);
					}
					right = num2;
				}
			}
			while (left < right);
		}

		// Token: 0x040009ED RID: 2541
		private static IArraySortHelper<T> defaultArraySortHelper;
	}
}
