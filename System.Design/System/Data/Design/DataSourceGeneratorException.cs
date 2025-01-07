using System;

namespace System.Data.Design
{
	internal sealed class DataSourceGeneratorException : Exception
	{
		internal DataSourceGeneratorException(string message)
			: base(message)
		{
		}
	}
}
