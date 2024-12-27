using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200014A RID: 330
	internal class DbgData
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00049AAE File Offset: 0x00048AAE
		public XPathNavigator StyleSheet
		{
			get
			{
				return this.styleSheet;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00049AB6 File Offset: 0x00048AB6
		public VariableAction[] Variables
		{
			get
			{
				return this.variables;
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00049AC0 File Offset: 0x00048AC0
		public DbgData(Compiler compiler)
		{
			DbgCompiler dbgCompiler = (DbgCompiler)compiler;
			this.styleSheet = dbgCompiler.Input.Navigator.Clone();
			this.variables = dbgCompiler.LocalVariables;
			dbgCompiler.Debugger.OnInstructionCompile(this.StyleSheet);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00049B0D File Offset: 0x00048B0D
		internal void ReplaceVariables(VariableAction[] vars)
		{
			this.variables = vars;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00049B16 File Offset: 0x00048B16
		private DbgData()
		{
			this.styleSheet = null;
			this.variables = new VariableAction[0];
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00049B31 File Offset: 0x00048B31
		public static DbgData Empty
		{
			get
			{
				return DbgData.s_nullDbgData;
			}
		}

		// Token: 0x04000961 RID: 2401
		private XPathNavigator styleSheet;

		// Token: 0x04000962 RID: 2402
		private VariableAction[] variables;

		// Token: 0x04000963 RID: 2403
		private static DbgData s_nullDbgData = new DbgData();
	}
}
