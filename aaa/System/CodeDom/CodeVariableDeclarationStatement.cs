using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000087 RID: 135
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeVariableDeclarationStatement : CodeStatement
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x000157D0 File Offset: 0x000147D0
		public CodeVariableDeclarationStatement()
		{
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000157D8 File Offset: 0x000147D8
		public CodeVariableDeclarationStatement(CodeTypeReference type, string name)
		{
			this.Type = type;
			this.Name = name;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x000157EE File Offset: 0x000147EE
		public CodeVariableDeclarationStatement(string type, string name)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00015809 File Offset: 0x00014809
		public CodeVariableDeclarationStatement(Type type, string name)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00015824 File Offset: 0x00014824
		public CodeVariableDeclarationStatement(CodeTypeReference type, string name, CodeExpression initExpression)
		{
			this.Type = type;
			this.Name = name;
			this.InitExpression = initExpression;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00015841 File Offset: 0x00014841
		public CodeVariableDeclarationStatement(string type, string name, CodeExpression initExpression)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
			this.InitExpression = initExpression;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00015863 File Offset: 0x00014863
		public CodeVariableDeclarationStatement(Type type, string name, CodeExpression initExpression)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
			this.InitExpression = initExpression;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00015885 File Offset: 0x00014885
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x0001588D File Offset: 0x0001488D
		public CodeExpression InitExpression
		{
			get
			{
				return this.initExpression;
			}
			set
			{
				this.initExpression = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00015896 File Offset: 0x00014896
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x000158AC File Offset: 0x000148AC
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

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x000158B5 File Offset: 0x000148B5
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x000158D5 File Offset: 0x000148D5
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

		// Token: 0x0400089B RID: 2203
		private CodeTypeReference type;

		// Token: 0x0400089C RID: 2204
		private string name;

		// Token: 0x0400089D RID: 2205
		private CodeExpression initExpression;
	}
}
