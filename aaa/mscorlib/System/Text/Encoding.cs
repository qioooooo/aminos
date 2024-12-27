using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Text
{
	// Token: 0x020003D4 RID: 980
	[ComVisible(true)]
	[Serializable]
	public abstract class Encoding : ICloneable
	{
		// Token: 0x060028B5 RID: 10421 RVA: 0x0007ED9C File Offset: 0x0007DD9C
		protected Encoding()
			: this(0)
		{
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x0007EDA5 File Offset: 0x0007DDA5
		protected Encoding(int codePage)
		{
			if (codePage < 0)
			{
				throw new ArgumentOutOfRangeException("codePage");
			}
			this.m_codePage = codePage;
			this.SetDefaultFallbacks();
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x0007EDD0 File Offset: 0x0007DDD0
		internal virtual void SetDefaultFallbacks()
		{
			this.encoderFallback = new InternalEncoderBestFitFallback(this);
			this.decoderFallback = new InternalDecoderBestFitFallback(this);
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x0007EDEA File Offset: 0x0007DDEA
		internal void OnDeserializing()
		{
			this.encoderFallback = null;
			this.decoderFallback = null;
			this.m_isReadOnly = true;
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x0007EE01 File Offset: 0x0007DE01
		internal void OnDeserialized()
		{
			if (this.encoderFallback == null || this.decoderFallback == null)
			{
				this.m_deserializedFromEverett = true;
				this.SetDefaultFallbacks();
			}
			this.dataItem = null;
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x0007EE27 File Offset: 0x0007DE27
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.OnDeserializing();
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x0007EE2F File Offset: 0x0007DE2F
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserialized();
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x0007EE37 File Offset: 0x0007DE37
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.dataItem = null;
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x0007EE40 File Offset: 0x0007DE40
		internal void DeserializeEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_codePage = (int)info.GetValue("m_codePage", typeof(int));
			this.dataItem = null;
			try
			{
				this.m_isReadOnly = (bool)info.GetValue("m_isReadOnly", typeof(bool));
				this.encoderFallback = (EncoderFallback)info.GetValue("encoderFallback", typeof(EncoderFallback));
				this.decoderFallback = (DecoderFallback)info.GetValue("decoderFallback", typeof(DecoderFallback));
			}
			catch (SerializationException)
			{
				this.m_deserializedFromEverett = true;
				this.m_isReadOnly = true;
				this.SetDefaultFallbacks();
			}
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x0007EF0C File Offset: 0x0007DF0C
		internal void SerializeEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("m_isReadOnly", this.m_isReadOnly);
			info.AddValue("encoderFallback", this.EncoderFallback);
			info.AddValue("decoderFallback", this.DecoderFallback);
			info.AddValue("m_codePage", this.m_codePage);
			info.AddValue("dataItem", null);
			info.AddValue("Encoding+m_codePage", this.m_codePage);
			info.AddValue("Encoding+dataItem", null);
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x0007EF94 File Offset: 0x0007DF94
		public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return Encoding.Convert(srcEncoding, dstEncoding, bytes, 0, bytes.Length);
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x0007EFB0 File Offset: 0x0007DFB0
		public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes, int index, int count)
		{
			if (srcEncoding == null || dstEncoding == null)
			{
				throw new ArgumentNullException((srcEncoding == null) ? "srcEncoding" : "dstEncoding", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return dstEncoding.GetBytes(srcEncoding.GetChars(bytes, index, count));
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x0007F00C File Offset: 0x0007E00C
		private static object InternalSyncObject
		{
			get
			{
				if (Encoding.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Encoding.s_InternalSyncObject, obj, null);
				}
				return Encoding.s_InternalSyncObject;
			}
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x0007F038 File Offset: 0x0007E038
		public static Encoding GetEncoding(int codepage)
		{
			if (codepage < 0 || codepage > 65535)
			{
				throw new ArgumentOutOfRangeException("codepage", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[] { 0, 65535 }));
			}
			Encoding encoding = null;
			if (Encoding.encodings != null)
			{
				encoding = (Encoding)Encoding.encodings[codepage];
			}
			if (encoding == null)
			{
				lock (Encoding.InternalSyncObject)
				{
					if (Encoding.encodings == null)
					{
						Encoding.encodings = new Hashtable();
					}
					if ((encoding = (Encoding)Encoding.encodings[codepage]) != null)
					{
						return encoding;
					}
					if (codepage <= 1201)
					{
						switch (codepage)
						{
						case 0:
							encoding = Encoding.Default;
							goto IL_0188;
						case 1:
						case 2:
						case 3:
							break;
						default:
							if (codepage != 42)
							{
								switch (codepage)
								{
								case 1200:
									encoding = Encoding.Unicode;
									goto IL_0188;
								case 1201:
									encoding = Encoding.BigEndianUnicode;
									goto IL_0188;
								default:
									goto IL_0177;
								}
							}
							break;
						}
						throw new ArgumentException(Environment.GetResourceString("Argument_CodepageNotSupported", new object[] { codepage }), "codepage");
					}
					if (codepage <= 20127)
					{
						if (codepage == 1252)
						{
							encoding = new SBCSCodePageEncoding(codepage);
							goto IL_0188;
						}
						if (codepage == 20127)
						{
							encoding = Encoding.ASCII;
							goto IL_0188;
						}
					}
					else
					{
						if (codepage == 28591)
						{
							encoding = Encoding.Latin1;
							goto IL_0188;
						}
						if (codepage == 65001)
						{
							encoding = Encoding.UTF8;
							goto IL_0188;
						}
					}
					IL_0177:
					encoding = Encoding.GetEncodingCodePage(codepage);
					if (encoding == null)
					{
						encoding = Encoding.GetEncodingRare(codepage);
					}
					IL_0188:
					Encoding.encodings.Add(codepage, encoding);
				}
				return encoding;
			}
			return encoding;
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x0007F208 File Offset: 0x0007E208
		public static Encoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			Encoding encoding = Encoding.GetEncoding(codepage);
			Encoding encoding2 = (Encoding)encoding.Clone();
			encoding2.EncoderFallback = encoderFallback;
			encoding2.DecoderFallback = decoderFallback;
			return encoding2;
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x0007F238 File Offset: 0x0007E238
		private static Encoding GetEncodingRare(int codepage)
		{
			if (codepage <= 51932)
			{
				if (codepage <= 12001)
				{
					if (codepage == 10003)
					{
						return new DBCSCodePageEncoding(10003, 20949);
					}
					if (codepage == 10008)
					{
						return new DBCSCodePageEncoding(10008, 20936);
					}
					switch (codepage)
					{
					case 12000:
						return Encoding.UTF32;
					case 12001:
						return new UTF32Encoding(true, true);
					default:
						goto IL_01B4;
					}
				}
				else
				{
					if (codepage == 38598)
					{
						return new SBCSCodePageEncoding(codepage, 28598);
					}
					switch (codepage)
					{
					case 50220:
					case 50221:
					case 50222:
					case 50225:
						break;
					case 50223:
					case 50224:
					case 50226:
					case 50228:
						goto IL_01B4;
					case 50227:
						goto IL_0172;
					case 50229:
						throw new NotSupportedException(Environment.GetResourceString("NotSupported_CodePage50229"));
					default:
						if (codepage != 51932)
						{
							goto IL_01B4;
						}
						return new EUCJPEncoding();
					}
				}
			}
			else if (codepage <= 52936)
			{
				if (codepage == 51936)
				{
					goto IL_0172;
				}
				if (codepage == 51949)
				{
					return new DBCSCodePageEncoding(codepage, 20949);
				}
				if (codepage != 52936)
				{
					goto IL_01B4;
				}
			}
			else
			{
				if (codepage == 54936)
				{
					return new GB18030Encoding();
				}
				switch (codepage)
				{
				case 57002:
				case 57003:
				case 57004:
				case 57005:
				case 57006:
				case 57007:
				case 57008:
				case 57009:
				case 57010:
				case 57011:
					return new ISCIIEncoding(codepage);
				default:
					if (codepage == 65000)
					{
						return Encoding.UTF7;
					}
					goto IL_01B4;
				}
			}
			return new ISO2022Encoding(codepage);
			IL_0172:
			return new DBCSCodePageEncoding(codepage, 936);
			IL_01B4:
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoCodepageData", new object[] { codepage }));
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x0007F41C File Offset: 0x0007E41C
		private static Encoding GetEncodingCodePage(int CodePage)
		{
			int codePageByteSize = BaseCodePageEncoding.GetCodePageByteSize(CodePage);
			if (codePageByteSize == 1)
			{
				return new SBCSCodePageEncoding(CodePage);
			}
			if (codePageByteSize == 2)
			{
				return new DBCSCodePageEncoding(CodePage);
			}
			return null;
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x0007F447 File Offset: 0x0007E447
		public static Encoding GetEncoding(string name)
		{
			return Encoding.GetEncoding(EncodingTable.GetCodePageFromName(name));
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x0007F454 File Offset: 0x0007E454
		public static Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			return Encoding.GetEncoding(EncodingTable.GetCodePageFromName(name), encoderFallback, decoderFallback);
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x0007F463 File Offset: 0x0007E463
		public static EncodingInfo[] GetEncodings()
		{
			return EncodingTable.GetEncodings();
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x0007F46A File Offset: 0x0007E46A
		public virtual byte[] GetPreamble()
		{
			return Encoding.emptyByteArray;
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x0007F474 File Offset: 0x0007E474
		private void GetDataItem()
		{
			if (this.dataItem == null)
			{
				this.dataItem = EncodingTable.GetCodePageDataItem(this.m_codePage);
				if (this.dataItem == null)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoCodepageData", new object[] { this.m_codePage }));
				}
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x0007F4C8 File Offset: 0x0007E4C8
		public virtual string BodyName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.BodyName;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x0007F4E3 File Offset: 0x0007E4E3
		public virtual string EncodingName
		{
			get
			{
				return Environment.GetResourceString("Globalization.cp_" + this.m_codePage);
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x0007F4FF File Offset: 0x0007E4FF
		public virtual string HeaderName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.HeaderName;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060028CE RID: 10446 RVA: 0x0007F51A File Offset: 0x0007E51A
		public virtual string WebName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.WebName;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x0007F535 File Offset: 0x0007E535
		public virtual int WindowsCodePage
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.UIFamilyCodePage;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x0007F550 File Offset: 0x0007E550
		public virtual bool IsBrowserDisplay
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 2U) != 0U;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x0007F573 File Offset: 0x0007E573
		public virtual bool IsBrowserSave
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 512U) != 0U;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x0007F59A File Offset: 0x0007E59A
		public virtual bool IsMailNewsDisplay
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 1U) != 0U;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x0007F5BD File Offset: 0x0007E5BD
		public virtual bool IsMailNewsSave
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 256U) != 0U;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x0007F5E4 File Offset: 0x0007E5E4
		[ComVisible(false)]
		public virtual bool IsSingleByte
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x0007F5E7 File Offset: 0x0007E5E7
		// (set) Token: 0x060028D6 RID: 10454 RVA: 0x0007F5EF File Offset: 0x0007E5EF
		[ComVisible(false)]
		public EncoderFallback EncoderFallback
		{
			get
			{
				return this.encoderFallback;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.encoderFallback = value;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060028D7 RID: 10455 RVA: 0x0007F61E File Offset: 0x0007E61E
		// (set) Token: 0x060028D8 RID: 10456 RVA: 0x0007F626 File Offset: 0x0007E626
		[ComVisible(false)]
		public DecoderFallback DecoderFallback
		{
			get
			{
				return this.decoderFallback;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.decoderFallback = value;
			}
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x0007F658 File Offset: 0x0007E658
		[ComVisible(false)]
		public virtual object Clone()
		{
			Encoding encoding = (Encoding)base.MemberwiseClone();
			encoding.m_isReadOnly = false;
			return encoding;
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x0007F679 File Offset: 0x0007E679
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060028DB RID: 10459 RVA: 0x0007F681 File Offset: 0x0007E681
		public static Encoding ASCII
		{
			get
			{
				if (Encoding.asciiEncoding == null)
				{
					Encoding.asciiEncoding = new ASCIIEncoding();
				}
				return Encoding.asciiEncoding;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060028DC RID: 10460 RVA: 0x0007F699 File Offset: 0x0007E699
		private static Encoding Latin1
		{
			get
			{
				if (Encoding.latin1Encoding == null)
				{
					Encoding.latin1Encoding = new Latin1Encoding();
				}
				return Encoding.latin1Encoding;
			}
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x0007F6B1 File Offset: 0x0007E6B1
		public virtual int GetByteCount(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetByteCount(chars, 0, chars.Length);
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x0007F6D8 File Offset: 0x0007E6D8
		public virtual int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			char[] array = s.ToCharArray();
			return this.GetByteCount(array, 0, array.Length);
		}

		// Token: 0x060028DF RID: 10463
		public abstract int GetByteCount(char[] chars, int index, int count);

		// Token: 0x060028E0 RID: 10464 RVA: 0x0007F708 File Offset: 0x0007E708
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			char[] array = new char[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = chars[i];
			}
			return this.GetByteCount(array, 0, count);
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x0007F76E File Offset: 0x0007E76E
		internal unsafe virtual int GetByteCount(char* chars, int count, EncoderNLS encoder)
		{
			return this.GetByteCount(chars, count);
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x0007F778 File Offset: 0x0007E778
		public virtual byte[] GetBytes(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetBytes(chars, 0, chars.Length);
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x0007F7A0 File Offset: 0x0007E7A0
		public virtual byte[] GetBytes(char[] chars, int index, int count)
		{
			byte[] array = new byte[this.GetByteCount(chars, index, count)];
			this.GetBytes(chars, index, count, array, 0);
			return array;
		}

		// Token: 0x060028E4 RID: 10468
		public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);

		// Token: 0x060028E5 RID: 10469 RVA: 0x0007F7CC File Offset: 0x0007E7CC
		public virtual byte[] GetBytes(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s", Environment.GetResourceString("ArgumentNull_String"));
			}
			char[] array = s.ToCharArray();
			return this.GetBytes(array, 0, array.Length);
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x0007F803 File Offset: 0x0007E803
		public virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return this.GetBytes(s.ToCharArray(), charIndex, charCount, bytes, byteIndex);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x0007F825 File Offset: 0x0007E825
		internal unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
		{
			return this.GetBytes(chars, charCount, bytes, byteCount);
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x0007F834 File Offset: 0x0007E834
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			char[] array = new char[charCount];
			for (int i = 0; i < charCount; i++)
			{
				array[i] = chars[i];
			}
			byte[] array2 = new byte[byteCount];
			int bytes2 = this.GetBytes(array, 0, charCount, array2, 0);
			if (bytes2 < byteCount)
			{
				byteCount = bytes2;
			}
			for (int i = 0; i < byteCount; i++)
			{
				bytes[i] = array2[i];
			}
			return byteCount;
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x0007F8E4 File Offset: 0x0007E8E4
		public virtual int GetCharCount(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetCharCount(bytes, 0, bytes.Length);
		}

		// Token: 0x060028EA RID: 10474
		public abstract int GetCharCount(byte[] bytes, int index, int count);

		// Token: 0x060028EB RID: 10475 RVA: 0x0007F90C File Offset: 0x0007E90C
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe virtual int GetCharCount(byte* bytes, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bytes[i];
			}
			return this.GetCharCount(array, 0, count);
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x0007F96F File Offset: 0x0007E96F
		internal unsafe virtual int GetCharCount(byte* bytes, int count, DecoderNLS decoder)
		{
			return this.GetCharCount(bytes, count);
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x0007F979 File Offset: 0x0007E979
		public virtual char[] GetChars(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetChars(bytes, 0, bytes.Length);
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x0007F9A0 File Offset: 0x0007E9A0
		public virtual char[] GetChars(byte[] bytes, int index, int count)
		{
			char[] array = new char[this.GetCharCount(bytes, index, count)];
			this.GetChars(bytes, index, count, array, 0);
			return array;
		}

		// Token: 0x060028EF RID: 10479
		public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

		// Token: 0x060028F0 RID: 10480 RVA: 0x0007F9CC File Offset: 0x0007E9CC
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (byteCount < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteCount < 0) ? "byteCount" : "charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			byte[] array = new byte[byteCount];
			for (int i = 0; i < byteCount; i++)
			{
				array[i] = bytes[i];
			}
			char[] array2 = new char[charCount];
			int chars2 = this.GetChars(array, 0, byteCount, array2, 0);
			if (chars2 < charCount)
			{
				charCount = chars2;
			}
			for (int i = 0; i < charCount; i++)
			{
				chars[i] = array2[i];
			}
			return charCount;
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x0007FA7C File Offset: 0x0007EA7C
		internal unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS decoder)
		{
			return this.GetChars(bytes, byteCount, chars, charCount);
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060028F2 RID: 10482 RVA: 0x0007FA89 File Offset: 0x0007EA89
		public virtual int CodePage
		{
			get
			{
				return this.m_codePage;
			}
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x0007FA91 File Offset: 0x0007EA91
		[ComVisible(false)]
		public bool IsAlwaysNormalized()
		{
			return this.IsAlwaysNormalized(NormalizationForm.FormC);
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x0007FA9A File Offset: 0x0007EA9A
		[ComVisible(false)]
		public virtual bool IsAlwaysNormalized(NormalizationForm form)
		{
			return false;
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x0007FA9D File Offset: 0x0007EA9D
		public virtual Decoder GetDecoder()
		{
			return new Encoding.DefaultDecoder(this);
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x0007FAA8 File Offset: 0x0007EAA8
		private static Encoding CreateDefaultEncoding()
		{
			int acp = Win32Native.GetACP();
			Encoding encoding;
			if (acp == 1252)
			{
				encoding = new SBCSCodePageEncoding(acp);
			}
			else
			{
				encoding = Encoding.GetEncoding(acp);
			}
			return encoding;
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x0007FAD4 File Offset: 0x0007EAD4
		public static Encoding Default
		{
			get
			{
				if (Encoding.defaultEncoding == null)
				{
					Encoding.defaultEncoding = Encoding.CreateDefaultEncoding();
				}
				return Encoding.defaultEncoding;
			}
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x0007FAEC File Offset: 0x0007EAEC
		public virtual Encoder GetEncoder()
		{
			return new Encoding.DefaultEncoder(this);
		}

		// Token: 0x060028F9 RID: 10489
		public abstract int GetMaxByteCount(int charCount);

		// Token: 0x060028FA RID: 10490
		public abstract int GetMaxCharCount(int byteCount);

		// Token: 0x060028FB RID: 10491 RVA: 0x0007FAF4 File Offset: 0x0007EAF4
		public virtual string GetString(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			return this.GetString(bytes, 0, bytes.Length);
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x0007FB19 File Offset: 0x0007EB19
		public virtual string GetString(byte[] bytes, int index, int count)
		{
			return new string(this.GetChars(bytes, index, count));
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060028FD RID: 10493 RVA: 0x0007FB29 File Offset: 0x0007EB29
		public static Encoding Unicode
		{
			get
			{
				if (Encoding.unicodeEncoding == null)
				{
					Encoding.unicodeEncoding = new UnicodeEncoding(false, true);
				}
				return Encoding.unicodeEncoding;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060028FE RID: 10494 RVA: 0x0007FB43 File Offset: 0x0007EB43
		public static Encoding BigEndianUnicode
		{
			get
			{
				if (Encoding.bigEndianUnicode == null)
				{
					Encoding.bigEndianUnicode = new UnicodeEncoding(true, true);
				}
				return Encoding.bigEndianUnicode;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060028FF RID: 10495 RVA: 0x0007FB5D File Offset: 0x0007EB5D
		public static Encoding UTF7
		{
			get
			{
				if (Encoding.utf7Encoding == null)
				{
					Encoding.utf7Encoding = new UTF7Encoding();
				}
				return Encoding.utf7Encoding;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002900 RID: 10496 RVA: 0x0007FB75 File Offset: 0x0007EB75
		public static Encoding UTF8
		{
			get
			{
				if (Encoding.utf8Encoding == null)
				{
					Encoding.utf8Encoding = new UTF8Encoding(true);
				}
				return Encoding.utf8Encoding;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002901 RID: 10497 RVA: 0x0007FB8E File Offset: 0x0007EB8E
		public static Encoding UTF32
		{
			get
			{
				if (Encoding.utf32Encoding == null)
				{
					Encoding.utf32Encoding = new UTF32Encoding(false, true);
				}
				return Encoding.utf32Encoding;
			}
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x0007FBA8 File Offset: 0x0007EBA8
		public override bool Equals(object value)
		{
			Encoding encoding = value as Encoding;
			return encoding != null && (this.m_codePage == encoding.m_codePage && this.EncoderFallback.Equals(encoding.EncoderFallback)) && this.DecoderFallback.Equals(encoding.DecoderFallback);
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x0007FBF5 File Offset: 0x0007EBF5
		public override int GetHashCode()
		{
			return this.m_codePage + this.EncoderFallback.GetHashCode() + this.DecoderFallback.GetHashCode();
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x0007FC15 File Offset: 0x0007EC15
		internal virtual char[] GetBestFitUnicodeToBytesData()
		{
			return new char[0];
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x0007FC1D File Offset: 0x0007EC1D
		internal virtual char[] GetBestFitBytesToUnicodeData()
		{
			return new char[0];
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x0007FC28 File Offset: 0x0007EC28
		internal void ThrowBytesOverflow()
		{
			throw new ArgumentException(Environment.GetResourceString("Argument_EncodingConversionOverflowBytes", new object[]
			{
				this.EncodingName,
				this.EncoderFallback.GetType()
			}), "bytes");
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x0007FC68 File Offset: 0x0007EC68
		internal void ThrowBytesOverflow(EncoderNLS encoder, bool nothingEncoded)
		{
			if (encoder == null || encoder.m_throwOnOverflow || nothingEncoded)
			{
				if (encoder != null && encoder.InternalHasFallbackBuffer)
				{
					encoder.FallbackBuffer.InternalReset();
				}
				this.ThrowBytesOverflow();
			}
			encoder.ClearMustFlush();
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x0007FC9C File Offset: 0x0007EC9C
		internal void ThrowCharsOverflow()
		{
			throw new ArgumentException(Environment.GetResourceString("Argument_EncodingConversionOverflowChars", new object[]
			{
				this.EncodingName,
				this.DecoderFallback.GetType()
			}), "chars");
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x0007FCDC File Offset: 0x0007ECDC
		internal void ThrowCharsOverflow(DecoderNLS decoder, bool nothingDecoded)
		{
			if (decoder == null || decoder.m_throwOnOverflow || nothingDecoded)
			{
				if (decoder != null && decoder.InternalHasFallbackBuffer)
				{
					decoder.FallbackBuffer.InternalReset();
				}
				this.ThrowCharsOverflow();
			}
			decoder.ClearMustFlush();
		}

		// Token: 0x040013A7 RID: 5031
		private const int MIMECONTF_MAILNEWS = 1;

		// Token: 0x040013A8 RID: 5032
		private const int MIMECONTF_BROWSER = 2;

		// Token: 0x040013A9 RID: 5033
		private const int MIMECONTF_SAVABLE_MAILNEWS = 256;

		// Token: 0x040013AA RID: 5034
		private const int MIMECONTF_SAVABLE_BROWSER = 512;

		// Token: 0x040013AB RID: 5035
		private const int CodePageDefault = 0;

		// Token: 0x040013AC RID: 5036
		private const int CodePageNoOEM = 1;

		// Token: 0x040013AD RID: 5037
		private const int CodePageNoMac = 2;

		// Token: 0x040013AE RID: 5038
		private const int CodePageNoThread = 3;

		// Token: 0x040013AF RID: 5039
		private const int CodePageNoSymbol = 42;

		// Token: 0x040013B0 RID: 5040
		private const int CodePageUnicode = 1200;

		// Token: 0x040013B1 RID: 5041
		private const int CodePageBigEndian = 1201;

		// Token: 0x040013B2 RID: 5042
		private const int CodePageWindows1252 = 1252;

		// Token: 0x040013B3 RID: 5043
		private const int CodePageMacGB2312 = 10008;

		// Token: 0x040013B4 RID: 5044
		private const int CodePageGB2312 = 20936;

		// Token: 0x040013B5 RID: 5045
		private const int CodePageMacKorean = 10003;

		// Token: 0x040013B6 RID: 5046
		private const int CodePageDLLKorean = 20949;

		// Token: 0x040013B7 RID: 5047
		private const int ISO2022JP = 50220;

		// Token: 0x040013B8 RID: 5048
		private const int ISO2022JPESC = 50221;

		// Token: 0x040013B9 RID: 5049
		private const int ISO2022JPSISO = 50222;

		// Token: 0x040013BA RID: 5050
		private const int ISOKorean = 50225;

		// Token: 0x040013BB RID: 5051
		private const int ISOSimplifiedCN = 50227;

		// Token: 0x040013BC RID: 5052
		private const int EUCJP = 51932;

		// Token: 0x040013BD RID: 5053
		private const int ChineseHZ = 52936;

		// Token: 0x040013BE RID: 5054
		private const int DuplicateEUCCN = 51936;

		// Token: 0x040013BF RID: 5055
		private const int EUCCN = 936;

		// Token: 0x040013C0 RID: 5056
		private const int EUCKR = 51949;

		// Token: 0x040013C1 RID: 5057
		internal const int CodePageASCII = 20127;

		// Token: 0x040013C2 RID: 5058
		internal const int ISO_8859_1 = 28591;

		// Token: 0x040013C3 RID: 5059
		private const int ISCIIAssemese = 57006;

		// Token: 0x040013C4 RID: 5060
		private const int ISCIIBengali = 57003;

		// Token: 0x040013C5 RID: 5061
		private const int ISCIIDevanagari = 57002;

		// Token: 0x040013C6 RID: 5062
		private const int ISCIIGujarathi = 57010;

		// Token: 0x040013C7 RID: 5063
		private const int ISCIIKannada = 57008;

		// Token: 0x040013C8 RID: 5064
		private const int ISCIIMalayalam = 57009;

		// Token: 0x040013C9 RID: 5065
		private const int ISCIIOriya = 57007;

		// Token: 0x040013CA RID: 5066
		private const int ISCIIPanjabi = 57011;

		// Token: 0x040013CB RID: 5067
		private const int ISCIITamil = 57004;

		// Token: 0x040013CC RID: 5068
		private const int ISCIITelugu = 57005;

		// Token: 0x040013CD RID: 5069
		private const int GB18030 = 54936;

		// Token: 0x040013CE RID: 5070
		private const int ISO_8859_8I = 38598;

		// Token: 0x040013CF RID: 5071
		private const int ISO_8859_8_Visual = 28598;

		// Token: 0x040013D0 RID: 5072
		private const int ENC50229 = 50229;

		// Token: 0x040013D1 RID: 5073
		private const int CodePageUTF7 = 65000;

		// Token: 0x040013D2 RID: 5074
		private const int CodePageUTF8 = 65001;

		// Token: 0x040013D3 RID: 5075
		private const int CodePageUTF32 = 12000;

		// Token: 0x040013D4 RID: 5076
		private const int CodePageUTF32BE = 12001;

		// Token: 0x040013D5 RID: 5077
		private static Encoding defaultEncoding;

		// Token: 0x040013D6 RID: 5078
		private static Encoding unicodeEncoding;

		// Token: 0x040013D7 RID: 5079
		private static Encoding bigEndianUnicode;

		// Token: 0x040013D8 RID: 5080
		private static Encoding utf7Encoding;

		// Token: 0x040013D9 RID: 5081
		private static Encoding utf8Encoding;

		// Token: 0x040013DA RID: 5082
		private static Encoding utf32Encoding;

		// Token: 0x040013DB RID: 5083
		private static Encoding asciiEncoding;

		// Token: 0x040013DC RID: 5084
		private static Encoding latin1Encoding;

		// Token: 0x040013DD RID: 5085
		private static Hashtable encodings;

		// Token: 0x040013DE RID: 5086
		internal int m_codePage;

		// Token: 0x040013DF RID: 5087
		internal CodePageDataItem dataItem;

		// Token: 0x040013E0 RID: 5088
		[NonSerialized]
		internal bool m_deserializedFromEverett;

		// Token: 0x040013E1 RID: 5089
		[OptionalField(VersionAdded = 2)]
		private bool m_isReadOnly = true;

		// Token: 0x040013E2 RID: 5090
		[OptionalField(VersionAdded = 2)]
		internal EncoderFallback encoderFallback;

		// Token: 0x040013E3 RID: 5091
		[OptionalField(VersionAdded = 2)]
		internal DecoderFallback decoderFallback;

		// Token: 0x040013E4 RID: 5092
		internal static readonly byte[] emptyByteArray = new byte[0];

		// Token: 0x040013E5 RID: 5093
		private static object s_InternalSyncObject;

		// Token: 0x020003D6 RID: 982
		[Serializable]
		internal class DefaultEncoder : Encoder, ISerializable, IObjectReference
		{
			// Token: 0x06002918 RID: 10520 RVA: 0x00080142 File Offset: 0x0007F142
			public DefaultEncoder(Encoding encoding)
			{
				this.m_encoding = encoding;
			}

			// Token: 0x06002919 RID: 10521 RVA: 0x00080154 File Offset: 0x0007F154
			internal DefaultEncoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
				try
				{
					this.m_fallback = (EncoderFallback)info.GetValue("m_fallback", typeof(EncoderFallback));
					this.charLeftOver = (char)info.GetValue("charLeftOver", typeof(char));
				}
				catch (SerializationException)
				{
				}
			}

			// Token: 0x0600291A RID: 10522 RVA: 0x000801EC File Offset: 0x0007F1EC
			public object GetRealObject(StreamingContext context)
			{
				Encoder encoder = this.m_encoding.GetEncoder();
				if (this.m_fallback != null)
				{
					encoder.m_fallback = this.m_fallback;
				}
				if (this.charLeftOver != '\0')
				{
					EncoderNLS encoderNLS = encoder as EncoderNLS;
					if (encoderNLS != null)
					{
						encoderNLS.charLeftOver = this.charLeftOver;
					}
				}
				return encoder;
			}

			// Token: 0x0600291B RID: 10523 RVA: 0x00080238 File Offset: 0x0007F238
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
			}

			// Token: 0x0600291C RID: 10524 RVA: 0x00080259 File Offset: 0x0007F259
			public override int GetByteCount(char[] chars, int index, int count, bool flush)
			{
				return this.m_encoding.GetByteCount(chars, index, count);
			}

			// Token: 0x0600291D RID: 10525 RVA: 0x00080269 File Offset: 0x0007F269
			public unsafe override int GetByteCount(char* chars, int count, bool flush)
			{
				return this.m_encoding.GetByteCount(chars, count);
			}

			// Token: 0x0600291E RID: 10526 RVA: 0x00080278 File Offset: 0x0007F278
			public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
			{
				return this.m_encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
			}

			// Token: 0x0600291F RID: 10527 RVA: 0x0008028C File Offset: 0x0007F28C
			public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, bool flush)
			{
				return this.m_encoding.GetBytes(chars, charCount, bytes, byteCount);
			}

			// Token: 0x040013E8 RID: 5096
			private Encoding m_encoding;

			// Token: 0x040013E9 RID: 5097
			[NonSerialized]
			internal char charLeftOver;
		}

		// Token: 0x020003D8 RID: 984
		[Serializable]
		internal class DefaultDecoder : Decoder, ISerializable, IObjectReference
		{
			// Token: 0x0600292F RID: 10543 RVA: 0x000806DE File Offset: 0x0007F6DE
			public DefaultDecoder(Encoding encoding)
			{
				this.m_encoding = encoding;
			}

			// Token: 0x06002930 RID: 10544 RVA: 0x000806F0 File Offset: 0x0007F6F0
			internal DefaultDecoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
				try
				{
					this.m_fallback = (DecoderFallback)info.GetValue("m_fallback", typeof(DecoderFallback));
				}
				catch (SerializationException)
				{
					this.m_fallback = null;
				}
			}

			// Token: 0x06002931 RID: 10545 RVA: 0x00080770 File Offset: 0x0007F770
			public object GetRealObject(StreamingContext context)
			{
				Decoder decoder = this.m_encoding.GetDecoder();
				if (this.m_fallback != null)
				{
					decoder.m_fallback = this.m_fallback;
				}
				return decoder;
			}

			// Token: 0x06002932 RID: 10546 RVA: 0x0008079E File Offset: 0x0007F79E
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
			}

			// Token: 0x06002933 RID: 10547 RVA: 0x000807BF File Offset: 0x0007F7BF
			public override int GetCharCount(byte[] bytes, int index, int count)
			{
				return this.GetCharCount(bytes, index, count, false);
			}

			// Token: 0x06002934 RID: 10548 RVA: 0x000807CB File Offset: 0x0007F7CB
			public override int GetCharCount(byte[] bytes, int index, int count, bool flush)
			{
				return this.m_encoding.GetCharCount(bytes, index, count);
			}

			// Token: 0x06002935 RID: 10549 RVA: 0x000807DB File Offset: 0x0007F7DB
			public unsafe override int GetCharCount(byte* bytes, int count, bool flush)
			{
				return this.m_encoding.GetCharCount(bytes, count);
			}

			// Token: 0x06002936 RID: 10550 RVA: 0x000807EA File Offset: 0x0007F7EA
			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
			{
				return this.GetChars(bytes, byteIndex, byteCount, chars, charIndex, false);
			}

			// Token: 0x06002937 RID: 10551 RVA: 0x000807FA File Offset: 0x0007F7FA
			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush)
			{
				return this.m_encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
			}

			// Token: 0x06002938 RID: 10552 RVA: 0x0008080E File Offset: 0x0007F80E
			public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, bool flush)
			{
				return this.m_encoding.GetChars(bytes, byteCount, chars, charCount);
			}

			// Token: 0x040013EC RID: 5100
			private Encoding m_encoding;
		}

		// Token: 0x020003D9 RID: 985
		internal class EncodingCharBuffer
		{
			// Token: 0x06002939 RID: 10553 RVA: 0x00080820 File Offset: 0x0007F820
			internal unsafe EncodingCharBuffer(Encoding enc, DecoderNLS decoder, char* charStart, int charCount, byte* byteStart, int byteCount)
			{
				this.enc = enc;
				this.decoder = decoder;
				this.chars = charStart;
				this.charStart = charStart;
				this.charEnd = charStart + charCount;
				this.byteStart = byteStart;
				this.bytes = byteStart;
				this.byteEnd = byteStart + byteCount;
				if (this.decoder == null)
				{
					this.fallbackBuffer = enc.DecoderFallback.CreateFallbackBuffer();
				}
				else
				{
					this.fallbackBuffer = this.decoder.FallbackBuffer;
				}
				this.fallbackBuffer.InternalInitialize(this.bytes, this.charEnd);
			}

			// Token: 0x0600293A RID: 10554 RVA: 0x000808BC File Offset: 0x0007F8BC
			internal unsafe bool AddChar(char ch, int numBytes)
			{
				if (this.chars != null)
				{
					if (this.chars >= this.charEnd)
					{
						this.bytes -= numBytes;
						this.enc.ThrowCharsOverflow(this.decoder, this.bytes == this.byteStart);
						return false;
					}
					*(this.chars++) = ch;
				}
				this.charCountResult++;
				return true;
			}

			// Token: 0x0600293B RID: 10555 RVA: 0x00080936 File Offset: 0x0007F936
			internal bool AddChar(char ch)
			{
				return this.AddChar(ch, 1);
			}

			// Token: 0x0600293C RID: 10556 RVA: 0x00080940 File Offset: 0x0007F940
			internal bool AddChar(char ch1, char ch2, int numBytes)
			{
				if (this.chars >= this.charEnd - 1)
				{
					this.bytes -= numBytes;
					this.enc.ThrowCharsOverflow(this.decoder, this.bytes == this.byteStart);
					return false;
				}
				return this.AddChar(ch1, numBytes) && this.AddChar(ch2, numBytes);
			}

			// Token: 0x0600293D RID: 10557 RVA: 0x000809A4 File Offset: 0x0007F9A4
			internal void AdjustBytes(int count)
			{
				this.bytes += count;
			}

			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x0600293E RID: 10558 RVA: 0x000809B4 File Offset: 0x0007F9B4
			internal bool MoreData
			{
				get
				{
					return this.bytes < this.byteEnd;
				}
			}

			// Token: 0x0600293F RID: 10559 RVA: 0x000809C4 File Offset: 0x0007F9C4
			internal bool EvenMoreData(int count)
			{
				return this.bytes == this.byteEnd - count;
			}

			// Token: 0x06002940 RID: 10560 RVA: 0x000809DC File Offset: 0x0007F9DC
			internal unsafe byte GetNextByte()
			{
				if (this.bytes >= this.byteEnd)
				{
					return 0;
				}
				return *(this.bytes++);
			}

			// Token: 0x170007D9 RID: 2009
			// (get) Token: 0x06002941 RID: 10561 RVA: 0x00080A0C File Offset: 0x0007FA0C
			internal int BytesUsed
			{
				get
				{
					return (int)((long)(this.bytes - this.byteStart));
				}
			}

			// Token: 0x06002942 RID: 10562 RVA: 0x00080A20 File Offset: 0x0007FA20
			internal bool Fallback(byte fallbackByte)
			{
				byte[] array = new byte[] { fallbackByte };
				return this.Fallback(array);
			}

			// Token: 0x06002943 RID: 10563 RVA: 0x00080A44 File Offset: 0x0007FA44
			internal bool Fallback(byte byte1, byte byte2)
			{
				byte[] array = new byte[] { byte1, byte2 };
				return this.Fallback(array);
			}

			// Token: 0x06002944 RID: 10564 RVA: 0x00080A6C File Offset: 0x0007FA6C
			internal bool Fallback(byte byte1, byte byte2, byte byte3, byte byte4)
			{
				byte[] array = new byte[] { byte1, byte2, byte3, byte4 };
				return this.Fallback(array);
			}

			// Token: 0x06002945 RID: 10565 RVA: 0x00080A9C File Offset: 0x0007FA9C
			internal unsafe bool Fallback(byte[] byteBuffer)
			{
				if (this.chars != null)
				{
					char* ptr = this.chars;
					if (!this.fallbackBuffer.InternalFallback(byteBuffer, this.bytes, ref this.chars))
					{
						this.bytes -= byteBuffer.Length;
						this.fallbackBuffer.InternalReset();
						this.enc.ThrowCharsOverflow(this.decoder, this.chars == this.charStart);
						return false;
					}
					this.charCountResult += (int)((long)(this.chars - ptr));
				}
				else
				{
					this.charCountResult += this.fallbackBuffer.InternalFallback(byteBuffer, this.bytes);
				}
				return true;
			}

			// Token: 0x170007DA RID: 2010
			// (get) Token: 0x06002946 RID: 10566 RVA: 0x00080B4B File Offset: 0x0007FB4B
			internal int Count
			{
				get
				{
					return this.charCountResult;
				}
			}

			// Token: 0x040013ED RID: 5101
			private unsafe char* chars;

			// Token: 0x040013EE RID: 5102
			private unsafe char* charStart;

			// Token: 0x040013EF RID: 5103
			private unsafe char* charEnd;

			// Token: 0x040013F0 RID: 5104
			private int charCountResult;

			// Token: 0x040013F1 RID: 5105
			private Encoding enc;

			// Token: 0x040013F2 RID: 5106
			private DecoderNLS decoder;

			// Token: 0x040013F3 RID: 5107
			private unsafe byte* byteStart;

			// Token: 0x040013F4 RID: 5108
			private unsafe byte* byteEnd;

			// Token: 0x040013F5 RID: 5109
			private unsafe byte* bytes;

			// Token: 0x040013F6 RID: 5110
			private DecoderFallbackBuffer fallbackBuffer;
		}

		// Token: 0x020003DA RID: 986
		internal class EncodingByteBuffer
		{
			// Token: 0x06002947 RID: 10567 RVA: 0x00080B54 File Offset: 0x0007FB54
			internal unsafe EncodingByteBuffer(Encoding inEncoding, EncoderNLS inEncoder, byte* inByteStart, int inByteCount, char* inCharStart, int inCharCount)
			{
				this.enc = inEncoding;
				this.encoder = inEncoder;
				this.charStart = inCharStart;
				this.chars = inCharStart;
				this.charEnd = inCharStart + inCharCount;
				this.bytes = inByteStart;
				this.byteStart = inByteStart;
				this.byteEnd = inByteStart + inByteCount;
				if (this.encoder == null)
				{
					this.fallbackBuffer = this.enc.EncoderFallback.CreateFallbackBuffer();
				}
				else
				{
					this.fallbackBuffer = this.encoder.FallbackBuffer;
					if (this.encoder.m_throwOnOverflow && this.encoder.InternalHasFallbackBuffer && this.fallbackBuffer.Remaining > 0)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_EncoderFallbackNotEmpty", new object[]
						{
							this.encoder.Encoding.EncodingName,
							this.encoder.Fallback.GetType()
						}));
					}
				}
				this.fallbackBuffer.InternalInitialize(this.chars, this.charEnd, this.encoder, this.bytes != null);
			}

			// Token: 0x06002948 RID: 10568 RVA: 0x00080C6C File Offset: 0x0007FC6C
			internal unsafe bool AddByte(byte b, int moreBytesExpected)
			{
				if (this.bytes != null)
				{
					if (this.bytes >= this.byteEnd - moreBytesExpected)
					{
						this.MovePrevious(true);
						return false;
					}
					*(this.bytes++) = b;
				}
				this.byteCountResult++;
				return true;
			}

			// Token: 0x06002949 RID: 10569 RVA: 0x00080CBF File Offset: 0x0007FCBF
			internal bool AddByte(byte b1)
			{
				return this.AddByte(b1, 0);
			}

			// Token: 0x0600294A RID: 10570 RVA: 0x00080CC9 File Offset: 0x0007FCC9
			internal bool AddByte(byte b1, byte b2)
			{
				return this.AddByte(b1, b2, 0);
			}

			// Token: 0x0600294B RID: 10571 RVA: 0x00080CD4 File Offset: 0x0007FCD4
			internal bool AddByte(byte b1, byte b2, int moreBytesExpected)
			{
				return this.AddByte(b1, 1 + moreBytesExpected) && this.AddByte(b2, moreBytesExpected);
			}

			// Token: 0x0600294C RID: 10572 RVA: 0x00080CEC File Offset: 0x0007FCEC
			internal bool AddByte(byte b1, byte b2, byte b3)
			{
				return this.AddByte(b1, b2, b3, 0);
			}

			// Token: 0x0600294D RID: 10573 RVA: 0x00080CF8 File Offset: 0x0007FCF8
			internal bool AddByte(byte b1, byte b2, byte b3, int moreBytesExpected)
			{
				return this.AddByte(b1, 2 + moreBytesExpected) && this.AddByte(b2, 1 + moreBytesExpected) && this.AddByte(b3, moreBytesExpected);
			}

			// Token: 0x0600294E RID: 10574 RVA: 0x00080D1F File Offset: 0x0007FD1F
			internal bool AddByte(byte b1, byte b2, byte b3, byte b4)
			{
				return this.AddByte(b1, 3) && this.AddByte(b2, 2) && this.AddByte(b3, 1) && this.AddByte(b4, 0);
			}

			// Token: 0x0600294F RID: 10575 RVA: 0x00080D4C File Offset: 0x0007FD4C
			internal void MovePrevious(bool bThrow)
			{
				if (this.fallbackBuffer.bFallingBack)
				{
					this.fallbackBuffer.MovePrevious();
				}
				else if (this.chars != this.charStart)
				{
					this.chars--;
				}
				if (bThrow)
				{
					this.enc.ThrowBytesOverflow(this.encoder, this.bytes == this.byteStart);
				}
			}

			// Token: 0x06002950 RID: 10576 RVA: 0x00080DB3 File Offset: 0x0007FDB3
			internal bool Fallback(char charFallback)
			{
				return this.fallbackBuffer.InternalFallback(charFallback, ref this.chars);
			}

			// Token: 0x170007DB RID: 2011
			// (get) Token: 0x06002951 RID: 10577 RVA: 0x00080DC7 File Offset: 0x0007FDC7
			internal bool MoreData
			{
				get
				{
					return this.fallbackBuffer.Remaining > 0 || this.chars < this.charEnd;
				}
			}

			// Token: 0x06002952 RID: 10578 RVA: 0x00080DE8 File Offset: 0x0007FDE8
			internal unsafe char GetNextChar()
			{
				char c = this.fallbackBuffer.InternalGetNextChar();
				if (c == '\0' && this.chars < this.charEnd)
				{
					c = *(this.chars++);
				}
				return c;
			}

			// Token: 0x170007DC RID: 2012
			// (get) Token: 0x06002953 RID: 10579 RVA: 0x00080E27 File Offset: 0x0007FE27
			internal int CharsUsed
			{
				get
				{
					return (int)((long)(this.chars - this.charStart));
				}
			}

			// Token: 0x170007DD RID: 2013
			// (get) Token: 0x06002954 RID: 10580 RVA: 0x00080E3A File Offset: 0x0007FE3A
			internal int Count
			{
				get
				{
					return this.byteCountResult;
				}
			}

			// Token: 0x040013F7 RID: 5111
			private unsafe byte* bytes;

			// Token: 0x040013F8 RID: 5112
			private unsafe byte* byteStart;

			// Token: 0x040013F9 RID: 5113
			private unsafe byte* byteEnd;

			// Token: 0x040013FA RID: 5114
			private unsafe char* chars;

			// Token: 0x040013FB RID: 5115
			private unsafe char* charStart;

			// Token: 0x040013FC RID: 5116
			private unsafe char* charEnd;

			// Token: 0x040013FD RID: 5117
			private int byteCountResult;

			// Token: 0x040013FE RID: 5118
			private Encoding enc;

			// Token: 0x040013FF RID: 5119
			private EncoderNLS encoder;

			// Token: 0x04001400 RID: 5120
			internal EncoderFallbackBuffer fallbackBuffer;
		}
	}
}
