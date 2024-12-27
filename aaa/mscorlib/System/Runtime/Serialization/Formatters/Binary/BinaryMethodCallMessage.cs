using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E6 RID: 2022
	[Serializable]
	internal sealed class BinaryMethodCallMessage
	{
		// Token: 0x06004802 RID: 18434 RVA: 0x000FA900 File Offset: 0x000F9900
		internal BinaryMethodCallMessage(string uri, string methodName, string typeName, Type[] instArgs, object[] args, object methodSignature, LogicalCallContext callContext, object[] properties)
		{
			this._methodName = methodName;
			this._typeName = typeName;
			if (args == null)
			{
				args = new object[0];
			}
			this._inargs = args;
			this._args = args;
			this._instArgs = instArgs;
			this._methodSignature = methodSignature;
			if (callContext == null)
			{
				this._logicalCallContext = new LogicalCallContext();
			}
			else
			{
				this._logicalCallContext = callContext;
			}
			this._properties = properties;
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004803 RID: 18435 RVA: 0x000FA96E File Offset: 0x000F996E
		public string MethodName
		{
			get
			{
				return this._methodName;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06004804 RID: 18436 RVA: 0x000FA976 File Offset: 0x000F9976
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06004805 RID: 18437 RVA: 0x000FA97E File Offset: 0x000F997E
		public Type[] InstantiationArgs
		{
			get
			{
				return this._instArgs;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06004806 RID: 18438 RVA: 0x000FA986 File Offset: 0x000F9986
		public object MethodSignature
		{
			get
			{
				return this._methodSignature;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06004807 RID: 18439 RVA: 0x000FA98E File Offset: 0x000F998E
		public object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06004808 RID: 18440 RVA: 0x000FA996 File Offset: 0x000F9996
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this._logicalCallContext;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004809 RID: 18441 RVA: 0x000FA99E File Offset: 0x000F999E
		public bool HasProperties
		{
			get
			{
				return this._properties != null;
			}
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x000FA9AC File Offset: 0x000F99AC
		internal void PopulateMessageProperties(IDictionary dict)
		{
			foreach (DictionaryEntry dictionaryEntry in this._properties)
			{
				dict[dictionaryEntry.Key] = dictionaryEntry.Value;
			}
		}

		// Token: 0x040024F3 RID: 9459
		private object[] _inargs;

		// Token: 0x040024F4 RID: 9460
		private string _methodName;

		// Token: 0x040024F5 RID: 9461
		private string _typeName;

		// Token: 0x040024F6 RID: 9462
		private object _methodSignature;

		// Token: 0x040024F7 RID: 9463
		private Type[] _instArgs;

		// Token: 0x040024F8 RID: 9464
		private object[] _args;

		// Token: 0x040024F9 RID: 9465
		private LogicalCallContext _logicalCallContext;

		// Token: 0x040024FA RID: 9466
		private object[] _properties;
	}
}
