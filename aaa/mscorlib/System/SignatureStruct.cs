using System;
using System.Reflection;

namespace System
{
	// Token: 0x0200010D RID: 269
	internal struct SignatureStruct
	{
		// Token: 0x06000FC7 RID: 4039 RVA: 0x0002D614 File Offset: 0x0002C614
		public SignatureStruct(RuntimeMethodHandle method, RuntimeTypeHandle[] arguments, RuntimeTypeHandle returnType, CallingConventions callingConvention)
		{
			this.m_pMethod = method;
			this.m_arguments = arguments;
			this.m_returnTypeORfieldType = returnType;
			this.m_managedCallingConvention = callingConvention;
			this.m_sig = null;
			this.m_pCallTarget = null;
			this.m_csig = 0;
			this.m_numVirtualFixedArgs = 0;
			this.m_64bitpad = 0;
			this.m_declaringType = default(RuntimeTypeHandle);
		}

		// Token: 0x0400050F RID: 1295
		internal RuntimeTypeHandle[] m_arguments;

		// Token: 0x04000510 RID: 1296
		internal unsafe void* m_sig;

		// Token: 0x04000511 RID: 1297
		internal unsafe void* m_pCallTarget;

		// Token: 0x04000512 RID: 1298
		internal CallingConventions m_managedCallingConvention;

		// Token: 0x04000513 RID: 1299
		internal int m_csig;

		// Token: 0x04000514 RID: 1300
		internal int m_numVirtualFixedArgs;

		// Token: 0x04000515 RID: 1301
		internal int m_64bitpad;

		// Token: 0x04000516 RID: 1302
		internal RuntimeMethodHandle m_pMethod;

		// Token: 0x04000517 RID: 1303
		internal RuntimeTypeHandle m_declaringType;

		// Token: 0x04000518 RID: 1304
		internal RuntimeTypeHandle m_returnTypeORfieldType;
	}
}
