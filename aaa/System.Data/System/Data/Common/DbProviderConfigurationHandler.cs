using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x0200013D RID: 317
	public class DbProviderConfigurationHandler : IConfigurationSectionHandler
	{
		// Token: 0x060014DC RID: 5340 RVA: 0x0022844C File Offset: 0x0022784C
		internal static NameValueCollection CloneParent(NameValueCollection parentConfig)
		{
			if (parentConfig == null)
			{
				parentConfig = new NameValueCollection();
			}
			else
			{
				parentConfig = new NameValueCollection(parentConfig);
			}
			return parentConfig;
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00228470 File Offset: 0x00227870
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			return DbProviderConfigurationHandler.CreateStatic(parent, configContext, section);
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00228488 File Offset: 0x00227888
		internal static object CreateStatic(object parent, object configContext, XmlNode section)
		{
			object obj = parent;
			if (section != null)
			{
				obj = DbProviderConfigurationHandler.CloneParent(parent as NameValueCollection);
				bool flag = false;
				HandlerBase.CheckForUnrecognizedAttributes(section);
				foreach (object obj2 in section.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj2;
					if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
					{
						string name = xmlNode.Name;
						string text;
						if ((text = name) == null || !(text == "settings"))
						{
							throw ADP.ConfigUnrecognizedElement(xmlNode);
						}
						if (flag)
						{
							throw ADP.ConfigSectionsUnique("settings");
						}
						flag = true;
						DbProviderConfigurationHandler.DbProviderDictionarySectionHandler.CreateStatic(obj as NameValueCollection, configContext, xmlNode);
					}
				}
			}
			return obj;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00228554 File Offset: 0x00227954
		internal static string RemoveAttribute(XmlNode node, string name)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode == null)
			{
				throw ADP.ConfigRequiredAttributeMissing(name, node);
			}
			string value = xmlNode.Value;
			if (value.Length == 0)
			{
				throw ADP.ConfigRequiredAttributeEmpty(name, node);
			}
			return value;
		}

		// Token: 0x04000C4F RID: 3151
		internal const string settings = "settings";

		// Token: 0x0200013E RID: 318
		private sealed class DbProviderDictionarySectionHandler
		{
			// Token: 0x060014E0 RID: 5344 RVA: 0x00228594 File Offset: 0x00227994
			internal static NameValueCollection CreateStatic(NameValueCollection config, object context, XmlNode section)
			{
				if (section != null)
				{
					HandlerBase.CheckForUnrecognizedAttributes(section);
				}
				foreach (object obj in section.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (!HandlerBase.IsIgnorableAlsoCheckForNonElement(xmlNode))
					{
						string name;
						if ((name = xmlNode.Name) != null)
						{
							if (name == "add")
							{
								DbProviderConfigurationHandler.DbProviderDictionarySectionHandler.HandleAdd(xmlNode, config);
								continue;
							}
							if (name == "remove")
							{
								DbProviderConfigurationHandler.DbProviderDictionarySectionHandler.HandleRemove(xmlNode, config);
								continue;
							}
							if (name == "clear")
							{
								DbProviderConfigurationHandler.DbProviderDictionarySectionHandler.HandleClear(xmlNode, config);
								continue;
							}
						}
						throw ADP.ConfigUnrecognizedElement(xmlNode);
					}
				}
				return config;
			}

			// Token: 0x060014E1 RID: 5345 RVA: 0x0022865C File Offset: 0x00227A5C
			private static void HandleAdd(XmlNode child, NameValueCollection config)
			{
				HandlerBase.CheckForChildNodes(child);
				string text = DbProviderConfigurationHandler.RemoveAttribute(child, "name");
				string text2 = DbProviderConfigurationHandler.RemoveAttribute(child, "value");
				HandlerBase.CheckForUnrecognizedAttributes(child);
				config.Add(text, text2);
			}

			// Token: 0x060014E2 RID: 5346 RVA: 0x00228698 File Offset: 0x00227A98
			private static void HandleRemove(XmlNode child, NameValueCollection config)
			{
				HandlerBase.CheckForChildNodes(child);
				string text = DbProviderConfigurationHandler.RemoveAttribute(child, "name");
				HandlerBase.CheckForUnrecognizedAttributes(child);
				config.Remove(text);
			}

			// Token: 0x060014E3 RID: 5347 RVA: 0x002286C4 File Offset: 0x00227AC4
			private static void HandleClear(XmlNode child, NameValueCollection config)
			{
				HandlerBase.CheckForChildNodes(child);
				HandlerBase.CheckForUnrecognizedAttributes(child);
				config.Clear();
			}
		}
	}
}
