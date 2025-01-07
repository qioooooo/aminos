using System;
using System.Collections;

namespace System.Data.Design
{
	internal class SimpleNamedObjectCollection : ArrayList, INamedObjectCollection, ICollection, IEnumerable
	{
		protected virtual INameService NameService
		{
			get
			{
				if (SimpleNamedObjectCollection.myNameService == null)
				{
					SimpleNamedObjectCollection.myNameService = new SimpleNameService();
				}
				return SimpleNamedObjectCollection.myNameService;
			}
		}

		public INameService GetNameService()
		{
			return this.NameService;
		}

		private static SimpleNameService myNameService;
	}
}
