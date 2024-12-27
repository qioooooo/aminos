using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200016D RID: 365
	public sealed class SerializeAbsoluteContext
	{
		// Token: 0x06000D83 RID: 3459 RVA: 0x000376E3 File Offset: 0x000366E3
		public SerializeAbsoluteContext()
		{
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000376EB File Offset: 0x000366EB
		public SerializeAbsoluteContext(MemberDescriptor member)
		{
			this._member = member;
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x000376FA File Offset: 0x000366FA
		public MemberDescriptor Member
		{
			get
			{
				return this._member;
			}
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00037702 File Offset: 0x00036702
		public bool ShouldSerialize(MemberDescriptor member)
		{
			return this._member == null || this._member == member;
		}

		// Token: 0x04000F17 RID: 3863
		private MemberDescriptor _member;
	}
}
