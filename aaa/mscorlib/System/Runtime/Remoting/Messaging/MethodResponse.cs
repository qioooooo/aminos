using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000708 RID: 1800
	[ComVisible(true)]
	[CLSCompliant(false)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class MethodResponse : IMethodReturnMessage, IMethodMessage, IMessage, ISerializable, ISerializationRootObject, IInternalMessage
	{
		// Token: 0x060040E9 RID: 16617 RVA: 0x000DDDE0 File Offset: 0x000DCDE0
		public MethodResponse(Header[] h1, IMethodCallMessage mcm)
		{
			if (mcm == null)
			{
				throw new ArgumentNullException("mcm");
			}
			Message message = mcm as Message;
			if (message != null)
			{
				this.MI = message.GetMethodBase();
			}
			else
			{
				this.MI = mcm.MethodBase;
			}
			if (this.MI == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MethodMissing"), new object[] { mcm.MethodName, mcm.TypeName }));
			}
			this._methodCache = InternalRemotingServices.GetReflectionCachedData(this.MI);
			this.argCount = this._methodCache.Parameters.Length;
			this.fSoap = true;
			this.FillHeaders(h1);
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x000DDE94 File Offset: 0x000DCE94
		internal MethodResponse(IMethodCallMessage msg, SmuggledMethodReturnMessage smuggledMrm, ArrayList deserializedArgs)
		{
			this.MI = msg.MethodBase;
			this._methodCache = InternalRemotingServices.GetReflectionCachedData(this.MI);
			this.methodName = msg.MethodName;
			this.uri = msg.Uri;
			this.typeName = msg.TypeName;
			if (this._methodCache.IsOverloaded())
			{
				this.methodSignature = (Type[])msg.MethodSignature;
			}
			this.retVal = smuggledMrm.GetReturnValue(deserializedArgs);
			this.outArgs = smuggledMrm.GetArgs(deserializedArgs);
			this.fault = smuggledMrm.GetException(deserializedArgs);
			this.callContext = smuggledMrm.GetCallContext(deserializedArgs);
			if (smuggledMrm.MessagePropertyCount > 0)
			{
				smuggledMrm.PopulateMessageProperties(this.Properties, deserializedArgs);
			}
			this.argCount = this._methodCache.Parameters.Length;
			this.fSoap = false;
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x000DDF6C File Offset: 0x000DCF6C
		internal MethodResponse(IMethodCallMessage msg, object handlerObject, BinaryMethodReturnMessage smuggledMrm)
		{
			if (msg != null)
			{
				this.MI = msg.MethodBase;
				this._methodCache = InternalRemotingServices.GetReflectionCachedData(this.MI);
				this.methodName = msg.MethodName;
				this.uri = msg.Uri;
				this.typeName = msg.TypeName;
				if (this._methodCache.IsOverloaded())
				{
					this.methodSignature = (Type[])msg.MethodSignature;
				}
				this.argCount = this._methodCache.Parameters.Length;
			}
			this.retVal = smuggledMrm.ReturnValue;
			this.outArgs = smuggledMrm.Args;
			this.fault = smuggledMrm.Exception;
			this.callContext = smuggledMrm.LogicalCallContext;
			if (smuggledMrm.HasProperties)
			{
				smuggledMrm.PopulateMessageProperties(this.Properties);
			}
			this.fSoap = false;
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x000DE03F File Offset: 0x000DD03F
		internal MethodResponse(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.SetObjectData(info, context);
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x000DE060 File Offset: 0x000DD060
		public virtual object HeaderHandler(Header[] h)
		{
			SerializationMonkey serializationMonkey = (SerializationMonkey)FormatterServices.GetUninitializedObject(typeof(SerializationMonkey));
			Header[] array;
			if (h != null && h.Length > 0 && h[0].Name == "__methodName")
			{
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
			Type type = null;
			MethodInfo methodInfo = this.MI as MethodInfo;
			if (methodInfo != null)
			{
				type = methodInfo.ReturnType;
			}
			ParameterInfo[] parameters = this._methodCache.Parameters;
			int num = this._methodCache.MarshalResponseArgMap.Length;
			if (type != null && type != typeof(void))
			{
				num++;
			}
			Type[] array2 = new Type[num];
			string[] array3 = new string[num];
			int num2 = 0;
			if (type != null && type != typeof(void))
			{
				array2[num2++] = type;
			}
			foreach (int num3 in this._methodCache.MarshalResponseArgMap)
			{
				array3[num2] = parameters[num3].Name;
				if (parameters[num3].ParameterType.IsByRef)
				{
					array2[num2++] = parameters[num3].ParameterType.GetElementType();
				}
				else
				{
					array2[num2++] = parameters[num3].ParameterType;
				}
			}
			((IFieldInfo)serializationMonkey).FieldTypes = array2;
			((IFieldInfo)serializationMonkey).FieldNames = array3;
			this.FillHeaders(array, true);
			serializationMonkey._obj = this;
			return serializationMonkey;
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x000DE1D8 File Offset: 0x000DD1D8
		public void RootSetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			this.SetObjectData(info, ctx);
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x000DE1E4 File Offset: 0x000DD1E4
		internal void SetObjectData(SerializationInfo info, StreamingContext ctx)
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
			bool flag = false;
			bool flag2 = false;
			while (enumerator.MoveNext())
			{
				if (enumerator.Name.Equals("__return"))
				{
					flag = true;
					break;
				}
				if (enumerator.Name.Equals("__fault"))
				{
					flag2 = true;
					this.fault = (Exception)enumerator.Value;
					break;
				}
				this.FillHeader(enumerator.Name, enumerator.Value);
			}
			if (flag2 && flag)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
			}
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x000DE289 File Offset: 0x000DD289
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x000DE29C File Offset: 0x000DD29C
		internal void SetObjectFromSoapData(SerializationInfo info)
		{
			Hashtable hashtable = (Hashtable)info.GetValue("__keyToNamespaceTable", typeof(Hashtable));
			ArrayList arrayList = (ArrayList)info.GetValue("__paramNameList", typeof(ArrayList));
			SoapFault soapFault = (SoapFault)info.GetValue("__fault", typeof(SoapFault));
			if (soapFault != null)
			{
				ServerFault serverFault = soapFault.Detail as ServerFault;
				if (serverFault != null)
				{
					if (serverFault.Exception != null)
					{
						this.fault = serverFault.Exception;
						return;
					}
					Type type = Type.GetType(serverFault.ExceptionType, false, false);
					if (type == null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("\nException Type: ");
						stringBuilder.Append(serverFault.ExceptionType);
						stringBuilder.Append("\n");
						stringBuilder.Append("Exception Message: ");
						stringBuilder.Append(serverFault.ExceptionMessage);
						stringBuilder.Append("\n");
						stringBuilder.Append(serverFault.StackTrace);
						this.fault = new ServerException(stringBuilder.ToString());
						return;
					}
					object[] array = new object[] { serverFault.ExceptionMessage };
					this.fault = (Exception)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, array, null, null);
					return;
				}
				else
				{
					if (soapFault.Detail != null && soapFault.Detail.GetType() == typeof(string) && ((string)soapFault.Detail).Length != 0)
					{
						this.fault = new ServerException((string)soapFault.Detail);
						return;
					}
					this.fault = new ServerException(soapFault.FaultString);
					return;
				}
			}
			else
			{
				MethodInfo methodInfo = this.MI as MethodInfo;
				int num = 0;
				if (methodInfo != null)
				{
					Type returnType = methodInfo.ReturnType;
					if (returnType != typeof(void))
					{
						num++;
						object value = info.GetValue((string)arrayList[0], typeof(object));
						if (value is string)
						{
							this.retVal = Message.SoapCoerceArg(value, returnType, hashtable);
						}
						else
						{
							this.retVal = value;
						}
					}
				}
				ParameterInfo[] parameters = this._methodCache.Parameters;
				object obj = ((this.InternalProperties == null) ? null : this.InternalProperties["__UnorderedParams"]);
				if (obj != null && obj is bool && (bool)obj)
				{
					for (int i = num; i < arrayList.Count; i++)
					{
						string text = (string)arrayList[i];
						int num2 = -1;
						for (int j = 0; j < parameters.Length; j++)
						{
							if (text.Equals(parameters[j].Name))
							{
								num2 = parameters[j].Position;
							}
						}
						if (num2 == -1)
						{
							if (!text.StartsWith("__param", StringComparison.Ordinal))
							{
								throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
							}
							num2 = int.Parse(text.Substring(7), CultureInfo.InvariantCulture);
						}
						if (num2 >= this.argCount)
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadSerialization"));
						}
						if (this.outArgs == null)
						{
							this.outArgs = new object[this.argCount];
						}
						this.outArgs[num2] = Message.SoapCoerceArg(info.GetValue(text, typeof(object)), parameters[num2].ParameterType, hashtable);
					}
					return;
				}
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, true);
				}
				for (int k = num; k < arrayList.Count; k++)
				{
					string text2 = (string)arrayList[k];
					if (this.outArgs == null)
					{
						this.outArgs = new object[this.argCount];
					}
					int num3 = this.argMapper.Map[k - num];
					this.outArgs[num3] = Message.SoapCoerceArg(info.GetValue(text2, typeof(object)), parameters[num3].ParameterType, hashtable);
				}
				return;
			}
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x000DE682 File Offset: 0x000DD682
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this.callContext == null)
			{
				this.callContext = new LogicalCallContext();
			}
			return this.callContext;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x000DE6A0 File Offset: 0x000DD6A0
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext ctx)
		{
			LogicalCallContext logicalCallContext = this.callContext;
			this.callContext = ctx;
			return logicalCallContext;
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x060040F4 RID: 16628 RVA: 0x000DE6BC File Offset: 0x000DD6BC
		// (set) Token: 0x060040F5 RID: 16629 RVA: 0x000DE6C4 File Offset: 0x000DD6C4
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

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x060040F6 RID: 16630 RVA: 0x000DE6CD File Offset: 0x000DD6CD
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x060040F7 RID: 16631 RVA: 0x000DE6D5 File Offset: 0x000DD6D5
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060040F8 RID: 16632 RVA: 0x000DE6DD File Offset: 0x000DD6DD
		public object MethodSignature
		{
			get
			{
				return this.methodSignature;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060040F9 RID: 16633 RVA: 0x000DE6E5 File Offset: 0x000DD6E5
		public MethodBase MethodBase
		{
			get
			{
				return this.MI;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060040FA RID: 16634 RVA: 0x000DE6ED File Offset: 0x000DD6ED
		public bool HasVarArgs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060040FB RID: 16635 RVA: 0x000DE6F0 File Offset: 0x000DD6F0
		public int ArgCount
		{
			get
			{
				if (this.outArgs == null)
				{
					return 0;
				}
				return this.outArgs.Length;
			}
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x000DE704 File Offset: 0x000DD704
		public object GetArg(int argNum)
		{
			return this.outArgs[argNum];
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x000DE710 File Offset: 0x000DD710
		public string GetArgName(int index)
		{
			if (this.MI == null)
			{
				return "__param" + index;
			}
			RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this.MI);
			ParameterInfo[] parameters = reflectionCachedData.Parameters;
			if (index < 0 || index >= parameters.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return reflectionCachedData.Parameters[index].Name;
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060040FE RID: 16638 RVA: 0x000DE76B File Offset: 0x000DD76B
		public object[] Args
		{
			get
			{
				return this.outArgs;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060040FF RID: 16639 RVA: 0x000DE773 File Offset: 0x000DD773
		public int OutArgCount
		{
			get
			{
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, true);
				}
				return this.argMapper.ArgCount;
			}
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x000DE795 File Offset: 0x000DD795
		public object GetOutArg(int argNum)
		{
			if (this.argMapper == null)
			{
				this.argMapper = new ArgMapper(this, true);
			}
			return this.argMapper.GetArg(argNum);
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x000DE7B8 File Offset: 0x000DD7B8
		public string GetOutArgName(int index)
		{
			if (this.argMapper == null)
			{
				this.argMapper = new ArgMapper(this, true);
			}
			return this.argMapper.GetArgName(index);
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004102 RID: 16642 RVA: 0x000DE7DB File Offset: 0x000DD7DB
		public object[] OutArgs
		{
			get
			{
				if (this.argMapper == null)
				{
					this.argMapper = new ArgMapper(this, true);
				}
				return this.argMapper.Args;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004103 RID: 16643 RVA: 0x000DE7FD File Offset: 0x000DD7FD
		public Exception Exception
		{
			get
			{
				return this.fault;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004104 RID: 16644 RVA: 0x000DE805 File Offset: 0x000DD805
		public object ReturnValue
		{
			get
			{
				return this.retVal;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x000DE810 File Offset: 0x000DD810
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
						this.ExternalProperties = new MRMDictionary(this, this.InternalProperties);
					}
					externalProperties = this.ExternalProperties;
				}
				return externalProperties;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06004106 RID: 16646 RVA: 0x000DE874 File Offset: 0x000DD874
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x000DE87C File Offset: 0x000DD87C
		internal void FillHeaders(Header[] h)
		{
			this.FillHeaders(h, false);
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x000DE888 File Offset: 0x000DD888
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

		// Token: 0x06004109 RID: 16649 RVA: 0x000DE910 File Offset: 0x000DD910
		internal void FillHeader(string name, object value)
		{
			if (name.Equals("__MethodName"))
			{
				this.methodName = (string)value;
				return;
			}
			if (name.Equals("__Uri"))
			{
				this.uri = (string)value;
				return;
			}
			if (name.Equals("__MethodSignature"))
			{
				this.methodSignature = (Type[])value;
				return;
			}
			if (name.Equals("__TypeName"))
			{
				this.typeName = (string)value;
				return;
			}
			if (name.Equals("__OutArgs"))
			{
				this.outArgs = (object[])value;
				return;
			}
			if (name.Equals("__CallContext"))
			{
				if (value is string)
				{
					this.callContext = new LogicalCallContext();
					this.callContext.RemotingData.LogicalCallID = (string)value;
					return;
				}
				this.callContext = (LogicalCallContext)value;
				return;
			}
			else
			{
				if (name.Equals("__Return"))
				{
					this.retVal = value;
					return;
				}
				if (this.InternalProperties == null)
				{
					this.InternalProperties = new Hashtable();
				}
				this.InternalProperties[name] = value;
				return;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x0600410A RID: 16650 RVA: 0x000DEA18 File Offset: 0x000DDA18
		// (set) Token: 0x0600410B RID: 16651 RVA: 0x000DEA1B File Offset: 0x000DDA1B
		ServerIdentity IInternalMessage.ServerIdentityObject
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x0600410C RID: 16652 RVA: 0x000DEA1D File Offset: 0x000DDA1D
		// (set) Token: 0x0600410D RID: 16653 RVA: 0x000DEA20 File Offset: 0x000DDA20
		Identity IInternalMessage.IdentityObject
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x000DEA22 File Offset: 0x000DDA22
		void IInternalMessage.SetURI(string val)
		{
			this.uri = val;
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x000DEA2B File Offset: 0x000DDA2B
		void IInternalMessage.SetCallContext(LogicalCallContext newCallContext)
		{
			this.callContext = newCallContext;
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x000DEA34 File Offset: 0x000DDA34
		bool IInternalMessage.HasProperties()
		{
			return this.ExternalProperties != null || this.InternalProperties != null;
		}

		// Token: 0x04002082 RID: 8322
		private MethodBase MI;

		// Token: 0x04002083 RID: 8323
		private string methodName;

		// Token: 0x04002084 RID: 8324
		private Type[] methodSignature;

		// Token: 0x04002085 RID: 8325
		private string uri;

		// Token: 0x04002086 RID: 8326
		private string typeName;

		// Token: 0x04002087 RID: 8327
		private object retVal;

		// Token: 0x04002088 RID: 8328
		private Exception fault;

		// Token: 0x04002089 RID: 8329
		private object[] outArgs;

		// Token: 0x0400208A RID: 8330
		private LogicalCallContext callContext;

		// Token: 0x0400208B RID: 8331
		protected IDictionary InternalProperties;

		// Token: 0x0400208C RID: 8332
		protected IDictionary ExternalProperties;

		// Token: 0x0400208D RID: 8333
		private int argCount;

		// Token: 0x0400208E RID: 8334
		private bool fSoap;

		// Token: 0x0400208F RID: 8335
		private ArgMapper argMapper;

		// Token: 0x04002090 RID: 8336
		private RemotingMethodCachedData _methodCache;
	}
}
