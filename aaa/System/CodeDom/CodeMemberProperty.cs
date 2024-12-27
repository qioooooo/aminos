using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000062 RID: 98
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeMemberProperty : CodeTypeMember
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00013A70 File Offset: 0x00012A70
		// (set) Token: 0x06000395 RID: 917 RVA: 0x00013A78 File Offset: 0x00012A78
		public CodeTypeReference PrivateImplementationType
		{
			get
			{
				return this.privateImplements;
			}
			set
			{
				this.privateImplements = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00013A81 File Offset: 0x00012A81
		public CodeTypeReferenceCollection ImplementationTypes
		{
			get
			{
				if (this.implementationTypes == null)
				{
					this.implementationTypes = new CodeTypeReferenceCollection();
				}
				return this.implementationTypes;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00013A9C File Offset: 0x00012A9C
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00013ABC File Offset: 0x00012ABC
		public CodeTypeReference Type
		{
			get
			{
				if (this.type == null)
				{
					this.type = new CodeTypeReference("");
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00013AC5 File Offset: 0x00012AC5
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00013ADF File Offset: 0x00012ADF
		public bool HasGet
		{
			get
			{
				return this.hasGet || this.getStatements.Count > 0;
			}
			set
			{
				this.hasGet = value;
				if (!value)
				{
					this.getStatements.Clear();
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00013AF6 File Offset: 0x00012AF6
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00013B10 File Offset: 0x00012B10
		public bool HasSet
		{
			get
			{
				return this.hasSet || this.setStatements.Count > 0;
			}
			set
			{
				this.hasSet = value;
				if (!value)
				{
					this.setStatements.Clear();
				}
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00013B27 File Offset: 0x00012B27
		public CodeStatementCollection GetStatements
		{
			get
			{
				return this.getStatements;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00013B2F File Offset: 0x00012B2F
		public CodeStatementCollection SetStatements
		{
			get
			{
				return this.setStatements;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00013B37 File Offset: 0x00012B37
		public CodeParameterDeclarationExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04000844 RID: 2116
		private CodeTypeReference type;

		// Token: 0x04000845 RID: 2117
		private CodeParameterDeclarationExpressionCollection parameters = new CodeParameterDeclarationExpressionCollection();

		// Token: 0x04000846 RID: 2118
		private bool hasGet;

		// Token: 0x04000847 RID: 2119
		private bool hasSet;

		// Token: 0x04000848 RID: 2120
		private CodeStatementCollection getStatements = new CodeStatementCollection();

		// Token: 0x04000849 RID: 2121
		private CodeStatementCollection setStatements = new CodeStatementCollection();

		// Token: 0x0400084A RID: 2122
		private CodeTypeReference privateImplements;

		// Token: 0x0400084B RID: 2123
		private CodeTypeReferenceCollection implementationTypes;
	}
}
