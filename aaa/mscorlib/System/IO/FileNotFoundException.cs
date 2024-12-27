using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x020005A3 RID: 1443
	[ComVisible(true)]
	[Serializable]
	public class FileNotFoundException : IOException
	{
		// Token: 0x060035C7 RID: 13767 RVA: 0x000B41D6 File Offset: 0x000B31D6
		public FileNotFoundException()
			: base(Environment.GetResourceString("IO.FileNotFound"))
		{
			base.SetErrorCode(-2147024894);
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000B41F3 File Offset: 0x000B31F3
		public FileNotFoundException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024894);
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000B4207 File Offset: 0x000B3207
		public FileNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024894);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000B421C File Offset: 0x000B321C
		public FileNotFoundException(string message, string fileName)
			: base(message)
		{
			base.SetErrorCode(-2147024894);
			this._fileName = fileName;
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000B4237 File Offset: 0x000B3237
		public FileNotFoundException(string message, string fileName, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024894);
			this._fileName = fileName;
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x060035CC RID: 13772 RVA: 0x000B4253 File Offset: 0x000B3253
		public override string Message
		{
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000B4264 File Offset: 0x000B3264
		private void SetMessageField()
		{
			if (this._message == null)
			{
				if (this._fileName == null && base.HResult == -2146233088)
				{
					this._message = Environment.GetResourceString("IO.FileNotFound");
					return;
				}
				if (this._fileName != null)
				{
					this._message = FileLoadException.FormatFileLoadExceptionMessage(this._fileName, base.HResult);
				}
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x060035CE RID: 13774 RVA: 0x000B42BE File Offset: 0x000B32BE
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x000B42C8 File Offset: 0x000B32C8
		public override string ToString()
		{
			string text = base.GetType().FullName + ": " + this.Message;
			if (this._fileName != null && this._fileName.Length != 0)
			{
				text = text + Environment.NewLine + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileName_Name"), new object[] { this._fileName });
			}
			if (base.InnerException != null)
			{
				text = text + " ---> " + base.InnerException.ToString();
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			try
			{
				if (this.FusionLog != null)
				{
					if (text == null)
					{
						text = " ";
					}
					text += Environment.NewLine;
					text += Environment.NewLine;
					text += this.FusionLog;
				}
			}
			catch (SecurityException)
			{
			}
			return text;
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x000B43BC File Offset: 0x000B33BC
		protected FileNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._fileName = info.GetString("FileNotFound_FileName");
			try
			{
				this._fusionLog = info.GetString("FileNotFound_FusionLog");
			}
			catch
			{
				this._fusionLog = null;
			}
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x000B4410 File Offset: 0x000B3410
		private FileNotFoundException(string fileName, string fusionLog, int hResult)
			: base(null)
		{
			base.SetErrorCode(hResult);
			this._fileName = fileName;
			this._fusionLog = fusionLog;
			this.SetMessageField();
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x060035D2 RID: 13778 RVA: 0x000B4434 File Offset: 0x000B3434
		public string FusionLog
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this._fusionLog;
			}
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x000B443C File Offset: 0x000B343C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("FileNotFound_FileName", this._fileName, typeof(string));
			try
			{
				info.AddValue("FileNotFound_FusionLog", this.FusionLog, typeof(string));
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x04001BEF RID: 7151
		private string _fileName;

		// Token: 0x04001BF0 RID: 7152
		private string _fusionLog;
	}
}
