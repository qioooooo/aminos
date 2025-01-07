using System;

namespace System.Xml.Serialization
{
	internal class ChoiceIdentifierAccessor : Accessor
	{
		internal string MemberName
		{
			get
			{
				return this.memberName;
			}
			set
			{
				this.memberName = value;
			}
		}

		internal string[] MemberIds
		{
			get
			{
				return this.memberIds;
			}
			set
			{
				this.memberIds = value;
			}
		}

		private string memberName;

		private string[] memberIds;
	}
}
