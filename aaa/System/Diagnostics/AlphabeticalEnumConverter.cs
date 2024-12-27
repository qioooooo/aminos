using System;
using System.Collections;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x0200073F RID: 1855
	internal class AlphabeticalEnumConverter : EnumConverter
	{
		// Token: 0x0600388A RID: 14474 RVA: 0x000EECB2 File Offset: 0x000EDCB2
		public AlphabeticalEnumConverter(Type type)
			: base(type)
		{
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x000EECBC File Offset: 0x000EDCBC
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (base.Values == null)
			{
				Array values = Enum.GetValues(base.EnumType);
				object[] array = new object[values.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.ConvertTo(context, null, values.GetValue(i), typeof(string));
				}
				Array.Sort(array, values, 0, values.Length, global::System.Collections.Comparer.Default);
				base.Values = new TypeConverter.StandardValuesCollection(values);
			}
			return base.Values;
		}
	}
}
