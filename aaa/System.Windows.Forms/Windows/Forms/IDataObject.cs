using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020003A2 RID: 930
	[ComVisible(true)]
	public interface IDataObject
	{
		// Token: 0x0600389E RID: 14494
		object GetData(string format, bool autoConvert);

		// Token: 0x0600389F RID: 14495
		object GetData(string format);

		// Token: 0x060038A0 RID: 14496
		object GetData(Type format);

		// Token: 0x060038A1 RID: 14497
		void SetData(string format, bool autoConvert, object data);

		// Token: 0x060038A2 RID: 14498
		void SetData(string format, object data);

		// Token: 0x060038A3 RID: 14499
		void SetData(Type format, object data);

		// Token: 0x060038A4 RID: 14500
		void SetData(object data);

		// Token: 0x060038A5 RID: 14501
		bool GetDataPresent(string format, bool autoConvert);

		// Token: 0x060038A6 RID: 14502
		bool GetDataPresent(string format);

		// Token: 0x060038A7 RID: 14503
		bool GetDataPresent(Type format);

		// Token: 0x060038A8 RID: 14504
		string[] GetFormats(bool autoConvert);

		// Token: 0x060038A9 RID: 14505
		string[] GetFormats();
	}
}
