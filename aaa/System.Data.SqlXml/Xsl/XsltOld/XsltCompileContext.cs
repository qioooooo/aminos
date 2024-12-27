using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x020001A1 RID: 417
	internal class XsltCompileContext : XsltContext
	{
		// Token: 0x06001189 RID: 4489 RVA: 0x00054859 File Offset: 0x00053859
		internal XsltCompileContext(InputScopeManager manager, Processor processor)
			: base(false)
		{
			this.manager = manager;
			this.processor = processor;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00054870 File Offset: 0x00053870
		internal XsltCompileContext()
			: base(false)
		{
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00054879 File Offset: 0x00053879
		internal void Recycle()
		{
			this.manager = null;
			this.processor = null;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00054889 File Offset: 0x00053889
		internal void Reinitialize(InputScopeManager manager, Processor processor)
		{
			this.manager = manager;
			this.processor = processor;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00054899 File Offset: 0x00053899
		public override int CompareDocument(string baseUri, string nextbaseUri)
		{
			return string.Compare(baseUri, nextbaseUri, StringComparison.Ordinal);
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x000548A3 File Offset: 0x000538A3
		public override string DefaultNamespace
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x000548AA File Offset: 0x000538AA
		public override string LookupNamespace(string prefix)
		{
			return this.manager.ResolveXPathNamespace(prefix);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x000548B8 File Offset: 0x000538B8
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			string text = this.LookupNamespace(prefix);
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, text);
			IXsltContextVariable xsltContextVariable = this.manager.VariableScope.ResolveVariable(xmlQualifiedName);
			if (xsltContextVariable == null)
			{
				throw XsltException.Create("Xslt_InvalidVariable", new string[] { xmlQualifiedName.ToString() });
			}
			return xsltContextVariable;
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00054908 File Offset: 0x00053908
		internal object EvaluateVariable(VariableAction variable)
		{
			object obj = this.processor.GetVariableValue(variable);
			if (obj == null && !variable.IsGlobal)
			{
				VariableAction variableAction = this.manager.VariableScope.ResolveGlobalVariable(variable.Name);
				if (variableAction != null)
				{
					obj = this.processor.GetVariableValue(variableAction);
				}
			}
			if (obj == null)
			{
				throw XsltException.Create("Xslt_InvalidVariable", new string[] { variable.Name.ToString() });
			}
			return obj;
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x00054979 File Offset: 0x00053979
		public override bool Whitespace
		{
			get
			{
				return this.processor.Stylesheet.Whitespace;
			}
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0005498B File Offset: 0x0005398B
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			node = node.Clone();
			node.MoveToParent();
			return this.processor.Stylesheet.PreserveWhiteSpace(this.processor, node);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x000549B4 File Offset: 0x000539B4
		private MethodInfo FindBestMethod(MethodInfo[] methods, bool ignoreCase, bool publicOnly, string name, XPathResultType[] argTypes)
		{
			int num = methods.Length;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (string.Compare(name, methods[i].Name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0 && (!publicOnly || methods[i].GetBaseDefinition().IsPublic))
				{
					methods[num2++] = methods[i];
				}
			}
			num = num2;
			if (num == 0)
			{
				return null;
			}
			if (argTypes == null)
			{
				return methods[0];
			}
			num2 = 0;
			for (int j = 0; j < num; j++)
			{
				if (methods[j].GetParameters().Length == argTypes.Length)
				{
					methods[num2++] = methods[j];
				}
			}
			num = num2;
			if (num <= 1)
			{
				return methods[0];
			}
			num2 = 0;
			for (int k = 0; k < num; k++)
			{
				bool flag = true;
				ParameterInfo[] parameters = methods[k].GetParameters();
				for (int l = 0; l < parameters.Length; l++)
				{
					XPathResultType xpathResultType = argTypes[l];
					if (xpathResultType != XPathResultType.Any)
					{
						XPathResultType xpathType = XsltCompileContext.GetXPathType(parameters[l].ParameterType);
						if (xpathType != xpathResultType && xpathType != XPathResultType.Any)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					methods[num2++] = methods[k];
				}
			}
			return methods[0];
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00054ABC File Offset: 0x00053ABC
		private IXsltContextFunction GetExtentionMethod(string ns, string name, XPathResultType[] argTypes, out object extension)
		{
			XsltCompileContext.FuncExtension funcExtension = null;
			extension = this.processor.GetScriptObject(ns);
			if (extension != null)
			{
				MethodInfo methodInfo = this.FindBestMethod(extension.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), true, false, name, argTypes);
				if (methodInfo != null)
				{
					funcExtension = new XsltCompileContext.FuncExtension(extension, methodInfo, null);
				}
				return funcExtension;
			}
			extension = this.processor.GetExtensionObject(ns);
			if (extension != null)
			{
				MethodInfo methodInfo2 = this.FindBestMethod(extension.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), false, true, name, argTypes);
				if (methodInfo2 != null)
				{
					funcExtension = new XsltCompileContext.FuncExtension(extension, methodInfo2, this.processor.permissions);
				}
				return funcExtension;
			}
			return null;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00054B54 File Offset: 0x00053B54
		public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
		{
			IXsltContextFunction xsltContextFunction;
			if (prefix.Length == 0)
			{
				xsltContextFunction = XsltCompileContext.s_FunctionTable[name] as IXsltContextFunction;
			}
			else
			{
				string text = this.LookupNamespace(prefix);
				if (text == "urn:schemas-microsoft-com:xslt" && name == "node-set")
				{
					xsltContextFunction = XsltCompileContext.s_FuncNodeSet;
				}
				else
				{
					object obj;
					xsltContextFunction = this.GetExtentionMethod(text, name, argTypes, out obj);
					if (obj == null)
					{
						throw XsltException.Create("Xslt_ScriptInvalidPrefix", new string[] { prefix });
					}
				}
			}
			if (xsltContextFunction == null)
			{
				throw XsltException.Create("Xslt_UnknownXsltFunction", new string[] { name });
			}
			if (argTypes.Length < xsltContextFunction.Minargs || xsltContextFunction.Maxargs < argTypes.Length)
			{
				throw XsltException.Create("Xslt_WrongNumberArgs", new string[]
				{
					name,
					argTypes.Length.ToString(CultureInfo.InvariantCulture)
				});
			}
			return xsltContextFunction;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00054C34 File Offset: 0x00053C34
		private Uri ComposeUri(string thisUri, string baseUri)
		{
			XmlResolver resolver = this.processor.Resolver;
			Uri uri = null;
			if (baseUri.Length != 0)
			{
				uri = resolver.ResolveUri(null, baseUri);
			}
			return resolver.ResolveUri(uri, thisUri);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00054C68 File Offset: 0x00053C68
		private XPathNodeIterator Document(object arg0, string baseUri)
		{
			if (this.processor.permissions != null)
			{
				this.processor.permissions.PermitOnly();
			}
			XPathNodeIterator xpathNodeIterator = arg0 as XPathNodeIterator;
			if (xpathNodeIterator != null)
			{
				ArrayList arrayList = new ArrayList();
				Hashtable hashtable = new Hashtable();
				while (xpathNodeIterator.MoveNext())
				{
					Uri uri = this.ComposeUri(xpathNodeIterator.Current.Value, baseUri ?? xpathNodeIterator.Current.BaseURI);
					if (!hashtable.ContainsKey(uri))
					{
						hashtable.Add(uri, null);
						arrayList.Add(this.processor.GetNavigator(uri));
					}
				}
				return new XPathArrayIterator(arrayList);
			}
			return new XPathSingletonIterator(this.processor.GetNavigator(this.ComposeUri(XmlConvert.ToXPathString(arg0), baseUri ?? this.manager.Navigator.BaseURI)));
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00054D30 File Offset: 0x00053D30
		private Hashtable BuildKeyTable(Key key, XPathNavigator root)
		{
			Hashtable hashtable = new Hashtable();
			string queryExpression = this.processor.GetQueryExpression(key.MatchKey);
			Query compiledQuery = this.processor.GetCompiledQuery(key.MatchKey);
			Query compiledQuery2 = this.processor.GetCompiledQuery(key.UseKey);
			XPathNodeIterator xpathNodeIterator = root.SelectDescendants(XPathNodeType.All, false);
			while (xpathNodeIterator.MoveNext())
			{
				XPathNavigator xpathNavigator = xpathNodeIterator.Current;
				XsltCompileContext.EvaluateKey(xpathNavigator, compiledQuery, queryExpression, compiledQuery2, hashtable);
				if (xpathNavigator.MoveToFirstAttribute())
				{
					do
					{
						XsltCompileContext.EvaluateKey(xpathNavigator, compiledQuery, queryExpression, compiledQuery2, hashtable);
					}
					while (xpathNavigator.MoveToNextAttribute());
					xpathNavigator.MoveToParent();
				}
			}
			return hashtable;
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00054DCC File Offset: 0x00053DCC
		private static void AddKeyValue(Hashtable keyTable, string key, XPathNavigator value, bool checkDuplicates)
		{
			ArrayList arrayList = (ArrayList)keyTable[key];
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				keyTable.Add(key, arrayList);
			}
			else if (checkDuplicates && value.ComparePosition((XPathNavigator)arrayList[arrayList.Count - 1]) == XmlNodeOrder.Same)
			{
				return;
			}
			arrayList.Add(value.Clone());
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00054E28 File Offset: 0x00053E28
		private static void EvaluateKey(XPathNavigator node, Query matchExpr, string matchStr, Query useExpr, Hashtable keyTable)
		{
			try
			{
				if (matchExpr.MatchNode(node) == null)
				{
					return;
				}
			}
			catch (XPathException)
			{
				throw XsltException.Create("Xslt_InvalidPattern", new string[] { matchStr });
			}
			object obj = useExpr.Evaluate(new XPathSingletonIterator(node, true));
			XPathNodeIterator xpathNodeIterator = obj as XPathNodeIterator;
			if (xpathNodeIterator != null)
			{
				bool flag = false;
				while (xpathNodeIterator.MoveNext())
				{
					XPathNavigator xpathNavigator = xpathNodeIterator.Current;
					XsltCompileContext.AddKeyValue(keyTable, xpathNavigator.Value, node, flag);
					flag = true;
				}
				return;
			}
			string text = XmlConvert.ToXPathString(obj);
			XsltCompileContext.AddKeyValue(keyTable, text, node, false);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00054EBC File Offset: 0x00053EBC
		private DecimalFormat ResolveFormatName(string formatName)
		{
			string text = string.Empty;
			string empty = string.Empty;
			if (formatName != null)
			{
				string text2;
				PrefixQName.ParseQualifiedName(formatName, out text2, out empty);
				text = this.LookupNamespace(text2);
			}
			DecimalFormat decimalFormat = this.processor.RootAction.GetDecimalFormat(new XmlQualifiedName(empty, text));
			if (decimalFormat == null)
			{
				if (formatName != null)
				{
					throw XsltException.Create("Xslt_NoDecimalFormat", new string[] { formatName });
				}
				decimalFormat = new DecimalFormat(new NumberFormatInfo(), '#', '0', ';');
			}
			return decimalFormat;
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00054F34 File Offset: 0x00053F34
		private bool ElementAvailable(string qname)
		{
			string text;
			string text2;
			PrefixQName.ParseQualifiedName(qname, out text, out text2);
			string text3 = this.manager.ResolveXmlNamespace(text);
			return text3 == "http://www.w3.org/1999/XSL/Transform" && (text2 == "apply-imports" || text2 == "apply-templates" || text2 == "attribute" || text2 == "call-template" || text2 == "choose" || text2 == "comment" || text2 == "copy" || text2 == "copy-of" || text2 == "element" || text2 == "fallback" || text2 == "for-each" || text2 == "if" || text2 == "message" || text2 == "number" || text2 == "processing-instruction" || text2 == "text" || text2 == "value-of" || text2 == "variable");
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0005506C File Offset: 0x0005406C
		private bool FunctionAvailable(string qname)
		{
			string text;
			string text2;
			PrefixQName.ParseQualifiedName(qname, out text, out text2);
			string text3 = this.LookupNamespace(text);
			if (text3 == "urn:schemas-microsoft-com:xslt")
			{
				return text2 == "node-set";
			}
			if (text3.Length == 0)
			{
				return text2 == "last" || text2 == "position" || text2 == "name" || text2 == "namespace-uri" || text2 == "local-name" || text2 == "count" || text2 == "id" || text2 == "string" || text2 == "concat" || text2 == "starts-with" || text2 == "contains" || text2 == "substring-before" || text2 == "substring-after" || text2 == "substring" || text2 == "string-length" || text2 == "normalize-space" || text2 == "translate" || text2 == "boolean" || text2 == "not" || text2 == "true" || text2 == "false" || text2 == "lang" || text2 == "number" || text2 == "sum" || text2 == "floor" || text2 == "ceiling" || text2 == "round" || (XsltCompileContext.s_FunctionTable[text2] != null && text2 != "unparsed-entity-uri");
			}
			object obj;
			return this.GetExtentionMethod(text3, text2, null, out obj) != null;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00055278 File Offset: 0x00054278
		private XPathNodeIterator Current()
		{
			XPathNavigator xpathNavigator = this.processor.Current;
			if (xpathNavigator != null)
			{
				return new XPathSingletonIterator(xpathNavigator.Clone());
			}
			return XPathEmptyIterator.Instance;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x000552A8 File Offset: 0x000542A8
		private string SystemProperty(string qname)
		{
			string text = string.Empty;
			string text2;
			string text3;
			PrefixQName.ParseQualifiedName(qname, out text2, out text3);
			string text4 = this.LookupNamespace(text2);
			if (text4 == "http://www.w3.org/1999/XSL/Transform")
			{
				if (text3 == "version")
				{
					text = "1";
				}
				else if (text3 == "vendor")
				{
					text = "Microsoft";
				}
				else if (text3 == "vendor-url")
				{
					text = "http://www.microsoft.com";
				}
				return text;
			}
			if (text4 == null && text2 != null)
			{
				throw XsltException.Create("Xslt_InvalidPrefix", new string[] { text2 });
			}
			return string.Empty;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00055340 File Offset: 0x00054340
		public static XPathResultType GetXPathType(Type type)
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			switch (typeCode)
			{
			case TypeCode.Object:
				if (typeof(XPathNavigator).IsAssignableFrom(type) || typeof(IXPathNavigable).IsAssignableFrom(type))
				{
					return XPathResultType.String;
				}
				if (typeof(XPathNodeIterator).IsAssignableFrom(type))
				{
					return XPathResultType.NodeSet;
				}
				return XPathResultType.Any;
			case TypeCode.DBNull:
				break;
			case TypeCode.Boolean:
				return XPathResultType.Boolean;
			default:
				switch (typeCode)
				{
				case TypeCode.DateTime:
					return XPathResultType.Error;
				case TypeCode.String:
					return XPathResultType.String;
				}
				break;
			}
			return XPathResultType.Number;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000553C4 File Offset: 0x000543C4
		private static Hashtable CreateFunctionTable()
		{
			Hashtable hashtable = new Hashtable(10);
			hashtable["current"] = new XsltCompileContext.FuncCurrent();
			hashtable["unparsed-entity-uri"] = new XsltCompileContext.FuncUnEntityUri();
			hashtable["generate-id"] = new XsltCompileContext.FuncGenerateId();
			hashtable["system-property"] = new XsltCompileContext.FuncSystemProp();
			hashtable["element-available"] = new XsltCompileContext.FuncElementAvailable();
			hashtable["function-available"] = new XsltCompileContext.FuncFunctionAvailable();
			hashtable["document"] = new XsltCompileContext.FuncDocument();
			hashtable["key"] = new XsltCompileContext.FuncKey();
			hashtable["format-number"] = new XsltCompileContext.FuncFormatNumber();
			return hashtable;
		}

		// Token: 0x04000BD8 RID: 3032
		private const string f_NodeSet = "node-set";

		// Token: 0x04000BD9 RID: 3033
		private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x04000BDA RID: 3034
		private InputScopeManager manager;

		// Token: 0x04000BDB RID: 3035
		private Processor processor;

		// Token: 0x04000BDC RID: 3036
		private static Hashtable s_FunctionTable = XsltCompileContext.CreateFunctionTable();

		// Token: 0x04000BDD RID: 3037
		private static IXsltContextFunction s_FuncNodeSet = new XsltCompileContext.FuncNodeSet();

		// Token: 0x020001A2 RID: 418
		private abstract class XsltFunctionImpl : IXsltContextFunction
		{
			// Token: 0x060011A4 RID: 4516 RVA: 0x00055480 File Offset: 0x00054480
			public XsltFunctionImpl()
			{
			}

			// Token: 0x060011A5 RID: 4517 RVA: 0x00055488 File Offset: 0x00054488
			public XsltFunctionImpl(int minArgs, int maxArgs, XPathResultType returnType, XPathResultType[] argTypes)
			{
				this.Init(minArgs, maxArgs, returnType, argTypes);
			}

			// Token: 0x060011A6 RID: 4518 RVA: 0x0005549B File Offset: 0x0005449B
			protected void Init(int minArgs, int maxArgs, XPathResultType returnType, XPathResultType[] argTypes)
			{
				this.minargs = minArgs;
				this.maxargs = maxArgs;
				this.returnType = returnType;
				this.argTypes = argTypes;
			}

			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x060011A7 RID: 4519 RVA: 0x000554BA File Offset: 0x000544BA
			public int Minargs
			{
				get
				{
					return this.minargs;
				}
			}

			// Token: 0x170002C1 RID: 705
			// (get) Token: 0x060011A8 RID: 4520 RVA: 0x000554C2 File Offset: 0x000544C2
			public int Maxargs
			{
				get
				{
					return this.maxargs;
				}
			}

			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x060011A9 RID: 4521 RVA: 0x000554CA File Offset: 0x000544CA
			public XPathResultType ReturnType
			{
				get
				{
					return this.returnType;
				}
			}

			// Token: 0x170002C3 RID: 707
			// (get) Token: 0x060011AA RID: 4522 RVA: 0x000554D2 File Offset: 0x000544D2
			public XPathResultType[] ArgTypes
			{
				get
				{
					return this.argTypes;
				}
			}

			// Token: 0x060011AB RID: 4523
			public abstract object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext);

			// Token: 0x060011AC RID: 4524 RVA: 0x000554DC File Offset: 0x000544DC
			public static XPathNodeIterator ToIterator(object argument)
			{
				XPathNodeIterator xpathNodeIterator = argument as XPathNodeIterator;
				if (xpathNodeIterator == null)
				{
					throw XsltException.Create("Xslt_NoNodeSetConversion", new string[0]);
				}
				return xpathNodeIterator;
			}

			// Token: 0x060011AD RID: 4525 RVA: 0x00055508 File Offset: 0x00054508
			public static XPathNavigator ToNavigator(object argument)
			{
				XPathNavigator xpathNavigator = argument as XPathNavigator;
				if (xpathNavigator == null)
				{
					throw XsltException.Create("Xslt_NoNavigatorConversion", new string[0]);
				}
				return xpathNavigator;
			}

			// Token: 0x060011AE RID: 4526 RVA: 0x00055531 File Offset: 0x00054531
			private static string IteratorToString(XPathNodeIterator it)
			{
				if (it.MoveNext())
				{
					return it.Current.Value;
				}
				return string.Empty;
			}

			// Token: 0x060011AF RID: 4527 RVA: 0x0005554C File Offset: 0x0005454C
			public static string ToString(object argument)
			{
				XPathNodeIterator xpathNodeIterator = argument as XPathNodeIterator;
				if (xpathNodeIterator != null)
				{
					return XsltCompileContext.XsltFunctionImpl.IteratorToString(xpathNodeIterator);
				}
				return XmlConvert.ToXPathString(argument);
			}

			// Token: 0x060011B0 RID: 4528 RVA: 0x00055570 File Offset: 0x00054570
			public static bool ToBoolean(object argument)
			{
				XPathNodeIterator xpathNodeIterator = argument as XPathNodeIterator;
				if (xpathNodeIterator != null)
				{
					return Convert.ToBoolean(XsltCompileContext.XsltFunctionImpl.IteratorToString(xpathNodeIterator), CultureInfo.InvariantCulture);
				}
				XPathNavigator xpathNavigator = argument as XPathNavigator;
				if (xpathNavigator != null)
				{
					return Convert.ToBoolean(xpathNavigator.ToString(), CultureInfo.InvariantCulture);
				}
				return Convert.ToBoolean(argument, CultureInfo.InvariantCulture);
			}

			// Token: 0x060011B1 RID: 4529 RVA: 0x000555C0 File Offset: 0x000545C0
			public static double ToNumber(object argument)
			{
				XPathNodeIterator xpathNodeIterator = argument as XPathNodeIterator;
				if (xpathNodeIterator != null)
				{
					return XmlConvert.ToXPathDouble(XsltCompileContext.XsltFunctionImpl.IteratorToString(xpathNodeIterator));
				}
				XPathNavigator xpathNavigator = argument as XPathNavigator;
				if (xpathNavigator != null)
				{
					return XmlConvert.ToXPathDouble(xpathNavigator.ToString());
				}
				return XmlConvert.ToXPathDouble(argument);
			}

			// Token: 0x060011B2 RID: 4530 RVA: 0x000555FF File Offset: 0x000545FF
			private static object ToNumeric(object argument, TypeCode typeCode)
			{
				return Convert.ChangeType(XsltCompileContext.XsltFunctionImpl.ToNumber(argument), typeCode, CultureInfo.InvariantCulture);
			}

			// Token: 0x060011B3 RID: 4531 RVA: 0x00055618 File Offset: 0x00054618
			public static object ConvertToXPathType(object val, XPathResultType xt, TypeCode typeCode)
			{
				switch (xt)
				{
				case XPathResultType.Number:
					return XsltCompileContext.XsltFunctionImpl.ToNumeric(val, typeCode);
				case XPathResultType.String:
					if (typeCode == TypeCode.String)
					{
						return XsltCompileContext.XsltFunctionImpl.ToString(val);
					}
					return XsltCompileContext.XsltFunctionImpl.ToNavigator(val);
				case XPathResultType.Boolean:
					return XsltCompileContext.XsltFunctionImpl.ToBoolean(val);
				case XPathResultType.NodeSet:
					return XsltCompileContext.XsltFunctionImpl.ToIterator(val);
				case XPathResultType.Any:
				case XPathResultType.Error:
					return val;
				}
				return val;
			}

			// Token: 0x04000BDE RID: 3038
			private int minargs;

			// Token: 0x04000BDF RID: 3039
			private int maxargs;

			// Token: 0x04000BE0 RID: 3040
			private XPathResultType returnType;

			// Token: 0x04000BE1 RID: 3041
			private XPathResultType[] argTypes;
		}

		// Token: 0x020001A3 RID: 419
		private class FuncCurrent : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011B4 RID: 4532 RVA: 0x0005567C File Offset: 0x0005467C
			public FuncCurrent()
				: base(0, 0, XPathResultType.NodeSet, new XPathResultType[0])
			{
			}

			// Token: 0x060011B5 RID: 4533 RVA: 0x0005568D File Offset: 0x0005468D
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				return ((XsltCompileContext)xsltContext).Current();
			}
		}

		// Token: 0x020001A4 RID: 420
		private class FuncUnEntityUri : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011B6 RID: 4534 RVA: 0x0005569C File Offset: 0x0005469C
			public FuncUnEntityUri()
				: base(1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String })
			{
			}

			// Token: 0x060011B7 RID: 4535 RVA: 0x000556C0 File Offset: 0x000546C0
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				throw XsltException.Create("Xslt_UnsuppFunction", new string[] { "unparsed-entity-uri" });
			}
		}

		// Token: 0x020001A5 RID: 421
		private class FuncGenerateId : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011B8 RID: 4536 RVA: 0x000556E8 File Offset: 0x000546E8
			public FuncGenerateId()
				: base(0, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.NodeSet })
			{
			}

			// Token: 0x060011B9 RID: 4537 RVA: 0x0005570C File Offset: 0x0005470C
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				if (args.Length <= 0)
				{
					return docContext.UniqueId;
				}
				XPathNodeIterator xpathNodeIterator = XsltCompileContext.XsltFunctionImpl.ToIterator(args[0]);
				if (xpathNodeIterator.MoveNext())
				{
					return xpathNodeIterator.Current.UniqueId;
				}
				return string.Empty;
			}
		}

		// Token: 0x020001A6 RID: 422
		private class FuncSystemProp : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011BA RID: 4538 RVA: 0x00055748 File Offset: 0x00054748
			public FuncSystemProp()
				: base(1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String })
			{
			}

			// Token: 0x060011BB RID: 4539 RVA: 0x0005576A File Offset: 0x0005476A
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				return ((XsltCompileContext)xsltContext).SystemProperty(XsltCompileContext.XsltFunctionImpl.ToString(args[0]));
			}
		}

		// Token: 0x020001A7 RID: 423
		private class FuncElementAvailable : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011BC RID: 4540 RVA: 0x00055780 File Offset: 0x00054780
			public FuncElementAvailable()
				: base(1, 1, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.String })
			{
			}

			// Token: 0x060011BD RID: 4541 RVA: 0x000557A2 File Offset: 0x000547A2
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				return ((XsltCompileContext)xsltContext).ElementAvailable(XsltCompileContext.XsltFunctionImpl.ToString(args[0]));
			}
		}

		// Token: 0x020001A8 RID: 424
		private class FuncFunctionAvailable : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011BE RID: 4542 RVA: 0x000557BC File Offset: 0x000547BC
			public FuncFunctionAvailable()
				: base(1, 1, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.String })
			{
			}

			// Token: 0x060011BF RID: 4543 RVA: 0x000557DE File Offset: 0x000547DE
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				return ((XsltCompileContext)xsltContext).FunctionAvailable(XsltCompileContext.XsltFunctionImpl.ToString(args[0]));
			}
		}

		// Token: 0x020001A9 RID: 425
		private class FuncDocument : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011C0 RID: 4544 RVA: 0x000557F8 File Offset: 0x000547F8
			public FuncDocument()
				: base(1, 2, XPathResultType.NodeSet, new XPathResultType[]
				{
					XPathResultType.Any,
					XPathResultType.NodeSet
				})
			{
			}

			// Token: 0x060011C1 RID: 4545 RVA: 0x00055820 File Offset: 0x00054820
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				string text = null;
				if (args.Length == 2)
				{
					XPathNodeIterator xpathNodeIterator = XsltCompileContext.XsltFunctionImpl.ToIterator(args[1]);
					if (xpathNodeIterator.MoveNext())
					{
						text = xpathNodeIterator.Current.BaseURI;
					}
					else
					{
						text = string.Empty;
					}
				}
				object obj;
				try
				{
					obj = ((XsltCompileContext)xsltContext).Document(args[0], text);
				}
				catch (Exception ex)
				{
					if (!XmlException.IsCatchableException(ex))
					{
						throw;
					}
					obj = XPathEmptyIterator.Instance;
				}
				return obj;
			}
		}

		// Token: 0x020001AA RID: 426
		private class FuncKey : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011C2 RID: 4546 RVA: 0x00055890 File Offset: 0x00054890
			public FuncKey()
				: base(2, 2, XPathResultType.NodeSet, new XPathResultType[]
				{
					XPathResultType.String,
					XPathResultType.Any
				})
			{
			}

			// Token: 0x060011C3 RID: 4547 RVA: 0x000558B8 File Offset: 0x000548B8
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				XsltCompileContext xsltCompileContext = (XsltCompileContext)xsltContext;
				string text;
				string text2;
				PrefixQName.ParseQualifiedName(XsltCompileContext.XsltFunctionImpl.ToString(args[0]), out text, out text2);
				string text3 = xsltContext.LookupNamespace(text);
				XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(text2, text3);
				XPathNavigator xpathNavigator = docContext.Clone();
				xpathNavigator.MoveToRoot();
				ArrayList arrayList = null;
				foreach (Key key in xsltCompileContext.processor.KeyList)
				{
					if (key.Name == xmlQualifiedName)
					{
						Hashtable hashtable = key.GetKeys(xpathNavigator);
						if (hashtable == null)
						{
							hashtable = xsltCompileContext.BuildKeyTable(key, xpathNavigator);
							key.AddKey(xpathNavigator, hashtable);
						}
						XPathNodeIterator xpathNodeIterator = args[1] as XPathNodeIterator;
						if (xpathNodeIterator != null)
						{
							xpathNodeIterator = xpathNodeIterator.Clone();
							while (xpathNodeIterator.MoveNext())
							{
								XPathNavigator xpathNavigator2 = xpathNodeIterator.Current;
								arrayList = XsltCompileContext.FuncKey.AddToList(arrayList, (ArrayList)hashtable[xpathNavigator2.Value]);
							}
						}
						else
						{
							arrayList = XsltCompileContext.FuncKey.AddToList(arrayList, (ArrayList)hashtable[XsltCompileContext.XsltFunctionImpl.ToString(args[1])]);
						}
					}
				}
				if (arrayList == null)
				{
					return XPathEmptyIterator.Instance;
				}
				if (arrayList[0] is XPathNavigator)
				{
					return new XPathArrayIterator(arrayList);
				}
				return new XPathMultyIterator(arrayList);
			}

			// Token: 0x060011C4 RID: 4548 RVA: 0x000559F0 File Offset: 0x000549F0
			private static ArrayList AddToList(ArrayList resultCollection, ArrayList newList)
			{
				if (newList == null)
				{
					return resultCollection;
				}
				if (resultCollection == null)
				{
					return newList;
				}
				if (!(resultCollection[0] is ArrayList))
				{
					ArrayList arrayList = resultCollection;
					resultCollection = new ArrayList();
					resultCollection.Add(arrayList);
				}
				resultCollection.Add(newList);
				return resultCollection;
			}
		}

		// Token: 0x020001AB RID: 427
		private class FuncFormatNumber : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011C5 RID: 4549 RVA: 0x00055A30 File Offset: 0x00054A30
			public FuncFormatNumber()
				: base(2, 3, XPathResultType.String, new XPathResultType[]
				{
					XPathResultType.Number,
					XPathResultType.String,
					XPathResultType.String
				})
			{
			}

			// Token: 0x060011C6 RID: 4550 RVA: 0x00055A58 File Offset: 0x00054A58
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				DecimalFormat decimalFormat = ((XsltCompileContext)xsltContext).ResolveFormatName((args.Length == 3) ? XsltCompileContext.XsltFunctionImpl.ToString(args[2]) : null);
				return DecimalFormatter.Format(XsltCompileContext.XsltFunctionImpl.ToNumber(args[0]), XsltCompileContext.XsltFunctionImpl.ToString(args[1]), decimalFormat);
			}
		}

		// Token: 0x020001AC RID: 428
		private class FuncNodeSet : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011C7 RID: 4551 RVA: 0x00055A98 File Offset: 0x00054A98
			public FuncNodeSet()
				: base(1, 1, XPathResultType.NodeSet, new XPathResultType[] { XPathResultType.String })
			{
			}

			// Token: 0x060011C8 RID: 4552 RVA: 0x00055ABA File Offset: 0x00054ABA
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				return new XPathSingletonIterator(XsltCompileContext.XsltFunctionImpl.ToNavigator(args[0]));
			}
		}

		// Token: 0x020001AD RID: 429
		private class FuncExtension : XsltCompileContext.XsltFunctionImpl
		{
			// Token: 0x060011C9 RID: 4553 RVA: 0x00055ACC File Offset: 0x00054ACC
			public FuncExtension(object extension, MethodInfo method, PermissionSet permissions)
			{
				this.extension = extension;
				this.method = method;
				this.permissions = permissions;
				XPathResultType xpathType = XsltCompileContext.GetXPathType(method.ReturnType);
				ParameterInfo[] parameters = method.GetParameters();
				int num = parameters.Length;
				int num2 = parameters.Length;
				this.typeCodes = new TypeCode[parameters.Length];
				XPathResultType[] array = new XPathResultType[parameters.Length];
				bool flag = true;
				int num3 = parameters.Length - 1;
				while (0 <= num3)
				{
					this.typeCodes[num3] = Type.GetTypeCode(parameters[num3].ParameterType);
					array[num3] = XsltCompileContext.GetXPathType(parameters[num3].ParameterType);
					if (flag)
					{
						if (parameters[num3].IsOptional)
						{
							num--;
						}
						else
						{
							flag = false;
						}
					}
					num3--;
				}
				base.Init(num, num2, xpathType, array);
			}

			// Token: 0x060011CA RID: 4554 RVA: 0x00055B8C File Offset: 0x00054B8C
			public override object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
			{
				int num = args.Length - 1;
				while (0 <= num)
				{
					args[num] = XsltCompileContext.XsltFunctionImpl.ConvertToXPathType(args[num], base.ArgTypes[num], this.typeCodes[num]);
					num--;
				}
				if (this.permissions != null)
				{
					this.permissions.PermitOnly();
				}
				return this.method.Invoke(this.extension, args);
			}

			// Token: 0x04000BE2 RID: 3042
			private object extension;

			// Token: 0x04000BE3 RID: 3043
			private MethodInfo method;

			// Token: 0x04000BE4 RID: 3044
			private TypeCode[] typeCodes;

			// Token: 0x04000BE5 RID: 3045
			private PermissionSet permissions;
		}
	}
}
