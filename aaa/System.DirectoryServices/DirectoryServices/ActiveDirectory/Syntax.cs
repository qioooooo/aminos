using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000083 RID: 131
	internal class Syntax
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x0001421F File Offset: 0x0001321F
		public Syntax(string attributeSyntax, int oMSyntax, OMObjectClass oMObjectClass)
		{
			this.attributeSyntax = attributeSyntax;
			this.oMSyntax = oMSyntax;
			this.oMObjectClass = oMObjectClass;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001423C File Offset: 0x0001323C
		public bool Equals(Syntax syntax)
		{
			bool flag = true;
			if (!syntax.attributeSyntax.Equals(this.attributeSyntax) || syntax.oMSyntax != this.oMSyntax)
			{
				flag = false;
			}
			else if ((this.oMObjectClass != null && syntax.oMObjectClass == null) || (this.oMObjectClass == null && syntax.oMObjectClass != null) || (this.oMObjectClass != null && syntax.oMObjectClass != null && !this.oMObjectClass.Equals(syntax.oMObjectClass)))
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x04000392 RID: 914
		public string attributeSyntax;

		// Token: 0x04000393 RID: 915
		public int oMSyntax;

		// Token: 0x04000394 RID: 916
		public OMObjectClass oMObjectClass;
	}
}
