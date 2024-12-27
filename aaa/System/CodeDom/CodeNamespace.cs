using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x02000066 RID: 102
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeNamespace : CodeObject
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060003B3 RID: 947 RVA: 0x00013CB5 File Offset: 0x00012CB5
		// (remove) Token: 0x060003B4 RID: 948 RVA: 0x00013CCE File Offset: 0x00012CCE
		public event EventHandler PopulateComments;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060003B5 RID: 949 RVA: 0x00013CE7 File Offset: 0x00012CE7
		// (remove) Token: 0x060003B6 RID: 950 RVA: 0x00013D00 File Offset: 0x00012D00
		public event EventHandler PopulateImports;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060003B7 RID: 951 RVA: 0x00013D19 File Offset: 0x00012D19
		// (remove) Token: 0x060003B8 RID: 952 RVA: 0x00013D32 File Offset: 0x00012D32
		public event EventHandler PopulateTypes;

		// Token: 0x060003B9 RID: 953 RVA: 0x00013D4B File Offset: 0x00012D4B
		public CodeNamespace()
		{
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00013D7F File Offset: 0x00012D7F
		public CodeNamespace(string name)
		{
			this.Name = name;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00013DBA File Offset: 0x00012DBA
		private CodeNamespace(SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00013DEE File Offset: 0x00012DEE
		public CodeTypeDeclarationCollection Types
		{
			get
			{
				if ((this.populated & 4) == 0)
				{
					this.populated |= 4;
					if (this.PopulateTypes != null)
					{
						this.PopulateTypes(this, EventArgs.Empty);
					}
				}
				return this.classes;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00013E27 File Offset: 0x00012E27
		public CodeNamespaceImportCollection Imports
		{
			get
			{
				if ((this.populated & 1) == 0)
				{
					this.populated |= 1;
					if (this.PopulateImports != null)
					{
						this.PopulateImports(this, EventArgs.Empty);
					}
				}
				return this.imports;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00013E60 File Offset: 0x00012E60
		// (set) Token: 0x060003BF RID: 959 RVA: 0x00013E76 File Offset: 0x00012E76
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00013E7F File Offset: 0x00012E7F
		public CodeCommentStatementCollection Comments
		{
			get
			{
				if ((this.populated & 2) == 0)
				{
					this.populated |= 2;
					if (this.PopulateComments != null)
					{
						this.PopulateComments(this, EventArgs.Empty);
					}
				}
				return this.comments;
			}
		}

		// Token: 0x04000852 RID: 2130
		private const int ImportsCollection = 1;

		// Token: 0x04000853 RID: 2131
		private const int CommentsCollection = 2;

		// Token: 0x04000854 RID: 2132
		private const int TypesCollection = 4;

		// Token: 0x04000855 RID: 2133
		private string name;

		// Token: 0x04000856 RID: 2134
		private CodeNamespaceImportCollection imports = new CodeNamespaceImportCollection();

		// Token: 0x04000857 RID: 2135
		private CodeCommentStatementCollection comments = new CodeCommentStatementCollection();

		// Token: 0x04000858 RID: 2136
		private CodeTypeDeclarationCollection classes = new CodeTypeDeclarationCollection();

		// Token: 0x04000859 RID: 2137
		private CodeNamespaceCollection namespaces = new CodeNamespaceCollection();

		// Token: 0x0400085A RID: 2138
		private int populated;
	}
}
