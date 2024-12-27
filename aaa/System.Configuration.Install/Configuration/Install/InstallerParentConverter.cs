using System;
using System.ComponentModel;

namespace System.Configuration.Install
{
	// Token: 0x02000011 RID: 17
	internal class InstallerParentConverter : ReferenceConverter
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00004024 File Offset: 0x00003024
		public InstallerParentConverter(Type type)
			: base(type)
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004030 File Offset: 0x00003030
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			TypeConverter.StandardValuesCollection standardValues = base.GetStandardValues(context);
			object instance = context.Instance;
			int i = 0;
			int num = 0;
			object[] array = new object[standardValues.Count - 1];
			while (i < standardValues.Count)
			{
				if (standardValues[i] != instance)
				{
					array[num] = standardValues[i];
					num++;
				}
				i++;
			}
			return new TypeConverter.StandardValuesCollection(array);
		}
	}
}
