using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000AE RID: 174
	internal sealed class HttpMultipartContentTemplateParser
	{
		// Token: 0x06000881 RID: 2177 RVA: 0x00026064 File Offset: 0x00025064
		private HttpMultipartContentTemplateParser(HttpRawUploadedContent data, int length, byte[] boundary, Encoding encoding)
		{
			this._data = data;
			this._length = length;
			this._boundary = boundary;
			this._encoding = encoding;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x000260BB File Offset: 0x000250BB
		private bool AtEndOfData()
		{
			return this._pos >= this._length || this._lastBoundaryFound;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x000260D4 File Offset: 0x000250D4
		private bool GetNextLine()
		{
			int i = this._pos;
			this._lineStart = -1;
			while (i < this._length)
			{
				if (this._data[i] == 10)
				{
					this._lineStart = this._pos;
					this._lineLength = i - this._pos;
					this._pos = i + 1;
					if (this._lineLength > 0 && this._data[i - 1] == 13)
					{
						this._lineLength--;
						break;
					}
					break;
				}
				else if (++i == this._length)
				{
					this._lineStart = this._pos;
					this._lineLength = i - this._pos;
					this._pos = this._length;
				}
			}
			return this._lineStart >= 0;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000261A0 File Offset: 0x000251A0
		private string ExtractValueFromContentDispositionHeader(string l, int pos, string name)
		{
			string text = name + "=\"";
			int num = CultureInfo.InvariantCulture.CompareInfo.IndexOf(l, text, pos, CompareOptions.IgnoreCase);
			if (num < 0)
			{
				return null;
			}
			num += text.Length;
			int num2 = l.IndexOf('"', num);
			if (num2 < 0)
			{
				return null;
			}
			if (num2 == num)
			{
				return string.Empty;
			}
			return l.Substring(num, num2 - num);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00026200 File Offset: 0x00025200
		private void ParsePartHeaders()
		{
			this._partName = null;
			this._partFilename = null;
			this._partContentType = null;
			while (this.GetNextLine())
			{
				if (this._lineLength == 0)
				{
					return;
				}
				byte[] array = new byte[this._lineLength];
				this._data.CopyBytes(this._lineStart, array, 0, this._lineLength);
				string @string = this._encoding.GetString(array);
				int num = @string.IndexOf(':');
				if (num >= 0)
				{
					string text = @string.Substring(0, num);
					if (StringUtil.EqualsIgnoreCase(text, "Content-Disposition"))
					{
						this._partName = this.ExtractValueFromContentDispositionHeader(@string, num + 1, "name");
						this._partFilename = this.ExtractValueFromContentDispositionHeader(@string, num + 1, "filename");
					}
					else if (StringUtil.EqualsIgnoreCase(text, "Content-Type"))
					{
						this._partContentType = @string.Substring(num + 1).Trim();
					}
				}
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x000262E0 File Offset: 0x000252E0
		private bool AtBoundaryLine()
		{
			int num = this._boundary.Length;
			if (this._lineLength != num && this._lineLength != num + 2)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (this._data[this._lineStart + i] != this._boundary[i])
				{
					return false;
				}
			}
			if (this._lineLength == num)
			{
				return true;
			}
			if (this._data[this._lineStart + num] != 45 || this._data[this._lineStart + num + 1] != 45)
			{
				return false;
			}
			this._lastBoundaryFound = true;
			return true;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0002637C File Offset: 0x0002537C
		private void ParsePartData()
		{
			this._partDataStart = this._pos;
			this._partDataLength = -1;
			while (this.GetNextLine())
			{
				if (this.AtBoundaryLine())
				{
					int num = this._lineStart - 1;
					if (this._data[num] == 10)
					{
						num--;
					}
					if (this._data[num] == 13)
					{
						num--;
					}
					this._partDataLength = num - this._partDataStart + 1;
					return;
				}
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000263F0 File Offset: 0x000253F0
		private void ParseIntoElementList()
		{
			while (this.GetNextLine() && !this.AtBoundaryLine())
			{
			}
			if (this.AtEndOfData())
			{
				return;
			}
			for (;;)
			{
				this.ParsePartHeaders();
				if (this.AtEndOfData())
				{
					break;
				}
				this.ParsePartData();
				if (this._partDataLength == -1)
				{
					return;
				}
				if (this._partName != null)
				{
					this._elements.Add(new MultipartContentElement(this._partName, this._partFilename, this._partContentType, this._data, this._partDataStart, this._partDataLength));
				}
				if (this.AtEndOfData())
				{
					return;
				}
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0002647C File Offset: 0x0002547C
		internal static MultipartContentElement[] Parse(HttpRawUploadedContent data, int length, byte[] boundary, Encoding encoding)
		{
			HttpMultipartContentTemplateParser httpMultipartContentTemplateParser = new HttpMultipartContentTemplateParser(data, length, boundary, encoding);
			httpMultipartContentTemplateParser.ParseIntoElementList();
			return (MultipartContentElement[])httpMultipartContentTemplateParser._elements.ToArray(typeof(MultipartContentElement));
		}

		// Token: 0x040011B4 RID: 4532
		private HttpRawUploadedContent _data;

		// Token: 0x040011B5 RID: 4533
		private int _length;

		// Token: 0x040011B6 RID: 4534
		private int _pos;

		// Token: 0x040011B7 RID: 4535
		private ArrayList _elements = new ArrayList();

		// Token: 0x040011B8 RID: 4536
		private int _lineStart = -1;

		// Token: 0x040011B9 RID: 4537
		private int _lineLength = -1;

		// Token: 0x040011BA RID: 4538
		private bool _lastBoundaryFound;

		// Token: 0x040011BB RID: 4539
		private byte[] _boundary;

		// Token: 0x040011BC RID: 4540
		private string _partName;

		// Token: 0x040011BD RID: 4541
		private string _partFilename;

		// Token: 0x040011BE RID: 4542
		private string _partContentType;

		// Token: 0x040011BF RID: 4543
		private int _partDataStart = -1;

		// Token: 0x040011C0 RID: 4544
		private int _partDataLength = -1;

		// Token: 0x040011C1 RID: 4545
		private Encoding _encoding;
	}
}
