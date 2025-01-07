using System;

namespace System.Data.Design
{
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DataSourceXmlElementAttribute : DataSourceXmlSerializationAttribute
	{
		internal DataSourceXmlElementAttribute()
			: this(null)
		{
		}

		internal DataSourceXmlElementAttribute(string elementName)
		{
			base.Name = elementName;
		}
	}
}
