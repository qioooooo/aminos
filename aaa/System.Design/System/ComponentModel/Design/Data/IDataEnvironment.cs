using System;
using System.CodeDom;
using System.Collections;
using System.Data.Common;
using System.Windows.Forms;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x0200014C RID: 332
	public interface IDataEnvironment
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000CAA RID: 3242
		ICollection Connections { get; }

		// Token: 0x06000CAB RID: 3243
		DesignerDataConnection BuildConnection(IWin32Window owner, DesignerDataConnection initialConnection);

		// Token: 0x06000CAC RID: 3244
		string BuildQuery(IWin32Window owner, DesignerDataConnection connection, QueryBuilderMode mode, string initialQueryText);

		// Token: 0x06000CAD RID: 3245
		DesignerDataConnection ConfigureConnection(IWin32Window owner, DesignerDataConnection connection, string name);

		// Token: 0x06000CAE RID: 3246
		IDesignerDataSchema GetConnectionSchema(DesignerDataConnection connection);

		// Token: 0x06000CAF RID: 3247
		DbConnection GetDesignTimeConnection(DesignerDataConnection connection);

		// Token: 0x06000CB0 RID: 3248
		CodeExpression GetCodeExpression(DesignerDataConnection connection);
	}
}
