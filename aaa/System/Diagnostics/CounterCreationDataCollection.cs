using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000743 RID: 1859
	[Serializable]
	public class CounterCreationDataCollection : CollectionBase
	{
		// Token: 0x060038A6 RID: 14502 RVA: 0x000EF29F File Offset: 0x000EE29F
		public CounterCreationDataCollection()
		{
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x000EF2A7 File Offset: 0x000EE2A7
		public CounterCreationDataCollection(CounterCreationDataCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x000EF2B6 File Offset: 0x000EE2B6
		public CounterCreationDataCollection(CounterCreationData[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000D1E RID: 3358
		public CounterCreationData this[int index]
		{
			get
			{
				return (CounterCreationData)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x000EF2E7 File Offset: 0x000EE2E7
		public int Add(CounterCreationData value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x000EF2F8 File Offset: 0x000EE2F8
		public void AddRange(CounterCreationData[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x000EF32C File Offset: 0x000EE32C
		public void AddRange(CounterCreationDataCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000EF368 File Offset: 0x000EE368
		public bool Contains(CounterCreationData value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x000EF376 File Offset: 0x000EE376
		public void CopyTo(CounterCreationData[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x000EF385 File Offset: 0x000EE385
		public int IndexOf(CounterCreationData value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x000EF393 File Offset: 0x000EE393
		public void Insert(int index, CounterCreationData value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x000EF3A2 File Offset: 0x000EE3A2
		public virtual void Remove(CounterCreationData value)
		{
			base.List.Remove(value);
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x000EF3B0 File Offset: 0x000EE3B0
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is CounterCreationData))
			{
				throw new ArgumentException(SR.GetString("MustAddCounterCreationData"));
			}
		}
	}
}
