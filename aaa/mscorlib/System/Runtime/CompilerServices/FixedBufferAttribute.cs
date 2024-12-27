using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005D1 RID: 1489
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class FixedBufferAttribute : Attribute
	{
		// Token: 0x06003795 RID: 14229 RVA: 0x000BB86F File Offset: 0x000BA86F
		public FixedBufferAttribute(Type elementType, int length)
		{
			this.elementType = elementType;
			this.length = length;
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003796 RID: 14230 RVA: 0x000BB885 File Offset: 0x000BA885
		public Type ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x000BB88D File Offset: 0x000BA88D
		public int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x04001C9B RID: 7323
		private Type elementType;

		// Token: 0x04001C9C RID: 7324
		private int length;
	}
}
