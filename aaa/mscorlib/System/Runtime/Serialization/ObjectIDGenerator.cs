using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200034D RID: 845
	[ComVisible(true)]
	[Serializable]
	public class ObjectIDGenerator
	{
		// Token: 0x060021BE RID: 8638 RVA: 0x00054F30 File Offset: 0x00053F30
		public ObjectIDGenerator()
		{
			this.m_currentCount = 1;
			this.m_currentSize = ObjectIDGenerator.sizes[0];
			this.m_ids = new long[this.m_currentSize * 4];
			this.m_objs = new object[this.m_currentSize * 4];
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x00054F80 File Offset: 0x00053F80
		private int FindElement(object obj, out bool found)
		{
			int num = RuntimeHelpers.GetHashCode(obj);
			int num2 = 1 + (num & int.MaxValue) % (this.m_currentSize - 2);
			int i;
			for (;;)
			{
				int num3 = (num & int.MaxValue) % this.m_currentSize * 4;
				for (i = num3; i < num3 + 4; i++)
				{
					if (this.m_objs[i] == null)
					{
						goto Block_1;
					}
					if (this.m_objs[i] == obj)
					{
						goto Block_2;
					}
				}
				num += num2;
			}
			Block_1:
			found = false;
			return i;
			Block_2:
			found = true;
			return i;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00054FEC File Offset: 0x00053FEC
		public virtual long GetId(object obj, out bool firstTime)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", Environment.GetResourceString("ArgumentNull_Obj"));
			}
			bool flag;
			int num = this.FindElement(obj, out flag);
			long num2;
			if (!flag)
			{
				this.m_objs[num] = obj;
				this.m_ids[num] = (long)this.m_currentCount++;
				num2 = this.m_ids[num];
				if (this.m_currentCount > this.m_currentSize * 4 / 2)
				{
					this.Rehash();
				}
			}
			else
			{
				num2 = this.m_ids[num];
			}
			firstTime = !flag;
			return num2;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x00055074 File Offset: 0x00054074
		public virtual long HasId(object obj, out bool firstTime)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", Environment.GetResourceString("ArgumentNull_Obj"));
			}
			bool flag;
			int num = this.FindElement(obj, out flag);
			if (flag)
			{
				firstTime = false;
				return this.m_ids[num];
			}
			firstTime = true;
			return 0L;
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000550B8 File Offset: 0x000540B8
		private void Rehash()
		{
			int num = 0;
			int currentSize = this.m_currentSize;
			while (num < ObjectIDGenerator.sizes.Length && ObjectIDGenerator.sizes[num] <= currentSize)
			{
				num++;
			}
			if (num == ObjectIDGenerator.sizes.Length)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_TooManyElements"));
			}
			this.m_currentSize = ObjectIDGenerator.sizes[num];
			long[] array = new long[this.m_currentSize * 4];
			object[] array2 = new object[this.m_currentSize * 4];
			long[] ids = this.m_ids;
			object[] objs = this.m_objs;
			this.m_ids = array;
			this.m_objs = array2;
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i] != null)
				{
					bool flag;
					int num2 = this.FindElement(objs[i], out flag);
					this.m_objs[num2] = objs[i];
					this.m_ids[num2] = ids[i];
				}
			}
		}

		// Token: 0x04000DF6 RID: 3574
		private const int numbins = 4;

		// Token: 0x04000DF7 RID: 3575
		internal int m_currentCount;

		// Token: 0x04000DF8 RID: 3576
		internal int m_currentSize;

		// Token: 0x04000DF9 RID: 3577
		internal long[] m_ids;

		// Token: 0x04000DFA RID: 3578
		internal object[] m_objs;

		// Token: 0x04000DFB RID: 3579
		private static readonly int[] sizes = new int[]
		{
			5, 11, 29, 47, 97, 197, 397, 797, 1597, 3203,
			6421, 12853, 25717, 51437, 102877, 205759, 411527, 823117, 1646237, 3292489,
			6584983
		};
	}
}
