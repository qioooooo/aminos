using System;
using System.Collections;

namespace System.Xml
{
	internal sealed class EmptyEnumerator : IEnumerator
	{
		bool IEnumerator.MoveNext()
		{
			return false;
		}

		void IEnumerator.Reset()
		{
		}

		object IEnumerator.Current
		{
			get
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}
	}
}
