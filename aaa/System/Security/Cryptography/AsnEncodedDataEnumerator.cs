using System;
using System.Collections;

namespace System.Security.Cryptography
{
	// Token: 0x020002C7 RID: 711
	public sealed class AsnEncodedDataEnumerator : IEnumerator
	{
		// Token: 0x0600184F RID: 6223 RVA: 0x000539AE File Offset: 0x000529AE
		private AsnEncodedDataEnumerator()
		{
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x000539B6 File Offset: 0x000529B6
		internal AsnEncodedDataEnumerator(AsnEncodedDataCollection asnEncodedDatas)
		{
			this.m_asnEncodedDatas = asnEncodedDatas;
			this.m_current = -1;
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001851 RID: 6225 RVA: 0x000539CC File Offset: 0x000529CC
		public AsnEncodedData Current
		{
			get
			{
				return this.m_asnEncodedDatas[this.m_current];
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001852 RID: 6226 RVA: 0x000539DF File Offset: 0x000529DF
		object IEnumerator.Current
		{
			get
			{
				return this.m_asnEncodedDatas[this.m_current];
			}
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x000539F2 File Offset: 0x000529F2
		public bool MoveNext()
		{
			if (this.m_current == this.m_asnEncodedDatas.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x00053A1A File Offset: 0x00052A1A
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001631 RID: 5681
		private AsnEncodedDataCollection m_asnEncodedDatas;

		// Token: 0x04001632 RID: 5682
		private int m_current;
	}
}
