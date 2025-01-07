using System;

namespace System.Web.UI.Design
{
	public interface IDataSourceSchema
	{
		IDataSourceViewSchema[] GetViews();
	}
}
