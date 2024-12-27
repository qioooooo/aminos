using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x0200080D RID: 2061
	[ComDefaultInterface(typeof(_EventBuilder))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class EventBuilder : _EventBuilder
	{
		// Token: 0x060049CC RID: 18892 RVA: 0x00101821 File Offset: 0x00100821
		private EventBuilder()
		{
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x00101829 File Offset: 0x00100829
		internal EventBuilder(Module mod, string name, EventAttributes attr, int eventType, TypeBuilder type, EventToken evToken)
		{
			this.m_name = name;
			this.m_module = mod;
			this.m_attributes = attr;
			this.m_evToken = evToken;
			this.m_type = type;
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x00101856 File Offset: 0x00100856
		public EventToken GetEventToken()
		{
			return this.m_evToken;
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x00101860 File Offset: 0x00100860
		public void SetAddOnMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.AddOn, mdBuilder.GetToken().Token);
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x001018AC File Offset: 0x001008AC
		public void SetRemoveOnMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.RemoveOn, mdBuilder.GetToken().Token);
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x001018F8 File Offset: 0x001008F8
		public void SetRaiseMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.Fire, mdBuilder.GetToken().Token);
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00101944 File Offset: 0x00100944
		public void AddOtherMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.Other, mdBuilder.GetToken().Token);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x00101990 File Offset: 0x00100990
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalCreateCustomAttribute(this.m_evToken.Token, ((ModuleBuilder)this.m_module).GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x001019F5 File Offset: 0x001009F5
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			this.m_type.ThrowIfCreated();
			customBuilder.CreateCustomAttribute((ModuleBuilder)this.m_module, this.m_evToken.Token);
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x00101A2C File Offset: 0x00100A2C
		void _EventBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x00101A33 File Offset: 0x00100A33
		void _EventBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x00101A3A File Offset: 0x00100A3A
		void _EventBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x00101A41 File Offset: 0x00100A41
		void _EventBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400258F RID: 9615
		private string m_name;

		// Token: 0x04002590 RID: 9616
		private EventToken m_evToken;

		// Token: 0x04002591 RID: 9617
		private Module m_module;

		// Token: 0x04002592 RID: 9618
		private EventAttributes m_attributes;

		// Token: 0x04002593 RID: 9619
		private TypeBuilder m_type;
	}
}
