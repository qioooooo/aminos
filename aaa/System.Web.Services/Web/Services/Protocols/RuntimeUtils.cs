using System;
using System.Globalization;
using System.IO;
using System.Web.Services.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000056 RID: 86
	internal class RuntimeUtils
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x000097B7 File Offset: 0x000087B7
		private RuntimeUtils()
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000097C0 File Offset: 0x000087C0
		internal static XmlDeserializationEvents GetDeserializationEvents()
		{
			return new XmlDeserializationEvents
			{
				OnUnknownElement = new XmlElementEventHandler(RuntimeUtils.OnUnknownElement),
				OnUnknownAttribute = new XmlAttributeEventHandler(RuntimeUtils.OnUnknownAttribute)
			};
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000097FC File Offset: 0x000087FC
		private static void OnUnknownAttribute(object sender, XmlAttributeEventArgs e)
		{
			if (e.Attr == null)
			{
				return;
			}
			if (RuntimeUtils.IsKnownNamespace(e.Attr.NamespaceURI))
			{
				return;
			}
			Tracing.OnUnknownAttribute(sender, e);
			if (e.ExpectedAttributes == null)
			{
				throw new InvalidOperationException(Res.GetString("WebUnknownAttribute", new object[]
				{
					e.Attr.Name,
					e.Attr.Value
				}));
			}
			if (e.ExpectedAttributes.Length == 0)
			{
				throw new InvalidOperationException(Res.GetString("WebUnknownAttribute2", new object[]
				{
					e.Attr.Name,
					e.Attr.Value
				}));
			}
			throw new InvalidOperationException(Res.GetString("WebUnknownAttribute3", new object[]
			{
				e.Attr.Name,
				e.Attr.Value,
				e.ExpectedAttributes
			}));
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000098E8 File Offset: 0x000088E8
		internal static string ElementString(XmlElement element)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			stringWriter.Write("<");
			stringWriter.Write(element.Name);
			if (element.NamespaceURI != null && element.NamespaceURI.Length > 0)
			{
				stringWriter.Write(" xmlns");
				if (element.Prefix != null && element.Prefix.Length > 0)
				{
					stringWriter.Write(":");
					stringWriter.Write(element.Prefix);
				}
				stringWriter.Write("='");
				stringWriter.Write(element.NamespaceURI);
				stringWriter.Write("'");
			}
			stringWriter.Write(">..</");
			stringWriter.Write(element.Name);
			stringWriter.Write(">");
			return stringWriter.ToString();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000099B0 File Offset: 0x000089B0
		internal static void OnUnknownElement(object sender, XmlElementEventArgs e)
		{
			if (e.Element == null)
			{
				return;
			}
			string text = RuntimeUtils.ElementString(e.Element);
			Tracing.OnUnknownElement(sender, e);
			if (e.ExpectedElements == null)
			{
				throw new InvalidOperationException(Res.GetString("WebUnknownElement", new object[] { text }));
			}
			if (e.ExpectedElements.Length == 0)
			{
				throw new InvalidOperationException(Res.GetString("WebUnknownElement1", new object[] { text }));
			}
			throw new InvalidOperationException(Res.GetString("WebUnknownElement2", new object[] { text, e.ExpectedElements }));
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009A4C File Offset: 0x00008A4C
		internal static bool IsKnownNamespace(string ns)
		{
			return ns == "http://www.w3.org/2001/XMLSchema-instance" || ns == "http://www.w3.org/XML/1998/namespace" || (ns == "http://schemas.xmlsoap.org/soap/encoding/" || ns == "http://schemas.xmlsoap.org/soap/envelope/") || (ns == "http://www.w3.org/2003/05/soap-envelope" || ns == "http://www.w3.org/2003/05/soap-encoding" || ns == "http://www.w3.org/2003/05/soap-rpc");
		}
	}
}
