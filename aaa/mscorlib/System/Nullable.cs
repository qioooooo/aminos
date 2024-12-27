using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x0200026E RID: 622
	[TypeDependency("System.Collections.Generic.NullableComparer`1")]
	[TypeDependency("System.Collections.Generic.NullableEqualityComparer`1")]
	[Serializable]
	public struct Nullable<T> where T : struct
	{
		// Token: 0x06001913 RID: 6419 RVA: 0x00041DEC File Offset: 0x00040DEC
		public Nullable(T value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001914 RID: 6420 RVA: 0x00041DFC File Offset: 0x00040DFC
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001915 RID: 6421 RVA: 0x00041E04 File Offset: 0x00040E04
		public T Value
		{
			get
			{
				if (this == null)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NoValue);
				}
				return this.value;
			}
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00041E1B File Offset: 0x00040E1B
		public T GetValueOrDefault()
		{
			return this.value;
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x00041E23 File Offset: 0x00040E23
		public T GetValueOrDefault(T defaultValue)
		{
			if (this == null)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x00041E35 File Offset: 0x00040E35
		public override bool Equals(object other)
		{
			if (this == null)
			{
				return other == null;
			}
			return other != null && this.value.Equals(other);
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x00041E5B File Offset: 0x00040E5B
		public override int GetHashCode()
		{
			if (this == null)
			{
				return 0;
			}
			return this.value.GetHashCode();
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x00041E78 File Offset: 0x00040E78
		public override string ToString()
		{
			if (this == null)
			{
				return "";
			}
			return this.value.ToString();
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x00041E99 File Offset: 0x00040E99
		public static implicit operator T?(T value)
		{
			return new T?(value);
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x00041EA1 File Offset: 0x00040EA1
		public static explicit operator T(T? value)
		{
			return value.Value;
		}

		// Token: 0x040009B4 RID: 2484
		private bool hasValue;

		// Token: 0x040009B5 RID: 2485
		internal T value;
	}
}
