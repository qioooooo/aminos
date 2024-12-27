using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x0200010B RID: 267
	internal class Signature
	{
		// Token: 0x06000FB7 RID: 4023 RVA: 0x0002D409 File Offset: 0x0002C409
		public static implicit operator SignatureStruct(Signature pThis)
		{
			return pThis.m_signature;
		}

		// Token: 0x06000FB8 RID: 4024
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetSignature(ref SignatureStruct signature, void* pCorSig, int cCorSig, IntPtr fieldHandle, IntPtr methodHandle, IntPtr declaringTypeHandle);

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0002D411 File Offset: 0x0002C411
		private unsafe static void GetSignature(ref SignatureStruct signature, void* pCorSig, int cCorSig, RuntimeFieldHandle fieldHandle, RuntimeMethodHandle methodHandle, RuntimeTypeHandle declaringTypeHandle)
		{
			Signature._GetSignature(ref signature, pCorSig, cCorSig, fieldHandle.Value, methodHandle.Value, declaringTypeHandle.Value);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0002D430 File Offset: 0x0002C430
		internal static void GetSignatureForDynamicMethod(ref SignatureStruct signature, RuntimeMethodHandle methodHandle)
		{
			Signature._GetSignature(ref signature, null, 0, (IntPtr)0, methodHandle.Value, (IntPtr)0);
		}

		// Token: 0x06000FBB RID: 4027
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCustomModifiers(ref SignatureStruct signature, int parameter, out RuntimeTypeHandle[] required, out RuntimeTypeHandle[] optional);

		// Token: 0x06000FBC RID: 4028
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CompareSig(ref SignatureStruct left, RuntimeTypeHandle typeLeft, ref SignatureStruct right, RuntimeTypeHandle typeRight);

		// Token: 0x06000FBD RID: 4029 RVA: 0x0002D450 File Offset: 0x0002C450
		public Signature(RuntimeMethodHandle method, RuntimeTypeHandle[] arguments, RuntimeTypeHandle returnType, CallingConventions callingConvention)
		{
			SignatureStruct signatureStruct = new SignatureStruct(method, arguments, returnType, callingConvention);
			Signature.GetSignatureForDynamicMethod(ref signatureStruct, method);
			this.m_signature = signatureStruct;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0002D484 File Offset: 0x0002C484
		public Signature(RuntimeMethodHandle methodHandle, RuntimeTypeHandle declaringTypeHandle)
		{
			SignatureStruct signatureStruct = default(SignatureStruct);
			Signature.GetSignature(ref signatureStruct, null, 0, new RuntimeFieldHandle(null), methodHandle, declaringTypeHandle);
			this.m_signature = signatureStruct;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0002D4BC File Offset: 0x0002C4BC
		public Signature(RuntimeFieldHandle fieldHandle, RuntimeTypeHandle declaringTypeHandle)
		{
			SignatureStruct signatureStruct = default(SignatureStruct);
			Signature.GetSignature(ref signatureStruct, null, 0, fieldHandle, new RuntimeMethodHandle(null), declaringTypeHandle);
			this.m_signature = signatureStruct;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0002D4F4 File Offset: 0x0002C4F4
		public unsafe Signature(void* pCorSig, int cCorSig, RuntimeTypeHandle declaringTypeHandle)
		{
			SignatureStruct signatureStruct = default(SignatureStruct);
			Signature.GetSignature(ref signatureStruct, pCorSig, cCorSig, new RuntimeFieldHandle(null), new RuntimeMethodHandle(null), declaringTypeHandle);
			this.m_signature = signatureStruct;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0002D52E File Offset: 0x0002C52E
		internal CallingConventions CallingConvention
		{
			get
			{
				return this.m_signature.m_managedCallingConvention & (CallingConventions)255;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0002D541 File Offset: 0x0002C541
		internal RuntimeTypeHandle[] Arguments
		{
			get
			{
				return this.m_signature.m_arguments;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0002D54E File Offset: 0x0002C54E
		internal RuntimeTypeHandle ReturnTypeHandle
		{
			get
			{
				return this.m_signature.m_returnTypeORfieldType;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0002D55B File Offset: 0x0002C55B
		internal RuntimeTypeHandle FieldTypeHandle
		{
			get
			{
				return this.m_signature.m_returnTypeORfieldType;
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0002D568 File Offset: 0x0002C568
		internal static bool DiffSigs(Signature sig1, RuntimeTypeHandle typeHandle1, Signature sig2, RuntimeTypeHandle typeHandle2)
		{
			SignatureStruct signatureStruct = sig1;
			SignatureStruct signatureStruct2 = sig2;
			return Signature.CompareSig(ref signatureStruct, typeHandle1, ref signatureStruct2, typeHandle2);
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0002D590 File Offset: 0x0002C590
		public Type[] GetCustomModifiers(int position, bool required)
		{
			RuntimeTypeHandle[] array = null;
			RuntimeTypeHandle[] array2 = null;
			SignatureStruct signatureStruct = this;
			Signature.GetCustomModifiers(ref signatureStruct, position, out array, out array2);
			Type[] array3 = new Type[required ? array.Length : array2.Length];
			if (required)
			{
				for (int i = 0; i < array3.Length; i++)
				{
					array3[i] = array[i].GetRuntimeType();
				}
			}
			else
			{
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = array2[j].GetRuntimeType();
				}
			}
			return array3;
		}

		// Token: 0x040004FD RID: 1277
		internal SignatureStruct m_signature;

		// Token: 0x0200010C RID: 268
		internal enum MdSigCallingConvention : byte
		{
			// Token: 0x040004FF RID: 1279
			Generics = 16,
			// Token: 0x04000500 RID: 1280
			HasThis = 32,
			// Token: 0x04000501 RID: 1281
			ExplicitThis = 64,
			// Token: 0x04000502 RID: 1282
			CallConvMask = 15,
			// Token: 0x04000503 RID: 1283
			Default = 0,
			// Token: 0x04000504 RID: 1284
			C,
			// Token: 0x04000505 RID: 1285
			StdCall,
			// Token: 0x04000506 RID: 1286
			ThisCall,
			// Token: 0x04000507 RID: 1287
			FastCall,
			// Token: 0x04000508 RID: 1288
			Vararg,
			// Token: 0x04000509 RID: 1289
			Field,
			// Token: 0x0400050A RID: 1290
			LocalSig,
			// Token: 0x0400050B RID: 1291
			Property,
			// Token: 0x0400050C RID: 1292
			Unmgd,
			// Token: 0x0400050D RID: 1293
			GenericInst,
			// Token: 0x0400050E RID: 1294
			Max
		}
	}
}
