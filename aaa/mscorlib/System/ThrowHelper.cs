using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x0200001C RID: 28
	internal static class ThrowHelper
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00004C4C File Offset: 0x00003C4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException()
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004C58 File Offset: 0x00003C58
		internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType)
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_WrongType", new object[] { key, targetType }), "key");
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004C8C File Offset: 0x00003C8C
		internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType)
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_WrongType", new object[] { value, targetType }), "value");
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004CBD File Offset: 0x00003CBD
		internal static void ThrowKeyNotFoundException()
		{
			throw new KeyNotFoundException();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004CC4 File Offset: 0x00003CC4
		internal static void ThrowArgumentException(ExceptionResource resource)
		{
			throw new ArgumentException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004CD6 File Offset: 0x00003CD6
		internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument)
		{
			throw new ArgumentException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)), ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004CEE File Offset: 0x00003CEE
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw new ArgumentNullException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004CFB File Offset: 0x00003CFB
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004D08 File Offset: 0x00003D08
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
		{
			throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004D20 File Offset: 0x00003D20
		internal static void ThrowInvalidOperationException(ExceptionResource resource)
		{
			throw new InvalidOperationException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004D32 File Offset: 0x00003D32
		internal static void ThrowSerializationException(ExceptionResource resource)
		{
			throw new SerializationException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004D44 File Offset: 0x00003D44
		internal static void ThrowSecurityException(ExceptionResource resource)
		{
			throw new SecurityException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004D56 File Offset: 0x00003D56
		internal static void ThrowNotSupportedException(ExceptionResource resource)
		{
			throw new NotSupportedException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004D68 File Offset: 0x00003D68
		internal static void ThrowUnauthorizedAccessException(ExceptionResource resource)
		{
			throw new UnauthorizedAccessException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004D7A File Offset: 0x00003D7A
		internal static void ThrowObjectDisposedException(string objectName, ExceptionResource resource)
		{
			throw new ObjectDisposedException(objectName, Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004D90 File Offset: 0x00003D90
		internal static string GetArgumentName(ExceptionArgument argument)
		{
			string text;
			switch (argument)
			{
			case ExceptionArgument.obj:
				text = "obj";
				break;
			case ExceptionArgument.dictionary:
				text = "dictionary";
				break;
			case ExceptionArgument.dictionaryCreationThreshold:
				text = "dictionaryCreationThreshold";
				break;
			case ExceptionArgument.array:
				text = "array";
				break;
			case ExceptionArgument.info:
				text = "info";
				break;
			case ExceptionArgument.key:
				text = "key";
				break;
			case ExceptionArgument.collection:
				text = "collection";
				break;
			case ExceptionArgument.list:
				text = "list";
				break;
			case ExceptionArgument.match:
				text = "match";
				break;
			case ExceptionArgument.converter:
				text = "converter";
				break;
			case ExceptionArgument.queue:
				text = "queue";
				break;
			case ExceptionArgument.stack:
				text = "stack";
				break;
			case ExceptionArgument.capacity:
				text = "capacity";
				break;
			case ExceptionArgument.index:
				text = "index";
				break;
			case ExceptionArgument.startIndex:
				text = "startIndex";
				break;
			case ExceptionArgument.value:
				text = "value";
				break;
			case ExceptionArgument.count:
				text = "count";
				break;
			case ExceptionArgument.arrayIndex:
				text = "arrayIndex";
				break;
			case ExceptionArgument.name:
				text = "name";
				break;
			case ExceptionArgument.mode:
				text = "mode";
				break;
			default:
				return string.Empty;
			}
			return text;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004EB0 File Offset: 0x00003EB0
		internal static string GetResourceName(ExceptionResource resource)
		{
			string text;
			switch (resource)
			{
			case ExceptionResource.Argument_ImplementIComparable:
				text = "Argument_ImplementIComparable";
				break;
			case ExceptionResource.Argument_InvalidType:
				text = "Argument_InvalidType";
				break;
			case ExceptionResource.Argument_InvalidArgumentForComparison:
				text = "Argument_InvalidArgumentForComparison";
				break;
			case ExceptionResource.Argument_InvalidRegistryKeyPermissionCheck:
				text = "Argument_InvalidRegistryKeyPermissionCheck";
				break;
			case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
				text = "ArgumentOutOfRange_NeedNonNegNum";
				break;
			case ExceptionResource.Arg_ArrayPlusOffTooSmall:
				text = "Arg_ArrayPlusOffTooSmall";
				break;
			case ExceptionResource.Arg_NonZeroLowerBound:
				text = "Arg_NonZeroLowerBound";
				break;
			case ExceptionResource.Arg_RankMultiDimNotSupported:
				text = "Arg_RankMultiDimNotSupported";
				break;
			case ExceptionResource.Arg_RegKeyDelHive:
				text = "Arg_RegKeyDelHive";
				break;
			case ExceptionResource.Arg_RegKeyStrLenBug:
				text = "Arg_RegKeyStrLenBug";
				break;
			case ExceptionResource.Arg_RegSetStrArrNull:
				text = "Arg_RegSetStrArrNull";
				break;
			case ExceptionResource.Arg_RegSetMismatchedKind:
				text = "Arg_RegSetMismatchedKind";
				break;
			case ExceptionResource.Arg_RegSubKeyAbsent:
				text = "Arg_RegSubKeyAbsent";
				break;
			case ExceptionResource.Arg_RegSubKeyValueAbsent:
				text = "Arg_RegSubKeyValueAbsent";
				break;
			case ExceptionResource.Argument_AddingDuplicate:
				text = "Argument_AddingDuplicate";
				break;
			case ExceptionResource.Serialization_InvalidOnDeser:
				text = "Serialization_InvalidOnDeser";
				break;
			case ExceptionResource.Serialization_MissingKeyValuePairs:
				text = "Serialization_MissingKeyValuePairs";
				break;
			case ExceptionResource.Serialization_NullKey:
				text = "Serialization_NullKey";
				break;
			case ExceptionResource.Argument_InvalidArrayType:
				text = "Argument_InvalidArrayType";
				break;
			case ExceptionResource.NotSupported_KeyCollectionSet:
				text = "NotSupported_KeyCollectionSet";
				break;
			case ExceptionResource.NotSupported_ValueCollectionSet:
				text = "NotSupported_ValueCollectionSet";
				break;
			case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
				text = "ArgumentOutOfRange_SmallCapacity";
				break;
			case ExceptionResource.ArgumentOutOfRange_Index:
				text = "ArgumentOutOfRange_Index";
				break;
			case ExceptionResource.Argument_InvalidOffLen:
				text = "Argument_InvalidOffLen";
				break;
			case ExceptionResource.Argument_ItemNotExist:
				text = "Argument_ItemNotExist";
				break;
			case ExceptionResource.ArgumentOutOfRange_Count:
				text = "ArgumentOutOfRange_Count";
				break;
			case ExceptionResource.ArgumentOutOfRange_InvalidThreshold:
				text = "ArgumentOutOfRange_InvalidThreshold";
				break;
			case ExceptionResource.ArgumentOutOfRange_ListInsert:
				text = "ArgumentOutOfRange_ListInsert";
				break;
			case ExceptionResource.NotSupported_ReadOnlyCollection:
				text = "NotSupported_ReadOnlyCollection";
				break;
			case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
				text = "InvalidOperation_CannotRemoveFromStackOrQueue";
				break;
			case ExceptionResource.InvalidOperation_EmptyQueue:
				text = "InvalidOperation_EmptyQueue";
				break;
			case ExceptionResource.InvalidOperation_EnumOpCantHappen:
				text = "InvalidOperation_EnumOpCantHappen";
				break;
			case ExceptionResource.InvalidOperation_EnumFailedVersion:
				text = "InvalidOperation_EnumFailedVersion";
				break;
			case ExceptionResource.InvalidOperation_EmptyStack:
				text = "InvalidOperation_EmptyStack";
				break;
			case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
				text = "ArgumentOutOfRange_BiggerThanCollection";
				break;
			case ExceptionResource.InvalidOperation_EnumNotStarted:
				text = "InvalidOperation_EnumNotStarted";
				break;
			case ExceptionResource.InvalidOperation_EnumEnded:
				text = "InvalidOperation_EnumEnded";
				break;
			case ExceptionResource.NotSupported_SortedListNestedWrite:
				text = "NotSupported_SortedListNestedWrite";
				break;
			case ExceptionResource.InvalidOperation_NoValue:
				text = "InvalidOperation_NoValue";
				break;
			case ExceptionResource.InvalidOperation_RegRemoveSubKey:
				text = "InvalidOperation_RegRemoveSubKey";
				break;
			case ExceptionResource.Security_RegistryPermission:
				text = "Security_RegistryPermission";
				break;
			case ExceptionResource.UnauthorizedAccess_RegistryNoWrite:
				text = "UnauthorizedAccess_RegistryNoWrite";
				break;
			case ExceptionResource.ObjectDisposed_RegKeyClosed:
				text = "ObjectDisposed_RegKeyClosed";
				break;
			case ExceptionResource.NotSupported_InComparableType:
				text = "NotSupported_InComparableType";
				break;
			default:
				return string.Empty;
			}
			return text;
		}
	}
}
