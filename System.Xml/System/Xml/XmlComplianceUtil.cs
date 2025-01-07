﻿using System;
using System.Text;

namespace System.Xml
{
	internal static class XmlComplianceUtil
	{
		public static string NonCDataNormalize(string value)
		{
			int length = value.Length;
			if (length <= 0)
			{
				return string.Empty;
			}
			int num = 0;
			StringBuilder stringBuilder = null;
			XmlCharType instance = XmlCharType.Instance;
			while (instance.IsWhiteSpace(value[num]))
			{
				num++;
				if (num == length)
				{
					return " ";
				}
			}
			int i = num;
			while (i < length)
			{
				if (!instance.IsWhiteSpace(value[i]))
				{
					i++;
				}
				else
				{
					int num2 = i + 1;
					while (num2 < length && instance.IsWhiteSpace(value[num2]))
					{
						num2++;
					}
					if (num2 == length)
					{
						if (stringBuilder == null)
						{
							return value.Substring(num, i - num);
						}
						stringBuilder.Append(value, num, i - num);
						return stringBuilder.ToString();
					}
					else if (num2 > i + 1 || value[i] != ' ')
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(length);
						}
						stringBuilder.Append(value, num, i - num);
						stringBuilder.Append(' ');
						num = num2;
						i = num2;
					}
					else
					{
						i++;
					}
				}
			}
			if (stringBuilder != null)
			{
				if (num < i)
				{
					stringBuilder.Append(value, num, i - num);
				}
				return stringBuilder.ToString();
			}
			if (num > 0)
			{
				return value.Substring(num, length - num);
			}
			return value;
		}

		public static string CDataNormalize(string value)
		{
			int length = value.Length;
			if (length <= 0)
			{
				return string.Empty;
			}
			int i = 0;
			int num = 0;
			StringBuilder stringBuilder = null;
			while (i < length)
			{
				char c = value[i];
				if (c >= ' ' || (c != '\t' && c != '\n' && c != '\r'))
				{
					i++;
				}
				else
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(length);
					}
					if (num < i)
					{
						stringBuilder.Append(value, num, i - num);
					}
					stringBuilder.Append(' ');
					if (c == '\r' && i + 1 < length && value[i + 1] == '\n')
					{
						i += 2;
					}
					else
					{
						i++;
					}
					num = i;
				}
			}
			if (stringBuilder == null)
			{
				return value;
			}
			if (i > num)
			{
				stringBuilder.Append(value, num, i - num);
			}
			return stringBuilder.ToString();
		}

		public static string StripSpaces(string value)
		{
			int length = value.Length;
			if (length <= 0)
			{
				return string.Empty;
			}
			int num = 0;
			StringBuilder stringBuilder = null;
			while (value[num] == ' ')
			{
				num++;
				if (num == length)
				{
					return " ";
				}
			}
			int i;
			for (i = num; i < length; i++)
			{
				if (value[i] == ' ')
				{
					int num2 = i + 1;
					while (num2 < length && value[num2] == ' ')
					{
						num2++;
					}
					if (num2 == length)
					{
						if (stringBuilder == null)
						{
							return value.Substring(num, i - num);
						}
						stringBuilder.Append(value, num, i - num);
						return stringBuilder.ToString();
					}
					else if (num2 > i + 1)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(length);
						}
						stringBuilder.Append(value, num, i - num + 1);
						num = num2;
						i = num2 - 1;
					}
				}
			}
			if (stringBuilder != null)
			{
				if (i > num)
				{
					stringBuilder.Append(value, num, i - num);
				}
				return stringBuilder.ToString();
			}
			if (num != 0)
			{
				return value.Substring(num, length - num);
			}
			return value;
		}

		public static void StripSpaces(char[] value, int index, ref int len)
		{
			if (len <= 0)
			{
				return;
			}
			int num = index;
			int num2 = index + len;
			while (value[num] == ' ')
			{
				num++;
				if (num == num2)
				{
					len = 1;
					return;
				}
			}
			int num3 = num - index;
			for (int i = num; i < num2; i++)
			{
				char c;
				if ((c = value[i]) == ' ')
				{
					int num4 = i + 1;
					while (num4 < num2 && value[num4] == ' ')
					{
						num4++;
					}
					if (num4 == num2)
					{
						num3 += num4 - i;
						break;
					}
					if (num4 > i + 1)
					{
						num3 += num4 - i - 1;
						i = num4 - 1;
					}
				}
				value[i - num3] = c;
			}
			len -= num3;
		}

		public static bool IsValidLanguageID(char[] value, int startPos, int length)
		{
			int num = length;
			if (num < 2)
			{
				return false;
			}
			bool flag = false;
			XmlCharType instance = XmlCharType.Instance;
			char c = value[startPos];
			if (instance.IsLetter(c))
			{
				int num2;
				if (instance.IsLetter(value[num2 = startPos + 1]))
				{
					if (num == 2)
					{
						return true;
					}
					num--;
					num2++;
				}
				else if ('I' != c && 'i' != c && 'X' != c && 'x' != c)
				{
					return false;
				}
				if (value[num2] != '-')
				{
					return false;
				}
				num -= 2;
				while (num-- > 0)
				{
					c = value[++num2];
					if (instance.IsLetter(c))
					{
						flag = true;
					}
					else
					{
						if (c != '-' || !flag)
						{
							return false;
						}
						flag = false;
					}
				}
				if (flag)
				{
					return true;
				}
			}
			return false;
		}
	}
}
