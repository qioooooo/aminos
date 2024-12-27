using System;
using System.Collections;
using System.Design;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Data.Design
{
	// Token: 0x020000B8 RID: 184
	internal class SimpleNameService : INameService
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x00014F9C File Offset: 0x00013F9C
		internal SimpleNameService()
		{
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00014FB6 File Offset: 0x00013FB6
		internal static SimpleNameService DefaultInstance
		{
			get
			{
				if (SimpleNameService.defaultInstance == null)
				{
					SimpleNameService.defaultInstance = new SimpleNameService();
				}
				return SimpleNameService.defaultInstance;
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00014FCE File Offset: 0x00013FCE
		public string CreateUniqueName(INamedObjectCollection container, string proposed)
		{
			if (!this.NameExist(container, proposed))
			{
				this.ValidateName(proposed);
				return proposed;
			}
			return this.CreateUniqueName(container, proposed, 1);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00014FEC File Offset: 0x00013FEC
		public string CreateUniqueName(INamedObjectCollection container, Type type)
		{
			return this.CreateUniqueName(container, type.Name, 1);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00014FFC File Offset: 0x00013FFC
		public string CreateUniqueName(INamedObjectCollection container, string proposedNameRoot, int startSuffix)
		{
			return this.CreateUniqueNameOnCollection(container, proposedNameRoot, startSuffix);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00015008 File Offset: 0x00014008
		public string CreateUniqueNameOnCollection(ICollection container, string proposedNameRoot, int startSuffix)
		{
			int num = startSuffix;
			if (num < 0)
			{
				num = 0;
			}
			this.ValidateName(proposedNameRoot);
			string text = proposedNameRoot + num.ToString(CultureInfo.CurrentCulture);
			while (this.NameExist(container, text))
			{
				num++;
				if (num >= this.maxNumberOfTrials)
				{
					throw new InternalException("Failed to create unique name after many attempts", 1, true);
				}
				text = proposedNameRoot + num.ToString(CultureInfo.CurrentCulture);
			}
			this.ValidateName(text);
			return text;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00015078 File Offset: 0x00014078
		private bool NameExist(ICollection container, string nameTobeChecked)
		{
			return this.NameExist(container, null, nameTobeChecked);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00015084 File Offset: 0x00014084
		private bool NameExist(ICollection container, INamedObject objTobeChecked, string nameTobeChecked)
		{
			if (StringUtil.Empty(nameTobeChecked) && objTobeChecked != null)
			{
				nameTobeChecked = objTobeChecked.Name;
			}
			foreach (object obj in container)
			{
				INamedObject namedObject = (INamedObject)obj;
				if (namedObject != objTobeChecked && StringUtil.EqualValue(namedObject.Name, nameTobeChecked, !this.caseSensitive))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00015108 File Offset: 0x00014108
		public virtual void ValidateName(string name)
		{
			if (StringUtil.EmptyOrSpace(name))
			{
				throw new NameValidationException(SR.GetString("CM_NameNotEmptyExcption"));
			}
			if (name.Length > 1024)
			{
				throw new NameValidationException(SR.GetString("CM_NameTooLongExcption"));
			}
			Match match = Regex.Match(name, SimpleNameService.regexIdentifier);
			if (!match.Success)
			{
				throw new NameValidationException(SR.GetString("CM_NameInvalid", new object[] { name }));
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001517A File Offset: 0x0001417A
		public void ValidateUniqueName(INamedObjectCollection container, string proposedName)
		{
			this.ValidateUniqueName(container, null, proposedName);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00015188 File Offset: 0x00014188
		public void ValidateUniqueName(INamedObjectCollection container, INamedObject namedObject, string proposedName)
		{
			this.ValidateName(proposedName);
			if (this.NameExist(container, namedObject, proposedName))
			{
				throw new NameValidationException(SR.GetString("CM_NameExist", new object[] { proposedName }));
			}
		}

		// Token: 0x04000C08 RID: 3080
		internal const int DEFAULT_MAX_TRIALS = 100000;

		// Token: 0x04000C09 RID: 3081
		private const int MAX_LENGTH = 1024;

		// Token: 0x04000C0A RID: 3082
		private int maxNumberOfTrials = 100000;

		// Token: 0x04000C0B RID: 3083
		private static readonly string regexAlphaCharacter = "[\\p{L}\\p{Nl}]";

		// Token: 0x04000C0C RID: 3084
		private static readonly string regexUnderscoreCharacter = "\\p{Pc}";

		// Token: 0x04000C0D RID: 3085
		private static readonly string regexIdentifierCharacter = "[\\p{L}\\p{Nl}\\p{Nd}\\p{Mn}\\p{Mc}\\p{Cf}]";

		// Token: 0x04000C0E RID: 3086
		private static readonly string regexIdentifierStart = string.Concat(new string[]
		{
			"(",
			SimpleNameService.regexAlphaCharacter,
			"|(",
			SimpleNameService.regexUnderscoreCharacter,
			SimpleNameService.regexIdentifierCharacter,
			"))"
		});

		// Token: 0x04000C0F RID: 3087
		private static readonly string regexIdentifier = SimpleNameService.regexIdentifierStart + SimpleNameService.regexIdentifierCharacter + "*";

		// Token: 0x04000C10 RID: 3088
		private static SimpleNameService defaultInstance;

		// Token: 0x04000C11 RID: 3089
		private bool caseSensitive = true;
	}
}
