using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006FA RID: 1786
	[Serializable]
	internal class Message : IMethodCallMessage, IMethodMessage, IMessage, IInternalMessage, ISerializable
	{
		// Token: 0x06003FE5 RID: 16357 RVA: 0x000DA8E4 File Offset: 0x000D98E4
		public virtual Exception GetFault()
		{
			return this._Fault;
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x000DA8EC File Offset: 0x000D98EC
		public virtual void SetFault(Exception e)
		{
			this._Fault = e;
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x000DA8F5 File Offset: 0x000D98F5
		internal virtual void SetOneWay()
		{
			this._flags |= 8;
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x000DA905 File Offset: 0x000D9905
		public virtual int GetCallType()
		{
			this.InitIfNecessary();
			return this._flags;
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x000DA913 File Offset: 0x000D9913
		internal IntPtr GetFramePtr()
		{
			return this._frame;
		}

		// Token: 0x06003FEA RID: 16362
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetAsyncBeginInfo(out AsyncCallback acbd, out object state);

		// Token: 0x06003FEB RID: 16363
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetThisPtr();

		// Token: 0x06003FEC RID: 16364
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IAsyncResult GetAsyncResult();

		// Token: 0x06003FED RID: 16365 RVA: 0x000DA91B File Offset: 0x000D991B
		public void Init()
		{
		}

		// Token: 0x06003FEE RID: 16366
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetReturnValue();

		// Token: 0x06003FEF RID: 16367 RVA: 0x000DA91D File Offset: 0x000D991D
		internal Message()
		{
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x000DA928 File Offset: 0x000D9928
		internal void InitFields(MessageData msgData)
		{
			this._frame = msgData.pFrame;
			this._delegateMD = msgData.pDelegateMD;
			this._methodDesc = msgData.pMethodDesc;
			this._flags = msgData.iFlags;
			this._initDone = true;
			this._metaSigHolder = msgData.pSig;
			this._governingType = msgData.thGoverningType;
			this._MethodName = null;
			this._MethodSignature = null;
			this._MethodBase = null;
			this._URI = null;
			this._Fault = null;
			this._ID = null;
			this._srvID = null;
			this._callContext = null;
			if (this._properties != null)
			{
				((IDictionary)this._properties).Clear();
			}
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x000DA9DA File Offset: 0x000D99DA
		private void InitIfNecessary()
		{
			if (!this._initDone)
			{
				this.Init();
				this._initDone = true;
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06003FF2 RID: 16370 RVA: 0x000DA9F1 File Offset: 0x000D99F1
		// (set) Token: 0x06003FF3 RID: 16371 RVA: 0x000DA9F9 File Offset: 0x000D99F9
		ServerIdentity IInternalMessage.ServerIdentityObject
		{
			get
			{
				return this._srvID;
			}
			set
			{
				this._srvID = value;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x000DAA02 File Offset: 0x000D9A02
		// (set) Token: 0x06003FF5 RID: 16373 RVA: 0x000DAA0A File Offset: 0x000D9A0A
		Identity IInternalMessage.IdentityObject
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x000DAA13 File Offset: 0x000D9A13
		void IInternalMessage.SetURI(string URI)
		{
			this._URI = URI;
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x000DAA1C File Offset: 0x000D9A1C
		void IInternalMessage.SetCallContext(LogicalCallContext callContext)
		{
			this._callContext = callContext;
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x000DAA25 File Offset: 0x000D9A25
		bool IInternalMessage.HasProperties()
		{
			return this._properties != null;
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003FF9 RID: 16377 RVA: 0x000DAA33 File Offset: 0x000D9A33
		public IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					Interlocked.CompareExchange(ref this._properties, new MCMDictionary(this, null), null);
				}
				return (IDictionary)this._properties;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06003FFA RID: 16378 RVA: 0x000DAA5C File Offset: 0x000D9A5C
		// (set) Token: 0x06003FFB RID: 16379 RVA: 0x000DAA64 File Offset: 0x000D9A64
		public string Uri
		{
			get
			{
				return this._URI;
			}
			set
			{
				this._URI = value;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06003FFC RID: 16380 RVA: 0x000DAA70 File Offset: 0x000D9A70
		public bool HasVarArgs
		{
			get
			{
				if ((this._flags & 16) == 0 && (this._flags & 32) == 0)
				{
					if (!this.InternalHasVarArgs())
					{
						this._flags |= 16;
					}
					else
					{
						this._flags |= 32;
					}
				}
				return 1 == (this._flags & 32);
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06003FFD RID: 16381 RVA: 0x000DAAC7 File Offset: 0x000D9AC7
		public int ArgCount
		{
			get
			{
				return this.InternalGetArgCount();
			}
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x000DAACF File Offset: 0x000D9ACF
		public object GetArg(int argNum)
		{
			return this.InternalGetArg(argNum);
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x000DAAD8 File Offset: 0x000D9AD8
		public string GetArgName(int index)
		{
			if (index >= this.ArgCount)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.GetMethodBase());
			ParameterInfo[] parameters = reflectionCachedData.Parameters;
			if (index < parameters.Length)
			{
				return parameters[index].Name;
			}
			return "VarArg" + (index - parameters.Length);
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06004000 RID: 16384 RVA: 0x000DAB2F File Offset: 0x000D9B2F
		public object[] Args
		{
			get
			{
				return this.InternalGetArgs();
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06004001 RID: 16385 RVA: 0x000DAB37 File Offset: 0x000D9B37
		public int InArgCount
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, false);
				}
				return this._argMapper.ArgCount;
			}
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x000DAB59 File Offset: 0x000D9B59
		public object GetInArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x000DAB7C File Offset: 0x000D9B7C
		public string GetInArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x000DAB9F File Offset: 0x000D9B9F
		public object[] InArgs
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, false);
				}
				return this._argMapper.Args;
			}
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x000DABC4 File Offset: 0x000D9BC4
		private void UpdateNames()
		{
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.GetMethodBase());
			this._typeName = reflectionCachedData.TypeAndAssemblyName;
			this._MethodName = reflectionCachedData.MethodName;
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06004006 RID: 16390 RVA: 0x000DABF5 File Offset: 0x000D9BF5
		public string MethodName
		{
			get
			{
				if (this._MethodName == null)
				{
					this.UpdateNames();
				}
				return this._MethodName;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06004007 RID: 16391 RVA: 0x000DAC0B File Offset: 0x000D9C0B
		public string TypeName
		{
			get
			{
				if (this._typeName == null)
				{
					this.UpdateNames();
				}
				return this._typeName;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004008 RID: 16392 RVA: 0x000DAC21 File Offset: 0x000D9C21
		public object MethodSignature
		{
			get
			{
				if (this._MethodSignature == null)
				{
					this._MethodSignature = Message.GenerateMethodSignature(this.GetMethodBase());
				}
				return this._MethodSignature;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06004009 RID: 16393 RVA: 0x000DAC42 File Offset: 0x000D9C42
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x0600400A RID: 16394 RVA: 0x000DAC4A File Offset: 0x000D9C4A
		public MethodBase MethodBase
		{
			get
			{
				return this.GetMethodBase();
			}
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x000DAC52 File Offset: 0x000D9C52
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x000DAC64 File Offset: 0x000D9C64
		internal unsafe MethodBase GetMethodBase()
		{
			if (this._MethodBase == null)
			{
				RuntimeMethodHandle runtimeMethodHandle = new RuntimeMethodHandle((void*)this._methodDesc);
				RuntimeTypeHandle runtimeTypeHandle = new RuntimeTypeHandle((void*)this._governingType);
				this._MethodBase = RuntimeType.GetMethodBase(runtimeTypeHandle, runtimeMethodHandle);
			}
			return this._MethodBase;
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x000DACB0 File Offset: 0x000D9CB0
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			LogicalCallContext callContext = this._callContext;
			this._callContext = callCtx;
			return callContext;
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x000DACCC File Offset: 0x000D9CCC
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this._callContext == null)
			{
				this._callContext = new LogicalCallContext();
			}
			return this._callContext;
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x000DACE8 File Offset: 0x000D9CE8
		internal static Type[] GenerateMethodSignature(MethodBase mb)
		{
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(mb);
			ParameterInfo[] parameters = reflectionCachedData.Parameters;
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			return array;
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x000DAD28 File Offset: 0x000D9D28
		internal static object[] CoerceArgs(IMethodMessage m)
		{
			MethodBase methodBase = m.MethodBase;
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(methodBase);
			return Message.CoerceArgs(m, reflectionCachedData.Parameters);
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x000DAD4F File Offset: 0x000D9D4F
		internal static object[] CoerceArgs(IMethodMessage m, ParameterInfo[] pi)
		{
			return Message.CoerceArgs(m.MethodBase, m.Args, pi);
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x000DAD64 File Offset: 0x000D9D64
		internal static object[] CoerceArgs(MethodBase mb, object[] args, ParameterInfo[] pi)
		{
			if (pi == null)
			{
				throw new ArgumentNullException("pi");
			}
			if (pi.Length != args.Length)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_ArgMismatch"), new object[]
				{
					mb.DeclaringType.FullName,
					mb.Name,
					args.Length,
					pi.Length
				}));
			}
			for (int i = 0; i < pi.Length; i++)
			{
				ParameterInfo parameterInfo = pi[i];
				Type parameterType = parameterInfo.ParameterType;
				object obj = args[i];
				if (obj != null)
				{
					args[i] = Message.CoerceArg(obj, parameterType);
				}
				else if (parameterType.IsByRef)
				{
					Type elementType = parameterType.GetElementType();
					if (elementType.IsValueType)
					{
						if (parameterInfo.IsOut)
						{
							args[i] = Activator.CreateInstance(elementType, true);
						}
						else if (!elementType.IsGenericType || elementType.GetGenericTypeDefinition() != typeof(Nullable<>))
						{
							throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MissingArgValue"), new object[] { elementType.FullName, i }));
						}
					}
				}
				else if (parameterType.IsValueType && (!parameterType.IsGenericType || parameterType.GetGenericTypeDefinition() != typeof(Nullable<>)))
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MissingArgValue"), new object[] { parameterType.FullName, i }));
				}
			}
			return args;
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x000DAEFC File Offset: 0x000D9EFC
		internal static object CoerceArg(object value, Type pt)
		{
			object obj = null;
			if (value != null)
			{
				Exception ex = null;
				try
				{
					if (pt.IsByRef)
					{
						pt = pt.GetElementType();
					}
					if (pt.IsInstanceOfType(value))
					{
						obj = value;
					}
					else
					{
						obj = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
					}
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
				if (obj == null)
				{
					string text;
					if (RemotingServices.IsTransparentProxy(value))
					{
						text = typeof(MarshalByRefObject).ToString();
					}
					else
					{
						text = value.ToString();
					}
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_CoercionFailed"), new object[] { text, pt }), ex);
				}
			}
			return obj;
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x000DAFAC File Offset: 0x000D9FAC
		internal static object SoapCoerceArg(object value, Type pt, Hashtable keyToNamespaceTable)
		{
			object obj = null;
			if (value != null)
			{
				try
				{
					if (pt.IsByRef)
					{
						pt = pt.GetElementType();
					}
					if (pt.IsInstanceOfType(value))
					{
						obj = value;
					}
					else
					{
						string text = value as string;
						if (text != null)
						{
							if (pt == typeof(double))
							{
								if (text == "INF")
								{
									obj = double.PositiveInfinity;
								}
								else if (text == "-INF")
								{
									obj = double.NegativeInfinity;
								}
								else
								{
									obj = double.Parse(text, CultureInfo.InvariantCulture);
								}
							}
							else if (pt == typeof(float))
							{
								if (text == "INF")
								{
									obj = float.PositiveInfinity;
								}
								else if (text == "-INF")
								{
									obj = float.NegativeInfinity;
								}
								else
								{
									obj = float.Parse(text, CultureInfo.InvariantCulture);
								}
							}
							else if (SoapType.typeofISoapXsd.IsAssignableFrom(pt))
							{
								if (pt == SoapType.typeofSoapTime)
								{
									obj = SoapTime.Parse(text);
								}
								else if (pt == SoapType.typeofSoapDate)
								{
									obj = SoapDate.Parse(text);
								}
								else if (pt == SoapType.typeofSoapYearMonth)
								{
									obj = SoapYearMonth.Parse(text);
								}
								else if (pt == SoapType.typeofSoapYear)
								{
									obj = SoapYear.Parse(text);
								}
								else if (pt == SoapType.typeofSoapMonthDay)
								{
									obj = SoapMonthDay.Parse(text);
								}
								else if (pt == SoapType.typeofSoapDay)
								{
									obj = SoapDay.Parse(text);
								}
								else if (pt == SoapType.typeofSoapMonth)
								{
									obj = SoapMonth.Parse(text);
								}
								else if (pt == SoapType.typeofSoapHexBinary)
								{
									obj = SoapHexBinary.Parse(text);
								}
								else if (pt == SoapType.typeofSoapBase64Binary)
								{
									obj = SoapBase64Binary.Parse(text);
								}
								else if (pt == SoapType.typeofSoapInteger)
								{
									obj = SoapInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapPositiveInteger)
								{
									obj = SoapPositiveInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNonPositiveInteger)
								{
									obj = SoapNonPositiveInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNonNegativeInteger)
								{
									obj = SoapNonNegativeInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNegativeInteger)
								{
									obj = SoapNegativeInteger.Parse(text);
								}
								else if (pt == SoapType.typeofSoapAnyUri)
								{
									obj = SoapAnyUri.Parse(text);
								}
								else if (pt == SoapType.typeofSoapQName)
								{
									obj = SoapQName.Parse(text);
									SoapQName soapQName = (SoapQName)obj;
									if (soapQName.Key.Length == 0)
									{
										soapQName.Namespace = (string)keyToNamespaceTable["xmlns"];
									}
									else
									{
										soapQName.Namespace = (string)keyToNamespaceTable["xmlns:" + soapQName.Key];
									}
								}
								else if (pt == SoapType.typeofSoapNotation)
								{
									obj = SoapNotation.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNormalizedString)
								{
									obj = SoapNormalizedString.Parse(text);
								}
								else if (pt == SoapType.typeofSoapToken)
								{
									obj = SoapToken.Parse(text);
								}
								else if (pt == SoapType.typeofSoapLanguage)
								{
									obj = SoapLanguage.Parse(text);
								}
								else if (pt == SoapType.typeofSoapName)
								{
									obj = SoapName.Parse(text);
								}
								else if (pt == SoapType.typeofSoapIdrefs)
								{
									obj = SoapIdrefs.Parse(text);
								}
								else if (pt == SoapType.typeofSoapEntities)
								{
									obj = SoapEntities.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNmtoken)
								{
									obj = SoapNmtoken.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNmtokens)
								{
									obj = SoapNmtokens.Parse(text);
								}
								else if (pt == SoapType.typeofSoapNcName)
								{
									obj = SoapNcName.Parse(text);
								}
								else if (pt == SoapType.typeofSoapId)
								{
									obj = SoapId.Parse(text);
								}
								else if (pt == SoapType.typeofSoapIdref)
								{
									obj = SoapIdref.Parse(text);
								}
								else if (pt == SoapType.typeofSoapEntity)
								{
									obj = SoapEntity.Parse(text);
								}
							}
							else if (pt == typeof(bool))
							{
								if (text == "1" || text == "true")
								{
									obj = true;
								}
								else
								{
									if (!(text == "0") && !(text == "false"))
									{
										throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_CoercionFailed"), new object[] { text, pt }));
									}
									obj = false;
								}
							}
							else if (pt == typeof(DateTime))
							{
								obj = SoapDateTime.Parse(text);
							}
							else if (pt.IsPrimitive)
							{
								obj = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
							}
							else if (pt == typeof(TimeSpan))
							{
								obj = SoapDuration.Parse(text);
							}
							else if (pt == typeof(char))
							{
								obj = text[0];
							}
							else
							{
								obj = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
							}
						}
						else
						{
							obj = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
						}
					}
				}
				catch (Exception)
				{
				}
				if (obj == null)
				{
					string text2;
					if (RemotingServices.IsTransparentProxy(value))
					{
						text2 = typeof(MarshalByRefObject).ToString();
					}
					else
					{
						text2 = value.ToString();
					}
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_CoercionFailed"), new object[] { text2, pt }));
				}
			}
			return obj;
		}

		// Token: 0x06004015 RID: 16405
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool InternalHasVarArgs();

		// Token: 0x06004016 RID: 16406
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int InternalGetArgCount();

		// Token: 0x06004017 RID: 16407
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object InternalGetArg(int argNum);

		// Token: 0x06004018 RID: 16408
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object[] InternalGetArgs();

		// Token: 0x06004019 RID: 16409
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void PropagateOutParameters(object[] OutArgs, object retVal);

		// Token: 0x0600401A RID: 16410
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Dispatch(object target, bool fExecuteInContext);

		// Token: 0x0600401B RID: 16411 RVA: 0x000DB4DC File Offset: 0x000DA4DC
		[Conditional("_REMOTING_DEBUG")]
		public static void DebugOut(string s)
		{
			Message.OutToUnmanagedDebugger(string.Concat(new object[]
			{
				"\nRMTING: Thrd ",
				Thread.CurrentThread.GetHashCode(),
				" : ",
				s
			}));
		}

		// Token: 0x0600401C RID: 16412
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OutToUnmanagedDebugger(string s);

		// Token: 0x0600401D RID: 16413 RVA: 0x000DB521 File Offset: 0x000DA521
		internal static LogicalCallContext PropagateCallContextFromMessageToThread(IMessage msg)
		{
			return CallContext.SetLogicalCallContext((LogicalCallContext)msg.Properties[Message.CallContextKey]);
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x000DB540 File Offset: 0x000DA540
		internal static void PropagateCallContextFromThreadToMessage(IMessage msg)
		{
			LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
			msg.Properties[Message.CallContextKey] = logicalCallContext;
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x000DB564 File Offset: 0x000DA564
		internal static void PropagateCallContextFromThreadToMessage(IMessage msg, LogicalCallContext oldcctx)
		{
			Message.PropagateCallContextFromThreadToMessage(msg);
			CallContext.SetLogicalCallContext(oldcctx);
		}

		// Token: 0x0400201F RID: 8223
		internal const int Sync = 0;

		// Token: 0x04002020 RID: 8224
		internal const int BeginAsync = 1;

		// Token: 0x04002021 RID: 8225
		internal const int EndAsync = 2;

		// Token: 0x04002022 RID: 8226
		internal const int Ctor = 4;

		// Token: 0x04002023 RID: 8227
		internal const int OneWay = 8;

		// Token: 0x04002024 RID: 8228
		internal const int CallMask = 15;

		// Token: 0x04002025 RID: 8229
		internal const int FixedArgs = 16;

		// Token: 0x04002026 RID: 8230
		internal const int VarArgs = 32;

		// Token: 0x04002027 RID: 8231
		private string _MethodName;

		// Token: 0x04002028 RID: 8232
		private Type[] _MethodSignature;

		// Token: 0x04002029 RID: 8233
		private MethodBase _MethodBase;

		// Token: 0x0400202A RID: 8234
		private object _properties;

		// Token: 0x0400202B RID: 8235
		private string _URI;

		// Token: 0x0400202C RID: 8236
		private string _typeName;

		// Token: 0x0400202D RID: 8237
		private Exception _Fault;

		// Token: 0x0400202E RID: 8238
		private Identity _ID;

		// Token: 0x0400202F RID: 8239
		private ServerIdentity _srvID;

		// Token: 0x04002030 RID: 8240
		private ArgMapper _argMapper;

		// Token: 0x04002031 RID: 8241
		private LogicalCallContext _callContext;

		// Token: 0x04002032 RID: 8242
		private IntPtr _frame;

		// Token: 0x04002033 RID: 8243
		private IntPtr _methodDesc;

		// Token: 0x04002034 RID: 8244
		private IntPtr _metaSigHolder;

		// Token: 0x04002035 RID: 8245
		private IntPtr _delegateMD;

		// Token: 0x04002036 RID: 8246
		private IntPtr _governingType;

		// Token: 0x04002037 RID: 8247
		private int _flags;

		// Token: 0x04002038 RID: 8248
		private bool _initDone;

		// Token: 0x04002039 RID: 8249
		internal static string CallContextKey = "__CallContext";

		// Token: 0x0400203A RID: 8250
		internal static string UriKey = "__Uri";
	}
}
