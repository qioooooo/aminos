using System;
using System.Collections;
using System.Reflection;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000704 RID: 1796
	internal class StackBasedReturnMessage : IMethodReturnMessage, IMethodMessage, IMessage, IInternalMessage
	{
		// Token: 0x06004092 RID: 16530 RVA: 0x000DCAE8 File Offset: 0x000DBAE8
		internal StackBasedReturnMessage()
		{
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x000DCAF0 File Offset: 0x000DBAF0
		internal void InitFields(Message m)
		{
			this._m = m;
			if (this._h != null)
			{
				this._h.Clear();
			}
			if (this._d != null)
			{
				this._d.Clear();
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06004094 RID: 16532 RVA: 0x000DCB1F File Offset: 0x000DBB1F
		public string Uri
		{
			get
			{
				return this._m.Uri;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x000DCB2C File Offset: 0x000DBB2C
		public string MethodName
		{
			get
			{
				return this._m.MethodName;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x000DCB39 File Offset: 0x000DBB39
		public string TypeName
		{
			get
			{
				return this._m.TypeName;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x000DCB46 File Offset: 0x000DBB46
		public object MethodSignature
		{
			get
			{
				return this._m.MethodSignature;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06004098 RID: 16536 RVA: 0x000DCB53 File Offset: 0x000DBB53
		public MethodBase MethodBase
		{
			get
			{
				return this._m.MethodBase;
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06004099 RID: 16537 RVA: 0x000DCB60 File Offset: 0x000DBB60
		public bool HasVarArgs
		{
			get
			{
				return this._m.HasVarArgs;
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x0600409A RID: 16538 RVA: 0x000DCB6D File Offset: 0x000DBB6D
		public int ArgCount
		{
			get
			{
				return this._m.ArgCount;
			}
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x000DCB7A File Offset: 0x000DBB7A
		public object GetArg(int argNum)
		{
			return this._m.GetArg(argNum);
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x000DCB88 File Offset: 0x000DBB88
		public string GetArgName(int index)
		{
			return this._m.GetArgName(index);
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600409D RID: 16541 RVA: 0x000DCB96 File Offset: 0x000DBB96
		public object[] Args
		{
			get
			{
				return this._m.Args;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600409E RID: 16542 RVA: 0x000DCBA3 File Offset: 0x000DBBA3
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this._m.GetLogicalCallContext();
			}
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x000DCBB0 File Offset: 0x000DBBB0
		internal LogicalCallContext GetLogicalCallContext()
		{
			return this._m.GetLogicalCallContext();
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x000DCBBD File Offset: 0x000DBBBD
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			return this._m.SetLogicalCallContext(callCtx);
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x060040A1 RID: 16545 RVA: 0x000DCBCB File Offset: 0x000DBBCB
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

		// Token: 0x060040A2 RID: 16546 RVA: 0x000DCBED File Offset: 0x000DBBED
		public object GetOutArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x000DCC10 File Offset: 0x000DBC10
		public string GetOutArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x060040A4 RID: 16548 RVA: 0x000DCC33 File Offset: 0x000DBC33
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

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x000DCC55 File Offset: 0x000DBC55
		public Exception Exception
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x000DCC58 File Offset: 0x000DBC58
		public object ReturnValue
		{
			get
			{
				return this._m.GetReturnValue();
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x000DCC68 File Offset: 0x000DBC68
		public IDictionary Properties
		{
			get
			{
				IDictionary d;
				lock (this)
				{
					if (this._h == null)
					{
						this._h = new Hashtable();
					}
					if (this._d == null)
					{
						this._d = new MRMDictionary(this, this._h);
					}
					d = this._d;
				}
				return d;
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x000DCCCC File Offset: 0x000DBCCC
		// (set) Token: 0x060040A9 RID: 16553 RVA: 0x000DCCCF File Offset: 0x000DBCCF
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

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x000DCCD1 File Offset: 0x000DBCD1
		// (set) Token: 0x060040AB RID: 16555 RVA: 0x000DCCD4 File Offset: 0x000DBCD4
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

		// Token: 0x060040AC RID: 16556 RVA: 0x000DCCD6 File Offset: 0x000DBCD6
		void IInternalMessage.SetURI(string val)
		{
			this._m.Uri = val;
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x000DCCE4 File Offset: 0x000DBCE4
		void IInternalMessage.SetCallContext(LogicalCallContext newCallContext)
		{
			this._m.SetLogicalCallContext(newCallContext);
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x000DCCF3 File Offset: 0x000DBCF3
		bool IInternalMessage.HasProperties()
		{
			return this._h != null;
		}

		// Token: 0x04002068 RID: 8296
		private Message _m;

		// Token: 0x04002069 RID: 8297
		private Hashtable _h;

		// Token: 0x0400206A RID: 8298
		private MRMDictionary _d;

		// Token: 0x0400206B RID: 8299
		private ArgMapper _argMapper;
	}
}
