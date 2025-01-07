using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace System.Data.Design
{
	internal sealed class GenericNameHandler
	{
		internal GenericNameHandler(ICollection initialNameSet, CodeDomProvider codeProvider)
		{
			this.validator = new MemberNameValidator(initialNameSet, codeProvider, true);
			this.names = new Hashtable(StringComparer.Ordinal);
		}

		internal string AddParameterNameToList(string originalName, string parameterPrefix)
		{
			if (originalName == null)
			{
				throw new ArgumentNullException("originalName");
			}
			string text = originalName;
			if (!StringUtil.Empty(parameterPrefix) && originalName.StartsWith(parameterPrefix, StringComparison.Ordinal))
			{
				text = originalName.Substring(parameterPrefix.Length);
			}
			string newMemberName = this.validator.GetNewMemberName(text);
			this.names.Add(originalName, newMemberName);
			return newMemberName;
		}

		internal string AddNameToList(string originalName)
		{
			if (originalName == null)
			{
				throw new InternalException("Parameter originalName should not be null.");
			}
			string newMemberName = this.validator.GetNewMemberName(originalName);
			this.names.Add(originalName, newMemberName);
			return newMemberName;
		}

		internal string GetNameFromList(string originalName)
		{
			if (originalName == null)
			{
				throw new InternalException("Parameter originalName should not be null.");
			}
			return (string)this.names[originalName];
		}

		private MemberNameValidator validator;

		private Hashtable names;
	}
}
