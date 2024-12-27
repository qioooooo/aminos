using System;
using System.Configuration.Internal;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006EB RID: 1771
	[Serializable]
	public class ConfigurationException : SystemException
	{
		// Token: 0x060036B2 RID: 14002 RVA: 0x000E9669 File Offset: 0x000E8669
		private void Init(string filename, int line)
		{
			base.HResult = -2146232062;
			this._filename = filename;
			this._line = line;
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x000E9684 File Offset: 0x000E8684
		protected ConfigurationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Init(info.GetString("filename"), info.GetInt32("line"));
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000E96AA File Offset: 0x000E86AA
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException()
			: this(null, null, null, 0)
		{
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x000E96B6 File Offset: 0x000E86B6
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message)
			: this(message, null, null, 0)
		{
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000E96C2 File Offset: 0x000E86C2
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner)
			: this(message, inner, null, 0)
		{
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000E96CE File Offset: 0x000E86CE
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, XmlNode node)
			: this(message, null, ConfigurationException.GetUnsafeXmlNodeFilename(node), ConfigurationException.GetXmlNodeLineNumber(node))
		{
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x000E96E4 File Offset: 0x000E86E4
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner, XmlNode node)
			: this(message, inner, ConfigurationException.GetUnsafeXmlNodeFilename(node), ConfigurationException.GetXmlNodeLineNumber(node))
		{
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x000E96FA File Offset: 0x000E86FA
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, string filename, int line)
			: this(message, null, filename, line)
		{
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000E9706 File Offset: 0x000E8706
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner, string filename, int line)
			: base(message, inner)
		{
			this.Init(filename, line);
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000E9719 File Offset: 0x000E8719
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("filename", this._filename);
			info.AddValue("line", this._line);
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x060036BC RID: 14012 RVA: 0x000E9748 File Offset: 0x000E8748
		public override string Message
		{
			get
			{
				string filename = this.Filename;
				if (!string.IsNullOrEmpty(filename))
				{
					if (this.Line != 0)
					{
						return string.Concat(new string[]
						{
							this.BareMessage,
							" (",
							filename,
							" line ",
							this.Line.ToString(CultureInfo.InvariantCulture),
							")"
						});
					}
					return this.BareMessage + " (" + filename + ")";
				}
				else
				{
					if (this.Line != 0)
					{
						return this.BareMessage + " (line " + this.Line.ToString("G", CultureInfo.InvariantCulture) + ")";
					}
					return this.BareMessage;
				}
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x060036BD RID: 14013 RVA: 0x000E9808 File Offset: 0x000E8808
		public virtual string BareMessage
		{
			get
			{
				return base.Message;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x000E9810 File Offset: 0x000E8810
		public virtual string Filename
		{
			get
			{
				return ConfigurationException.SafeFilename(this._filename);
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x060036BF RID: 14015 RVA: 0x000E981D File Offset: 0x000E881D
		public virtual int Line
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x000E9825 File Offset: 0x000E8825
		[Obsolete("This class is obsolete, use System.Configuration!System.Configuration.ConfigurationErrorsException.GetFilename instead")]
		public static string GetXmlNodeFilename(XmlNode node)
		{
			return ConfigurationException.SafeFilename(ConfigurationException.GetUnsafeXmlNodeFilename(node));
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x000E9834 File Offset: 0x000E8834
		[Obsolete("This class is obsolete, use System.Configuration!System.Configuration.ConfigurationErrorsException.GetLinenumber instead")]
		public static int GetXmlNodeLineNumber(XmlNode node)
		{
			IConfigErrorInfo configErrorInfo = node as IConfigErrorInfo;
			if (configErrorInfo != null)
			{
				return configErrorInfo.LineNumber;
			}
			return 0;
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x000E9854 File Offset: 0x000E8854
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static string FullPathWithAssert(string filename)
		{
			string text = null;
			try
			{
				text = Path.GetFullPath(filename);
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000E9880 File Offset: 0x000E8880
		internal static string SafeFilename(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return filename;
			}
			if (filename.StartsWith("http:", StringComparison.OrdinalIgnoreCase))
			{
				return filename;
			}
			try
			{
				Path.GetFullPath(filename);
			}
			catch (SecurityException)
			{
				try
				{
					string text = ConfigurationException.FullPathWithAssert(filename);
					filename = Path.GetFileName(text);
				}
				catch
				{
					filename = null;
				}
			}
			catch
			{
				filename = null;
			}
			return filename;
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000E98FC File Offset: 0x000E88FC
		private static string GetUnsafeXmlNodeFilename(XmlNode node)
		{
			IConfigErrorInfo configErrorInfo = node as IConfigErrorInfo;
			if (configErrorInfo != null)
			{
				return configErrorInfo.Filename;
			}
			return string.Empty;
		}

		// Token: 0x04003193 RID: 12691
		private const string HTTP_PREFIX = "http:";

		// Token: 0x04003194 RID: 12692
		private string _filename;

		// Token: 0x04003195 RID: 12693
		private int _line;
	}
}
