using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000C6 RID: 198
	[ComVisible(true)]
	[Serializable]
	public struct IntPtr : ISerializable
	{
		// Token: 0x06000B67 RID: 2919 RVA: 0x00022CB6 File Offset: 0x00021CB6
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal bool IsNull()
		{
			return this.m_value == null;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00022CC2 File Offset: 0x00021CC2
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public IntPtr(int value)
		{
			this.m_value = value;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00022CCC File Offset: 0x00021CCC
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public IntPtr(long value)
		{
			this.m_value = checked((int)value);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00022CD7 File Offset: 0x00021CD7
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe IntPtr(void* value)
		{
			this.m_value = value;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00022CE0 File Offset: 0x00021CE0
		private IntPtr(SerializationInfo info, StreamingContext context)
		{
			long @int = info.GetInt64("value");
			if (IntPtr.Size == 4 && (@int > 2147483647L || @int < -2147483648L))
			{
				throw new ArgumentException(Environment.GetResourceString("Serialization_InvalidPtrValue"));
			}
			this.m_value = @int;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00022D2B File Offset: 0x00021D2B
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("value", (long)this.m_value);
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00022D50 File Offset: 0x00021D50
		public override bool Equals(object obj)
		{
			return obj is IntPtr && this.m_value == ((IntPtr)obj).m_value;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00022D7D File Offset: 0x00021D7D
		public override int GetHashCode()
		{
			return this.m_value;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00022D87 File Offset: 0x00021D87
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int ToInt32()
		{
			return this.m_value;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00022D90 File Offset: 0x00021D90
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public long ToInt64()
		{
			return (long)this.m_value;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00022D9C File Offset: 0x00021D9C
		public override string ToString()
		{
			return this.m_value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00022DC0 File Offset: 0x00021DC0
		public string ToString(string format)
		{
			return this.m_value.ToString(format, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00022DE2 File Offset: 0x00021DE2
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static explicit operator IntPtr(int value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00022DEA File Offset: 0x00021DEA
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static explicit operator IntPtr(long value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00022DF2 File Offset: 0x00021DF2
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static explicit operator IntPtr(void* value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00022DFA File Offset: 0x00021DFA
		[CLSCompliant(false)]
		public unsafe static explicit operator void*(IntPtr value)
		{
			return value.ToPointer();
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00022E03 File Offset: 0x00021E03
		public static explicit operator int(IntPtr value)
		{
			return value.m_value;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00022E0D File Offset: 0x00021E0D
		public static explicit operator long(IntPtr value)
		{
			return (long)value.m_value;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00022E18 File Offset: 0x00021E18
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool operator ==(IntPtr value1, IntPtr value2)
		{
			return value1.m_value == value2.m_value;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00022E2A File Offset: 0x00021E2A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool operator !=(IntPtr value1, IntPtr value2)
		{
			return value1.m_value != value2.m_value;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00022E3F File Offset: 0x00021E3F
		public static int Size
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return 4;
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00022E42 File Offset: 0x00021E42
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public unsafe void* ToPointer()
		{
			return this.m_value;
		}

		// Token: 0x040003FE RID: 1022
		private unsafe void* m_value;

		// Token: 0x040003FF RID: 1023
		public static readonly IntPtr Zero;
	}
}
