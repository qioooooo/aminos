using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000270 RID: 624
	[TypeDependency("System.Collections.Generic.GenericComparer`1")]
	[Serializable]
	public abstract class Comparer<T> : IComparer, IComparer<T>
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001920 RID: 6432 RVA: 0x00041F6C File Offset: 0x00040F6C
		public static Comparer<T> Default
		{
			get
			{
				Comparer<T> comparer = Comparer<T>.defaultComparer;
				if (comparer == null)
				{
					comparer = Comparer<T>.CreateComparer();
					Comparer<T>.defaultComparer = comparer;
				}
				return comparer;
			}
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x00041F90 File Offset: 0x00040F90
		private static Comparer<T> CreateComparer()
		{
			Type typeFromHandle = typeof(T);
			if (typeof(IComparable<T>).IsAssignableFrom(typeFromHandle))
			{
				return (Comparer<T>)typeof(GenericComparer<int>).TypeHandle.CreateInstanceForAnotherGenericParameter(typeFromHandle);
			}
			if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				Type type = typeFromHandle.GetGenericArguments()[0];
				if (typeof(IComparable<>).MakeGenericType(new Type[] { type }).IsAssignableFrom(type))
				{
					return (Comparer<T>)typeof(NullableComparer<int>).TypeHandle.CreateInstanceForAnotherGenericParameter(type);
				}
			}
			return new ObjectComparer<T>();
		}

		// Token: 0x06001922 RID: 6434
		public abstract int Compare(T x, T y);

		// Token: 0x06001923 RID: 6435 RVA: 0x00042042 File Offset: 0x00041042
		int IComparer.Compare(object x, object y)
		{
			if (x == null)
			{
				if (y != null)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (y == null)
				{
					return 1;
				}
				if (x is T && y is T)
				{
					return this.Compare((T)((object)x), (T)((object)y));
				}
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
				return 0;
			}
		}

		// Token: 0x040009B6 RID: 2486
		private static Comparer<T> defaultComparer;
	}
}
