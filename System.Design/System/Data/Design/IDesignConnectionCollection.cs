using System;
using System.Collections;

namespace System.Data.Design
{
	internal interface IDesignConnectionCollection : INamedObjectCollection, ICollection, IEnumerable
	{
		IDesignConnection Get(string name);

		void Set(IDesignConnection connection);

		void Remove(string name);

		void Clear();
	}
}
