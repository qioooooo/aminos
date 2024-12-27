using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000818 RID: 2072
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_LocalBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class LocalBuilder : LocalVariableInfo, _LocalBuilder
	{
		// Token: 0x06004A30 RID: 18992 RVA: 0x00102908 File Offset: 0x00101908
		private LocalBuilder()
		{
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x00102910 File Offset: 0x00101910
		internal LocalBuilder(int localIndex, Type localType, MethodInfo methodBuilder)
			: this(localIndex, localType, methodBuilder, false)
		{
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x0010291C File Offset: 0x0010191C
		internal LocalBuilder(int localIndex, Type localType, MethodInfo methodBuilder, bool isPinned)
		{
			this.m_isPinned = isPinned;
			this.m_localIndex = localIndex;
			this.m_localType = localType;
			this.m_methodBuilder = methodBuilder;
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x00102941 File Offset: 0x00101941
		internal int GetLocalIndex()
		{
			return this.m_localIndex;
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x00102949 File Offset: 0x00101949
		internal MethodInfo GetMethodBuilder()
		{
			return this.m_methodBuilder;
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06004A35 RID: 18997 RVA: 0x00102951 File Offset: 0x00101951
		public override bool IsPinned
		{
			get
			{
				return this.m_isPinned;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06004A36 RID: 18998 RVA: 0x00102959 File Offset: 0x00101959
		public override Type LocalType
		{
			get
			{
				return this.m_localType;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06004A37 RID: 18999 RVA: 0x00102961 File Offset: 0x00101961
		public override int LocalIndex
		{
			get
			{
				return this.m_localIndex;
			}
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x00102969 File Offset: 0x00101969
		public void SetLocalSymInfo(string name)
		{
			this.SetLocalSymInfo(name, 0, 0);
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x00102974 File Offset: 0x00101974
		public void SetLocalSymInfo(string name, int startOffset, int endOffset)
		{
			MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder == null)
			{
				throw new NotSupportedException();
			}
			ModuleBuilder moduleBuilder = (ModuleBuilder)methodBuilder.Module;
			if (methodBuilder.IsTypeCreated())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
			}
			if (moduleBuilder.GetSymWriter() == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			SignatureHelper fieldSigHelper = SignatureHelper.GetFieldSigHelper(moduleBuilder);
			fieldSigHelper.AddArgument(this.m_localType);
			int num;
			byte[] array = fieldSigHelper.InternalGetSignature(out num);
			byte[] array2 = new byte[num - 1];
			Array.Copy(array, 1, array2, 0, num - 1);
			int currentActiveScopeIndex = methodBuilder.GetILGenerator().m_ScopeTree.GetCurrentActiveScopeIndex();
			if (currentActiveScopeIndex == -1)
			{
				methodBuilder.m_localSymInfo.AddLocalSymInfo(name, array2, this.m_localIndex, startOffset, endOffset);
				return;
			}
			methodBuilder.GetILGenerator().m_ScopeTree.AddLocalSymInfoToCurrentScope(name, array2, this.m_localIndex, startOffset, endOffset);
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x00102A55 File Offset: 0x00101A55
		void _LocalBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A3B RID: 19003 RVA: 0x00102A5C File Offset: 0x00101A5C
		void _LocalBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x00102A63 File Offset: 0x00101A63
		void _LocalBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x00102A6A File Offset: 0x00101A6A
		void _LocalBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040025D0 RID: 9680
		private int m_localIndex;

		// Token: 0x040025D1 RID: 9681
		private Type m_localType;

		// Token: 0x040025D2 RID: 9682
		private MethodInfo m_methodBuilder;

		// Token: 0x040025D3 RID: 9683
		private bool m_isPinned;
	}
}
