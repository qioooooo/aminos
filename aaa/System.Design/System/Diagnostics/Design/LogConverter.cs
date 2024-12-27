using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Diagnostics.Design
{
	// Token: 0x02000313 RID: 787
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class LogConverter : TypeConverter
	{
		// Token: 0x06001DEC RID: 7660 RVA: 0x000AB036 File Offset: 0x000AA036
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000AB050 File Offset: 0x000AA050
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return ((string)value).Trim();
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000AB07C File Offset: 0x000AA07C
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			EventLog eventLog = ((context == null) ? null : (context.Instance as EventLog));
			string text = ".";
			if (eventLog != null)
			{
				text = eventLog.MachineName;
			}
			if (this.values != null)
			{
				if (!(text != this.oldMachineName))
				{
					goto IL_007F;
				}
			}
			try
			{
				EventLog[] eventLogs = EventLog.GetEventLogs(text);
				object[] array = new object[eventLogs.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = eventLogs[i].Log;
				}
				this.values = new TypeConverter.StandardValuesCollection(array);
				this.oldMachineName = text;
			}
			catch (Exception)
			{
			}
			IL_007F:
			return this.values;
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000AB120 File Offset: 0x000AA120
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04001720 RID: 5920
		private TypeConverter.StandardValuesCollection values;

		// Token: 0x04001721 RID: 5921
		private string oldMachineName;
	}
}
