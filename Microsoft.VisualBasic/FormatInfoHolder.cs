using System;
using System.Globalization;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	internal sealed class FormatInfoHolder : IFormatProvider
	{
		internal FormatInfoHolder(NumberFormatInfo nfi)
		{
			this.nfi = nfi;
		}

		object IFormatProvider.GetFormat(Type service)
		{
			if (service == typeof(NumberFormatInfo))
			{
				return this.nfi;
			}
			throw new ArgumentException(Utils.GetResourceString("InternalError"));
		}

		private NumberFormatInfo nfi;
	}
}
