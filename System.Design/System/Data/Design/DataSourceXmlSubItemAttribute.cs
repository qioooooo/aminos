using System;

namespace System.Data.Design
{
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DataSourceXmlSubItemAttribute : DataSourceXmlSerializationAttribute
	{
		internal DataSourceXmlSubItemAttribute()
		{
		}

		internal DataSourceXmlSubItemAttribute(Type itemType)
		{
			base.ItemType = itemType;
		}
	}
}
