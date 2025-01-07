using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class CharArrayType
	{
		private CharArrayType()
		{
		}

		public static char[] FromString(string Value)
		{
			if (Value == null)
			{
				Value = "";
			}
			return Value.ToCharArray();
		}

		public static char[] FromObject(object Value)
		{
			if (Value == null)
			{
				return "".ToCharArray();
			}
			char[] array = Value as char[];
			if (array != null && array.Rank == 1)
			{
				return array;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null && convertible.GetTypeCode() == TypeCode.String)
			{
				return convertible.ToString(null).ToCharArray();
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Char()"
			}));
		}
	}
}
