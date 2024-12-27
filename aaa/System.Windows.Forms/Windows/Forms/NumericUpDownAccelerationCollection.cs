using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020005A7 RID: 1447
	[ListBindable(false)]
	public class NumericUpDownAccelerationCollection : MarshalByRefObject, ICollection<NumericUpDownAcceleration>, IEnumerable<NumericUpDownAcceleration>, IEnumerable
	{
		// Token: 0x06004B07 RID: 19207 RVA: 0x00110038 File Offset: 0x0010F038
		public void Add(NumericUpDownAcceleration acceleration)
		{
			if (acceleration == null)
			{
				throw new ArgumentNullException("acceleration");
			}
			int num = 0;
			while (num < this.items.Count && acceleration.Seconds >= this.items[num].Seconds)
			{
				num++;
			}
			this.items.Insert(num, acceleration);
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x0011008F File Offset: 0x0010F08F
		public void Clear()
		{
			this.items.Clear();
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x0011009C File Offset: 0x0010F09C
		public bool Contains(NumericUpDownAcceleration acceleration)
		{
			return this.items.Contains(acceleration);
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x001100AA File Offset: 0x0010F0AA
		public void CopyTo(NumericUpDownAcceleration[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06004B0B RID: 19211 RVA: 0x001100B9 File Offset: 0x0010F0B9
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06004B0C RID: 19212 RVA: 0x001100C6 File Offset: 0x0010F0C6
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x001100C9 File Offset: 0x0010F0C9
		public bool Remove(NumericUpDownAcceleration acceleration)
		{
			return this.items.Remove(acceleration);
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x001100D7 File Offset: 0x0010F0D7
		IEnumerator<NumericUpDownAcceleration> IEnumerable<NumericUpDownAcceleration>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x001100E9 File Offset: 0x0010F0E9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.items).GetEnumerator();
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x001100F6 File Offset: 0x0010F0F6
		public NumericUpDownAccelerationCollection()
		{
			this.items = new List<NumericUpDownAcceleration>();
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x0011010C File Offset: 0x0010F10C
		public void AddRange(params NumericUpDownAcceleration[] accelerations)
		{
			if (accelerations == null)
			{
				throw new ArgumentNullException("accelerations");
			}
			for (int i = 0; i < accelerations.Length; i++)
			{
				if (accelerations[i] == null)
				{
					throw new ArgumentNullException(SR.GetString("NumericUpDownAccelerationCollectionAtLeastOneEntryIsNull"));
				}
			}
			foreach (NumericUpDownAcceleration numericUpDownAcceleration in accelerations)
			{
				this.Add(numericUpDownAcceleration);
			}
		}

		// Token: 0x17000ECE RID: 3790
		public NumericUpDownAcceleration this[int index]
		{
			get
			{
				return this.items[index];
			}
		}

		// Token: 0x040030EB RID: 12523
		private List<NumericUpDownAcceleration> items;
	}
}
