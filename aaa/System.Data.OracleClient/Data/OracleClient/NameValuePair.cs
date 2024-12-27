using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace System.Data.OracleClient
{
	// Token: 0x02000085 RID: 133
	[Serializable]
	internal sealed class NameValuePair
	{
		// Token: 0x0600079D RID: 1949 RVA: 0x00071824 File Offset: 0x00070C24
		internal NameValuePair(string name, string value, int length)
		{
			this._name = name;
			this._value = value;
			this._length = length;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0007184C File Offset: 0x00070C4C
		internal int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00071860 File Offset: 0x00070C60
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00071874 File Offset: 0x00070C74
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x00071888 File Offset: 0x00070C88
		internal NameValuePair Next
		{
			get
			{
				return this._next;
			}
			set
			{
				if (this._next != null || value == null)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.NameValuePairNext);
				}
				this._next = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000718B0 File Offset: 0x00070CB0
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040004F5 RID: 1269
		private readonly string _name;

		// Token: 0x040004F6 RID: 1270
		private readonly string _value;

		// Token: 0x040004F7 RID: 1271
		[OptionalField(VersionAdded = 2)]
		private readonly int _length;

		// Token: 0x040004F8 RID: 1272
		private NameValuePair _next;
	}
}
