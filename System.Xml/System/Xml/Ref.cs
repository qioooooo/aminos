using System;

namespace System.Xml
{
	internal abstract class Ref
	{
		public static bool Equal(string strA, string strB)
		{
			return strA == strB;
		}
	}
}
