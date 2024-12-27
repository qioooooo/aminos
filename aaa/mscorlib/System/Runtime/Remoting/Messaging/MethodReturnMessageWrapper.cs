using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000712 RID: 1810
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class MethodReturnMessageWrapper : InternalMessageWrapper, IMethodReturnMessage, IMethodMessage, IMessage
	{
		// Token: 0x06004162 RID: 16738 RVA: 0x000DF77C File Offset: 0x000DE77C
		public MethodReturnMessageWrapper(IMethodReturnMessage msg)
			: base(msg)
		{
			this._msg = msg;
			this._args = this._msg.Args;
			this._returnValue = this._msg.ReturnValue;
			this._exception = this._msg.Exception;
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06004163 RID: 16739 RVA: 0x000DF7CA File Offset: 0x000DE7CA
		// (set) Token: 0x06004164 RID: 16740 RVA: 0x000DF7D7 File Offset: 0x000DE7D7
		public string Uri
		{
			get
			{
				return this._msg.Uri;
			}
			set
			{
				this._msg.Properties[Message.UriKey] = value;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06004165 RID: 16741 RVA: 0x000DF7EF File Offset: 0x000DE7EF
		public virtual string MethodName
		{
			get
			{
				return this._msg.MethodName;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x000DF7FC File Offset: 0x000DE7FC
		public virtual string TypeName
		{
			get
			{
				return this._msg.TypeName;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06004167 RID: 16743 RVA: 0x000DF809 File Offset: 0x000DE809
		public virtual object MethodSignature
		{
			get
			{
				return this._msg.MethodSignature;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06004168 RID: 16744 RVA: 0x000DF816 File Offset: 0x000DE816
		public virtual LogicalCallContext LogicalCallContext
		{
			get
			{
				return this._msg.LogicalCallContext;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004169 RID: 16745 RVA: 0x000DF823 File Offset: 0x000DE823
		public virtual MethodBase MethodBase
		{
			get
			{
				return this._msg.MethodBase;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x0600416A RID: 16746 RVA: 0x000DF830 File Offset: 0x000DE830
		public virtual int ArgCount
		{
			get
			{
				if (this._args != null)
				{
					return this._args.Length;
				}
				return 0;
			}
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x000DF844 File Offset: 0x000DE844
		public virtual string GetArgName(int index)
		{
			return this._msg.GetArgName(index);
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x000DF852 File Offset: 0x000DE852
		public virtual object GetArg(int argNum)
		{
			return this._args[argNum];
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600416D RID: 16749 RVA: 0x000DF85C File Offset: 0x000DE85C
		// (set) Token: 0x0600416E RID: 16750 RVA: 0x000DF864 File Offset: 0x000DE864
		public virtual object[] Args
		{
			get
			{
				return this._args;
			}
			set
			{
				this._args = value;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600416F RID: 16751 RVA: 0x000DF86D File Offset: 0x000DE86D
		public virtual bool HasVarArgs
		{
			get
			{
				return this._msg.HasVarArgs;
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06004170 RID: 16752 RVA: 0x000DF87A File Offset: 0x000DE87A
		public virtual int OutArgCount
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

		// Token: 0x06004171 RID: 16753 RVA: 0x000DF89C File Offset: 0x000DE89C
		public virtual object GetOutArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x000DF8BF File Offset: 0x000DE8BF
		public virtual string GetOutArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x000DF8E2 File Offset: 0x000DE8E2
		public virtual object[] OutArgs
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

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004174 RID: 16756 RVA: 0x000DF904 File Offset: 0x000DE904
		// (set) Token: 0x06004175 RID: 16757 RVA: 0x000DF90C File Offset: 0x000DE90C
		public virtual Exception Exception
		{
			get
			{
				return this._exception;
			}
			set
			{
				this._exception = value;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06004176 RID: 16758 RVA: 0x000DF915 File Offset: 0x000DE915
		// (set) Token: 0x06004177 RID: 16759 RVA: 0x000DF91D File Offset: 0x000DE91D
		public virtual object ReturnValue
		{
			get
			{
				return this._returnValue;
			}
			set
			{
				this._returnValue = value;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06004178 RID: 16760 RVA: 0x000DF926 File Offset: 0x000DE926
		public virtual IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					this._properties = new MethodReturnMessageWrapper.MRMWrapperDictionary(this, this._msg.Properties);
				}
				return this._properties;
			}
		}

		// Token: 0x040020AC RID: 8364
		private IMethodReturnMessage _msg;

		// Token: 0x040020AD RID: 8365
		private IDictionary _properties;

		// Token: 0x040020AE RID: 8366
		private ArgMapper _argMapper;

		// Token: 0x040020AF RID: 8367
		private object[] _args;

		// Token: 0x040020B0 RID: 8368
		private object _returnValue;

		// Token: 0x040020B1 RID: 8369
		private Exception _exception;

		// Token: 0x02000713 RID: 1811
		private class MRMWrapperDictionary : Hashtable
		{
			// Token: 0x06004179 RID: 16761 RVA: 0x000DF94D File Offset: 0x000DE94D
			public MRMWrapperDictionary(IMethodReturnMessage msg, IDictionary idict)
			{
				this._mrmsg = msg;
				this._idict = idict;
			}

			// Token: 0x17000B8F RID: 2959
			public override object this[object key]
			{
				get
				{
					string text = key as string;
					string text2;
					if (text != null && (text2 = text) != null)
					{
						if (text2 == "__Uri")
						{
							return this._mrmsg.Uri;
						}
						if (text2 == "__MethodName")
						{
							return this._mrmsg.MethodName;
						}
						if (text2 == "__MethodSignature")
						{
							return this._mrmsg.MethodSignature;
						}
						if (text2 == "__TypeName")
						{
							return this._mrmsg.TypeName;
						}
						if (text2 == "__Return")
						{
							return this._mrmsg.ReturnValue;
						}
						if (text2 == "__OutArgs")
						{
							return this._mrmsg.OutArgs;
						}
					}
					return this._idict[key];
				}
				set
				{
					string text = key as string;
					if (text != null)
					{
						string text2;
						if ((text2 = text) != null && (text2 == "__MethodName" || text2 == "__MethodSignature" || text2 == "__TypeName" || text2 == "__Return" || text2 == "__OutArgs"))
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
						}
						this._idict[key] = value;
					}
				}
			}

			// Token: 0x040020B2 RID: 8370
			private IMethodReturnMessage _mrmsg;

			// Token: 0x040020B3 RID: 8371
			private IDictionary _idict;
		}
	}
}
