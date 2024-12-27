using System;
using System.Globalization;

namespace System.Data.OleDb
{
	// Token: 0x0200023E RID: 574
	public sealed class OleDbSchemaGuid
	{
		// Token: 0x06002067 RID: 8295 RVA: 0x002620E0 File Offset: 0x002614E0
		internal static string GetTextFromValue(Guid guid)
		{
			if (guid == OleDbSchemaGuid.Primary_Keys)
			{
				return "Primary_Keys";
			}
			if (guid == OleDbSchemaGuid.Indexes)
			{
				return "Indexes";
			}
			if (guid == OleDbSchemaGuid.Procedure_Parameters)
			{
				return "Procedure_Parameters";
			}
			if (guid == OleDbSchemaGuid.Procedures)
			{
				return "Procedures";
			}
			if (guid == OleDbSchemaGuid.Tables_Info)
			{
				return "Tables_Info";
			}
			if (guid == OleDbSchemaGuid.Trustee)
			{
				return "Trustee";
			}
			if (guid == OleDbSchemaGuid.Assertions)
			{
				return "Assertions";
			}
			if (guid == OleDbSchemaGuid.Catalogs)
			{
				return "Catalogs";
			}
			if (guid == OleDbSchemaGuid.Character_Sets)
			{
				return "Character_Sets";
			}
			if (guid == OleDbSchemaGuid.Collations)
			{
				return "Collations";
			}
			if (guid == OleDbSchemaGuid.Columns)
			{
				return "Columns";
			}
			if (guid == OleDbSchemaGuid.Check_Constraints)
			{
				return "Check_Constraints";
			}
			if (guid == OleDbSchemaGuid.Constraint_Column_Usage)
			{
				return "Constraint_Column_Usage";
			}
			if (guid == OleDbSchemaGuid.Constraint_Table_Usage)
			{
				return "Constraint_Table_Usage";
			}
			if (guid == OleDbSchemaGuid.Key_Column_Usage)
			{
				return "Key_Column_Usage";
			}
			if (guid == OleDbSchemaGuid.Referential_Constraints)
			{
				return "Referential_Constraints";
			}
			if (guid == OleDbSchemaGuid.Table_Constraints)
			{
				return "Table_Constraints";
			}
			if (guid == OleDbSchemaGuid.Column_Domain_Usage)
			{
				return "Column_Domain_Usage";
			}
			if (guid == OleDbSchemaGuid.Column_Privileges)
			{
				return "Column_Privileges";
			}
			if (guid == OleDbSchemaGuid.Table_Privileges)
			{
				return "Table_Privileges";
			}
			if (guid == OleDbSchemaGuid.Usage_Privileges)
			{
				return "Usage_Privileges";
			}
			if (guid == OleDbSchemaGuid.Schemata)
			{
				return "Schemata";
			}
			if (guid == OleDbSchemaGuid.Sql_Languages)
			{
				return "Sql_Languages";
			}
			if (guid == OleDbSchemaGuid.Statistics)
			{
				return "Statistics";
			}
			if (guid == OleDbSchemaGuid.Tables)
			{
				return "Tables";
			}
			if (guid == OleDbSchemaGuid.Translations)
			{
				return "Translations";
			}
			if (guid == OleDbSchemaGuid.Provider_Types)
			{
				return "Provider_Types";
			}
			if (guid == OleDbSchemaGuid.Views)
			{
				return "Views";
			}
			if (guid == OleDbSchemaGuid.View_Column_Usage)
			{
				return "View_Column_Usage";
			}
			if (guid == OleDbSchemaGuid.View_Table_Usage)
			{
				return "View_Table_Usage";
			}
			if (guid == OleDbSchemaGuid.Foreign_Keys)
			{
				return "Foreign_Keys";
			}
			if (guid == OleDbSchemaGuid.Procedure_Columns)
			{
				return "Procedure_Columns";
			}
			if (guid == OleDbSchemaGuid.Table_Statistics)
			{
				return "Table_Statistics";
			}
			if (guid == OleDbSchemaGuid.Check_Constraints_By_Table)
			{
				return "Check_Constraints_By_Table";
			}
			return "{" + guid.ToString("D", CultureInfo.InvariantCulture) + ")";
		}

		// Token: 0x04001472 RID: 5234
		public static readonly Guid Tables_Info = new Guid(3367314144U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001473 RID: 5235
		public static readonly Guid Trustee = new Guid(3367314159U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001474 RID: 5236
		public static readonly Guid Assertions = new Guid(3367313936U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001475 RID: 5237
		public static readonly Guid Catalogs = new Guid(3367313937U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001476 RID: 5238
		public static readonly Guid Character_Sets = new Guid(3367313938U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001477 RID: 5239
		public static readonly Guid Collations = new Guid(3367313939U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001478 RID: 5240
		public static readonly Guid Columns = new Guid(3367313940U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001479 RID: 5241
		public static readonly Guid Check_Constraints = new Guid(3367313941U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400147A RID: 5242
		public static readonly Guid Constraint_Column_Usage = new Guid(3367313942U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400147B RID: 5243
		public static readonly Guid Constraint_Table_Usage = new Guid(3367313943U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400147C RID: 5244
		public static readonly Guid Key_Column_Usage = new Guid(3367313944U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400147D RID: 5245
		public static readonly Guid Referential_Constraints = new Guid(3367313945U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400147E RID: 5246
		public static readonly Guid Table_Constraints = new Guid(3367313946U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400147F RID: 5247
		public static readonly Guid Column_Domain_Usage = new Guid(3367313947U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001480 RID: 5248
		public static readonly Guid Indexes = new Guid(3367313950U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001481 RID: 5249
		public static readonly Guid Column_Privileges = new Guid(3367313953U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001482 RID: 5250
		public static readonly Guid Table_Privileges = new Guid(3367313954U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001483 RID: 5251
		public static readonly Guid Usage_Privileges = new Guid(3367313955U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001484 RID: 5252
		public static readonly Guid Procedures = new Guid(3367313956U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001485 RID: 5253
		public static readonly Guid Schemata = new Guid(3367313957U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001486 RID: 5254
		public static readonly Guid Sql_Languages = new Guid(3367313958U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001487 RID: 5255
		public static readonly Guid Statistics = new Guid(3367313959U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001488 RID: 5256
		public static readonly Guid Tables = new Guid(3367313961U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001489 RID: 5257
		public static readonly Guid Translations = new Guid(3367313962U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400148A RID: 5258
		public static readonly Guid Provider_Types = new Guid(3367313964U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400148B RID: 5259
		public static readonly Guid Views = new Guid(3367313965U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400148C RID: 5260
		public static readonly Guid View_Column_Usage = new Guid(3367313966U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400148D RID: 5261
		public static readonly Guid View_Table_Usage = new Guid(3367313967U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400148E RID: 5262
		public static readonly Guid Procedure_Parameters = new Guid(3367314104U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x0400148F RID: 5263
		public static readonly Guid Foreign_Keys = new Guid(3367314116U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001490 RID: 5264
		public static readonly Guid Primary_Keys = new Guid(3367314117U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001491 RID: 5265
		public static readonly Guid Procedure_Columns = new Guid(3367314121U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001492 RID: 5266
		public static readonly Guid Table_Statistics = new Guid(3367314175U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001493 RID: 5267
		public static readonly Guid Check_Constraints_By_Table = new Guid(3367314177U, 23795, 4558, 173, 229, 0, 170, 0, 68, 119, 61);

		// Token: 0x04001494 RID: 5268
		public static readonly Guid SchemaGuids = new Guid(4079373467U, 6240, 19966, 183, 27, 41, 97, 178, 234, 145, 189);

		// Token: 0x04001495 RID: 5269
		public static readonly Guid DbInfoKeywords = new Guid(4079373468U, 6240, 19966, 183, 27, 41, 97, 178, 234, 145, 189);

		// Token: 0x04001496 RID: 5270
		public static readonly Guid DbInfoLiterals = new Guid(4079373469U, 6240, 19966, 183, 27, 41, 97, 178, 234, 145, 189);
	}
}
