using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001B1 RID: 433
	public struct MemberRelationship
	{
		// Token: 0x06000D42 RID: 3394 RVA: 0x0002AB6E File Offset: 0x00029B6E
		public MemberRelationship(object owner, MemberDescriptor member)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this._owner = owner;
			this._member = member;
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x0002AB9A File Offset: 0x00029B9A
		public bool IsEmpty
		{
			get
			{
				return this._owner == null;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x0002ABA5 File Offset: 0x00029BA5
		public MemberDescriptor Member
		{
			get
			{
				return this._member;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0002ABAD File Offset: 0x00029BAD
		public object Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0002ABB8 File Offset: 0x00029BB8
		public override bool Equals(object obj)
		{
			if (!(obj is MemberRelationship))
			{
				return false;
			}
			MemberRelationship memberRelationship = (MemberRelationship)obj;
			return memberRelationship.Owner == this.Owner && memberRelationship.Member == this.Member;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0002ABF6 File Offset: 0x00029BF6
		public override int GetHashCode()
		{
			if (this._owner == null)
			{
				return base.GetHashCode();
			}
			return this._owner.GetHashCode() ^ this._member.GetHashCode();
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0002AC28 File Offset: 0x00029C28
		public static bool operator ==(MemberRelationship left, MemberRelationship right)
		{
			return left.Owner == right.Owner && left.Member == right.Member;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0002AC4C File Offset: 0x00029C4C
		public static bool operator !=(MemberRelationship left, MemberRelationship right)
		{
			return !(left == right);
		}

		// Token: 0x04000EB4 RID: 3764
		private object _owner;

		// Token: 0x04000EB5 RID: 3765
		private MemberDescriptor _member;

		// Token: 0x04000EB6 RID: 3766
		public static readonly MemberRelationship Empty = default(MemberRelationship);
	}
}
