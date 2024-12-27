using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001EB RID: 491
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class CodeGeneratorOptions
	{
		// Token: 0x1700033A RID: 826
		public object this[string index]
		{
			get
			{
				return this.options[index];
			}
			set
			{
				this.options[index] = value;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00035754 File Offset: 0x00034754
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x00035781 File Offset: 0x00034781
		public string IndentString
		{
			get
			{
				object obj = this.options["IndentString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "    ";
			}
			set
			{
				this.options["IndentString"] = value;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00035794 File Offset: 0x00034794
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x000357C1 File Offset: 0x000347C1
		public string BracingStyle
		{
			get
			{
				object obj = this.options["BracingStyle"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "Block";
			}
			set
			{
				this.options["BracingStyle"] = value;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x000357D4 File Offset: 0x000347D4
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x000357FD File Offset: 0x000347FD
		public bool ElseOnClosing
		{
			get
			{
				object obj = this.options["ElseOnClosing"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.options["ElseOnClosing"] = value;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x00035818 File Offset: 0x00034818
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x00035841 File Offset: 0x00034841
		public bool BlankLinesBetweenMembers
		{
			get
			{
				object obj = this.options["BlankLinesBetweenMembers"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.options["BlankLinesBetweenMembers"] = value;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x0003585C File Offset: 0x0003485C
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x00035885 File Offset: 0x00034885
		[ComVisible(false)]
		public bool VerbatimOrder
		{
			get
			{
				object obj = this.options["VerbatimOrder"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.options["VerbatimOrder"] = value;
			}
		}

		// Token: 0x04000F5A RID: 3930
		private IDictionary options = new ListDictionary();
	}
}
