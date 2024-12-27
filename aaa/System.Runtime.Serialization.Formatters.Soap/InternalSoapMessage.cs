using System;
using System.Collections;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	internal sealed class InternalSoapMessage : ISerializable, IFieldInfo
	{
		// Token: 0x0600006B RID: 107 RVA: 0x000057E6 File Offset: 0x000047E6
		internal InternalSoapMessage()
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000057EE File Offset: 0x000047EE
		internal InternalSoapMessage(string methodName, string xmlNameSpace, string[] paramNames, object[] paramValues, Type[] paramTypes)
		{
			this.methodName = methodName;
			this.xmlNameSpace = xmlNameSpace;
			this.paramNames = paramNames;
			this.paramValues = paramValues;
			this.paramTypes = paramTypes;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000581B File Offset: 0x0000481B
		internal InternalSoapMessage(SerializationInfo info, StreamingContext context)
		{
			this.SetObjectData(info, context);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000582B File Offset: 0x0000482B
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00005833 File Offset: 0x00004833
		public string[] FieldNames
		{
			get
			{
				return this.paramNames;
			}
			set
			{
				this.paramNames = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000583C File Offset: 0x0000483C
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00005844 File Offset: 0x00004844
		public Type[] FieldTypes
		{
			get
			{
				return this.paramTypes;
			}
			set
			{
				this.paramTypes = value;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005850 File Offset: 0x00004850
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (this.paramValues != null)
			{
				int num = this.paramValues.Length;
			}
			info.FullTypeName = this.methodName;
			if (this.xmlNameSpace != null)
			{
				info.AssemblyName = this.xmlNameSpace;
			}
			if (this.paramValues != null)
			{
				for (int i = 0; i < this.paramValues.Length; i++)
				{
					string text;
					if (this.paramNames != null && this.paramNames[i] == null)
					{
						text = "param" + i;
					}
					else
					{
						text = this.paramNames[i];
					}
					info.AddValue(text, this.paramValues[i], typeof(object));
				}
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000058F4 File Offset: 0x000048F4
		internal void SetObjectData(SerializationInfo info, StreamingContext context)
		{
			ArrayList arrayList = new ArrayList(20);
			this.methodName = info.GetString("__methodName");
			this.keyToNamespaceTable = (Hashtable)info.GetValue("__keyToNamespaceTable", typeof(Hashtable));
			ArrayList arrayList2 = (ArrayList)info.GetValue("__paramNameList", typeof(ArrayList));
			this.xmlNameSpace = info.GetString("__xmlNameSpace");
			for (int i = 0; i < arrayList2.Count; i++)
			{
				arrayList.Add(info.GetValue((string)arrayList2[i], Converter.typeofObject));
			}
			this.paramNames = new string[arrayList2.Count];
			this.paramValues = new object[arrayList.Count];
			for (int j = 0; j < arrayList2.Count; j++)
			{
				this.paramNames[j] = (string)arrayList2[j];
				this.paramValues[j] = arrayList[j];
			}
		}

		// Token: 0x0400008D RID: 141
		internal string methodName;

		// Token: 0x0400008E RID: 142
		internal string xmlNameSpace;

		// Token: 0x0400008F RID: 143
		internal string[] paramNames;

		// Token: 0x04000090 RID: 144
		internal object[] paramValues;

		// Token: 0x04000091 RID: 145
		internal Type[] paramTypes;

		// Token: 0x04000092 RID: 146
		internal Hashtable keyToNamespaceTable;
	}
}
