using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000706 RID: 1798
	[CLSCompliant(false)]
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class MethodCall : IMethodCallMessage, IMethodMessage, IMessage, ISerializable, IInternalMessage, ISerializationRootObject
	{
		// Token: 0x060040B0 RID: 16560 RVA: 0x000DCD01 File Offset: 0x000DBD01
		public MethodCall(Header[] h1)
		{
			this.Init();
			this.fSoap = true;
			this.FillHeaders(h1);
			this.ResolveMethod();
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x000DCD24 File Offset: 0x000DBD24
		public MethodCall(IMessage msg)
		{
			if (msg == null)
			{
				throw new ArgumentNullException("msg");
			}
			this.Init();
			IDictionaryEnumerator enumerator = msg.Properties.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.FillHeader(enumerator.Key.ToString(), enumerator.Value);
			}
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			if (methodCallMessage != null)
			{
				this.MI = methodCallMessage.MethodBase;
			}
			this.ResolveMethod();
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x000DCD94 File Offset: 0x000DBD94
		internal MethodCall(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.Init();
			this.SetObjectData(info, context);
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x000DCDB8 File Offset: 0x000DBDB8
		internal MethodCall(SmuggledMethodCallMessage smuggledMsg, ArrayList deserializedArgs)
		{
			this.uri = smuggledMsg.Uri;
			this.typeName = smuggledMsg.TypeName;
			this.methodName = smuggledMsg.MethodName;
			this.methodSignature = (Type[])smuggledMsg.GetMethodSignature(deserializedArgs);
			this.args = smuggledMsg.GetArgs(deserializedArgs);
			this.instArgs = smuggledMsg.GetInstantiation(deserializedArgs);
			this.callContext = smuggledMsg.GetCallContext(deserializedArgs);
			this.ResolveMethod();
			if (smuggledMsg.MessagePropertyCount > 0)
			{
				smuggledMsg.PopulateMessageProperties(this.Properties, deserializedArgs);
			}
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x000DCE44 File Offset: 0x000DBE44
		internal MethodCall(object handlerObject, BinaryMethodCallMessage smuggledMsg)
		{
			if (handlerObject != null)
			{
				this.uri = handlerObject as string;
				if (this.uri == null)
				{
					MarshalByRefObject marshalByRefObject = handlerObject as MarshalByRefObject;
					if (marshalByRefObject != null)
					{
						bool flag;
						this.srvID = MarshalByRefObject.GetIdentity(marshalByRefObject, out flag) as ServerIdentity;
						this.uri = this.srvID.URI;
					}
				}
			}
			this.typeName = smuggledMsg.TypeName;
			this.methodName = smuggledMsg.MethodName;
			this.methodSignature = (Type[])smuggledMsg.MethodSignature;
			this.args = smuggledMsg.Args;
			this.instArgs = smuggledMsg.InstantiationArgs;
			this.callContext = smuggledMsg.LogicalCallContext;
			this.ResolveMethod();
			if (smuggledMsg.HasProperties)
			{
				smuggledMsg.PopulateMessageProperties(this.Properties);
			}
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x000DCF03 File Offset: 0x000DBF03
		public void RootSetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			this.SetObjectData(info, ctx);
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x000DCF10 File Offset: 0x000DBF10
		internal void SetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.fSoap)
			{
				this.SetObjectFromSoapData(info);
				return;
			}
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.FillHeader(enumerator.Name, enumerator.Value);
			}
			if (context.State == StreamingContextStates.Remoting && context.Context != null)
			{
				Header[] array = context.Context as Header[];
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						this.FillHeader(array[i].Name, array[i].Value);
					}
				}
			}
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x000DCFA8 File Offset: 0x000DBFA8
		internal Type ResolveType()
		{
			Type type = null;
			if (this.srvID == null)
			{
				this.srvID = IdentityHolder.CasualResolveIdentity(this.uri) as ServerIdentity;
			}
			if (this.srvID != null)
			{
				Type type2 = this.srvID.GetLastCalledType(this.typeName);
				if (type2 != null)
				{
					return type2;
				}
				int num = 0;
				if (string.CompareOrdinal(this.typeName, 0, "clr:", 0, 4) == 0)
				{
					num = 4;
				}
				int num2 = this.typeName.IndexOf(',', num);
				if (num2 == -1)
				{
					num2 = this.typeName.Length;
				}
				type2 = this.srvID.ServerType;
				type = Type.ResolveTypeRelativeTo(this.typeName, num, num2 - num, type2);
			}
			if (type == null)
			{
				type = RemotingServices.InternalGetTypeFromQualifiedTypeName(this.typeName);
			}
			if (this.srvID != null)
			{
				this.srvID.SetLastCalledType(this.typeName, type);
			}
			return type;
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x000DD073 File Offset: 0x000DC073
		public void ResolveMethod()
		{
			this.ResolveMethod(true);
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x000DD07C File Offset: 0x000DC07C
		internal void ResolveMethod(bool bThrowIfNotResolved)
		{
			if (this.MI == null && this.methodName != null)
			{
				RuntimeType runtimeType = this.ResolveType() as RuntimeType;
				if (this.methodName.Equals(".ctor"))
				{
					return;
				}
				if (runtimeType == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[] { this.typeName }));
				}
				if (this.methodSignature != null)
				{
					try
					{
						this.MI = runtimeType.GetMethod(this.methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, this.methodSignature, null);
					}
					catch (AmbiguousMatchException)
					{
						MemberInfo[] array = runtimeType.FindMembers(MemberTypes.Method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, Type.FilterName, this.methodName);
						int num = ((this.instArgs == null) ? 0 : this.instArgs.Length);
						int num2 = 0;
						for (int i = 0; i < array.Length; i++)
						{
							MethodInfo methodInfo = (MethodInfo)array[i];
							int num3 = (methodInfo.IsGenericMethod ? methodInfo.GetGenericArguments().Length : 0);
							if (num3 == num)
							{
								if (i > num2)
								{
									array[num2] = array[i];
								}
								num2++;
							}
						}
						MethodInfo[] array2 = new MethodInfo[num2];
						for (int j = 0; j < num2; j++)
						{
							array2[j] = (MethodInfo)array[j];
						}
						this.MI = Type.DefaultBinder.SelectMethod(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, array2, this.methodSignature, null);
					}
					if (this.instArgs != null && this.instArgs.Length > 0)
					{
						this.MI = ((MethodInfo)this.MI).MakeGenericMethod(this.instArgs);
					}
				}
				else
				{
					RemotingTypeCachedData remotingTypeCachedData = null;
					if (this.instArgs == null)
					{
						remotingTypeCachedData = InternalRemotingServices.GetReflectionCachedData(runtimeType);
						this.MI = remotingTypeCachedData.GetLastCalledMethod(this.methodName);
						if (this.MI != null)
						{
							return;
						}
					}
					bool flag = false;
					try
					{
						this.MI = runtimeType.GetMethod(this.methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (this.instArgs != null && this.instArgs.Length > 0)
						{
							this.MI = ((MethodInfo)this.MI).MakeGenericMethod(this.instArgs);
						}
					}
					catch (AmbiguousMatchException)
					{
						flag = true;
						this.ResolveOverloadedMethod(runtimeType);
					}
					if (this.MI != null && !flag && remotingTypeCachedData != null)
					{
						remotingTypeCachedData.SetLastCalledMethod(this.methodName, this.MI);
					}
				}
				if (this.MI == null && bThrowIfNotResolved)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MethodMissing"), new object[] { this.methodName, this.typeName }));
				}
			}
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x000DD314 File Offset: 0x000DC314
		private void ResolveOverloadedMethod(RuntimeType t)
		{
			if (this.args == null)
			{
				return;
			}
			MemberInfo[] member = t.GetMember(this.methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			int num = member.Length;
			if (num == 1)
			{
				this.MI = member[0] as MethodBase;
				return;
			}
			if (num == 0)
			{
				return;
			}
			int num2 = this.args.Length;
			MethodBase methodBase = null;
			for (int i = 0; i < num; i++)
			{
				MethodBase methodBase2 = member[i] as MethodBase;
				if (methodBase2.GetParameters().Length == num2)
				{
					if (methodBase != null)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_AmbiguousMethod"));
					}
					methodBase = methodBase2;
				}
			}
			if (methodBase != null)
			{
				this.MI = methodBase;
			}
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x000DD3A8 File Offset: 0x000DC3A8
		private void ResolveOverloadedMethod(RuntimeType t, string methodName, ArrayList argNames, ArrayList argValues)
		{
			MemberInfo[] member = t.GetMember(methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			int num = member.Length;
			if (num == 1)
			{
				this.MI = member[0] as MethodBase;
				return;
			}
			if (num == 0)
			{
				return;
			}
			MethodBase methodBase = null;
			for (int i = 0; i < num; i++)
			{
				MethodBase methodBase2 = member[i] as MethodBase;
				ParameterInfo[] parameters = methodBase2.GetParameters();
				if (parameters.Length == argValues.Count)
				{
					bool flag = true;
					for (int j = 0; j < parameters.Length; j++)
					{
						Type type = parameters[j].ParameterType;
						if (type.IsByRef)
						{
							type = type.GetElementType();
						}
						if (type != argValues[j].GetType())
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						methodBase = methodBase2;
						break;
					}
				}
			}
			if (methodBase == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_AmbiguousMethod"));
			}
			this.MI = methodBase;
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x000DD477 File Offset: 0x000DC477
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x000DD488 File Offset: 0x000DC488
		internal void SetObjectFromSoapData(SerializationInfo info)
		{
			this.methodName = info.GetString("__methodName");
			ArrayList arrayList = (ArrayList)info.GetValue("__paramNameList", typeof(ArrayList));
			Hashtable hashtable = (Hashtable)info.GetValue("__keyToNamespaceTable", typeof(Hashtable));
			if (this.MI == null)
			{
				ArrayList arrayList2 = new ArrayList();
				ArrayList arrayList3 = arrayList;
				for (int i = 0; i < arrayList3.Count; i++)
				{
					arrayList2.Add(info.GetValue((string)arrayList3[i], typeof(object)));
				}
				RuntimeType runtimeType = this.ResolveType() as RuntimeType;
				if (runtimeType == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[] { this.typeName }));
				}
				this.ResolveOverloadedMethod(runtimeType, this.methodName, arrayList3, arrayList2);
				if (this.MI == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MethodMissing"), new object[] { this.methodName, this.typeName }));
				}
			}
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.MI);
			ParameterInfo[] parameters = reflectionCachedData.Parameters;
			int[] marshalRequestArgMap = reflectionCachedData.MarshalRequestArgMap;
			int[] outOnlyArgMap = reflectionCachedData.OutOnlyArgMap;
			object obj = ((this.InternalProperties == null) ? null : this.InternalProperties["__UnorderedParams"]);
			this.args = new object[parameters.Length];
			if (obj != null && obj is bool && (bool)obj)
			{
				for (int j = 0; j < arrayList.Count; j++)
				{
					string text = (string)arrayList[j];
					int num = -1;
					for (int k = 0; k < parameters.Length; k++)
					{
						if (text.Equals(parameters[k].Name))
						{
							num = parameters[k].Position;
							break;
						}
					}
					if (num == -1)
					{
						if (!text.StartsWith("__param", StringComparison.Ordinal))
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
						}
						num = int.Parse(text.Substring(7), CultureInfo.InvariantCulture);
					}
					if (num >= this.args.Length)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
					}
					this.args[num] = Message.SoapCoerceArg(info.GetValue(text, typeof(object)), parameters[num].ParameterType, hashtable);
				}
				return;
			}
			for (int l = 0; l < arrayList.Count; l++)
			{
				string text2 = (string)arrayList[l];
				this.args[marshalRequestArgMap[l]] = Message.SoapCoerceArg(info.GetValue(text2, typeof(object)), parameters[marshalRequestArgMap[l]].ParameterType, hashtable);
			}
			foreach (int num2 in outOnlyArgMap)
			{
				Type elementType = parameters[num2].ParameterType.GetElementType();
				if (elementType.IsValueType)
				{
					this.args[num2] = Activator.CreateInstance(elementType, true);
				}
			}
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x000DD7A9 File Offset: 0x000DC7A9
		public virtual void Init()
		{
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x060040BF RID: 16575 RVA: 0x000DD7AB File Offset: 0x000DC7AB
		public int ArgCount
		{
			get
			{
				if (this.args != null)
				{
					return this.args.Length;
				}
				return 0;
			}
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x000DD7BF File Offset: 0x000DC7BF
		public object GetArg(int argNum)
		{
			return this.args[argNum];
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x000DD7CC File Offset: 0x000DC7CC
		public string GetArgName(int index)
		{
			this.ResolveMethod();
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.MI);
			return reflectionCachedData.Parameters[index].Name;
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x000DD7F8 File Offset: 0x000DC7F8
		public object[] Args
		{
			get
			{
				return this.args;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x060040C3 RID: 16579 RVA: 0x000DD800 File Offset: 0x000DC800
		public int InArgCount
		{
			get
			{
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, false);
				}
				return this.argMapper.ArgCount;
			}
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x000DD822 File Offset: 0x000DC822
		public object GetInArg(int argNum)
		{
			if (this.argMapper == null)
			{
				this.argMapper = new ArgMapper(this, false);
			}
			return this.argMapper.GetArg(argNum);
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x000DD845 File Offset: 0x000DC845
		public string GetInArgName(int index)
		{
			if (this.argMapper == null)
			{
				this.argMapper = new ArgMapper(this, false);
			}
			return this.argMapper.GetArgName(index);
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x000DD868 File Offset: 0x000DC868
		public object[] InArgs
		{
			get
			{
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, false);
				}
				return this.argMapper.Args;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x060040C7 RID: 16583 RVA: 0x000DD88A File Offset: 0x000DC88A
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x000DD892 File Offset: 0x000DC892
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060040C9 RID: 16585 RVA: 0x000DD89A File Offset: 0x000DC89A
		public object MethodSignature
		{
			get
			{
				if (this.methodSignature != null)
				{
					return this.methodSignature;
				}
				if (this.MI != null)
				{
					this.methodSignature = Message.GenerateMethodSignature(this.MethodBase);
				}
				return null;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x060040CA RID: 16586 RVA: 0x000DD8C5 File Offset: 0x000DC8C5
		public MethodBase MethodBase
		{
			get
			{
				if (this.MI == null)
				{
					this.MI = RemotingServices.InternalGetMethodBaseFromMethodMessage(this);
				}
				return this.MI;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x060040CB RID: 16587 RVA: 0x000DD8E1 File Offset: 0x000DC8E1
		// (set) Token: 0x060040CC RID: 16588 RVA: 0x000DD8E9 File Offset: 0x000DC8E9
		public string Uri
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x060040CD RID: 16589 RVA: 0x000DD8F2 File Offset: 0x000DC8F2
		public bool HasVarArgs
		{
			get
			{
				return this.fVarArgs;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x060040CE RID: 16590 RVA: 0x000DD8FC File Offset: 0x000DC8FC
		public virtual IDictionary Properties
		{
			get
			{
				IDictionary externalProperties;
				lock (this)
				{
					if (this.InternalProperties == null)
					{
						this.InternalProperties = new Hashtable();
					}
					if (this.ExternalProperties == null)
					{
						this.ExternalProperties = new MCMDictionary(this, this.InternalProperties);
					}
					externalProperties = this.ExternalProperties;
				}
				return externalProperties;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060040CF RID: 16591 RVA: 0x000DD960 File Offset: 0x000DC960
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x000DD968 File Offset: 0x000DC968
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this.callContext == null)
			{
				this.callContext = new LogicalCallContext();
			}
			return this.callContext;
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x000DD984 File Offset: 0x000DC984
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext ctx)
		{
			LogicalCallContext logicalCallContext = this.callContext;
			this.callContext = ctx;
			return logicalCallContext;
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x060040D2 RID: 16594 RVA: 0x000DD9A0 File Offset: 0x000DC9A0
		// (set) Token: 0x060040D3 RID: 16595 RVA: 0x000DD9A8 File Offset: 0x000DC9A8
		ServerIdentity IInternalMessage.ServerIdentityObject
		{
			get
			{
				return this.srvID;
			}
			set
			{
				this.srvID = value;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x060040D4 RID: 16596 RVA: 0x000DD9B1 File Offset: 0x000DC9B1
		// (set) Token: 0x060040D5 RID: 16597 RVA: 0x000DD9B9 File Offset: 0x000DC9B9
		Identity IInternalMessage.IdentityObject
		{
			get
			{
				return this.identity;
			}
			set
			{
				this.identity = value;
			}
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x000DD9C2 File Offset: 0x000DC9C2
		void IInternalMessage.SetURI(string val)
		{
			this.uri = val;
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x000DD9CB File Offset: 0x000DC9CB
		void IInternalMessage.SetCallContext(LogicalCallContext newCallContext)
		{
			this.callContext = newCallContext;
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x000DD9D4 File Offset: 0x000DC9D4
		bool IInternalMessage.HasProperties()
		{
			return this.ExternalProperties != null || this.InternalProperties != null;
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x000DD9EC File Offset: 0x000DC9EC
		internal void FillHeaders(Header[] h)
		{
			this.FillHeaders(h, false);
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x000DD9F8 File Offset: 0x000DC9F8
		private void FillHeaders(Header[] h, bool bFromHeaderHandler)
		{
			if (h == null)
			{
				return;
			}
			if (bFromHeaderHandler && this.fSoap)
			{
				foreach (Header header in h)
				{
					if (header.HeaderNamespace == "http://schemas.microsoft.com/clr/soap/messageProperties")
					{
						this.FillHeader(header.Name, header.Value);
					}
					else
					{
						string propertyKeyForHeader = LogicalCallContext.GetPropertyKeyForHeader(header);
						this.FillHeader(propertyKeyForHeader, header);
					}
				}
				return;
			}
			for (int j = 0; j < h.Length; j++)
			{
				this.FillHeader(h[j].Name, h[j].Value);
			}
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x000DDA80 File Offset: 0x000DCA80
		internal virtual bool FillSpecialHeader(string key, object value)
		{
			if (key != null)
			{
				if (key.Equals("__Uri"))
				{
					this.uri = (string)value;
				}
				else if (key.Equals("__MethodName"))
				{
					this.methodName = (string)value;
				}
				else if (key.Equals("__MethodSignature"))
				{
					this.methodSignature = (Type[])value;
				}
				else if (key.Equals("__TypeName"))
				{
					this.typeName = (string)value;
				}
				else if (key.Equals("__Args"))
				{
					this.args = (object[])value;
				}
				else
				{
					if (!key.Equals("__CallContext"))
					{
						return false;
					}
					if (value is string)
					{
						this.callContext = new LogicalCallContext();
						this.callContext.RemotingData.LogicalCallID = (string)value;
					}
					else
					{
						this.callContext = (LogicalCallContext)value;
					}
				}
			}
			return true;
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x000DDB69 File Offset: 0x000DCB69
		internal void FillHeader(string key, object value)
		{
			if (!this.FillSpecialHeader(key, value))
			{
				if (this.InternalProperties == null)
				{
					this.InternalProperties = new Hashtable();
				}
				this.InternalProperties[key] = value;
			}
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x000DDB98 File Offset: 0x000DCB98
		public virtual object HeaderHandler(Header[] h)
		{
			SerializationMonkey serializationMonkey = (SerializationMonkey)FormatterServices.GetUninitializedObject(typeof(SerializationMonkey));
			Header[] array;
			if (h != null && h.Length > 0 && h[0].Name == "__methodName")
			{
				this.methodName = (string)h[0].Value;
				if (h.Length > 1)
				{
					array = new Header[h.Length - 1];
					Array.Copy(h, 1, array, 0, h.Length - 1);
				}
				else
				{
					array = null;
				}
			}
			else
			{
				array = h;
			}
			this.FillHeaders(array, true);
			this.ResolveMethod(false);
			serializationMonkey._obj = this;
			if (this.MI != null)
			{
				ArgMapper argMapper = new ArgMapper(this.MI, false);
				serializationMonkey.fieldNames = argMapper.ArgNames;
				serializationMonkey.fieldTypes = argMapper.ArgTypes;
			}
			return serializationMonkey;
		}

		// Token: 0x0400206C RID: 8300
		private const BindingFlags LookupAll = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x0400206D RID: 8301
		private const BindingFlags LookupPublic = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x0400206E RID: 8302
		private string uri;

		// Token: 0x0400206F RID: 8303
		private string methodName;

		// Token: 0x04002070 RID: 8304
		private MethodBase MI;

		// Token: 0x04002071 RID: 8305
		private string typeName;

		// Token: 0x04002072 RID: 8306
		private object[] args;

		// Token: 0x04002073 RID: 8307
		private Type[] instArgs;

		// Token: 0x04002074 RID: 8308
		private LogicalCallContext callContext;

		// Token: 0x04002075 RID: 8309
		private Type[] methodSignature;

		// Token: 0x04002076 RID: 8310
		protected IDictionary ExternalProperties;

		// Token: 0x04002077 RID: 8311
		protected IDictionary InternalProperties;

		// Token: 0x04002078 RID: 8312
		private ServerIdentity srvID;

		// Token: 0x04002079 RID: 8313
		private Identity identity;

		// Token: 0x0400207A RID: 8314
		private bool fSoap;

		// Token: 0x0400207B RID: 8315
		private bool fVarArgs;

		// Token: 0x0400207C RID: 8316
		private ArgMapper argMapper;
	}
}
