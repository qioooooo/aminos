using System;
using System.Globalization;
using System.Reflection;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000A6 RID: 166
	internal class XmlExtensionFunction
	{
		// Token: 0x060007D5 RID: 2005 RVA: 0x00027E91 File Offset: 0x00026E91
		public XmlExtensionFunction()
		{
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00027E99 File Offset: 0x00026E99
		public XmlExtensionFunction(string name, string namespaceUri, MethodInfo meth)
		{
			this.name = name;
			this.namespaceUri = namespaceUri;
			this.Bind(meth);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00027EB6 File Offset: 0x00026EB6
		public XmlExtensionFunction(string name, string namespaceUri, int numArgs, Type objectType, BindingFlags flags)
		{
			this.Init(name, namespaceUri, numArgs, objectType, flags);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00027ECC File Offset: 0x00026ECC
		public void Init(string name, string namespaceUri, int numArgs, Type objectType, BindingFlags flags)
		{
			this.name = name;
			this.namespaceUri = namespaceUri;
			this.numArgs = numArgs;
			this.objectType = objectType;
			this.flags = flags;
			this.meth = null;
			this.argClrTypes = null;
			this.retClrType = null;
			this.argXmlTypes = null;
			this.retXmlType = null;
			this.hashCode = namespaceUri.GetHashCode() ^ name.GetHashCode() ^ (int)((int)flags << 16) ^ numArgs;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00027F3C File Offset: 0x00026F3C
		public MethodInfo Method
		{
			get
			{
				return this.meth;
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00027F44 File Offset: 0x00026F44
		public Type GetClrArgumentType(int index)
		{
			return this.argClrTypes[index];
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00027F4E File Offset: 0x00026F4E
		public Type ClrReturnType
		{
			get
			{
				return this.retClrType;
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00027F56 File Offset: 0x00026F56
		public XmlQueryType GetXmlArgumentType(int index)
		{
			return this.argXmlTypes[index];
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00027F60 File Offset: 0x00026F60
		public XmlQueryType XmlReturnType
		{
			get
			{
				return this.retXmlType;
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00027F68 File Offset: 0x00026F68
		public bool CanBind()
		{
			MethodInfo[] methods = this.objectType.GetMethods(this.flags);
			StringComparison stringComparison = (((this.flags & BindingFlags.IgnoreCase) != BindingFlags.Default) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.Name.Equals(this.name, stringComparison) && (this.numArgs == -1 || methodInfo.GetParameters().Length == this.numArgs) && !methodInfo.IsGenericMethodDefinition)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00027FF8 File Offset: 0x00026FF8
		public void Bind()
		{
			MethodInfo[] array = this.objectType.GetMethods(this.flags);
			MethodInfo methodInfo = null;
			StringComparison stringComparison = (((this.flags & BindingFlags.IgnoreCase) != BindingFlags.Default) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			foreach (MethodInfo methodInfo2 in array)
			{
				if (methodInfo2.Name.Equals(this.name, stringComparison) && (this.numArgs == -1 || methodInfo2.GetParameters().Length == this.numArgs))
				{
					if (methodInfo != null)
					{
						throw new XslTransformException("XmlIl_AmbiguousExtensionMethod", new string[]
						{
							this.namespaceUri,
							this.name,
							this.numArgs.ToString(CultureInfo.InvariantCulture)
						});
					}
					methodInfo = methodInfo2;
				}
			}
			if (methodInfo == null)
			{
				array = this.objectType.GetMethods(this.flags | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo3 in array)
				{
					if (methodInfo3.Name.Equals(this.name, stringComparison) && methodInfo3.GetParameters().Length == this.numArgs)
					{
						throw new XslTransformException("XmlIl_NonPublicExtensionMethod", new string[] { this.namespaceUri, this.name });
					}
				}
				throw new XslTransformException("XmlIl_NoExtensionMethod", new string[]
				{
					this.namespaceUri,
					this.name,
					this.numArgs.ToString(CultureInfo.InvariantCulture)
				});
			}
			if (methodInfo.IsGenericMethodDefinition)
			{
				throw new XslTransformException("XmlIl_GenericExtensionMethod", new string[] { this.namespaceUri, this.name });
			}
			this.Bind(methodInfo);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000281C0 File Offset: 0x000271C0
		private void Bind(MethodInfo meth)
		{
			ParameterInfo[] parameters = meth.GetParameters();
			this.meth = meth;
			this.argClrTypes = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				this.argClrTypes[i] = this.GetClrType(parameters[i].ParameterType);
			}
			this.retClrType = this.GetClrType(this.meth.ReturnType);
			this.argXmlTypes = new XmlQueryType[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				this.argXmlTypes[i] = this.InferXmlType(this.argClrTypes[i]);
				if (this.namespaceUri.Length == 0)
				{
					if (object.Equals(this.argXmlTypes[i], XmlQueryTypeFactory.NodeNotRtf))
					{
						this.argXmlTypes[i] = XmlQueryTypeFactory.Node;
					}
					else if (object.Equals(this.argXmlTypes[i], XmlQueryTypeFactory.NodeDodS))
					{
						this.argXmlTypes[i] = XmlQueryTypeFactory.NodeS;
					}
				}
				else if (object.Equals(this.argXmlTypes[i], XmlQueryTypeFactory.NodeDodS))
				{
					this.argXmlTypes[i] = XmlQueryTypeFactory.NodeNotRtfS;
				}
			}
			this.retXmlType = this.InferXmlType(this.retClrType);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000282E4 File Offset: 0x000272E4
		public object Invoke(object extObj, object[] args)
		{
			object obj;
			try
			{
				obj = this.meth.Invoke(extObj, this.flags, null, args, CultureInfo.InvariantCulture);
			}
			catch (TargetInvocationException ex)
			{
				throw new XslTransformException(ex.InnerException, "XmlIl_ExtensionError", new string[] { this.name });
			}
			catch (Exception ex2)
			{
				if (!XmlException.IsCatchableException(ex2))
				{
					throw;
				}
				throw new XslTransformException(ex2, "XmlIl_ExtensionError", new string[] { this.name });
			}
			return obj;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00028378 File Offset: 0x00027378
		public override bool Equals(object other)
		{
			XmlExtensionFunction xmlExtensionFunction = other as XmlExtensionFunction;
			return this.hashCode == xmlExtensionFunction.hashCode && this.name == xmlExtensionFunction.name && this.namespaceUri == xmlExtensionFunction.namespaceUri && this.numArgs == xmlExtensionFunction.numArgs && this.objectType == xmlExtensionFunction.objectType && this.flags == xmlExtensionFunction.flags;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000283EC File Offset: 0x000273EC
		public override int GetHashCode()
		{
			return this.hashCode;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000283F4 File Offset: 0x000273F4
		private Type GetClrType(Type clrType)
		{
			if (clrType.IsEnum)
			{
				return Enum.GetUnderlyingType(clrType);
			}
			if (clrType.IsByRef)
			{
				throw new XslTransformException("XmlIl_ByRefType", new string[] { this.namespaceUri, this.name });
			}
			return clrType;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002843E File Offset: 0x0002743E
		private XmlQueryType InferXmlType(Type clrType)
		{
			return XsltConvert.InferXsltType(clrType);
		}

		// Token: 0x04000556 RID: 1366
		private string namespaceUri;

		// Token: 0x04000557 RID: 1367
		private string name;

		// Token: 0x04000558 RID: 1368
		private int numArgs;

		// Token: 0x04000559 RID: 1369
		private Type objectType;

		// Token: 0x0400055A RID: 1370
		private BindingFlags flags;

		// Token: 0x0400055B RID: 1371
		private int hashCode;

		// Token: 0x0400055C RID: 1372
		private MethodInfo meth;

		// Token: 0x0400055D RID: 1373
		private Type[] argClrTypes;

		// Token: 0x0400055E RID: 1374
		private Type retClrType;

		// Token: 0x0400055F RID: 1375
		private XmlQueryType[] argXmlTypes;

		// Token: 0x04000560 RID: 1376
		private XmlQueryType retXmlType;
	}
}
