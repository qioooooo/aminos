using System;

namespace System.Xml.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
	public class XmlChoiceIdentifierAttribute : Attribute
	{
		public XmlChoiceIdentifierAttribute()
		{
		}

		public XmlChoiceIdentifierAttribute(string name)
		{
			this.name = name;
		}

		public string MemberName
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		private string name;
	}
}
