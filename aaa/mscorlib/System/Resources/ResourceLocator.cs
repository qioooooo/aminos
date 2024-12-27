using System;

namespace System.Resources
{
	// Token: 0x02000420 RID: 1056
	internal struct ResourceLocator
	{
		// Token: 0x06002BB1 RID: 11185 RVA: 0x00093D3B File Offset: 0x00092D3B
		internal ResourceLocator(int dataPos, object value)
		{
			this._dataPos = dataPos;
			this._value = value;
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x00093D4B File Offset: 0x00092D4B
		internal int DataPosition
		{
			get
			{
				return this._dataPos;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002BB3 RID: 11187 RVA: 0x00093D53 File Offset: 0x00092D53
		// (set) Token: 0x06002BB4 RID: 11188 RVA: 0x00093D5B File Offset: 0x00092D5B
		internal object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x00093D64 File Offset: 0x00092D64
		internal static bool CanCache(ResourceTypeCode value)
		{
			return value <= ResourceTypeCode.TimeSpan;
		}

		// Token: 0x04001525 RID: 5413
		internal object _value;

		// Token: 0x04001526 RID: 5414
		internal int _dataPos;
	}
}
