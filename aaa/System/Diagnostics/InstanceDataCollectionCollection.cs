using System;
using System.Collections;
using System.Globalization;

namespace System.Diagnostics
{
	// Token: 0x02000760 RID: 1888
	public class InstanceDataCollectionCollection : DictionaryBase
	{
		// Token: 0x060039F7 RID: 14839 RVA: 0x000F5473 File Offset: 0x000F4473
		[Obsolete("This constructor has been deprecated.  Please use System.Diagnostics.PerformanceCounterCategory.ReadCategory() to get an instance of this collection instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public InstanceDataCollectionCollection()
		{
		}

		// Token: 0x17000D86 RID: 3462
		public InstanceDataCollection this[string counterName]
		{
			get
			{
				if (counterName == null)
				{
					throw new ArgumentNullException("counterName");
				}
				object obj = counterName.ToLower(CultureInfo.InvariantCulture);
				return (InstanceDataCollection)base.Dictionary[obj];
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x060039F9 RID: 14841 RVA: 0x000F54B4 File Offset: 0x000F44B4
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x060039FA RID: 14842 RVA: 0x000F54C1 File Offset: 0x000F44C1
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x000F54D0 File Offset: 0x000F44D0
		internal void Add(string counterName, InstanceDataCollection value)
		{
			object obj = counterName.ToLower(CultureInfo.InvariantCulture);
			base.Dictionary.Add(obj, value);
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x000F54F8 File Offset: 0x000F44F8
		public bool Contains(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			object obj = counterName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(obj);
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x000F552B File Offset: 0x000F452B
		public void CopyTo(InstanceDataCollection[] counters, int index)
		{
			base.Dictionary.Values.CopyTo(counters, index);
		}
	}
}
