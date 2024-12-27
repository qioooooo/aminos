using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Metadata;
using System.Security.Principal;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000783 RID: 1923
	[Serializable]
	internal class StackBuilderSink : IMessageSink
	{
		// Token: 0x060044F0 RID: 17648 RVA: 0x000EB44E File Offset: 0x000EA44E
		public StackBuilderSink(MarshalByRefObject server)
		{
			this._server = server;
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x000EB45D File Offset: 0x000EA45D
		public StackBuilderSink(object server)
		{
			this._server = server;
			if (this._server == null)
			{
				this._bStatic = true;
			}
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x000EB47B File Offset: 0x000EA47B
		public virtual IMessage SyncProcessMessage(IMessage msg)
		{
			return this.SyncProcessMessage(msg, 0, false);
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x000EB488 File Offset: 0x000EA488
		internal virtual IMessage SyncProcessMessage(IMessage msg, int methodPtr, bool fExecuteInContext)
		{
			IMessage message = InternalSink.ValidateMessage(msg);
			if (message != null)
			{
				return message;
			}
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			LogicalCallContext logicalCallContext = null;
			LogicalCallContext logicalCallContext2 = CallContext.GetLogicalCallContext();
			object data = logicalCallContext2.GetData("__xADCall");
			bool flag = false;
			IMessage message2;
			try
			{
				object server = this._server;
				StackBuilderSink.VerifyIsOkToCallMethod(server, methodCallMessage);
				LogicalCallContext logicalCallContext3;
				if (methodCallMessage != null)
				{
					logicalCallContext3 = methodCallMessage.LogicalCallContext;
				}
				else
				{
					logicalCallContext3 = (LogicalCallContext)msg.Properties["__CallContext"];
				}
				logicalCallContext = CallContext.SetLogicalCallContext(logicalCallContext3);
				flag = true;
				logicalCallContext3.PropagateIncomingHeadersToCallContext(msg);
				StackBuilderSink.PreserveThreadPrincipalIfNecessary(logicalCallContext3, logicalCallContext);
				if (this.IsOKToStackBlt(methodCallMessage, server) && ((Message)methodCallMessage).Dispatch(server, fExecuteInContext))
				{
					message2 = new StackBasedReturnMessage();
					((StackBasedReturnMessage)message2).InitFields((Message)methodCallMessage);
					LogicalCallContext logicalCallContext4 = CallContext.GetLogicalCallContext();
					logicalCallContext4.PropagateOutgoingHeadersToMessage(message2);
					((StackBasedReturnMessage)message2).SetLogicalCallContext(logicalCallContext4);
				}
				else
				{
					MethodBase methodBase = StackBuilderSink.GetMethodBase(methodCallMessage);
					object[] array = null;
					RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(methodBase);
					object[] array2 = Message.CoerceArgs(methodCallMessage, reflectionCachedData.Parameters);
					object obj = this.PrivateProcessMessage(methodBase.MethodHandle, array2, server, methodPtr, fExecuteInContext, out array);
					this.CopyNonByrefOutArgsFromOriginalArgs(reflectionCachedData, array2, ref array);
					LogicalCallContext logicalCallContext5 = CallContext.GetLogicalCallContext();
					if (data != null && (bool)data && logicalCallContext5 != null)
					{
						logicalCallContext5.RemovePrincipalIfNotSerializable();
					}
					message2 = new ReturnMessage(obj, array, (array == null) ? 0 : array.Length, logicalCallContext5, methodCallMessage);
					logicalCallContext5.PropagateOutgoingHeadersToMessage(message2);
					CallContext.SetLogicalCallContext(logicalCallContext);
				}
			}
			catch (Exception ex)
			{
				message2 = new ReturnMessage(ex, methodCallMessage);
				((ReturnMessage)message2).SetLogicalCallContext(methodCallMessage.LogicalCallContext);
				if (flag)
				{
					CallContext.SetLogicalCallContext(logicalCallContext);
				}
			}
			return message2;
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x000EB644 File Offset: 0x000EA644
		public virtual IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			IMessageCtrl messageCtrl = null;
			IMessage message = null;
			LogicalCallContext logicalCallContext = null;
			bool flag = false;
			try
			{
				try
				{
					LogicalCallContext logicalCallContext2 = (LogicalCallContext)methodCallMessage.Properties[Message.CallContextKey];
					object server = this._server;
					StackBuilderSink.VerifyIsOkToCallMethod(server, methodCallMessage);
					logicalCallContext = CallContext.SetLogicalCallContext(logicalCallContext2);
					flag = true;
					logicalCallContext2.PropagateIncomingHeadersToCallContext(msg);
					StackBuilderSink.PreserveThreadPrincipalIfNecessary(logicalCallContext2, logicalCallContext);
					ServerChannelSinkStack serverChannelSinkStack = msg.Properties["__SinkStack"] as ServerChannelSinkStack;
					if (serverChannelSinkStack != null)
					{
						serverChannelSinkStack.ServerObject = server;
					}
					MethodBase methodBase = StackBuilderSink.GetMethodBase(methodCallMessage);
					object[] array = null;
					RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(methodBase);
					object[] array2 = Message.CoerceArgs(methodCallMessage, reflectionCachedData.Parameters);
					object obj = this.PrivateProcessMessage(methodBase.MethodHandle, array2, server, 0, false, out array);
					this.CopyNonByrefOutArgsFromOriginalArgs(reflectionCachedData, array2, ref array);
					if (replySink != null)
					{
						LogicalCallContext logicalCallContext3 = CallContext.GetLogicalCallContext();
						if (logicalCallContext3 != null)
						{
							logicalCallContext3.RemovePrincipalIfNotSerializable();
						}
						message = new ReturnMessage(obj, array, (array == null) ? 0 : array.Length, logicalCallContext3, methodCallMessage);
						logicalCallContext3.PropagateOutgoingHeadersToMessage(message);
					}
				}
				catch (Exception ex)
				{
					if (replySink != null)
					{
						message = new ReturnMessage(ex, methodCallMessage);
						((ReturnMessage)message).SetLogicalCallContext((LogicalCallContext)methodCallMessage.Properties[Message.CallContextKey]);
					}
				}
				finally
				{
					if (replySink != null)
					{
						replySink.SyncProcessMessage(message);
					}
				}
			}
			finally
			{
				if (flag)
				{
					CallContext.SetLogicalCallContext(logicalCallContext);
				}
			}
			return messageCtrl;
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060044F5 RID: 17653 RVA: 0x000EB7E4 File Offset: 0x000EA7E4
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x000EB7E8 File Offset: 0x000EA7E8
		internal bool IsOKToStackBlt(IMethodMessage mcMsg, object server)
		{
			bool flag = false;
			Message message = mcMsg as Message;
			if (message != null)
			{
				IInternalMessage internalMessage = message;
				if (message.GetFramePtr() != IntPtr.Zero && message.GetThisPtr() == server && (internalMessage.IdentityObject == null || (internalMessage.IdentityObject != null && internalMessage.IdentityObject == internalMessage.ServerIdentityObject)))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x000EB840 File Offset: 0x000EA840
		private static MethodBase GetMethodBase(IMethodMessage msg)
		{
			MethodBase methodBase = msg.MethodBase;
			if (methodBase == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Message_MethodMissing"), new object[] { msg.MethodName, msg.TypeName }));
			}
			return methodBase;
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x000EB88C File Offset: 0x000EA88C
		private static void VerifyIsOkToCallMethod(object server, IMethodMessage msg)
		{
			bool flag = false;
			MarshalByRefObject marshalByRefObject = server as MarshalByRefObject;
			if (marshalByRefObject != null)
			{
				bool flag2;
				Identity identity = MarshalByRefObject.GetIdentity(marshalByRefObject, out flag2);
				if (identity != null)
				{
					ServerIdentity serverIdentity = identity as ServerIdentity;
					if (serverIdentity != null && serverIdentity.MarshaledAsSpecificType)
					{
						Type serverType = serverIdentity.ServerType;
						if (serverType != null)
						{
							MethodBase methodBase = StackBuilderSink.GetMethodBase(msg);
							Type declaringType = methodBase.DeclaringType;
							if (declaringType != serverType && !declaringType.IsAssignableFrom(serverType))
							{
								throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_InvalidCallingType"), new object[]
								{
									methodBase.DeclaringType.FullName,
									serverType.FullName
								}));
							}
							if (declaringType.IsInterface)
							{
								StackBuilderSink.VerifyNotIRemoteDispatch(declaringType);
							}
							flag = true;
						}
					}
				}
				if (!flag)
				{
					MethodBase methodBase2 = StackBuilderSink.GetMethodBase(msg);
					Type reflectedType = methodBase2.ReflectedType;
					if (!reflectedType.IsInterface)
					{
						if (!reflectedType.IsInstanceOfType(marshalByRefObject))
						{
							throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_InvalidCallingType"), new object[]
							{
								reflectedType.FullName,
								marshalByRefObject.GetType().FullName
							}));
						}
					}
					else
					{
						StackBuilderSink.VerifyNotIRemoteDispatch(reflectedType);
					}
				}
			}
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x000EB9BE File Offset: 0x000EA9BE
		private static void VerifyNotIRemoteDispatch(Type reflectedType)
		{
			if (reflectedType.FullName.Equals(StackBuilderSink.sIRemoteDispatch) && reflectedType.Module.Assembly.nGetSimpleName().Equals(StackBuilderSink.sIRemoteDispatchAssembly))
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_CantInvokeIRemoteDispatch"));
			}
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x000EBA00 File Offset: 0x000EAA00
		internal void CopyNonByrefOutArgsFromOriginalArgs(RemotingMethodCachedData methodCache, object[] args, ref object[] marshalResponseArgs)
		{
			int[] nonRefOutArgMap = methodCache.NonRefOutArgMap;
			if (nonRefOutArgMap.Length > 0)
			{
				if (marshalResponseArgs == null)
				{
					marshalResponseArgs = new object[methodCache.Parameters.Length];
				}
				foreach (int num in nonRefOutArgMap)
				{
					marshalResponseArgs[num] = args[num];
				}
			}
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x000EBA48 File Offset: 0x000EAA48
		internal static void PreserveThreadPrincipalIfNecessary(LogicalCallContext messageCallContext, LogicalCallContext threadCallContext)
		{
			if (threadCallContext != null && messageCallContext.Principal == null)
			{
				IPrincipal principal = threadCallContext.Principal;
				if (principal != null)
				{
					messageCallContext.Principal = principal;
				}
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x060044FC RID: 17660 RVA: 0x000EBA71 File Offset: 0x000EAA71
		internal object ServerObject
		{
			get
			{
				return this._server;
			}
		}

		// Token: 0x060044FD RID: 17661
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object _PrivateProcessMessage(IntPtr md, object[] args, object server, int methodPtr, bool fExecuteInContext, out object[] outArgs);

		// Token: 0x060044FE RID: 17662 RVA: 0x000EBA79 File Offset: 0x000EAA79
		public object PrivateProcessMessage(RuntimeMethodHandle md, object[] args, object server, int methodPtr, bool fExecuteInContext, out object[] outArgs)
		{
			return this._PrivateProcessMessage(md.Value, args, server, methodPtr, fExecuteInContext, out outArgs);
		}

		// Token: 0x0400221B RID: 8731
		private object _server;

		// Token: 0x0400221C RID: 8732
		private static string sIRemoteDispatch = "System.EnterpriseServices.IRemoteDispatch";

		// Token: 0x0400221D RID: 8733
		private static string sIRemoteDispatchAssembly = "System.EnterpriseServices";

		// Token: 0x0400221E RID: 8734
		private bool _bStatic;
	}
}
