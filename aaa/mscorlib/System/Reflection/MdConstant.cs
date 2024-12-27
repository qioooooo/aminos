using System;

namespace System.Reflection
{
	// Token: 0x0200033C RID: 828
	internal static class MdConstant
	{
		// Token: 0x060020EB RID: 8427 RVA: 0x0005259C File Offset: 0x0005159C
		public unsafe static object GetValue(MetadataImport scope, int token, RuntimeTypeHandle fieldTypeHandle, bool raw)
		{
			CorElementType corElementType = CorElementType.End;
			long num = 0L;
			int num2;
			scope.GetDefaultValue(token, out num, out num2, out corElementType);
			Type typeFromHandle = Type.GetTypeFromHandle(fieldTypeHandle);
			if (typeFromHandle.IsEnum && !raw)
			{
				long num3;
				switch (corElementType)
				{
				case CorElementType.Void:
					return DBNull.Value;
				case CorElementType.Char:
					num3 = (long)((ulong)(*(ushort*)(&num)));
					goto IL_00C8;
				case CorElementType.I1:
					num3 = (long)(*(sbyte*)(&num));
					goto IL_00C8;
				case CorElementType.U1:
					num3 = (long)((ulong)(*(byte*)(&num)));
					goto IL_00C8;
				case CorElementType.I2:
					num3 = (long)(*(short*)(&num));
					goto IL_00C8;
				case CorElementType.U2:
					num3 = (long)((ulong)(*(ushort*)(&num)));
					goto IL_00C8;
				case CorElementType.I4:
					num3 = (long)(*(int*)(&num));
					goto IL_00C8;
				case CorElementType.U4:
					num3 = (long)((ulong)(*(uint*)(&num)));
					goto IL_00C8;
				case CorElementType.I8:
					num3 = num;
					goto IL_00C8;
				case CorElementType.U8:
					num3 = num;
					goto IL_00C8;
				}
				throw new FormatException(Environment.GetResourceString("Arg_BadLiteralFormat"));
				IL_00C8:
				return RuntimeType.CreateEnum(fieldTypeHandle, num3);
			}
			if (typeFromHandle != typeof(DateTime))
			{
				switch (corElementType)
				{
				case CorElementType.Void:
					return DBNull.Value;
				case CorElementType.Boolean:
					return *(byte*)(&num) != 0;
				case CorElementType.Char:
					return (char)(*(ushort*)(&num));
				case CorElementType.I1:
					return *(sbyte*)(&num);
				case CorElementType.U1:
					return *(byte*)(&num);
				case CorElementType.I2:
					return *(short*)(&num);
				case CorElementType.U2:
					return *(ushort*)(&num);
				case CorElementType.I4:
					return *(int*)(&num);
				case CorElementType.U4:
					return *(uint*)(&num);
				case CorElementType.I8:
					return num;
				case CorElementType.U8:
					return (ulong)num;
				case CorElementType.R4:
					return *(float*)(&num);
				case CorElementType.R8:
					return *(double*)(&num);
				case CorElementType.String:
					return new string(num, 0, num2 / 2);
				case CorElementType.Class:
					return null;
				}
				throw new FormatException(Environment.GetResourceString("Arg_BadLiteralFormat"));
			}
			CorElementType corElementType2 = corElementType;
			if (corElementType2 != CorElementType.Void)
			{
				long num4;
				switch (corElementType2)
				{
				case CorElementType.I8:
					num4 = num;
					break;
				case CorElementType.U8:
					num4 = num;
					break;
				default:
					throw new FormatException(Environment.GetResourceString("Arg_BadLiteralFormat"));
				}
				return new DateTime(num4);
			}
			return DBNull.Value;
		}
	}
}
