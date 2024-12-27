using System;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000011 RID: 17
	internal class ValueFixup : ITrace
	{
		// Token: 0x06000066 RID: 102 RVA: 0x000056DE File Offset: 0x000046DE
		internal ValueFixup(Array arrayObj, int[] indexMap)
		{
			this.valueFixupEnum = ValueFixupEnum.Array;
			this.arrayObj = arrayObj;
			this.indexMap = indexMap;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000056FB File Offset: 0x000046FB
		internal ValueFixup(object memberObject, string memberName, ReadObjectInfo objectInfo)
		{
			this.valueFixupEnum = ValueFixupEnum.Member;
			this.memberObject = memberObject;
			this.memberName = memberName;
			this.objectInfo = objectInfo;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00005720 File Offset: 0x00004720
		internal virtual void Fixup(ParseRecord record, ParseRecord parent)
		{
			object prnewObj = record.PRnewObj;
			switch (this.valueFixupEnum)
			{
			case ValueFixupEnum.Array:
				this.arrayObj.SetValue(prnewObj, this.indexMap);
				return;
			case ValueFixupEnum.Header:
				break;
			case ValueFixupEnum.Member:
			{
				if (this.objectInfo.isSi)
				{
					this.objectInfo.objectManager.RecordDelayedFixup(parent.PRobjectId, this.memberName, record.PRobjectId);
					return;
				}
				MemberInfo memberInfo = this.objectInfo.GetMemberInfo(this.memberName);
				this.objectInfo.objectManager.RecordFixup(parent.PRobjectId, memberInfo, record.PRobjectId);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000057C2 File Offset: 0x000047C2
		public virtual string Trace()
		{
			return "ValueFixup" + this.valueFixupEnum.ToString();
		}

		// Token: 0x04000082 RID: 130
		internal ValueFixupEnum valueFixupEnum;

		// Token: 0x04000083 RID: 131
		internal Array arrayObj;

		// Token: 0x04000084 RID: 132
		internal int[] indexMap;

		// Token: 0x04000085 RID: 133
		internal object memberObject;

		// Token: 0x04000086 RID: 134
		internal ReadObjectInfo objectInfo;

		// Token: 0x04000087 RID: 135
		internal string memberName;
	}
}
