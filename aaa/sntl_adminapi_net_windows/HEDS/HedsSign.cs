using System;
using System.IO;

namespace HEDS
{
	// Token: 0x02000016 RID: 22
	internal class HedsSign
	{
		// Token: 0x06000070 RID: 112 RVA: 0x000032B4 File Offset: 0x000014B4
		private bool IsValidMagic()
		{
			return this.hdr.ulMagic == 1396983112;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000032E4 File Offset: 0x000014E4
		public bool LoadSignature(Stream s)
		{
			BinaryReader binaryReader = new BinaryReader(s);
			this.hdr.ulMagic = binaryReader.ReadInt32();
			bool flag;
			if (!this.IsValidMagic())
			{
				flag = false;
			}
			else
			{
				this.hdr.ulCount = binaryReader.ReadInt32();
				if (this.hdr.ulCount > 16)
				{
					flag = false;
				}
				else
				{
					this.sign = new HedsSign.Signature[this.hdr.ulCount];
					for (int i = 0; i < this.hdr.ulCount; i++)
					{
						this.sign[i] = new HedsSign.Signature();
						this.sign[i].iGeneration = binaryReader.ReadInt32();
						this.sign[i].iSignatureLen = binaryReader.ReadInt32();
						this.sign[i].bSignature = new byte[this.sign[i].iSignatureLen];
						binaryReader.Read(this.sign[i].bSignature, 0, this.sign[i].bSignature.Length);
					}
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000033FC File Offset: 0x000015FC
		public byte[] GetSignature(int iGen)
		{
			for (int i = 0; i < this.hdr.ulCount; i++)
			{
				if (this.sign[i].iGeneration == iGen)
				{
					return this.sign[i].bSignature;
				}
			}
			return null;
		}

		// Token: 0x04000053 RID: 83
		private HedsSign.Header hdr = new HedsSign.Header();

		// Token: 0x04000054 RID: 84
		private HedsSign.Signature[] sign;

		// Token: 0x02000017 RID: 23
		private class Header
		{
			// Token: 0x04000055 RID: 85
			public int ulMagic = 1396983112;

			// Token: 0x04000056 RID: 86
			public int ulCount = 0;
		}

		// Token: 0x02000018 RID: 24
		private class Signature
		{
			// Token: 0x04000057 RID: 87
			public int iGeneration = 0;

			// Token: 0x04000058 RID: 88
			public int iSignatureLen = 0;

			// Token: 0x04000059 RID: 89
			public byte[] bSignature;
		}
	}
}
