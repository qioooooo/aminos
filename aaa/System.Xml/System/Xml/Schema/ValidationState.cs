using System;
using System.Collections.Generic;

namespace System.Xml.Schema
{
	// Token: 0x0200021C RID: 540
	internal sealed class ValidationState
	{
		// Token: 0x04001014 RID: 4116
		public bool IsNill;

		// Token: 0x04001015 RID: 4117
		public bool IsDefault;

		// Token: 0x04001016 RID: 4118
		public bool NeedValidateChildren;

		// Token: 0x04001017 RID: 4119
		public bool CheckRequiredAttribute;

		// Token: 0x04001018 RID: 4120
		public bool ValidationSkipped;

		// Token: 0x04001019 RID: 4121
		public int Depth;

		// Token: 0x0400101A RID: 4122
		public XmlSchemaContentProcessing ProcessContents;

		// Token: 0x0400101B RID: 4123
		public XmlSchemaValidity Validity;

		// Token: 0x0400101C RID: 4124
		public SchemaElementDecl ElementDecl;

		// Token: 0x0400101D RID: 4125
		public SchemaElementDecl ElementDeclBeforeXsi;

		// Token: 0x0400101E RID: 4126
		public string LocalName;

		// Token: 0x0400101F RID: 4127
		public string Namespace;

		// Token: 0x04001020 RID: 4128
		public ConstraintStruct[] Constr;

		// Token: 0x04001021 RID: 4129
		public StateUnion CurrentState;

		// Token: 0x04001022 RID: 4130
		public bool HasMatched;

		// Token: 0x04001023 RID: 4131
		public BitSet[] CurPos = new BitSet[2];

		// Token: 0x04001024 RID: 4132
		public BitSet AllElementsSet;

		// Token: 0x04001025 RID: 4133
		public List<RangePositionInfo> RunningPositions;

		// Token: 0x04001026 RID: 4134
		public bool TooComplex;
	}
}
