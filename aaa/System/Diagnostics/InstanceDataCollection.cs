using System;
using System.Collections;
using System.Globalization;

namespace System.Diagnostics
{
	// Token: 0x0200075F RID: 1887
	public class InstanceDataCollection : DictionaryBase
	{
		// Token: 0x060039EF RID: 14831 RVA: 0x000F537B File Offset: 0x000F437B
		[Obsolete("This constructor has been deprecated.  Please use System.Diagnostics.InstanceDataCollectionCollection.get_Item to get an instance of this collection instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public InstanceDataCollection(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			this.counterName = counterName;
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x060039F0 RID: 14832 RVA: 0x000F5398 File Offset: 0x000F4398
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x060039F1 RID: 14833 RVA: 0x000F53A0 File Offset: 0x000F43A0
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x060039F2 RID: 14834 RVA: 0x000F53AD File Offset: 0x000F43AD
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x17000D85 RID: 3461
		public InstanceData this[string instanceName]
		{
			get
			{
				if (instanceName == null)
				{
					throw new ArgumentNullException("instanceName");
				}
				if (instanceName.Length == 0)
				{
					instanceName = "systemdiagnosticsperfcounterlibsingleinstance";
				}
				object obj = instanceName.ToLower(CultureInfo.InvariantCulture);
				return (InstanceData)base.Dictionary[obj];
			}
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x000F5404 File Offset: 0x000F4404
		internal void Add(string instanceName, InstanceData value)
		{
			object obj = instanceName.ToLower(CultureInfo.InvariantCulture);
			base.Dictionary.Add(obj, value);
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x000F542C File Offset: 0x000F442C
		public bool Contains(string instanceName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			object obj = instanceName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(obj);
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x000F545F File Offset: 0x000F445F
		public void CopyTo(InstanceData[] instances, int index)
		{
			base.Dictionary.Values.CopyTo(instances, index);
		}

		// Token: 0x040032E6 RID: 13030
		private string counterName;
	}
}
