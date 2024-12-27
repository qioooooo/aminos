using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200011E RID: 286
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public struct UIntPtr : ISerializable
	{
		// Token: 0x060010D2 RID: 4306 RVA: 0x0002F2DA File Offset: 0x0002E2DA
		public UIntPtr(uint value)
		{
			this.m_value = value;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0002F2E4 File Offset: 0x0002E2E4
		public UIntPtr(ulong value)
		{
			this.m_value = checked((uint)value);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0002F2EF File Offset: 0x0002E2EF
		[CLSCompliant(false)]
		public unsafe UIntPtr(void* value)
		{
			this.m_value = value;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0002F2F8 File Offset: 0x0002E2F8
		private UIntPtr(SerializationInfo info, StreamingContext context)
		{
			ulong @uint = info.GetUInt64("value");
			if (UIntPtr.Size == 4 && @uint > (ulong)(-1))
			{
				throw new ArgumentException(Environment.GetResourceString("Serialization_InvalidPtrValue"));
			}
			this.m_value = @uint;
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0002F336 File Offset: 0x0002E336
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("value", this.m_value);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0002F358 File Offset: 0x0002E358
		public override bool Equals(object obj)
		{
			return obj is UIntPtr && this.m_value == ((UIntPtr)obj).m_value;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0002F385 File Offset: 0x0002E385
		public override int GetHashCode()
		{
			return this.m_value & int.MaxValue;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0002F395 File Offset: 0x0002E395
		public uint ToUInt32()
		{
			return this.m_value;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0002F39E File Offset: 0x0002E39E
		public ulong ToUInt64()
		{
			return this.m_value;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0002F3A8 File Offset: 0x0002E3A8
		public override string ToString()
		{
			return this.m_value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0002F3C9 File Offset: 0x0002E3C9
		public static explicit operator UIntPtr(uint value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0002F3D1 File Offset: 0x0002E3D1
		public static explicit operator UIntPtr(ulong value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0002F3D9 File Offset: 0x0002E3D9
		public static explicit operator uint(UIntPtr value)
		{
			return value.m_value;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0002F3E3 File Offset: 0x0002E3E3
		public static explicit operator ulong(UIntPtr value)
		{
			return value.m_value;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0002F3ED File Offset: 0x0002E3ED
		[CLSCompliant(false)]
		public unsafe static explicit operator UIntPtr(void* value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0002F3F5 File Offset: 0x0002E3F5
		[CLSCompliant(false)]
		public unsafe static explicit operator void*(UIntPtr value)
		{
			return value.ToPointer();
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0002F3FE File Offset: 0x0002E3FE
		public static bool operator ==(UIntPtr value1, UIntPtr value2)
		{
			return value1.m_value == value2.m_value;
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0002F410 File Offset: 0x0002E410
		public static bool operator !=(UIntPtr value1, UIntPtr value2)
		{
			return value1.m_value != value2.m_value;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x0002F425 File Offset: 0x0002E425
		public static int Size
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0002F428 File Offset: 0x0002E428
		[CLSCompliant(false)]
		public unsafe void* ToPointer()
		{
			return this.m_value;
		}

		// Token: 0x04000572 RID: 1394
		private unsafe void* m_value;

		// Token: 0x04000573 RID: 1395
		public static readonly UIntPtr Zero;
	}
}
