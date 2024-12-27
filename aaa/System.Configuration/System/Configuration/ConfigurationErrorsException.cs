using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public class ConfigurationErrorsException : ConfigurationException
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000EF0F File Offset: 0x0000DF0F
		private void Init(string filename, int line)
		{
			base.HResult = -2146232062;
			if (line == -1)
			{
				line = 0;
			}
			this._firstFilename = filename;
			this._firstLine = line;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000EF31 File Offset: 0x0000DF31
		public ConfigurationErrorsException(string message, Exception inner, string filename, int line)
			: base(message, inner)
		{
			this.Init(filename, line);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000EF44 File Offset: 0x0000DF44
		public ConfigurationErrorsException()
			: this(null, null, null, 0)
		{
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000EF50 File Offset: 0x0000DF50
		public ConfigurationErrorsException(string message)
			: this(message, null, null, 0)
		{
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000EF5C File Offset: 0x0000DF5C
		public ConfigurationErrorsException(string message, Exception inner)
			: this(message, inner, null, 0)
		{
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000EF68 File Offset: 0x0000DF68
		public ConfigurationErrorsException(string message, string filename, int line)
			: this(message, null, filename, line)
		{
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000EF74 File Offset: 0x0000DF74
		public ConfigurationErrorsException(string message, XmlNode node)
			: this(message, null, ConfigurationErrorsException.GetUnsafeFilename(node), ConfigurationErrorsException.GetLineNumber(node))
		{
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000EF8A File Offset: 0x0000DF8A
		public ConfigurationErrorsException(string message, Exception inner, XmlNode node)
			: this(message, inner, ConfigurationErrorsException.GetUnsafeFilename(node), ConfigurationErrorsException.GetLineNumber(node))
		{
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000EFA0 File Offset: 0x0000DFA0
		public ConfigurationErrorsException(string message, XmlReader reader)
			: this(message, null, ConfigurationErrorsException.GetUnsafeFilename(reader), ConfigurationErrorsException.GetLineNumber(reader))
		{
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000EFB6 File Offset: 0x0000DFB6
		public ConfigurationErrorsException(string message, Exception inner, XmlReader reader)
			: this(message, inner, ConfigurationErrorsException.GetUnsafeFilename(reader), ConfigurationErrorsException.GetLineNumber(reader))
		{
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000EFCC File Offset: 0x0000DFCC
		internal ConfigurationErrorsException(string message, IConfigErrorInfo errorInfo)
			: this(message, null, ConfigurationErrorsException.GetUnsafeConfigErrorInfoFilename(errorInfo), ConfigurationErrorsException.GetConfigErrorInfoLineNumber(errorInfo))
		{
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000EFE2 File Offset: 0x0000DFE2
		internal ConfigurationErrorsException(string message, Exception inner, IConfigErrorInfo errorInfo)
			: this(message, inner, ConfigurationErrorsException.GetUnsafeConfigErrorInfoFilename(errorInfo), ConfigurationErrorsException.GetConfigErrorInfoLineNumber(errorInfo))
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000EFF8 File Offset: 0x0000DFF8
		internal ConfigurationErrorsException(ConfigurationException e)
			: this(ConfigurationErrorsException.GetBareMessage(e), ConfigurationErrorsException.GetInnerException(e), ConfigurationErrorsException.GetUnsafeFilename(e), ConfigurationErrorsException.GetLineNumber(e))
		{
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000F018 File Offset: 0x0000E018
		internal ConfigurationErrorsException(ICollection<ConfigurationException> coll)
			: this(ConfigurationErrorsException.GetFirstException(coll))
		{
			if (coll.Count > 1)
			{
				this._errors = new ConfigurationException[coll.Count];
				coll.CopyTo(this._errors, 0);
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000F050 File Offset: 0x0000E050
		internal ConfigurationErrorsException(ArrayList coll)
			: this((ConfigurationException)((coll.Count > 0) ? coll[0] : null))
		{
			if (coll.Count > 1)
			{
				this._errors = new ConfigurationException[coll.Count];
				coll.CopyTo(this._errors, 0);
				foreach (ConfigurationException obj in this._errors)
				{
					ConfigurationException ex = (ConfigurationException)obj;
				}
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000F0C4 File Offset: 0x0000E0C4
		private static ConfigurationException GetFirstException(ICollection<ConfigurationException> coll)
		{
			using (IEnumerator<ConfigurationException> enumerator = coll.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000F10C File Offset: 0x0000E10C
		private static string GetBareMessage(ConfigurationException e)
		{
			if (e != null)
			{
				return e.BareMessage;
			}
			return null;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000F119 File Offset: 0x0000E119
		private static Exception GetInnerException(ConfigurationException e)
		{
			if (e != null)
			{
				return e.InnerException;
			}
			return null;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000F126 File Offset: 0x0000E126
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static string GetUnsafeFilename(ConfigurationException e)
		{
			if (e != null)
			{
				return e.Filename;
			}
			return null;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000F133 File Offset: 0x0000E133
		private static int GetLineNumber(ConfigurationException e)
		{
			if (e != null)
			{
				return e.Line;
			}
			return 0;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000F140 File Offset: 0x0000E140
		protected ConfigurationErrorsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			string @string = info.GetString("firstFilename");
			int @int = info.GetInt32("firstLine");
			this.Init(@string, @int);
			int int2 = info.GetInt32("count");
			if (int2 != 0)
			{
				this._errors = new ConfigurationException[int2];
				for (int i = 0; i < int2; i++)
				{
					string text = i.ToString(CultureInfo.InvariantCulture);
					string string2 = info.GetString(text + "_errors_type");
					Type type = Type.GetType(string2, true);
					if (type != typeof(ConfigurationException) && type != typeof(ConfigurationErrorsException))
					{
						throw ExceptionUtil.UnexpectedError("ConfigurationErrorsException");
					}
					this._errors[i] = (ConfigurationException)info.GetValue(text + "_errors", type);
				}
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000F218 File Offset: 0x0000E218
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			int num = 0;
			base.GetObjectData(info, context);
			info.AddValue("firstFilename", this.Filename);
			info.AddValue("firstLine", this.Line);
			if (this._errors != null && this._errors.Length > 1)
			{
				num = this._errors.Length;
				for (int i = 0; i < this._errors.Length; i++)
				{
					string text = i.ToString(CultureInfo.InvariantCulture);
					info.AddValue(text + "_errors", this._errors[i]);
					info.AddValue(text + "_errors_type", this._errors[i].GetType());
				}
			}
			info.AddValue("count", num);
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000F2D0 File Offset: 0x0000E2D0
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
							this.Line.ToString(CultureInfo.CurrentCulture),
							")"
						});
					}
					return this.BareMessage + " (" + filename + ")";
				}
				else
				{
					if (this.Line != 0)
					{
						return this.BareMessage + " (line " + this.Line.ToString("G", CultureInfo.CurrentCulture) + ")";
					}
					return this.BareMessage;
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000F390 File Offset: 0x0000E390
		public override string BareMessage
		{
			get
			{
				return base.BareMessage;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000F398 File Offset: 0x0000E398
		public override string Filename
		{
			get
			{
				return ConfigurationErrorsException.SafeFilename(this._firstFilename);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000F3A5 File Offset: 0x0000E3A5
		public override int Line
		{
			get
			{
				return this._firstLine;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000F3B0 File Offset: 0x0000E3B0
		public ICollection Errors
		{
			get
			{
				if (this._errors != null)
				{
					return this._errors;
				}
				ConfigurationErrorsException ex = new ConfigurationErrorsException(this.BareMessage, base.InnerException, this._firstFilename, this._firstLine);
				return new ConfigurationException[] { ex };
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000F3F8 File Offset: 0x0000E3F8
		internal ICollection<ConfigurationException> ErrorsGeneric
		{
			get
			{
				return (ICollection<ConfigurationException>)this.Errors;
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000F405 File Offset: 0x0000E405
		public static int GetLineNumber(XmlNode node)
		{
			return ConfigurationErrorsException.GetConfigErrorInfoLineNumber(node as IConfigErrorInfo);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000F412 File Offset: 0x0000E412
		public static string GetFilename(XmlNode node)
		{
			return ConfigurationErrorsException.SafeFilename(ConfigurationErrorsException.GetUnsafeFilename(node));
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000F41F File Offset: 0x0000E41F
		private static string GetUnsafeFilename(XmlNode node)
		{
			return ConfigurationErrorsException.GetUnsafeConfigErrorInfoFilename(node as IConfigErrorInfo);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000F42C File Offset: 0x0000E42C
		public static int GetLineNumber(XmlReader reader)
		{
			return ConfigurationErrorsException.GetConfigErrorInfoLineNumber(reader as IConfigErrorInfo);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000F439 File Offset: 0x0000E439
		public static string GetFilename(XmlReader reader)
		{
			return ConfigurationErrorsException.SafeFilename(ConfigurationErrorsException.GetUnsafeFilename(reader));
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000F446 File Offset: 0x0000E446
		private static string GetUnsafeFilename(XmlReader reader)
		{
			return ConfigurationErrorsException.GetUnsafeConfigErrorInfoFilename(reader as IConfigErrorInfo);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000F453 File Offset: 0x0000E453
		private static int GetConfigErrorInfoLineNumber(IConfigErrorInfo errorInfo)
		{
			if (errorInfo != null)
			{
				return errorInfo.LineNumber;
			}
			return 0;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000F460 File Offset: 0x0000E460
		private static string GetUnsafeConfigErrorInfoFilename(IConfigErrorInfo errorInfo)
		{
			if (errorInfo != null)
			{
				return errorInfo.Filename;
			}
			return null;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000F470 File Offset: 0x0000E470
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

		// Token: 0x0600024D RID: 589 RVA: 0x0000F49C File Offset: 0x0000E49C
		internal static string SafeFilename(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return filename;
			}
			if (StringUtil.StartsWithIgnoreCase(filename, "http:"))
			{
				return filename;
			}
			try
			{
				if (!Path.IsPathRooted(filename))
				{
					return filename;
				}
			}
			catch
			{
				return null;
			}
			try
			{
				Path.GetFullPath(filename);
			}
			catch (SecurityException)
			{
				try
				{
					string text = ConfigurationErrorsException.FullPathWithAssert(filename);
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

		// Token: 0x0600024E RID: 590 RVA: 0x0000F538 File Offset: 0x0000E538
		internal static string AlwaysSafeFilename(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return filename;
			}
			if (StringUtil.StartsWithIgnoreCase(filename, "http:"))
			{
				return filename;
			}
			try
			{
				if (!Path.IsPathRooted(filename))
				{
					return filename;
				}
			}
			catch
			{
				return null;
			}
			try
			{
				string text = ConfigurationErrorsException.FullPathWithAssert(filename);
				filename = Path.GetFileName(text);
			}
			catch
			{
				filename = null;
			}
			return filename;
		}

		// Token: 0x04000250 RID: 592
		private const string HTTP_PREFIX = "http:";

		// Token: 0x04000251 RID: 593
		private const string SERIALIZATION_PARAM_FILENAME = "firstFilename";

		// Token: 0x04000252 RID: 594
		private const string SERIALIZATION_PARAM_LINE = "firstLine";

		// Token: 0x04000253 RID: 595
		private const string SERIALIZATION_PARAM_ERROR_COUNT = "count";

		// Token: 0x04000254 RID: 596
		private const string SERIALIZATION_PARAM_ERROR_DATA = "_errors";

		// Token: 0x04000255 RID: 597
		private const string SERIALIZATION_PARAM_ERROR_TYPE = "_errors_type";

		// Token: 0x04000256 RID: 598
		private string _firstFilename;

		// Token: 0x04000257 RID: 599
		private int _firstLine;

		// Token: 0x04000258 RID: 600
		private ConfigurationException[] _errors;
	}
}
