using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000122 RID: 290
	internal class SoapParameter
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x000415FC File Offset: 0x000405FC
		internal bool IsOut
		{
			get
			{
				return (this.codeFlags & CodeFlags.IsOut) != (CodeFlags)0;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0004160D File Offset: 0x0004060D
		internal bool IsByRef
		{
			get
			{
				return (this.codeFlags & CodeFlags.IsByRef) != (CodeFlags)0;
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00041620 File Offset: 0x00040620
		internal static string[] GetTypeFullNames(IList parameters, int specifiedCount, CodeDomProvider codeProvider)
		{
			string[] array = new string[parameters.Count + specifiedCount];
			SoapParameter.GetTypeFullNames(parameters, array, 0, specifiedCount, codeProvider);
			return array;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00041648 File Offset: 0x00040648
		internal static void GetTypeFullNames(IList parameters, string[] typeFullNames, int start, int specifiedCount, CodeDomProvider codeProvider)
		{
			int num = 0;
			for (int i = 0; i < parameters.Count; i++)
			{
				typeFullNames[i + start + num] = WebCodeGenerator.FullTypeName(((SoapParameter)parameters[i]).mapping, codeProvider);
				if (((SoapParameter)parameters[i]).mapping.CheckSpecified)
				{
					num++;
					typeFullNames[i + start + num] = typeof(bool).FullName;
				}
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000416BC File Offset: 0x000406BC
		internal static string[] GetNames(IList parameters, int specifiedCount)
		{
			string[] array = new string[parameters.Count + specifiedCount];
			SoapParameter.GetNames(parameters, array, 0, specifiedCount);
			return array;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x000416E4 File Offset: 0x000406E4
		internal static void GetNames(IList parameters, string[] names, int start, int specifiedCount)
		{
			int num = 0;
			for (int i = 0; i < parameters.Count; i++)
			{
				names[i + start + num] = ((SoapParameter)parameters[i]).name;
				if (((SoapParameter)parameters[i]).mapping.CheckSpecified)
				{
					num++;
					names[i + start + num] = ((SoapParameter)parameters[i]).specifiedName;
				}
			}
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00041750 File Offset: 0x00040750
		internal static CodeFlags[] GetCodeFlags(IList parameters, int specifiedCount)
		{
			CodeFlags[] array = new CodeFlags[parameters.Count + specifiedCount];
			SoapParameter.GetCodeFlags(parameters, array, 0, specifiedCount);
			return array;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00041778 File Offset: 0x00040778
		internal static void GetCodeFlags(IList parameters, CodeFlags[] codeFlags, int start, int specifiedCount)
		{
			int num = 0;
			for (int i = 0; i < parameters.Count; i++)
			{
				codeFlags[i + start + num] = ((SoapParameter)parameters[i]).codeFlags;
				if (((SoapParameter)parameters[i]).mapping.CheckSpecified)
				{
					num++;
					codeFlags[i + start + num] = ((SoapParameter)parameters[i]).codeFlags;
				}
			}
		}

		// Token: 0x040005CE RID: 1486
		internal CodeFlags codeFlags;

		// Token: 0x040005CF RID: 1487
		internal string name;

		// Token: 0x040005D0 RID: 1488
		internal XmlMemberMapping mapping;

		// Token: 0x040005D1 RID: 1489
		internal string specifiedName;
	}
}
