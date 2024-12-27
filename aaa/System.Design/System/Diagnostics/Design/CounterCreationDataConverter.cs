using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Diagnostics.Design
{
	// Token: 0x020000DC RID: 220
	internal class CounterCreationDataConverter : ExpandableObjectConverter
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x0001EA0F File Offset: 0x0001DA0F
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001EA28 File Offset: 0x0001DA28
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is CounterCreationData)
			{
				CounterCreationData counterCreationData = (CounterCreationData)value;
				ConstructorInfo constructor = typeof(CounterCreationData).GetConstructor(new Type[]
				{
					typeof(string),
					typeof(string),
					typeof(PerformanceCounterType)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[] { counterCreationData.CounterName, counterCreationData.CounterHelp, counterCreationData.CounterType });
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
