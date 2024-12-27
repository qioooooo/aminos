using System;
using System.ComponentModel;
using System.Globalization;

namespace System.ServiceProcess.Design
{
	// Token: 0x0200003B RID: 59
	internal class ServiceNameConverter : TypeConverter
	{
		// Token: 0x06000124 RID: 292 RVA: 0x00006A8D File Offset: 0x00005A8D
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006AA8 File Offset: 0x00005AA8
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return ((string)value).Trim();
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006AD4 File Offset: 0x00005AD4
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ServiceController serviceController = ((context == null) ? null : (context.Instance as ServiceController));
			string text = ".";
			if (serviceController != null)
			{
				text = serviceController.MachineName;
			}
			if (this.values != null)
			{
				if (!(text != this.previousMachineName))
				{
					goto IL_007F;
				}
			}
			try
			{
				ServiceController[] services = ServiceController.GetServices(text);
				string[] array = new string[services.Length];
				for (int i = 0; i < services.Length; i++)
				{
					array[i] = services[i].ServiceName;
				}
				this.values = new TypeConverter.StandardValuesCollection(array);
				this.previousMachineName = text;
			}
			catch
			{
			}
			IL_007F:
			return this.values;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006B78 File Offset: 0x00005B78
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006B7B File Offset: 0x00005B7B
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0400024F RID: 591
		private TypeConverter.StandardValuesCollection values;

		// Token: 0x04000250 RID: 592
		private string previousMachineName;
	}
}
