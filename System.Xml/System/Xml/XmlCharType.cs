using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace System.Xml
{
	internal struct XmlCharType
	{
		private static object StaticLock
		{
			get
			{
				if (XmlCharType.s_Lock == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref XmlCharType.s_Lock, obj, null);
				}
				return XmlCharType.s_Lock;
			}
		}

		private unsafe static void InitInstance()
		{
			lock (XmlCharType.StaticLock)
			{
				if (XmlCharType.s_CharProperties == null)
				{
					UnmanagedMemoryStream unmanagedMemoryStream = (UnmanagedMemoryStream)Assembly.GetExecutingAssembly().GetManifestResourceStream("XmlCharType.bin");
					byte* positionPointer = unmanagedMemoryStream.PositionPointer;
					Thread.MemoryBarrier();
					XmlCharType.s_CharProperties = positionPointer;
				}
			}
		}

		private unsafe XmlCharType(byte* charProperties)
		{
			this.charProperties = charProperties;
		}

		internal static XmlCharType Instance
		{
			get
			{
				if (XmlCharType.s_CharProperties == null)
				{
					XmlCharType.InitInstance();
				}
				return new XmlCharType(XmlCharType.s_CharProperties);
			}
		}

		public unsafe bool IsWhiteSpace(char ch)
		{
			return (this.charProperties[ch] & 1) != 0;
		}

		public unsafe bool IsLetter(char ch)
		{
			return (this.charProperties[ch] & 2) != 0;
		}

		public bool IsExtender(char ch)
		{
			return ch == '·';
		}

		public unsafe bool IsNCNameChar(char ch)
		{
			return (this.charProperties[ch] & 8) != 0;
		}

		public unsafe bool IsStartNCNameChar(char ch)
		{
			return (this.charProperties[ch] & 4) != 0;
		}

		public unsafe bool IsCharData(char ch)
		{
			return (this.charProperties[ch] & 16) != 0;
		}

		public unsafe bool IsPubidChar(char ch)
		{
			return (this.charProperties[ch] & 32) != 0;
		}

		internal unsafe bool IsTextChar(char ch)
		{
			return (this.charProperties[ch] & 64) != 0;
		}

		internal unsafe bool IsAttributeValueChar(char ch)
		{
			return (this.charProperties[ch] & 128) != 0;
		}

		public bool IsNameChar(char ch)
		{
			return this.IsNCNameChar(ch) || ch == ':';
		}

		public bool IsStartNameChar(char ch)
		{
			return this.IsStartNCNameChar(ch) || ch == ':';
		}

		public bool IsDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}

		public bool IsHexDigit(char ch)
		{
			return (ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
		}

		internal bool IsOnlyWhitespace(string str)
		{
			return this.IsOnlyWhitespaceWithPos(str) == -1;
		}

		internal unsafe int IsOnlyWhitespaceWithPos(string str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					if ((this.charProperties[str[i]] & 1) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		internal bool IsName(string str)
		{
			if (str.Length == 0 || !this.IsStartNameChar(str[0]))
			{
				return false;
			}
			for (int i = 1; i < str.Length; i++)
			{
				if (!this.IsNameChar(str[i]))
				{
					return false;
				}
			}
			return true;
		}

		internal unsafe bool IsNmToken(string str)
		{
			if (str.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < str.Length; i++)
			{
				if ((this.charProperties[str[i]] & 8) == 0 && str[i] != ':')
				{
					return false;
				}
			}
			return true;
		}

		internal unsafe int IsOnlyCharData(string str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					if ((this.charProperties[str[i]] & 16) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		internal unsafe int IsPublicId(string str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					if ((this.charProperties[str[i]] & 32) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		internal const int fWhitespace = 1;

		internal const int fLetter = 2;

		internal const int fNCStartName = 4;

		internal const int fNCName = 8;

		internal const int fCharData = 16;

		internal const int fPublicId = 32;

		internal const int fText = 64;

		internal const int fAttrValue = 128;

		private const uint CharPropertiesSize = 65536U;

		private static object s_Lock;

		private unsafe static byte* s_CharProperties;

		internal unsafe byte* charProperties;
	}
}
