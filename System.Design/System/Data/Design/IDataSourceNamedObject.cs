using System;

namespace System.Data.Design
{
	internal interface IDataSourceNamedObject : INamedObject
	{
		string PublicTypeName { get; }
	}
}
