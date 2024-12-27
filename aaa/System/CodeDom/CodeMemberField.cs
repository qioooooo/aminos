using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000061 RID: 97
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeMemberField : CodeTypeMember
	{
		// Token: 0x0600038C RID: 908 RVA: 0x000139E2 File Offset: 0x000129E2
		public CodeMemberField()
		{
		}

		// Token: 0x0600038D RID: 909 RVA: 0x000139EA File Offset: 0x000129EA
		public CodeMemberField(CodeTypeReference type, string name)
		{
			this.Type = type;
			base.Name = name;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00013A00 File Offset: 0x00012A00
		public CodeMemberField(string type, string name)
		{
			this.Type = new CodeTypeReference(type);
			base.Name = name;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00013A1B File Offset: 0x00012A1B
		public CodeMemberField(Type type, string name)
		{
			this.Type = new CodeTypeReference(type);
			base.Name = name;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00013A36 File Offset: 0x00012A36
		// (set) Token: 0x06000391 RID: 913 RVA: 0x00013A56 File Offset: 0x00012A56
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00013A5F File Offset: 0x00012A5F
		// (set) Token: 0x06000393 RID: 915 RVA: 0x00013A67 File Offset: 0x00012A67
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

		// Token: 0x04000842 RID: 2114
		private CodeTypeReference type;

		// Token: 0x04000843 RID: 2115
		private CodeExpression initExpression;
	}
}
