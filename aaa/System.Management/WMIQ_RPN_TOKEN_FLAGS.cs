﻿using System;

namespace System.Management
{
	// Token: 0x020000F3 RID: 243
	internal enum WMIQ_RPN_TOKEN_FLAGS
	{
		// Token: 0x04000489 RID: 1161
		WMIQ_RPN_TOKEN_EXPRESSION = 1,
		// Token: 0x0400048A RID: 1162
		WMIQ_RPN_TOKEN_AND,
		// Token: 0x0400048B RID: 1163
		WMIQ_RPN_TOKEN_OR,
		// Token: 0x0400048C RID: 1164
		WMIQ_RPN_TOKEN_NOT,
		// Token: 0x0400048D RID: 1165
		WMIQ_RPN_OP_UNDEFINED = 0,
		// Token: 0x0400048E RID: 1166
		WMIQ_RPN_OP_EQ,
		// Token: 0x0400048F RID: 1167
		WMIQ_RPN_OP_NE,
		// Token: 0x04000490 RID: 1168
		WMIQ_RPN_OP_GE,
		// Token: 0x04000491 RID: 1169
		WMIQ_RPN_OP_LE,
		// Token: 0x04000492 RID: 1170
		WMIQ_RPN_OP_LT,
		// Token: 0x04000493 RID: 1171
		WMIQ_RPN_OP_GT,
		// Token: 0x04000494 RID: 1172
		WMIQ_RPN_OP_LIKE,
		// Token: 0x04000495 RID: 1173
		WMIQ_RPN_OP_ISA,
		// Token: 0x04000496 RID: 1174
		WMIQ_RPN_OP_ISNOTA,
		// Token: 0x04000497 RID: 1175
		WMIQ_RPN_LEFT_PROPERTY_NAME = 1,
		// Token: 0x04000498 RID: 1176
		WMIQ_RPN_RIGHT_PROPERTY_NAME,
		// Token: 0x04000499 RID: 1177
		WMIQ_RPN_CONST2 = 4,
		// Token: 0x0400049A RID: 1178
		WMIQ_RPN_CONST = 8,
		// Token: 0x0400049B RID: 1179
		WMIQ_RPN_RELOP = 16,
		// Token: 0x0400049C RID: 1180
		WMIQ_RPN_LEFT_FUNCTION = 32,
		// Token: 0x0400049D RID: 1181
		WMIQ_RPN_RIGHT_FUNCTION = 64,
		// Token: 0x0400049E RID: 1182
		WMIQ_RPN_GET_TOKEN_TYPE = 1,
		// Token: 0x0400049F RID: 1183
		WMIQ_RPN_GET_EXPR_SHAPE,
		// Token: 0x040004A0 RID: 1184
		WMIQ_RPN_GET_LEFT_FUNCTION,
		// Token: 0x040004A1 RID: 1185
		WMIQ_RPN_GET_RIGHT_FUNCTION,
		// Token: 0x040004A2 RID: 1186
		WMIQ_RPN_GET_RELOP,
		// Token: 0x040004A3 RID: 1187
		WMIQ_RPN_NEXT_TOKEN = 1,
		// Token: 0x040004A4 RID: 1188
		WMIQ_RPN_FROM_UNARY = 1,
		// Token: 0x040004A5 RID: 1189
		WMIQ_RPN_FROM_PATH,
		// Token: 0x040004A6 RID: 1190
		WMIQ_RPN_FROM_CLASS_LIST = 4
	}
}
