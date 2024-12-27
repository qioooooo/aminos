using System;

namespace System.Web.UI
{
	// Token: 0x0200038E RID: 910
	internal abstract class TemplateControlDependencyParser : DependencyParser
	{
		// Token: 0x06002C5D RID: 11357 RVA: 0x000C6394 File Offset: 0x000C5394
		internal override void ProcessMainDirectiveAttribute(string deviceName, string name, string value)
		{
			if (name != null && name == "masterpagefile")
			{
				value = value.Trim();
				if (value.Length > 0)
				{
					base.AddDependency(VirtualPath.Create(value));
					return;
				}
			}
			else
			{
				base.ProcessMainDirectiveAttribute(deviceName, name, value);
			}
		}
	}
}
