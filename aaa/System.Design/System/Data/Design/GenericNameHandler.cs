using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x020000A5 RID: 165
	internal sealed class GenericNameHandler
	{
		// Token: 0x060007B8 RID: 1976 RVA: 0x00011D18 File Offset: 0x00010D18
		internal GenericNameHandler(ICollection initialNameSet, CodeDomProvider codeProvider)
		{
			this.validator = new MemberNameValidator(initialNameSet, codeProvider, true);
			this.names = new Hashtable(StringComparer.Ordinal);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00011D40 File Offset: 0x00010D40
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

		// Token: 0x060007BA RID: 1978 RVA: 0x00011D98 File Offset: 0x00010D98
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

		// Token: 0x060007BB RID: 1979 RVA: 0x00011DCE File Offset: 0x00010DCE
		internal string GetNameFromList(string originalName)
		{
			if (originalName == null)
			{
				throw new InternalException("Parameter originalName should not be null.");
			}
			return (string)this.names[originalName];
		}

		// Token: 0x04000BB7 RID: 2999
		private MemberNameValidator validator;

		// Token: 0x04000BB8 RID: 3000
		private Hashtable names;
	}
}
