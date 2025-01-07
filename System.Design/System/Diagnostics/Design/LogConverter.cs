using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Diagnostics.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class LogConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				return ((string)value).Trim();
			}
			return base.ConvertFrom(context, culture, value);
		}

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

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		private TypeConverter.StandardValuesCollection values;

		private string oldMachineName;
	}
}
