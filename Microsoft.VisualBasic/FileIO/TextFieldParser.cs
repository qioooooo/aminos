using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.FileIO
{
	public class TextFieldParser : IDisposable
	{
		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public TextFieldParser(string path)
		{
			this.m_CommentTokens = new string[0];
			this.m_LineNumber = 1L;
			this.m_EndOfData = false;
			this.m_ErrorLine = "";
			this.m_ErrorLineNumber = -1L;
			this.m_TextFieldType = FieldType.Delimited;
			this.m_WhitespaceCodes = new int[]
			{
				9, 11, 12, 32, 133, 160, 5760, 8192, 8193, 8194,
				8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8232,
				8233, 12288, 65279
			};
			this.m_WhiteSpaceRegEx = new Regex("\\s", RegexOptions.CultureInvariant);
			this.m_TrimWhiteSpace = true;
			this.m_Position = 0;
			this.m_PeekPosition = 0;
			this.m_CharsRead = 0;
			this.m_NeedPropertyCheck = true;
			this.m_Buffer = new char[4096];
			this.m_HasFieldsEnclosedInQuotes = true;
			this.m_MaxLineSize = 10000000;
			this.m_MaxBufferSize = 10000000;
			this.m_LeaveOpen = false;
			this.InitializeFromPath(path, Encoding.UTF8, true);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public TextFieldParser(string path, Encoding defaultEncoding)
		{
			this.m_CommentTokens = new string[0];
			this.m_LineNumber = 1L;
			this.m_EndOfData = false;
			this.m_ErrorLine = "";
			this.m_ErrorLineNumber = -1L;
			this.m_TextFieldType = FieldType.Delimited;
			this.m_WhitespaceCodes = new int[]
			{
				9, 11, 12, 32, 133, 160, 5760, 8192, 8193, 8194,
				8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8232,
				8233, 12288, 65279
			};
			this.m_WhiteSpaceRegEx = new Regex("\\s", RegexOptions.CultureInvariant);
			this.m_TrimWhiteSpace = true;
			this.m_Position = 0;
			this.m_PeekPosition = 0;
			this.m_CharsRead = 0;
			this.m_NeedPropertyCheck = true;
			this.m_Buffer = new char[4096];
			this.m_HasFieldsEnclosedInQuotes = true;
			this.m_MaxLineSize = 10000000;
			this.m_MaxBufferSize = 10000000;
			this.m_LeaveOpen = false;
			this.InitializeFromPath(path, defaultEncoding, true);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public TextFieldParser(string path, Encoding defaultEncoding, bool detectEncoding)
		{
			this.m_CommentTokens = new string[0];
			this.m_LineNumber = 1L;
			this.m_EndOfData = false;
			this.m_ErrorLine = "";
			this.m_ErrorLineNumber = -1L;
			this.m_TextFieldType = FieldType.Delimited;
			this.m_WhitespaceCodes = new int[]
			{
				9, 11, 12, 32, 133, 160, 5760, 8192, 8193, 8194,
				8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8232,
				8233, 12288, 65279
			};
			this.m_WhiteSpaceRegEx = new Regex("\\s", RegexOptions.CultureInvariant);
			this.m_TrimWhiteSpace = true;
			this.m_Position = 0;
			this.m_PeekPosition = 0;
			this.m_CharsRead = 0;
			this.m_NeedPropertyCheck = true;
			this.m_Buffer = new char[4096];
			this.m_HasFieldsEnclosedInQuotes = true;
			this.m_MaxLineSize = 10000000;
			this.m_MaxBufferSize = 10000000;
			this.m_LeaveOpen = false;
			this.InitializeFromPath(path, defaultEncoding, detectEncoding);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public TextFieldParser(Stream stream)
		{
			this.m_CommentTokens = new string[0];
			this.m_LineNumber = 1L;
			this.m_EndOfData = false;
			this.m_ErrorLine = "";
			this.m_ErrorLineNumber = -1L;
			this.m_TextFieldType = FieldType.Delimited;
			this.m_WhitespaceCodes = new int[]
			{
				9, 11, 12, 32, 133, 160, 5760, 8192, 8193, 8194,
				8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8232,
				8233, 12288, 65279
			};
			this.m_WhiteSpaceRegEx = new Regex("\\s", RegexOptions.CultureInvariant);
			this.m_TrimWhiteSpace = true;
			this.m_Position = 0;
			this.m_PeekPosition = 0;
			this.m_CharsRead = 0;
			this.m_NeedPropertyCheck = true;
			this.m_Buffer = new char[4096];
			this.m_HasFieldsEnclosedInQuotes = true;
			this.m_MaxLineSize = 10000000;
			this.m_MaxBufferSize = 10000000;
			this.m_LeaveOpen = false;
			this.InitializeFromStream(stream, Encoding.UTF8, true);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public TextFieldParser(Stream stream, Encoding defaultEncoding)
		{
			this.m_CommentTokens = new string[0];
			this.m_LineNumber = 1L;
			this.m_EndOfData = false;
			this.m_ErrorLine = "";
			this.m_ErrorLineNumber = -1L;
			this.m_TextFieldType = FieldType.Delimited;
			this.m_WhitespaceCodes = new int[]
			{
				9, 11, 12, 32, 133, 160, 5760, 8192, 8193, 8194,
				8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8232,
				8233, 12288, 65279
			};
			this.m_WhiteSpaceRegEx = new Regex("\\s", RegexOptions.CultureInvariant);
			this.m_TrimWhiteSpace = true;
			this.m_Position = 0;
			this.m_PeekPosition = 0;
			this.m_CharsRead = 0;
			this.m_NeedPropertyCheck = true;
			this.m_Buffer = new char[4096];
			this.m_HasFieldsEnclosedInQuotes = true;
			this.m_MaxLineSize = 10000000;
			this.m_MaxBufferSize = 10000000;
			this.m_LeaveOpen = false;
			this.InitializeFromStream(stream, defaultEncoding, true);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public TextFieldParser(Stream stream, Encoding defaultEncoding, bool detectEncoding)
		{
			this.m_CommentTokens = new string[0];
			this.m_LineNumber = 1L;
			this.m_EndOfData = false;
			this.m_ErrorLine = "";
			this.m_ErrorLineNumber = -1L;
			this.m_TextFieldType = FieldType.Delimited;
			this.m_WhitespaceCodes = new int[]
			{
				9, 11, 12, 32, 133, 160, 5760, 8192, 8193, 8194,
				8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8232,
				8233, 12288, 65279
			};
			this.m_WhiteSpaceRegEx = new Regex("\\s", RegexOptions.CultureInvariant);
			this.m_TrimWhiteSpace = true;
			this.m_Position = 0;
			this.m_PeekPosition = 0;
			this.m_CharsRead = 0;
			this.m_NeedPropertyCheck = true;
			this.m_Buffer = new char[4096];
			this.m_HasFieldsEnclosedInQuotes = true;
			this.m_MaxLineSize = 10000000;
			this.m_MaxBufferSize = 10000000;
			this.m_LeaveOpen = false;
			this.InitializeFromStream(stream, defaultEncoding, detectEncoding);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public TextFieldParser(Stream stream, Encoding defaultEncoding, bool detectEncoding, bool leaveOpen)
		{
			this.m_CommentTokens = new string[0];
			this.m_LineNumber = 1L;
			this.m_EndOfData = false;
			this.m_ErrorLine = "";
			this.m_ErrorLineNumber = -1L;
			this.m_TextFieldType = FieldType.Delimited;
			this.m_WhitespaceCodes = new int[]
			{
				9, 11, 12, 32, 133, 160, 5760, 8192, 8193, 8194,
				8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8232,
				8233, 12288, 65279
			};
			this.m_WhiteSpaceRegEx = new Regex("\\s", RegexOptions.CultureInvariant);
			this.m_TrimWhiteSpace = true;
			this.m_Position = 0;
			this.m_PeekPosition = 0;
			this.m_CharsRead = 0;
			this.m_NeedPropertyCheck = true;
			this.m_Buffer = new char[4096];
			this.m_HasFieldsEnclosedInQuotes = true;
			this.m_MaxLineSize = 10000000;
			this.m_MaxBufferSize = 10000000;
			this.m_LeaveOpen = false;
			this.m_LeaveOpen = leaveOpen;
			this.InitializeFromStream(stream, defaultEncoding, detectEncoding);
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public TextFieldParser(TextReader reader)
		{
			this.m_CommentTokens = new string[0];
			this.m_LineNumber = 1L;
			this.m_EndOfData = false;
			this.m_ErrorLine = "";
			this.m_ErrorLineNumber = -1L;
			this.m_TextFieldType = FieldType.Delimited;
			this.m_WhitespaceCodes = new int[]
			{
				9, 11, 12, 32, 133, 160, 5760, 8192, 8193, 8194,
				8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8232,
				8233, 12288, 65279
			};
			this.m_WhiteSpaceRegEx = new Regex("\\s", RegexOptions.CultureInvariant);
			this.m_TrimWhiteSpace = true;
			this.m_Position = 0;
			this.m_PeekPosition = 0;
			this.m_CharsRead = 0;
			this.m_NeedPropertyCheck = true;
			this.m_Buffer = new char[4096];
			this.m_HasFieldsEnclosedInQuotes = true;
			this.m_MaxLineSize = 10000000;
			this.m_MaxBufferSize = 10000000;
			this.m_LeaveOpen = false;
			if (reader == null)
			{
				throw ExceptionUtils.GetArgumentNullException("reader");
			}
			this.m_Reader = reader;
			this.ReadToBuffer();
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string[] CommentTokens
		{
			get
			{
				return this.m_CommentTokens;
			}
			set
			{
				this.CheckCommentTokensForWhitespace(value);
				this.m_CommentTokens = value;
				this.m_NeedPropertyCheck = true;
			}
		}

		public bool EndOfData
		{
			get
			{
				if (this.m_EndOfData)
				{
					return this.m_EndOfData;
				}
				if ((this.m_Reader == null) | (this.m_Buffer == null))
				{
					this.m_EndOfData = true;
					return true;
				}
				if (this.PeekNextDataLine() != null)
				{
					return false;
				}
				this.m_EndOfData = true;
				return true;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public long LineNumber
		{
			get
			{
				if (this.m_LineNumber != -1L && ((this.m_Reader.Peek() == -1) & (this.m_Position == this.m_CharsRead)))
				{
					this.CloseReader();
				}
				return this.m_LineNumber;
			}
		}

		public string ErrorLine
		{
			get
			{
				return this.m_ErrorLine;
			}
		}

		public long ErrorLineNumber
		{
			get
			{
				return this.m_ErrorLineNumber;
			}
		}

		public FieldType TextFieldType
		{
			get
			{
				return this.m_TextFieldType;
			}
			set
			{
				this.ValidateFieldTypeEnumValue(value, "value");
				this.m_TextFieldType = value;
				this.m_NeedPropertyCheck = true;
			}
		}

		public int[] FieldWidths
		{
			get
			{
				return this.m_FieldWidths;
			}
			set
			{
				if (value != null)
				{
					this.ValidateFieldWidthsOnInput(value);
					this.m_FieldWidthsCopy = (int[])value.Clone();
				}
				else
				{
					this.m_FieldWidthsCopy = null;
				}
				this.m_FieldWidths = value;
				this.m_NeedPropertyCheck = true;
			}
		}

		public string[] Delimiters
		{
			get
			{
				return this.m_Delimiters;
			}
			set
			{
				if (value != null)
				{
					this.ValidateDelimiters(value);
					this.m_DelimitersCopy = (string[])value.Clone();
				}
				else
				{
					this.m_DelimitersCopy = null;
				}
				this.m_Delimiters = value;
				this.m_NeedPropertyCheck = true;
				this.m_BeginQuotesRegex = null;
			}
		}

		public void SetDelimiters(params string[] delimiters)
		{
			this.Delimiters = delimiters;
		}

		public void SetFieldWidths(params int[] fieldWidths)
		{
			this.FieldWidths = fieldWidths;
		}

		public bool TrimWhiteSpace
		{
			get
			{
				return this.m_TrimWhiteSpace;
			}
			set
			{
				this.m_TrimWhiteSpace = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string ReadLine()
		{
			if ((this.m_Reader == null) | (this.m_Buffer == null))
			{
				return null;
			}
			TextFieldParser.ChangeBufferFunction changeBufferFunction = new TextFieldParser.ChangeBufferFunction(this.ReadToBuffer);
			string text = this.ReadNextLine(ref this.m_Position, changeBufferFunction);
			if (text == null)
			{
				this.FinishReading();
				return null;
			}
			checked
			{
				this.m_LineNumber += 1L;
				return text.TrimEnd(new char[] { '\r', '\n' });
			}
		}

		public string[] ReadFields()
		{
			if ((this.m_Reader == null) | (this.m_Buffer == null))
			{
				return null;
			}
			this.ValidateReadyToRead();
			switch (this.m_TextFieldType)
			{
			case FieldType.Delimited:
				return this.ParseDelimitedLine();
			case FieldType.FixedWidth:
				return this.ParseFixedWidthLine();
			default:
				return null;
			}
		}

		public string PeekChars(int numberOfChars)
		{
			if (numberOfChars <= 0)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("numberOfChars", "TextFieldParser_NumberOfCharsMustBePositive", new string[] { "numberOfChars" });
			}
			if ((this.m_Reader == null) | (this.m_Buffer == null))
			{
				return null;
			}
			if (this.m_EndOfData)
			{
				return null;
			}
			string text = this.PeekNextDataLine();
			if (text == null)
			{
				this.m_EndOfData = true;
				return null;
			}
			text = text.TrimEnd(new char[] { '\r', '\n' });
			if (text.Length < numberOfChars)
			{
				return text;
			}
			StringInfo stringInfo = new StringInfo(text);
			return stringInfo.SubstringByTextElements(0, numberOfChars);
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string ReadToEnd()
		{
			if ((this.m_Reader == null) | (this.m_Buffer == null))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(this.m_Buffer.Length);
			stringBuilder.Append(this.m_Buffer, this.m_Position, checked(this.m_CharsRead - this.m_Position));
			stringBuilder.Append(this.m_Reader.ReadToEnd());
			this.FinishReading();
			return stringBuilder.ToString();
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool HasFieldsEnclosedInQuotes
		{
			get
			{
				return this.m_HasFieldsEnclosedInQuotes;
			}
			set
			{
				this.m_HasFieldsEnclosedInQuotes = value;
			}
		}

		public void Close()
		{
			this.CloseReader();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!this.m_Disposed)
				{
					this.Close();
				}
				this.m_Disposed = true;
			}
		}

		private void ValidateFieldTypeEnumValue(FieldType value, string paramName)
		{
			if (value < FieldType.Delimited || value > FieldType.FixedWidth)
			{
				throw new InvalidEnumArgumentException(paramName, (int)value, typeof(FieldType));
			}
		}

		protected override void Finalize()
		{
			this.Dispose(false);
			base.Finalize();
		}

		private void CloseReader()
		{
			this.FinishReading();
			if (this.m_Reader != null)
			{
				if (!this.m_LeaveOpen)
				{
					this.m_Reader.Close();
				}
				this.m_Reader = null;
			}
		}

		private void FinishReading()
		{
			this.m_LineNumber = -1L;
			this.m_EndOfData = true;
			this.m_Buffer = null;
			this.m_DelimiterRegex = null;
			this.m_BeginQuotesRegex = null;
		}

		private void InitializeFromPath(string path, Encoding defaultEncoding, bool detectEncoding)
		{
			if (Operators.CompareString(path, "", false) == 0)
			{
				throw ExceptionUtils.GetArgumentNullException("path");
			}
			if (defaultEncoding == null)
			{
				throw ExceptionUtils.GetArgumentNullException("defaultEncoding");
			}
			string text = this.ValidatePath(path);
			FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			this.m_Reader = new StreamReader(fileStream, defaultEncoding, detectEncoding);
			this.ReadToBuffer();
		}

		private void InitializeFromStream(Stream stream, Encoding defaultEncoding, bool detectEncoding)
		{
			if (stream == null)
			{
				throw ExceptionUtils.GetArgumentNullException("stream");
			}
			if (!stream.CanRead)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("stream", "TextFieldParser_StreamNotReadable", new string[] { "stream" });
			}
			if (defaultEncoding == null)
			{
				throw ExceptionUtils.GetArgumentNullException("defaultEncoding");
			}
			this.m_Reader = new StreamReader(stream, defaultEncoding, detectEncoding);
			this.ReadToBuffer();
		}

		private string ValidatePath(string path)
		{
			string text = FileSystem.NormalizeFilePath(path, "path");
			if (!File.Exists(text))
			{
				throw new FileNotFoundException(Utils.GetResourceString("IO_FileNotFound_Path", new string[] { text }));
			}
			return text;
		}

		private bool IgnoreLine(string line)
		{
			if (line == null)
			{
				return false;
			}
			string text = line.Trim();
			if (text.Length == 0)
			{
				return true;
			}
			if (this.m_CommentTokens != null)
			{
				foreach (string text2 in this.m_CommentTokens)
				{
					if (Operators.CompareString(text2, "", false) != 0)
					{
						if (text.StartsWith(text2, StringComparison.Ordinal))
						{
							return true;
						}
						if (line.StartsWith(text2, StringComparison.Ordinal))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private int ReadToBuffer()
		{
			this.m_Position = 0;
			int num = this.m_Buffer.Length;
			if (num > 4096)
			{
				num = 4096;
				this.m_Buffer = new char[checked(num - 1 + 1)];
			}
			this.m_CharsRead = this.m_Reader.Read(this.m_Buffer, 0, num);
			return this.m_CharsRead;
		}

		private int SlideCursorToStartOfBuffer()
		{
			checked
			{
				if (this.m_Position > 0)
				{
					int num = this.m_Buffer.Length;
					int num2 = this.m_CharsRead - this.m_Position;
					char[] array = new char[num - 1 + 1];
					Array.Copy(this.m_Buffer, this.m_Position, array, 0, num2);
					int num3 = this.m_Reader.Read(array, num2, num - num2);
					this.m_CharsRead = num2 + num3;
					this.m_Position = 0;
					this.m_Buffer = array;
					return num3;
				}
				return 0;
			}
		}

		private int IncreaseBufferSize()
		{
			this.m_PeekPosition = this.m_CharsRead;
			checked
			{
				int num = this.m_Buffer.Length + 4096;
				if (num > this.m_MaxBufferSize)
				{
					throw ExceptionUtils.GetInvalidOperationException("TextFieldParser_BufferExceededMaxSize", new string[0]);
				}
				char[] array = new char[num - 1 + 1];
				Array.Copy(this.m_Buffer, array, this.m_Buffer.Length);
				int num2 = this.m_Reader.Read(array, this.m_Buffer.Length, 4096);
				this.m_Buffer = array;
				this.m_CharsRead += num2;
				return num2;
			}
		}

		private string ReadNextDataLine()
		{
			TextFieldParser.ChangeBufferFunction changeBufferFunction = new TextFieldParser.ChangeBufferFunction(this.ReadToBuffer);
			checked
			{
				string text;
				do
				{
					text = this.ReadNextLine(ref this.m_Position, changeBufferFunction);
					this.m_LineNumber += 1L;
				}
				while (this.IgnoreLine(text));
				if (text == null)
				{
					this.CloseReader();
				}
				return text;
			}
		}

		private string PeekNextDataLine()
		{
			TextFieldParser.ChangeBufferFunction changeBufferFunction = new TextFieldParser.ChangeBufferFunction(this.IncreaseBufferSize);
			this.SlideCursorToStartOfBuffer();
			this.m_PeekPosition = 0;
			string text;
			do
			{
				text = this.ReadNextLine(ref this.m_PeekPosition, changeBufferFunction);
			}
			while (this.IgnoreLine(text));
			return text;
		}

		private string ReadNextLine(ref int Cursor, TextFieldParser.ChangeBufferFunction ChangeBuffer)
		{
			if (Cursor == this.m_CharsRead && ChangeBuffer() == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int i;
			char c;
			checked
			{
				for (;;)
				{
					int num = Cursor;
					int num2 = this.m_CharsRead - 1;
					for (i = num; i <= num2; i++)
					{
						c = this.m_Buffer[i];
						if ((Operators.CompareString(Conversions.ToString(c), "\r", false) == 0) | (Operators.CompareString(Conversions.ToString(c), "\n", false) == 0))
						{
							goto Block_3;
						}
					}
					int num3 = this.m_CharsRead - Cursor;
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(num3 + 10);
					}
					stringBuilder.Append(this.m_Buffer, Cursor, num3);
					if (ChangeBuffer() <= 0)
					{
						goto Block_12;
					}
				}
			}
			Block_3:
			checked
			{
				if (stringBuilder != null)
				{
					stringBuilder.Append(this.m_Buffer, Cursor, i - Cursor + 1);
				}
				else
				{
					stringBuilder = new StringBuilder(i + 1);
					stringBuilder.Append(this.m_Buffer, Cursor, i - Cursor + 1);
				}
				Cursor = i + 1;
				if (Operators.CompareString(Conversions.ToString(c), "\r", false) == 0)
				{
					if (Cursor < this.m_CharsRead)
					{
						if (Operators.CompareString(Conversions.ToString(this.m_Buffer[Cursor]), "\n", false) == 0)
						{
							Cursor++;
							stringBuilder.Append("\n");
						}
					}
					else if (ChangeBuffer() > 0 && Operators.CompareString(Conversions.ToString(this.m_Buffer[Cursor]), "\n", false) == 0)
					{
						Cursor++;
						stringBuilder.Append("\n");
					}
				}
				return stringBuilder.ToString();
			}
			Block_12:
			return stringBuilder.ToString();
		}

		private string[] ParseDelimitedLine()
		{
			string text = this.ReadNextDataLine();
			if (text == null)
			{
				return null;
			}
			checked
			{
				long num = this.m_LineNumber - 1L;
				int i = 0;
				List<string> list = new List<string>();
				int num2 = this.GetEndOfLineIndex(text);
				while (i <= num2)
				{
					Match match = null;
					bool flag = false;
					if (this.m_HasFieldsEnclosedInQuotes)
					{
						match = this.BeginQuotesRegex.Match(text, i);
						flag = match.Success;
					}
					if (flag)
					{
						i = match.Index + match.Length;
						QuoteDelimitedFieldBuilder quoteDelimitedFieldBuilder = new QuoteDelimitedFieldBuilder(this.m_DelimiterWithEndCharsRegex, this.m_SpaceChars);
						quoteDelimitedFieldBuilder.BuildField(text, i);
						if (quoteDelimitedFieldBuilder.MalformedLine)
						{
							this.m_ErrorLine = text.TrimEnd(new char[] { '\r', '\n' });
							this.m_ErrorLineNumber = num;
							throw new MalformedLineException(Utils.GetResourceString("TextFieldParser_MalFormedDelimitedLine", new string[] { num.ToString(CultureInfo.InvariantCulture) }), num);
						}
						string text3;
						if (!quoteDelimitedFieldBuilder.FieldFinished)
						{
							do
							{
								int length = text.Length;
								string text2 = this.ReadNextDataLine();
								if (text2 == null)
								{
									goto Block_6;
								}
								if (text.Length + text2.Length > this.m_MaxLineSize)
								{
									goto Block_7;
								}
								text += text2;
								num2 = this.GetEndOfLineIndex(text);
								quoteDelimitedFieldBuilder.BuildField(text, length);
								if (quoteDelimitedFieldBuilder.MalformedLine)
								{
									goto Block_8;
								}
							}
							while (!quoteDelimitedFieldBuilder.FieldFinished);
							text3 = quoteDelimitedFieldBuilder.Field;
							i = quoteDelimitedFieldBuilder.Index + quoteDelimitedFieldBuilder.DelimiterLength;
							goto IL_0286;
							Block_6:
							this.m_ErrorLine = text.TrimEnd(new char[] { '\r', '\n' });
							this.m_ErrorLineNumber = num;
							throw new MalformedLineException(Utils.GetResourceString("TextFieldParser_MalFormedDelimitedLine", new string[] { num.ToString(CultureInfo.InvariantCulture) }), num);
							Block_7:
							this.m_ErrorLine = text.TrimEnd(new char[] { '\r', '\n' });
							this.m_ErrorLineNumber = num;
							throw new MalformedLineException(Utils.GetResourceString("TextFieldParser_MaxLineSizeExceeded", new string[] { num.ToString(CultureInfo.InvariantCulture) }), num);
							Block_8:
							this.m_ErrorLine = text.TrimEnd(new char[] { '\r', '\n' });
							this.m_ErrorLineNumber = num;
							throw new MalformedLineException(Utils.GetResourceString("TextFieldParser_MalFormedDelimitedLine", new string[] { num.ToString(CultureInfo.InvariantCulture) }), num);
						}
						text3 = quoteDelimitedFieldBuilder.Field;
						i = quoteDelimitedFieldBuilder.Index + quoteDelimitedFieldBuilder.DelimiterLength;
						IL_0286:
						if (this.m_TrimWhiteSpace)
						{
							text3 = text3.Trim();
						}
						list.Add(text3);
					}
					else
					{
						Match match2 = this.m_DelimiterRegex.Match(text, i);
						string text3;
						if (!match2.Success)
						{
							text3 = text.Substring(i).TrimEnd(new char[] { '\r', '\n' });
							if (this.m_TrimWhiteSpace)
							{
								text3 = text3.Trim();
							}
							list.Add(text3);
							break;
						}
						text3 = text.Substring(i, match2.Index - i);
						if (this.m_TrimWhiteSpace)
						{
							text3 = text3.Trim();
						}
						list.Add(text3);
						i = match2.Index + match2.Length;
					}
				}
				return list.ToArray();
			}
		}

		private string[] ParseFixedWidthLine()
		{
			string text = this.ReadNextDataLine();
			if (text == null)
			{
				return null;
			}
			text = text.TrimEnd(new char[] { '\r', '\n' });
			StringInfo stringInfo = new StringInfo(text);
			checked
			{
				this.ValidateFixedWidthLine(stringInfo, this.m_LineNumber - 1L);
				int num = 0;
				int num2 = this.m_FieldWidths.Length - 1;
				string[] array = new string[num2 + 1];
				int num3 = 0;
				int num4 = num2;
				for (int i = num3; i <= num4; i++)
				{
					array[i] = this.GetFixedWidthField(stringInfo, num, this.m_FieldWidths[i]);
					num += this.m_FieldWidths[i];
				}
				return array;
			}
		}

		private string GetFixedWidthField(StringInfo Line, int Index, int FieldLength)
		{
			string text;
			if (FieldLength > 0)
			{
				text = Line.SubstringByTextElements(Index, FieldLength);
			}
			else if (Index >= Line.LengthInTextElements)
			{
				text = string.Empty;
			}
			else
			{
				text = Line.SubstringByTextElements(Index).TrimEnd(new char[] { '\r', '\n' });
			}
			if (this.m_TrimWhiteSpace)
			{
				return text.Trim();
			}
			return text;
		}

		private int GetEndOfLineIndex(string Line)
		{
			int length = Line.Length;
			if (length == 1)
			{
				return length;
			}
			checked
			{
				if ((Operators.CompareString(Conversions.ToString(Line[length - 2]), "\r", false) == 0) | (Operators.CompareString(Conversions.ToString(Line[length - 2]), "\n", false) == 0))
				{
					return length - 2;
				}
				if ((Operators.CompareString(Conversions.ToString(Line[length - 1]), "\r", false) == 0) | (Operators.CompareString(Conversions.ToString(Line[length - 1]), "\n", false) == 0))
				{
					return length - 1;
				}
				return length;
			}
		}

		private void ValidateFixedWidthLine(StringInfo Line, long LineNumber)
		{
			if (Line.LengthInTextElements < this.m_LineLength)
			{
				this.m_ErrorLine = Line.String;
				this.m_ErrorLineNumber = checked(this.m_LineNumber - 1L);
				throw new MalformedLineException(Utils.GetResourceString("TextFieldParser_MalFormedFixedWidthLine", new string[] { LineNumber.ToString(CultureInfo.InvariantCulture) }), LineNumber);
			}
		}

		private void ValidateFieldWidths()
		{
			if (this.m_FieldWidths == null)
			{
				throw ExceptionUtils.GetInvalidOperationException("TextFieldParser_FieldWidthsNothing", new string[0]);
			}
			if (this.m_FieldWidths.Length == 0)
			{
				throw ExceptionUtils.GetInvalidOperationException("TextFieldParser_FieldWidthsNothing", new string[0]);
			}
			checked
			{
				int num = this.m_FieldWidths.Length - 1;
				this.m_LineLength = 0;
				int num2 = 0;
				int num3 = num - 1;
				for (int i = num2; i <= num3; i++)
				{
					this.m_LineLength += this.m_FieldWidths[i];
				}
				if (this.m_FieldWidths[num] > 0)
				{
					this.m_LineLength += this.m_FieldWidths[num];
				}
			}
		}

		private void ValidateFieldWidthsOnInput(int[] Widths)
		{
			checked
			{
				int num = Widths.Length - 1;
				int num2 = 0;
				int num3 = num - 1;
				for (int i = num2; i <= num3; i++)
				{
					if (Widths[i] < 1)
					{
						throw ExceptionUtils.GetArgumentExceptionWithArgName("FieldWidths", "TextFieldParser_FieldWidthsMustPositive", new string[] { "FieldWidths" });
					}
				}
			}
		}

		private void ValidateAndEscapeDelimiters()
		{
			if (this.m_Delimiters == null)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("Delimiters", "TextFieldParser_DelimitersNothing", new string[] { "Delimiters" });
			}
			if (this.m_Delimiters.Length == 0)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("Delimiters", "TextFieldParser_DelimitersNothing", new string[] { "Delimiters" });
			}
			int num = this.m_Delimiters.Length;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(this.EndQuotePattern + "(");
			int num2 = 0;
			checked
			{
				int num3 = num - 1;
				for (int i = num2; i <= num3; i++)
				{
					if (this.m_Delimiters[i] != null)
					{
						if (this.m_HasFieldsEnclosedInQuotes && this.m_Delimiters[i].IndexOf('"') > -1)
						{
							throw ExceptionUtils.GetInvalidOperationException("TextFieldParser_IllegalDelimiter", new string[0]);
						}
						string text = Regex.Escape(this.m_Delimiters[i]);
						stringBuilder.Append(text + "|");
						stringBuilder2.Append(text + "|");
					}
				}
				this.m_SpaceChars = this.WhitespaceCharacters;
				this.m_DelimiterRegex = new Regex(stringBuilder.ToString(0, stringBuilder.Length - 1), RegexOptions.CultureInvariant);
				stringBuilder.Append("\r|\n");
				this.m_DelimiterWithEndCharsRegex = new Regex(stringBuilder.ToString(), RegexOptions.CultureInvariant);
				stringBuilder2.Append("\r|\n)|\"$");
			}
		}

		private void ValidateReadyToRead()
		{
			if (this.m_NeedPropertyCheck | this.ArrayHasChanged())
			{
				switch (this.m_TextFieldType)
				{
				case FieldType.Delimited:
					this.ValidateAndEscapeDelimiters();
					break;
				case FieldType.FixedWidth:
					this.ValidateFieldWidths();
					break;
				}
				if (this.m_CommentTokens != null)
				{
					foreach (string text in this.m_CommentTokens)
					{
						if (Operators.CompareString(text, "", false) != 0 && (this.m_HasFieldsEnclosedInQuotes & (this.m_TextFieldType == FieldType.Delimited)) && string.Compare(text.Trim(), "\"", StringComparison.Ordinal) == 0)
						{
							throw ExceptionUtils.GetInvalidOperationException("TextFieldParser_InvalidComment", new string[0]);
						}
					}
				}
				this.m_NeedPropertyCheck = false;
			}
		}

		private void ValidateDelimiters(string[] delimiterArray)
		{
			if (delimiterArray == null)
			{
				return;
			}
			foreach (string text in delimiterArray)
			{
				if (Operators.CompareString(text, "", false) == 0)
				{
					throw ExceptionUtils.GetArgumentExceptionWithArgName("Delimiters", "TextFieldParser_DelimiterNothing", new string[] { "Delimiters" });
				}
				if (text.IndexOfAny(new char[] { '\r', '\n' }) > -1)
				{
					throw ExceptionUtils.GetArgumentExceptionWithArgName("Delimiters", "TextFieldParser_EndCharsInDelimiter", new string[0]);
				}
			}
		}

		private bool ArrayHasChanged()
		{
			checked
			{
				switch (this.m_TextFieldType)
				{
				case FieldType.Delimited:
				{
					if (this.m_Delimiters == null)
					{
						return false;
					}
					int num = this.m_DelimitersCopy.GetLowerBound(0);
					int num2 = this.m_DelimitersCopy.GetUpperBound(0);
					int num3 = num;
					int num4 = num2;
					for (int i = num3; i <= num4; i++)
					{
						if (Operators.CompareString(this.m_Delimiters[i], this.m_DelimitersCopy[i], false) != 0)
						{
							return true;
						}
					}
					break;
				}
				case FieldType.FixedWidth:
				{
					if (this.m_FieldWidths == null)
					{
						return false;
					}
					int num = this.m_FieldWidthsCopy.GetLowerBound(0);
					int num2 = this.m_FieldWidthsCopy.GetUpperBound(0);
					int num5 = num;
					int num6 = num2;
					for (int j = num5; j <= num6; j++)
					{
						if (this.m_FieldWidths[j] != this.m_FieldWidthsCopy[j])
						{
							return true;
						}
					}
					break;
				}
				}
				return false;
			}
		}

		private void CheckCommentTokensForWhitespace(string[] tokens)
		{
			if (tokens == null)
			{
				return;
			}
			foreach (string text in tokens)
			{
				if (this.m_WhiteSpaceRegEx.IsMatch(text))
				{
					throw ExceptionUtils.GetArgumentExceptionWithArgName("CommentTokens", "TextFieldParser_WhitespaceInToken", new string[0]);
				}
			}
		}

		private Regex BeginQuotesRegex
		{
			get
			{
				if (this.m_BeginQuotesRegex == null)
				{
					string text = string.Format(CultureInfo.InvariantCulture, "\\G[{0}]*\"", new object[] { this.WhitespacePattern });
					this.m_BeginQuotesRegex = new Regex(text, RegexOptions.CultureInvariant);
				}
				return this.m_BeginQuotesRegex;
			}
		}

		private string EndQuotePattern
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "\"[{0}]*", new object[] { this.WhitespacePattern });
			}
		}

		private string WhitespaceCharacters
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (int num in this.m_WhitespaceCodes)
				{
					char c = Strings.ChrW(num);
					if (!this.CharacterIsInDelimiter(c))
					{
						stringBuilder.Append(c);
					}
				}
				return stringBuilder.ToString();
			}
		}

		private string WhitespacePattern
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (int num in this.m_WhitespaceCodes)
				{
					char c = Strings.ChrW(num);
					if (!this.CharacterIsInDelimiter(c))
					{
						stringBuilder.Append("\\u" + num.ToString("X4", CultureInfo.InvariantCulture));
					}
				}
				return stringBuilder.ToString();
			}
		}

		private bool CharacterIsInDelimiter(char testCharacter)
		{
			foreach (string text in this.m_Delimiters)
			{
				if (text.IndexOf(testCharacter) > -1)
				{
					return true;
				}
			}
			return false;
		}

		private bool m_Disposed;

		private TextReader m_Reader;

		private string[] m_CommentTokens;

		private long m_LineNumber;

		private bool m_EndOfData;

		private string m_ErrorLine;

		private long m_ErrorLineNumber;

		private FieldType m_TextFieldType;

		private int[] m_FieldWidths;

		private string[] m_Delimiters;

		private int[] m_FieldWidthsCopy;

		private string[] m_DelimitersCopy;

		private Regex m_DelimiterRegex;

		private Regex m_DelimiterWithEndCharsRegex;

		private const RegexOptions REGEX_OPTIONS = RegexOptions.CultureInvariant;

		private int[] m_WhitespaceCodes;

		private Regex m_BeginQuotesRegex;

		private Regex m_WhiteSpaceRegEx;

		private bool m_TrimWhiteSpace;

		private int m_Position;

		private int m_PeekPosition;

		private int m_CharsRead;

		private bool m_NeedPropertyCheck;

		private const int DEFAULT_BUFFER_LENGTH = 4096;

		private const int DEFAULT_BUILDER_INCREASE = 10;

		private char[] m_Buffer;

		private int m_LineLength;

		private bool m_HasFieldsEnclosedInQuotes;

		private string m_SpaceChars;

		private int m_MaxLineSize;

		private int m_MaxBufferSize;

		private const string BEGINS_WITH_QUOTE = "\\G[{0}]*\"";

		private const string ENDING_QUOTE = "\"[{0}]*";

		private bool m_LeaveOpen;

		private delegate int ChangeBufferFunction();
	}
}
