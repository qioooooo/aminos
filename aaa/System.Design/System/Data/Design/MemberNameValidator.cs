using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.Data.Design
{
	// Token: 0x020000A7 RID: 167
	internal sealed class MemberNameValidator
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00011EFB File Offset: 0x00010EFB
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x00011F03 File Offset: 0x00010F03
		internal bool UseSuffix
		{
			get
			{
				return this.useSuffix;
			}
			set
			{
				this.useSuffix = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00011F0C File Offset: 0x00010F0C
		private static Dictionary<string, string[]> InvalidEverettIdentifiers
		{
			get
			{
				if (MemberNameValidator.invalidEverettIdentifiers == null)
				{
					MemberNameValidator.invalidEverettIdentifiers = new Dictionary<string, string[]>();
					MemberNameValidator.invalidEverettIdentifiers.Add(".vb", MemberNameValidator.invalidEverettIdentifiersVb);
				}
				return MemberNameValidator.invalidEverettIdentifiers;
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00011F38 File Offset: 0x00010F38
		internal MemberNameValidator(ICollection initialNameSet, CodeDomProvider codeProvider, bool languageCaseInsensitive)
		{
			this.codeProvider = codeProvider;
			this.languageCaseInsensitive = languageCaseInsensitive;
			if (initialNameSet != null)
			{
				this.bookedMemberNames = new ArrayList(initialNameSet.Count);
				using (IEnumerator enumerator = initialNameSet.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						string text = (string)obj;
						this.AddNameToList(text);
					}
					return;
				}
			}
			this.bookedMemberNames = new ArrayList();
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00011FC0 File Offset: 0x00010FC0
		internal string GetCandidateMemberName(string originalName)
		{
			if (originalName == null)
			{
				throw new InternalException("Member name cannot be null.");
			}
			string text = this.GenerateIdName(originalName);
			string text2 = text;
			int num = 0;
			while (this.ListContains(text))
			{
				num++;
				text = text2 + num.ToString(CultureInfo.CurrentCulture);
				if (!this.codeProvider.IsValidIdentifier(text))
				{
					throw new InternalException(string.Format(CultureInfo.CurrentCulture, "Unable to generate valid identifier from name: {0}.", new object[] { originalName }));
				}
				if (num > 200)
				{
					throw new InternalException(string.Format(CultureInfo.CurrentCulture, "Unable to generate unique identifier from name: {0}. Too many attempts.", new object[] { originalName }));
				}
			}
			return text;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00012068 File Offset: 0x00011068
		internal string GetNewMemberName(string originalName)
		{
			string candidateMemberName = this.GetCandidateMemberName(originalName);
			this.AddNameToList(candidateMemberName);
			return candidateMemberName;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00012085 File Offset: 0x00011085
		internal string GenerateIdName(string name)
		{
			return MemberNameValidator.GenerateIdName(name, this.codeProvider, this.UseSuffix);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00012099 File Offset: 0x00011099
		internal static string GenerateIdName(string name, CodeDomProvider codeProvider, bool useSuffix)
		{
			return MemberNameValidator.GenerateIdName(name, codeProvider, useSuffix, 100);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000120A8 File Offset: 0x000110A8
		internal static string GenerateIdName(string name, CodeDomProvider codeProvider, bool useSuffix, int additionalCharsToTruncate)
		{
			if (!useSuffix)
			{
				name = MemberNameValidator.GetBackwardCompatibleIdentifier(name, codeProvider);
			}
			if (codeProvider.IsValidIdentifier(name))
			{
				return name;
			}
			string text = name.Replace(' ', '_');
			if (!codeProvider.IsValidIdentifier(text))
			{
				if (!useSuffix)
				{
					text = "_" + text;
				}
				for (int i = 0; i < text.Length; i++)
				{
					UnicodeCategory unicodeCategory = char.GetUnicodeCategory(text[i]);
					if (unicodeCategory != UnicodeCategory.UppercaseLetter && UnicodeCategory.LowercaseLetter != unicodeCategory && UnicodeCategory.TitlecaseLetter != unicodeCategory && UnicodeCategory.ModifierLetter != unicodeCategory && UnicodeCategory.OtherLetter != unicodeCategory && UnicodeCategory.NonSpacingMark != unicodeCategory && UnicodeCategory.SpacingCombiningMark != unicodeCategory && UnicodeCategory.DecimalDigitNumber != unicodeCategory && UnicodeCategory.ConnectorPunctuation != unicodeCategory)
					{
						text = text.Replace(text[i], '_');
					}
				}
			}
			int num = 0;
			string text2 = text;
			while (!codeProvider.IsValidIdentifier(text) && num < 200)
			{
				num++;
				text = "_" + text;
			}
			if (num >= 200)
			{
				text = text2;
				while (!codeProvider.IsValidIdentifier(text) && text.Length > 0)
				{
					text = text.Remove(text.Length - 1);
				}
				if (text.Length == 0)
				{
					return text2;
				}
				if (additionalCharsToTruncate > 0 && text.Length > additionalCharsToTruncate && codeProvider.IsValidIdentifier(text.Remove(text.Length - additionalCharsToTruncate)))
				{
					text = text.Remove(text.Length - additionalCharsToTruncate);
				}
			}
			return text;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000121D5 File Offset: 0x000111D5
		private void AddNameToList(string name)
		{
			if (this.languageCaseInsensitive)
			{
				this.bookedMemberNames.Add(name.ToUpperInvariant());
				return;
			}
			this.bookedMemberNames.Add(name);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000121FF File Offset: 0x000111FF
		private bool ListContains(string name)
		{
			if (this.languageCaseInsensitive)
			{
				return this.bookedMemberNames.Contains(name.ToUpperInvariant());
			}
			return this.bookedMemberNames.Contains(name);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00012228 File Offset: 0x00011228
		private static string GetBackwardCompatibleIdentifier(string identifier, CodeDomProvider provider)
		{
			string text = "." + provider.FileExtension;
			if (text.StartsWith("..", StringComparison.Ordinal))
			{
				text = text.Substring(1);
			}
			if (MemberNameValidator.InvalidEverettIdentifiers.ContainsKey(text))
			{
				string[] array = MemberNameValidator.InvalidEverettIdentifiers[text];
				if (array != null)
				{
					bool flag = (provider.LanguageOptions & LanguageOptions.CaseInsensitive) > LanguageOptions.None;
					for (int i = 0; i < array.Length; i++)
					{
						if (StringUtil.EqualValue(identifier, array[i], flag))
						{
							return "_" + identifier;
						}
					}
				}
			}
			return identifier;
		}

		// Token: 0x04000BBD RID: 3005
		private const int maxGenerationAttempts = 200;

		// Token: 0x04000BBE RID: 3006
		private const int additionalTruncationChars = 100;

		// Token: 0x04000BBF RID: 3007
		private ArrayList bookedMemberNames;

		// Token: 0x04000BC0 RID: 3008
		private CodeDomProvider codeProvider;

		// Token: 0x04000BC1 RID: 3009
		private bool languageCaseInsensitive;

		// Token: 0x04000BC2 RID: 3010
		private bool useSuffix;

		// Token: 0x04000BC3 RID: 3011
		private static string[] invalidEverettIdentifiersVb = new string[] { "region", "externalsource" };

		// Token: 0x04000BC4 RID: 3012
		private static Dictionary<string, string[]> invalidEverettIdentifiers = null;
	}
}
