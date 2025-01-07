using System;
using System.Collections;

namespace System.Data.Design
{
	internal interface INamedObjectCollection : ICollection, IEnumerable
	{
		INameService GetNameService();
	}
}
