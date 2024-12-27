using System;
using System.Diagnostics;
using System.IO;
using SafeNet.Sentinel;

namespace HEDS
{
	// Token: 0x02000014 RID: 20
	internal class HedsFile
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00002FDC File Offset: 0x000011DC
		public string CheckFile(string str)
		{
			string text = str;
			if (!text.EndsWith("\\"))
			{
				text += "\\";
			}
			text += this.strFileName;
			string text2;
			if (File.Exists(text))
			{
				text2 = text;
			}
			else
			{
				text2 = null;
			}
			return text2;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000302C File Offset: 0x0000122C
		public string FindFile()
		{
			try
			{
				string text = this.CheckFile(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
				if (text != null)
				{
					return text;
				}
				text = this.CheckFile(Path.GetFullPath(Environment.CurrentDirectory));
				if (text != null)
				{
					return text;
				}
				text = this.CheckFile(Path.GetFullPath(Environment.SystemDirectory));
				if (text != null)
				{
					return text;
				}
				text = this.CheckFile(Path.GetFullPath(Environment.GetEnvironmentVariable("windir") + "\\system"));
				if (text != null)
				{
					return text;
				}
				text = this.CheckFile(Path.GetFullPath(Environment.GetEnvironmentVariable("windir")));
				if (text != null)
				{
					return text;
				}
				string[] array = Environment.GetEnvironmentVariable("path").Split(new char[] { ';' });
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && array[i].Length > 0)
					{
						text = this.CheckFile(Path.GetFullPath(array[i]));
						if (text != null)
						{
							return text;
						}
					}
				}
			}
			catch (FileNotFoundException ex)
			{
			}
			return null;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000031A8 File Offset: 0x000013A8
		public HedsFile.heds_status CheckSignature(string strFile, int iGeneration)
		{
			try
			{
				if (strFile == null)
				{
					return HedsFile.heds_status.HEDS_FILE_NOT_FOUND;
				}
				this.hedsSign = new HedsSign();
				FileStream fileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read);
				fileStream.Seek(-4L, SeekOrigin.End);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				int num = binaryReader.ReadInt32();
				if (num > 0 && (long)num < fileStream.Length)
				{
					fileStream.Seek((long)num, SeekOrigin.Begin);
					if (this.hedsSign.LoadSignature(fileStream))
					{
						int num2 = num;
						fileStream.Seek(0L, SeekOrigin.Begin);
						byte[] array = binaryReader.ReadBytes(num2);
						byte[] signature = this.hedsSign.GetSignature(iGeneration);
						if (signature != null)
						{
							if (this.hedsCrypt.VerifyData(array, signature))
							{
								return HedsFile.heds_status.HEDS_STATUS_OK;
							}
							return HedsFile.heds_status.HEDS_SIGNATURE_BROKEN;
						}
					}
				}
			}
			catch (DllBrokenException)
			{
				return HedsFile.heds_status.HEDS_SIGNATURE_BROKEN;
			}
			return HedsFile.heds_status.HEDS_GENERATION_NOT_FOUND;
		}

		// Token: 0x0400004B RID: 75
		public string strFileName;

		// Token: 0x0400004C RID: 76
		public HedsCrypt hedsCrypt;

		// Token: 0x0400004D RID: 77
		private HedsSign hedsSign;

		// Token: 0x02000015 RID: 21
		public enum heds_status
		{
			// Token: 0x0400004F RID: 79
			HEDS_STATUS_OK,
			// Token: 0x04000050 RID: 80
			HEDS_SIGNATURE_BROKEN = -1,
			// Token: 0x04000051 RID: 81
			HEDS_GENERATION_NOT_FOUND = -2,
			// Token: 0x04000052 RID: 82
			HEDS_FILE_NOT_FOUND = -3
		}
	}
}
