using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x0200027D RID: 637
	[TypeDependency("System.Collections.Generic.GenericEqualityComparer`1")]
	[Serializable]
	public abstract class EqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060019A5 RID: 6565 RVA: 0x000438B0 File Offset: 0x000428B0
		public static EqualityComparer<T> Default
		{
			get
			{
				EqualityComparer<T> equalityComparer = EqualityComparer<T>.defaultComparer;
				if (equalityComparer == null)
				{
					equalityComparer = EqualityComparer<T>.CreateComparer();
					EqualityComparer<T>.defaultComparer = equalityComparer;
				}
				return equalityComparer;
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x000438D4 File Offset: 0x000428D4
		private static EqualityComparer<T> CreateComparer()
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(byte))
			{
				return (EqualityComparer<T>)new ByteEqualityComparer();
			}
			if (typeof(IEquatable<T>).IsAssignableFrom(typeFromHandle))
			{
				return (EqualityComparer<T>)typeof(GenericEqualityComparer<int>).TypeHandle.CreateInstanceForAnotherGenericParameter(typeFromHandle);
			}
			if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				Type type = typeFromHandle.GetGenericArguments()[0];
				if (typeof(IEquatable<>).MakeGenericType(new Type[] { type }).IsAssignableFrom(type))
				{
					return (EqualityComparer<T>)typeof(NullableEqualityComparer<int>).TypeHandle.CreateInstanceForAnotherGenericParameter(type);
				}
			}
			return new ObjectEqualityComparer<T>();
		}

		// Token: 0x060019A7 RID: 6567
		public abstract bool Equals(T x, T y);

		// Token: 0x060019A8 RID: 6568
		public abstract int GetHashCode(T obj);

		// Token: 0x060019A9 RID: 6569 RVA: 0x000439A0 File Offset: 0x000429A0
		internal virtual int IndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (this.Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x000439D4 File Offset: 0x000429D4
		internal virtual int LastIndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			for (int i = startIndex; i >= num; i--)
			{
				if (this.Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00043A07 File Offset: 0x00042A07
		int IEqualityComparer.GetHashCode(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			if (obj is T)
			{
				return this.GetHashCode((T)((object)obj));
			}
			ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
			return 0;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00043A2A File Offset: 0x00042A2A
		bool IEqualityComparer.Equals(object x, object y)
		{
			if (x == y)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			if (x is T && y is T)
			{
				return this.Equals((T)((object)x), (T)((object)y));
			}
			ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
			return false;
		}

		// Token: 0x040009DB RID: 2523
		private static EqualityComparer<T> defaultComparer;
	}
}
