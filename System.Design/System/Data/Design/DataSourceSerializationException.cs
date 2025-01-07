using System;

namespace System.Data.Design
{
	[Serializable]
	internal sealed class DataSourceSerializationException : ApplicationException
	{
		public DataSourceSerializationException(string message)
			: base(message)
		{
		}
	}
}
