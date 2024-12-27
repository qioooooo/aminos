using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D8 RID: 2008
	[Serializable]
	internal sealed class IntSizedArray : ICloneable
	{
		// Token: 0x06004764 RID: 18276 RVA: 0x000F59D4 File Offset: 0x000F49D4
		public IntSizedArray()
		{
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x000F59F8 File Offset: 0x000F49F8
		private IntSizedArray(IntSizedArray sizedArray)
		{
			this.objects = new int[sizedArray.objects.Length];
			sizedArray.objects.CopyTo(this.objects, 0);
			this.negObjects = new int[sizedArray.negObjects.Length];
			sizedArray.negObjects.CopyTo(this.negObjects, 0);
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x000F5A6E File Offset: 0x000F4A6E
		public object Clone()
		{
			return new IntSizedArray(this);
		}

		// Token: 0x17000C7C RID: 3196
		internal int this[int index]
		{
			get
			{
				if (index < 0)
				{
					if (-index > this.negObjects.Length - 1)
					{
						return 0;
					}
					return this.negObjects[-index];
				}
				else
				{
					if (index > this.objects.Length - 1)
					{
						return 0;
					}
					return this.objects[index];
				}
			}
			set
			{
				if (index < 0)
				{
					if (-index > this.negObjects.Length - 1)
					{
						this.IncreaseCapacity(index);
					}
					this.negObjects[-index] = value;
					return;
				}
				if (index > this.objects.Length - 1)
				{
					this.IncreaseCapacity(index);
				}
				this.objects[index] = value;
			}
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x000F5B00 File Offset: 0x000F4B00
		internal void IncreaseCapacity(int index)
		{
			try
			{
				if (index < 0)
				{
					int num = Math.Max(this.negObjects.Length * 2, -index + 1);
					int[] array = new int[num];
					Array.Copy(this.negObjects, 0, array, 0, this.negObjects.Length);
					this.negObjects = array;
				}
				else
				{
					int num2 = Math.Max(this.objects.Length * 2, index + 1);
					int[] array2 = new int[num2];
					Array.Copy(this.objects, 0, array2, 0, this.objects.Length);
					this.objects = array2;
				}
			}
			catch (Exception)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_CorruptedStream"));
			}
		}

		// Token: 0x0400243A RID: 9274
		internal int[] objects = new int[16];

		// Token: 0x0400243B RID: 9275
		internal int[] negObjects = new int[4];
	}
}
