using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000076 RID: 118
	[ComVisible(true)]
	[Serializable]
	public class BadImageFormatException : SystemException
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x00015DFD File Offset: 0x00014DFD
		public BadImageFormatException()
			: base(Environment.GetResourceString("Arg_BadImageFormatException"))
		{
			base.SetErrorCode(-2147024885);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00015E1A File Offset: 0x00014E1A
		public BadImageFormatException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024885);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00015E2E File Offset: 0x00014E2E
		public BadImageFormatException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147024885);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00015E43 File Offset: 0x00014E43
		public BadImageFormatException(string message, string fileName)
			: base(message)
		{
			base.SetErrorCode(-2147024885);
			this._fileName = fileName;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00015E5E File Offset: 0x00014E5E
		public BadImageFormatException(string message, string fileName, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147024885);
			this._fileName = fileName;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00015E7A File Offset: 0x00014E7A
		public override string Message
		{
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00015E88 File Offset: 0x00014E88
		private void SetMessageField()
		{
			if (this._message == null)
			{
				if (this._fileName == null && base.HResult == -2146233088)
				{
					this._message = Environment.GetResourceString("Arg_BadImageFormatException");
					return;
				}
				this._message = FileLoadException.FormatFileLoadExceptionMessage(this._fileName, base.HResult);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00015EDA File Offset: 0x00014EDA
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00015EE4 File Offset: 0x00014EE4
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

		// Token: 0x06000689 RID: 1673 RVA: 0x00015FD8 File Offset: 0x00014FD8
		protected BadImageFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._fileName = info.GetString("BadImageFormat_FileName");
			try
			{
				this._fusionLog = info.GetString("BadImageFormat_FusionLog");
			}
			catch
			{
				this._fusionLog = null;
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001602C File Offset: 0x0001502C
		private BadImageFormatException(string fileName, string fusionLog, int hResult)
			: base(null)
		{
			base.SetErrorCode(hResult);
			this._fileName = fileName;
			this._fusionLog = fusionLog;
			this.SetMessageField();
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00016050 File Offset: 0x00015050
		public string FusionLog
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this._fusionLog;
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00016058 File Offset: 0x00015058
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("BadImageFormat_FileName", this._fileName, typeof(string));
			try
			{
				info.AddValue("BadImageFormat_FusionLog", this.FusionLog, typeof(string));
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x04000213 RID: 531
		private string _fileName;

		// Token: 0x04000214 RID: 532
		private string _fusionLog;
	}
}
