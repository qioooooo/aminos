using System;
using System.Data.Common;
using System.Globalization;

namespace System.Data.OracleClient
{
	// Token: 0x0200004C RID: 76
	public struct OracleBoolean : IComparable
	{
		// Token: 0x06000262 RID: 610 RVA: 0x0005C4C0 File Offset: 0x0005B8C0
		public OracleBoolean(bool value)
		{
			this._value = (value ? 1 : 2);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0005C4DC File Offset: 0x0005B8DC
		public OracleBoolean(int value)
		{
			this = new OracleBoolean(value, false);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0005C4F4 File Offset: 0x0005B8F4
		private OracleBoolean(int value, bool isNull)
		{
			if (isNull)
			{
				this._value = 0;
				return;
			}
			this._value = ((value != 0) ? 1 : 2);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0005C51C File Offset: 0x0005B91C
		private byte ByteValue
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0005C530 File Offset: 0x0005B930
		public bool IsFalse
		{
			get
			{
				return this._value == 2;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0005C548 File Offset: 0x0005B948
		public bool IsNull
		{
			get
			{
				return this._value == 0;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0005C560 File Offset: 0x0005B960
		public bool IsTrue
		{
			get
			{
				return this._value == 1;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0005C578 File Offset: 0x0005B978
		public bool Value
		{
			get
			{
				switch (this._value)
				{
				case 1:
					return true;
				case 2:
					return false;
				default:
					throw ADP.DataIsNull();
				}
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0005C5A8 File Offset: 0x0005B9A8
		public int CompareTo(object obj)
		{
			if (!(obj is OracleBoolean))
			{
				throw ADP.WrongType(obj.GetType(), typeof(OracleBoolean));
			}
			OracleBoolean oracleBoolean = (OracleBoolean)obj;
			if (this.IsNull)
			{
				if (!oracleBoolean.IsNull)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (oracleBoolean.IsNull)
				{
					return 1;
				}
				if (this.ByteValue < oracleBoolean.ByteValue)
				{
					return -1;
				}
				if (this.ByteValue > oracleBoolean.ByteValue)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0005C620 File Offset: 0x0005BA20
		public override bool Equals(object value)
		{
			if (!(value is OracleBoolean))
			{
				return false;
			}
			OracleBoolean oracleBoolean = (OracleBoolean)value;
			if (oracleBoolean.IsNull || this.IsNull)
			{
				return oracleBoolean.IsNull && this.IsNull;
			}
			return (this == oracleBoolean).Value;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0005C678 File Offset: 0x0005BA78
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this._value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0005C69C File Offset: 0x0005BA9C
		public static OracleBoolean Parse(string s)
		{
			OracleBoolean oracleBoolean;
			try
			{
				oracleBoolean = new OracleBoolean(int.Parse(s, CultureInfo.InvariantCulture));
			}
			catch (Exception ex)
			{
				Type type = ex.GetType();
				if (type != ADP.ArgumentNullExceptionType && type != ADP.FormatExceptionType && type != ADP.OverflowExceptionType)
				{
					throw ex;
				}
				oracleBoolean = new OracleBoolean(bool.Parse(s));
			}
			return oracleBoolean;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0005C70C File Offset: 0x0005BB0C
		public override string ToString()
		{
			if (this.IsNull)
			{
				return ADP.NullString;
			}
			return this.Value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0005C73C File Offset: 0x0005BB3C
		public static OracleBoolean And(OracleBoolean x, OracleBoolean y)
		{
			return x & y;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0005C750 File Offset: 0x0005BB50
		public static OracleBoolean Equals(OracleBoolean x, OracleBoolean y)
		{
			return x == y;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0005C764 File Offset: 0x0005BB64
		public static OracleBoolean NotEquals(OracleBoolean x, OracleBoolean y)
		{
			return x != y;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0005C778 File Offset: 0x0005BB78
		public static OracleBoolean OnesComplement(OracleBoolean x)
		{
			return ~x;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0005C78C File Offset: 0x0005BB8C
		public static OracleBoolean Or(OracleBoolean x, OracleBoolean y)
		{
			return x | y;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0005C7A0 File Offset: 0x0005BBA0
		public static OracleBoolean Xor(OracleBoolean x, OracleBoolean y)
		{
			return x ^ y;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0005C7B4 File Offset: 0x0005BBB4
		public static implicit operator OracleBoolean(bool x)
		{
			return new OracleBoolean(x);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0005C7C8 File Offset: 0x0005BBC8
		public static explicit operator OracleBoolean(string x)
		{
			return OracleBoolean.Parse(x);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0005C7DC File Offset: 0x0005BBDC
		public static explicit operator OracleBoolean(OracleNumber x)
		{
			if (!x.IsNull)
			{
				return new OracleBoolean(x.Value != 0m);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0005C810 File Offset: 0x0005BC10
		public static explicit operator bool(OracleBoolean x)
		{
			return x.Value;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0005C824 File Offset: 0x0005BC24
		public static OracleBoolean operator !(OracleBoolean x)
		{
			switch (x._value)
			{
			case 1:
				return OracleBoolean.False;
			case 2:
				return OracleBoolean.True;
			default:
				return OracleBoolean.Null;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0005C85C File Offset: 0x0005BC5C
		public static OracleBoolean operator ~(OracleBoolean x)
		{
			return !x;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0005C870 File Offset: 0x0005BC70
		public static bool operator true(OracleBoolean x)
		{
			return x.IsTrue;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0005C884 File Offset: 0x0005BC84
		public static bool operator false(OracleBoolean x)
		{
			return x.IsFalse;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0005C898 File Offset: 0x0005BC98
		public static OracleBoolean operator &(OracleBoolean x, OracleBoolean y)
		{
			if (x._value == 2 || y._value == 2)
			{
				return OracleBoolean.False;
			}
			if (x._value == 1 && y._value == 1)
			{
				return OracleBoolean.True;
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0005C8E0 File Offset: 0x0005BCE0
		public static OracleBoolean operator ==(OracleBoolean x, OracleBoolean y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x._value == y._value);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0005C91C File Offset: 0x0005BD1C
		public static OracleBoolean operator !=(OracleBoolean x, OracleBoolean y)
		{
			return !(x == y);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0005C938 File Offset: 0x0005BD38
		public static OracleBoolean operator |(OracleBoolean x, OracleBoolean y)
		{
			if (x._value == 1 || y._value == 1)
			{
				return OracleBoolean.True;
			}
			if (x._value == 2 && y._value == 2)
			{
				return OracleBoolean.False;
			}
			return OracleBoolean.Null;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0005C980 File Offset: 0x0005BD80
		public static OracleBoolean operator ^(OracleBoolean x, OracleBoolean y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new OracleBoolean(x._value != y._value);
			}
			return OracleBoolean.Null;
		}

		// Token: 0x04000333 RID: 819
		private const byte x_Null = 0;

		// Token: 0x04000334 RID: 820
		private const byte x_True = 1;

		// Token: 0x04000335 RID: 821
		private const byte x_False = 2;

		// Token: 0x04000336 RID: 822
		private byte _value;

		// Token: 0x04000337 RID: 823
		public static readonly OracleBoolean False = new OracleBoolean(false);

		// Token: 0x04000338 RID: 824
		public static readonly OracleBoolean Null = new OracleBoolean(0, true);

		// Token: 0x04000339 RID: 825
		public static readonly OracleBoolean One = new OracleBoolean(1);

		// Token: 0x0400033A RID: 826
		public static readonly OracleBoolean True = new OracleBoolean(true);

		// Token: 0x0400033B RID: 827
		public static readonly OracleBoolean Zero = new OracleBoolean(0);
	}
}
