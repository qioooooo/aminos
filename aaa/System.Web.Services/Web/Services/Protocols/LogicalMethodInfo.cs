using System;
using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200004B RID: 75
	public sealed class LogicalMethodInfo
	{
		// Token: 0x06000180 RID: 384 RVA: 0x000068BC File Offset: 0x000058BC
		public LogicalMethodInfo(MethodInfo methodInfo)
			: this(methodInfo, null)
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000068C8 File Offset: 0x000058C8
		internal LogicalMethodInfo(MethodInfo methodInfo, WebMethod webMethod)
		{
			if (methodInfo.IsStatic)
			{
				throw new InvalidOperationException(Res.GetString("WebMethodStatic", new object[] { methodInfo.Name }));
			}
			this.methodInfo = methodInfo;
			if (webMethod != null)
			{
				this.binding = webMethod.binding;
				this.attribute = webMethod.attribute;
				this.declaration = webMethod.declaration;
			}
			MethodInfo methodInfo2 = ((this.declaration != null) ? this.declaration : methodInfo);
			this.parameters = methodInfo2.GetParameters();
			this.inParams = LogicalMethodInfo.GetInParameters(methodInfo2, this.parameters, 0, this.parameters.Length, false);
			this.outParams = LogicalMethodInfo.GetOutParameters(methodInfo2, this.parameters, 0, this.parameters.Length, false);
			this.retType = methodInfo2.ReturnType;
			this.isVoid = this.retType == typeof(void);
			this.methodName = methodInfo2.Name;
			this.attributes = new Hashtable();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000069C4 File Offset: 0x000059C4
		private LogicalMethodInfo(MethodInfo beginMethodInfo, MethodInfo endMethodInfo, WebMethod webMethod)
		{
			this.methodInfo = beginMethodInfo;
			this.endMethodInfo = endMethodInfo;
			this.methodName = beginMethodInfo.Name.Substring(5);
			if (webMethod != null)
			{
				this.binding = webMethod.binding;
				this.attribute = webMethod.attribute;
				this.declaration = webMethod.declaration;
			}
			ParameterInfo[] array = beginMethodInfo.GetParameters();
			if (array.Length < 2 || array[array.Length - 1].ParameterType != typeof(object) || array[array.Length - 2].ParameterType != typeof(AsyncCallback))
			{
				throw new InvalidOperationException(Res.GetString("WebMethodMissingParams", new object[]
				{
					beginMethodInfo.DeclaringType.FullName,
					beginMethodInfo.Name,
					typeof(AsyncCallback).FullName,
					typeof(object).FullName
				}));
			}
			this.stateParam = array[array.Length - 1];
			this.callbackParam = array[array.Length - 2];
			this.inParams = LogicalMethodInfo.GetInParameters(beginMethodInfo, array, 0, array.Length - 2, true);
			ParameterInfo[] array2 = endMethodInfo.GetParameters();
			this.resultParam = array2[0];
			this.outParams = LogicalMethodInfo.GetOutParameters(endMethodInfo, array2, 1, array2.Length - 1, true);
			this.parameters = new ParameterInfo[this.inParams.Length + this.outParams.Length];
			this.inParams.CopyTo(this.parameters, 0);
			this.outParams.CopyTo(this.parameters, this.inParams.Length);
			this.retType = endMethodInfo.ReturnType;
			this.isVoid = this.retType == typeof(void);
			this.attributes = new Hashtable();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006B76 File Offset: 0x00005B76
		public override string ToString()
		{
			return this.methodInfo.ToString();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006B84 File Offset: 0x00005B84
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public object[] Invoke(object target, object[] values)
		{
			if (this.outParams.Length > 0)
			{
				object[] array = new object[this.parameters.Length];
				for (int i = 0; i < this.inParams.Length; i++)
				{
					array[this.inParams[i].Position] = values[i];
				}
				values = array;
			}
			object obj = this.methodInfo.Invoke(target, values);
			if (this.outParams.Length > 0)
			{
				int num = this.outParams.Length;
				if (!this.isVoid)
				{
					num++;
				}
				object[] array2 = new object[num];
				num = 0;
				if (!this.isVoid)
				{
					array2[num++] = obj;
				}
				for (int j = 0; j < this.outParams.Length; j++)
				{
					array2[num++] = values[this.outParams[j].Position];
				}
				return array2;
			}
			if (this.isVoid)
			{
				return LogicalMethodInfo.emptyObjectArray;
			}
			return new object[] { obj };
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006C6C File Offset: 0x00005C6C
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public IAsyncResult BeginInvoke(object target, object[] values, AsyncCallback callback, object asyncState)
		{
			object[] array = new object[values.Length + 2];
			values.CopyTo(array, 0);
			array[values.Length] = callback;
			array[values.Length + 1] = asyncState;
			return (IAsyncResult)this.methodInfo.Invoke(target, array);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006CB0 File Offset: 0x00005CB0
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public object[] EndInvoke(object target, IAsyncResult asyncResult)
		{
			object[] array = new object[this.outParams.Length + 1];
			array[0] = asyncResult;
			object obj = this.endMethodInfo.Invoke(target, array);
			if (!this.isVoid)
			{
				array[0] = obj;
				return array;
			}
			if (this.outParams.Length > 0)
			{
				object[] array2 = new object[this.outParams.Length];
				Array.Copy(array, 1, array2, 0, array2.Length);
				return array2;
			}
			return LogicalMethodInfo.emptyObjectArray;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00006D19 File Offset: 0x00005D19
		internal WebServiceBindingAttribute Binding
		{
			get
			{
				return this.binding;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00006D21 File Offset: 0x00005D21
		internal MethodInfo Declaration
		{
			get
			{
				return this.declaration;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00006D29 File Offset: 0x00005D29
		public Type DeclaringType
		{
			get
			{
				return this.methodInfo.DeclaringType;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00006D36 File Offset: 0x00005D36
		public string Name
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006D3E File Offset: 0x00005D3E
		public ParameterInfo AsyncResultParameter
		{
			get
			{
				return this.resultParam;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00006D46 File Offset: 0x00005D46
		public ParameterInfo AsyncCallbackParameter
		{
			get
			{
				return this.callbackParam;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00006D4E File Offset: 0x00005D4E
		public ParameterInfo AsyncStateParameter
		{
			get
			{
				return this.stateParam;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00006D56 File Offset: 0x00005D56
		public Type ReturnType
		{
			get
			{
				return this.retType;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00006D5E File Offset: 0x00005D5E
		public bool IsVoid
		{
			get
			{
				return this.isVoid;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00006D66 File Offset: 0x00005D66
		public bool IsAsync
		{
			get
			{
				return this.endMethodInfo != null;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00006D74 File Offset: 0x00005D74
		public ParameterInfo[] InParameters
		{
			get
			{
				return this.inParams;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00006D7C File Offset: 0x00005D7C
		public ParameterInfo[] OutParameters
		{
			get
			{
				return this.outParams;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00006D84 File Offset: 0x00005D84
		public ParameterInfo[] Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006D8C File Offset: 0x00005D8C
		public object[] GetCustomAttributes(Type type)
		{
			object[] array = null;
			array = (object[])this.attributes[type];
			if (array != null)
			{
				return array;
			}
			lock (this.attributes)
			{
				array = (object[])this.attributes[type];
				if (array == null)
				{
					if (this.declaration != null)
					{
						object[] customAttributes = this.declaration.GetCustomAttributes(type, false);
						object[] customAttributes2 = this.methodInfo.GetCustomAttributes(type, false);
						if (customAttributes2.Length > 0)
						{
							if (!LogicalMethodInfo.CanMerge(type))
							{
								throw new InvalidOperationException(Res.GetString("ContractOverride", new object[]
								{
									this.methodInfo.Name,
									this.methodInfo.DeclaringType.FullName,
									this.declaration.DeclaringType.FullName,
									this.declaration.ToString(),
									customAttributes2[0].ToString()
								}));
							}
							ArrayList arrayList = new ArrayList();
							for (int i = 0; i < customAttributes.Length; i++)
							{
								arrayList.Add(customAttributes[i]);
							}
							for (int j = 0; j < customAttributes2.Length; j++)
							{
								arrayList.Add(customAttributes2[j]);
							}
							array = (object[])arrayList.ToArray(type);
						}
						else
						{
							array = customAttributes;
						}
					}
					else
					{
						array = this.methodInfo.GetCustomAttributes(type, false);
					}
					this.attributes[type] = array;
				}
			}
			return array;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006F14 File Offset: 0x00005F14
		public object GetCustomAttribute(Type type)
		{
			object[] customAttributes = this.GetCustomAttributes(type);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			return customAttributes[0];
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00006F33 File Offset: 0x00005F33
		internal WebMethodAttribute MethodAttribute
		{
			get
			{
				if (this.attribute == null)
				{
					this.attribute = (WebMethodAttribute)this.GetCustomAttribute(typeof(WebMethodAttribute));
					if (this.attribute == null)
					{
						this.attribute = new WebMethodAttribute();
					}
				}
				return this.attribute;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00006F71 File Offset: 0x00005F71
		public ICustomAttributeProvider CustomAttributeProvider
		{
			get
			{
				return this.methodInfo;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00006F79 File Offset: 0x00005F79
		public ICustomAttributeProvider ReturnTypeCustomAttributeProvider
		{
			get
			{
				if (this.declaration != null)
				{
					return this.declaration.ReturnTypeCustomAttributes;
				}
				return this.methodInfo.ReturnTypeCustomAttributes;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006F9A File Offset: 0x00005F9A
		public MethodInfo MethodInfo
		{
			get
			{
				if (this.endMethodInfo != null)
				{
					return null;
				}
				return this.methodInfo;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00006FAC File Offset: 0x00005FAC
		public MethodInfo BeginMethodInfo
		{
			get
			{
				return this.methodInfo;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00006FB4 File Offset: 0x00005FB4
		public MethodInfo EndMethodInfo
		{
			get
			{
				return this.endMethodInfo;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006FBC File Offset: 0x00005FBC
		private static ParameterInfo[] GetInParameters(MethodInfo methodInfo, ParameterInfo[] paramInfos, int start, int length, bool mustBeIn)
		{
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				ParameterInfo parameterInfo = paramInfos[i + start];
				if (LogicalMethodInfo.IsInParameter(parameterInfo))
				{
					num++;
				}
				else if (mustBeIn)
				{
					throw new InvalidOperationException(Res.GetString("WebBadOutParameter", new object[]
					{
						parameterInfo.Name,
						methodInfo.DeclaringType.FullName,
						parameterInfo.Name
					}));
				}
			}
			ParameterInfo[] array = new ParameterInfo[num];
			num = 0;
			for (int j = 0; j < length; j++)
			{
				ParameterInfo parameterInfo2 = paramInfos[j + start];
				if (LogicalMethodInfo.IsInParameter(parameterInfo2))
				{
					array[num++] = parameterInfo2;
				}
			}
			return array;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007060 File Offset: 0x00006060
		private static ParameterInfo[] GetOutParameters(MethodInfo methodInfo, ParameterInfo[] paramInfos, int start, int length, bool mustBeOut)
		{
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				ParameterInfo parameterInfo = paramInfos[i + start];
				if (LogicalMethodInfo.IsOutParameter(parameterInfo))
				{
					num++;
				}
				else if (mustBeOut)
				{
					throw new InvalidOperationException(Res.GetString("WebInOutParameter", new object[]
					{
						parameterInfo.Name,
						methodInfo.DeclaringType.FullName,
						parameterInfo.Name
					}));
				}
			}
			ParameterInfo[] array = new ParameterInfo[num];
			num = 0;
			for (int j = 0; j < length; j++)
			{
				ParameterInfo parameterInfo2 = paramInfos[j + start];
				if (LogicalMethodInfo.IsOutParameter(parameterInfo2))
				{
					array[num++] = parameterInfo2;
				}
			}
			return array;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007104 File Offset: 0x00006104
		private static bool IsInParameter(ParameterInfo paramInfo)
		{
			return !paramInfo.IsOut;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000710F File Offset: 0x0000610F
		private static bool IsOutParameter(ParameterInfo paramInfo)
		{
			return paramInfo.IsOut || paramInfo.ParameterType.IsByRef;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007126 File Offset: 0x00006126
		public static bool IsBeginMethod(MethodInfo methodInfo)
		{
			return typeof(IAsyncResult).IsAssignableFrom(methodInfo.ReturnType) && methodInfo.Name.StartsWith("Begin", StringComparison.Ordinal);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007154 File Offset: 0x00006154
		public static bool IsEndMethod(MethodInfo methodInfo)
		{
			ParameterInfo[] array = methodInfo.GetParameters();
			return array.Length > 0 && typeof(IAsyncResult).IsAssignableFrom(array[0].ParameterType) && methodInfo.Name.StartsWith("End", StringComparison.Ordinal);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000719A File Offset: 0x0000619A
		public static LogicalMethodInfo[] Create(MethodInfo[] methodInfos)
		{
			return LogicalMethodInfo.Create(methodInfos, (LogicalMethodTypes)3, null);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000071A4 File Offset: 0x000061A4
		public static LogicalMethodInfo[] Create(MethodInfo[] methodInfos, LogicalMethodTypes types)
		{
			return LogicalMethodInfo.Create(methodInfos, types, null);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000071B0 File Offset: 0x000061B0
		internal static LogicalMethodInfo[] Create(MethodInfo[] methodInfos, LogicalMethodTypes types, Hashtable declarations)
		{
			ArrayList arrayList = (((types & LogicalMethodTypes.Async) != (LogicalMethodTypes)0) ? new ArrayList() : null);
			Hashtable hashtable = (((types & LogicalMethodTypes.Async) != (LogicalMethodTypes)0) ? new Hashtable() : null);
			ArrayList arrayList2 = (((types & LogicalMethodTypes.Sync) != (LogicalMethodTypes)0) ? new ArrayList() : null);
			foreach (MethodInfo methodInfo in methodInfos)
			{
				if (LogicalMethodInfo.IsBeginMethod(methodInfo))
				{
					if (arrayList != null)
					{
						arrayList.Add(methodInfo);
					}
				}
				else if (LogicalMethodInfo.IsEndMethod(methodInfo))
				{
					if (hashtable != null)
					{
						hashtable.Add(methodInfo.Name, methodInfo);
					}
				}
				else if (arrayList2 != null)
				{
					arrayList2.Add(methodInfo);
				}
			}
			int num = ((arrayList == null) ? 0 : arrayList.Count);
			int num2 = ((arrayList2 == null) ? 0 : arrayList2.Count);
			int num3 = num2 + num;
			LogicalMethodInfo[] array = new LogicalMethodInfo[num3];
			num3 = 0;
			for (int j = 0; j < num2; j++)
			{
				MethodInfo methodInfo2 = (MethodInfo)arrayList2[j];
				WebMethod webMethod = ((declarations == null) ? null : ((WebMethod)declarations[methodInfo2]));
				array[num3] = new LogicalMethodInfo(methodInfo2, webMethod);
				array[num3].CheckContractOverride();
				num3++;
			}
			for (int k = 0; k < num; k++)
			{
				MethodInfo methodInfo3 = (MethodInfo)arrayList[k];
				string text = "End" + methodInfo3.Name.Substring(5);
				MethodInfo methodInfo4 = (MethodInfo)hashtable[text];
				if (methodInfo4 == null)
				{
					throw new InvalidOperationException(Res.GetString("WebAsyncMissingEnd", new object[]
					{
						methodInfo3.DeclaringType.FullName,
						methodInfo3.Name,
						text
					}));
				}
				WebMethod webMethod2 = ((declarations == null) ? null : ((WebMethod)declarations[methodInfo3]));
				array[num3++] = new LogicalMethodInfo(methodInfo3, methodInfo4, webMethod2);
			}
			return array;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007375 File Offset: 0x00006375
		internal static HashAlgorithm HashAlgorithm
		{
			get
			{
				if (LogicalMethodInfo.hash == null)
				{
					LogicalMethodInfo.hash = SHA1.Create();
				}
				return LogicalMethodInfo.hash;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007390 File Offset: 0x00006390
		internal string GetKey()
		{
			if (this.methodInfo == null)
			{
				return string.Empty;
			}
			string text = this.methodInfo.DeclaringType.FullName + ":" + this.methodInfo.ToString();
			if (text.Length > 1024)
			{
				byte[] array = LogicalMethodInfo.HashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
				text = Convert.ToBase64String(array);
			}
			return text;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000073FC File Offset: 0x000063FC
		internal void CheckContractOverride()
		{
			if (this.declaration == null)
			{
				return;
			}
			this.methodInfo.GetParameters();
			ParameterInfo[] array = this.methodInfo.GetParameters();
			foreach (ParameterInfo parameterInfo in array)
			{
				object[] customAttributes = parameterInfo.GetCustomAttributes(false);
				foreach (object obj in customAttributes)
				{
					if (obj.GetType().Namespace == "System.Xml.Serialization")
					{
						throw new InvalidOperationException(Res.GetString("ContractOverride", new object[]
						{
							this.methodInfo.Name,
							this.methodInfo.DeclaringType.FullName,
							this.declaration.DeclaringType.FullName,
							this.declaration.ToString(),
							obj.ToString()
						}));
					}
				}
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000074F7 File Offset: 0x000064F7
		internal static bool CanMerge(Type type)
		{
			return type == typeof(SoapHeaderAttribute) || typeof(SoapExtensionAttribute).IsAssignableFrom(type);
		}

		// Token: 0x04000299 RID: 665
		private MethodInfo methodInfo;

		// Token: 0x0400029A RID: 666
		private MethodInfo endMethodInfo;

		// Token: 0x0400029B RID: 667
		private ParameterInfo[] inParams;

		// Token: 0x0400029C RID: 668
		private ParameterInfo[] outParams;

		// Token: 0x0400029D RID: 669
		private ParameterInfo[] parameters;

		// Token: 0x0400029E RID: 670
		private Hashtable attributes;

		// Token: 0x0400029F RID: 671
		private Type retType;

		// Token: 0x040002A0 RID: 672
		private ParameterInfo callbackParam;

		// Token: 0x040002A1 RID: 673
		private ParameterInfo stateParam;

		// Token: 0x040002A2 RID: 674
		private ParameterInfo resultParam;

		// Token: 0x040002A3 RID: 675
		private string methodName;

		// Token: 0x040002A4 RID: 676
		private bool isVoid;

		// Token: 0x040002A5 RID: 677
		private static object[] emptyObjectArray = new object[0];

		// Token: 0x040002A6 RID: 678
		private WebServiceBindingAttribute binding;

		// Token: 0x040002A7 RID: 679
		private WebMethodAttribute attribute;

		// Token: 0x040002A8 RID: 680
		private MethodInfo declaration;

		// Token: 0x040002A9 RID: 681
		private static HashAlgorithm hash;
	}
}
