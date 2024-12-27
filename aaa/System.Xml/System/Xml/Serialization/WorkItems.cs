using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x02000317 RID: 791
	internal class WorkItems
	{
		// Token: 0x17000938 RID: 2360
		internal ImportStructWorkItem this[int index]
		{
			get
			{
				return (ImportStructWorkItem)this.list[index];
			}
			set
			{
				this.list[index] = value;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06002580 RID: 9600 RVA: 0x000B3468 File Offset: 0x000B2468
		internal int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000B3475 File Offset: 0x000B2475
		internal void Add(ImportStructWorkItem item)
		{
			this.list.Add(item);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000B3484 File Offset: 0x000B2484
		internal bool Contains(StructMapping mapping)
		{
			return this.IndexOf(mapping) >= 0;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000B3494 File Offset: 0x000B2494
		internal int IndexOf(StructMapping mapping)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].Mapping == mapping)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000B34C4 File Offset: 0x000B24C4
		internal void RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		// Token: 0x040015A4 RID: 5540
		private ArrayList list = new ArrayList();
	}
}
