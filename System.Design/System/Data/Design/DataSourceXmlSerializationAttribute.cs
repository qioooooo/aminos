using System;

namespace System.Data.Design
{
	internal abstract class DataSourceXmlSerializationAttribute : Attribute
	{
		internal DataSourceXmlSerializationAttribute()
		{
			this.specialWay = false;
		}

		public Type ItemType
		{
			get
			{
				return this.itemType;
			}
			set
			{
				this.itemType = value;
			}
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

		public bool SpecialWay
		{
			get
			{
				return this.specialWay;
			}
			set
			{
				this.specialWay = value;
			}
		}

		private bool specialWay;

		private Type itemType;

		private string name;
	}
}
