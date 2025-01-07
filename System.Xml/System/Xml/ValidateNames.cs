using System;
using System.Xml.XPath;

namespace System.Xml
{
	internal class ValidateNames
	{
		private ValidateNames()
		{
		}

		public unsafe static int ParseNCName(string s, int offset)
		{
			int num = offset;
			XmlCharType instance = XmlCharType.Instance;
			if (offset < s.Length && (instance.charProperties[s[offset]] & 4) != 0)
			{
				offset++;
				while (offset < s.Length && (instance.charProperties[s[offset]] & 8) != 0)
				{
					offset++;
				}
			}
			return offset - num;
		}

		public static string ParseNCNameThrow(string s)
		{
			ValidateNames.ParseNCNameInternal(s, true);
			return s;
		}

		private static bool ParseNCNameInternal(string s, bool throwOnError)
		{
			int num = ValidateNames.ParseNCName(s, 0);
			if (num == 0 || num != s.Length)
			{
				if (throwOnError)
				{
					ValidateNames.ThrowInvalidName(s, 0, num);
				}
				return false;
			}
			return true;
		}

		public static int ParseQName(string s, int offset, out int colonOffset)
		{
			colonOffset = 0;
			int num = ValidateNames.ParseNCName(s, offset);
			if (num != 0)
			{
				offset += num;
				if (offset < s.Length && s[offset] == ':')
				{
					int num2 = ValidateNames.ParseNCName(s, offset + 1);
					if (num2 != 0)
					{
						colonOffset = offset;
						num += num2 + 1;
					}
				}
			}
			return num;
		}

		public static void ParseQNameThrow(string s, out string prefix, out string localName)
		{
			int num2;
			int num = ValidateNames.ParseQName(s, 0, out num2);
			if (num == 0 || num != s.Length)
			{
				ValidateNames.ThrowInvalidName(s, 0, num);
			}
			if (num2 != 0)
			{
				prefix = s.Substring(0, num2);
				localName = s.Substring(num2 + 1);
				return;
			}
			prefix = "";
			localName = s;
		}

		public static void ParseNameTestThrow(string s, out string prefix, out string localName)
		{
			int num;
			if (s.Length != 0 && s[0] == '*')
			{
				string text;
				localName = (text = null);
				prefix = text;
				num = 1;
			}
			else
			{
				num = ValidateNames.ParseNCName(s, 0);
				if (num != 0)
				{
					localName = s.Substring(0, num);
					if (num < s.Length && s[num] == ':')
					{
						prefix = localName;
						int num2 = num + 1;
						if (num2 < s.Length && s[num2] == '*')
						{
							localName = null;
							num += 2;
						}
						else
						{
							int num3 = ValidateNames.ParseNCName(s, num2);
							if (num3 != 0)
							{
								localName = s.Substring(num2, num3);
								num += num3 + 1;
							}
						}
					}
					else
					{
						prefix = string.Empty;
					}
				}
				else
				{
					string text2;
					localName = (text2 = null);
					prefix = text2;
				}
			}
			if (num == 0 || num != s.Length)
			{
				ValidateNames.ThrowInvalidName(s, 0, num);
			}
		}

		public static void ThrowInvalidName(string s, int offsetStartChar, int offsetBadChar)
		{
			if (offsetStartChar >= s.Length)
			{
				throw new XmlException("Xml_EmptyName", string.Empty);
			}
			if (XmlCharType.Instance.IsNCNameChar(s[offsetBadChar]) && !XmlCharType.Instance.IsStartNCNameChar(s[offsetBadChar]))
			{
				throw new XmlException("Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(s[offsetBadChar]));
			}
			throw new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(s[offsetBadChar]));
		}

		public static Exception GetInvalidNameException(string s, int offsetStartChar, int offsetBadChar)
		{
			if (offsetStartChar >= s.Length)
			{
				return new XmlException("Xml_EmptyName", string.Empty);
			}
			if (XmlCharType.Instance.IsNCNameChar(s[offsetBadChar]) && !XmlCharType.Instance.IsStartNCNameChar(s[offsetBadChar]))
			{
				return new XmlException("Xml_BadStartNameChar", XmlException.BuildCharExceptionStr(s[offsetBadChar]));
			}
			return new XmlException("Xml_BadNameChar", XmlException.BuildCharExceptionStr(s[offsetBadChar]));
		}

		public static bool StartsWithXml(string s)
		{
			return s.Length >= 3 && (s[0] == 'x' || s[0] == 'X') && (s[1] == 'm' || s[1] == 'M') && (s[2] == 'l' || s[2] == 'L');
		}

		public static bool IsReservedNamespace(string s)
		{
			return s.Equals("http://www.w3.org/XML/1998/namespace") || s.Equals("http://www.w3.org/2000/xmlns/");
		}

		public static void ValidateNameThrow(string prefix, string localName, string ns, XPathNodeType nodeKind, ValidateNames.Flags flags)
		{
			ValidateNames.ValidateNameInternal(prefix, localName, ns, nodeKind, flags, true);
		}

		public static bool ValidateName(string prefix, string localName, string ns, XPathNodeType nodeKind, ValidateNames.Flags flags)
		{
			return ValidateNames.ValidateNameInternal(prefix, localName, ns, nodeKind, flags, false);
		}

		private static bool ValidateNameInternal(string prefix, string localName, string ns, XPathNodeType nodeKind, ValidateNames.Flags flags, bool throwOnError)
		{
			if ((flags & ValidateNames.Flags.NCNames) != (ValidateNames.Flags)0)
			{
				if (prefix.Length != 0 && !ValidateNames.ParseNCNameInternal(prefix, throwOnError))
				{
					return false;
				}
				if (localName.Length != 0 && !ValidateNames.ParseNCNameInternal(localName, throwOnError))
				{
					return false;
				}
			}
			if ((flags & ValidateNames.Flags.CheckLocalName) != (ValidateNames.Flags)0)
			{
				switch (nodeKind)
				{
				case XPathNodeType.Element:
					break;
				case XPathNodeType.Attribute:
					if (ns.Length == 0 && localName.Equals("xmlns"))
					{
						if (throwOnError)
						{
							throw new XmlException("XmlBadName", new string[]
							{
								nodeKind.ToString(),
								localName
							});
						}
						return false;
					}
					break;
				default:
					if (nodeKind != XPathNodeType.ProcessingInstruction)
					{
						if (localName.Length == 0)
						{
							goto IL_0102;
						}
						if (throwOnError)
						{
							throw new XmlException("XmlNoNameAllowed", nodeKind.ToString());
						}
						return false;
					}
					else
					{
						if (localName.Length != 0 && (localName.Length != 3 || !ValidateNames.StartsWithXml(localName)))
						{
							goto IL_0102;
						}
						if (throwOnError)
						{
							throw new XmlException("Xml_InvalidPIName", localName);
						}
						return false;
					}
					break;
				}
				if (localName.Length == 0)
				{
					if (throwOnError)
					{
						throw new XmlException("Xdom_Empty_LocalName", string.Empty);
					}
					return false;
				}
			}
			IL_0102:
			if ((flags & ValidateNames.Flags.CheckPrefixMapping) != (ValidateNames.Flags)0)
			{
				switch (nodeKind)
				{
				case XPathNodeType.Element:
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
					if (ns.Length == 0)
					{
						if (prefix.Length == 0)
						{
							return true;
						}
						if (throwOnError)
						{
							throw new XmlException("Xml_PrefixForEmptyNs", string.Empty);
						}
						return false;
					}
					else if (prefix.Length == 0 && nodeKind == XPathNodeType.Attribute)
					{
						if (throwOnError)
						{
							throw new XmlException("XmlBadName", new string[]
							{
								nodeKind.ToString(),
								localName
							});
						}
						return false;
					}
					else if (prefix.Equals("xml"))
					{
						if (ns.Equals("http://www.w3.org/XML/1998/namespace"))
						{
							return true;
						}
						if (throwOnError)
						{
							throw new XmlException("Xml_XmlPrefix", string.Empty);
						}
						return false;
					}
					else if (prefix.Equals("xmlns"))
					{
						if (throwOnError)
						{
							throw new XmlException("Xml_XmlnsPrefix", string.Empty);
						}
						return false;
					}
					else
					{
						if (!ValidateNames.IsReservedNamespace(ns))
						{
							return true;
						}
						if (throwOnError)
						{
							throw new XmlException("Xml_NamespaceDeclXmlXmlns", string.Empty);
						}
						return false;
					}
					break;
				case XPathNodeType.ProcessingInstruction:
					if (prefix.Length == 0 && ns.Length == 0)
					{
						return true;
					}
					if (throwOnError)
					{
						throw new XmlException("Xml_InvalidPIName", ValidateNames.CreateName(prefix, localName));
					}
					return false;
				}
				if (prefix.Length != 0 || ns.Length != 0)
				{
					if (throwOnError)
					{
						throw new XmlException("XmlNoNameAllowed", nodeKind.ToString());
					}
					return false;
				}
			}
			return true;
		}

		private static string CreateName(string prefix, string localName)
		{
			if (prefix.Length == 0)
			{
				return localName;
			}
			return prefix + ":" + localName;
		}

		internal static void SplitQName(string name, out string prefix, out string lname)
		{
			int num = name.IndexOf(':');
			if (-1 == num)
			{
				prefix = string.Empty;
				lname = name;
				return;
			}
			if (num == 0 || name.Length - 1 == num)
			{
				throw new ArgumentException(Res.GetString("Xml_BadNameChar", XmlException.BuildCharExceptionStr(':')), "name");
			}
			prefix = name.Substring(0, num);
			num++;
			lname = name.Substring(num, name.Length - num);
		}

		public enum Flags
		{
			NCNames = 1,
			CheckLocalName,
			CheckPrefixMapping = 4,
			All = 7,
			AllExceptNCNames = 6,
			AllExceptPrefixMapping = 3
		}
	}
}
