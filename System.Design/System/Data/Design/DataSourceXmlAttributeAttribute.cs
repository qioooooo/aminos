using System;

namespace System.Data.Design
{
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DataSourceXmlAttributeAttribute : DataSourceXmlSerializationAttribute
	{
		internal DataSourceXmlAttributeAttribute()
			: this(null)
		{
		}

		internal DataSourceXmlAttributeAttribute(string attributeName)
		{
			base.Name = attributeName;
		}
	}
}
