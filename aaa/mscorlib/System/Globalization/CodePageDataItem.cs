using System;

namespace System.Globalization
{
	// Token: 0x0200039B RID: 923
	[Serializable]
	internal class CodePageDataItem
	{
		// Token: 0x0600255D RID: 9565 RVA: 0x00068C5C File Offset: 0x00067C5C
		internal unsafe CodePageDataItem(int dataIndex)
		{
			this.m_dataIndex = dataIndex;
			this.m_codePage = 0;
			this.m_uiFamilyCodePage = EncodingTable.codePageDataPtr[dataIndex].uiFamilyCodePage;
			this.m_webName = null;
			this.m_headerName = null;
			this.m_bodyName = null;
			this.m_description = null;
			this.m_flags = EncodingTable.codePageDataPtr[dataIndex].flags;
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x00068CCD File Offset: 0x00067CCD
		public unsafe virtual string WebName
		{
			get
			{
				if (this.m_webName == null)
				{
					this.m_webName = new string(EncodingTable.codePageDataPtr[this.m_dataIndex].webName);
				}
				return this.m_webName;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x00068D01 File Offset: 0x00067D01
		public virtual int UIFamilyCodePage
		{
			get
			{
				return this.m_uiFamilyCodePage;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x00068D09 File Offset: 0x00067D09
		public unsafe virtual string HeaderName
		{
			get
			{
				if (this.m_headerName == null)
				{
					this.m_headerName = new string(EncodingTable.codePageDataPtr[this.m_dataIndex].headerName);
				}
				return this.m_headerName;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002561 RID: 9569 RVA: 0x00068D3D File Offset: 0x00067D3D
		public unsafe virtual string BodyName
		{
			get
			{
				if (this.m_bodyName == null)
				{
					this.m_bodyName = new string(EncodingTable.codePageDataPtr[this.m_dataIndex].bodyName);
				}
				return this.m_bodyName;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002562 RID: 9570 RVA: 0x00068D71 File Offset: 0x00067D71
		public virtual uint Flags
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x040010C1 RID: 4289
		internal int m_dataIndex;

		// Token: 0x040010C2 RID: 4290
		internal int m_codePage;

		// Token: 0x040010C3 RID: 4291
		internal int m_uiFamilyCodePage;

		// Token: 0x040010C4 RID: 4292
		internal string m_webName;

		// Token: 0x040010C5 RID: 4293
		internal string m_headerName;

		// Token: 0x040010C6 RID: 4294
		internal string m_bodyName;

		// Token: 0x040010C7 RID: 4295
		internal string m_description;

		// Token: 0x040010C8 RID: 4296
		internal uint m_flags;
	}
}
