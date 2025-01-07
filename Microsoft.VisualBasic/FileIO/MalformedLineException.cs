using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.FileIO
{
	[Serializable]
	public class MalformedLineException : Exception
	{
		public MalformedLineException()
		{
		}

		public MalformedLineException(string message, long lineNumber)
			: base(message)
		{
			this.m_LineNumber = lineNumber;
		}

		public MalformedLineException(string message)
			: base(message)
		{
		}

		public MalformedLineException(string message, long lineNumber, Exception innerException)
			: base(message, innerException)
		{
			this.m_LineNumber = lineNumber;
		}

		public MalformedLineException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected MalformedLineException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info != null)
			{
				this.m_LineNumber = (long)info.GetInt32("LineNumber");
			}
			else
			{
				this.m_LineNumber = -1L;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		public long LineNumber
		{
			get
			{
				return this.m_LineNumber;
			}
			set
			{
				this.m_LineNumber = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info != null)
			{
				info.AddValue("LineNumber", this.m_LineNumber, typeof(long));
			}
			base.GetObjectData(info, context);
		}

		public override string ToString()
		{
			return base.ToString() + " " + Utils.GetResourceString("TextFieldParser_MalformedExtraData", new string[] { this.LineNumber.ToString(CultureInfo.InvariantCulture) });
		}

		private long m_LineNumber;

		private const string LINE_NUMBER_PROPERTY = "LineNumber";
	}
}
