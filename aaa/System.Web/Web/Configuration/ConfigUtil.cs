using System;
using System.Configuration;
using System.Threading;
using System.Web.Compilation;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x020001CA RID: 458
	internal class ConfigUtil
	{
		// Token: 0x06001A00 RID: 6656 RVA: 0x0007AA5E File Offset: 0x00079A5E
		private ConfigUtil()
		{
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0007AA68 File Offset: 0x00079A68
		internal static void CheckBaseType(Type expectedBaseType, Type userBaseType, string propertyName, ConfigurationElement configElement)
		{
			if (!expectedBaseType.IsAssignableFrom(userBaseType))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_type_to_inherit_from", new object[] { userBaseType.FullName, expectedBaseType.FullName }), configElement.ElementInformation.Properties[propertyName].Source, configElement.ElementInformation.Properties[propertyName].LineNumber);
			}
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0007AAD4 File Offset: 0x00079AD4
		internal static Type GetType(string typeName, string propertyName, ConfigurationElement configElement, XmlNode node, bool checkAptcaBit, bool ignoreCase)
		{
			Type type;
			try
			{
				type = BuildManager.GetType(typeName, true, ignoreCase);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (node != null)
				{
					throw new ConfigurationErrorsException(ex.Message, ex, node);
				}
				if (configElement != null)
				{
					throw new ConfigurationErrorsException(ex.Message, ex, configElement.ElementInformation.Properties[propertyName].Source, configElement.ElementInformation.Properties[propertyName].LineNumber);
				}
				throw new ConfigurationErrorsException(ex.Message, ex);
			}
			if (checkAptcaBit)
			{
				if (node != null)
				{
					HttpRuntime.FailIfNoAPTCABit(type, node);
				}
				else
				{
					HttpRuntime.FailIfNoAPTCABit(type, (configElement != null) ? configElement.ElementInformation : null, propertyName);
				}
			}
			return type;
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0007AB98 File Offset: 0x00079B98
		internal static Type GetType(string typeName, string propertyName, ConfigurationElement configElement)
		{
			return ConfigUtil.GetType(typeName, propertyName, configElement, true);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0007ABA3 File Offset: 0x00079BA3
		internal static Type GetType(string typeName, string propertyName, ConfigurationElement configElement, bool checkAptcaBit)
		{
			return ConfigUtil.GetType(typeName, propertyName, configElement, checkAptcaBit, false);
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0007ABAF File Offset: 0x00079BAF
		internal static Type GetType(string typeName, string propertyName, ConfigurationElement configElement, bool checkAptcaBit, bool ignoreCase)
		{
			return ConfigUtil.GetType(typeName, propertyName, configElement, null, checkAptcaBit, ignoreCase);
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0007ABBD File Offset: 0x00079BBD
		internal static Type GetType(string typeName, XmlNode node)
		{
			return ConfigUtil.GetType(typeName, node, false);
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0007ABC7 File Offset: 0x00079BC7
		internal static Type GetType(string typeName, XmlNode node, bool ignoreCase)
		{
			return ConfigUtil.GetType(typeName, null, null, node, true, ignoreCase);
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0007ABD4 File Offset: 0x00079BD4
		internal static void CheckAssignableType(Type baseType, Type type, ConfigurationElement configElement, string propertyName)
		{
			if (!baseType.IsAssignableFrom(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_doesnt_inherit_from_type", new object[] { type.FullName, baseType.FullName }), configElement.ElementInformation.Properties[propertyName].Source, configElement.ElementInformation.Properties[propertyName].LineNumber);
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0007AC40 File Offset: 0x00079C40
		internal static void CheckAssignableType(Type baseType, Type baseType2, Type type, ConfigurationElement configElement, string propertyName)
		{
			if (!baseType.IsAssignableFrom(type) && !baseType2.IsAssignableFrom(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_doesnt_inherit_from_type", new object[] { type.FullName, baseType.FullName }), configElement.ElementInformation.Properties[propertyName].Source, configElement.ElementInformation.Properties[propertyName].LineNumber);
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0007ACB7 File Offset: 0x00079CB7
		internal static bool IsTypeHandlerOrFactory(Type t)
		{
			return typeof(IHttpHandler).IsAssignableFrom(t) || typeof(IHttpHandlerFactory).IsAssignableFrom(t);
		}
	}
}
