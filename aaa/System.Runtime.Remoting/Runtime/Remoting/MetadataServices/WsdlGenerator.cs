using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Remoting.Services;
using System.Runtime.Serialization;
using System.Text;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x020000A7 RID: 167
	internal class WsdlGenerator
	{
		// Token: 0x060004FA RID: 1274 RVA: 0x0001BD68 File Offset: 0x0001AD68
		internal WsdlGenerator(Type[] types, TextWriter output)
		{
			this._textWriter = output;
			this._queue = new Queue();
			this._name = null;
			this._namespaces = new ArrayList();
			this._dynamicAssembly = null;
			this._serviceEndpoint = null;
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] != null && types[i].BaseType != null)
				{
					this.ProcessTypeAttributes(types[i]);
					this._queue.Enqueue(types[i]);
				}
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001BDEC File Offset: 0x0001ADEC
		internal WsdlGenerator(Type[] types, SdlType sdlType, TextWriter output)
		{
			this._textWriter = output;
			this._queue = new Queue();
			this._name = null;
			this._namespaces = new ArrayList();
			this._dynamicAssembly = null;
			this._serviceEndpoint = null;
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] != null && types[i].BaseType != null)
				{
					this.ProcessTypeAttributes(types[i]);
					this._queue.Enqueue(types[i]);
				}
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001BE70 File Offset: 0x0001AE70
		internal WsdlGenerator(Type[] types, TextWriter output, Assembly assembly, string url)
			: this(types, output)
		{
			this._dynamicAssembly = assembly;
			this._serviceEndpoint = url;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001BE89 File Offset: 0x0001AE89
		internal WsdlGenerator(Type[] types, SdlType sdlType, TextWriter output, Assembly assembly, string url)
			: this(types, output)
		{
			this._dynamicAssembly = assembly;
			this._serviceEndpoint = url;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001BEA4 File Offset: 0x0001AEA4
		internal WsdlGenerator(ServiceType[] serviceTypes, SdlType sdlType, TextWriter output)
		{
			this._textWriter = output;
			this._queue = new Queue();
			this._name = null;
			this._namespaces = new ArrayList();
			this._dynamicAssembly = null;
			this._serviceEndpoint = null;
			for (int i = 0; i < serviceTypes.Length; i++)
			{
				if (serviceTypes[i] != null && serviceTypes[i].ObjectType.BaseType != null)
				{
					this.ProcessTypeAttributes(serviceTypes[i].ObjectType);
					this._queue.Enqueue(serviceTypes[i].ObjectType);
				}
				if (serviceTypes[i].Url != null)
				{
					if (this._typeToServiceEndpoint == null)
					{
						this._typeToServiceEndpoint = new Hashtable(10);
					}
					if (this._typeToServiceEndpoint.ContainsKey(serviceTypes[i].ObjectType.Name))
					{
						ArrayList arrayList = (ArrayList)this._typeToServiceEndpoint[serviceTypes[i].ObjectType.Name];
						arrayList.Add(serviceTypes[i].Url);
					}
					else
					{
						ArrayList arrayList2 = new ArrayList(10);
						arrayList2.Add(serviceTypes[i].Url);
						this._typeToServiceEndpoint[serviceTypes[i].ObjectType.Name] = arrayList2;
					}
				}
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001BFD8 File Offset: 0x0001AFD8
		internal static void QualifyName(StringBuilder sb, string ns, string name)
		{
			if (ns != null && ns.Length != 0)
			{
				sb.Append(ns);
				sb.Append('.');
			}
			sb.Append(name);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001C000 File Offset: 0x0001B000
		internal static string RefName(Type type)
		{
			string text = type.Name;
			if (!type.IsPublic && !type.IsNotPublic)
			{
				text = type.FullName;
				int num = text.LastIndexOf('.');
				if (num > 0)
				{
					text = text.Substring(num + 1);
				}
				text = text.Replace('+', '.');
			}
			return text;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001C050 File Offset: 0x0001B050
		internal void ProcessTypeAttributes(Type type)
		{
			SoapTypeAttribute soapTypeAttribute = InternalRemotingServices.GetCachedSoapAttribute(type) as SoapTypeAttribute;
			if (soapTypeAttribute != null)
			{
				SoapOption soapOption = soapTypeAttribute.SoapOptions;
				if ((soapOption &= SoapOption.Option1) == SoapOption.Option1)
				{
					this._xsdVersion = XsdVersion.V1999;
					return;
				}
				if ((soapOption & SoapOption.Option2) == SoapOption.Option2)
				{
					this._xsdVersion = XsdVersion.V2000;
					return;
				}
				this._xsdVersion = XsdVersion.V2001;
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001C09C File Offset: 0x0001B09C
		internal void Generate()
		{
			while (this._queue.Count > 0)
			{
				Type type = (Type)this._queue.Dequeue();
				this.ProcessType(type);
			}
			this.Resolve();
			this.PrintWsdl();
			this._textWriter.Flush();
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001C0E8 File Offset: 0x0001B0E8
		internal void ProcessType(Type type)
		{
			string text;
			Assembly assembly;
			bool nsandAssembly = WsdlGenerator.GetNSAndAssembly(type, out text, out assembly);
			WsdlGenerator.XMLNamespace xmlnamespace = this.LookupNamespace(text, assembly);
			if (xmlnamespace != null)
			{
				string text2 = WsdlGenerator.RefName(type);
				if (xmlnamespace.LookupSchemaType(text2) != null)
				{
					return;
				}
			}
			else
			{
				xmlnamespace = this.AddNamespace(text, assembly, nsandAssembly);
			}
			this._typeToInteropNS[type] = xmlnamespace;
			if (!type.IsArray)
			{
				WsdlGenerator.SimpleSchemaType simpleSchemaType = WsdlGenerator.SimpleSchemaType.GetSimpleSchemaType(type, xmlnamespace, false);
				if (simpleSchemaType != null)
				{
					xmlnamespace.AddSimpleSchemaType(simpleSchemaType);
					return;
				}
				bool flag = false;
				string text3 = null;
				Hashtable hashtable = null;
				if (this._name == null && WsdlGenerator.s_marshalByRefType.IsAssignableFrom(type))
				{
					this._name = type.Name;
					this._targetNS = xmlnamespace.Namespace;
					this._targetNSPrefix = xmlnamespace.Prefix;
					text3 = this._serviceEndpoint;
					hashtable = this._typeToServiceEndpoint;
					flag = true;
				}
				WsdlGenerator.RealSchemaType realSchemaType = new WsdlGenerator.RealSchemaType(type, xmlnamespace, text3, hashtable, flag, this);
				xmlnamespace.AddRealSchemaType(realSchemaType);
				this.EnqueueReachableTypes(realSchemaType);
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001C1D0 File Offset: 0x0001B1D0
		private void EnqueueReachableTypes(WsdlGenerator.RealSchemaType rsType)
		{
			WsdlGenerator.XMLNamespace xns = rsType.XNS;
			if (rsType.Type.BaseType != null && (rsType.Type.BaseType != WsdlGenerator.s_valueType || rsType.Type.BaseType != WsdlGenerator.s_objectType))
			{
				this.AddType(rsType.Type.BaseType, this.GetNamespace(rsType.Type.BaseType));
			}
			bool flag = rsType.Type.IsInterface || WsdlGenerator.s_marshalByRefType.IsAssignableFrom(rsType.Type) || WsdlGenerator.s_delegateType.IsAssignableFrom(rsType.Type);
			if (flag)
			{
				FieldInfo[] instanceFields = rsType.GetInstanceFields();
				for (int i = 0; i < instanceFields.Length; i++)
				{
					if (instanceFields[i].FieldType != null)
					{
						this.AddType(instanceFields[i].FieldType, xns);
					}
				}
				Type[] introducedInterfaces = rsType.GetIntroducedInterfaces();
				if (introducedInterfaces.Length > 0)
				{
					for (int j = 0; j < introducedInterfaces.Length; j++)
					{
						this.AddType(introducedInterfaces[j], xns);
					}
				}
				this.ProcessMethods(rsType);
				return;
			}
			FieldInfo[] instanceFields2 = rsType.GetInstanceFields();
			for (int k = 0; k < instanceFields2.Length; k++)
			{
				if (instanceFields2[k].FieldType != null)
				{
					this.AddType(instanceFields2[k].FieldType, xns);
				}
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001C30C File Offset: 0x0001B30C
		private void ProcessMethods(WsdlGenerator.RealSchemaType rsType)
		{
			WsdlGenerator.XMLNamespace xns = rsType.XNS;
			MethodInfo[] introducedMethods = rsType.GetIntroducedMethods();
			if (introducedMethods.Length > 0)
			{
				WsdlGenerator.XMLNamespace xmlnamespace;
				if (xns.IsInteropType)
				{
					string text = xns.Name;
					xmlnamespace = xns;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					WsdlGenerator.QualifyName(stringBuilder, xns.Name, rsType.Name);
					string text = stringBuilder.ToString();
					xmlnamespace = this.AddNamespace(text, xns.Assem);
					xns.DependsOnSchemaNS(xmlnamespace, false);
				}
				foreach (MethodInfo methodInfo in introducedMethods)
				{
					this.AddType(methodInfo.ReturnType, xmlnamespace);
					ParameterInfo[] parameters = methodInfo.GetParameters();
					for (int j = 0; j < parameters.Length; j++)
					{
						this.AddType(parameters[j].ParameterType, xmlnamespace);
					}
				}
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001C3D4 File Offset: 0x0001B3D4
		private void AddType(Type type, WsdlGenerator.XMLNamespace xns)
		{
			Type type2 = type.GetElementType();
			Type type3 = type2;
			while (type3 != null)
			{
				type3 = type2.GetElementType();
				if (type3 != null)
				{
					type2 = type3;
				}
			}
			if (type2 != null)
			{
				this.EnqueueType(type2, xns);
			}
			if (!type.IsArray && !type.IsByRef)
			{
				this.EnqueueType(type, xns);
			}
			if (!type.IsPublic && !type.IsNotPublic)
			{
				string fullName = type.FullName;
				int num = fullName.IndexOf("+");
				if (num > 0)
				{
					string text = fullName.Substring(0, num);
					Assembly assembly = type.Module.Assembly;
					Type type4 = assembly.GetType(text, true);
					this.EnqueueType(type4, xns);
				}
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001C474 File Offset: 0x0001B474
		private void EnqueueType(Type type, WsdlGenerator.XMLNamespace xns)
		{
			if (!type.IsPrimitive || type == WsdlGenerator.s_charType)
			{
				string text;
				Assembly assembly;
				bool nsandAssembly = WsdlGenerator.GetNSAndAssembly(type, out text, out assembly);
				WsdlGenerator.XMLNamespace xmlnamespace = this.LookupNamespace(text, assembly);
				if (xmlnamespace == null)
				{
					xmlnamespace = this.AddNamespace(text, assembly, nsandAssembly);
				}
				string text2 = SudsConverter.MapClrTypeToXsdType(type);
				if (type.IsInterface || text2 != null || type == WsdlGenerator.s_voidType)
				{
					xns.DependsOnSchemaNS(xmlnamespace, false);
				}
				else
				{
					xns.DependsOnSchemaNS(xmlnamespace, true);
				}
				if (!type.FullName.StartsWith("System."))
				{
					this._queue.Enqueue(type);
				}
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001C500 File Offset: 0x0001B500
		private static bool GetNSAndAssembly(Type type, out string ns, out Assembly assem)
		{
			string text = null;
			string text2 = null;
			SoapServices.GetXmlElementForInteropType(type, out text2, out text);
			bool flag;
			if (text != null)
			{
				ns = text;
				assem = type.Module.Assembly;
				flag = true;
			}
			else
			{
				ns = type.Namespace;
				assem = type.Module.Assembly;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001C550 File Offset: 0x0001B550
		private WsdlGenerator.XMLNamespace LookupNamespace(string name, Assembly assem)
		{
			for (int i = 0; i < this._namespaces.Count; i++)
			{
				WsdlGenerator.XMLNamespace xmlnamespace = (WsdlGenerator.XMLNamespace)this._namespaces[i];
				if (name == xmlnamespace.Name)
				{
					return xmlnamespace;
				}
			}
			return null;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001C596 File Offset: 0x0001B596
		private WsdlGenerator.XMLNamespace AddNamespace(string name, Assembly assem)
		{
			return this.AddNamespace(name, assem, false);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001C5A4 File Offset: 0x0001B5A4
		private WsdlGenerator.XMLNamespace AddNamespace(string name, Assembly assem, bool bInteropType)
		{
			WsdlGenerator.XMLNamespace xmlnamespace = new WsdlGenerator.XMLNamespace(name, assem, this._serviceEndpoint, this._typeToServiceEndpoint, "ns" + this._namespaces.Count, bInteropType, this);
			this._namespaces.Add(xmlnamespace);
			return xmlnamespace;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001C5F0 File Offset: 0x0001B5F0
		private WsdlGenerator.XMLNamespace GetNamespace(Type type)
		{
			string text = null;
			Assembly assembly = null;
			bool nsandAssembly = WsdlGenerator.GetNSAndAssembly(type, out text, out assembly);
			WsdlGenerator.XMLNamespace xmlnamespace = this.LookupNamespace(text, assembly);
			if (xmlnamespace == null)
			{
				xmlnamespace = this.AddNamespace(text, assembly, nsandAssembly);
			}
			return xmlnamespace;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001C624 File Offset: 0x0001B624
		private void Resolve()
		{
			for (int i = 0; i < this._namespaces.Count; i++)
			{
				((WsdlGenerator.XMLNamespace)this._namespaces[i]).Resolve();
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001C660 File Offset: 0x0001B660
		private void PrintWsdl()
		{
			if (this._targetNS == null || this._targetNS.Length == 0)
			{
				if (this._namespaces.Count > 0)
				{
					this._targetNS = ((WsdlGenerator.XMLNamespace)this._namespaces[0]).Namespace;
				}
				else
				{
					this._targetNS = "http://schemas.xmlsoap.org/wsdl/";
				}
			}
			string text = "";
			string text2 = WsdlGenerator.IndentP(text);
			string text3 = WsdlGenerator.IndentP(text2);
			string text4 = WsdlGenerator.IndentP(text3);
			WsdlGenerator.IndentP(text4);
			StringBuilder stringBuilder = new StringBuilder(256);
			this._textWriter.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");
			stringBuilder.Length = 0;
			stringBuilder.Append("<definitions ");
			if (this._name != null)
			{
				stringBuilder.Append("name='");
				stringBuilder.Append(this._name);
				stringBuilder.Append("' ");
			}
			stringBuilder.Append("targetNamespace='");
			stringBuilder.Append(this._targetNS);
			stringBuilder.Append("'");
			this._textWriter.WriteLine(stringBuilder);
			this.PrintWsdlNamespaces(this._textWriter, stringBuilder, text4);
			bool flag = false;
			for (int i = 0; i < this._namespaces.Count; i++)
			{
				if (((WsdlGenerator.XMLNamespace)this._namespaces[i]).CheckForSchemaContent())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.PrintTypesBeginWsdl(this._textWriter, stringBuilder, text2);
				for (int j = 0; j < this._namespaces.Count; j++)
				{
					if (((WsdlGenerator.XMLNamespace)this._namespaces[j]).CheckForSchemaContent())
					{
						((WsdlGenerator.XMLNamespace)this._namespaces[j]).PrintSchemaWsdl(this._textWriter, stringBuilder, text3);
					}
				}
				this.PrintTypesEndWsdl(this._textWriter, stringBuilder, text2);
			}
			ArrayList arrayList = new ArrayList(25);
			for (int k = 0; k < this._namespaces.Count; k++)
			{
				((WsdlGenerator.XMLNamespace)this._namespaces[k]).PrintMessageWsdl(this._textWriter, stringBuilder, text2, arrayList);
			}
			this.PrintServiceWsdl(this._textWriter, stringBuilder, text2, arrayList);
			this._textWriter.WriteLine("</definitions>");
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001C898 File Offset: 0x0001B898
		private void PrintWsdlNamespaces(TextWriter textWriter, StringBuilder sb, string indent)
		{
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("xmlns='http://schemas.xmlsoap.org/wsdl/'");
			textWriter.WriteLine(sb);
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("xmlns:tns='");
			sb.Append(this._targetNS);
			sb.Append("'");
			textWriter.WriteLine(sb);
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("xmlns:xsd='");
			sb.Append(SudsConverter.GetXsdVersion(this._xsdVersion));
			sb.Append("'");
			textWriter.WriteLine(sb);
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("xmlns:xsi='");
			sb.Append(SudsConverter.GetXsiVersion(this._xsdVersion));
			sb.Append("'");
			textWriter.WriteLine(sb);
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("xmlns:suds='http://www.w3.org/2000/wsdl/suds'");
			textWriter.WriteLine(sb);
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("xmlns:wsdl='http://schemas.xmlsoap.org/wsdl/'");
			textWriter.WriteLine(sb);
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("xmlns:soapenc='http://schemas.xmlsoap.org/soap/encoding/'");
			textWriter.WriteLine(sb);
			Hashtable hashtable = new Hashtable(10);
			for (int i = 0; i < this._namespaces.Count; i++)
			{
				((WsdlGenerator.XMLNamespace)this._namespaces[i]).PrintDependsOnWsdl(this._textWriter, sb, indent, hashtable);
			}
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("xmlns:soap='http://schemas.xmlsoap.org/wsdl/soap/'>");
			textWriter.WriteLine(sb);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001CA47 File Offset: 0x0001BA47
		private void PrintTypesBeginWsdl(TextWriter textWriter, StringBuilder sb, string indent)
		{
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("<types>");
			textWriter.WriteLine(sb);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001CA6B File Offset: 0x0001BA6B
		private void PrintTypesEndWsdl(TextWriter textWriter, StringBuilder sb, string indent)
		{
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("</types>");
			textWriter.WriteLine(sb);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001CA90 File Offset: 0x0001BA90
		internal void PrintServiceWsdl(TextWriter textWriter, StringBuilder sb, string indent, ArrayList refNames)
		{
			string text = WsdlGenerator.IndentP(indent);
			string text2 = WsdlGenerator.IndentP(text);
			WsdlGenerator.IndentP(text2);
			sb.Length = 0;
			sb.Append("\n");
			sb.Append(indent);
			sb.Append("<service name='");
			sb.Append(this._name);
			sb.Append("Service'");
			sb.Append(">");
			textWriter.WriteLine(sb);
			for (int i = 0; i < refNames.Count; i++)
			{
				if ((this._typeToServiceEndpoint != null && this._typeToServiceEndpoint.ContainsKey(refNames[i])) || this._serviceEndpoint != null)
				{
					sb.Length = 0;
					sb.Append(text);
					sb.Append("<port name='");
					sb.Append(refNames[i]);
					sb.Append("Port'");
					sb.Append(" ");
					sb.Append("binding='tns:");
					sb.Append(refNames[i]);
					sb.Append("Binding");
					sb.Append("'>");
					textWriter.WriteLine(sb);
					if (this._typeToServiceEndpoint != null && this._typeToServiceEndpoint.ContainsKey(refNames[i]))
					{
						using (IEnumerator enumerator = ((ArrayList)this._typeToServiceEndpoint[refNames[i]]).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								string text3 = (string)obj;
								sb.Length = 0;
								sb.Append(text2);
								sb.Append("<soap:address location='");
								sb.Append(this.UrlEncode(text3));
								sb.Append("'/>");
								textWriter.WriteLine(sb);
							}
							goto IL_0203;
						}
						goto IL_01C0;
					}
					goto IL_01C0;
					IL_0203:
					sb.Length = 0;
					sb.Append(text);
					sb.Append("</port>");
					textWriter.WriteLine(sb);
					goto IL_0225;
					IL_01C0:
					if (this._serviceEndpoint != null)
					{
						sb.Length = 0;
						sb.Append(text2);
						sb.Append("<soap:address location='");
						sb.Append(this._serviceEndpoint);
						sb.Append("'/>");
						textWriter.WriteLine(sb);
						goto IL_0203;
					}
					goto IL_0203;
				}
				IL_0225:;
			}
			sb.Length = 0;
			sb.Append(indent);
			sb.Append("</service>");
			textWriter.WriteLine(sb);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001CD08 File Offset: 0x0001BD08
		private string UrlEncode(string url)
		{
			if (url == null || url.Length == 0)
			{
				return url;
			}
			int num = url.IndexOf("&amp;");
			if (num > -1)
			{
				return url;
			}
			num = url.IndexOf('&');
			if (num > -1)
			{
				return url.Replace("&", "&amp;");
			}
			return url;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001CD53 File Offset: 0x0001BD53
		internal static string IndentP(string indentStr)
		{
			return indentStr + "    ";
		}

		// Token: 0x0400044B RID: 1099
		private TextWriter _textWriter;

		// Token: 0x0400044C RID: 1100
		internal Queue _queue;

		// Token: 0x0400044D RID: 1101
		private string _name;

		// Token: 0x0400044E RID: 1102
		private string _targetNS;

		// Token: 0x0400044F RID: 1103
		private string _targetNSPrefix;

		// Token: 0x04000450 RID: 1104
		private ArrayList _namespaces;

		// Token: 0x04000451 RID: 1105
		private Assembly _dynamicAssembly;

		// Token: 0x04000452 RID: 1106
		private string _serviceEndpoint;

		// Token: 0x04000453 RID: 1107
		private XsdVersion _xsdVersion;

		// Token: 0x04000454 RID: 1108
		internal Hashtable _typeToServiceEndpoint;

		// Token: 0x04000455 RID: 1109
		internal Hashtable _typeToInteropNS = new Hashtable();

		// Token: 0x04000456 RID: 1110
		private static Type s_marshalByRefType = typeof(MarshalByRefObject);

		// Token: 0x04000457 RID: 1111
		private static Type s_contextBoundType = typeof(ContextBoundObject);

		// Token: 0x04000458 RID: 1112
		private static Type s_delegateType = typeof(Delegate);

		// Token: 0x04000459 RID: 1113
		private static Type s_valueType = typeof(ValueType);

		// Token: 0x0400045A RID: 1114
		private static Type s_objectType = typeof(object);

		// Token: 0x0400045B RID: 1115
		private static Type s_charType = typeof(char);

		// Token: 0x0400045C RID: 1116
		private static Type s_voidType = typeof(void);

		// Token: 0x0400045D RID: 1117
		private static Type s_remotingClientProxyType = typeof(RemotingClientProxy);

		// Token: 0x0400045E RID: 1118
		private static SchemaBlockType blockDefault = SchemaBlockType.SEQUENCE;

		// Token: 0x020000A8 RID: 168
		private interface IAbstractElement
		{
			// Token: 0x06000516 RID: 1302
			void Print(TextWriter textWriter, StringBuilder sb, string indent);
		}

		// Token: 0x020000A9 RID: 169
		private class EnumElement : WsdlGenerator.IAbstractElement
		{
			// Token: 0x06000517 RID: 1303 RVA: 0x0001CDEB File Offset: 0x0001BDEB
			internal EnumElement(string value)
			{
				this._value = value;
			}

			// Token: 0x06000518 RID: 1304 RVA: 0x0001CDFA File Offset: 0x0001BDFA
			public void Print(TextWriter textWriter, StringBuilder sb, string indent)
			{
				sb.Length = 0;
				sb.Append(indent);
				sb.Append("<enumeration value='");
				sb.Append(this._value);
				sb.Append("'/>");
				textWriter.WriteLine(sb);
			}

			// Token: 0x0400045F RID: 1119
			private string _value;
		}

		// Token: 0x020000AA RID: 170
		private abstract class Particle : WsdlGenerator.IAbstractElement
		{
			// Token: 0x0600051A RID: 1306
			public abstract string Name();

			// Token: 0x0600051B RID: 1307
			public abstract void Print(TextWriter textWriter, StringBuilder sb, string indent);
		}

		// Token: 0x020000AB RID: 171
		private class Restriction : WsdlGenerator.Particle
		{
			// Token: 0x0600051C RID: 1308 RVA: 0x0001CE3F File Offset: 0x0001BE3F
			internal Restriction()
			{
			}

			// Token: 0x0600051D RID: 1309 RVA: 0x0001CE52 File Offset: 0x0001BE52
			internal Restriction(string baseName, WsdlGenerator.XMLNamespace baseNS)
			{
				this._baseName = baseName;
				this._baseNS = baseNS;
			}

			// Token: 0x0600051E RID: 1310 RVA: 0x0001CE73 File Offset: 0x0001BE73
			internal void AddArray(WsdlGenerator.SchemaAttribute attribute)
			{
				this._rtype = WsdlGenerator.Restriction.RestrictionType.Array;
				this._attribute = attribute;
			}

			// Token: 0x0600051F RID: 1311 RVA: 0x0001CE83 File Offset: 0x0001BE83
			public override string Name()
			{
				return this._baseName;
			}

			// Token: 0x06000520 RID: 1312 RVA: 0x0001CE8C File Offset: 0x0001BE8C
			public override void Print(TextWriter textWriter, StringBuilder sb, string indent)
			{
				string text = WsdlGenerator.IndentP(indent);
				sb.Length = 0;
				sb.Append(indent);
				if (this._rtype == WsdlGenerator.Restriction.RestrictionType.Array)
				{
					sb.Append("<restriction base='soapenc:Array'>");
				}
				else if (this._rtype == WsdlGenerator.Restriction.RestrictionType.Enum)
				{
					sb.Append("<restriction base='xsd:string'>");
				}
				else
				{
					sb.Append("<restriction base='");
					sb.Append(this._baseNS.Prefix);
					sb.Append(':');
					sb.Append(this._baseName);
					sb.Append("'>");
				}
				textWriter.WriteLine(sb);
				foreach (object obj in this._abstractElms)
				{
					WsdlGenerator.IAbstractElement abstractElement = (WsdlGenerator.IAbstractElement)obj;
					abstractElement.Print(textWriter, sb, WsdlGenerator.IndentP(text));
				}
				if (this._attribute != null)
				{
					this._attribute.Print(textWriter, sb, WsdlGenerator.IndentP(text));
				}
				sb.Length = 0;
				sb.Append(indent);
				sb.Append("</restriction>");
				textWriter.WriteLine(sb);
			}

			// Token: 0x04000460 RID: 1120
			private string _baseName;

			// Token: 0x04000461 RID: 1121
			private WsdlGenerator.XMLNamespace _baseNS;

			// Token: 0x04000462 RID: 1122
			internal WsdlGenerator.Restriction.RestrictionType _rtype;

			// Token: 0x04000463 RID: 1123
			private WsdlGenerator.SchemaAttribute _attribute;

			// Token: 0x04000464 RID: 1124
			internal ArrayList _abstractElms = new ArrayList();

			// Token: 0x020000AC RID: 172
			internal enum RestrictionType
			{
				// Token: 0x04000466 RID: 1126
				None,
				// Token: 0x04000467 RID: 1127
				Array,
				// Token: 0x04000468 RID: 1128
				Enum
			}
		}

		// Token: 0x020000AD RID: 173
		private class SchemaAttribute : WsdlGenerator.IAbstractElement
		{
			// Token: 0x06000521 RID: 1313 RVA: 0x0001CFB4 File Offset: 0x0001BFB4
			internal SchemaAttribute()
			{
			}

			// Token: 0x06000522 RID: 1314 RVA: 0x0001CFBC File Offset: 0x0001BFBC
			internal void AddArray(string wireQname)
			{
				this._wireQname = wireQname;
			}

			// Token: 0x06000523 RID: 1315 RVA: 0x0001CFC8 File Offset: 0x0001BFC8
			public void Print(TextWriter textWriter, StringBuilder sb, string indent)
			{
				sb.Length = 0;
				sb.Append(indent);
				sb.Append("<attribute ref='soapenc:arrayType'");
				sb.Append(" wsdl:arrayType ='");
				sb.Append(this._wireQname);
				sb.Append("'/>");
				textWriter.WriteLine(sb);
			}

			// Token: 0x04000469 RID: 1129
			private string _wireQname;
		}

		// Token: 0x020000AE RID: 174
		private class SchemaElement : WsdlGenerator.Particle
		{
			// Token: 0x06000524 RID: 1316 RVA: 0x0001D01C File Offset: 0x0001C01C
			internal SchemaElement(string name, Type type, bool bEmbedded, WsdlGenerator.XMLNamespace xns)
			{
				this._name = name;
				this._typeString = null;
				this._schemaType = WsdlGenerator.SimpleSchemaType.GetSimpleSchemaType(type, xns, true);
				this._typeString = WsdlGenerator.RealSchemaType.TypeName(type, bEmbedded, xns);
			}

			// Token: 0x06000525 RID: 1317 RVA: 0x0001D050 File Offset: 0x0001C050
			public override string Name()
			{
				return this._name;
			}

			// Token: 0x06000526 RID: 1318 RVA: 0x0001D058 File Offset: 0x0001C058
			public override void Print(TextWriter textWriter, StringBuilder sb, string indent)
			{
				string text = WsdlGenerator.IndentP(indent);
				sb.Length = 0;
				sb.Append(indent);
				sb.Append("<element name='");
				sb.Append(this._name);
				if (this._schemaType != null && (!(this._schemaType is WsdlGenerator.SimpleSchemaType) || !((WsdlGenerator.SimpleSchemaType)this._schemaType).Type.IsEnum))
				{
					sb.Append("'>");
					textWriter.WriteLine(sb);
					this._schemaType.PrintSchemaType(textWriter, sb, WsdlGenerator.IndentP(text), true);
					sb.Length = 0;
					sb.Append(indent);
					sb.Append("</element>");
				}
				else
				{
					if (this._typeString != null)
					{
						sb.Append("' type='");
						sb.Append(this._typeString);
						sb.Append('\'');
					}
					sb.Append("/>");
				}
				textWriter.WriteLine(sb);
			}

			// Token: 0x0400046A RID: 1130
			private string _name;

			// Token: 0x0400046B RID: 1131
			private string _typeString;

			// Token: 0x0400046C RID: 1132
			private WsdlGenerator.SchemaType _schemaType;
		}

		// Token: 0x020000AF RID: 175
		private abstract class SchemaType
		{
			// Token: 0x06000527 RID: 1319
			internal abstract void PrintSchemaType(TextWriter textWriter, StringBuilder sb, string indent, bool bAnonymous);
		}

		// Token: 0x020000B0 RID: 176
		private class SimpleSchemaType : WsdlGenerator.SchemaType
		{
			// Token: 0x06000529 RID: 1321 RVA: 0x0001D149 File Offset: 0x0001C149
			private SimpleSchemaType(Type type, WsdlGenerator.XMLNamespace xns)
			{
				this._type = type;
				this._xns = xns;
				this._abstractElms = new ArrayList();
				this._fullRefName = WsdlGenerator.RefName(type);
			}

			// Token: 0x17000127 RID: 295
			// (get) Token: 0x0600052A RID: 1322 RVA: 0x0001D181 File Offset: 0x0001C181
			internal Type Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x17000128 RID: 296
			// (get) Token: 0x0600052B RID: 1323 RVA: 0x0001D189 File Offset: 0x0001C189
			internal string FullRefName
			{
				get
				{
					return this._fullRefName;
				}
			}

			// Token: 0x17000129 RID: 297
			// (get) Token: 0x0600052C RID: 1324 RVA: 0x0001D191 File Offset: 0x0001C191
			internal string BaseName
			{
				get
				{
					return this._baseName;
				}
			}

			// Token: 0x0600052D RID: 1325 RVA: 0x0001D19C File Offset: 0x0001C19C
			internal override void PrintSchemaType(TextWriter textWriter, StringBuilder sb, string indent, bool bAnonymous)
			{
				sb.Length = 0;
				sb.Append(indent);
				if (!bAnonymous)
				{
					sb.Append("<simpleType name='");
					sb.Append(this.FullRefName);
					sb.Append("'");
					if (this.BaseName != null)
					{
						sb.Append(" base='");
						sb.Append(this.BaseName);
						sb.Append("'");
					}
					if (this._restriction._rtype == WsdlGenerator.Restriction.RestrictionType.Enum)
					{
						sb.Append(" suds:enumType='");
						sb.Append(this._restriction.Name());
						sb.Append("'");
					}
				}
				else if (this.BaseName != null)
				{
					sb.Append("<simpleType base='");
					sb.Append(this.BaseName);
					sb.Append("'");
				}
				else
				{
					sb.Append("<simpleType");
				}
				bool flag = this._abstractElms.Count == 0 && this._restriction == null;
				if (flag)
				{
					sb.Append("/>");
				}
				else
				{
					sb.Append(">");
				}
				textWriter.WriteLine(sb);
				if (flag)
				{
					return;
				}
				if (this._abstractElms.Count > 0)
				{
					for (int i = 0; i < this._abstractElms.Count; i++)
					{
						((WsdlGenerator.IAbstractElement)this._abstractElms[i]).Print(textWriter, sb, WsdlGenerator.IndentP(indent));
					}
				}
				if (this._restriction != null)
				{
					this._restriction.Print(textWriter, sb, WsdlGenerator.IndentP(indent));
				}
				textWriter.Write(indent);
				textWriter.WriteLine("</simpleType>");
			}

			// Token: 0x0600052E RID: 1326 RVA: 0x0001D338 File Offset: 0x0001C338
			internal static WsdlGenerator.SimpleSchemaType GetSimpleSchemaType(Type type, WsdlGenerator.XMLNamespace xns, bool fInline)
			{
				WsdlGenerator.SimpleSchemaType simpleSchemaType = null;
				if (type.IsEnum)
				{
					simpleSchemaType = new WsdlGenerator.SimpleSchemaType(type, xns);
					string text = WsdlGenerator.RealSchemaType.TypeName(Enum.GetUnderlyingType(type), true, xns);
					simpleSchemaType._restriction = new WsdlGenerator.Restriction(text, xns);
					string[] names = Enum.GetNames(type);
					for (int i = 0; i < names.Length; i++)
					{
						simpleSchemaType._restriction._abstractElms.Add(new WsdlGenerator.EnumElement(names[i]));
					}
					simpleSchemaType._restriction._rtype = WsdlGenerator.Restriction.RestrictionType.Enum;
				}
				return simpleSchemaType;
			}

			// Token: 0x0400046D RID: 1133
			private Type _type;

			// Token: 0x0400046E RID: 1134
			internal string _baseName;

			// Token: 0x0400046F RID: 1135
			private WsdlGenerator.XMLNamespace _xns;

			// Token: 0x04000470 RID: 1136
			internal WsdlGenerator.Restriction _restriction;

			// Token: 0x04000471 RID: 1137
			private string _fullRefName;

			// Token: 0x04000472 RID: 1138
			private ArrayList _abstractElms = new ArrayList();
		}

		// Token: 0x020000B1 RID: 177
		private abstract class ComplexSchemaType : WsdlGenerator.SchemaType
		{
			// Token: 0x0600052F RID: 1327 RVA: 0x0001D3B0 File Offset: 0x0001C3B0
			internal ComplexSchemaType(string name, bool bSealed)
			{
				this._name = name;
				this._fullRefName = this._name;
				this._blockType = SchemaBlockType.ALL;
				this._baseName = null;
				this._elementName = name;
				this._bSealed = bSealed;
				this._particles = new ArrayList();
				this._abstractElms = new ArrayList();
			}

			// Token: 0x06000530 RID: 1328 RVA: 0x0001D408 File Offset: 0x0001C408
			internal ComplexSchemaType(string name, SchemaBlockType blockType, bool bSealed)
			{
				this._name = name;
				this._fullRefName = this._name;
				this._blockType = blockType;
				this._baseName = null;
				this._elementName = name;
				this._bSealed = bSealed;
				this._particles = new ArrayList();
				this._abstractElms = new ArrayList();
			}

			// Token: 0x06000531 RID: 1329 RVA: 0x0001D460 File Offset: 0x0001C460
			internal ComplexSchemaType(Type type)
			{
				this._blockType = SchemaBlockType.ALL;
				this._type = type;
				this.Init();
			}

			// Token: 0x06000532 RID: 1330 RVA: 0x0001D47C File Offset: 0x0001C47C
			private void Init()
			{
				this._name = this._type.Name;
				this._bSealed = this._type.IsSealed;
				this._baseName = null;
				this._elementName = this._name;
				this._particles = new ArrayList();
				this._abstractElms = new ArrayList();
				this._fullRefName = WsdlGenerator.RefName(this._type);
			}

			// Token: 0x1700012A RID: 298
			// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001D4E5 File Offset: 0x0001C4E5
			internal string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x1700012B RID: 299
			// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001D4ED File Offset: 0x0001C4ED
			internal string FullRefName
			{
				get
				{
					return this._fullRefName;
				}
			}

			// Token: 0x1700012C RID: 300
			// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001D4F5 File Offset: 0x0001C4F5
			// (set) Token: 0x06000536 RID: 1334 RVA: 0x0001D4FD File Offset: 0x0001C4FD
			protected string BaseName
			{
				get
				{
					return this._baseName;
				}
				set
				{
					this._baseName = value;
				}
			}

			// Token: 0x1700012D RID: 301
			// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001D506 File Offset: 0x0001C506
			// (set) Token: 0x06000538 RID: 1336 RVA: 0x0001D50E File Offset: 0x0001C50E
			internal string ElementName
			{
				get
				{
					return this._elementName;
				}
				set
				{
					this._elementName = value;
				}
			}

			// Token: 0x1700012E RID: 302
			// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001D517 File Offset: 0x0001C517
			protected bool IsSealed
			{
				get
				{
					return this._bSealed;
				}
			}

			// Token: 0x1700012F RID: 303
			// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001D51F File Offset: 0x0001C51F
			protected bool IsEmpty
			{
				get
				{
					return this._abstractElms.Count == 0 && this._particles.Count == 0;
				}
			}

			// Token: 0x0600053B RID: 1339 RVA: 0x0001D53E File Offset: 0x0001C53E
			internal void AddParticle(WsdlGenerator.Particle particle)
			{
				this._particles.Add(particle);
			}

			// Token: 0x0600053C RID: 1340 RVA: 0x0001D550 File Offset: 0x0001C550
			protected void PrintBody(TextWriter textWriter, StringBuilder sb, string indent)
			{
				int count = this._particles.Count;
				string text = WsdlGenerator.IndentP(indent);
				string text2 = WsdlGenerator.IndentP(text);
				if (count > 0)
				{
					bool flag = WsdlGenerator.blockDefault != this._blockType;
					if (flag)
					{
						sb.Length = 0;
						sb.Append(text);
						sb.Append(WsdlGenerator.ComplexSchemaType.schemaBlockBegin[(int)this._blockType]);
						textWriter.WriteLine(sb);
					}
					for (int i = 0; i < count; i++)
					{
						((WsdlGenerator.Particle)this._particles[i]).Print(textWriter, sb, WsdlGenerator.IndentP(text2));
					}
					if (flag)
					{
						sb.Length = 0;
						sb.Append(text);
						sb.Append(WsdlGenerator.ComplexSchemaType.schemaBlockEnd[(int)this._blockType]);
						textWriter.WriteLine(sb);
					}
				}
				int count2 = this._abstractElms.Count;
				for (int j = 0; j < count2; j++)
				{
					((WsdlGenerator.IAbstractElement)this._abstractElms[j]).Print(textWriter, sb, WsdlGenerator.IndentP(indent));
				}
			}

			// Token: 0x04000473 RID: 1139
			private string _name;

			// Token: 0x04000474 RID: 1140
			private Type _type;

			// Token: 0x04000475 RID: 1141
			private string _fullRefName;

			// Token: 0x04000476 RID: 1142
			private string _baseName;

			// Token: 0x04000477 RID: 1143
			private string _elementName;

			// Token: 0x04000478 RID: 1144
			private bool _bSealed;

			// Token: 0x04000479 RID: 1145
			private SchemaBlockType _blockType;

			// Token: 0x0400047A RID: 1146
			private ArrayList _particles;

			// Token: 0x0400047B RID: 1147
			private ArrayList _abstractElms;

			// Token: 0x0400047C RID: 1148
			private static string[] schemaBlockBegin = new string[] { "<all>", "<sequence>", "<choice>", "<complexContent>" };

			// Token: 0x0400047D RID: 1149
			private static string[] schemaBlockEnd = new string[] { "</all>", "</sequence>", "</choice>", "</complexContent>" };
		}

		// Token: 0x020000B2 RID: 178
		private class PhonySchemaType : WsdlGenerator.ComplexSchemaType
		{
			// Token: 0x0600053E RID: 1342 RVA: 0x0001D6BB File Offset: 0x0001C6BB
			internal PhonySchemaType(string name)
				: base(name, true)
			{
				this._numOverloadedTypes = 0;
			}

			// Token: 0x0600053F RID: 1343 RVA: 0x0001D6CC File Offset: 0x0001C6CC
			internal int OverloadedType()
			{
				return ++this._numOverloadedTypes;
			}

			// Token: 0x06000540 RID: 1344 RVA: 0x0001D6EA File Offset: 0x0001C6EA
			internal override void PrintSchemaType(TextWriter textWriter, StringBuilder sb, string indent, bool bAnonymous)
			{
			}

			// Token: 0x0400047E RID: 1150
			private int _numOverloadedTypes;

			// Token: 0x0400047F RID: 1151
			internal ArrayList _inParamTypes;

			// Token: 0x04000480 RID: 1152
			internal ArrayList _inParamNames;

			// Token: 0x04000481 RID: 1153
			internal ArrayList _outParamTypes;

			// Token: 0x04000482 RID: 1154
			internal ArrayList _outParamNames;

			// Token: 0x04000483 RID: 1155
			internal ArrayList _paramNamesOrder;

			// Token: 0x04000484 RID: 1156
			internal string _returnType;

			// Token: 0x04000485 RID: 1157
			internal string _returnName;
		}

		// Token: 0x020000B3 RID: 179
		private class ArraySchemaType : WsdlGenerator.ComplexSchemaType
		{
			// Token: 0x06000541 RID: 1345 RVA: 0x0001D6EC File Offset: 0x0001C6EC
			internal ArraySchemaType(Type type, string name, SchemaBlockType blockType, bool bSealed)
				: base(name, blockType, bSealed)
			{
				this._type = type;
			}

			// Token: 0x17000130 RID: 304
			// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001D6FF File Offset: 0x0001C6FF
			internal Type Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x06000543 RID: 1347 RVA: 0x0001D708 File Offset: 0x0001C708
			internal override void PrintSchemaType(TextWriter textWriter, StringBuilder sb, string indent, bool bAnonymous)
			{
				string text = WsdlGenerator.IndentP(indent);
				sb.Length = 0;
				sb.Append(indent);
				sb.Append("<complexType name='");
				sb.Append(base.FullRefName);
				sb.Append("'>");
				textWriter.WriteLine(sb);
				base.PrintBody(textWriter, sb, text);
				sb.Length = 0;
				sb.Append(indent);
				sb.Append("</complexType>");
				textWriter.WriteLine(sb);
			}

			// Token: 0x04000486 RID: 1158
			private Type _type;
		}

		// Token: 0x020000B4 RID: 180
		private class RealSchemaType : WsdlGenerator.ComplexSchemaType
		{
			// Token: 0x06000544 RID: 1348 RVA: 0x0001D784 File Offset: 0x0001C784
			internal RealSchemaType(Type type, WsdlGenerator.XMLNamespace xns, string serviceEndpoint, Hashtable typeToServiceEndpoint, bool bUnique, WsdlGenerator WsdlGenerator)
				: base(type)
			{
				this._type = type;
				this._serviceEndpoint = serviceEndpoint;
				this._typeToServiceEndpoint = typeToServiceEndpoint;
				this._bUnique = bUnique;
				this._WsdlGenerator = WsdlGenerator;
				this._bStruct = type.IsValueType;
				this._xns = xns;
				this._implIFaces = null;
				this._iFaces = null;
				this._methods = null;
				this._fields = null;
				this._methodTypes = null;
				this._nestedTypes = type.GetNestedTypes();
				if (this._nestedTypes != null)
				{
					foreach (Type type2 in this._nestedTypes)
					{
						this._WsdlGenerator.AddType(type2, xns);
					}
				}
			}

			// Token: 0x17000131 RID: 305
			// (get) Token: 0x06000545 RID: 1349 RVA: 0x0001D82E File Offset: 0x0001C82E
			internal Type Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x17000132 RID: 306
			// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001D836 File Offset: 0x0001C836
			internal WsdlGenerator.XMLNamespace XNS
			{
				get
				{
					return this._xns;
				}
			}

			// Token: 0x17000133 RID: 307
			// (get) Token: 0x06000547 RID: 1351 RVA: 0x0001D83E File Offset: 0x0001C83E
			internal bool IsUnique
			{
				get
				{
					return this._bUnique;
				}
			}

			// Token: 0x17000134 RID: 308
			// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001D848 File Offset: 0x0001C848
			internal bool IsSUDSType
			{
				get
				{
					return (this._iFaces != null && this._iFaces.Length > 0) || (this._methods != null && this._methods.Length > 0) || (this._type != null && this._type.IsInterface) || (WsdlGenerator.s_delegateType != null && WsdlGenerator.s_delegateType.IsAssignableFrom(this._type));
				}
			}

			// Token: 0x06000549 RID: 1353 RVA: 0x0001D8AB File Offset: 0x0001C8AB
			internal Type[] GetIntroducedInterfaces()
			{
				this._iFaces = WsdlGenerator.RealSchemaType.GetIntroducedInterfaces(this._type);
				return this._iFaces;
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x0001D8C4 File Offset: 0x0001C8C4
			internal MethodInfo[] GetIntroducedMethods()
			{
				this._methods = WsdlGenerator.RealSchemaType.GetIntroducedMethods(this._type, ref this._methodAttributes);
				this._methodTypes = new string[2 * this._methods.Length];
				return this._methods;
			}

			// Token: 0x0600054B RID: 1355 RVA: 0x0001D8F8 File Offset: 0x0001C8F8
			internal FieldInfo[] GetInstanceFields()
			{
				this._fields = WsdlGenerator.RealSchemaType.GetInstanceFields(this._type);
				return this._fields;
			}

			// Token: 0x0600054C RID: 1356 RVA: 0x0001D914 File Offset: 0x0001C914
			private bool IsNotSystemDefinedRoot(Type type, Type baseType)
			{
				return !type.IsInterface && !type.IsValueType && baseType != null && baseType.BaseType != null && baseType != WsdlGenerator.s_marshalByRefType && baseType != WsdlGenerator.s_valueType && baseType != WsdlGenerator.s_objectType && baseType != WsdlGenerator.s_contextBoundType && baseType != WsdlGenerator.s_remotingClientProxyType && baseType.FullName != "System.EnterpriseServices.ServicedComponent" && baseType.FullName != "System.__ComObject";
			}

			// Token: 0x0600054D RID: 1357 RVA: 0x0001D98C File Offset: 0x0001C98C
			internal void Resolve(StringBuilder sb)
			{
				sb.Length = 0;
				bool isSUDSType = this.IsSUDSType;
				Type baseType = this._type.BaseType;
				if (this.IsNotSystemDefinedRoot(this._type, baseType))
				{
					WsdlGenerator.XMLNamespace @namespace = this._WsdlGenerator.GetNamespace(baseType);
					sb.Append(@namespace.Prefix);
					sb.Append(':');
					sb.Append(baseType.Name);
					base.BaseName = sb.ToString();
					if (isSUDSType)
					{
						this._xns.DependsOnSUDSNS(@namespace);
					}
					Type type = this._type;
					Type type2 = type.BaseType;
					while (type2 != null && this.IsNotSystemDefinedRoot(type, type2))
					{
						if (this._typeToServiceEndpoint != null && !this._typeToServiceEndpoint.ContainsKey(type2.Name) && this._typeToServiceEndpoint.ContainsKey(type.Name))
						{
							this._typeToServiceEndpoint[type2.Name] = this._typeToServiceEndpoint[type.Name];
						}
						type = type2;
						type2 = type.BaseType;
					}
				}
				this._xns.DependsOnSchemaNS(this._xns, false);
				if (isSUDSType)
				{
					this._xns.AddRealSUDSType(this);
					if (this._iFaces.Length > 0)
					{
						this._implIFaces = new string[this._iFaces.Length];
						for (int i = 0; i < this._iFaces.Length; i++)
						{
							string text;
							Assembly assembly;
							WsdlGenerator.GetNSAndAssembly(this._iFaces[i], out text, out assembly);
							WsdlGenerator.XMLNamespace xmlnamespace = this._xns.LookupSchemaNamespace(text, assembly);
							sb.Length = 0;
							sb.Append(xmlnamespace.Prefix);
							sb.Append(':');
							sb.Append(this._iFaces[i].Name);
							this._implIFaces[i] = sb.ToString();
							this._xns.DependsOnSUDSNS(xmlnamespace);
						}
					}
					if (this._methods.Length > 0)
					{
						string text2;
						if (this._xns.IsInteropType)
						{
							text2 = this._xns.Name;
						}
						else
						{
							sb.Length = 0;
							WsdlGenerator.QualifyName(sb, this._xns.Name, base.Name);
							text2 = sb.ToString();
						}
						WsdlGenerator.XMLNamespace xmlnamespace2 = this._xns.LookupSchemaNamespace(text2, this._xns.Assem);
						this._xns.DependsOnSUDSNS(xmlnamespace2);
						this._phony = new WsdlGenerator.PhonySchemaType[this._methods.Length];
						for (int j = 0; j < this._methods.Length; j++)
						{
							MethodInfo methodInfo = this._methods[j];
							string name = methodInfo.Name;
							ParameterInfo[] parameters = methodInfo.GetParameters();
							WsdlGenerator.PhonySchemaType phonySchemaType = new WsdlGenerator.PhonySchemaType(name);
							phonySchemaType._inParamTypes = new ArrayList(10);
							phonySchemaType._inParamNames = new ArrayList(10);
							phonySchemaType._outParamTypes = new ArrayList(10);
							phonySchemaType._outParamNames = new ArrayList(10);
							phonySchemaType._paramNamesOrder = new ArrayList(10);
							int num = 0;
							foreach (ParameterInfo parameterInfo in parameters)
							{
								bool flag = false;
								bool flag2 = false;
								phonySchemaType._paramNamesOrder.Add(parameterInfo.Name);
								this.ParamInOut(parameterInfo, out flag, out flag2);
								Type parameterType = parameterInfo.ParameterType;
								string text3 = parameterInfo.Name;
								if (text3 == null || text3.Length == 0)
								{
									text3 = "param" + num++;
								}
								string text4 = WsdlGenerator.RealSchemaType.TypeName(parameterType, true, xmlnamespace2);
								if (flag)
								{
									phonySchemaType._inParamNames.Add(text3);
									phonySchemaType._inParamTypes.Add(text4);
								}
								if (flag2)
								{
									phonySchemaType._outParamNames.Add(text3);
									phonySchemaType._outParamTypes.Add(text4);
								}
							}
							xmlnamespace2.AddPhonySchemaType(phonySchemaType);
							this._phony[j] = phonySchemaType;
							this._methodTypes[2 * j] = phonySchemaType.ElementName;
							if (!RemotingServices.IsOneWay(methodInfo))
							{
								SoapMethodAttribute soapMethodAttribute = (SoapMethodAttribute)InternalRemotingServices.GetCachedSoapAttribute(methodInfo);
								string text5;
								if (soapMethodAttribute.ReturnXmlElementName != null)
								{
									text5 = soapMethodAttribute.ReturnXmlElementName;
								}
								else
								{
									text5 = "return";
								}
								string text6;
								if (soapMethodAttribute.ResponseXmlElementName != null)
								{
									text6 = soapMethodAttribute.ResponseXmlElementName;
								}
								else
								{
									text6 = name + "Response";
								}
								WsdlGenerator.PhonySchemaType phonySchemaType2 = new WsdlGenerator.PhonySchemaType(text6);
								phonySchemaType._returnName = text5;
								Type returnType = methodInfo.ReturnType;
								if (returnType != null && returnType != typeof(void))
								{
									phonySchemaType._returnType = WsdlGenerator.RealSchemaType.TypeName(returnType, true, xmlnamespace2);
								}
								xmlnamespace2.AddPhonySchemaType(phonySchemaType2);
								this._methodTypes[2 * j + 1] = phonySchemaType2.ElementName;
							}
						}
					}
				}
				if (this._fields != null)
				{
					for (int l = 0; l < this._fields.Length; l++)
					{
						FieldInfo fieldInfo = this._fields[l];
						Type type3 = fieldInfo.FieldType;
						if (type3 == null)
						{
							type3 = typeof(object);
						}
						base.AddParticle(new WsdlGenerator.SchemaElement(fieldInfo.Name, type3, false, this._xns));
					}
				}
			}

			// Token: 0x0600054E RID: 1358 RVA: 0x0001DE88 File Offset: 0x0001CE88
			private void ParamInOut(ParameterInfo param, out bool bMarshalIn, out bool bMarshalOut)
			{
				bool isIn = param.IsIn;
				bool isOut = param.IsOut;
				bool isByRef = param.ParameterType.IsByRef;
				bMarshalIn = false;
				bMarshalOut = false;
				if (!isByRef)
				{
					bMarshalIn = true;
					bMarshalOut = isOut;
					return;
				}
				if (isIn == isOut)
				{
					bMarshalIn = true;
					bMarshalOut = true;
					return;
				}
				bMarshalIn = isIn;
				bMarshalOut = isOut;
			}

			// Token: 0x0600054F RID: 1359 RVA: 0x0001DED0 File Offset: 0x0001CED0
			internal override void PrintSchemaType(TextWriter textWriter, StringBuilder sb, string indent, bool bAnonymous)
			{
				if (!bAnonymous)
				{
					sb.Length = 0;
					sb.Append(indent);
					sb.Append("<element name='");
					sb.Append(base.ElementName);
					sb.Append("' type='");
					sb.Append(this._xns.Prefix);
					sb.Append(':');
					sb.Append(base.FullRefName);
					sb.Append("'/>");
					textWriter.WriteLine(sb);
				}
				sb.Length = 0;
				sb.Append(indent);
				if (!bAnonymous)
				{
					sb.Append("<complexType name='");
					sb.Append(base.FullRefName);
					sb.Append('\'');
				}
				else
				{
					sb.Append("<complexType ");
				}
				if (base.BaseName != null)
				{
					sb.Append(" base='");
					sb.Append(base.BaseName);
					sb.Append('\'');
				}
				if (base.IsSealed && !bAnonymous)
				{
					sb.Append(" final='#all'");
				}
				bool isEmpty = base.IsEmpty;
				if (isEmpty)
				{
					sb.Append("/>");
				}
				else
				{
					sb.Append('>');
				}
				textWriter.WriteLine(sb);
				if (isEmpty)
				{
					return;
				}
				base.PrintBody(textWriter, sb, indent);
				textWriter.Write(indent);
				textWriter.WriteLine("</complexType>");
			}

			// Token: 0x06000550 RID: 1360 RVA: 0x0001E01C File Offset: 0x0001D01C
			internal void PrintMessageWsdl(TextWriter textWriter, StringBuilder sb, string indent, ArrayList refNames)
			{
				string text = WsdlGenerator.IndentP(indent);
				string text2 = WsdlGenerator.IndentP(text);
				string text3 = WsdlGenerator.IndentP(text2);
				string text4 = null;
				string text5 = null;
				bool flag = false;
				string text6;
				if (this._xns.IsInteropType)
				{
					text6 = this._xns.Name;
				}
				else
				{
					sb.Length = 0;
					WsdlGenerator.QualifyName(sb, this._xns.Name, base.Name);
					text6 = sb.ToString();
				}
				WsdlGenerator.XMLNamespace xmlnamespace = this._xns.LookupSchemaNamespace(text6, this._xns.Assem);
				int num = 0;
				if (this._methods != null)
				{
					num = this._methods.Length;
				}
				if (num > 0)
				{
					text4 = xmlnamespace.Namespace;
					string prefix = xmlnamespace.Prefix;
				}
				refNames.Add(base.Name);
				for (int i = 0; i < num; i++)
				{
					MethodInfo methodInfo = this._methods[i];
					flag = RemotingServices.IsOneWay(methodInfo);
					string text7 = WsdlGenerator.RealSchemaType.PrintMethodName(methodInfo);
					sb.Length = 0;
					WsdlGenerator.QualifyName(sb, base.Name, this._methodTypes[2 * i]);
					text5 = sb.ToString();
					sb.Length = 0;
					sb.Append("\n");
					sb.Append(indent);
					sb.Append("<message name='");
					sb.Append(text5 + "Input");
					sb.Append("'>");
					textWriter.WriteLine(sb);
					WsdlGenerator.PhonySchemaType phonySchemaType = this._phony[i];
					if (phonySchemaType._inParamTypes != null)
					{
						for (int j = 0; j < phonySchemaType._inParamTypes.Count; j++)
						{
							sb.Length = 0;
							sb.Append(text);
							sb.Append("<part name='");
							sb.Append(phonySchemaType._inParamNames[j]);
							sb.Append("' type='");
							sb.Append(phonySchemaType._inParamTypes[j]);
							sb.Append("'/>");
							textWriter.WriteLine(sb);
						}
						sb.Length = 0;
						sb.Append(indent);
						sb.Append("</message>");
						textWriter.WriteLine(sb);
						if (!flag)
						{
							sb.Length = 0;
							sb.Append(indent);
							sb.Append("<message name='");
							sb.Append(text5 + "Output");
							sb.Append("'>");
							textWriter.WriteLine(sb);
							if (phonySchemaType._returnType != null || phonySchemaType._outParamTypes != null)
							{
								if (phonySchemaType._returnType != null)
								{
									sb.Length = 0;
									sb.Append(text);
									sb.Append("<part name='");
									sb.Append(phonySchemaType._returnName);
									sb.Append("' type='");
									sb.Append(phonySchemaType._returnType);
									sb.Append("'/>");
									textWriter.WriteLine(sb);
								}
								if (phonySchemaType._outParamTypes != null)
								{
									for (int k = 0; k < phonySchemaType._outParamTypes.Count; k++)
									{
										sb.Length = 0;
										sb.Append(text);
										sb.Append("<part name='");
										sb.Append(phonySchemaType._outParamNames[k]);
										sb.Append("' type='");
										sb.Append(phonySchemaType._outParamTypes[k]);
										sb.Append("'/>");
										textWriter.WriteLine(sb);
									}
								}
							}
							sb.Length = 0;
							sb.Append(indent);
							sb.Append("</message>");
							textWriter.WriteLine(sb);
						}
					}
				}
				sb.Length = 0;
				sb.Append("\n");
				sb.Append(indent);
				sb.Append("<portType name='");
				sb.Append(base.Name);
				sb.Append("PortType");
				sb.Append("'>");
				textWriter.WriteLine(sb);
				for (int l = 0; l < num; l++)
				{
					MethodInfo methodInfo = this._methods[l];
					WsdlGenerator.PhonySchemaType phonySchemaType2 = this._phony[l];
					flag = RemotingServices.IsOneWay(methodInfo);
					string text7 = WsdlGenerator.RealSchemaType.PrintMethodName(methodInfo);
					sb.Length = 0;
					sb.Append("tns:");
					WsdlGenerator.QualifyName(sb, base.Name, this._methodTypes[2 * l]);
					text5 = sb.ToString();
					sb.Length = 0;
					sb.Append(text);
					sb.Append("<operation name='");
					sb.Append(text7);
					sb.Append("'");
					if (phonySchemaType2 != null && phonySchemaType2._paramNamesOrder.Count > 0)
					{
						sb.Append(" parameterOrder='");
						bool flag2 = true;
						foreach (object obj in phonySchemaType2._paramNamesOrder)
						{
							string text8 = (string)obj;
							if (!flag2)
							{
								sb.Append(" ");
							}
							sb.Append(text8);
							flag2 = false;
						}
						sb.Append("'");
					}
					sb.Append(">");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text2);
					sb.Append("<input name='");
					sb.Append(this._methodTypes[2 * l]);
					sb.Append("Request' ");
					sb.Append("message='");
					sb.Append(text5);
					sb.Append("Input");
					sb.Append("'/>");
					textWriter.WriteLine(sb);
					if (!flag)
					{
						sb.Length = 0;
						sb.Append(text2);
						sb.Append("<output name='");
						sb.Append(this._methodTypes[2 * l]);
						sb.Append("Response' ");
						sb.Append("message='");
						sb.Append(text5);
						sb.Append("Output");
						sb.Append("'/>");
						textWriter.WriteLine(sb);
					}
					sb.Length = 0;
					sb.Append(text);
					sb.Append("</operation>");
					textWriter.WriteLine(sb);
				}
				sb.Length = 0;
				sb.Append(indent);
				sb.Append("</portType>");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append("\n");
				sb.Append(indent);
				sb.Append("<binding name='");
				sb.Append(base.Name);
				sb.Append("Binding");
				sb.Append("' ");
				sb.Append("type='tns:");
				sb.Append(base.Name);
				sb.Append("PortType");
				sb.Append("'>");
				textWriter.WriteLine(sb);
				sb.Length = 0;
				sb.Append(text);
				sb.Append("<soap:binding style='rpc' transport='http://schemas.xmlsoap.org/soap/http'/>");
				textWriter.WriteLine(sb);
				if (this._type.IsInterface || this.IsSUDSType)
				{
					this.PrintSuds(this._type, this._implIFaces, this._nestedTypes, textWriter, sb, indent);
				}
				if (!this._xns.IsClassesPrinted)
				{
					for (int m = 0; m < this._xns._realSchemaTypes.Count; m++)
					{
						WsdlGenerator.RealSchemaType realSchemaType = (WsdlGenerator.RealSchemaType)this._xns._realSchemaTypes[m];
						Type type = realSchemaType._type;
						if (!realSchemaType.Type.IsInterface && !realSchemaType.IsSUDSType)
						{
							Type[] introducedInterfaces = WsdlGenerator.RealSchemaType.GetIntroducedInterfaces(realSchemaType._type);
							string[] array = null;
							bool flag3 = false;
							if (introducedInterfaces.Length > 0)
							{
								array = new string[introducedInterfaces.Length];
								int num2 = 0;
								while (m < introducedInterfaces.Length)
								{
									string text9;
									Assembly assembly;
									WsdlGenerator.GetNSAndAssembly(introducedInterfaces[num2], out text9, out assembly);
									WsdlGenerator.XMLNamespace xmlnamespace2 = this._xns.LookupSchemaNamespace(text9, assembly);
									sb.Length = 0;
									sb.Append(xmlnamespace2.Prefix);
									sb.Append(':');
									sb.Append(introducedInterfaces[num2].Name);
									array[num2] = sb.ToString();
									if (array[num2].Length > 0)
									{
										flag3 = true;
									}
									m++;
								}
							}
							if (!flag3)
							{
								array = null;
							}
							this.PrintSuds(type, array, realSchemaType._nestedTypes, textWriter, sb, indent);
						}
					}
					this._xns.IsClassesPrinted = true;
				}
				for (int n = 0; n < num; n++)
				{
					MethodInfo methodInfo = this._methods[n];
					string text7 = WsdlGenerator.RealSchemaType.PrintMethodName(methodInfo);
					flag = RemotingServices.IsOneWay(methodInfo);
					sb.Length = 0;
					sb.Append(text);
					sb.Append("<operation name='");
					sb.Append(text7);
					sb.Append("'>");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text2);
					sb.Append("<soap:operation soapAction='");
					string soapActionFromMethodBase = SoapServices.GetSoapActionFromMethodBase(methodInfo);
					if (soapActionFromMethodBase != null || soapActionFromMethodBase.Length > 0)
					{
						sb.Append(soapActionFromMethodBase);
					}
					else
					{
						sb.Append(text4);
						sb.Append('#');
						sb.Append(text7);
					}
					sb.Append("'/>");
					textWriter.WriteLine(sb);
					if (this._methodAttributes != null && n < this._methodAttributes.Length && this._methodAttributes[n] != null)
					{
						sb.Length = 0;
						sb.Append(text2);
						sb.Append("<suds:method attributes='");
						sb.Append(this._methodAttributes[n]);
						sb.Append("'/>");
						textWriter.WriteLine(sb);
					}
					sb.Length = 0;
					sb.Append(text2);
					sb.Append("<input name='");
					sb.Append(this._methodTypes[2 * n]);
					sb.Append("Request'>");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text3);
					sb.Append("<soap:body use='encoded' encodingStyle='http://schemas.xmlsoap.org/soap/encoding/' namespace='");
					string text10 = SoapServices.GetXmlNamespaceForMethodCall(methodInfo);
					if (text10 == null)
					{
						sb.Append(text4);
					}
					else
					{
						sb.Append(text10);
					}
					sb.Append("'/>");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text2);
					sb.Append("</input>");
					textWriter.WriteLine(sb);
					if (!flag)
					{
						sb.Length = 0;
						sb.Append(text2);
						sb.Append("<output name='");
						sb.Append(this._methodTypes[2 * n]);
						sb.Append("Response'>");
						textWriter.WriteLine(sb);
						sb.Length = 0;
						sb.Append(text3);
						sb.Append("<soap:body use='encoded' encodingStyle='http://schemas.xmlsoap.org/soap/encoding/' namespace='");
						text10 = SoapServices.GetXmlNamespaceForMethodResponse(methodInfo);
						if (text10 == null)
						{
							sb.Append(text4);
						}
						else
						{
							sb.Append(text10);
						}
						sb.Append("'/>");
						textWriter.WriteLine(sb);
						sb.Length = 0;
						sb.Append(text2);
						sb.Append("</output>");
						textWriter.WriteLine(sb);
					}
					sb.Length = 0;
					sb.Append(text);
					sb.Append("</operation>");
					textWriter.WriteLine(sb);
				}
				sb.Length = 0;
				sb.Append(indent);
				sb.Append("</binding>");
				textWriter.WriteLine(sb);
			}

			// Token: 0x06000551 RID: 1361 RVA: 0x0001EB60 File Offset: 0x0001DB60
			private void PrintSuds(Type type, string[] implIFaces, Type[] nestedTypes, TextWriter textWriter, StringBuilder sb, string indent)
			{
				string text = WsdlGenerator.IndentP(indent);
				string text2 = WsdlGenerator.IndentP(text);
				WsdlGenerator.IndentP(text2);
				sb.Length = 0;
				sb.Append(text);
				string text3;
				if (type.IsInterface)
				{
					sb.Append("<suds:interface type='");
					text3 = "</suds:interface>";
				}
				else if (type.IsValueType)
				{
					sb.Append("<suds:struct type='");
					text3 = "</suds:struct>";
				}
				else
				{
					sb.Append("<suds:class type='");
					text3 = "</suds:class>";
				}
				sb.Append(this._xns.Prefix);
				sb.Append(':');
				sb.Append(WsdlGenerator.RefName(type));
				sb.Append("'");
				Type baseType = type.BaseType;
				if (this.IsNotSystemDefinedRoot(type, baseType))
				{
					WsdlGenerator.XMLNamespace @namespace = this._WsdlGenerator.GetNamespace(baseType);
					sb.Append(" extends='");
					sb.Append(@namespace.Prefix);
					sb.Append(':');
					sb.Append(baseType.Name);
					sb.Append("'");
				}
				if (baseType != null && baseType.FullName == "System.EnterpriseServices.ServicedComponent")
				{
					sb.Append(" rootType='ServicedComponent'");
				}
				else if (typeof(Delegate).IsAssignableFrom(type) || typeof(MulticastDelegate).IsAssignableFrom(type))
				{
					sb.Append(" rootType='Delegate'");
				}
				else if (typeof(MarshalByRefObject).IsAssignableFrom(type))
				{
					sb.Append(" rootType='MarshalByRefObject'");
				}
				else if (typeof(ISerializable).IsAssignableFrom(type))
				{
					sb.Append(" rootType='ISerializable'");
				}
				if (implIFaces == null && nestedTypes == null)
				{
					sb.Append("/>");
				}
				else
				{
					sb.Append(">");
				}
				textWriter.WriteLine(sb);
				string text4;
				if (type.IsInterface)
				{
					text4 = "<suds:extends type='";
				}
				else
				{
					text4 = "<suds:implements type='";
				}
				if (implIFaces != null)
				{
					for (int i = 0; i < implIFaces.Length; i++)
					{
						if (implIFaces[i] != null && !(implIFaces[i] == string.Empty))
						{
							sb.Length = 0;
							sb.Append(text2);
							sb.Append(text4);
							sb.Append(implIFaces[i]);
							sb.Append("'/>");
							textWriter.WriteLine(sb);
						}
					}
				}
				if (nestedTypes != null)
				{
					for (int j = 0; j < nestedTypes.Length; j++)
					{
						sb.Length = 0;
						sb.Append(text2);
						sb.Append("<suds:nestedType name='");
						sb.Append(nestedTypes[j].Name);
						sb.Append("' type='");
						sb.Append(this._xns.Prefix);
						sb.Append(':');
						sb.Append(WsdlGenerator.RefName(nestedTypes[j]));
						sb.Append("'/>");
						textWriter.WriteLine(sb);
					}
				}
				if (implIFaces != null || nestedTypes != null)
				{
					sb.Length = 0;
					sb.Append(text);
					sb.Append(text3);
					textWriter.WriteLine(sb);
				}
			}

			// Token: 0x06000552 RID: 1362 RVA: 0x0001EE88 File Offset: 0x0001DE88
			private static string ProcessArray(Type type, WsdlGenerator.XMLNamespace xns)
			{
				bool flag = false;
				Type type2 = type.GetElementType();
				string text = "ArrayOf";
				while (type2.IsArray)
				{
					text += "ArrayOf";
					type2 = type2.GetElementType();
				}
				string text2 = WsdlGenerator.RealSchemaType.TypeName(type2, true, xns);
				int num = text2.IndexOf(":");
				text2.Substring(0, num);
				string text3 = text2.Substring(num + 1);
				int arrayRank = type.GetArrayRank();
				string text4 = "";
				if (arrayRank > 1)
				{
					text4 = arrayRank.ToString(CultureInfo.InvariantCulture);
				}
				string text5 = text + text3.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + text3.Substring(1) + text4;
				text5 = text5.Replace('+', 'N');
				if (xns.LookupArraySchemaType(text5) == null)
				{
					WsdlGenerator.ArraySchemaType arraySchemaType = new WsdlGenerator.ArraySchemaType(type, text5, SchemaBlockType.ComplexContent, false);
					WsdlGenerator.Restriction restriction = new WsdlGenerator.Restriction();
					WsdlGenerator.SchemaAttribute schemaAttribute = new WsdlGenerator.SchemaAttribute();
					if (flag)
					{
						schemaAttribute.AddArray(text2);
					}
					else
					{
						string name = type.Name;
						num = name.IndexOf("[");
						schemaAttribute.AddArray(text2 + name.Substring(num));
					}
					restriction.AddArray(schemaAttribute);
					arraySchemaType.AddParticle(restriction);
					xns.AddArraySchemaType(arraySchemaType);
				}
				return xns.Prefix + ":" + text5;
			}

			// Token: 0x06000553 RID: 1363 RVA: 0x0001EFD4 File Offset: 0x0001DFD4
			internal static string TypeName(Type type, bool bEmbedded, WsdlGenerator.XMLNamespace thisxns)
			{
				if (type.IsArray)
				{
					return WsdlGenerator.RealSchemaType.ProcessArray(type, thisxns);
				}
				string text = WsdlGenerator.RefName(type);
				Type type2 = type;
				if (type.IsByRef)
				{
					type2 = type.GetElementType();
					text = WsdlGenerator.RefName(type2);
					if (type2.IsArray)
					{
						return WsdlGenerator.RealSchemaType.ProcessArray(type2, thisxns);
					}
				}
				string text2 = SudsConverter.MapClrTypeToXsdType(type2);
				if (text2 == null)
				{
					string @namespace = type.Namespace;
					Assembly assembly = type.Module.Assembly;
					WsdlGenerator.XMLNamespace xmlnamespace = (WsdlGenerator.XMLNamespace)thisxns.Generator._typeToInteropNS[type];
					if (xmlnamespace == null)
					{
						xmlnamespace = thisxns.LookupSchemaNamespace(@namespace, assembly);
						if (xmlnamespace == null)
						{
							xmlnamespace = thisxns.Generator.LookupNamespace(@namespace, assembly);
							if (xmlnamespace == null)
							{
								xmlnamespace = thisxns.Generator.AddNamespace(@namespace, assembly);
							}
							thisxns.DependsOnSchemaNS(xmlnamespace, false);
						}
					}
					StringBuilder stringBuilder = new StringBuilder(256);
					stringBuilder.Append(xmlnamespace.Prefix);
					stringBuilder.Append(':');
					stringBuilder.Append(text);
					text2 = stringBuilder.ToString();
				}
				return text2;
			}

			// Token: 0x06000554 RID: 1364 RVA: 0x0001F0D8 File Offset: 0x0001E0D8
			private static Type[] GetIntroducedInterfaces(Type type)
			{
				ArrayList arrayList = new ArrayList();
				Type[] interfaces = type.GetInterfaces();
				foreach (Type type2 in interfaces)
				{
					if (!type2.FullName.StartsWith("System."))
					{
						arrayList.Add(type2);
					}
				}
				Type[] array2 = new Type[arrayList.Count];
				for (int j = 0; j < arrayList.Count; j++)
				{
					array2[j] = (Type)arrayList[j];
				}
				return array2;
			}

			// Token: 0x06000555 RID: 1365 RVA: 0x0001F15C File Offset: 0x0001E15C
			private static void FindMethodAttributes(Type type, MethodInfo[] infos, ref string[] methodAttributes, BindingFlags bFlags)
			{
				Type type2 = type;
				ArrayList arrayList = new ArrayList();
				for (;;)
				{
					type2 = type2.BaseType;
					if (type2 == null || type2.FullName.StartsWith("System."))
					{
						break;
					}
					arrayList.Add(type2);
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < infos.Length; i++)
				{
					MethodBase methodBase = infos[i];
					stringBuilder.Length = 0;
					MethodAttributes attributes = methodBase.Attributes;
					bool isVirtual = methodBase.IsVirtual;
					bool flag = (attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.VtableLayoutMask;
					if (methodBase.IsPublic)
					{
						stringBuilder.Append("public");
					}
					else if (methodBase.IsFamily)
					{
						stringBuilder.Append("protected");
					}
					else if (methodBase.IsAssembly)
					{
						stringBuilder.Append("internal");
					}
					bool flag2 = false;
					for (int j = 0; j < arrayList.Count; j++)
					{
						type2 = (Type)arrayList[j];
						ParameterInfo[] parameters = methodBase.GetParameters();
						Type[] array = new Type[parameters.Length];
						for (int k = 0; k < array.Length; k++)
						{
							array[k] = parameters[k].ParameterType;
						}
						MethodInfo method = type2.GetMethod(methodBase.Name, array);
						if (method != null)
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append(" ");
							}
							if (flag || method.IsFinal)
							{
								stringBuilder.Append("new");
							}
							else if (method.IsVirtual && isVirtual)
							{
								stringBuilder.Append("override");
							}
							else
							{
								stringBuilder.Append("new");
							}
							flag2 = true;
							break;
						}
					}
					if (!flag2 && isVirtual)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(" ");
						}
						stringBuilder.Append("virtual");
					}
					if (stringBuilder.Length > 0)
					{
						methodAttributes[i] = stringBuilder.ToString();
					}
				}
			}

			// Token: 0x06000556 RID: 1366 RVA: 0x0001F334 File Offset: 0x0001E334
			private static MethodInfo[] GetIntroducedMethods(Type type, ref string[] methodAttributes)
			{
				BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
				MethodInfo[] methods = type.GetMethods(bindingFlags);
				if (type.IsInterface)
				{
					return methods;
				}
				methodAttributes = new string[methods.Length];
				WsdlGenerator.RealSchemaType.FindMethodAttributes(type, methods, ref methodAttributes, bindingFlags);
				ArrayList arrayList = new ArrayList();
				Type[] interfaces = type.GetInterfaces();
				foreach (Type type2 in interfaces)
				{
					foreach (MethodInfo methodInfo in type.GetInterfaceMap(type2).TargetMethods)
					{
						if (!methodInfo.IsPublic && type.GetMethod(methodInfo.Name, bindingFlags | BindingFlags.NonPublic) != null)
						{
							arrayList.Add(methodInfo);
						}
					}
				}
				MethodInfo[] array2;
				if (arrayList.Count > 0)
				{
					array2 = new MethodInfo[methods.Length + arrayList.Count];
					for (int k = 0; k < methods.Length; k++)
					{
						array2[k] = methods[k];
					}
					for (int l = 0; l < arrayList.Count; l++)
					{
						array2[methods.Length + l] = (MethodInfo)arrayList[l];
					}
				}
				else
				{
					array2 = methods;
				}
				return array2;
			}

			// Token: 0x06000557 RID: 1367 RVA: 0x0001F450 File Offset: 0x0001E450
			internal static string PrintMethodName(MethodInfo methodInfo)
			{
				string name = methodInfo.Name;
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < name.Length; i++)
				{
					if (name[i] == '.')
					{
						num2 = num;
						num = i;
					}
				}
				string text = name;
				if (num2 > 0)
				{
					text = name.Substring(num2 + 1);
				}
				return text;
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x0001F49C File Offset: 0x0001E49C
			private static FieldInfo[] GetInstanceFields(Type type)
			{
				BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
				if (!WsdlGenerator.s_marshalByRefType.IsAssignableFrom(type))
				{
					bindingFlags |= BindingFlags.NonPublic;
				}
				FieldInfo[] fields = type.GetFields(bindingFlags);
				int num = fields.Length;
				if (num == 0)
				{
					return WsdlGenerator.RealSchemaType.emptyFieldSet;
				}
				for (int i = 0; i < fields.Length; i++)
				{
					if (fields[i].IsStatic)
					{
						num--;
						fields[i] = fields[num];
						fields[num] = null;
					}
				}
				if (num < fields.Length)
				{
					FieldInfo[] array = new FieldInfo[num];
					Array.Copy(fields, array, num);
					return array;
				}
				return fields;
			}

			// Token: 0x04000487 RID: 1159
			private WsdlGenerator _WsdlGenerator;

			// Token: 0x04000488 RID: 1160
			private Type _type;

			// Token: 0x04000489 RID: 1161
			private string _serviceEndpoint;

			// Token: 0x0400048A RID: 1162
			private Hashtable _typeToServiceEndpoint;

			// Token: 0x0400048B RID: 1163
			private bool _bUnique;

			// Token: 0x0400048C RID: 1164
			private WsdlGenerator.XMLNamespace _xns;

			// Token: 0x0400048D RID: 1165
			private bool _bStruct;

			// Token: 0x0400048E RID: 1166
			private string[] _implIFaces;

			// Token: 0x0400048F RID: 1167
			private Type[] _iFaces;

			// Token: 0x04000490 RID: 1168
			private MethodInfo[] _methods;

			// Token: 0x04000491 RID: 1169
			private string[] _methodAttributes;

			// Token: 0x04000492 RID: 1170
			private string[] _methodTypes;

			// Token: 0x04000493 RID: 1171
			private FieldInfo[] _fields;

			// Token: 0x04000494 RID: 1172
			private WsdlGenerator.PhonySchemaType[] _phony;

			// Token: 0x04000495 RID: 1173
			internal Type[] _nestedTypes;

			// Token: 0x04000496 RID: 1174
			private static Type[] emptyTypeSet = new Type[0];

			// Token: 0x04000497 RID: 1175
			private static MethodInfo[] emptyMethodSet = new MethodInfo[0];

			// Token: 0x04000498 RID: 1176
			private static FieldInfo[] emptyFieldSet = new FieldInfo[0];
		}

		// Token: 0x020000B5 RID: 181
		private class XMLNamespace
		{
			// Token: 0x0600055A RID: 1370 RVA: 0x0001F538 File Offset: 0x0001E538
			internal XMLNamespace(string name, Assembly assem, string serviceEndpoint, Hashtable typeToServiceEndpoint, string prefix, bool bInteropType, WsdlGenerator generator)
			{
				this._name = name;
				this._assem = assem;
				this._bUnique = false;
				this._bInteropType = bInteropType;
				this._generator = generator;
				StringBuilder stringBuilder = new StringBuilder(256);
				Assembly assembly = typeof(string).Module.Assembly;
				if (!this._bInteropType)
				{
					if (assem == assembly)
					{
						stringBuilder.Append(SoapServices.CodeXmlNamespaceForClrTypeNamespace(name, null));
					}
					else if (assem != null)
					{
						stringBuilder.Append(SoapServices.CodeXmlNamespaceForClrTypeNamespace(name, assem.FullName));
					}
				}
				else
				{
					stringBuilder.Append(name);
				}
				this._namespace = stringBuilder.ToString();
				this._prefix = prefix;
				this._dependsOnSchemaNS = new ArrayList();
				this._realSUDSTypes = new ArrayList();
				this._dependsOnSUDSNS = new ArrayList();
				this._realSchemaTypes = new ArrayList();
				this._phonySchemaTypes = new ArrayList();
				this._simpleSchemaTypes = new ArrayList();
				this._arraySchemaTypes = new ArrayList();
				this._xnsImports = new ArrayList();
				this._serviceEndpoint = serviceEndpoint;
				this._typeToServiceEndpoint = typeToServiceEndpoint;
			}

			// Token: 0x17000135 RID: 309
			// (get) Token: 0x0600055B RID: 1371 RVA: 0x0001F647 File Offset: 0x0001E647
			internal string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x17000136 RID: 310
			// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001F64F File Offset: 0x0001E64F
			internal Assembly Assem
			{
				get
				{
					return this._assem;
				}
			}

			// Token: 0x17000137 RID: 311
			// (get) Token: 0x0600055D RID: 1373 RVA: 0x0001F657 File Offset: 0x0001E657
			internal string Prefix
			{
				get
				{
					return this._prefix;
				}
			}

			// Token: 0x17000138 RID: 312
			// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001F65F File Offset: 0x0001E65F
			internal string Namespace
			{
				get
				{
					return this._namespace;
				}
			}

			// Token: 0x17000139 RID: 313
			// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001F667 File Offset: 0x0001E667
			internal bool IsInteropType
			{
				get
				{
					return this._bInteropType;
				}
			}

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001F66F File Offset: 0x0001E66F
			internal WsdlGenerator Generator
			{
				get
				{
					return this._generator;
				}
			}

			// Token: 0x1700013B RID: 315
			// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001F677 File Offset: 0x0001E677
			// (set) Token: 0x06000562 RID: 1378 RVA: 0x0001F67F File Offset: 0x0001E67F
			internal bool IsClassesPrinted
			{
				get
				{
					return this._bClassesPrinted;
				}
				set
				{
					this._bClassesPrinted = value;
				}
			}

			// Token: 0x06000563 RID: 1379 RVA: 0x0001F688 File Offset: 0x0001E688
			internal Type LookupSchemaType(string name)
			{
				Type type = null;
				WsdlGenerator.RealSchemaType realSchemaType = this.LookupRealSchemaType(name);
				if (realSchemaType != null)
				{
					type = realSchemaType.Type;
				}
				WsdlGenerator.SimpleSchemaType simpleSchemaType = this.LookupSimpleSchemaType(name);
				if (simpleSchemaType != null)
				{
					type = simpleSchemaType.Type;
				}
				WsdlGenerator.ArraySchemaType arraySchemaType = this.LookupArraySchemaType(name);
				if (arraySchemaType != null)
				{
					type = arraySchemaType.Type;
				}
				return type;
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x0001F6D0 File Offset: 0x0001E6D0
			internal WsdlGenerator.SimpleSchemaType LookupSimpleSchemaType(string name)
			{
				for (int i = 0; i < this._simpleSchemaTypes.Count; i++)
				{
					WsdlGenerator.SimpleSchemaType simpleSchemaType = (WsdlGenerator.SimpleSchemaType)this._simpleSchemaTypes[i];
					if (simpleSchemaType.FullRefName == name)
					{
						return simpleSchemaType;
					}
				}
				return null;
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x0001F718 File Offset: 0x0001E718
			internal bool CheckForSchemaContent()
			{
				if (this._arraySchemaTypes.Count > 0 || this._simpleSchemaTypes.Count > 0)
				{
					return true;
				}
				if (this._realSchemaTypes.Count == 0)
				{
					return false;
				}
				bool flag = false;
				for (int i = 0; i < this._realSchemaTypes.Count; i++)
				{
					WsdlGenerator.RealSchemaType realSchemaType = (WsdlGenerator.RealSchemaType)this._realSchemaTypes[i];
					if (!realSchemaType.Type.IsInterface && !realSchemaType.IsSUDSType)
					{
						flag = true;
						break;
					}
				}
				return flag;
			}

			// Token: 0x06000566 RID: 1382 RVA: 0x0001F79C File Offset: 0x0001E79C
			internal WsdlGenerator.RealSchemaType LookupRealSchemaType(string name)
			{
				for (int i = 0; i < this._realSchemaTypes.Count; i++)
				{
					WsdlGenerator.RealSchemaType realSchemaType = (WsdlGenerator.RealSchemaType)this._realSchemaTypes[i];
					if (realSchemaType.FullRefName == name)
					{
						return realSchemaType;
					}
				}
				return null;
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x0001F7E4 File Offset: 0x0001E7E4
			internal WsdlGenerator.ArraySchemaType LookupArraySchemaType(string name)
			{
				for (int i = 0; i < this._arraySchemaTypes.Count; i++)
				{
					WsdlGenerator.ArraySchemaType arraySchemaType = (WsdlGenerator.ArraySchemaType)this._arraySchemaTypes[i];
					if (arraySchemaType.Name == name)
					{
						return arraySchemaType;
					}
				}
				return null;
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x0001F82A File Offset: 0x0001E82A
			internal void AddRealSUDSType(WsdlGenerator.RealSchemaType rsType)
			{
				this._realSUDSTypes.Add(rsType);
			}

			// Token: 0x06000569 RID: 1385 RVA: 0x0001F839 File Offset: 0x0001E839
			internal void AddRealSchemaType(WsdlGenerator.RealSchemaType rsType)
			{
				this._realSchemaTypes.Add(rsType);
				if (rsType.IsUnique)
				{
					this._bUnique = true;
				}
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x0001F857 File Offset: 0x0001E857
			internal void AddArraySchemaType(WsdlGenerator.ArraySchemaType asType)
			{
				this._arraySchemaTypes.Add(asType);
			}

			// Token: 0x0600056B RID: 1387 RVA: 0x0001F866 File Offset: 0x0001E866
			internal void AddSimpleSchemaType(WsdlGenerator.SimpleSchemaType ssType)
			{
				this._simpleSchemaTypes.Add(ssType);
			}

			// Token: 0x0600056C RID: 1388 RVA: 0x0001F878 File Offset: 0x0001E878
			internal WsdlGenerator.PhonySchemaType LookupPhonySchemaType(string name)
			{
				for (int i = 0; i < this._phonySchemaTypes.Count; i++)
				{
					WsdlGenerator.PhonySchemaType phonySchemaType = (WsdlGenerator.PhonySchemaType)this._phonySchemaTypes[i];
					if (phonySchemaType.Name == name)
					{
						return phonySchemaType;
					}
				}
				return null;
			}

			// Token: 0x0600056D RID: 1389 RVA: 0x0001F8C0 File Offset: 0x0001E8C0
			internal void AddPhonySchemaType(WsdlGenerator.PhonySchemaType phType)
			{
				WsdlGenerator.PhonySchemaType phonySchemaType = this.LookupPhonySchemaType(phType.Name);
				if (phonySchemaType != null)
				{
					phType.ElementName = phType.Name + phonySchemaType.OverloadedType();
				}
				this._phonySchemaTypes.Add(phType);
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x0001F908 File Offset: 0x0001E908
			internal WsdlGenerator.XMLNamespace LookupSchemaNamespace(string ns, Assembly assem)
			{
				for (int i = 0; i < this._dependsOnSchemaNS.Count; i++)
				{
					WsdlGenerator.XMLNamespace xmlnamespace = (WsdlGenerator.XMLNamespace)this._dependsOnSchemaNS[i];
					if (xmlnamespace.Name == ns && xmlnamespace.Assem == assem)
					{
						return xmlnamespace;
					}
				}
				return null;
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x0001F958 File Offset: 0x0001E958
			internal void DependsOnSchemaNS(WsdlGenerator.XMLNamespace xns, bool bImport)
			{
				if (this.LookupSchemaNamespace(xns.Name, xns.Assem) != null)
				{
					return;
				}
				this._dependsOnSchemaNS.Add(xns);
				if (bImport && this.Namespace != xns.Namespace)
				{
					this._xnsImports.Add(xns);
				}
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x0001F9AC File Offset: 0x0001E9AC
			private WsdlGenerator.XMLNamespace LookupSUDSNamespace(string ns, Assembly assem)
			{
				for (int i = 0; i < this._dependsOnSUDSNS.Count; i++)
				{
					WsdlGenerator.XMLNamespace xmlnamespace = (WsdlGenerator.XMLNamespace)this._dependsOnSUDSNS[i];
					if (xmlnamespace.Name == ns && xmlnamespace.Assem == assem)
					{
						return xmlnamespace;
					}
				}
				return null;
			}

			// Token: 0x06000571 RID: 1393 RVA: 0x0001F9FB File Offset: 0x0001E9FB
			internal void DependsOnSUDSNS(WsdlGenerator.XMLNamespace xns)
			{
				if (this.LookupSUDSNamespace(xns.Name, xns.Assem) != null)
				{
					return;
				}
				this._dependsOnSUDSNS.Add(xns);
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x0001FA20 File Offset: 0x0001EA20
			internal void Resolve()
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				for (int i = 0; i < this._realSchemaTypes.Count; i++)
				{
					((WsdlGenerator.RealSchemaType)this._realSchemaTypes[i]).Resolve(stringBuilder);
				}
			}

			// Token: 0x06000573 RID: 1395 RVA: 0x0001FA68 File Offset: 0x0001EA68
			internal void PrintDependsOnWsdl(TextWriter textWriter, StringBuilder sb, string indent, Hashtable usedNames)
			{
				if (this._dependsOnSchemaNS.Count > 0)
				{
					for (int i = 0; i < this._dependsOnSchemaNS.Count; i++)
					{
						WsdlGenerator.XMLNamespace xmlnamespace = (WsdlGenerator.XMLNamespace)this._dependsOnSchemaNS[i];
						if (!usedNames.ContainsKey(xmlnamespace.Prefix))
						{
							usedNames[xmlnamespace.Prefix] = null;
							sb.Length = 0;
							sb.Append(indent);
							sb.Append("xmlns:");
							sb.Append(xmlnamespace.Prefix);
							sb.Append("='");
							sb.Append(xmlnamespace.Namespace);
							sb.Append("'");
							textWriter.WriteLine(sb);
						}
					}
				}
			}

			// Token: 0x06000574 RID: 1396 RVA: 0x0001FB28 File Offset: 0x0001EB28
			internal void PrintSchemaWsdl(TextWriter textWriter, StringBuilder sb, string indent)
			{
				bool flag = false;
				if (this._simpleSchemaTypes.Count > 0 || this._realSchemaTypes.Count > 0 || this._arraySchemaTypes.Count > 0)
				{
					flag = true;
				}
				if (flag)
				{
					string text = WsdlGenerator.IndentP(indent);
					string text2 = WsdlGenerator.IndentP(text);
					string text3 = WsdlGenerator.IndentP(text2);
					WsdlGenerator.IndentP(text3);
					sb.Length = 0;
					sb.Append(indent);
					sb.Append("<schema ");
					sb.Append("targetNamespace='");
					sb.Append(this.Namespace);
					sb.Append("'");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text2);
					sb.Append("xmlns='");
					sb.Append(SudsConverter.GetXsdVersion(this._generator._xsdVersion));
					sb.Append("'");
					textWriter.WriteLine(sb);
					sb.Length = 0;
					sb.Append(text2);
					sb.Append("elementFormDefault='unqualified' attributeFormDefault='unqualified'>");
					textWriter.WriteLine(sb);
					foreach (object obj in this._xnsImports)
					{
						WsdlGenerator.XMLNamespace xmlnamespace = (WsdlGenerator.XMLNamespace)obj;
						sb.Length = 0;
						sb.Append(text);
						sb.Append("<import namespace='");
						sb.Append(xmlnamespace.Namespace);
						sb.Append("'/>");
						textWriter.WriteLine(sb);
					}
					for (int i = 0; i < this._simpleSchemaTypes.Count; i++)
					{
						WsdlGenerator.SimpleSchemaType simpleSchemaType = (WsdlGenerator.SimpleSchemaType)this._simpleSchemaTypes[i];
						simpleSchemaType.PrintSchemaType(textWriter, sb, text, false);
					}
					for (int j = 0; j < this._realSchemaTypes.Count; j++)
					{
						WsdlGenerator.RealSchemaType realSchemaType = (WsdlGenerator.RealSchemaType)this._realSchemaTypes[j];
						if (!realSchemaType.Type.IsInterface && !realSchemaType.IsSUDSType)
						{
							realSchemaType.PrintSchemaType(textWriter, sb, text, false);
						}
					}
					for (int k = 0; k < this._arraySchemaTypes.Count; k++)
					{
						WsdlGenerator.ArraySchemaType arraySchemaType = (WsdlGenerator.ArraySchemaType)this._arraySchemaTypes[k];
						arraySchemaType.PrintSchemaType(textWriter, sb, text, false);
					}
					sb.Length = 0;
					sb.Append(indent);
					sb.Append("</schema>");
					textWriter.WriteLine(sb);
				}
			}

			// Token: 0x06000575 RID: 1397 RVA: 0x0001FDA0 File Offset: 0x0001EDA0
			internal void PrintMessageWsdl(TextWriter textWriter, StringBuilder sb, string indent, ArrayList refNames)
			{
				for (int i = 0; i < this._realSUDSTypes.Count; i++)
				{
					((WsdlGenerator.RealSchemaType)this._realSUDSTypes[i]).PrintMessageWsdl(textWriter, sb, indent, refNames);
				}
				if (this._realSUDSTypes.Count == 0 && this._realSchemaTypes.Count > 0)
				{
					((WsdlGenerator.RealSchemaType)this._realSchemaTypes[0]).PrintMessageWsdl(textWriter, sb, indent, new ArrayList());
				}
			}

			// Token: 0x04000499 RID: 1177
			private string _name;

			// Token: 0x0400049A RID: 1178
			private Assembly _assem;

			// Token: 0x0400049B RID: 1179
			private string _namespace;

			// Token: 0x0400049C RID: 1180
			private string _prefix;

			// Token: 0x0400049D RID: 1181
			internal bool _bUnique;

			// Token: 0x0400049E RID: 1182
			private ArrayList _dependsOnSUDSNS;

			// Token: 0x0400049F RID: 1183
			private ArrayList _realSUDSTypes;

			// Token: 0x040004A0 RID: 1184
			private ArrayList _dependsOnSchemaNS;

			// Token: 0x040004A1 RID: 1185
			internal ArrayList _realSchemaTypes;

			// Token: 0x040004A2 RID: 1186
			private ArrayList _phonySchemaTypes;

			// Token: 0x040004A3 RID: 1187
			private ArrayList _simpleSchemaTypes;

			// Token: 0x040004A4 RID: 1188
			private ArrayList _arraySchemaTypes;

			// Token: 0x040004A5 RID: 1189
			private bool _bInteropType;

			// Token: 0x040004A6 RID: 1190
			private string _serviceEndpoint;

			// Token: 0x040004A7 RID: 1191
			private Hashtable _typeToServiceEndpoint;

			// Token: 0x040004A8 RID: 1192
			private WsdlGenerator _generator;

			// Token: 0x040004A9 RID: 1193
			private ArrayList _xnsImports;

			// Token: 0x040004AA RID: 1194
			private bool _bClassesPrinted;
		}
	}
}
