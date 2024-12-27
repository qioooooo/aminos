using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x0200035F RID: 863
	internal class SerializationEvents
	{
		// Token: 0x06002241 RID: 8769 RVA: 0x00056F14 File Offset: 0x00055F14
		private List<MethodInfo> GetMethodsWithAttribute(Type attribute, Type t)
		{
			List<MethodInfo> list = new List<MethodInfo>();
			Type type = t;
			while (type != null && type != typeof(object))
			{
				RuntimeType runtimeType = (RuntimeType)type;
				MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.IsDefined(attribute, false))
					{
						list.Add(methodInfo);
					}
				}
				type = type.BaseType;
			}
			list.Reverse();
			if (list.Count != 0)
			{
				return list;
			}
			return null;
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00056F90 File Offset: 0x00055F90
		internal SerializationEvents(Type t)
		{
			this.m_OnSerializingMethods = this.GetMethodsWithAttribute(typeof(OnSerializingAttribute), t);
			this.m_OnSerializedMethods = this.GetMethodsWithAttribute(typeof(OnSerializedAttribute), t);
			this.m_OnDeserializingMethods = this.GetMethodsWithAttribute(typeof(OnDeserializingAttribute), t);
			this.m_OnDeserializedMethods = this.GetMethodsWithAttribute(typeof(OnDeserializedAttribute), t);
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002243 RID: 8771 RVA: 0x00056FFF File Offset: 0x00055FFF
		internal bool HasOnSerializingEvents
		{
			get
			{
				return this.m_OnSerializingMethods != null || this.m_OnSerializedMethods != null;
			}
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x00057018 File Offset: 0x00056018
		internal void InvokeOnSerializing(object obj, StreamingContext context)
		{
			if (this.m_OnSerializingMethods != null)
			{
				SerializationEventHandler serializationEventHandler = null;
				foreach (MethodInfo methodInfo in this.m_OnSerializingMethods)
				{
					SerializationEventHandler serializationEventHandler2 = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, methodInfo);
					serializationEventHandler = (SerializationEventHandler)Delegate.Combine(serializationEventHandler, serializationEventHandler2);
				}
				serializationEventHandler(context);
			}
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x0005709C File Offset: 0x0005609C
		internal void InvokeOnDeserializing(object obj, StreamingContext context)
		{
			if (this.m_OnDeserializingMethods != null)
			{
				SerializationEventHandler serializationEventHandler = null;
				foreach (MethodInfo methodInfo in this.m_OnDeserializingMethods)
				{
					SerializationEventHandler serializationEventHandler2 = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, methodInfo);
					serializationEventHandler = (SerializationEventHandler)Delegate.Combine(serializationEventHandler, serializationEventHandler2);
				}
				serializationEventHandler(context);
			}
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x00057120 File Offset: 0x00056120
		internal void InvokeOnDeserialized(object obj, StreamingContext context)
		{
			if (this.m_OnDeserializedMethods != null)
			{
				SerializationEventHandler serializationEventHandler = null;
				foreach (MethodInfo methodInfo in this.m_OnDeserializedMethods)
				{
					SerializationEventHandler serializationEventHandler2 = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, methodInfo);
					serializationEventHandler = (SerializationEventHandler)Delegate.Combine(serializationEventHandler, serializationEventHandler2);
				}
				serializationEventHandler(context);
			}
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000571A4 File Offset: 0x000561A4
		internal SerializationEventHandler AddOnSerialized(object obj, SerializationEventHandler handler)
		{
			if (this.m_OnSerializedMethods != null)
			{
				foreach (MethodInfo methodInfo in this.m_OnSerializedMethods)
				{
					SerializationEventHandler serializationEventHandler = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, methodInfo);
					handler = (SerializationEventHandler)Delegate.Combine(handler, serializationEventHandler);
				}
			}
			return handler;
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x00057220 File Offset: 0x00056220
		internal SerializationEventHandler AddOnDeserialized(object obj, SerializationEventHandler handler)
		{
			if (this.m_OnDeserializedMethods != null)
			{
				foreach (MethodInfo methodInfo in this.m_OnDeserializedMethods)
				{
					SerializationEventHandler serializationEventHandler = (SerializationEventHandler)Delegate.InternalCreateDelegate(typeof(SerializationEventHandler), obj, methodInfo);
					handler = (SerializationEventHandler)Delegate.Combine(handler, serializationEventHandler);
				}
			}
			return handler;
		}

		// Token: 0x04000E3F RID: 3647
		private List<MethodInfo> m_OnSerializingMethods;

		// Token: 0x04000E40 RID: 3648
		private List<MethodInfo> m_OnSerializedMethods;

		// Token: 0x04000E41 RID: 3649
		private List<MethodInfo> m_OnDeserializingMethods;

		// Token: 0x04000E42 RID: 3650
		private List<MethodInfo> m_OnDeserializedMethods;
	}
}
