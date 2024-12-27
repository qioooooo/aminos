using System;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x0200063F RID: 1599
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongNamePublicKeyBlob
	{
		// Token: 0x06003A24 RID: 14884 RVA: 0x000C4ED2 File Offset: 0x000C3ED2
		internal StrongNamePublicKeyBlob()
		{
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x000C4EDA File Offset: 0x000C3EDA
		public StrongNamePublicKeyBlob(byte[] publicKey)
		{
			if (publicKey == null)
			{
				throw new ArgumentNullException("PublicKey");
			}
			this.PublicKey = new byte[publicKey.Length];
			Array.Copy(publicKey, 0, this.PublicKey, 0, publicKey.Length);
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x000C4F0F File Offset: 0x000C3F0F
		internal StrongNamePublicKeyBlob(string publicKey)
		{
			this.PublicKey = Hex.DecodeHexString(publicKey);
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x000C4F24 File Offset: 0x000C3F24
		private static bool CompareArrays(byte[] first, byte[] second)
		{
			if (first.Length != second.Length)
			{
				return false;
			}
			int num = first.Length;
			for (int i = 0; i < num; i++)
			{
				if (first[i] != second[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x000C4F56 File Offset: 0x000C3F56
		internal bool Equals(StrongNamePublicKeyBlob blob)
		{
			return blob != null && StrongNamePublicKeyBlob.CompareArrays(this.PublicKey, blob.PublicKey);
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x000C4F6E File Offset: 0x000C3F6E
		public override bool Equals(object obj)
		{
			return obj != null && obj is StrongNamePublicKeyBlob && this.Equals((StrongNamePublicKeyBlob)obj);
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x000C4F8C File Offset: 0x000C3F8C
		private static int GetByteArrayHashCode(byte[] baData)
		{
			if (baData == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < baData.Length; i++)
			{
				num = (num << 8) ^ (int)baData[i] ^ (num >> 24);
			}
			return num;
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x000C4FBC File Offset: 0x000C3FBC
		public override int GetHashCode()
		{
			return StrongNamePublicKeyBlob.GetByteArrayHashCode(this.PublicKey);
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x000C4FC9 File Offset: 0x000C3FC9
		public override string ToString()
		{
			return Hex.EncodeHexString(this.PublicKey);
		}

		// Token: 0x04001E09 RID: 7689
		internal byte[] PublicKey;
	}
}
