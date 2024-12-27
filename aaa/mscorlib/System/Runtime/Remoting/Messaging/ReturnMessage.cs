using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006FB RID: 1787
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ReturnMessage : IMethodReturnMessage, IMethodMessage, IMessage
	{
		// Token: 0x06004021 RID: 16417 RVA: 0x000DB58C File Offset: 0x000DA58C
		public ReturnMessage(object ret, object[] outArgs, int outArgsCount, LogicalCallContext callCtx, IMethodCallMessage mcm)
		{
			this._ret = ret;
			this._outArgs = outArgs;
			this._outArgsCount = outArgsCount;
			if (callCtx != null)
			{
				this._callContext = callCtx;
			}
			else
			{
				this._callContext = CallContext.GetLogicalCallContext();
			}
			if (mcm != null)
			{
				this._URI = mcm.Uri;
				this._methodName = mcm.MethodName;
				this._methodSignature = null;
				this._typeName = mcm.TypeName;
				this._hasVarArgs = mcm.HasVarArgs;
				this._methodBase = mcm.MethodBase;
			}
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x000DB61C File Offset: 0x000DA61C
		public ReturnMessage(Exception e, IMethodCallMessage mcm)
		{
			this._e = (ReturnMessage.IsCustomErrorEnabled() ? new RemotingException(Environment.GetResourceString("Remoting_InternalError")) : e);
			this._callContext = CallContext.GetLogicalCallContext();
			if (mcm != null)
			{
				this._URI = mcm.Uri;
				this._methodName = mcm.MethodName;
				this._methodSignature = null;
				this._typeName = mcm.TypeName;
				this._hasVarArgs = mcm.HasVarArgs;
				this._methodBase = mcm.MethodBase;
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06004023 RID: 16419 RVA: 0x000DB69F File Offset: 0x000DA69F
		// (set) Token: 0x06004024 RID: 16420 RVA: 0x000DB6A7 File Offset: 0x000DA6A7
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

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06004025 RID: 16421 RVA: 0x000DB6B0 File Offset: 0x000DA6B0
		public string MethodName
		{
			get
			{
				return this._methodName;
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06004026 RID: 16422 RVA: 0x000DB6B8 File Offset: 0x000DA6B8
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06004027 RID: 16423 RVA: 0x000DB6C0 File Offset: 0x000DA6C0
		public object MethodSignature
		{
			get
			{
				if (this._methodSignature == null && this._methodBase != null)
				{
					this._methodSignature = Message.GenerateMethodSignature(this._methodBase);
				}
				return this._methodSignature;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06004028 RID: 16424 RVA: 0x000DB6E9 File Offset: 0x000DA6E9
		public MethodBase MethodBase
		{
			get
			{
				return this._methodBase;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06004029 RID: 16425 RVA: 0x000DB6F1 File Offset: 0x000DA6F1
		public bool HasVarArgs
		{
			get
			{
				return this._hasVarArgs;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x0600402A RID: 16426 RVA: 0x000DB6F9 File Offset: 0x000DA6F9
		public int ArgCount
		{
			get
			{
				if (this._outArgs == null)
				{
					return this._outArgsCount;
				}
				return this._outArgs.Length;
			}
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x000DB714 File Offset: 0x000DA714
		public object GetArg(int argNum)
		{
			if (this._outArgs == null)
			{
				if (argNum < 0 || argNum >= this._outArgsCount)
				{
					throw new ArgumentOutOfRangeException("argNum");
				}
				return null;
			}
			else
			{
				if (argNum < 0 || argNum >= this._outArgs.Length)
				{
					throw new ArgumentOutOfRangeException("argNum");
				}
				return this._outArgs[argNum];
			}
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x000DB768 File Offset: 0x000DA768
		public string GetArgName(int index)
		{
			if (this._outArgs == null)
			{
				if (index < 0 || index >= this._outArgsCount)
				{
					throw new ArgumentOutOfRangeException("index");
				}
			}
			else if (index < 0 || index >= this._outArgs.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (this._methodBase != null)
			{
				RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this._methodBase);
				return reflectionCachedData.Parameters[index].Name;
			}
			return "__param" + index;
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x000DB7E1 File Offset: 0x000DA7E1
		public object[] Args
		{
			get
			{
				if (this._outArgs == null)
				{
					return new object[this._outArgsCount];
				}
				return this._outArgs;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x0600402E RID: 16430 RVA: 0x000DB7FD File Offset: 0x000DA7FD
		public int OutArgCount
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, true);
				}
				return this._argMapper.ArgCount;
			}
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x000DB81F File Offset: 0x000DA81F
		public object GetOutArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x000DB842 File Offset: 0x000DA842
		public string GetOutArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x000DB865 File Offset: 0x000DA865
		public object[] OutArgs
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, true);
				}
				return this._argMapper.Args;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x000DB887 File Offset: 0x000DA887
		public Exception Exception
		{
			get
			{
				return this._e;
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06004033 RID: 16435 RVA: 0x000DB88F File Offset: 0x000DA88F
		public virtual object ReturnValue
		{
			get
			{
				return this._ret;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x000DB897 File Offset: 0x000DA897
		public virtual IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					this._properties = new MRMDictionary(this, null);
				}
				return (MRMDictionary)this._properties;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06004035 RID: 16437 RVA: 0x000DB8B9 File Offset: 0x000DA8B9
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x000DB8C1 File Offset: 0x000DA8C1
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this._callContext == null)
			{
				this._callContext = new LogicalCallContext();
			}
			return this._callContext;
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x000DB8DC File Offset: 0x000DA8DC
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext ctx)
		{
			LogicalCallContext callContext = this._callContext;
			this._callContext = ctx;
			return callContext;
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x000DB8F8 File Offset: 0x000DA8F8
		internal bool HasProperties()
		{
			return this._properties != null;
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x000DB908 File Offset: 0x000DA908
		internal static bool IsCustomErrorEnabled()
		{
			object data = CallContext.GetData("__CustomErrorsEnabled");
			return data != null && (bool)data;
		}

		// Token: 0x0400203B RID: 8251
		internal object _ret;

		// Token: 0x0400203C RID: 8252
		internal object _properties;

		// Token: 0x0400203D RID: 8253
		internal string _URI;

		// Token: 0x0400203E RID: 8254
		internal Exception _e;

		// Token: 0x0400203F RID: 8255
		internal object[] _outArgs;

		// Token: 0x04002040 RID: 8256
		internal int _outArgsCount;

		// Token: 0x04002041 RID: 8257
		internal string _methodName;

		// Token: 0x04002042 RID: 8258
		internal string _typeName;

		// Token: 0x04002043 RID: 8259
		internal Type[] _methodSignature;

		// Token: 0x04002044 RID: 8260
		internal bool _hasVarArgs;

		// Token: 0x04002045 RID: 8261
		internal LogicalCallContext _callContext;

		// Token: 0x04002046 RID: 8262
		internal ArgMapper _argMapper;

		// Token: 0x04002047 RID: 8263
		internal MethodBase _methodBase;
	}
}
