using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000325 RID: 805
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class Pointer : ISerializable
	{
		// Token: 0x06001F61 RID: 8033 RVA: 0x0004F66E File Offset: 0x0004E66E
		private Pointer()
		{
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0004F678 File Offset: 0x0004E678
		private Pointer(SerializationInfo info, StreamingContext context)
		{
			this._ptr = ((IntPtr)info.GetValue("_ptr", typeof(IntPtr))).ToPointer();
			this._ptrType = (Type)info.GetValue("_ptrType", typeof(Type));
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0004F6D4 File Offset: 0x0004E6D4
		public unsafe static object Box(void* ptr, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsPointer)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBePointer"), "ptr");
			}
			return new Pointer
			{
				_ptr = ptr,
				_ptrType = type
			};
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0004F721 File Offset: 0x0004E721
		public unsafe static void* Unbox(object ptr)
		{
			if (!(ptr is Pointer))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBePointer"), "ptr");
			}
			return ((Pointer)ptr)._ptr;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0004F74B File Offset: 0x0004E74B
		internal Type GetPointerType()
		{
			return this._ptrType;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0004F753 File Offset: 0x0004E753
		internal object GetPointerValue()
		{
			return (IntPtr)this._ptr;
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0004F765 File Offset: 0x0004E765
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_ptr", new IntPtr(this._ptr));
			info.AddValue("_ptrType", this._ptrType);
		}

		// Token: 0x04000D5D RID: 3421
		private unsafe void* _ptr;

		// Token: 0x04000D5E RID: 3422
		private Type _ptrType;
	}
}
