using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000385 RID: 901
	[Obsolete("The recommended alternative is System.Web.UI.Design.WebFormsReferenceManager. The WebFormsReferenceManager contains additional functionality and allows for more extensibility. To get the WebFormsReferenceManager use the RootDesigner.ReferenceManager property from your ControlDesigner. http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface IWebFormReferenceManager
	{
		// Token: 0x06002157 RID: 8535
		Type GetObjectType(string tagPrefix, string typeName);

		// Token: 0x06002158 RID: 8536
		string GetTagPrefix(Type objectType);

		// Token: 0x06002159 RID: 8537
		string GetRegisterDirectives();
	}
}
