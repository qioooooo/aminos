using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace System.Reflection
{
	// Token: 0x020000FF RID: 255
	[Serializable]
	internal sealed class CerArrayList<V>
	{
		// Token: 0x06000EA8 RID: 3752 RVA: 0x0002BD48 File Offset: 0x0002AD48
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal CerArrayList(List<V> list)
		{
			this.m_array = new V[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				this.m_array[i] = list[i];
			}
			this.m_count = list.Count;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0002BD9C File Offset: 0x0002AD9C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal CerArrayList(int length)
		{
			if (length < 4)
			{
				length = 4;
			}
			this.m_array = new V[length];
			this.m_count = 0;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x0002BDBE File Offset: 0x0002ADBE
		internal int Count
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0002BDC8 File Offset: 0x0002ADC8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal void Preallocate(int addition)
		{
			if (this.m_array.Length - this.m_count > addition)
			{
				return;
			}
			int num = ((this.m_array.Length * 2 > this.m_array.Length + addition) ? (this.m_array.Length * 2) : (this.m_array.Length + addition));
			V[] array = new V[num];
			for (int i = 0; i < this.m_count; i++)
			{
				array[i] = this.m_array[i];
			}
			this.m_array = array;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0002BE47 File Offset: 0x0002AE47
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Add(V value)
		{
			this.m_array[this.m_count] = value;
			this.m_count++;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0002BE69 File Offset: 0x0002AE69
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Replace(int index, V value)
		{
			if (index >= this.m_count)
			{
				throw new InvalidOperationException();
			}
			this.m_array[index] = value;
		}

		// Token: 0x170001D5 RID: 469
		internal V this[int index]
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.m_array[index];
			}
		}

		// Token: 0x040004E9 RID: 1257
		private const int MinSize = 4;

		// Token: 0x040004EA RID: 1258
		private V[] m_array;

		// Token: 0x040004EB RID: 1259
		private int m_count;
	}
}
