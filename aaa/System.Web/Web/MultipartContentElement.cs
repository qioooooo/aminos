using System;
using System.Text;

namespace System.Web
{
	// Token: 0x020000AD RID: 173
	internal sealed class MultipartContentElement
	{
		// Token: 0x0600087B RID: 2171 RVA: 0x00025FB5 File Offset: 0x00024FB5
		internal MultipartContentElement(string name, string filename, string contentType, HttpRawUploadedContent data, int offset, int length)
		{
			this._name = name;
			this._filename = filename;
			this._contentType = contentType;
			this._data = data;
			this._offset = offset;
			this._length = length;
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00025FEA File Offset: 0x00024FEA
		internal bool IsFile
		{
			get
			{
				return this._filename != null;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00025FF8 File Offset: 0x00024FF8
		internal bool IsFormItem
		{
			get
			{
				return this._filename == null;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00026003 File Offset: 0x00025003
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0002600B File Offset: 0x0002500B
		internal HttpPostedFile GetAsPostedFile()
		{
			return new HttpPostedFile(this._filename, this._contentType, new HttpInputStream(this._data, this._offset, this._length));
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00026035 File Offset: 0x00025035
		internal string GetAsString(Encoding encoding)
		{
			if (this._length > 0)
			{
				return encoding.GetString(this._data.GetAsByteArray(this._offset, this._length));
			}
			return string.Empty;
		}

		// Token: 0x040011AE RID: 4526
		private string _name;

		// Token: 0x040011AF RID: 4527
		private string _filename;

		// Token: 0x040011B0 RID: 4528
		private string _contentType;

		// Token: 0x040011B1 RID: 4529
		private HttpRawUploadedContent _data;

		// Token: 0x040011B2 RID: 4530
		private int _offset;

		// Token: 0x040011B3 RID: 4531
		private int _length;
	}
}
