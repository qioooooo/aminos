using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000334 RID: 820
	[ComDefaultInterface(typeof(_EventInfo))]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class EventInfo : MemberInfo, _EventInfo
	{
		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x000505E8 File Offset: 0x0004F5E8
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Event;
			}
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000505EB File Offset: 0x0004F5EB
		public virtual MethodInfo[] GetOtherMethods(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002034 RID: 8244
		public abstract MethodInfo GetAddMethod(bool nonPublic);

		// Token: 0x06002035 RID: 8245
		public abstract MethodInfo GetRemoveMethod(bool nonPublic);

		// Token: 0x06002036 RID: 8246
		public abstract MethodInfo GetRaiseMethod(bool nonPublic);

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06002037 RID: 8247
		public abstract EventAttributes Attributes { get; }

		// Token: 0x06002038 RID: 8248 RVA: 0x000505F2 File Offset: 0x0004F5F2
		public MethodInfo[] GetOtherMethods()
		{
			return this.GetOtherMethods(false);
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000505FB File Offset: 0x0004F5FB
		public MethodInfo GetAddMethod()
		{
			return this.GetAddMethod(false);
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x00050604 File Offset: 0x0004F604
		public MethodInfo GetRemoveMethod()
		{
			return this.GetRemoveMethod(false);
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0005060D File Offset: 0x0004F60D
		public MethodInfo GetRaiseMethod()
		{
			return this.GetRaiseMethod(false);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00050618 File Offset: 0x0004F618
		[DebuggerStepThrough]
		[DebuggerHidden]
		public void AddEventHandler(object target, Delegate handler)
		{
			MethodInfo addMethod = this.GetAddMethod();
			if (addMethod == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoPublicAddMethod"));
			}
			addMethod.Invoke(target, new object[] { handler });
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00050654 File Offset: 0x0004F654
		[DebuggerHidden]
		[DebuggerStepThrough]
		public void RemoveEventHandler(object target, Delegate handler)
		{
			MethodInfo removeMethod = this.GetRemoveMethod();
			if (removeMethod == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoPublicRemoveMethod"));
			}
			removeMethod.Invoke(target, new object[] { handler });
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x00050690 File Offset: 0x0004F690
		public Type EventHandlerType
		{
			get
			{
				MethodInfo addMethod = this.GetAddMethod(true);
				ParameterInfo[] parametersNoCopy = addMethod.GetParametersNoCopy();
				Type typeFromHandle = typeof(Delegate);
				for (int i = 0; i < parametersNoCopy.Length; i++)
				{
					Type parameterType = parametersNoCopy[i].ParameterType;
					if (parameterType.IsSubclassOf(typeFromHandle))
					{
						return parameterType;
					}
				}
				return null;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x000506DD File Offset: 0x0004F6DD
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & EventAttributes.SpecialName) != EventAttributes.None;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x000506F4 File Offset: 0x0004F6F4
		public bool IsMulticast
		{
			get
			{
				Type eventHandlerType = this.EventHandlerType;
				Type typeFromHandle = typeof(MulticastDelegate);
				return typeFromHandle.IsAssignableFrom(eventHandlerType);
			}
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x0005071A File Offset: 0x0004F71A
		Type _EventInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x00050722 File Offset: 0x0004F722
		void _EventInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x00050729 File Offset: 0x0004F729
		void _EventInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x00050730 File Offset: 0x0004F730
		void _EventInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x00050737 File Offset: 0x0004F737
		void _EventInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}
