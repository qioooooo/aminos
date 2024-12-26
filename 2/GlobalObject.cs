using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.JScript
{
	// Token: 0x02000088 RID: 136
	public class GlobalObject
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x0002CD04 File Offset: 0x0002BD04
		internal GlobalObject()
		{
			this.originalActiveXObjectField = null;
			this.originalArrayField = null;
			this.originalBooleanField = null;
			this.originalDateField = null;
			this.originalEnumeratorField = null;
			this.originalEvalErrorField = null;
			this.originalErrorField = null;
			this.originalFunctionField = null;
			this.originalNumberField = null;
			this.originalObjectField = null;
			this.originalObjectPrototypeField = null;
			this.originalRangeErrorField = null;
			this.originalReferenceErrorField = null;
			this.originalRegExpField = null;
			this.originalStringField = null;
			this.originalSyntaxErrorField = null;
			this.originalTypeErrorField = null;
			this.originalVBArrayField = null;
			this.originalURIErrorField = null;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0002CD9C File Offset: 0x0002BD9C
		public static ActiveXObjectConstructor ActiveXObject
		{
			get
			{
				return ActiveXObjectConstructor.ob;
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0002CDA4 File Offset: 0x0002BDA4
		private static void AppendInHex(StringBuilder bs, int value)
		{
			bs.Append('%');
			int num = (value >> 4) & 15;
			bs.Append((char)((num >= 10) ? (num - 10 + 65) : (num + 48)));
			num = value & 15;
			bs.Append((char)((num >= 10) ? (num - 10 + 65) : (num + 48)));
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0002CDFA File Offset: 0x0002BDFA
		public static ArrayConstructor Array
		{
			get
			{
				return ArrayConstructor.ob;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x0002CE01 File Offset: 0x0002BE01
		public static BooleanConstructor Boolean
		{
			get
			{
				return BooleanConstructor.ob;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0002CE08 File Offset: 0x0002BE08
		public static Type boolean
		{
			get
			{
				return Typeob.Boolean;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0002CE0F File Offset: 0x0002BE0F
		public static Type @byte
		{
			get
			{
				return Typeob.Byte;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0002CE16 File Offset: 0x0002BE16
		public static Type @char
		{
			get
			{
				return Typeob.Char;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0002CE1D File Offset: 0x0002BE1D
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_CollectGarbage)]
		public static void CollectGarbage()
		{
			GC.Collect();
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0002CE24 File Offset: 0x0002BE24
		public static DateConstructor Date
		{
			get
			{
				return DateConstructor.ob;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0002CE2B File Offset: 0x0002BE2B
		public static Type @decimal
		{
			get
			{
				return Typeob.Decimal;
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0002CE34 File Offset: 0x0002BE34
		private static string Decode(object encodedURI, GlobalObject.URISetType flags)
		{
			string text = Convert.ToString(encodedURI);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c != '%')
				{
					stringBuilder.Append(c);
				}
				else
				{
					int num = i;
					if (i + 2 >= text.Length)
					{
						throw new JScriptException(JSError.URIDecodeError);
					}
					byte b = GlobalObject.HexValue(text[i + 1], text[i + 2]);
					i += 2;
					char c2;
					if ((b & 128) == 0)
					{
						c2 = (char)b;
					}
					else
					{
						int j = 1;
						while ((((int)b << j) & 128) != 0)
						{
							j++;
						}
						if (j == 1 || j > 4 || i + (j - 1) * 3 >= text.Length)
						{
							throw new JScriptException(JSError.URIDecodeError);
						}
						int num2 = (int)b & (255 >> j + 1);
						while (j > 1)
						{
							if (text[i + 1] != '%')
							{
								throw new JScriptException(JSError.URIDecodeError);
							}
							b = GlobalObject.HexValue(text[i + 2], text[i + 3]);
							i += 3;
							if ((b & 192) != 128)
							{
								throw new JScriptException(JSError.URIDecodeError);
							}
							num2 = (num2 << 6) | (int)(b & 63);
							j--;
						}
						if (num2 >= 55296 && num2 < 57344)
						{
							throw new JScriptException(JSError.URIDecodeError);
						}
						if (num2 < 65536)
						{
							c2 = (char)num2;
						}
						else
						{
							if (num2 > 1114111)
							{
								throw new JScriptException(JSError.URIDecodeError);
							}
							stringBuilder.Append((char)(((num2 - 65536 >> 10) & 1023) + 55296));
							stringBuilder.Append((char)(((num2 - 65536) & 1023) + 56320));
							goto IL_01D4;
						}
					}
					if (GlobalObject.InURISet(c2, flags))
					{
						stringBuilder.Append(text, num, i - num + 1);
					}
					else
					{
						stringBuilder.Append(c2);
					}
				}
				IL_01D4:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0002D02B File Offset: 0x0002C02B
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_decodeURI)]
		public static string decodeURI(object encodedURI)
		{
			return GlobalObject.Decode(encodedURI, GlobalObject.URISetType.Reserved);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0002D034 File Offset: 0x0002C034
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_decodeURIComponent)]
		public static string decodeURIComponent(object encodedURI)
		{
			return GlobalObject.Decode(encodedURI, GlobalObject.URISetType.None);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0002D03D File Offset: 0x0002C03D
		public static Type @double
		{
			get
			{
				return Typeob.Double;
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0002D044 File Offset: 0x0002C044
		private static string Encode(object uri, GlobalObject.URISetType flags)
		{
			string text = Convert.ToString(uri);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (GlobalObject.InURISet(c, flags))
				{
					stringBuilder.Append(c);
				}
				else
				{
					int num = (int)c;
					if (num >= 0 && num <= 127)
					{
						GlobalObject.AppendInHex(stringBuilder, num);
					}
					else if (num >= 128 && num <= 2047)
					{
						GlobalObject.AppendInHex(stringBuilder, (num >> 6) | 192);
						GlobalObject.AppendInHex(stringBuilder, (num & 63) | 128);
					}
					else if (num < 55296 || num > 57343)
					{
						GlobalObject.AppendInHex(stringBuilder, (num >> 12) | 224);
						GlobalObject.AppendInHex(stringBuilder, ((num >> 6) & 63) | 128);
						GlobalObject.AppendInHex(stringBuilder, (num & 63) | 128);
					}
					else
					{
						if (num >= 56320 && num <= 57343)
						{
							throw new JScriptException(JSError.URIEncodeError);
						}
						if (++i >= text.Length)
						{
							throw new JScriptException(JSError.URIEncodeError);
						}
						int num2 = (int)text[i];
						if (num2 < 56320 || num2 > 57343)
						{
							throw new JScriptException(JSError.URIEncodeError);
						}
						num = (num - 55296 << 10) + num2 + 9216;
						GlobalObject.AppendInHex(stringBuilder, (num >> 18) | 240);
						GlobalObject.AppendInHex(stringBuilder, ((num >> 12) & 63) | 128);
						GlobalObject.AppendInHex(stringBuilder, ((num >> 6) & 63) | 128);
						GlobalObject.AppendInHex(stringBuilder, (num & 63) | 128);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0002D1F2 File Offset: 0x0002C1F2
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_encodeURI)]
		public static string encodeURI(object uri)
		{
			return GlobalObject.Encode(uri, (GlobalObject.URISetType)3);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0002D1FB File Offset: 0x0002C1FB
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_encodeURIComponent)]
		public static string encodeURIComponent(object uriComponent)
		{
			return GlobalObject.Encode(uriComponent, GlobalObject.URISetType.Unescaped);
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x0002D204 File Offset: 0x0002C204
		public static EnumeratorConstructor Enumerator
		{
			get
			{
				return EnumeratorConstructor.ob;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0002D20B File Offset: 0x0002C20B
		public static ErrorConstructor Error
		{
			get
			{
				return ErrorConstructor.ob;
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0002D214 File Offset: 0x0002C214
		[NotRecommended("escape")]
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_escape)]
		public static string escape(object @string)
		{
			string text = Convert.ToString(@string);
			string text2 = "0123456789ABCDEF";
			int length = text.Length;
			StringBuilder stringBuilder = new StringBuilder(length * 2);
			int num = -1;
			while (++num < length)
			{
				char c = text[num];
				int num2 = (int)c;
				if ((65 > num2 || num2 > 90) && (97 > num2 || num2 > 122) && (48 > num2 || num2 > 57) && c != '@' && c != '*' && c != '_' && c != '+' && c != '-' && c != '.' && c != '/')
				{
					stringBuilder.Append('%');
					if (num2 < 256)
					{
						stringBuilder.Append(text2[num2 / 16]);
						c = text2[num2 % 16];
					}
					else
					{
						stringBuilder.Append('u');
						stringBuilder.Append(text2[(num2 >> 12) % 16]);
						stringBuilder.Append(text2[(num2 >> 8) % 16]);
						stringBuilder.Append(text2[(num2 >> 4) % 16]);
						c = text2[num2 % 16];
					}
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0002D362 File Offset: 0x0002C362
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_eval)]
		public static object eval(object x)
		{
			throw new JScriptException(JSError.IllegalEval);
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0002D36E File Offset: 0x0002C36E
		public static ErrorConstructor EvalError
		{
			get
			{
				return ErrorConstructor.evalOb;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0002D375 File Offset: 0x0002C375
		public static Type @float
		{
			get
			{
				return Typeob.Single;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0002D37C File Offset: 0x0002C37C
		public static FunctionConstructor Function
		{
			get
			{
				return FunctionConstructor.ob;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0002D384 File Offset: 0x0002C384
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_GetObject)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static object GetObject(object moniker, object progId)
		{
			moniker = Convert.ToPrimitive(moniker, PreferredType.Either);
			if (!(progId is Missing))
			{
				progId = Convert.ToPrimitive(progId, PreferredType.Either);
			}
			string text = ((Convert.GetTypeCode(moniker) == TypeCode.String) ? moniker.ToString() : null);
			string text2 = ((Convert.GetTypeCode(progId) == TypeCode.String) ? progId.ToString() : null);
			if (text == null || (text.Length == 0 && text2 == null))
			{
				throw new JScriptException(JSError.TypeMismatch);
			}
			if (text2 == null && !(progId is Missing))
			{
				throw new JScriptException(JSError.TypeMismatch);
			}
			if (text2 != null && text2.Length == 0)
			{
				throw new JScriptException(JSError.InvalidCall);
			}
			if (text2 == null || text2.Length == 0)
			{
				return Marshal.BindToMoniker(text);
			}
			if (text == null || text.Length == 0)
			{
				return Marshal.GetActiveObject(text2);
			}
			Type typeFromProgID = Type.GetTypeFromProgID(text2);
			object obj = Activator.CreateInstance(typeFromProgID);
			if (obj is UCOMIPersistFile)
			{
				((UCOMIPersistFile)obj).Load(text, 0);
				return obj;
			}
			throw new JScriptException(JSError.FileNotFound);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0002D460 File Offset: 0x0002C460
		internal static int HexDigit(char c)
		{
			if (c >= '0' && c <= '9')
			{
				return (int)(c - '0');
			}
			if (c >= 'A' && c <= 'F')
			{
				return (int)('\n' + c - 'A');
			}
			if (c >= 'a' && c <= 'f')
			{
				return (int)('\n' + c - 'a');
			}
			return -1;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0002D498 File Offset: 0x0002C498
		private static byte HexValue(char ch1, char ch2)
		{
			int num;
			int num2;
			if ((num = GlobalObject.HexDigit(ch1)) < 0 || (num2 = GlobalObject.HexDigit(ch2)) < 0)
			{
				throw new JScriptException(JSError.URIDecodeError);
			}
			return (byte)((num << 4) | num2);
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0002D4CC File Offset: 0x0002C4CC
		public static Type @int
		{
			get
			{
				return Typeob.Int32;
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0002D4D4 File Offset: 0x0002C4D4
		private static bool InURISet(char ch, GlobalObject.URISetType flags)
		{
			if ((flags & GlobalObject.URISetType.Unescaped) != GlobalObject.URISetType.None)
			{
				if ((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
				{
					return true;
				}
				if (ch <= '.')
				{
					if (ch != '!')
					{
						switch (ch)
						{
						case '\'':
						case '(':
						case ')':
						case '*':
						case '-':
						case '.':
							break;
						case '+':
						case ',':
							goto IL_0068;
						default:
							goto IL_0068;
						}
					}
				}
				else if (ch != '_' && ch != '~')
				{
					goto IL_0068;
				}
				return true;
			}
			IL_0068:
			if ((flags & GlobalObject.URISetType.Reserved) != GlobalObject.URISetType.None)
			{
				switch (ch)
				{
				case '#':
				case '$':
				case '&':
					break;
				case '%':
					return false;
				default:
					switch (ch)
					{
					case '+':
					case ',':
					case '/':
						break;
					case '-':
					case '.':
						return false;
					default:
						switch (ch)
						{
						case ':':
						case ';':
						case '=':
						case '?':
						case '@':
							break;
						case '<':
						case '>':
							return false;
						default:
							return false;
						}
						break;
					}
					break;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002D5B0 File Offset: 0x0002C5B0
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_isNaN)]
		public static bool isNaN(object num)
		{
			double num2 = Convert.ToNumber(num);
			return num2 != num2;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0002D5CB File Offset: 0x0002C5CB
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_isFinite)]
		public static bool isFinite(double number)
		{
			return !double.IsInfinity(number) && !double.IsNaN(number);
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0002D5E0 File Offset: 0x0002C5E0
		public static Type @long
		{
			get
			{
				return Typeob.Int64;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0002D5E7 File Offset: 0x0002C5E7
		public static MathObject Math
		{
			get
			{
				if (MathObject.ob == null)
				{
					MathObject.ob = new MathObject(ObjectPrototype.ob);
				}
				return MathObject.ob;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x0002D604 File Offset: 0x0002C604
		public static NumberConstructor Number
		{
			get
			{
				return NumberConstructor.ob;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x0002D60B File Offset: 0x0002C60B
		public static ObjectConstructor Object
		{
			get
			{
				return ObjectConstructor.ob;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0002D612 File Offset: 0x0002C612
		internal virtual ActiveXObjectConstructor originalActiveXObject
		{
			get
			{
				if (this.originalActiveXObjectField == null)
				{
					this.originalActiveXObjectField = ActiveXObjectConstructor.ob;
				}
				return this.originalActiveXObjectField;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x0002D62D File Offset: 0x0002C62D
		internal virtual ArrayConstructor originalArray
		{
			get
			{
				if (this.originalArrayField == null)
				{
					this.originalArrayField = ArrayConstructor.ob;
				}
				return this.originalArrayField;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0002D648 File Offset: 0x0002C648
		internal virtual BooleanConstructor originalBoolean
		{
			get
			{
				if (this.originalBooleanField == null)
				{
					this.originalBooleanField = BooleanConstructor.ob;
				}
				return this.originalBooleanField;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0002D663 File Offset: 0x0002C663
		internal virtual DateConstructor originalDate
		{
			get
			{
				if (this.originalDateField == null)
				{
					this.originalDateField = DateConstructor.ob;
				}
				return this.originalDateField;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0002D67E File Offset: 0x0002C67E
		internal virtual EnumeratorConstructor originalEnumerator
		{
			get
			{
				if (this.originalEnumeratorField == null)
				{
					this.originalEnumeratorField = EnumeratorConstructor.ob;
				}
				return this.originalEnumeratorField;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0002D699 File Offset: 0x0002C699
		internal virtual ErrorConstructor originalError
		{
			get
			{
				if (this.originalErrorField == null)
				{
					this.originalErrorField = ErrorConstructor.ob;
				}
				return this.originalErrorField;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0002D6B4 File Offset: 0x0002C6B4
		internal virtual ErrorConstructor originalEvalError
		{
			get
			{
				if (this.originalEvalErrorField == null)
				{
					this.originalEvalErrorField = ErrorConstructor.evalOb;
				}
				return this.originalEvalErrorField;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0002D6CF File Offset: 0x0002C6CF
		internal virtual FunctionConstructor originalFunction
		{
			get
			{
				if (this.originalFunctionField == null)
				{
					this.originalFunctionField = FunctionConstructor.ob;
				}
				return this.originalFunctionField;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0002D6EA File Offset: 0x0002C6EA
		internal virtual NumberConstructor originalNumber
		{
			get
			{
				if (this.originalNumberField == null)
				{
					this.originalNumberField = NumberConstructor.ob;
				}
				return this.originalNumberField;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0002D705 File Offset: 0x0002C705
		internal virtual ObjectConstructor originalObject
		{
			get
			{
				if (this.originalObjectField == null)
				{
					this.originalObjectField = ObjectConstructor.ob;
				}
				return this.originalObjectField;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0002D720 File Offset: 0x0002C720
		internal virtual ObjectPrototype originalObjectPrototype
		{
			get
			{
				if (this.originalObjectPrototypeField == null)
				{
					this.originalObjectPrototypeField = ObjectPrototype.ob;
				}
				return this.originalObjectPrototypeField;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0002D73B File Offset: 0x0002C73B
		internal virtual ErrorConstructor originalRangeError
		{
			get
			{
				if (this.originalRangeErrorField == null)
				{
					this.originalRangeErrorField = ErrorConstructor.rangeOb;
				}
				return this.originalRangeErrorField;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0002D756 File Offset: 0x0002C756
		internal virtual ErrorConstructor originalReferenceError
		{
			get
			{
				if (this.originalReferenceErrorField == null)
				{
					this.originalReferenceErrorField = ErrorConstructor.referenceOb;
				}
				return this.originalReferenceErrorField;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0002D771 File Offset: 0x0002C771
		internal virtual RegExpConstructor originalRegExp
		{
			get
			{
				if (this.originalRegExpField == null)
				{
					this.originalRegExpField = RegExpConstructor.ob;
				}
				return this.originalRegExpField;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0002D78C File Offset: 0x0002C78C
		internal virtual StringConstructor originalString
		{
			get
			{
				if (this.originalStringField == null)
				{
					this.originalStringField = StringConstructor.ob;
				}
				return this.originalStringField;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0002D7A7 File Offset: 0x0002C7A7
		internal virtual ErrorConstructor originalSyntaxError
		{
			get
			{
				if (this.originalSyntaxErrorField == null)
				{
					this.originalSyntaxErrorField = ErrorConstructor.syntaxOb;
				}
				return this.originalSyntaxErrorField;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0002D7C2 File Offset: 0x0002C7C2
		internal virtual ErrorConstructor originalTypeError
		{
			get
			{
				if (this.originalTypeErrorField == null)
				{
					this.originalTypeErrorField = ErrorConstructor.typeOb;
				}
				return this.originalTypeErrorField;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0002D7DD File Offset: 0x0002C7DD
		internal virtual ErrorConstructor originalURIError
		{
			get
			{
				if (this.originalURIErrorField == null)
				{
					this.originalURIErrorField = ErrorConstructor.uriOb;
				}
				return this.originalURIErrorField;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x0002D7F8 File Offset: 0x0002C7F8
		internal virtual VBArrayConstructor originalVBArray
		{
			get
			{
				if (this.originalVBArrayField == null)
				{
					this.originalVBArrayField = VBArrayConstructor.ob;
				}
				return this.originalVBArrayField;
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0002D814 File Offset: 0x0002C814
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_parseFloat)]
		public static double parseFloat(object @string)
		{
			string text = Convert.ToString(@string);
			return Convert.ToNumber(text, false, false, Missing.Value);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0002D838 File Offset: 0x0002C838
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_parseInt)]
		public static double parseInt(object @string, object radix)
		{
			string text = Convert.ToString(@string);
			return Convert.ToNumber(text, true, true, radix);
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0002D855 File Offset: 0x0002C855
		public static ErrorConstructor RangeError
		{
			get
			{
				return ErrorConstructor.rangeOb;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0002D85C File Offset: 0x0002C85C
		public static ErrorConstructor ReferenceError
		{
			get
			{
				return ErrorConstructor.referenceOb;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0002D863 File Offset: 0x0002C863
		public static RegExpConstructor RegExp
		{
			get
			{
				return RegExpConstructor.ob;
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0002D86A File Offset: 0x0002C86A
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_ScriptEngine)]
		public static string ScriptEngine()
		{
			return "JScript";
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0002D871 File Offset: 0x0002C871
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_ScriptEngineBuildVersion)]
		public static int ScriptEngineBuildVersion()
		{
			return 50727;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0002D878 File Offset: 0x0002C878
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_ScriptEngineMajorVersion)]
		public static int ScriptEngineMajorVersion()
		{
			return 8;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0002D87B File Offset: 0x0002C87B
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_ScriptEngineMinorVersion)]
		public static int ScriptEngineMinorVersion()
		{
			return 0;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0002D87E File Offset: 0x0002C87E
		public static Type @sbyte
		{
			get
			{
				return Typeob.SByte;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0002D885 File Offset: 0x0002C885
		public static Type @short
		{
			get
			{
				return Typeob.Int16;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0002D88C File Offset: 0x0002C88C
		public static StringConstructor String
		{
			get
			{
				return StringConstructor.ob;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0002D893 File Offset: 0x0002C893
		public static ErrorConstructor SyntaxError
		{
			get
			{
				return ErrorConstructor.syntaxOb;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0002D89A File Offset: 0x0002C89A
		public static ErrorConstructor TypeError
		{
			get
			{
				return ErrorConstructor.typeOb;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0002D8A4 File Offset: 0x0002C8A4
		[NotRecommended("unescape")]
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Global_unescape)]
		public static string unescape(object @string)
		{
			string text = Convert.ToString(@string);
			int length = text.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			int num = -1;
			while (++num < length)
			{
				char c = text[num];
				if (c == '%')
				{
					int num2;
					int num3;
					int num4;
					int num5;
					if (num + 5 < length && text[num + 1] == 'u' && (num2 = GlobalObject.HexDigit(text[num + 2])) != -1 && (num3 = GlobalObject.HexDigit(text[num + 3])) != -1 && (num4 = GlobalObject.HexDigit(text[num + 4])) != -1 && (num5 = GlobalObject.HexDigit(text[num + 5])) != -1)
					{
						c = (char)((num2 << 12) + (num3 << 8) + (num4 << 4) + num5);
						num += 5;
					}
					else if (num + 2 < length && (num2 = GlobalObject.HexDigit(text[num + 1])) != -1 && (num3 = GlobalObject.HexDigit(text[num + 2])) != -1)
					{
						c = (char)((num2 << 4) + num3);
						num += 2;
					}
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0002D9BF File Offset: 0x0002C9BF
		public static ErrorConstructor URIError
		{
			get
			{
				return ErrorConstructor.uriOb;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0002D9C6 File Offset: 0x0002C9C6
		public static VBArrayConstructor VBArray
		{
			get
			{
				return VBArrayConstructor.ob;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0002D9CD File Offset: 0x0002C9CD
		public static Type @void
		{
			get
			{
				return Typeob.Void;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0002D9D4 File Offset: 0x0002C9D4
		public static Type @uint
		{
			get
			{
				return Typeob.UInt32;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0002D9DB File Offset: 0x0002C9DB
		public static Type @ulong
		{
			get
			{
				return Typeob.UInt64;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0002D9E2 File Offset: 0x0002C9E2
		public static Type @ushort
		{
			get
			{
				return Typeob.UInt16;
			}
		}

		// Token: 0x040002C0 RID: 704
		public const double Infinity = double.PositiveInfinity;

		// Token: 0x040002C1 RID: 705
		public const double NaN = double.NaN;

		// Token: 0x040002C2 RID: 706
		internal static readonly GlobalObject commonInstance = new GlobalObject();

		// Token: 0x040002C3 RID: 707
		public static readonly Empty undefined = null;

		// Token: 0x040002C4 RID: 708
		protected ActiveXObjectConstructor originalActiveXObjectField;

		// Token: 0x040002C5 RID: 709
		protected ArrayConstructor originalArrayField;

		// Token: 0x040002C6 RID: 710
		protected BooleanConstructor originalBooleanField;

		// Token: 0x040002C7 RID: 711
		protected DateConstructor originalDateField;

		// Token: 0x040002C8 RID: 712
		protected EnumeratorConstructor originalEnumeratorField;

		// Token: 0x040002C9 RID: 713
		protected ErrorConstructor originalErrorField;

		// Token: 0x040002CA RID: 714
		protected ErrorConstructor originalEvalErrorField;

		// Token: 0x040002CB RID: 715
		protected FunctionConstructor originalFunctionField;

		// Token: 0x040002CC RID: 716
		protected NumberConstructor originalNumberField;

		// Token: 0x040002CD RID: 717
		protected ObjectConstructor originalObjectField;

		// Token: 0x040002CE RID: 718
		protected ObjectPrototype originalObjectPrototypeField;

		// Token: 0x040002CF RID: 719
		protected ErrorConstructor originalRangeErrorField;

		// Token: 0x040002D0 RID: 720
		protected ErrorConstructor originalReferenceErrorField;

		// Token: 0x040002D1 RID: 721
		protected RegExpConstructor originalRegExpField;

		// Token: 0x040002D2 RID: 722
		protected StringConstructor originalStringField;

		// Token: 0x040002D3 RID: 723
		protected ErrorConstructor originalSyntaxErrorField;

		// Token: 0x040002D4 RID: 724
		protected ErrorConstructor originalTypeErrorField;

		// Token: 0x040002D5 RID: 725
		protected VBArrayConstructor originalVBArrayField;

		// Token: 0x040002D6 RID: 726
		protected ErrorConstructor originalURIErrorField;

		// Token: 0x02000089 RID: 137
		private enum URISetType
		{
			// Token: 0x040002D8 RID: 728
			None,
			// Token: 0x040002D9 RID: 729
			Reserved,
			// Token: 0x040002DA RID: 730
			Unescaped
		}
	}
}
