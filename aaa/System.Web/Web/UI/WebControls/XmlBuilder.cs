using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200068C RID: 1676
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class XmlBuilder : ControlBuilder
	{
		// Token: 0x0600520E RID: 21006 RVA: 0x0014B9EA File Offset: 0x0014A9EA
		public override void AppendLiteralString(string s)
		{
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x0014B9EC File Offset: 0x0014A9EC
		public override Type GetChildControlType(string tagName, IDictionary attribs)
		{
			return null;
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x0014B9EF File Offset: 0x0014A9EF
		public override bool NeedsTagInnerText()
		{
			return true;
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x0014B9F4 File Offset: 0x0014A9F4
		public override void SetTagInnerText(string text)
		{
			if (!Util.IsWhiteSpaceString(text))
			{
				int num = Util.FirstNonWhiteSpaceIndex(text);
				string text2 = text.Substring(num);
				base.Line += Util.LineCount(text, 0, num);
				XmlDocument xmlDocument = new XmlDocument();
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.LineNumberOffset = base.Line - 1;
				xmlReaderSettings.ProhibitDtd = false;
				xmlReaderSettings.CheckCharacters = false;
				XmlReader xmlReader = XmlReader.Create(new StringReader(text2), xmlReaderSettings, string.Empty);
				try
				{
					xmlDocument.Load(xmlReader);
				}
				catch (XmlException ex)
				{
					if (ex.LineNumber >= 0)
					{
						base.Line = ex.LineNumber;
					}
					throw;
				}
				base.AppendLiteralString(text);
			}
		}
	}
}
