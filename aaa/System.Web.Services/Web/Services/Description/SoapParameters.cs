using System;
using System.Collections;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000121 RID: 289
	internal class SoapParameters
	{
		// Token: 0x060008CB RID: 2251 RVA: 0x0004119C File Offset: 0x0004019C
		internal SoapParameters(XmlMembersMapping request, XmlMembersMapping response, string[] parameterOrder, CodeIdentifiers identifiers)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			SoapParameters.AddMappings(arrayList, request);
			if (response != null)
			{
				SoapParameters.AddMappings(arrayList2, response);
			}
			if (parameterOrder != null)
			{
				foreach (string text in parameterOrder)
				{
					XmlMemberMapping xmlMemberMapping = SoapParameters.FindMapping(arrayList, text);
					SoapParameter soapParameter = new SoapParameter();
					if (xmlMemberMapping != null)
					{
						if (SoapParameters.RemoveByRefMapping(arrayList2, xmlMemberMapping))
						{
							soapParameter.codeFlags = CodeFlags.IsByRef;
						}
						soapParameter.mapping = xmlMemberMapping;
						arrayList.Remove(xmlMemberMapping);
						this.AddParameter(soapParameter);
					}
					else
					{
						XmlMemberMapping xmlMemberMapping2 = SoapParameters.FindMapping(arrayList2, text);
						if (xmlMemberMapping2 != null)
						{
							soapParameter.codeFlags = CodeFlags.IsOut;
							soapParameter.mapping = xmlMemberMapping2;
							arrayList2.Remove(xmlMemberMapping2);
							this.AddParameter(soapParameter);
						}
					}
				}
			}
			foreach (object obj in arrayList)
			{
				XmlMemberMapping xmlMemberMapping3 = (XmlMemberMapping)obj;
				SoapParameter soapParameter2 = new SoapParameter();
				if (SoapParameters.RemoveByRefMapping(arrayList2, xmlMemberMapping3))
				{
					soapParameter2.codeFlags = CodeFlags.IsByRef;
				}
				soapParameter2.mapping = xmlMemberMapping3;
				this.AddParameter(soapParameter2);
			}
			if (arrayList2.Count > 0)
			{
				if (!((XmlMemberMapping)arrayList2[0]).CheckSpecified)
				{
					this.ret = (XmlMemberMapping)arrayList2[0];
					arrayList2.RemoveAt(0);
				}
				foreach (object obj2 in arrayList2)
				{
					XmlMemberMapping xmlMemberMapping4 = (XmlMemberMapping)obj2;
					this.AddParameter(new SoapParameter
					{
						mapping = xmlMemberMapping4,
						codeFlags = CodeFlags.IsOut
					});
				}
			}
			foreach (object obj3 in this.parameters)
			{
				SoapParameter soapParameter3 = (SoapParameter)obj3;
				soapParameter3.name = identifiers.MakeUnique(CodeIdentifier.MakeValid(soapParameter3.mapping.MemberName));
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000413F0 File Offset: 0x000403F0
		private void AddParameter(SoapParameter parameter)
		{
			this.parameters.Add(parameter);
			if (parameter.mapping.CheckSpecified)
			{
				this.checkSpecifiedCount++;
			}
			if (parameter.IsByRef)
			{
				this.inParameters.Add(parameter);
				this.outParameters.Add(parameter);
				if (parameter.mapping.CheckSpecified)
				{
					this.inCheckSpecifiedCount++;
					this.outCheckSpecifiedCount++;
					return;
				}
			}
			else if (parameter.IsOut)
			{
				this.outParameters.Add(parameter);
				if (parameter.mapping.CheckSpecified)
				{
					this.outCheckSpecifiedCount++;
					return;
				}
			}
			else
			{
				this.inParameters.Add(parameter);
				if (parameter.mapping.CheckSpecified)
				{
					this.inCheckSpecifiedCount++;
				}
			}
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x000414CC File Offset: 0x000404CC
		private static bool RemoveByRefMapping(ArrayList responseList, XmlMemberMapping requestMapping)
		{
			XmlMemberMapping xmlMemberMapping = SoapParameters.FindMapping(responseList, requestMapping.ElementName);
			if (xmlMemberMapping == null)
			{
				return false;
			}
			if (requestMapping.TypeFullName != xmlMemberMapping.TypeFullName)
			{
				return false;
			}
			if (requestMapping.Namespace != xmlMemberMapping.Namespace)
			{
				return false;
			}
			if (requestMapping.MemberName != xmlMemberMapping.MemberName)
			{
				return false;
			}
			responseList.Remove(xmlMemberMapping);
			return true;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00041534 File Offset: 0x00040534
		private static void AddMappings(ArrayList mappingsList, XmlMembersMapping mappings)
		{
			for (int i = 0; i < mappings.Count; i++)
			{
				mappingsList.Add(mappings[i]);
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00041560 File Offset: 0x00040560
		private static XmlMemberMapping FindMapping(ArrayList mappingsList, string elementName)
		{
			foreach (object obj in mappingsList)
			{
				XmlMemberMapping xmlMemberMapping = (XmlMemberMapping)obj;
				if (xmlMemberMapping.ElementName == elementName)
				{
					return xmlMemberMapping;
				}
			}
			return null;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x000415C4 File Offset: 0x000405C4
		internal XmlMemberMapping Return
		{
			get
			{
				return this.ret;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x000415CC File Offset: 0x000405CC
		internal IList Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x000415D4 File Offset: 0x000405D4
		internal IList InParameters
		{
			get
			{
				return this.inParameters;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x000415DC File Offset: 0x000405DC
		internal IList OutParameters
		{
			get
			{
				return this.outParameters;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x000415E4 File Offset: 0x000405E4
		internal int CheckSpecifiedCount
		{
			get
			{
				return this.checkSpecifiedCount;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x000415EC File Offset: 0x000405EC
		internal int InCheckSpecifiedCount
		{
			get
			{
				return this.inCheckSpecifiedCount;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x000415F4 File Offset: 0x000405F4
		internal int OutCheckSpecifiedCount
		{
			get
			{
				return this.outCheckSpecifiedCount;
			}
		}

		// Token: 0x040005C7 RID: 1479
		private XmlMemberMapping ret;

		// Token: 0x040005C8 RID: 1480
		private ArrayList parameters = new ArrayList();

		// Token: 0x040005C9 RID: 1481
		private ArrayList inParameters = new ArrayList();

		// Token: 0x040005CA RID: 1482
		private ArrayList outParameters = new ArrayList();

		// Token: 0x040005CB RID: 1483
		private int checkSpecifiedCount;

		// Token: 0x040005CC RID: 1484
		private int inCheckSpecifiedCount;

		// Token: 0x040005CD RID: 1485
		private int outCheckSpecifiedCount;
	}
}
