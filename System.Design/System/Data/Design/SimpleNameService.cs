using System;
using System.Collections;
using System.Design;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Data.Design
{
	internal class SimpleNameService : INameService
	{
		internal SimpleNameService()
		{
		}

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

		public string CreateUniqueName(INamedObjectCollection container, string proposed)
		{
			if (!this.NameExist(container, proposed))
			{
				this.ValidateName(proposed);
				return proposed;
			}
			return this.CreateUniqueName(container, proposed, 1);
		}

		public string CreateUniqueName(INamedObjectCollection container, Type type)
		{
			return this.CreateUniqueName(container, type.Name, 1);
		}

		public string CreateUniqueName(INamedObjectCollection container, string proposedNameRoot, int startSuffix)
		{
			return this.CreateUniqueNameOnCollection(container, proposedNameRoot, startSuffix);
		}

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

		private bool NameExist(ICollection container, string nameTobeChecked)
		{
			return this.NameExist(container, null, nameTobeChecked);
		}

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

		public void ValidateUniqueName(INamedObjectCollection container, string proposedName)
		{
			this.ValidateUniqueName(container, null, proposedName);
		}

		public void ValidateUniqueName(INamedObjectCollection container, INamedObject namedObject, string proposedName)
		{
			this.ValidateName(proposedName);
			if (this.NameExist(container, namedObject, proposedName))
			{
				throw new NameValidationException(SR.GetString("CM_NameExist", new object[] { proposedName }));
			}
		}

		internal const int DEFAULT_MAX_TRIALS = 100000;

		private const int MAX_LENGTH = 1024;

		private int maxNumberOfTrials = 100000;

		private static readonly string regexAlphaCharacter = "[\\p{L}\\p{Nl}]";

		private static readonly string regexUnderscoreCharacter = "\\p{Pc}";

		private static readonly string regexIdentifierCharacter = "[\\p{L}\\p{Nl}\\p{Nd}\\p{Mn}\\p{Mc}\\p{Cf}]";

		private static readonly string regexIdentifierStart = string.Concat(new string[]
		{
			"(",
			SimpleNameService.regexAlphaCharacter,
			"|(",
			SimpleNameService.regexUnderscoreCharacter,
			SimpleNameService.regexIdentifierCharacter,
			"))"
		});

		private static readonly string regexIdentifier = SimpleNameService.regexIdentifierStart + SimpleNameService.regexIdentifierCharacter + "*";

		private static SimpleNameService defaultInstance;

		private bool caseSensitive = true;
	}
}
