using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006BB RID: 1723
	internal class CrossAppDomainSink : InternalSink, IMessageSink
	{
		// Token: 0x06003E9A RID: 16026 RVA: 0x000D7518 File Offset: 0x000D6518
		internal CrossAppDomainSink(CrossAppDomainData xadData)
		{
			this._xadData = xadData;
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x000D7528 File Offset: 0x000D6528
		internal static void GrowArrays(int oldSize)
		{
			if (CrossAppDomainSink._sinks == null)
			{
				CrossAppDomainSink._sinks = new CrossAppDomainSink[8];
				CrossAppDomainSink._sinkKeys = new int[8];
				return;
			}
			CrossAppDomainSink[] array = new CrossAppDomainSink[CrossAppDomainSink._sinks.Length + 8];
			int[] array2 = new int[CrossAppDomainSink._sinkKeys.Length + 8];
			Array.Copy(CrossAppDomainSink._sinks, array, CrossAppDomainSink._sinks.Length);
			Array.Copy(CrossAppDomainSink._sinkKeys, array2, CrossAppDomainSink._sinkKeys.Length);
			CrossAppDomainSink._sinks = array;
			CrossAppDomainSink._sinkKeys = array2;
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x000D75A4 File Offset: 0x000D65A4
		internal static CrossAppDomainSink FindOrCreateSink(CrossAppDomainData xadData)
		{
			CrossAppDomainSink crossAppDomainSink;
			lock (CrossAppDomainSink.staticSyncObject)
			{
				int domainID = xadData.DomainID;
				if (CrossAppDomainSink._sinks == null)
				{
					CrossAppDomainSink.GrowArrays(0);
				}
				int num = 0;
				while (CrossAppDomainSink._sinks[num] != null)
				{
					if (CrossAppDomainSink._sinkKeys[num] == domainID)
					{
						return CrossAppDomainSink._sinks[num];
					}
					num++;
					if (num == CrossAppDomainSink._sinks.Length)
					{
						CrossAppDomainSink.GrowArrays(num);
						break;
					}
				}
				CrossAppDomainSink._sinks[num] = new CrossAppDomainSink(xadData);
				CrossAppDomainSink._sinkKeys[num] = domainID;
				crossAppDomainSink = CrossAppDomainSink._sinks[num];
			}
			return crossAppDomainSink;
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x000D7640 File Offset: 0x000D6640
		internal static void DomainUnloaded(int domainID)
		{
			lock (CrossAppDomainSink.staticSyncObject)
			{
				if (CrossAppDomainSink._sinks != null)
				{
					int num = 0;
					int num2 = -1;
					while (CrossAppDomainSink._sinks[num] != null)
					{
						if (CrossAppDomainSink._sinkKeys[num] == domainID)
						{
							num2 = num;
						}
						num++;
						if (num == CrossAppDomainSink._sinks.Length)
						{
							break;
						}
					}
					if (num2 != -1)
					{
						CrossAppDomainSink._sinkKeys[num2] = CrossAppDomainSink._sinkKeys[num - 1];
						CrossAppDomainSink._sinks[num2] = CrossAppDomainSink._sinks[num - 1];
						CrossAppDomainSink._sinkKeys[num - 1] = 0;
						CrossAppDomainSink._sinks[num - 1] = null;
					}
				}
			}
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x000D76E0 File Offset: 0x000D66E0
		internal static byte[] DoDispatch(byte[] reqStmBuff, SmuggledMethodCallMessage smuggledMcm, out SmuggledMethodReturnMessage smuggledMrm)
		{
			IMessage message;
			if (smuggledMcm != null)
			{
				ArrayList arrayList = smuggledMcm.FixupForNewAppDomain();
				message = new MethodCall(smuggledMcm, arrayList);
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream(reqStmBuff);
				message = CrossAppDomainSerializer.DeserializeMessage(memoryStream);
			}
			LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
			logicalCallContext.SetData("__xADCall", true);
			IMessage message2 = ChannelServices.SyncDispatchMessage(message);
			logicalCallContext.FreeNamedDataSlot("__xADCall");
			smuggledMrm = SmuggledMethodReturnMessage.SmuggleIfPossible(message2);
			if (smuggledMrm != null)
			{
				return null;
			}
			if (message2 != null)
			{
				LogicalCallContext logicalCallContext2 = (LogicalCallContext)message2.Properties[Message.CallContextKey];
				if (logicalCallContext2 != null && logicalCallContext2.Principal != null)
				{
					logicalCallContext2.Principal = null;
				}
				return CrossAppDomainSerializer.SerializeMessage(message2).GetBuffer();
			}
			return null;
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x000D778C File Offset: 0x000D678C
		internal static object DoTransitionDispatchCallback(object[] args)
		{
			byte[] array = (byte[])args[0];
			SmuggledMethodCallMessage smuggledMethodCallMessage = (SmuggledMethodCallMessage)args[1];
			SmuggledMethodReturnMessage smuggledMethodReturnMessage = null;
			byte[] array2 = null;
			try
			{
				array2 = CrossAppDomainSink.DoDispatch(array, smuggledMethodCallMessage, out smuggledMethodReturnMessage);
			}
			catch (Exception ex)
			{
				IMessage message = new ReturnMessage(ex, new ErrorMessage());
				array2 = CrossAppDomainSerializer.SerializeMessage(message).GetBuffer();
			}
			args[2] = smuggledMethodReturnMessage;
			return array2;
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x000D77F4 File Offset: 0x000D67F4
		internal byte[] DoTransitionDispatch(byte[] reqStmBuff, SmuggledMethodCallMessage smuggledMcm, out SmuggledMethodReturnMessage smuggledMrm)
		{
			object[] array = new object[3];
			array[0] = reqStmBuff;
			array[1] = smuggledMcm;
			object[] array2 = array;
			byte[] array3 = (byte[])Thread.CurrentThread.InternalCrossContextCallback(null, this._xadData.ContextID, this._xadData.DomainID, CrossAppDomainSink.s_xctxDel, array2);
			smuggledMrm = (SmuggledMethodReturnMessage)array2[2];
			return array3;
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x000D784C File Offset: 0x000D684C
		public virtual IMessage SyncProcessMessage(IMessage reqMsg)
		{
			IMessage message = InternalSink.ValidateMessage(reqMsg);
			if (message != null)
			{
				return message;
			}
			IPrincipal principal = null;
			IMessage message2 = null;
			try
			{
				IMethodCallMessage methodCallMessage = reqMsg as IMethodCallMessage;
				if (methodCallMessage != null)
				{
					LogicalCallContext logicalCallContext = methodCallMessage.LogicalCallContext;
					if (logicalCallContext != null)
					{
						principal = logicalCallContext.RemovePrincipalIfNotSerializable();
					}
				}
				MemoryStream memoryStream = null;
				SmuggledMethodCallMessage smuggledMethodCallMessage = SmuggledMethodCallMessage.SmuggleIfPossible(reqMsg);
				if (smuggledMethodCallMessage == null)
				{
					memoryStream = CrossAppDomainSerializer.SerializeMessage(reqMsg);
				}
				LogicalCallContext logicalCallContext2 = CallContext.SetLogicalCallContext(null);
				byte[] array = null;
				SmuggledMethodReturnMessage smuggledMethodReturnMessage;
				try
				{
					if (smuggledMethodCallMessage != null)
					{
						array = this.DoTransitionDispatch(null, smuggledMethodCallMessage, out smuggledMethodReturnMessage);
					}
					else
					{
						array = this.DoTransitionDispatch(memoryStream.GetBuffer(), null, out smuggledMethodReturnMessage);
					}
				}
				finally
				{
					CallContext.SetLogicalCallContext(logicalCallContext2);
				}
				if (smuggledMethodReturnMessage != null)
				{
					ArrayList arrayList = smuggledMethodReturnMessage.FixupForNewAppDomain();
					message2 = new MethodResponse((IMethodCallMessage)reqMsg, smuggledMethodReturnMessage, arrayList);
				}
				else if (array != null)
				{
					MemoryStream memoryStream2 = new MemoryStream(array);
					message2 = CrossAppDomainSerializer.DeserializeMessage(memoryStream2, reqMsg as IMethodCallMessage);
				}
			}
			catch (Exception ex)
			{
				try
				{
					message2 = new ReturnMessage(ex, reqMsg as IMethodCallMessage);
				}
				catch (Exception)
				{
				}
			}
			if (principal != null)
			{
				IMethodReturnMessage methodReturnMessage = message2 as IMethodReturnMessage;
				if (methodReturnMessage != null)
				{
					LogicalCallContext logicalCallContext3 = methodReturnMessage.LogicalCallContext;
					logicalCallContext3.Principal = principal;
				}
			}
			return message2;
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x000D7978 File Offset: 0x000D6978
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			ADAsyncWorkItem adasyncWorkItem = new ADAsyncWorkItem(reqMsg, this, replySink);
			WaitCallback waitCallback = new WaitCallback(adasyncWorkItem.FinishAsyncWork);
			ThreadPool.QueueUserWorkItem(waitCallback);
			return null;
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06003EA3 RID: 16035 RVA: 0x000D79A4 File Offset: 0x000D69A4
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04001FA9 RID: 8105
		internal const int GROW_BY = 8;

		// Token: 0x04001FAA RID: 8106
		internal const string LCC_DATA_KEY = "__xADCall";

		// Token: 0x04001FAB RID: 8107
		internal static int[] _sinkKeys;

		// Token: 0x04001FAC RID: 8108
		internal static CrossAppDomainSink[] _sinks;

		// Token: 0x04001FAD RID: 8109
		private static object staticSyncObject = new object();

		// Token: 0x04001FAE RID: 8110
		private static InternalCrossContextDelegate s_xctxDel = new InternalCrossContextDelegate(CrossAppDomainSink.DoTransitionDispatchCallback);

		// Token: 0x04001FAF RID: 8111
		internal CrossAppDomainData _xadData;
	}
}
