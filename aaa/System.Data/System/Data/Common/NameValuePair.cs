using System;
using System.Runtime.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000154 RID: 340
	[Serializable]
	internal sealed class NameValuePair
	{
		// Token: 0x0600158F RID: 5519 RVA: 0x0022BB40 File Offset: 0x0022AF40
		internal NameValuePair(string name, string value, int length)
		{
			this._name = name;
			this._value = value;
			this._length = length;
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x0022BB68 File Offset: 0x0022AF68
		internal int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x0022BB7C File Offset: 0x0022AF7C
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x0022BB90 File Offset: 0x0022AF90
		// (set) Token: 0x06001593 RID: 5523 RVA: 0x0022BBA4 File Offset: 0x0022AFA4
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

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x0022BBCC File Offset: 0x0022AFCC
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x04000CA0 RID: 3232
		private readonly string _name;

		// Token: 0x04000CA1 RID: 3233
		private readonly string _value;

		// Token: 0x04000CA2 RID: 3234
		[OptionalField(VersionAdded = 2)]
		private readonly int _length;

		// Token: 0x04000CA3 RID: 3235
		private NameValuePair _next;
	}
}
