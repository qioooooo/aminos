using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x020005A1 RID: 1441
	[ComVisible(true)]
	[Serializable]
	public class FileLoadException : IOException
	{
		// Token: 0x060035B7 RID: 13751 RVA: 0x000B3F1B File Offset: 0x000B2F1B
		public FileLoadException()
			: base(Environment.GetResourceString("IO.FileLoad"))
		{
			base.SetErrorCode(-2146232799);
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000B3F38 File Offset: 0x000B2F38
		public FileLoadException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232799);
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000B3F4C File Offset: 0x000B2F4C
		public FileLoadException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146232799);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000B3F61 File Offset: 0x000B2F61
		public FileLoadException(string message, string fileName)
			: base(message)
		{
			base.SetErrorCode(-2146232799);
			this._fileName = fileName;
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000B3F7C File Offset: 0x000B2F7C
		public FileLoadException(string message, string fileName, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146232799);
			this._fileName = fileName;
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060035BC RID: 13756 RVA: 0x000B3F98 File Offset: 0x000B2F98
		public override string Message
		{
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000B3FA6 File Offset: 0x000B2FA6
		private void SetMessageField()
		{
			if (this._message == null)
			{
				this._message = FileLoadException.FormatFileLoadExceptionMessage(this._fileName, base.HResult);
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060035BE RID: 13758 RVA: 0x000B3FC7 File Offset: 0x000B2FC7
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000B3FD0 File Offset: 0x000B2FD0
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

		// Token: 0x060035C0 RID: 13760 RVA: 0x000B40C4 File Offset: 0x000B30C4
		protected FileLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._fileName = info.GetString("FileLoad_FileName");
			try
			{
				this._fusionLog = info.GetString("FileLoad_FusionLog");
			}
			catch
			{
				this._fusionLog = null;
			}
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000B4118 File Offset: 0x000B3118
		private FileLoadException(string fileName, string fusionLog, int hResult)
			: base(null)
		{
			base.SetErrorCode(hResult);
			this._fileName = fileName;
			this._fusionLog = fusionLog;
			this.SetMessageField();
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x060035C2 RID: 13762 RVA: 0x000B413C File Offset: 0x000B313C
		public string FusionLog
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return this._fusionLog;
			}
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x000B4144 File Offset: 0x000B3144
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("FileLoad_FileName", this._fileName, typeof(string));
			try
			{
				info.AddValue("FileLoad_FusionLog", this.FusionLog, typeof(string));
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000B41A4 File Offset: 0x000B31A4
		internal static string FormatFileLoadExceptionMessage(string fileName, int hResult)
		{
			return string.Format(CultureInfo.CurrentCulture, FileLoadException.GetFileLoadExceptionMessage(hResult), new object[]
			{
				fileName,
				FileLoadException.GetMessageForHR(hResult)
			});
		}

		// Token: 0x060035C5 RID: 13765
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetFileLoadExceptionMessage(int hResult);

		// Token: 0x060035C6 RID: 13766
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetMessageForHR(int hresult);

		// Token: 0x04001BE6 RID: 7142
		private string _fileName;

		// Token: 0x04001BE7 RID: 7143
		private string _fusionLog;
	}
}
