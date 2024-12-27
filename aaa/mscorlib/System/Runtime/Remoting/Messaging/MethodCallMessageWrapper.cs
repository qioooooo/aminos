using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000710 RID: 1808
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class MethodCallMessageWrapper : InternalMessageWrapper, IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x0600414C RID: 16716 RVA: 0x000DF4C6 File Offset: 0x000DE4C6
		public MethodCallMessageWrapper(IMethodCallMessage msg)
			: base(msg)
		{
			this._msg = msg;
			this._args = this._msg.Args;
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x0600414D RID: 16717 RVA: 0x000DF4E7 File Offset: 0x000DE4E7
		// (set) Token: 0x0600414E RID: 16718 RVA: 0x000DF4F4 File Offset: 0x000DE4F4
		public virtual string Uri
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

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x0600414F RID: 16719 RVA: 0x000DF50C File Offset: 0x000DE50C
		public virtual string MethodName
		{
			get
			{
				return this._msg.MethodName;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004150 RID: 16720 RVA: 0x000DF519 File Offset: 0x000DE519
		public virtual string TypeName
		{
			get
			{
				return this._msg.TypeName;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x000DF526 File Offset: 0x000DE526
		public virtual object MethodSignature
		{
			get
			{
				return this._msg.MethodSignature;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004152 RID: 16722 RVA: 0x000DF533 File Offset: 0x000DE533
		public virtual LogicalCallContext LogicalCallContext
		{
			get
			{
				return this._msg.LogicalCallContext;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06004153 RID: 16723 RVA: 0x000DF540 File Offset: 0x000DE540
		public virtual MethodBase MethodBase
		{
			get
			{
				return this._msg.MethodBase;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06004154 RID: 16724 RVA: 0x000DF54D File Offset: 0x000DE54D
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

		// Token: 0x06004155 RID: 16725 RVA: 0x000DF561 File Offset: 0x000DE561
		public virtual string GetArgName(int index)
		{
			return this._msg.GetArgName(index);
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x000DF56F File Offset: 0x000DE56F
		public virtual object GetArg(int argNum)
		{
			return this._args[argNum];
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x000DF579 File Offset: 0x000DE579
		// (set) Token: 0x06004158 RID: 16728 RVA: 0x000DF581 File Offset: 0x000DE581
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

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06004159 RID: 16729 RVA: 0x000DF58A File Offset: 0x000DE58A
		public virtual bool HasVarArgs
		{
			get
			{
				return this._msg.HasVarArgs;
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x0600415A RID: 16730 RVA: 0x000DF597 File Offset: 0x000DE597
		public virtual int InArgCount
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

		// Token: 0x0600415B RID: 16731 RVA: 0x000DF5B9 File Offset: 0x000DE5B9
		public virtual object GetInArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x000DF5DC File Offset: 0x000DE5DC
		public virtual string GetInArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x0600415D RID: 16733 RVA: 0x000DF5FF File Offset: 0x000DE5FF
		public virtual object[] InArgs
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

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x0600415E RID: 16734 RVA: 0x000DF621 File Offset: 0x000DE621
		public virtual IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					this._properties = new MethodCallMessageWrapper.MCMWrapperDictionary(this, this._msg.Properties);
				}
				return this._properties;
			}
		}

		// Token: 0x040020A6 RID: 8358
		private IMethodCallMessage _msg;

		// Token: 0x040020A7 RID: 8359
		private IDictionary _properties;

		// Token: 0x040020A8 RID: 8360
		private ArgMapper _argMapper;

		// Token: 0x040020A9 RID: 8361
		private object[] _args;

		// Token: 0x02000711 RID: 1809
		private class MCMWrapperDictionary : Hashtable
		{
			// Token: 0x0600415F RID: 16735 RVA: 0x000DF648 File Offset: 0x000DE648
			public MCMWrapperDictionary(IMethodCallMessage msg, IDictionary idict)
			{
				this._mcmsg = msg;
				this._idict = idict;
			}

			// Token: 0x17000B80 RID: 2944
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
							return this._mcmsg.Uri;
						}
						if (text2 == "__MethodName")
						{
							return this._mcmsg.MethodName;
						}
						if (text2 == "__MethodSignature")
						{
							return this._mcmsg.MethodSignature;
						}
						if (text2 == "__TypeName")
						{
							return this._mcmsg.TypeName;
						}
						if (text2 == "__Args")
						{
							return this._mcmsg.Args;
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
						if ((text2 = text) != null && (text2 == "__MethodName" || text2 == "__MethodSignature" || text2 == "__TypeName" || text2 == "__Args"))
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
						}
						this._idict[key] = value;
					}
				}
			}

			// Token: 0x040020AA RID: 8362
			private IMethodCallMessage _mcmsg;

			// Token: 0x040020AB RID: 8363
			private IDictionary _idict;
		}
	}
}
