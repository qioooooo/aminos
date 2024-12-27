using System;
using System.Collections;

namespace System.Security.AccessControl
{
	// Token: 0x020008EF RID: 2287
	public abstract class GenericAcl : ICollection, IEnumerable
	{
		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x0600532B RID: 21291
		public abstract byte Revision { get; }

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x0600532C RID: 21292
		public abstract int BinaryLength { get; }

		// Token: 0x17000E68 RID: 3688
		public abstract GenericAce this[int index] { get; set; }

		// Token: 0x0600532F RID: 21295
		public abstract void GetBinaryForm(byte[] binaryForm, int offset);

		// Token: 0x06005330 RID: 21296 RVA: 0x0012DD14 File Offset: 0x0012CD14
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new RankException(Environment.GetResourceString("Rank_MultiDimNotSupported"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentOutOfRangeException("array", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index + i);
			}
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x0012DDA7 File Offset: 0x0012CDA7
		public void CopyTo(GenericAce[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06005332 RID: 21298
		public abstract int Count { get; }

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06005333 RID: 21299 RVA: 0x0012DDB1 File Offset: 0x0012CDB1
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06005334 RID: 21300 RVA: 0x0012DDB4 File Offset: 0x0012CDB4
		public object SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x0012DDB7 File Offset: 0x0012CDB7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new AceEnumerator(this);
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x0012DDBF File Offset: 0x0012CDBF
		public AceEnumerator GetEnumerator()
		{
			return ((IEnumerable)this).GetEnumerator() as AceEnumerator;
		}

		// Token: 0x04002B01 RID: 11009
		internal const int HeaderLength = 8;

		// Token: 0x04002B02 RID: 11010
		public static readonly byte AclRevision = 2;

		// Token: 0x04002B03 RID: 11011
		public static readonly byte AclRevisionDS = 4;

		// Token: 0x04002B04 RID: 11012
		public static readonly int MaxBinaryLength = 65535;
	}
}
