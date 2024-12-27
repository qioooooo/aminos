using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000088 RID: 136
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeVariableReferenceExpression : CodeExpression
	{
		// Token: 0x060004D8 RID: 1240 RVA: 0x000158DE File Offset: 0x000148DE
		public CodeVariableReferenceExpression()
		{
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000158E6 File Offset: 0x000148E6
		public CodeVariableReferenceExpression(string variableName)
		{
			this.variableName = variableName;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x000158F5 File Offset: 0x000148F5
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x0001590B File Offset: 0x0001490B
		public string VariableName
		{
			get
			{
				if (this.variableName != null)
				{
					return this.variableName;
				}
				return string.Empty;
			}
			set
			{
				this.variableName = value;
			}
		}

		// Token: 0x0400089E RID: 2206
		private string variableName;
	}
}
