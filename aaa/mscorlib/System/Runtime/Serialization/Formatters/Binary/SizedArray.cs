using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D7 RID: 2007
	[Serializable]
	internal sealed class SizedArray : ICloneable
	{
		// Token: 0x0600475D RID: 18269 RVA: 0x000F57F7 File Offset: 0x000F47F7
		internal SizedArray()
		{
			this.objects = new object[16];
			this.negObjects = new object[4];
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x000F5818 File Offset: 0x000F4818
		internal SizedArray(int length)
		{
			this.objects = new object[length];
			this.negObjects = new object[length];
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x000F5838 File Offset: 0x000F4838
		private SizedArray(SizedArray sizedArray)
		{
			this.objects = new object[sizedArray.objects.Length];
			sizedArray.objects.CopyTo(this.objects, 0);
			this.negObjects = new object[sizedArray.negObjects.Length];
			sizedArray.negObjects.CopyTo(this.negObjects, 0);
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x000F5895 File Offset: 0x000F4895
		public object Clone()
		{
			return new SizedArray(this);
		}

		// Token: 0x17000C7B RID: 3195
		internal object this[int index]
		{
			get
			{
				if (index < 0)
				{
					if (-index > this.negObjects.Length - 1)
					{
						return null;
					}
					return this.negObjects[-index];
				}
				else
				{
					if (index > this.objects.Length - 1)
					{
						return null;
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
				object obj = this.objects[index];
				this.objects[index] = value;
			}
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x000F592C File Offset: 0x000F492C
		internal void IncreaseCapacity(int index)
		{
			try
			{
				if (index < 0)
				{
					int num = Math.Max(this.negObjects.Length * 2, -index + 1);
					object[] array = new object[num];
					Array.Copy(this.negObjects, 0, array, 0, this.negObjects.Length);
					this.negObjects = array;
				}
				else
				{
					int num2 = Math.Max(this.objects.Length * 2, index + 1);
					object[] array2 = new object[num2];
					Array.Copy(this.objects, 0, array2, 0, this.objects.Length);
					this.objects = array2;
				}
			}
			catch (Exception)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_CorruptedStream"));
			}
		}

		// Token: 0x04002438 RID: 9272
		internal object[] objects;

		// Token: 0x04002439 RID: 9273
		internal object[] negObjects;
	}
}
