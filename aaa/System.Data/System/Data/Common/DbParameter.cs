using System;
using System.ComponentModel;

namespace System.Data.Common
{
	// Token: 0x0200013B RID: 315
	public abstract class DbParameter : MarshalByRefObject, IDbDataParameter, IDataParameter
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060014A5 RID: 5285
		// (set) Token: 0x060014A6 RID: 5286
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbParameter_DbType")]
		public abstract DbType DbType { get; set; }

		// Token: 0x060014A7 RID: 5287
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public abstract void ResetDbType();

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060014A8 RID: 5288
		// (set) Token: 0x060014A9 RID: 5289
		[ResDescription("DbParameter_Direction")]
		[DefaultValue(ParameterDirection.Input)]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		public abstract ParameterDirection Direction { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060014AA RID: 5290
		// (set) Token: 0x060014AB RID: 5291
		[Browsable(false)]
		[DesignOnly(true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public abstract bool IsNullable { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060014AC RID: 5292
		// (set) Token: 0x060014AD RID: 5293
		[ResDescription("DbParameter_ParameterName")]
		[DefaultValue("")]
		[ResCategory("DataCategory_Data")]
		public abstract string ParameterName { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x0022832C File Offset: 0x0022772C
		// (set) Token: 0x060014AF RID: 5295 RVA: 0x0022833C File Offset: 0x0022773C
		byte IDbDataParameter.Precision
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0022834C File Offset: 0x0022774C
		// (set) Token: 0x060014B1 RID: 5297 RVA: 0x0022835C File Offset: 0x0022775C
		byte IDbDataParameter.Scale
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060014B2 RID: 5298
		// (set) Token: 0x060014B3 RID: 5299
		[ResDescription("DbParameter_Size")]
		[ResCategory("DataCategory_Data")]
		public abstract int Size { get; set; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060014B4 RID: 5300
		// (set) Token: 0x060014B5 RID: 5301
		[DefaultValue("")]
		[ResDescription("DbParameter_SourceColumn")]
		[ResCategory("DataCategory_Update")]
		public abstract string SourceColumn { get; set; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060014B6 RID: 5302
		// (set) Token: 0x060014B7 RID: 5303
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbParameter_SourceColumnNullMapping")]
		[DefaultValue(false)]
		[ResCategory("DataCategory_Update")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public abstract bool SourceColumnNullMapping { get; set; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060014B8 RID: 5304
		// (set) Token: 0x060014B9 RID: 5305
		[ResDescription("DbParameter_SourceVersion")]
		[ResCategory("DataCategory_Update")]
		[DefaultValue(DataRowVersion.Current)]
		public abstract DataRowVersion SourceVersion { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060014BA RID: 5306
		// (set) Token: 0x060014BB RID: 5307
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbParameter_Value")]
		[DefaultValue(null)]
		[ResCategory("DataCategory_Data")]
		public abstract object Value { get; set; }
	}
}
