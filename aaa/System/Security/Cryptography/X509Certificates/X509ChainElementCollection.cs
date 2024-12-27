using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000334 RID: 820
	public sealed class X509ChainElementCollection : ICollection, IEnumerable
	{
		// Token: 0x060019E8 RID: 6632 RVA: 0x0005A4D0 File Offset: 0x000594D0
		internal X509ChainElementCollection()
		{
			this.m_elements = new X509ChainElement[0];
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0005A4E4 File Offset: 0x000594E4
		internal unsafe X509ChainElementCollection(IntPtr pSimpleChain)
		{
			CAPIBase.CERT_SIMPLE_CHAIN cert_SIMPLE_CHAIN = new CAPIBase.CERT_SIMPLE_CHAIN(Marshal.SizeOf(typeof(CAPIBase.CERT_SIMPLE_CHAIN)));
			uint num = (uint)Marshal.ReadInt32(pSimpleChain);
			if ((ulong)num > (ulong)((long)Marshal.SizeOf(cert_SIMPLE_CHAIN)))
			{
				num = (uint)Marshal.SizeOf(cert_SIMPLE_CHAIN);
			}
			X509Utils.memcpy(pSimpleChain, new IntPtr((void*)(&cert_SIMPLE_CHAIN)), num);
			this.m_elements = new X509ChainElement[cert_SIMPLE_CHAIN.cElement];
			for (int i = 0; i < this.m_elements.Length; i++)
			{
				this.m_elements[i] = new X509ChainElement(Marshal.ReadIntPtr(new IntPtr((long)cert_SIMPLE_CHAIN.rgpElement + (long)(i * Marshal.SizeOf(typeof(IntPtr))))));
			}
		}

		// Token: 0x170004F2 RID: 1266
		public X509ChainElement this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumNotStarted"));
				}
				if (index >= this.m_elements.Length)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
				}
				return this.m_elements[index];
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x0005A5DE File Offset: 0x000595DE
		public int Count
		{
			get
			{
				return this.m_elements.Length;
			}
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0005A5E8 File Offset: 0x000595E8
		public X509ChainElementEnumerator GetEnumerator()
		{
			return new X509ChainElementEnumerator(this);
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x0005A5F0 File Offset: 0x000595F0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new X509ChainElementEnumerator(this);
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x0005A5F8 File Offset: 0x000595F8
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0005A692 File Offset: 0x00059692
		public void CopyTo(X509ChainElement[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060019F0 RID: 6640 RVA: 0x0005A69C File Offset: 0x0005969C
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x0005A69F File Offset: 0x0005969F
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04001AE5 RID: 6885
		private X509ChainElement[] m_elements;
	}
}
