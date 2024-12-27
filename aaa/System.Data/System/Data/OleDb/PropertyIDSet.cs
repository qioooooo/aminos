using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200025B RID: 603
	internal sealed class PropertyIDSet : DbBuffer
	{
		// Token: 0x060020AD RID: 8365 RVA: 0x00263BF4 File Offset: 0x00262FF4
		internal PropertyIDSet(Guid propertySet, int propertyID)
			: base(PropertyIDSet.PropertyIDSetAndValueSize)
		{
			this._count = 1;
			IntPtr intPtr = ADP.IntPtrOffset(this.handle, PropertyIDSet.PropertyIDSetSize);
			Marshal.WriteIntPtr(this.handle, 0, intPtr);
			Marshal.WriteInt32(this.handle, ADP.PtrSize, 1);
			intPtr = ADP.IntPtrOffset(this.handle, ODB.OffsetOf_tagDBPROPIDSET_PropertySet);
			Marshal.StructureToPtr(propertySet, intPtr, false);
			Marshal.WriteInt32(this.handle, PropertyIDSet.PropertyIDSetSize, propertyID);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00263C74 File Offset: 0x00263074
		internal PropertyIDSet(Guid[] propertySets)
			: base(PropertyIDSet.PropertyIDSetSize * propertySets.Length)
		{
			this._count = propertySets.Length;
			for (int i = 0; i < propertySets.Length; i++)
			{
				IntPtr intPtr = ADP.IntPtrOffset(this.handle, i * PropertyIDSet.PropertyIDSetSize + ODB.OffsetOf_tagDBPROPIDSET_PropertySet);
				Marshal.StructureToPtr(propertySets[i], intPtr, false);
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060020AF RID: 8367 RVA: 0x00263CD8 File Offset: 0x002630D8
		internal int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x04001529 RID: 5417
		private static readonly int PropertyIDSetAndValueSize = ODB.SizeOf_tagDBPROPIDSET + ADP.PtrSize;

		// Token: 0x0400152A RID: 5418
		private static readonly int PropertyIDSetSize = ODB.SizeOf_tagDBPROPIDSET;

		// Token: 0x0400152B RID: 5419
		private int _count;
	}
}
