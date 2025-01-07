using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.Data.Design
{
	internal sealed class MemberNameValidator
	{
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

		internal string GetNewMemberName(string originalName)
		{
			string candidateMemberName = this.GetCandidateMemberName(originalName);
			this.AddNameToList(candidateMemberName);
			return candidateMemberName;
		}

		internal string GenerateIdName(string name)
		{
			return MemberNameValidator.GenerateIdName(name, this.codeProvider, this.UseSuffix);
		}

		internal static string GenerateIdName(string name, CodeDomProvider codeProvider, bool useSuffix)
		{
			return MemberNameValidator.GenerateIdName(name, codeProvider, useSuffix, 100);
		}

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

		private void AddNameToList(string name)
		{
			if (this.languageCaseInsensitive)
			{
				this.bookedMemberNames.Add(name.ToUpperInvariant());
				return;
			}
			this.bookedMemberNames.Add(name);
		}

		private bool ListContains(string name)
		{
			if (this.languageCaseInsensitive)
			{
				return this.bookedMemberNames.Contains(name.ToUpperInvariant());
			}
			return this.bookedMemberNames.Contains(name);
		}

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

		private const int maxGenerationAttempts = 200;

		private const int additionalTruncationChars = 100;

		private ArrayList bookedMemberNames;

		private CodeDomProvider codeProvider;

		private bool languageCaseInsensitive;

		private bool useSuffix;

		private static string[] invalidEverettIdentifiersVb = new string[] { "region", "externalsource" };

		private static Dictionary<string, string[]> invalidEverettIdentifiers = null;
	}
}
