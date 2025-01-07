using System;

namespace System.ComponentModel.Design.Serialization
{
	public sealed class SerializeAbsoluteContext
	{
		public SerializeAbsoluteContext()
		{
		}

		public SerializeAbsoluteContext(MemberDescriptor member)
		{
			this._member = member;
		}

		public MemberDescriptor Member
		{
			get
			{
				return this._member;
			}
		}

		public bool ShouldSerialize(MemberDescriptor member)
		{
			return this._member == null || this._member == member;
		}

		private MemberDescriptor _member;
	}
}
