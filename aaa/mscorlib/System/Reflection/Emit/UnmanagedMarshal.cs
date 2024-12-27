using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x0200083F RID: 2111
	[ComVisible(true)]
	[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[Serializable]
	public sealed class UnmanagedMarshal
	{
		// Token: 0x06004DF5 RID: 19957 RVA: 0x0010F562 File Offset: 0x0010E562
		public static UnmanagedMarshal DefineUnmanagedMarshal(UnmanagedType unmanagedType)
		{
			if (unmanagedType == UnmanagedType.ByValTStr || unmanagedType == UnmanagedType.SafeArray || unmanagedType == UnmanagedType.ByValArray || unmanagedType == UnmanagedType.LPArray || unmanagedType == UnmanagedType.CustomMarshaler)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotASimpleNativeType"));
			}
			return new UnmanagedMarshal(unmanagedType, Guid.Empty, 0, (UnmanagedType)0);
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x0010F59A File Offset: 0x0010E59A
		public static UnmanagedMarshal DefineByValTStr(int elemCount)
		{
			return new UnmanagedMarshal(UnmanagedType.ByValTStr, Guid.Empty, elemCount, (UnmanagedType)0);
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x0010F5AA File Offset: 0x0010E5AA
		public static UnmanagedMarshal DefineSafeArray(UnmanagedType elemType)
		{
			return new UnmanagedMarshal(UnmanagedType.SafeArray, Guid.Empty, 0, elemType);
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x0010F5BA File Offset: 0x0010E5BA
		public static UnmanagedMarshal DefineByValArray(int elemCount)
		{
			return new UnmanagedMarshal(UnmanagedType.ByValArray, Guid.Empty, elemCount, (UnmanagedType)0);
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x0010F5CA File Offset: 0x0010E5CA
		public static UnmanagedMarshal DefineLPArray(UnmanagedType elemType)
		{
			return new UnmanagedMarshal(UnmanagedType.LPArray, Guid.Empty, 0, elemType);
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06004DFA RID: 19962 RVA: 0x0010F5DA File Offset: 0x0010E5DA
		public UnmanagedType GetUnmanagedType
		{
			get
			{
				return this.m_unmanagedType;
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06004DFB RID: 19963 RVA: 0x0010F5E2 File Offset: 0x0010E5E2
		public Guid IIDGuid
		{
			get
			{
				if (this.m_unmanagedType != UnmanagedType.CustomMarshaler)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotACustomMarshaler"));
				}
				return this.m_guid;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06004DFC RID: 19964 RVA: 0x0010F604 File Offset: 0x0010E604
		public int ElementCount
		{
			get
			{
				if (this.m_unmanagedType != UnmanagedType.ByValArray && this.m_unmanagedType != UnmanagedType.ByValTStr)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NoUnmanagedElementCount"));
				}
				return this.m_numElem;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06004DFD RID: 19965 RVA: 0x0010F630 File Offset: 0x0010E630
		public UnmanagedType BaseType
		{
			get
			{
				if (this.m_unmanagedType != UnmanagedType.LPArray && this.m_unmanagedType != UnmanagedType.SafeArray)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NoNestedMarshal"));
				}
				return this.m_baseType;
			}
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x0010F65C File Offset: 0x0010E65C
		private UnmanagedMarshal(UnmanagedType unmanagedType, Guid guid, int numElem, UnmanagedType type)
		{
			this.m_unmanagedType = unmanagedType;
			this.m_guid = guid;
			this.m_numElem = numElem;
			this.m_baseType = type;
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x0010F684 File Offset: 0x0010E684
		internal byte[] InternalGetBytes()
		{
			if (this.m_unmanagedType == UnmanagedType.SafeArray || this.m_unmanagedType == UnmanagedType.LPArray)
			{
				int num = 2;
				byte[] array = new byte[num];
				array[0] = (byte)this.m_unmanagedType;
				array[1] = (byte)this.m_baseType;
				return array;
			}
			if (this.m_unmanagedType == UnmanagedType.ByValArray || this.m_unmanagedType == UnmanagedType.ByValTStr)
			{
				int num2 = 0;
				int num3;
				if (this.m_numElem <= 127)
				{
					num3 = 1;
				}
				else if (this.m_numElem <= 16383)
				{
					num3 = 2;
				}
				else
				{
					num3 = 4;
				}
				num3++;
				byte[] array = new byte[num3];
				array[num2++] = (byte)this.m_unmanagedType;
				if (this.m_numElem <= 127)
				{
					array[num2++] = (byte)(this.m_numElem & 255);
				}
				else if (this.m_numElem <= 16383)
				{
					array[num2++] = (byte)((this.m_numElem >> 8) | 128);
					array[num2++] = (byte)(this.m_numElem & 255);
				}
				else if (this.m_numElem <= 536870911)
				{
					array[num2++] = (byte)((this.m_numElem >> 24) | 192);
					array[num2++] = (byte)((this.m_numElem >> 16) & 255);
					array[num2++] = (byte)((this.m_numElem >> 8) & 255);
					array[num2++] = (byte)(this.m_numElem & 255);
				}
				return array;
			}
			return new byte[] { (byte)this.m_unmanagedType };
		}

		// Token: 0x04002809 RID: 10249
		internal UnmanagedType m_unmanagedType;

		// Token: 0x0400280A RID: 10250
		internal Guid m_guid;

		// Token: 0x0400280B RID: 10251
		internal int m_numElem;

		// Token: 0x0400280C RID: 10252
		internal UnmanagedType m_baseType;
	}
}
