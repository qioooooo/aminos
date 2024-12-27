using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200007A RID: 122
	internal static class SoapReflector
	{
		// Token: 0x0600032A RID: 810 RVA: 0x0000E6BD File Offset: 0x0000D6BD
		internal static bool ServiceDefaultIsEncoded(Type type)
		{
			return SoapReflector.ServiceDefaultIsEncoded(SoapReflector.GetSoapServiceAttribute(type));
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E6CA File Offset: 0x0000D6CA
		internal static bool ServiceDefaultIsEncoded(object soapServiceAttribute)
		{
			if (soapServiceAttribute == null)
			{
				return false;
			}
			if (soapServiceAttribute is SoapDocumentServiceAttribute)
			{
				return ((SoapDocumentServiceAttribute)soapServiceAttribute).Use == SoapBindingUse.Encoded;
			}
			return soapServiceAttribute is SoapRpcServiceAttribute && ((SoapRpcServiceAttribute)soapServiceAttribute).Use == SoapBindingUse.Encoded;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E700 File Offset: 0x0000D700
		internal static string GetEncodedNamespace(string ns, bool serviceDefaultIsEncoded)
		{
			if (serviceDefaultIsEncoded)
			{
				return ns;
			}
			if (ns.EndsWith("/", StringComparison.Ordinal))
			{
				return ns + "encodedTypes";
			}
			return ns + "/encodedTypes";
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E72C File Offset: 0x0000D72C
		internal static string GetLiteralNamespace(string ns, bool serviceDefaultIsEncoded)
		{
			if (!serviceDefaultIsEncoded)
			{
				return ns;
			}
			if (ns.EndsWith("/", StringComparison.Ordinal))
			{
				return ns + "literalTypes";
			}
			return ns + "/literalTypes";
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000E758 File Offset: 0x0000D758
		internal static SoapReflectionImporter CreateSoapImporter(string defaultNs, bool serviceDefaultIsEncoded)
		{
			return new SoapReflectionImporter(SoapReflector.GetEncodedNamespace(defaultNs, serviceDefaultIsEncoded));
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000E766 File Offset: 0x0000D766
		internal static XmlReflectionImporter CreateXmlImporter(string defaultNs, bool serviceDefaultIsEncoded)
		{
			return new XmlReflectionImporter(SoapReflector.GetLiteralNamespace(defaultNs, serviceDefaultIsEncoded));
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000E774 File Offset: 0x0000D774
		internal static void IncludeTypes(LogicalMethodInfo[] methods, SoapReflectionImporter importer)
		{
			foreach (LogicalMethodInfo logicalMethodInfo in methods)
			{
				SoapReflector.IncludeTypes(logicalMethodInfo, importer);
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000E79A File Offset: 0x0000D79A
		internal static void IncludeTypes(LogicalMethodInfo method, SoapReflectionImporter importer)
		{
			if (method.Declaration != null)
			{
				importer.IncludeTypes(method.Declaration.DeclaringType);
				importer.IncludeTypes(method.Declaration);
			}
			importer.IncludeTypes(method.DeclaringType);
			importer.IncludeTypes(method.CustomAttributeProvider);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000E7DC File Offset: 0x0000D7DC
		internal static object GetSoapMethodAttribute(LogicalMethodInfo methodInfo)
		{
			object[] customAttributes = methodInfo.GetCustomAttributes(typeof(SoapRpcMethodAttribute));
			object[] customAttributes2 = methodInfo.GetCustomAttributes(typeof(SoapDocumentMethodAttribute));
			if (customAttributes.Length > 0)
			{
				if (customAttributes2.Length > 0)
				{
					throw new ArgumentException(Res.GetString("WebBothMethodAttrs"), "methodInfo");
				}
				return customAttributes[0];
			}
			else
			{
				if (customAttributes2.Length > 0)
				{
					return customAttributes2[0];
				}
				return null;
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000E83C File Offset: 0x0000D83C
		internal static object GetSoapServiceAttribute(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(SoapRpcServiceAttribute), false);
			object[] customAttributes2 = type.GetCustomAttributes(typeof(SoapDocumentServiceAttribute), false);
			if (customAttributes.Length > 0)
			{
				if (customAttributes2.Length > 0)
				{
					throw new ArgumentException(Res.GetString("WebBothServiceAttrs"), "methodInfo");
				}
				return customAttributes[0];
			}
			else
			{
				if (customAttributes2.Length > 0)
				{
					return customAttributes2[0];
				}
				return null;
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000E89D File Offset: 0x0000D89D
		internal static SoapServiceRoutingStyle GetSoapServiceRoutingStyle(object soapServiceAttribute)
		{
			if (soapServiceAttribute is SoapRpcServiceAttribute)
			{
				return ((SoapRpcServiceAttribute)soapServiceAttribute).RoutingStyle;
			}
			if (soapServiceAttribute is SoapDocumentServiceAttribute)
			{
				return ((SoapDocumentServiceAttribute)soapServiceAttribute).RoutingStyle;
			}
			return SoapServiceRoutingStyle.SoapAction;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E8C8 File Offset: 0x0000D8C8
		internal static string GetSoapMethodBinding(LogicalMethodInfo method)
		{
			object[] array = method.GetCustomAttributes(typeof(SoapDocumentMethodAttribute));
			string text;
			if (array.Length == 0)
			{
				array = method.GetCustomAttributes(typeof(SoapRpcMethodAttribute));
				if (array.Length == 0)
				{
					text = string.Empty;
				}
				else
				{
					text = ((SoapRpcMethodAttribute)array[0]).Binding;
				}
			}
			else
			{
				text = ((SoapDocumentMethodAttribute)array[0]).Binding;
			}
			if (method.Binding == null)
			{
				return text;
			}
			if (text.Length > 0 && text != method.Binding.Name)
			{
				throw new InvalidOperationException(Res.GetString("WebInvalidBindingName", new object[]
				{
					text,
					method.Binding.Name
				}));
			}
			return method.Binding.Name;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000E984 File Offset: 0x0000D984
		internal static SoapReflectedMethod ReflectMethod(LogicalMethodInfo methodInfo, bool client, XmlReflectionImporter xmlImporter, SoapReflectionImporter soapImporter, string defaultNs)
		{
			SoapReflectedMethod soapReflectedMethod2;
			try
			{
				string key = methodInfo.GetKey();
				SoapReflectedMethod soapReflectedMethod = new SoapReflectedMethod();
				SoapReflector.MethodAttribute methodAttribute = new SoapReflector.MethodAttribute();
				object soapServiceAttribute = SoapReflector.GetSoapServiceAttribute(methodInfo.DeclaringType);
				bool flag = SoapReflector.ServiceDefaultIsEncoded(soapServiceAttribute);
				object obj = SoapReflector.GetSoapMethodAttribute(methodInfo);
				if (obj == null)
				{
					if (client)
					{
						return null;
					}
					if (soapServiceAttribute is SoapRpcServiceAttribute)
					{
						obj = new SoapRpcMethodAttribute
						{
							Use = ((SoapRpcServiceAttribute)soapServiceAttribute).Use
						};
					}
					else if (soapServiceAttribute is SoapDocumentServiceAttribute)
					{
						obj = new SoapDocumentMethodAttribute
						{
							Use = ((SoapDocumentServiceAttribute)soapServiceAttribute).Use
						};
					}
					else
					{
						obj = new SoapDocumentMethodAttribute();
					}
				}
				if (obj is SoapRpcMethodAttribute)
				{
					SoapRpcMethodAttribute soapRpcMethodAttribute = (SoapRpcMethodAttribute)obj;
					soapReflectedMethod.rpc = true;
					soapReflectedMethod.use = soapRpcMethodAttribute.Use;
					soapReflectedMethod.oneWay = soapRpcMethodAttribute.OneWay;
					methodAttribute.action = soapRpcMethodAttribute.Action;
					methodAttribute.binding = soapRpcMethodAttribute.Binding;
					methodAttribute.requestName = soapRpcMethodAttribute.RequestElementName;
					methodAttribute.requestNs = soapRpcMethodAttribute.RequestNamespace;
					methodAttribute.responseName = soapRpcMethodAttribute.ResponseElementName;
					methodAttribute.responseNs = soapRpcMethodAttribute.ResponseNamespace;
				}
				else
				{
					SoapDocumentMethodAttribute soapDocumentMethodAttribute = (SoapDocumentMethodAttribute)obj;
					soapReflectedMethod.rpc = false;
					soapReflectedMethod.use = soapDocumentMethodAttribute.Use;
					soapReflectedMethod.paramStyle = soapDocumentMethodAttribute.ParameterStyle;
					soapReflectedMethod.oneWay = soapDocumentMethodAttribute.OneWay;
					methodAttribute.action = soapDocumentMethodAttribute.Action;
					methodAttribute.binding = soapDocumentMethodAttribute.Binding;
					methodAttribute.requestName = soapDocumentMethodAttribute.RequestElementName;
					methodAttribute.requestNs = soapDocumentMethodAttribute.RequestNamespace;
					methodAttribute.responseName = soapDocumentMethodAttribute.ResponseElementName;
					methodAttribute.responseNs = soapDocumentMethodAttribute.ResponseNamespace;
					if (soapReflectedMethod.use == SoapBindingUse.Default)
					{
						if (soapServiceAttribute is SoapDocumentServiceAttribute)
						{
							soapReflectedMethod.use = ((SoapDocumentServiceAttribute)soapServiceAttribute).Use;
						}
						if (soapReflectedMethod.use == SoapBindingUse.Default)
						{
							soapReflectedMethod.use = SoapBindingUse.Literal;
						}
					}
					if (soapReflectedMethod.paramStyle == SoapParameterStyle.Default)
					{
						if (soapServiceAttribute is SoapDocumentServiceAttribute)
						{
							soapReflectedMethod.paramStyle = ((SoapDocumentServiceAttribute)soapServiceAttribute).ParameterStyle;
						}
						if (soapReflectedMethod.paramStyle == SoapParameterStyle.Default)
						{
							soapReflectedMethod.paramStyle = SoapParameterStyle.Wrapped;
						}
					}
				}
				if (methodAttribute.binding.Length > 0)
				{
					if (client)
					{
						throw new InvalidOperationException(Res.GetString("WebInvalidBindingPlacement", new object[] { obj.GetType().Name }));
					}
					soapReflectedMethod.binding = WebServiceBindingReflector.GetAttribute(methodInfo, methodAttribute.binding);
				}
				WebMethodAttribute methodAttribute2 = methodInfo.MethodAttribute;
				soapReflectedMethod.name = methodAttribute2.MessageName;
				if (soapReflectedMethod.name.Length == 0)
				{
					soapReflectedMethod.name = methodInfo.Name;
				}
				string text;
				if (soapReflectedMethod.rpc)
				{
					text = ((methodAttribute.requestName.Length == 0 || !client) ? methodInfo.Name : methodAttribute.requestName);
				}
				else
				{
					text = ((methodAttribute.requestName.Length == 0) ? soapReflectedMethod.name : methodAttribute.requestName);
				}
				string text2 = methodAttribute.requestNs;
				if (text2 == null)
				{
					if (soapReflectedMethod.binding != null && soapReflectedMethod.binding.Namespace != null && soapReflectedMethod.binding.Namespace.Length != 0)
					{
						text2 = soapReflectedMethod.binding.Namespace;
					}
					else
					{
						text2 = defaultNs;
					}
				}
				string text3;
				if (soapReflectedMethod.rpc && soapReflectedMethod.use != SoapBindingUse.Encoded)
				{
					text3 = methodInfo.Name + "Response";
				}
				else
				{
					text3 = ((methodAttribute.responseName.Length == 0) ? (soapReflectedMethod.name + "Response") : methodAttribute.responseName);
				}
				string text4 = methodAttribute.responseNs;
				if (text4 == null)
				{
					if (soapReflectedMethod.binding != null && soapReflectedMethod.binding.Namespace != null && soapReflectedMethod.binding.Namespace.Length != 0)
					{
						text4 = soapReflectedMethod.binding.Namespace;
					}
					else
					{
						text4 = defaultNs;
					}
				}
				SoapReflector.SoapParameterInfo[] array = SoapReflector.ReflectParameters(methodInfo.InParameters, text2);
				SoapReflector.SoapParameterInfo[] array2 = SoapReflector.ReflectParameters(methodInfo.OutParameters, text4);
				soapReflectedMethod.action = methodAttribute.action;
				if (soapReflectedMethod.action == null)
				{
					soapReflectedMethod.action = SoapReflector.GetDefaultAction(defaultNs, methodInfo);
				}
				soapReflectedMethod.methodInfo = methodInfo;
				if (soapReflectedMethod.oneWay)
				{
					if (array2.Length > 0)
					{
						throw new ArgumentException(Res.GetString("WebOneWayOutParameters"), "methodInfo");
					}
					if (methodInfo.ReturnType != typeof(void))
					{
						throw new ArgumentException(Res.GetString("WebOneWayReturnValue"), "methodInfo");
					}
				}
				XmlReflectionMember[] array3 = new XmlReflectionMember[array.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					SoapReflector.SoapParameterInfo soapParameterInfo = array[i];
					XmlReflectionMember xmlReflectionMember = new XmlReflectionMember();
					xmlReflectionMember.MemberName = soapParameterInfo.parameterInfo.Name;
					xmlReflectionMember.MemberType = soapParameterInfo.parameterInfo.ParameterType;
					if (xmlReflectionMember.MemberType.IsByRef)
					{
						xmlReflectionMember.MemberType = xmlReflectionMember.MemberType.GetElementType();
					}
					xmlReflectionMember.XmlAttributes = soapParameterInfo.xmlAttributes;
					xmlReflectionMember.SoapAttributes = soapParameterInfo.soapAttributes;
					array3[i] = xmlReflectionMember;
				}
				soapReflectedMethod.requestMappings = SoapReflector.ImportMembersMapping(xmlImporter, soapImporter, flag, soapReflectedMethod.rpc, soapReflectedMethod.use, soapReflectedMethod.paramStyle, text, text2, methodAttribute.requestNs == null, array3, true, false, key, client);
				if (SoapReflector.GetSoapServiceRoutingStyle(soapServiceAttribute) == SoapServiceRoutingStyle.RequestElement && soapReflectedMethod.paramStyle == SoapParameterStyle.Bare && soapReflectedMethod.requestMappings.Count != 1)
				{
					throw new ArgumentException(Res.GetString("WhenUsingAMessageStyleOfParametersAsDocument0"), "methodInfo");
				}
				string text5 = "";
				string text6 = "";
				if (soapReflectedMethod.paramStyle == SoapParameterStyle.Bare)
				{
					if (soapReflectedMethod.requestMappings.Count == 1)
					{
						text5 = soapReflectedMethod.requestMappings[0].XsdElementName;
						text6 = soapReflectedMethod.requestMappings[0].Namespace;
					}
				}
				else
				{
					text5 = soapReflectedMethod.requestMappings.XsdElementName;
					text6 = soapReflectedMethod.requestMappings.Namespace;
				}
				soapReflectedMethod.requestElementName = new XmlQualifiedName(text5, text6);
				if (!soapReflectedMethod.oneWay)
				{
					int num = array2.Length;
					int num2 = 0;
					CodeIdentifiers codeIdentifiers = null;
					if (methodInfo.ReturnType != typeof(void))
					{
						num++;
						num2 = 1;
						codeIdentifiers = new CodeIdentifiers();
					}
					array3 = new XmlReflectionMember[num];
					foreach (SoapReflector.SoapParameterInfo soapParameterInfo2 in array2)
					{
						XmlReflectionMember xmlReflectionMember2 = new XmlReflectionMember();
						xmlReflectionMember2.MemberName = soapParameterInfo2.parameterInfo.Name;
						xmlReflectionMember2.MemberType = soapParameterInfo2.parameterInfo.ParameterType;
						if (xmlReflectionMember2.MemberType.IsByRef)
						{
							xmlReflectionMember2.MemberType = xmlReflectionMember2.MemberType.GetElementType();
						}
						xmlReflectionMember2.XmlAttributes = soapParameterInfo2.xmlAttributes;
						xmlReflectionMember2.SoapAttributes = soapParameterInfo2.soapAttributes;
						array3[num2++] = xmlReflectionMember2;
						if (codeIdentifiers != null)
						{
							codeIdentifiers.Add(xmlReflectionMember2.MemberName, null);
						}
					}
					if (methodInfo.ReturnType != typeof(void))
					{
						array3[0] = new XmlReflectionMember
						{
							MemberName = codeIdentifiers.MakeUnique(soapReflectedMethod.name + "Result"),
							MemberType = methodInfo.ReturnType,
							IsReturnValue = true,
							XmlAttributes = new XmlAttributes(methodInfo.ReturnTypeCustomAttributeProvider),
							XmlAttributes = 
							{
								XmlRoot = null
							},
							SoapAttributes = new SoapAttributes(methodInfo.ReturnTypeCustomAttributeProvider)
						};
					}
					soapReflectedMethod.responseMappings = SoapReflector.ImportMembersMapping(xmlImporter, soapImporter, flag, soapReflectedMethod.rpc, soapReflectedMethod.use, soapReflectedMethod.paramStyle, text3, text4, methodAttribute.responseNs == null, array3, false, false, key + ":Response", !client);
				}
				SoapExtensionAttribute[] array4 = (SoapExtensionAttribute[])methodInfo.GetCustomAttributes(typeof(SoapExtensionAttribute));
				soapReflectedMethod.extensions = new SoapReflectedExtension[array4.Length];
				for (int k = 0; k < array4.Length; k++)
				{
					soapReflectedMethod.extensions[k] = new SoapReflectedExtension(array4[k].ExtensionType, array4[k]);
				}
				Array.Sort<SoapReflectedExtension>(soapReflectedMethod.extensions);
				SoapHeaderAttribute[] array5 = (SoapHeaderAttribute[])methodInfo.GetCustomAttributes(typeof(SoapHeaderAttribute));
				Array.Sort(array5, new SoapHeaderAttributeComparer());
				Hashtable hashtable = new Hashtable();
				soapReflectedMethod.headers = new SoapReflectedHeader[array5.Length];
				int num3 = 0;
				int num4 = soapReflectedMethod.headers.Length;
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				for (int l = 0; l < soapReflectedMethod.headers.Length; l++)
				{
					SoapHeaderAttribute soapHeaderAttribute = array5[l];
					SoapReflectedHeader soapReflectedHeader = new SoapReflectedHeader();
					Type declaringType = methodInfo.DeclaringType;
					if ((soapReflectedHeader.memberInfo = declaringType.GetField(soapHeaderAttribute.MemberName)) != null)
					{
						soapReflectedHeader.headerType = ((FieldInfo)soapReflectedHeader.memberInfo).FieldType;
					}
					else
					{
						if ((soapReflectedHeader.memberInfo = declaringType.GetProperty(soapHeaderAttribute.MemberName)) == null)
						{
							throw SoapReflector.HeaderException(soapHeaderAttribute.MemberName, methodInfo.DeclaringType, "WebHeaderMissing");
						}
						soapReflectedHeader.headerType = ((PropertyInfo)soapReflectedHeader.memberInfo).PropertyType;
					}
					if (soapReflectedHeader.headerType.IsArray)
					{
						soapReflectedHeader.headerType = soapReflectedHeader.headerType.GetElementType();
						soapReflectedHeader.repeats = true;
						if (soapReflectedHeader.headerType != typeof(SoapUnknownHeader) && soapReflectedHeader.headerType != typeof(SoapHeader))
						{
							throw SoapReflector.HeaderException(soapHeaderAttribute.MemberName, methodInfo.DeclaringType, "WebHeaderType");
						}
					}
					if (MemberHelper.IsStatic(soapReflectedHeader.memberInfo))
					{
						throw SoapReflector.HeaderException(soapHeaderAttribute.MemberName, methodInfo.DeclaringType, "WebHeaderStatic");
					}
					if (!MemberHelper.CanRead(soapReflectedHeader.memberInfo))
					{
						throw SoapReflector.HeaderException(soapHeaderAttribute.MemberName, methodInfo.DeclaringType, "WebHeaderRead");
					}
					if (!MemberHelper.CanWrite(soapReflectedHeader.memberInfo))
					{
						throw SoapReflector.HeaderException(soapHeaderAttribute.MemberName, methodInfo.DeclaringType, "WebHeaderWrite");
					}
					if (!typeof(SoapHeader).IsAssignableFrom(soapReflectedHeader.headerType))
					{
						throw SoapReflector.HeaderException(soapHeaderAttribute.MemberName, methodInfo.DeclaringType, "WebHeaderType");
					}
					SoapHeaderDirection direction = soapHeaderAttribute.Direction;
					if (soapReflectedMethod.oneWay && (direction & (SoapHeaderDirection.Out | SoapHeaderDirection.Fault)) != (SoapHeaderDirection)0)
					{
						throw SoapReflector.HeaderException(soapHeaderAttribute.MemberName, methodInfo.DeclaringType, "WebHeaderOneWayOut");
					}
					if (hashtable.Contains(soapReflectedHeader.headerType))
					{
						SoapHeaderDirection soapHeaderDirection = (SoapHeaderDirection)hashtable[soapReflectedHeader.headerType];
						if ((soapHeaderDirection & direction) != (SoapHeaderDirection)0)
						{
							throw SoapReflector.HeaderException(soapHeaderAttribute.MemberName, methodInfo.DeclaringType, "WebMultiplyDeclaredHeaderTypes");
						}
						hashtable[soapReflectedHeader.headerType] = direction | soapHeaderDirection;
					}
					else
					{
						hashtable[soapReflectedHeader.headerType] = direction;
					}
					if (soapReflectedHeader.headerType != typeof(SoapHeader) && soapReflectedHeader.headerType != typeof(SoapUnknownHeader))
					{
						XmlReflectionMember xmlReflectionMember3 = new XmlReflectionMember();
						xmlReflectionMember3.MemberName = soapReflectedHeader.headerType.Name;
						xmlReflectionMember3.MemberType = soapReflectedHeader.headerType;
						XmlAttributes xmlAttributes = new XmlAttributes(soapReflectedHeader.headerType);
						if (xmlAttributes.XmlRoot != null)
						{
							xmlReflectionMember3.XmlAttributes = new XmlAttributes();
							XmlElementAttribute xmlElementAttribute = new XmlElementAttribute();
							xmlElementAttribute.ElementName = xmlAttributes.XmlRoot.ElementName;
							xmlElementAttribute.Namespace = xmlAttributes.XmlRoot.Namespace;
							xmlReflectionMember3.XmlAttributes.XmlElements.Add(xmlElementAttribute);
						}
						xmlReflectionMember3.OverrideIsNullable = true;
						if ((direction & SoapHeaderDirection.In) != (SoapHeaderDirection)0)
						{
							arrayList.Add(xmlReflectionMember3);
						}
						if ((direction & (SoapHeaderDirection.Out | SoapHeaderDirection.Fault)) != (SoapHeaderDirection)0)
						{
							arrayList2.Add(xmlReflectionMember3);
						}
						soapReflectedHeader.custom = true;
					}
					soapReflectedHeader.direction = direction;
					if (!soapReflectedHeader.custom)
					{
						soapReflectedMethod.headers[--num4] = soapReflectedHeader;
					}
					else
					{
						soapReflectedMethod.headers[num3++] = soapReflectedHeader;
					}
				}
				soapReflectedMethod.inHeaderMappings = SoapReflector.ImportMembersMapping(xmlImporter, soapImporter, flag, false, soapReflectedMethod.use, SoapParameterStyle.Bare, text + "InHeaders", defaultNs, true, (XmlReflectionMember[])arrayList.ToArray(typeof(XmlReflectionMember)), false, true, key + ":InHeaders", client);
				if (!soapReflectedMethod.oneWay)
				{
					soapReflectedMethod.outHeaderMappings = SoapReflector.ImportMembersMapping(xmlImporter, soapImporter, flag, false, soapReflectedMethod.use, SoapParameterStyle.Bare, text3 + "OutHeaders", defaultNs, true, (XmlReflectionMember[])arrayList2.ToArray(typeof(XmlReflectionMember)), false, true, key + ":OutHeaders", !client);
				}
				soapReflectedMethod2 = soapReflectedMethod;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new InvalidOperationException(Res.GetString("WebReflectionErrorMethod", new object[]
				{
					methodInfo.DeclaringType.Name,
					methodInfo.Name
				}), ex);
			}
			catch
			{
				throw new InvalidOperationException(Res.GetString("WebReflectionErrorMethod", new object[]
				{
					methodInfo.DeclaringType.Name,
					methodInfo.Name
				}), null);
			}
			return soapReflectedMethod2;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000F67C File Offset: 0x0000E67C
		private static XmlMembersMapping ImportMembersMapping(XmlReflectionImporter xmlImporter, SoapReflectionImporter soapImporter, bool serviceDefaultIsEncoded, bool rpc, SoapBindingUse use, SoapParameterStyle paramStyle, string elementName, string elementNamespace, bool nsIsDefault, XmlReflectionMember[] members, bool validate, bool openModel, string key, bool writeAccess)
		{
			XmlMembersMapping xmlMembersMapping;
			if (use == SoapBindingUse.Encoded)
			{
				string text = ((!rpc && paramStyle != SoapParameterStyle.Bare && nsIsDefault) ? SoapReflector.GetEncodedNamespace(elementNamespace, serviceDefaultIsEncoded) : elementNamespace);
				xmlMembersMapping = soapImporter.ImportMembersMapping(elementName, text, members, rpc || paramStyle != SoapParameterStyle.Bare, rpc, validate, writeAccess ? XmlMappingAccess.Write : XmlMappingAccess.Read);
			}
			else
			{
				string text2 = (nsIsDefault ? SoapReflector.GetLiteralNamespace(elementNamespace, serviceDefaultIsEncoded) : elementNamespace);
				xmlMembersMapping = xmlImporter.ImportMembersMapping(elementName, text2, members, paramStyle != SoapParameterStyle.Bare, rpc, openModel, writeAccess ? XmlMappingAccess.Write : XmlMappingAccess.Read);
			}
			if (xmlMembersMapping != null)
			{
				xmlMembersMapping.SetKey(key);
			}
			return xmlMembersMapping;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000F70C File Offset: 0x0000E70C
		private static Exception HeaderException(string memberName, Type declaringType, string description)
		{
			return new Exception(Res.GetString(description, new object[] { declaringType.Name, memberName }));
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000F73C File Offset: 0x0000E73C
		private static SoapReflector.SoapParameterInfo[] ReflectParameters(ParameterInfo[] paramInfos, string ns)
		{
			SoapReflector.SoapParameterInfo[] array = new SoapReflector.SoapParameterInfo[paramInfos.Length];
			for (int i = 0; i < paramInfos.Length; i++)
			{
				SoapReflector.SoapParameterInfo soapParameterInfo = new SoapReflector.SoapParameterInfo();
				ParameterInfo parameterInfo = paramInfos[i];
				if (parameterInfo.ParameterType.IsArray && parameterInfo.ParameterType.GetArrayRank() > 1)
				{
					throw new InvalidOperationException(Res.GetString("WebMultiDimArray"));
				}
				soapParameterInfo.xmlAttributes = new XmlAttributes(parameterInfo);
				soapParameterInfo.soapAttributes = new SoapAttributes(parameterInfo);
				soapParameterInfo.parameterInfo = parameterInfo;
				array[i] = soapParameterInfo;
			}
			return array;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000F7BC File Offset: 0x0000E7BC
		private static string GetDefaultAction(string defaultNs, LogicalMethodInfo methodInfo)
		{
			WebMethodAttribute methodAttribute = methodInfo.MethodAttribute;
			string text = methodAttribute.MessageName;
			if (text.Length == 0)
			{
				text = methodInfo.Name;
			}
			if (defaultNs.EndsWith("/", StringComparison.Ordinal))
			{
				return defaultNs + text;
			}
			return defaultNs + "/" + text;
		}

		// Token: 0x0200007B RID: 123
		private class SoapParameterInfo
		{
			// Token: 0x0400035D RID: 861
			internal ParameterInfo parameterInfo;

			// Token: 0x0400035E RID: 862
			internal XmlAttributes xmlAttributes;

			// Token: 0x0400035F RID: 863
			internal SoapAttributes soapAttributes;
		}

		// Token: 0x0200007C RID: 124
		private class MethodAttribute
		{
			// Token: 0x04000360 RID: 864
			internal string action;

			// Token: 0x04000361 RID: 865
			internal string binding;

			// Token: 0x04000362 RID: 866
			internal string requestName;

			// Token: 0x04000363 RID: 867
			internal string requestNs;

			// Token: 0x04000364 RID: 868
			internal string responseName;

			// Token: 0x04000365 RID: 869
			internal string responseNs;
		}
	}
}
