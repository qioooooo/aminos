using System;
using System.Diagnostics;
using System.IO;
using Aladdin.HASP;

namespace HEDS
{
	internal class HedsFile
	{
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

		public string FindFile(string additionalPath)
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
				if (additionalPath != null)
				{
					text = this.CheckFile(additionalPath);
					if (text != null)
					{
						return text;
					}
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

		public string strFileName;

		public HedsCrypt hedsCrypt;

		private HedsSign hedsSign;

		public enum heds_status
		{
			HEDS_STATUS_OK,
			HEDS_SIGNATURE_BROKEN = -1,
			HEDS_GENERATION_NOT_FOUND = -2,
			HEDS_FILE_NOT_FOUND = -3
		}
	}
}
