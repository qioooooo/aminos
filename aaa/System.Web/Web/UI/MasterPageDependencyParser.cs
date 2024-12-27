using System;
using System.Collections;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000391 RID: 913
	internal class MasterPageDependencyParser : UserControlDependencyParser
	{
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06002C65 RID: 11365 RVA: 0x000C648A File Offset: 0x000C548A
		internal override string DefaultDirectiveName
		{
			get
			{
				return "master";
			}
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000C6494 File Offset: 0x000C5494
		internal override void ProcessDirective(string directiveName, IDictionary directive)
		{
			base.ProcessDirective(directiveName, directive);
			if (StringUtil.EqualsIgnoreCase(directiveName, "masterType"))
			{
				VirtualPath andRemoveVirtualPathAttribute = Util.GetAndRemoveVirtualPathAttribute(directive, "virtualPath");
				if (andRemoveVirtualPathAttribute != null)
				{
					base.AddDependency(andRemoveVirtualPathAttribute);
				}
			}
		}
	}
}
