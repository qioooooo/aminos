using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007DA RID: 2010
	internal sealed class ValueFixup
	{
		// Token: 0x0600476E RID: 18286 RVA: 0x000F5BE3 File Offset: 0x000F4BE3
		internal ValueFixup(Array arrayObj, int[] indexMap)
		{
			this.valueFixupEnum = ValueFixupEnum.Array;
			this.arrayObj = arrayObj;
			this.indexMap = indexMap;
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x000F5C00 File Offset: 0x000F4C00
		internal ValueFixup(object memberObject, string memberName, ReadObjectInfo objectInfo)
		{
			this.valueFixupEnum = ValueFixupEnum.Member;
			this.memberObject = memberObject;
			this.memberName = memberName;
			this.objectInfo = objectInfo;
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x000F5C24 File Offset: 0x000F4C24
		internal void Fixup(ParseRecord record, ParseRecord parent)
		{
			object prnewObj = record.PRnewObj;
			switch (this.valueFixupEnum)
			{
			case ValueFixupEnum.Array:
				this.arrayObj.SetValue(prnewObj, this.indexMap);
				return;
			case ValueFixupEnum.Header:
			{
				Type typeFromHandle = typeof(Header);
				if (ValueFixup.valueInfo == null)
				{
					MemberInfo[] member = typeFromHandle.GetMember("Value");
					if (member.Length != 1)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_HeaderReflection"), new object[] { member.Length }));
					}
					ValueFixup.valueInfo = member[0];
				}
				FormatterServices.SerializationSetValue(ValueFixup.valueInfo, this.header, prnewObj);
				return;
			}
			case ValueFixupEnum.Member:
			{
				if (this.objectInfo.isSi)
				{
					this.objectInfo.objectManager.RecordDelayedFixup(parent.PRobjectId, this.memberName, record.PRobjectId);
					return;
				}
				MemberInfo memberInfo = this.objectInfo.GetMemberInfo(this.memberName);
				if (memberInfo != null)
				{
					this.objectInfo.objectManager.RecordFixup(parent.PRobjectId, memberInfo, record.PRobjectId);
				}
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x0400243E RID: 9278
		internal ValueFixupEnum valueFixupEnum;

		// Token: 0x0400243F RID: 9279
		internal Array arrayObj;

		// Token: 0x04002440 RID: 9280
		internal int[] indexMap;

		// Token: 0x04002441 RID: 9281
		internal object header;

		// Token: 0x04002442 RID: 9282
		internal object memberObject;

		// Token: 0x04002443 RID: 9283
		internal static MemberInfo valueInfo;

		// Token: 0x04002444 RID: 9284
		internal ReadObjectInfo objectInfo;

		// Token: 0x04002445 RID: 9285
		internal string memberName;
	}
}
