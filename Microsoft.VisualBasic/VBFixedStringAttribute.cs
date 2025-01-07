using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class VBFixedStringAttribute : Attribute
	{
		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		public VBFixedStringAttribute(int Length)
		{
			if (Length < 1 || Length > 32767)
			{
				throw new ArgumentException(Utils.GetResourceString("Invalid_VBFixedString"));
			}
			this.m_Length = Length;
		}

		private int m_Length;
	}
}
