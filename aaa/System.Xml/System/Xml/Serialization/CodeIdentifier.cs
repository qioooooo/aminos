using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using Microsoft.CSharp;

namespace System.Xml.Serialization
{
	// Token: 0x020002AE RID: 686
	public class CodeIdentifier
	{
		// Token: 0x060020FA RID: 8442 RVA: 0x0009C094 File Offset: 0x0009B094
		[Obsolete("This class should never get constructed as it contains only static methods.")]
		public CodeIdentifier()
		{
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x0009C09C File Offset: 0x0009B09C
		public static string MakePascal(string identifier)
		{
			identifier = CodeIdentifier.MakeValid(identifier);
			if (identifier.Length <= 2)
			{
				return identifier.ToUpper(CultureInfo.InvariantCulture);
			}
			if (char.IsLower(identifier[0]))
			{
				return char.ToUpper(identifier[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) + identifier.Substring(1);
			}
			return identifier;
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x0009C100 File Offset: 0x0009B100
		public static string MakeCamel(string identifier)
		{
			identifier = CodeIdentifier.MakeValid(identifier);
			if (identifier.Length <= 2)
			{
				return identifier.ToLower(CultureInfo.InvariantCulture);
			}
			if (char.IsUpper(identifier[0]))
			{
				return char.ToLower(identifier[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) + identifier.Substring(1);
			}
			return identifier;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x0009C164 File Offset: 0x0009B164
		public static string MakeValid(string identifier)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			while (num < identifier.Length && stringBuilder.Length < 511)
			{
				char c = identifier[num];
				if (CodeIdentifier.IsValid(c))
				{
					if (stringBuilder.Length == 0 && !CodeIdentifier.IsValidStart(c))
					{
						stringBuilder.Append("Item");
					}
					stringBuilder.Append(c);
				}
				num++;
			}
			if (stringBuilder.Length == 0)
			{
				return "Item";
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x0009C1DD File Offset: 0x0009B1DD
		internal static string MakeValidInternal(string identifier)
		{
			if (identifier.Length > 30)
			{
				return "Item";
			}
			return CodeIdentifier.MakeValid(identifier);
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x0009C1F5 File Offset: 0x0009B1F5
		private static bool IsValidStart(char c)
		{
			return char.GetUnicodeCategory(c) != UnicodeCategory.DecimalDigitNumber;
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x0009C204 File Offset: 0x0009B204
		private static bool IsValid(char c)
		{
			switch (char.GetUnicodeCategory(c))
			{
			case UnicodeCategory.UppercaseLetter:
			case UnicodeCategory.LowercaseLetter:
			case UnicodeCategory.TitlecaseLetter:
			case UnicodeCategory.ModifierLetter:
			case UnicodeCategory.OtherLetter:
			case UnicodeCategory.NonSpacingMark:
			case UnicodeCategory.SpacingCombiningMark:
			case UnicodeCategory.DecimalDigitNumber:
			case UnicodeCategory.ConnectorPunctuation:
				return true;
			case UnicodeCategory.EnclosingMark:
			case UnicodeCategory.LetterNumber:
			case UnicodeCategory.OtherNumber:
			case UnicodeCategory.SpaceSeparator:
			case UnicodeCategory.LineSeparator:
			case UnicodeCategory.ParagraphSeparator:
			case UnicodeCategory.Control:
			case UnicodeCategory.Format:
			case UnicodeCategory.Surrogate:
			case UnicodeCategory.PrivateUse:
			case UnicodeCategory.DashPunctuation:
			case UnicodeCategory.OpenPunctuation:
			case UnicodeCategory.ClosePunctuation:
			case UnicodeCategory.InitialQuotePunctuation:
			case UnicodeCategory.FinalQuotePunctuation:
			case UnicodeCategory.OtherPunctuation:
			case UnicodeCategory.MathSymbol:
			case UnicodeCategory.CurrencySymbol:
			case UnicodeCategory.ModifierSymbol:
			case UnicodeCategory.OtherSymbol:
			case UnicodeCategory.OtherNotAssigned:
				return false;
			default:
				return false;
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x0009C2A0 File Offset: 0x0009B2A0
		internal static void CheckValidIdentifier(string ident)
		{
			if (!CodeGenerator.IsValidLanguageIndependentIdentifier(ident))
			{
				throw new ArgumentException(Res.GetString("XmlInvalidIdentifier", new object[] { ident }), "ident");
			}
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x0009C2D6 File Offset: 0x0009B2D6
		internal static string GetCSharpName(string name)
		{
			return CodeIdentifier.EscapeKeywords(name.Replace('+', '.'), CodeIdentifier.csharp);
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x0009C2EC File Offset: 0x0009B2EC
		private static int GetCSharpName(Type t, Type[] parameters, int index, StringBuilder sb)
		{
			if (t.DeclaringType != null && t.DeclaringType != t)
			{
				index = CodeIdentifier.GetCSharpName(t.DeclaringType, parameters, index, sb);
				sb.Append(".");
			}
			string name = t.Name;
			int num = name.IndexOf('`');
			if (num < 0)
			{
				num = name.IndexOf('!');
			}
			if (num > 0)
			{
				CodeIdentifier.EscapeKeywords(name.Substring(0, num), CodeIdentifier.csharp, sb);
				sb.Append("<");
				int num2 = int.Parse(name.Substring(num + 1), CultureInfo.InvariantCulture) + index;
				while (index < num2)
				{
					sb.Append(CodeIdentifier.GetCSharpName(parameters[index]));
					if (index < num2 - 1)
					{
						sb.Append(",");
					}
					index++;
				}
				sb.Append(">");
			}
			else
			{
				CodeIdentifier.EscapeKeywords(name, CodeIdentifier.csharp, sb);
			}
			return index;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x0009C3C4 File Offset: 0x0009B3C4
		internal static string GetCSharpName(Type t)
		{
			int num = 0;
			while (t.IsArray)
			{
				t = t.GetElementType();
				num++;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("global::");
			string @namespace = t.Namespace;
			if (@namespace != null && @namespace.Length > 0)
			{
				string[] array = @namespace.Split(new char[] { '.' });
				for (int i = 0; i < array.Length; i++)
				{
					CodeIdentifier.EscapeKeywords(array[i], CodeIdentifier.csharp, stringBuilder);
					stringBuilder.Append(".");
				}
			}
			Type[] array2 = ((t.IsGenericType || t.ContainsGenericParameters) ? t.GetGenericArguments() : new Type[0]);
			CodeIdentifier.GetCSharpName(t, array2, 0, stringBuilder);
			for (int j = 0; j < num; j++)
			{
				stringBuilder.Append("[]");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x0009C4A0 File Offset: 0x0009B4A0
		private static void EscapeKeywords(string identifier, CodeDomProvider codeProvider, StringBuilder sb)
		{
			if (identifier == null || identifier.Length == 0)
			{
				return;
			}
			int num = 0;
			while (identifier.EndsWith("[]", StringComparison.Ordinal))
			{
				num++;
				identifier = identifier.Substring(0, identifier.Length - 2);
			}
			if (identifier.Length > 0)
			{
				CodeIdentifier.CheckValidIdentifier(identifier);
				identifier = codeProvider.CreateEscapedIdentifier(identifier);
				sb.Append(identifier);
			}
			for (int i = 0; i < num; i++)
			{
				sb.Append("[]");
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x0009C520 File Offset: 0x0009B520
		private static string EscapeKeywords(string identifier, CodeDomProvider codeProvider)
		{
			if (identifier == null || identifier.Length == 0)
			{
				return identifier;
			}
			string[] array = identifier.Split(new char[] { '.', ',', '<', '>' });
			StringBuilder stringBuilder = new StringBuilder();
			int num = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (num >= 0)
				{
					stringBuilder.Append(identifier.Substring(num, 1));
				}
				num++;
				num += array[i].Length;
				string text = array[i].Trim();
				CodeIdentifier.EscapeKeywords(text, codeProvider, stringBuilder);
			}
			if (stringBuilder.Length != identifier.Length)
			{
				return stringBuilder.ToString();
			}
			return identifier;
		}

		// Token: 0x0400142C RID: 5164
		internal const int MaxIdentifierLength = 511;

		// Token: 0x0400142D RID: 5165
		internal static CodeDomProvider csharp = new CSharpCodeProvider();
	}
}
