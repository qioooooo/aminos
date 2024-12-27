using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000294 RID: 660
	[TypeDependency("System.Collections.Generic.GenericArraySortHelper`2")]
	internal class ArraySortHelper<TKey, TValue> : IArraySortHelper<TKey, TValue>
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x00045794 File Offset: 0x00044794
		public static IArraySortHelper<TKey, TValue> Default
		{
			get
			{
				IArraySortHelper<TKey, TValue> arraySortHelper = ArraySortHelper<TKey, TValue>.defaultArraySortHelper;
				if (arraySortHelper == null)
				{
					arraySortHelper = ArraySortHelper<TKey, TValue>.CreateArraySortHelper();
				}
				return arraySortHelper;
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x000457B4 File Offset: 0x000447B4
		public static IArraySortHelper<TKey, TValue> CreateArraySortHelper()
		{
			if (typeof(IComparable<TKey>).IsAssignableFrom(typeof(TKey)))
			{
				ArraySortHelper<TKey, TValue>.defaultArraySortHelper = (IArraySortHelper<TKey, TValue>)typeof(GenericArraySortHelper<string, string>).TypeHandle.Instantiate(new RuntimeTypeHandle[]
				{
					typeof(TKey).TypeHandle,
					typeof(TValue).TypeHandle
				}).Allocate();
			}
			else
			{
				ArraySortHelper<TKey, TValue>.defaultArraySortHelper = new ArraySortHelper<TKey, TValue>();
			}
			return ArraySortHelper<TKey, TValue>.defaultArraySortHelper;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00045854 File Offset: 0x00044854
		public void Sort(TKey[] keys, TValue[] values, int index, int length, IComparer<TKey> comparer)
		{
			try
			{
				if (comparer == null || comparer == Comparer<TKey>.Default)
				{
					comparer = Comparer<TKey>.Default;
				}
				ArraySortHelper<TKey, TValue>.QuickSort(keys, values, index, index + (length - 1), comparer);
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_BogusIComparer", new object[]
				{
					null,
					typeof(TKey).Name,
					comparer
				}));
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_IComparerFailed"), ex);
			}
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000458E8 File Offset: 0x000448E8
		private static void SwapIfGreaterWithItems(TKey[] keys, TValue[] values, IComparer<TKey> comparer, int a, int b)
		{
			if (a != b && comparer.Compare(keys[a], keys[b]) > 0)
			{
				TKey tkey = keys[a];
				keys[a] = keys[b];
				keys[b] = tkey;
				if (values != null)
				{
					TValue tvalue = values[a];
					values[a] = values[b];
					values[b] = tvalue;
				}
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x00045958 File Offset: 0x00044958
		internal static void QuickSort(TKey[] keys, TValue[] values, int left, int right, IComparer<TKey> comparer)
		{
			do
			{
				int num = left;
				int num2 = right;
				int num3 = num + (num2 - num >> 1);
				ArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, comparer, num, num3);
				ArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, comparer, num, num2);
				ArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, comparer, num3, num2);
				TKey tkey = keys[num3];
				for (;;)
				{
					if (comparer.Compare(keys[num], tkey) >= 0)
					{
						while (comparer.Compare(tkey, keys[num2]) < 0)
						{
							num2--;
						}
						if (num > num2)
						{
							break;
						}
						if (num < num2)
						{
							TKey tkey2 = keys[num];
							keys[num] = keys[num2];
							keys[num2] = tkey2;
							if (values != null)
							{
								TValue tvalue = values[num];
								values[num] = values[num2];
								values[num2] = tvalue;
							}
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
						ArraySortHelper<TKey, TValue>.QuickSort(keys, values, left, num2, comparer);
					}
					left = num;
				}
				else
				{
					if (num < right)
					{
						ArraySortHelper<TKey, TValue>.QuickSort(keys, values, num, right, comparer);
					}
					right = num2;
				}
			}
			while (left < right);
		}

		// Token: 0x040009EE RID: 2542
		private static IArraySortHelper<TKey, TValue> defaultArraySortHelper;
	}
}
