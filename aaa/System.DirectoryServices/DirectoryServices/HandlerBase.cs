using System;
using System.Configuration;
using System.Xml;

namespace System.DirectoryServices
{
	// Token: 0x02000042 RID: 66
	internal class HandlerBase
	{
		// Token: 0x060001DC RID: 476 RVA: 0x00007C08 File Offset: 0x00006C08
		private HandlerBase()
		{
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007C10 File Offset: 0x00006C10
		internal static void RemoveBooleanAttribute(XmlNode node, string name, ref bool value)
		{
			value = false;
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode != null)
			{
				try
				{
					value = bool.Parse(xmlNode.Value);
				}
				catch (FormatException)
				{
					throw new ConfigurationErrorsException(Res.GetString("Invalid_boolean_attribute", new object[] { name }));
				}
			}
		}
	}
}
