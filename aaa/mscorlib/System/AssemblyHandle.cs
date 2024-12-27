using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000109 RID: 265
	internal struct AssemblyHandle
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0002CDD1 File Offset: 0x0002BDD1
		internal unsafe void* Value
		{
			get
			{
				return this.m_ptr.ToPointer();
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0002CDDE File Offset: 0x0002BDDE
		internal unsafe AssemblyHandle(void* pAssembly)
		{
			this.m_ptr = new IntPtr(pAssembly);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0002CDEC File Offset: 0x0002BDEC
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.m_ptr);
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0002CDFC File Offset: 0x0002BDFC
		public override bool Equals(object obj)
		{
			return obj is AssemblyHandle && ((AssemblyHandle)obj).m_ptr == this.m_ptr;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0002CE2C File Offset: 0x0002BE2C
		public bool Equals(AssemblyHandle handle)
		{
			return handle.m_ptr == this.m_ptr;
		}

		// Token: 0x06000F8E RID: 3982
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Assembly GetAssembly();

		// Token: 0x06000F8F RID: 3983
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetManifestModule();

		// Token: 0x06000F90 RID: 3984 RVA: 0x0002CE40 File Offset: 0x0002BE40
		internal ModuleHandle GetManifestModule()
		{
			return new ModuleHandle(this._GetManifestModule());
		}

		// Token: 0x06000F91 RID: 3985
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool _AptcaCheck(IntPtr sourceAssembly);

		// Token: 0x06000F92 RID: 3986 RVA: 0x0002CE4D File Offset: 0x0002BE4D
		internal bool AptcaCheck(AssemblyHandle sourceAssembly)
		{
			return this._AptcaCheck((IntPtr)sourceAssembly.Value);
		}

		// Token: 0x06000F93 RID: 3987
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetToken();

		// Token: 0x040004FA RID: 1274
		private IntPtr m_ptr;
	}
}
