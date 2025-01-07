using System;
using System.Collections;

namespace System.Web.UI.Design
{
	public interface IDataSourceProvider
	{
		object GetSelectedDataSource();

		IEnumerable GetResolvedSelectedDataSource();
	}
}
