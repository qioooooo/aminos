using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class VBFixedArrayAttribute : Attribute
	{
		public int[] Bounds
		{
			get
			{
				if (this.SecondBound == -1)
				{
					return new int[] { this.FirstBound };
				}
				return new int[] { this.FirstBound, this.SecondBound };
			}
		}

		public int Length
		{
			get
			{
				checked
				{
					if (this.SecondBound == -1)
					{
						return this.FirstBound + 1;
					}
					return (this.FirstBound + 1) * (this.SecondBound + 1);
				}
			}
		}

		public VBFixedArrayAttribute(int UpperBound1)
		{
			if (UpperBound1 < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Invalid_VBFixedArray"));
			}
			this.FirstBound = UpperBound1;
			this.SecondBound = -1;
		}

		public VBFixedArrayAttribute(int UpperBound1, int UpperBound2)
		{
			if (UpperBound1 < 0 || UpperBound2 < 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Invalid_VBFixedArray"));
			}
			this.FirstBound = UpperBound1;
			this.SecondBound = UpperBound2;
		}

		internal int FirstBound;

		internal int SecondBound;
	}
}
