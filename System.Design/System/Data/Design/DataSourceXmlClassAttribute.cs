using System;

namespace System.Data.Design
{
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class DataSourceXmlClassAttribute : Attribute
	{
		internal DataSourceXmlClassAttribute(string elementName)
		{
			this.name = elementName;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		private string name;
	}
}
