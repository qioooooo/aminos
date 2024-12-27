using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000015 RID: 21
	internal sealed class NameInfo
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00005A64 File Offset: 0x00004A64
		internal void Init()
		{
			this.NInameSpaceEnum = InternalNameSpaceE.None;
			this.NIname = null;
			this.NIobjectId = 0L;
			this.NIassemId = 0L;
			this.NIprimitiveTypeEnum = InternalPrimitiveTypeE.Invalid;
			this.NItype = null;
			this.NIisSealed = false;
			this.NItransmitTypeOnObject = false;
			this.NItransmitTypeOnMember = false;
			this.NIisParentTypeOnObject = false;
			this.NIisMustUnderstand = false;
			this.NInamespace = null;
			this.NIheaderPrefix = null;
			this.NIitemName = null;
			this.NIisArray = false;
			this.NIisArrayItem = false;
			this.NIisTopLevelObject = false;
			this.NIisNestedObject = false;
			this.NIisHeader = false;
			this.NIisRemoteRecord = false;
			this.NIattributeInfo = null;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005B06 File Offset: 0x00004B06
		[Conditional("SER_LOGGING")]
		internal void Dump(string value)
		{
			SoapAttributeInfo niattributeInfo = this.NIattributeInfo;
		}

		// Token: 0x04000098 RID: 152
		internal InternalNameSpaceE NInameSpaceEnum;

		// Token: 0x04000099 RID: 153
		internal string NIname;

		// Token: 0x0400009A RID: 154
		internal long NIobjectId;

		// Token: 0x0400009B RID: 155
		internal long NIassemId;

		// Token: 0x0400009C RID: 156
		internal InternalPrimitiveTypeE NIprimitiveTypeEnum;

		// Token: 0x0400009D RID: 157
		internal Type NItype;

		// Token: 0x0400009E RID: 158
		internal bool NIisSealed;

		// Token: 0x0400009F RID: 159
		internal bool NIisMustUnderstand;

		// Token: 0x040000A0 RID: 160
		internal string NInamespace;

		// Token: 0x040000A1 RID: 161
		internal string NIheaderPrefix;

		// Token: 0x040000A2 RID: 162
		internal string NIitemName;

		// Token: 0x040000A3 RID: 163
		internal bool NIisArray;

		// Token: 0x040000A4 RID: 164
		internal bool NIisArrayItem;

		// Token: 0x040000A5 RID: 165
		internal bool NIisTopLevelObject;

		// Token: 0x040000A6 RID: 166
		internal bool NIisNestedObject;

		// Token: 0x040000A7 RID: 167
		internal bool NItransmitTypeOnObject;

		// Token: 0x040000A8 RID: 168
		internal bool NItransmitTypeOnMember;

		// Token: 0x040000A9 RID: 169
		internal bool NIisParentTypeOnObject;

		// Token: 0x040000AA RID: 170
		internal bool NIisHeader;

		// Token: 0x040000AB RID: 171
		internal bool NIisRemoteRecord;

		// Token: 0x040000AC RID: 172
		internal SoapAttributeInfo NIattributeInfo;
	}
}
