using System;
using System.Configuration;
using System.Xml;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001E8 RID: 488
	internal class CodeDomConfigurationHandler : IConfigurationSectionHandler
	{
		// Token: 0x0600100C RID: 4108 RVA: 0x000351C8 File Offset: 0x000341C8
		internal CodeDomConfigurationHandler()
		{
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x000351D0 File Offset: 0x000341D0
		public virtual object Create(object inheritedObject, object configContextObj, XmlNode node)
		{
			return CodeDomCompilationConfiguration.SectionHandler.CreateStatic(inheritedObject, node);
		}
	}
}
