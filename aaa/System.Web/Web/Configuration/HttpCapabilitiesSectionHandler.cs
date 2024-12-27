using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x020001F4 RID: 500
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HttpCapabilitiesSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x06001B5A RID: 7002 RVA: 0x0007E9BC File Offset: 0x0007D9BC
		public object Create(object parent, object configurationContext, XmlNode section)
		{
			if (!HandlerBase.IsServerConfiguration(configurationContext))
			{
				return null;
			}
			HttpCapabilitiesSectionHandler.ParseState parseState = new HttpCapabilitiesSectionHandler.ParseState();
			parseState.SectionName = section.Name;
			parseState.Evaluator = new HttpCapabilitiesEvaluator((HttpCapabilitiesEvaluator)parent);
			int num = 0;
			if (parent != null)
			{
				num = ((HttpCapabilitiesEvaluator)parent).UserAgentCacheKeyLength;
			}
			HandlerBase.GetAndRemovePositiveIntegerAttribute(section, "userAgentCacheKeyLength", ref num);
			if (num == 0)
			{
				num = 64;
			}
			parseState.Evaluator.UserAgentCacheKeyLength = num;
			HandlerBase.CheckForUnrecognizedAttributes(section);
			ArrayList arrayList = HttpCapabilitiesSectionHandler.RuleListFromElement(parseState, section, true);
			if (arrayList.Count > 0)
			{
				parseState.RuleList.Add(new CapabilitiesSection(2, null, null, arrayList));
			}
			if (parseState.FileList.Count > 0)
			{
				parseState.IsExternalFile = true;
				HttpCapabilitiesSectionHandler.ResolveFiles(parseState, configurationContext);
			}
			parseState.Evaluator.AddRuleList(parseState.RuleList);
			return parseState.Evaluator;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0007EA88 File Offset: 0x0007DA88
		private static CapabilitiesRule RuleFromElement(HttpCapabilitiesSectionHandler.ParseState parseState, XmlNode element)
		{
			int num;
			if (element.Name == "filter")
			{
				num = 2;
			}
			else if (element.Name == "case")
			{
				num = 3;
			}
			else
			{
				if (element.Name == "use")
				{
					HandlerBase.CheckForNonCommentChildNodes(element);
					string text = HandlerBase.RemoveRequiredAttribute(element, "var");
					string text2 = HandlerBase.RemoveAttribute(element, "as");
					HandlerBase.CheckForUnrecognizedAttributes(element);
					if (text2 == null)
					{
						text2 = string.Empty;
					}
					parseState.Evaluator.AddDependency(text);
					return new CapabilitiesUse(text, text2);
				}
				throw new ConfigurationErrorsException(SR.GetString("Unknown_tag_in_caps_config", new object[] { element.Name }), element);
			}
			string text3 = HandlerBase.RemoveAttribute(element, "match");
			string text4 = HandlerBase.RemoveAttribute(element, "with");
			HandlerBase.CheckForUnrecognizedAttributes(element);
			DelayedRegex delayedRegex;
			CapabilitiesPattern capabilitiesPattern;
			if (text3 == null)
			{
				if (text4 != null)
				{
					throw new ConfigurationErrorsException(SR.GetString("Cannot_specify_test_without_match"), element);
				}
				delayedRegex = null;
				capabilitiesPattern = null;
			}
			else
			{
				try
				{
					delayedRegex = new DelayedRegex(text3);
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(ex.Message, ex, element);
				}
				if (text4 == null)
				{
					capabilitiesPattern = CapabilitiesPattern.Default;
				}
				else
				{
					capabilitiesPattern = new CapabilitiesPattern(text4);
				}
			}
			ArrayList arrayList = HttpCapabilitiesSectionHandler.RuleListFromElement(parseState, element, false);
			return new CapabilitiesSection(num, delayedRegex, capabilitiesPattern, arrayList);
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0007EBD4 File Offset: 0x0007DBD4
		private static ArrayList RuleListFromElement(HttpCapabilitiesSectionHandler.ParseState parseState, XmlNode node, bool top)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlNodeType nodeType = xmlNode.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
				{
					string name;
					if ((name = xmlNode.Name) == null)
					{
						goto IL_00DD;
					}
					if (!(name == "result"))
					{
						if (!(name == "file"))
						{
							goto IL_00DD;
						}
						if (parseState.IsExternalFile)
						{
							throw new ConfigurationErrorsException(SR.GetString("File_element_only_valid_in_config"), xmlNode);
						}
						HttpCapabilitiesSectionHandler.ProcessFile(parseState.FileList, xmlNode);
					}
					else
					{
						if (!top)
						{
							throw new ConfigurationErrorsException(SR.GetString("Result_must_be_at_the_top_browser_section"), xmlNode);
						}
						HttpCapabilitiesSectionHandler.ProcessResult(parseState.Evaluator, xmlNode);
					}
					IL_00EB:
					top = false;
					continue;
					IL_00DD:
					arrayList.Add(HttpCapabilitiesSectionHandler.RuleFromElement(parseState, xmlNode));
					goto IL_00EB;
				}
				case XmlNodeType.Attribute:
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					top = false;
					HttpCapabilitiesSectionHandler.AppendLines(arrayList, xmlNode.Value, node);
					continue;
				default:
					if (nodeType == XmlNodeType.Comment || nodeType == XmlNodeType.Whitespace)
					{
						continue;
					}
					break;
				}
				HandlerBase.ThrowUnrecognizedElement(xmlNode);
			}
			return arrayList;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0007ED0C File Offset: 0x0007DD0C
		private static void ProcessFile(ArrayList fileList, XmlNode node)
		{
			string text = null;
			XmlNode andRemoveRequiredStringAttribute = HandlerBase.GetAndRemoveRequiredStringAttribute(node, "src", ref text);
			HandlerBase.CheckForUnrecognizedAttributes(node);
			HandlerBase.CheckForNonCommentChildNodes(node);
			fileList.Add(new Pair(text, andRemoveRequiredStringAttribute));
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0007ED44 File Offset: 0x0007DD44
		private static void ProcessResult(HttpCapabilitiesEvaluator capabilitiesEvaluator, XmlNode node)
		{
			bool flag = true;
			HandlerBase.GetAndRemoveBooleanAttribute(node, "inherit", ref flag);
			if (!flag)
			{
				capabilitiesEvaluator.ClearParent();
			}
			Type type = null;
			XmlNode xmlNode = HandlerBase.GetAndRemoveTypeAttribute(node, "type", ref type);
			if (xmlNode != null && !type.Equals(capabilitiesEvaluator._resultType))
			{
				HandlerBase.CheckAssignableType(xmlNode, capabilitiesEvaluator._resultType, type);
				capabilitiesEvaluator._resultType = type;
			}
			int num = 0;
			xmlNode = HandlerBase.GetAndRemovePositiveIntegerAttribute(node, "cacheTime", ref num);
			if (xmlNode != null)
			{
				capabilitiesEvaluator.SetCacheTime(num);
			}
			HandlerBase.CheckForUnrecognizedAttributes(node);
			HandlerBase.CheckForNonCommentChildNodes(node);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0007EDC8 File Offset: 0x0007DDC8
		private static void ResolveFiles(HttpCapabilitiesSectionHandler.ParseState parseState, object configurationContext)
		{
			HttpConfigurationContext httpConfigurationContext = (HttpConfigurationContext)configurationContext;
			string text = null;
			bool flag = false;
			try
			{
				if (httpConfigurationContext.VirtualPath == null)
				{
					flag = true;
					new FileIOPermission(PermissionState.None)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Assert();
				}
				Pair pair = (Pair)parseState.FileList[0];
				XmlNode xmlNode = (XmlNode)pair.Second;
				text = Path.GetDirectoryName(ConfigurationErrorsException.GetFilename(xmlNode));
			}
			finally
			{
				if (flag)
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			foreach (object obj in parseState.FileList)
			{
				Pair pair2 = (Pair)obj;
				string text2 = (string)pair2.First;
				string text3 = Path.Combine(text, text2);
				XmlNode documentElement;
				try
				{
					if (flag)
					{
						InternalSecurityPermissions.FileReadAccess(text3).Assert();
					}
					Exception ex = null;
					try
					{
						HttpConfigurationSystem.AddFileDependency(text3);
					}
					catch (Exception ex2)
					{
						ex = ex2;
					}
					ConfigXmlDocument configXmlDocument = new ConfigXmlDocument();
					try
					{
						configXmlDocument.Load(text3);
						documentElement = configXmlDocument.DocumentElement;
					}
					catch (Exception ex3)
					{
						throw new ConfigurationErrorsException(SR.GetString("Error_loading_XML_file", new object[] { text3, ex3.Message }), ex3, (XmlNode)pair2.Second);
					}
					if (ex != null)
					{
						throw ex;
					}
				}
				finally
				{
					if (flag)
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				if (documentElement.Name != parseState.SectionName)
				{
					throw new ConfigurationErrorsException(SR.GetString("Capability_file_root_element", new object[] { parseState.SectionName }), documentElement);
				}
				HandlerBase.CheckForUnrecognizedAttributes(documentElement);
				ArrayList arrayList = HttpCapabilitiesSectionHandler.RuleListFromElement(parseState, documentElement, true);
				if (arrayList.Count > 0)
				{
					parseState.RuleList.Add(new CapabilitiesSection(2, null, null, arrayList));
				}
			}
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0007F008 File Offset: 0x0007E008
		private static void AppendLines(ArrayList setlist, string text, XmlNode node)
		{
			int num = ConfigurationErrorsException.GetLineNumber(node);
			int num2 = 0;
			Match match;
			for (;;)
			{
				if ((match = HttpCapabilitiesSectionHandler.wsRegex.Match(text, num2)).Success)
				{
					num += Util.LineCount(text, num2, match.Index + match.Length);
					num2 = match.Index + match.Length;
				}
				if (num2 == text.Length)
				{
					break;
				}
				if (!(match = HttpCapabilitiesSectionHandler.lineRegex.Match(text, num2)).Success)
				{
					goto IL_00C7;
				}
				setlist.Add(new CapabilitiesAssignment(match.Groups["var"].Value, new CapabilitiesPattern(match.Groups["pat"].Value)));
				num += Util.LineCount(text, num2, match.Index + match.Length);
				num2 = match.Index + match.Length;
			}
			return;
			IL_00C7:
			match = HttpCapabilitiesSectionHandler.errRegex.Match(text, num2);
			throw new ConfigurationErrorsException(SR.GetString("Problem_reading_caps_config", new object[] { match.ToString() }), ConfigurationErrorsException.GetFilename(node), num);
		}

		// Token: 0x0400184C RID: 6220
		private const int _defaultUserAgentCacheKeyLength = 64;

		// Token: 0x0400184D RID: 6221
		private static Regex lineRegex = new Regex("\\G(?<var>\\w+)\\s*=\\s*(?:\"(?<pat>[^\"\r\n\\\\]*(?:\\\\.[^\"\r\n\\\\]*)*)\"|(?!\")(?<pat>\\S+))\\s*");

		// Token: 0x0400184E RID: 6222
		private static Regex wsRegex = new Regex("\\G\\s*");

		// Token: 0x0400184F RID: 6223
		private static Regex errRegex = new Regex("\\G\\S {0,8}");

		// Token: 0x020001F5 RID: 501
		private class ParseState
		{
			// Token: 0x06001B62 RID: 7010 RVA: 0x0007F13F File Offset: 0x0007E13F
			internal ParseState()
			{
			}

			// Token: 0x04001850 RID: 6224
			internal string SectionName;

			// Token: 0x04001851 RID: 6225
			internal HttpCapabilitiesEvaluator Evaluator;

			// Token: 0x04001852 RID: 6226
			internal ArrayList RuleList = new ArrayList();

			// Token: 0x04001853 RID: 6227
			internal ArrayList FileList = new ArrayList();

			// Token: 0x04001854 RID: 6228
			internal bool IsExternalFile;
		}
	}
}
