using System;
using System.Text;

namespace System.Data.OracleClient
{
	// Token: 0x02000064 RID: 100
	internal sealed class OracleEncoding : Encoding
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00066BD4 File Offset: 0x00065FD4
		internal OciHandle Handle
		{
			get
			{
				OciHandle ociHandle = this._connection.SessionHandle;
				if (ociHandle == null || ociHandle.IsInvalid)
				{
					ociHandle = this._connection.EnvironmentHandle;
				}
				return ociHandle;
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00066C08 File Offset: 0x00066008
		public OracleEncoding(OracleInternalConnection connection)
		{
			this._connection = connection;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00066C24 File Offset: 0x00066024
		public override int GetByteCount(char[] chars, int index, int count)
		{
			return this.GetBytes(chars, index, count, null, 0);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00066C40 File Offset: 0x00066040
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			OciHandle handle = this.Handle;
			return checked((int)handle.GetBytes(chars, charIndex, (uint)charCount, bytes, byteIndex));
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00066C64 File Offset: 0x00066064
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return this.GetChars(bytes, index, count, null, 0);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00066C80 File Offset: 0x00066080
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			OciHandle handle = this.Handle;
			return checked((int)handle.GetChars(bytes, byteIndex, (uint)byteCount, chars, charIndex));
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00066CA4 File Offset: 0x000660A4
		public override int GetMaxByteCount(int charCount)
		{
			return checked(charCount * 4);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00066CB4 File Offset: 0x000660B4
		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}

		// Token: 0x0400042A RID: 1066
		private OracleInternalConnection _connection;
	}
}
