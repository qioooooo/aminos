using System;
using System.Text;

namespace System.Net
{
	// Token: 0x020004A7 RID: 1191
	internal class HostHeaderString
	{
		// Token: 0x06002470 RID: 9328 RVA: 0x0008F51E File Offset: 0x0008E51E
		internal HostHeaderString()
		{
			this.Init(null);
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0008F52D File Offset: 0x0008E52D
		internal HostHeaderString(string s)
		{
			this.Init(s);
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x0008F53C File Offset: 0x0008E53C
		private void Init(string s)
		{
			this.m_String = s;
			this.m_Converted = false;
			this.m_Bytes = null;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x0008F554 File Offset: 0x0008E554
		private void Convert()
		{
			if (this.m_String != null && !this.m_Converted)
			{
				this.m_Bytes = Encoding.Default.GetBytes(this.m_String);
				string @string = Encoding.Default.GetString(this.m_Bytes);
				if (string.Compare(this.m_String, @string, StringComparison.Ordinal) != 0)
				{
					this.m_Bytes = Encoding.UTF8.GetBytes(this.m_String);
				}
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x0008F5BD File Offset: 0x0008E5BD
		// (set) Token: 0x06002475 RID: 9333 RVA: 0x0008F5C5 File Offset: 0x0008E5C5
		internal string String
		{
			get
			{
				return this.m_String;
			}
			set
			{
				this.Init(value);
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002476 RID: 9334 RVA: 0x0008F5CE File Offset: 0x0008E5CE
		internal int ByteCount
		{
			get
			{
				this.Convert();
				return this.m_Bytes.Length;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x0008F5DE File Offset: 0x0008E5DE
		internal byte[] Bytes
		{
			get
			{
				this.Convert();
				return this.m_Bytes;
			}
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x0008F5EC File Offset: 0x0008E5EC
		internal void Copy(byte[] destBytes, int destByteIndex)
		{
			this.Convert();
			Array.Copy(this.m_Bytes, 0, destBytes, destByteIndex, this.m_Bytes.Length);
		}

		// Token: 0x040024BA RID: 9402
		private bool m_Converted;

		// Token: 0x040024BB RID: 9403
		private string m_String;

		// Token: 0x040024BC RID: 9404
		private byte[] m_Bytes;
	}
}
