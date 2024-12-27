using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000043 RID: 67
	internal class SmiMetaDataPropertyCollection
	{
		// Token: 0x0600026D RID: 621 RVA: 0x001CCE20 File Offset: 0x001CC220
		static SmiMetaDataPropertyCollection()
		{
			SmiMetaDataPropertyCollection.EmptyInstance.SetReadOnly();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x001CCE70 File Offset: 0x001CC270
		internal SmiMetaDataPropertyCollection()
		{
			this._properties = new SmiMetaDataProperty[3];
			this._isReadOnly = false;
			this._properties[0] = SmiMetaDataPropertyCollection.__emptyDefaultFields;
			this._properties[1] = SmiMetaDataPropertyCollection.__emptySortOrder;
			this._properties[2] = SmiMetaDataPropertyCollection.__emptyUniqueKey;
		}

		// Token: 0x17000041 RID: 65
		internal SmiMetaDataProperty this[SmiPropertySelector key]
		{
			get
			{
				return this._properties[(int)key];
			}
			set
			{
				if (value == null)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
				}
				this.EnsureWritable();
				this._properties[(int)key] = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000271 RID: 625 RVA: 0x001CCF00 File Offset: 0x001CC300
		internal bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000272 RID: 626 RVA: 0x001CCF14 File Offset: 0x001CC314
		internal IEnumerable<SmiMetaDataProperty> Values
		{
			get
			{
				return new List<SmiMetaDataProperty>(this._properties);
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x001CCF2C File Offset: 0x001CC32C
		internal void SetReadOnly()
		{
			this._isReadOnly = true;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x001CCF40 File Offset: 0x001CC340
		private void EnsureWritable()
		{
			if (this.IsReadOnly)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
		}

		// Token: 0x040005E2 RID: 1506
		private const int SelectorCount = 3;

		// Token: 0x040005E3 RID: 1507
		private SmiMetaDataProperty[] _properties;

		// Token: 0x040005E4 RID: 1508
		private bool _isReadOnly;

		// Token: 0x040005E5 RID: 1509
		internal static readonly SmiMetaDataPropertyCollection EmptyInstance = new SmiMetaDataPropertyCollection();

		// Token: 0x040005E6 RID: 1510
		private static readonly SmiDefaultFieldsProperty __emptyDefaultFields = new SmiDefaultFieldsProperty(new List<bool>());

		// Token: 0x040005E7 RID: 1511
		private static readonly SmiOrderProperty __emptySortOrder = new SmiOrderProperty(new List<SmiOrderProperty.SmiColumnOrder>());

		// Token: 0x040005E8 RID: 1512
		private static readonly SmiUniqueKeyProperty __emptyUniqueKey = new SmiUniqueKeyProperty(new List<bool>());
	}
}
