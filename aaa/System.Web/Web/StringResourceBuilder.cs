using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Web
{
	// Token: 0x020000D8 RID: 216
	internal class StringResourceBuilder
	{
		// Token: 0x060009E0 RID: 2528 RVA: 0x0002B494 File Offset: 0x0002A494
		internal StringResourceBuilder()
		{
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0002B49C File Offset: 0x0002A49C
		internal void AddString(string s, out int offset, out int size, out bool fAsciiOnly)
		{
			if (this._literalStrings == null)
			{
				this._literalStrings = new ArrayList();
			}
			this._literalStrings.Add(s);
			size = Encoding.UTF8.GetByteCount(s);
			fAsciiOnly = size == s.Length;
			offset = this._offset;
			this._offset += size;
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0002B4FA File Offset: 0x0002A4FA
		internal bool HasStrings
		{
			get
			{
				return this._literalStrings != null;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002B508 File Offset: 0x0002A508
		internal void CreateResourceFile(string resFileName)
		{
			using (Stream stream = new FileStream(resFileName, FileMode.Create))
			{
				Encoding utf = Encoding.UTF8;
				BinaryWriter binaryWriter = new BinaryWriter(stream, utf);
				binaryWriter.Write(0);
				binaryWriter.Write(32);
				binaryWriter.Write(65535);
				binaryWriter.Write(65535);
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				binaryWriter.Write(this._offset);
				binaryWriter.Write(32);
				binaryWriter.Write(247201791);
				binaryWriter.Write(6684671);
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				foreach (object obj in this._literalStrings)
				{
					string text = (string)obj;
					byte[] bytes = utf.GetBytes(text);
					binaryWriter.Write(bytes);
				}
			}
		}

		// Token: 0x04001264 RID: 4708
		private ArrayList _literalStrings;

		// Token: 0x04001265 RID: 4709
		private int _offset;
	}
}
