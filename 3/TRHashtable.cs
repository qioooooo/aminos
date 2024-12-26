using System;

namespace Microsoft.JScript
{
	// Token: 0x02000128 RID: 296
	internal sealed class TRHashtable
	{
		// Token: 0x06000C91 RID: 3217 RVA: 0x0005BC2C File Offset: 0x0005AC2C
		internal TRHashtable()
		{
			this.table = new TypeReflector[511];
			this.count = 0;
			this.threshold = 256;
		}

		// Token: 0x17000287 RID: 647
		internal TypeReflector this[Type type]
		{
			get
			{
				uint hashCode = (uint)type.GetHashCode();
				int num = (int)(hashCode % (uint)this.table.Length);
				for (TypeReflector typeReflector = this.table[num]; typeReflector != null; typeReflector = typeReflector.next)
				{
					if (typeReflector.type == type)
					{
						return typeReflector;
					}
				}
				return null;
			}
			set
			{
				if (++this.count >= this.threshold)
				{
					this.Rehash();
				}
				int num = (int)(value.hashCode % (uint)this.table.Length);
				value.next = this.table[num];
				this.table[num] = value;
			}
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0005BCEC File Offset: 0x0005ACEC
		private void Rehash()
		{
			TypeReflector[] array = this.table;
			int num = (this.threshold = array.Length + 1);
			int num2 = num * 2 - 1;
			TypeReflector[] array2 = (this.table = new TypeReflector[num2]);
			int num3 = num - 1;
			while (num3-- > 0)
			{
				TypeReflector typeReflector = array[num3];
				while (typeReflector != null)
				{
					TypeReflector typeReflector2 = typeReflector;
					typeReflector = typeReflector.next;
					int num4 = (int)(typeReflector2.hashCode % (uint)num2);
					typeReflector2.next = array2[num4];
					array2[num4] = typeReflector2;
				}
			}
		}

		// Token: 0x04000723 RID: 1827
		private TypeReflector[] table;

		// Token: 0x04000724 RID: 1828
		private int count;

		// Token: 0x04000725 RID: 1829
		private int threshold;
	}
}
