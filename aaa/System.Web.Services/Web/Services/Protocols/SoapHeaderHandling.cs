using System;
using System.Collections;
using System.Security.Permissions;
using System.Threading;
using System.Web.Services.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200006E RID: 110
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class SoapHeaderHandling
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000D9F0 File Offset: 0x0000C9F0
		private void OnUnknownElement(object sender, XmlElementEventArgs e)
		{
			if (Thread.CurrentThread.GetHashCode() != this.currentThread)
			{
				return;
			}
			if (e.Element == null)
			{
				return;
			}
			SoapUnknownHeader soapUnknownHeader = new SoapUnknownHeader();
			soapUnknownHeader.Element = e.Element;
			this.unknownHeaders.Add(soapUnknownHeader);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000DA38 File Offset: 0x0000CA38
		private void OnUnreferencedObject(object sender, UnreferencedObjectEventArgs e)
		{
			if (Thread.CurrentThread.GetHashCode() != this.currentThread)
			{
				return;
			}
			object unreferencedObject = e.UnreferencedObject;
			if (unreferencedObject == null)
			{
				return;
			}
			if (typeof(SoapHeader).IsAssignableFrom(unreferencedObject.GetType()))
			{
				this.unreferencedHeaders.Add((SoapHeader)unreferencedObject);
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000DA8C File Offset: 0x0000CA8C
		public string ReadHeaders(XmlReader reader, XmlSerializer serializer, SoapHeaderCollection headers, SoapHeaderMapping[] mappings, SoapHeaderDirection direction, string envelopeNS, string encodingStyle, bool checkRequiredHeaders)
		{
			string text = null;
			reader.MoveToContent();
			if (!reader.IsStartElement("Header", envelopeNS))
			{
				if (checkRequiredHeaders && mappings != null && mappings.Length > 0)
				{
					text = SoapHeaderHandling.GetHeaderElementName(mappings[0].headerType);
				}
				return text;
			}
			if (reader.IsEmptyElement)
			{
				reader.Skip();
				return text;
			}
			this.unknownHeaders = new SoapHeaderCollection();
			this.unreferencedHeaders = new SoapHeaderCollection();
			this.currentThread = Thread.CurrentThread.GetHashCode();
			this.envelopeNS = envelopeNS;
			int depth = reader.Depth;
			reader.ReadStartElement();
			reader.MoveToContent();
			XmlDeserializationEvents xmlDeserializationEvents = default(XmlDeserializationEvents);
			xmlDeserializationEvents.OnUnknownElement = new XmlElementEventHandler(this.OnUnknownElement);
			xmlDeserializationEvents.OnUnreferencedObject = new UnreferencedObjectEventHandler(this.OnUnreferencedObject);
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "ReadHeaders", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceReadHeaders"), traceMethod, new TraceMethod(serializer, "Deserialize", new object[] { reader, encodingStyle }));
			}
			object[] array = (object[])serializer.Deserialize(reader, encodingStyle, xmlDeserializationEvents);
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceReadHeaders"), traceMethod);
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					SoapHeader soapHeader = (SoapHeader)array[i];
					soapHeader.DidUnderstand = true;
					headers.Add(soapHeader);
				}
				else if (checkRequiredHeaders && text == null)
				{
					text = SoapHeaderHandling.GetHeaderElementName(mappings[i].headerType);
				}
			}
			this.currentThread = 0;
			this.envelopeNS = null;
			foreach (object obj in this.unreferencedHeaders)
			{
				SoapHeader soapHeader2 = (SoapHeader)obj;
				headers.Add(soapHeader2);
			}
			this.unreferencedHeaders = null;
			foreach (object obj2 in this.unknownHeaders)
			{
				SoapHeader soapHeader3 = (SoapHeader)obj2;
				headers.Add(soapHeader3);
			}
			this.unknownHeaders = null;
			while (depth < reader.Depth && reader.Read())
			{
			}
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				reader.Read();
			}
			return text;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DD04 File Offset: 0x0000CD04
		public static void WriteHeaders(XmlWriter writer, XmlSerializer serializer, SoapHeaderCollection headers, SoapHeaderMapping[] mappings, SoapHeaderDirection direction, bool isEncoded, string defaultNS, bool serviceDefaultIsEncoded, string envelopeNS)
		{
			if (headers.Count == 0)
			{
				return;
			}
			writer.WriteStartElement("Header", envelopeNS);
			SoapProtocolVersion soapProtocolVersion;
			string text;
			if (envelopeNS == "http://www.w3.org/2003/05/soap-envelope")
			{
				soapProtocolVersion = SoapProtocolVersion.Soap12;
				text = "http://www.w3.org/2003/05/soap-encoding";
			}
			else
			{
				soapProtocolVersion = SoapProtocolVersion.Soap11;
				text = "http://schemas.xmlsoap.org/soap/encoding/";
			}
			int num = 0;
			ArrayList arrayList = new ArrayList();
			SoapHeader[] array = new SoapHeader[mappings.Length];
			bool[] array2 = new bool[array.Length];
			for (int i = 0; i < headers.Count; i++)
			{
				SoapHeader soapHeader = headers[i];
				if (soapHeader != null)
				{
					soapHeader.version = soapProtocolVersion;
					int num2;
					if (soapHeader is SoapUnknownHeader)
					{
						arrayList.Add(soapHeader);
						num++;
					}
					else if ((num2 = SoapHeaderHandling.FindMapping(mappings, soapHeader, direction)) >= 0 && !array2[num2])
					{
						array[num2] = soapHeader;
						array2[num2] = true;
					}
					else
					{
						arrayList.Add(soapHeader);
					}
				}
			}
			int num3 = arrayList.Count - num;
			if (isEncoded && num3 > 0)
			{
				SoapHeader[] array3 = new SoapHeader[mappings.Length + num3];
				array.CopyTo(array3, 0);
				int num4 = mappings.Length;
				for (int j = 0; j < arrayList.Count; j++)
				{
					if (!(arrayList[j] is SoapUnknownHeader))
					{
						array3[num4++] = (SoapHeader)arrayList[j];
					}
				}
				array = array3;
			}
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(typeof(SoapHeaderHandling), "WriteHeaders", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceWriteHeaders"), traceMethod, new TraceMethod(serializer, "Serialize", new object[]
				{
					writer,
					array,
					null,
					isEncoded ? text : null,
					"h_"
				}));
			}
			serializer.Serialize(writer, array, null, isEncoded ? text : null, "h_");
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceWriteHeaders"), traceMethod);
			}
			foreach (object obj in arrayList)
			{
				SoapHeader soapHeader2 = (SoapHeader)obj;
				if (soapHeader2 is SoapUnknownHeader)
				{
					SoapUnknownHeader soapUnknownHeader = (SoapUnknownHeader)soapHeader2;
					if (soapUnknownHeader.Element != null)
					{
						soapUnknownHeader.Element.WriteTo(writer);
					}
				}
				else if (!isEncoded)
				{
					string literalNamespace = SoapReflector.GetLiteralNamespace(defaultNS, serviceDefaultIsEncoded);
					XmlSerializer xmlSerializer = new XmlSerializer(soapHeader2.GetType(), literalNamespace);
					if (Tracing.On)
					{
						Tracing.Enter(Tracing.TraceId("TraceWriteHeaders"), traceMethod, new TraceMethod(xmlSerializer, "Serialize", new object[] { writer, soapHeader2 }));
					}
					xmlSerializer.Serialize(writer, soapHeader2);
					if (Tracing.On)
					{
						Tracing.Exit(Tracing.TraceId("TraceWriteHeaders"), traceMethod);
					}
				}
			}
			for (int k = 0; k < headers.Count; k++)
			{
				SoapHeader soapHeader3 = headers[k];
				if (soapHeader3 != null)
				{
					soapHeader3.version = SoapProtocolVersion.Default;
				}
			}
			writer.WriteEndElement();
			writer.Flush();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000E014 File Offset: 0x0000D014
		public static void WriteUnknownHeaders(XmlWriter writer, SoapHeaderCollection headers, string envelopeNS)
		{
			bool flag = true;
			foreach (object obj in headers)
			{
				SoapHeader soapHeader = (SoapHeader)obj;
				SoapUnknownHeader soapUnknownHeader = soapHeader as SoapUnknownHeader;
				if (soapUnknownHeader != null)
				{
					if (flag)
					{
						writer.WriteStartElement("Header", envelopeNS);
						flag = false;
					}
					if (soapUnknownHeader.Element != null)
					{
						soapUnknownHeader.Element.WriteTo(writer);
					}
				}
			}
			if (!flag)
			{
				writer.WriteEndElement();
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000E0A0 File Offset: 0x0000D0A0
		public static void SetHeaderMembers(SoapHeaderCollection headers, object target, SoapHeaderMapping[] mappings, SoapHeaderDirection direction, bool client)
		{
			bool[] array = new bool[headers.Count];
			if (mappings != null)
			{
				foreach (SoapHeaderMapping soapHeaderMapping in mappings)
				{
					if ((soapHeaderMapping.direction & direction) != (SoapHeaderDirection)0)
					{
						if (soapHeaderMapping.repeats)
						{
							ArrayList arrayList = new ArrayList();
							for (int j = 0; j < headers.Count; j++)
							{
								SoapHeader soapHeader = headers[j];
								if (!array[j] && soapHeaderMapping.headerType.IsAssignableFrom(soapHeader.GetType()))
								{
									arrayList.Add(soapHeader);
									array[j] = true;
								}
							}
							MemberHelper.SetValue(soapHeaderMapping.memberInfo, target, arrayList.ToArray(soapHeaderMapping.headerType));
						}
						else
						{
							bool flag = false;
							for (int k = 0; k < headers.Count; k++)
							{
								SoapHeader soapHeader2 = headers[k];
								if (!array[k] && soapHeaderMapping.headerType.IsAssignableFrom(soapHeader2.GetType()))
								{
									if (flag)
									{
										soapHeader2.DidUnderstand = false;
									}
									else
									{
										flag = true;
										MemberHelper.SetValue(soapHeaderMapping.memberInfo, target, soapHeader2);
										array[k] = true;
									}
								}
							}
						}
					}
				}
			}
			for (int l = 0; l < array.Length; l++)
			{
				if (!array[l])
				{
					SoapHeader soapHeader3 = headers[l];
					if (soapHeader3.MustUnderstand && !soapHeader3.DidUnderstand)
					{
						throw new SoapHeaderException(Res.GetString("WebCannotUnderstandHeader", new object[] { SoapHeaderHandling.GetHeaderElementName(soapHeader3) }), new XmlQualifiedName("MustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/"));
					}
				}
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000E21C File Offset: 0x0000D21C
		public static void GetHeaderMembers(SoapHeaderCollection headers, object target, SoapHeaderMapping[] mappings, SoapHeaderDirection direction, bool client)
		{
			if (mappings == null || mappings.Length == 0)
			{
				return;
			}
			foreach (SoapHeaderMapping soapHeaderMapping in mappings)
			{
				if ((soapHeaderMapping.direction & direction) != (SoapHeaderDirection)0)
				{
					object value = MemberHelper.GetValue(soapHeaderMapping.memberInfo, target);
					if (soapHeaderMapping.repeats)
					{
						object[] array = (object[])value;
						if (array != null)
						{
							for (int j = 0; j < array.Length; j++)
							{
								if (array[j] != null)
								{
									headers.Add((SoapHeader)array[j]);
								}
							}
						}
					}
					else if (value != null)
					{
						headers.Add((SoapHeader)value);
					}
				}
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000E2A8 File Offset: 0x0000D2A8
		public static void EnsureHeadersUnderstood(SoapHeaderCollection headers)
		{
			for (int i = 0; i < headers.Count; i++)
			{
				SoapHeader soapHeader = headers[i];
				if (soapHeader.MustUnderstand && !soapHeader.DidUnderstand)
				{
					throw new SoapHeaderException(Res.GetString("WebCannotUnderstandHeader", new object[] { SoapHeaderHandling.GetHeaderElementName(soapHeader) }), new XmlQualifiedName("MustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/"));
				}
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000E310 File Offset: 0x0000D310
		private static int FindMapping(SoapHeaderMapping[] mappings, SoapHeader header, SoapHeaderDirection direction)
		{
			if (mappings == null || mappings.Length == 0)
			{
				return -1;
			}
			Type type = header.GetType();
			for (int i = 0; i < mappings.Length; i++)
			{
				SoapHeaderMapping soapHeaderMapping = mappings[i];
				if ((soapHeaderMapping.direction & direction) != (SoapHeaderDirection)0 && soapHeaderMapping.custom && soapHeaderMapping.headerType.IsAssignableFrom(type))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000E364 File Offset: 0x0000D364
		private static string GetHeaderElementName(Type headerType)
		{
			XmlReflectionImporter xmlReflectionImporter = SoapReflector.CreateXmlImporter(null, false);
			XmlTypeMapping xmlTypeMapping = xmlReflectionImporter.ImportTypeMapping(headerType);
			return xmlTypeMapping.XsdElementName;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000E387 File Offset: 0x0000D387
		private static string GetHeaderElementName(SoapHeader header)
		{
			if (header is SoapUnknownHeader)
			{
				return ((SoapUnknownHeader)header).Element.LocalName;
			}
			return SoapHeaderHandling.GetHeaderElementName(header.GetType());
		}

		// Token: 0x0400032C RID: 812
		private SoapHeaderCollection unknownHeaders;

		// Token: 0x0400032D RID: 813
		private SoapHeaderCollection unreferencedHeaders;

		// Token: 0x0400032E RID: 814
		private int currentThread;

		// Token: 0x0400032F RID: 815
		private string envelopeNS;
	}
}
