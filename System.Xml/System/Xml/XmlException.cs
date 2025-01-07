using System;
using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Xml
{
	[Serializable]
	public class XmlException : SystemException
	{
		protected XmlException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.res = (string)info.GetValue("res", typeof(string));
			this.args = (string[])info.GetValue("args", typeof(string[]));
			this.lineNumber = (int)info.GetValue("lineNumber", typeof(int));
			this.linePosition = (int)info.GetValue("linePosition", typeof(int));
			this.sourceUri = string.Empty;
			string text = null;
			foreach (SerializationEntry serializationEntry in info)
			{
				string name;
				if ((name = serializationEntry.Name) != null)
				{
					if (!(name == "sourceUri"))
					{
						if (name == "version")
						{
							text = (string)serializationEntry.Value;
						}
					}
					else
					{
						this.sourceUri = (string)serializationEntry.Value;
					}
				}
			}
			if (text == null)
			{
				this.message = XmlException.CreateMessage(this.res, this.args, this.lineNumber, this.linePosition);
				return;
			}
			this.message = null;
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("res", this.res);
			info.AddValue("args", this.args);
			info.AddValue("lineNumber", this.lineNumber);
			info.AddValue("linePosition", this.linePosition);
			info.AddValue("sourceUri", this.sourceUri);
			info.AddValue("version", "2.0");
		}

		public XmlException()
			: this(null)
		{
		}

		public XmlException(string message)
			: this(message, null, 0, 0)
		{
		}

		public XmlException(string message, Exception innerException)
			: this(message, innerException, 0, 0)
		{
		}

		public XmlException(string message, Exception innerException, int lineNumber, int linePosition)
			: this(message, innerException, lineNumber, linePosition, null)
		{
		}

		internal XmlException(string message, Exception innerException, int lineNumber, int linePosition, string sourceUri)
			: this((message == null) ? "Xml_DefaultException" : "Xml_UserException", new string[] { message }, innerException, lineNumber, linePosition, sourceUri)
		{
		}

		internal XmlException(string res, string[] args)
			: this(res, args, null, 0, 0, null)
		{
		}

		internal XmlException(string res, string[] args, string sourceUri)
			: this(res, args, null, 0, 0, sourceUri)
		{
		}

		internal XmlException(string res, string arg)
			: this(res, new string[] { arg }, null, 0, 0, null)
		{
		}

		internal XmlException(string res, string arg, string sourceUri)
			: this(res, new string[] { arg }, null, 0, 0, sourceUri)
		{
		}

		internal XmlException(string res, string arg, IXmlLineInfo lineInfo)
			: this(res, new string[] { arg }, lineInfo, null)
		{
		}

		internal XmlException(string res, string arg, Exception innerException, IXmlLineInfo lineInfo)
			: this(res, new string[] { arg }, innerException, (lineInfo == null) ? 0 : lineInfo.LineNumber, (lineInfo == null) ? 0 : lineInfo.LinePosition, null)
		{
		}

		internal XmlException(string res, string arg, IXmlLineInfo lineInfo, string sourceUri)
			: this(res, new string[] { arg }, lineInfo, sourceUri)
		{
		}

		internal XmlException(string res, string[] args, IXmlLineInfo lineInfo)
			: this(res, args, lineInfo, null)
		{
		}

		internal XmlException(string res, string[] args, IXmlLineInfo lineInfo, string sourceUri)
			: this(res, args, null, (lineInfo == null) ? 0 : lineInfo.LineNumber, (lineInfo == null) ? 0 : lineInfo.LinePosition, sourceUri)
		{
		}

		internal XmlException(string res, int lineNumber, int linePosition)
			: this(res, null, null, lineNumber, linePosition)
		{
		}

		internal XmlException(string res, string arg, int lineNumber, int linePosition)
			: this(res, new string[] { arg }, null, lineNumber, linePosition, null)
		{
		}

		internal XmlException(string res, string arg, int lineNumber, int linePosition, string sourceUri)
			: this(res, new string[] { arg }, null, lineNumber, linePosition, sourceUri)
		{
		}

		internal XmlException(string res, string[] args, int lineNumber, int linePosition)
			: this(res, args, null, lineNumber, linePosition, null)
		{
		}

		internal XmlException(string res, string[] args, int lineNumber, int linePosition, string sourceUri)
			: this(res, args, null, lineNumber, linePosition, sourceUri)
		{
		}

		internal XmlException(string res, string[] args, Exception innerException, int lineNumber, int linePosition)
			: this(res, args, innerException, lineNumber, linePosition, null)
		{
		}

		internal XmlException(string res, string[] args, Exception innerException, int lineNumber, int linePosition, string sourceUri)
			: base(XmlException.CreateMessage(res, args, lineNumber, linePosition), innerException)
		{
			base.HResult = -2146232000;
			this.res = res;
			this.args = args;
			this.sourceUri = sourceUri;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		private static string CreateMessage(string res, string[] args, int lineNumber, int linePosition)
		{
			string text2;
			try
			{
				string text = Res.GetString(res, args);
				if (lineNumber != 0)
				{
					text = text + " " + Res.GetString("Xml_ErrorPosition", new string[]
					{
						lineNumber.ToString(CultureInfo.InvariantCulture),
						linePosition.ToString(CultureInfo.InvariantCulture)
					});
				}
				text2 = text;
			}
			catch (MissingManifestResourceException)
			{
				text2 = "UNKNOWN(" + res + ")";
			}
			return text2;
		}

		internal static string[] BuildCharExceptionStr(char ch)
		{
			string[] array = new string[2];
			if (ch == '\0')
			{
				array[0] = ".";
			}
			else
			{
				array[0] = ch.ToString(CultureInfo.InvariantCulture);
			}
			string[] array2 = array;
			int num = 1;
			string text = "0x";
			int num2 = (int)ch;
			array2[num] = text + num2.ToString("X2", CultureInfo.InvariantCulture);
			return array;
		}

		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		public string SourceUri
		{
			get
			{
				return this.sourceUri;
			}
		}

		public override string Message
		{
			get
			{
				if (this.message != null)
				{
					return this.message;
				}
				return base.Message;
			}
		}

		internal string ResString
		{
			get
			{
				return this.res;
			}
		}

		internal static bool IsCatchableException(Exception e)
		{
			return !(e is StackOverflowException) && !(e is OutOfMemoryException) && !(e is ThreadAbortException) && !(e is ThreadInterruptedException) && !(e is NullReferenceException) && !(e is AccessViolationException);
		}

		private string res;

		private string[] args;

		private int lineNumber;

		private int linePosition;

		[OptionalField]
		private string sourceUri;

		private string message;
	}
}
