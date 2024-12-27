using System;
using System.Configuration.Internal;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000085 RID: 133
	internal class PropertySourceInfo
	{
		// Token: 0x060004F6 RID: 1270 RVA: 0x00019420 File Offset: 0x00018420
		internal PropertySourceInfo(XmlReader reader)
		{
			this._fileName = this.GetFilename(reader);
			this._lineNumber = this.GetLineNumber(reader);
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00019444 File Offset: 0x00018444
		internal string FileName
		{
			get
			{
				string text = this._fileName;
				try
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				}
				catch (SecurityException)
				{
					text = Path.GetFileName(this._fileName);
					if (text == null)
					{
						text = string.Empty;
					}
				}
				return text;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00019490 File Offset: 0x00018490
		internal int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00019498 File Offset: 0x00018498
		private string GetFilename(XmlReader reader)
		{
			IConfigErrorInfo configErrorInfo = reader as IConfigErrorInfo;
			if (configErrorInfo != null)
			{
				return configErrorInfo.Filename;
			}
			return "";
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000194BC File Offset: 0x000184BC
		private int GetLineNumber(XmlReader reader)
		{
			IConfigErrorInfo configErrorInfo = reader as IConfigErrorInfo;
			if (configErrorInfo != null)
			{
				return configErrorInfo.LineNumber;
			}
			return 0;
		}

		// Token: 0x04000363 RID: 867
		private string _fileName;

		// Token: 0x04000364 RID: 868
		private int _lineNumber;
	}
}
