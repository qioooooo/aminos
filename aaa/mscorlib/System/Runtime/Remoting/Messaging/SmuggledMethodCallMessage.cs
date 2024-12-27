using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000717 RID: 1815
	internal class SmuggledMethodCallMessage : MessageSmuggler
	{
		// Token: 0x06004187 RID: 16775 RVA: 0x000DFDCC File Offset: 0x000DEDCC
		internal static SmuggledMethodCallMessage SmuggleIfPossible(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			if (methodCallMessage == null)
			{
				return null;
			}
			return new SmuggledMethodCallMessage(methodCallMessage);
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x000DFDEB File Offset: 0x000DEDEB
		private SmuggledMethodCallMessage()
		{
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x000DFDF4 File Offset: 0x000DEDF4
		private SmuggledMethodCallMessage(IMethodCallMessage mcm)
		{
			this._uri = mcm.Uri;
			this._methodName = mcm.MethodName;
			this._typeName = mcm.TypeName;
			ArrayList arrayList = null;
			IInternalMessage internalMessage = mcm as IInternalMessage;
			if (internalMessage == null || internalMessage.HasProperties())
			{
				this._propertyCount = MessageSmuggler.StoreUserPropertiesForMethodMessage(mcm, ref arrayList);
			}
			if (mcm.MethodBase.IsGenericMethod)
			{
				Type[] genericArguments = mcm.MethodBase.GetGenericArguments();
				if (genericArguments != null && genericArguments.Length > 0)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					this._instantiation = new MessageSmuggler.SerializedArg(arrayList.Count);
					arrayList.Add(genericArguments);
				}
			}
			if (RemotingServices.IsMethodOverloaded(mcm))
			{
				if (arrayList == null)
				{
					arrayList = new ArrayList();
				}
				this._methodSignature = new MessageSmuggler.SerializedArg(arrayList.Count);
				arrayList.Add(mcm.MethodSignature);
			}
			LogicalCallContext logicalCallContext = mcm.LogicalCallContext;
			if (logicalCallContext == null)
			{
				this._callContext = null;
			}
			else if (logicalCallContext.HasInfo)
			{
				if (arrayList == null)
				{
					arrayList = new ArrayList();
				}
				this._callContext = new MessageSmuggler.SerializedArg(arrayList.Count);
				arrayList.Add(logicalCallContext);
			}
			else
			{
				this._callContext = logicalCallContext.RemotingData.LogicalCallID;
			}
			this._args = MessageSmuggler.FixupArgs(mcm.Args, ref arrayList);
			if (arrayList != null)
			{
				MemoryStream memoryStream = CrossAppDomainSerializer.SerializeMessageParts(arrayList);
				this._serializedArgs = memoryStream.GetBuffer();
			}
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x000DFF3C File Offset: 0x000DEF3C
		internal ArrayList FixupForNewAppDomain()
		{
			ArrayList arrayList = null;
			if (this._serializedArgs != null)
			{
				arrayList = CrossAppDomainSerializer.DeserializeMessageParts(new MemoryStream(this._serializedArgs));
				this._serializedArgs = null;
			}
			return arrayList;
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x000DFF6C File Offset: 0x000DEF6C
		internal string Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x000DFF74 File Offset: 0x000DEF74
		internal string MethodName
		{
			get
			{
				return this._methodName;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x000DFF7C File Offset: 0x000DEF7C
		internal string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x000DFF84 File Offset: 0x000DEF84
		internal Type[] GetInstantiation(ArrayList deserializedArgs)
		{
			if (this._instantiation != null)
			{
				return (Type[])deserializedArgs[this._instantiation.Index];
			}
			return null;
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x000DFFA6 File Offset: 0x000DEFA6
		internal object[] GetMethodSignature(ArrayList deserializedArgs)
		{
			if (this._methodSignature != null)
			{
				return (object[])deserializedArgs[this._methodSignature.Index];
			}
			return null;
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x000DFFC8 File Offset: 0x000DEFC8
		internal object[] GetArgs(ArrayList deserializedArgs)
		{
			return MessageSmuggler.UndoFixupArgs(this._args, deserializedArgs);
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x000DFFD8 File Offset: 0x000DEFD8
		internal LogicalCallContext GetCallContext(ArrayList deserializedArgs)
		{
			if (this._callContext == null)
			{
				return null;
			}
			if (this._callContext is string)
			{
				return new LogicalCallContext
				{
					RemotingData = 
					{
						LogicalCallID = (string)this._callContext
					}
				};
			}
			return (LogicalCallContext)deserializedArgs[((MessageSmuggler.SerializedArg)this._callContext).Index];
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x000E0035 File Offset: 0x000DF035
		internal int MessagePropertyCount
		{
			get
			{
				return this._propertyCount;
			}
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x000E0040 File Offset: 0x000DF040
		internal void PopulateMessageProperties(IDictionary dict, ArrayList deserializedArgs)
		{
			for (int i = 0; i < this._propertyCount; i++)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)deserializedArgs[i];
				dict[dictionaryEntry.Key] = dictionaryEntry.Value;
			}
		}

		// Token: 0x040020B6 RID: 8374
		private string _uri;

		// Token: 0x040020B7 RID: 8375
		private string _methodName;

		// Token: 0x040020B8 RID: 8376
		private string _typeName;

		// Token: 0x040020B9 RID: 8377
		private object[] _args;

		// Token: 0x040020BA RID: 8378
		private byte[] _serializedArgs;

		// Token: 0x040020BB RID: 8379
		private MessageSmuggler.SerializedArg _methodSignature;

		// Token: 0x040020BC RID: 8380
		private MessageSmuggler.SerializedArg _instantiation;

		// Token: 0x040020BD RID: 8381
		private object _callContext;

		// Token: 0x040020BE RID: 8382
		private int _propertyCount;
	}
}
