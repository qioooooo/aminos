using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200023C RID: 572
	internal static class ThrowHelper
	{
		// Token: 0x06001372 RID: 4978 RVA: 0x00041278 File Offset: 0x00040278
		internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType)
		{
			throw new ArgumentException(SR.GetString("Arg_WrongType", new object[] { key, targetType }), "key");
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x000412AC File Offset: 0x000402AC
		internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType)
		{
			throw new ArgumentException(SR.GetString("Arg_WrongType", new object[] { value, targetType }), "value");
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x000412DD File Offset: 0x000402DD
		internal static void ThrowKeyNotFoundException()
		{
			throw new KeyNotFoundException();
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x000412E4 File Offset: 0x000402E4
		internal static void ThrowArgumentException(ExceptionResource resource)
		{
			throw new ArgumentException(SR.GetString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x000412F6 File Offset: 0x000402F6
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw new ArgumentNullException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00041303 File Offset: 0x00040303
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00041310 File Offset: 0x00040310
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
		{
			throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), SR.GetString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00041328 File Offset: 0x00040328
		internal static void ThrowInvalidOperationException(ExceptionResource resource)
		{
			throw new InvalidOperationException(SR.GetString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0004133A File Offset: 0x0004033A
		internal static void ThrowSerializationException(ExceptionResource resource)
		{
			throw new SerializationException(SR.GetString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0004134C File Offset: 0x0004034C
		internal static void ThrowNotSupportedException(ExceptionResource resource)
		{
			throw new NotSupportedException(SR.GetString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00041360 File Offset: 0x00040360
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
			default:
				return string.Empty;
			}
			return text;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00041444 File Offset: 0x00040444
		internal static string GetResourceName(ExceptionResource resource)
		{
			switch (resource)
			{
			case ExceptionResource.Argument_ImplementIComparable:
				return "Argument_ImplementIComparable";
			case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
				return "ArgumentOutOfRange_NeedNonNegNum";
			case ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired:
				return "ArgumentOutOfRange_NeedNonNegNumRequired";
			case ExceptionResource.Arg_ArrayPlusOffTooSmall:
				return "Arg_ArrayPlusOffTooSmall";
			case ExceptionResource.Argument_AddingDuplicate:
				return "Argument_AddingDuplicate";
			case ExceptionResource.Serialization_InvalidOnDeser:
				return "Serialization_InvalidOnDeser";
			case ExceptionResource.Serialization_MismatchedCount:
				return "Serialization_MismatchedCount";
			case ExceptionResource.Serialization_MissingValues:
				return "Serialization_MissingValues";
			case ExceptionResource.Arg_RankMultiDimNotSupported:
				return "Arg_MultiRank";
			case ExceptionResource.Arg_NonZeroLowerBound:
				return "Arg_NonZeroLowerBound";
			case ExceptionResource.Argument_InvalidArrayType:
				return "Invalid_Array_Type";
			case ExceptionResource.NotSupported_KeyCollectionSet:
				return "NotSupported_KeyCollectionSet";
			case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
				return "ArgumentOutOfRange_SmallCapacity";
			case ExceptionResource.ArgumentOutOfRange_Index:
				return "ArgumentOutOfRange_Index";
			case ExceptionResource.Argument_InvalidOffLen:
				return "Argument_InvalidOffLen";
			case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
				return "InvalidOperation_CannotRemoveFromStackOrQueue";
			case ExceptionResource.InvalidOperation_EmptyCollection:
				return "InvalidOperation_EmptyCollection";
			case ExceptionResource.InvalidOperation_EmptyQueue:
				return "InvalidOperation_EmptyQueue";
			case ExceptionResource.InvalidOperation_EnumOpCantHappen:
				return "InvalidOperation_EnumOpCantHappen";
			case ExceptionResource.InvalidOperation_EnumFailedVersion:
				return "InvalidOperation_EnumFailedVersion";
			case ExceptionResource.InvalidOperation_EmptyStack:
				return "InvalidOperation_EmptyStack";
			case ExceptionResource.InvalidOperation_EnumNotStarted:
				return "InvalidOperation_EnumNotStarted";
			case ExceptionResource.InvalidOperation_EnumEnded:
				return "InvalidOperation_EnumEnded";
			case ExceptionResource.NotSupported_SortedListNestedWrite:
				return "NotSupported_SortedListNestedWrite";
			case ExceptionResource.NotSupported_ValueCollectionSet:
				return "NotSupported_ValueCollectionSet";
			}
			return string.Empty;
		}
	}
}
