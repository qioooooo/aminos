using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000735 RID: 1845
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class SoapFieldAttribute : SoapAttribute
	{
		// Token: 0x06004260 RID: 16992 RVA: 0x000E2673 File Offset: 0x000E1673
		public bool IsInteropXmlElement()
		{
			return (this._explicitlySet & SoapFieldAttribute.ExplicitlySet.XmlElementName) != SoapFieldAttribute.ExplicitlySet.None;
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06004261 RID: 16993 RVA: 0x000E2683 File Offset: 0x000E1683
		// (set) Token: 0x06004262 RID: 16994 RVA: 0x000E26B1 File Offset: 0x000E16B1
		public string XmlElementName
		{
			get
			{
				if (this._xmlElementName == null && this.ReflectInfo != null)
				{
					this._xmlElementName = ((FieldInfo)this.ReflectInfo).Name;
				}
				return this._xmlElementName;
			}
			set
			{
				this._xmlElementName = value;
				this._explicitlySet |= SoapFieldAttribute.ExplicitlySet.XmlElementName;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06004263 RID: 16995 RVA: 0x000E26C8 File Offset: 0x000E16C8
		// (set) Token: 0x06004264 RID: 16996 RVA: 0x000E26D0 File Offset: 0x000E16D0
		public int Order
		{
			get
			{
				return this._order;
			}
			set
			{
				this._order = value;
			}
		}

		// Token: 0x04002131 RID: 8497
		private SoapFieldAttribute.ExplicitlySet _explicitlySet;

		// Token: 0x04002132 RID: 8498
		private string _xmlElementName;

		// Token: 0x04002133 RID: 8499
		private int _order;

		// Token: 0x02000736 RID: 1846
		[Flags]
		[Serializable]
		private enum ExplicitlySet
		{
			// Token: 0x04002135 RID: 8501
			None = 0,
			// Token: 0x04002136 RID: 8502
			XmlElementName = 1
		}
	}
}
