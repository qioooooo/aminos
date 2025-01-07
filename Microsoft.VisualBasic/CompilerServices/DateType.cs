using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class DateType
	{
		private DateType()
		{
		}

		public static DateTime FromString(string Value)
		{
			return DateType.FromString(Value, Utils.GetCultureInfo());
		}

		public static DateTime FromString(string Value, CultureInfo culture)
		{
			DateTime dateTime;
			if (DateType.TryParse(Value, ref dateTime))
			{
				return dateTime;
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
			{
				Strings.Left(Value, 32),
				"Date"
			}));
		}

		public static DateTime FromObject(object Value)
		{
			if (Value == null)
			{
				DateTime dateTime;
				return dateTime;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.DateTime:
					return convertible.ToDateTime(null);
				case TypeCode.String:
					return DateType.FromString(convertible.ToString(null), Utils.GetCultureInfo());
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Date"
			}));
		}

		internal static bool TryParse(string Value, ref DateTime Result)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			return DateTime.TryParse(Utils.ToHalfwidthNumbers(Value, cultureInfo), cultureInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault, out Result);
		}
	}
}
