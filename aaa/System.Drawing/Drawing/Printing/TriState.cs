using System;

namespace System.Drawing.Printing
{
	// Token: 0x0200012C RID: 300
	[Serializable]
	internal struct TriState
	{
		// Token: 0x06000F65 RID: 3941 RVA: 0x0002DE26 File Offset: 0x0002CE26
		private TriState(byte value)
		{
			this.value = value;
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0002DE2F File Offset: 0x0002CE2F
		public bool IsDefault
		{
			get
			{
				return this == TriState.Default;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0002DE41 File Offset: 0x0002CE41
		public bool IsFalse
		{
			get
			{
				return this == TriState.False;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0002DE53 File Offset: 0x0002CE53
		public bool IsNotDefault
		{
			get
			{
				return this != TriState.Default;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0002DE65 File Offset: 0x0002CE65
		public bool IsTrue
		{
			get
			{
				return this == TriState.True;
			}
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0002DE77 File Offset: 0x0002CE77
		public static bool operator ==(TriState left, TriState right)
		{
			return left.value == right.value;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0002DE89 File Offset: 0x0002CE89
		public static bool operator !=(TriState left, TriState right)
		{
			return !(left == right);
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0002DE98 File Offset: 0x0002CE98
		public override bool Equals(object o)
		{
			TriState triState = (TriState)o;
			return this.value == triState.value;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0002DEBB File Offset: 0x0002CEBB
		public override int GetHashCode()
		{
			return (int)this.value;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0002DEC3 File Offset: 0x0002CEC3
		public static implicit operator TriState(bool value)
		{
			if (!value)
			{
				return TriState.False;
			}
			return TriState.True;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0002DED3 File Offset: 0x0002CED3
		public static explicit operator bool(TriState value)
		{
			if (value.IsDefault)
			{
				throw new InvalidCastException(SR.GetString("TriStateCompareError"));
			}
			return value == TriState.True;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0002DEF9 File Offset: 0x0002CEF9
		public override string ToString()
		{
			if (this == TriState.Default)
			{
				return "Default";
			}
			if (this == TriState.False)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x04000C7C RID: 3196
		private byte value;

		// Token: 0x04000C7D RID: 3197
		public static readonly TriState Default = new TriState(0);

		// Token: 0x04000C7E RID: 3198
		public static readonly TriState False = new TriState(1);

		// Token: 0x04000C7F RID: 3199
		public static readonly TriState True = new TriState(2);
	}
}
