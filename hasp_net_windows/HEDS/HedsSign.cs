using System;
using System.IO;

namespace HEDS
{
	internal class HedsSign
	{
		private bool IsValidMagic()
		{
			return this.hdr.ulMagic == 1396983112;
		}

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

		private HedsSign.Header hdr = new HedsSign.Header();

		private HedsSign.Signature[] sign;

		private class Header
		{
			public int ulMagic = 1396983112;

			public int ulCount = 0;
		}

		private class Signature
		{
			public int iGeneration = 0;

			public int iSignatureLen = 0;

			public byte[] bSignature;
		}
	}
}
