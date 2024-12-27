using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Web.Services.Configuration;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000BC RID: 188
	internal abstract class HttpProtocolImporter : ProtocolImporter
	{
		// Token: 0x06000508 RID: 1288 RVA: 0x00019064 File Offset: 0x00018064
		protected HttpProtocolImporter(bool hasInputPayload)
		{
			Type[] mimeImporterTypes = WebServicesSection.Current.MimeImporterTypes;
			this.importers = new MimeImporter[mimeImporterTypes.Length];
			this.importedParameters = new ArrayList[mimeImporterTypes.Length];
			this.importedReturns = new ArrayList[mimeImporterTypes.Length];
			for (int i = 0; i < this.importers.Length; i++)
			{
				MimeImporter mimeImporter = (MimeImporter)Activator.CreateInstance(mimeImporterTypes[i]);
				mimeImporter.ImportContext = this;
				this.importedParameters[i] = new ArrayList();
				this.importedReturns[i] = new ArrayList();
				this.importers[i] = mimeImporter;
			}
			this.hasInputPayload = hasInputPayload;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001910C File Offset: 0x0001810C
		private MimeParameterCollection ImportMimeParameters()
		{
			for (int i = 0; i < this.importers.Length; i++)
			{
				MimeParameterCollection mimeParameterCollection = this.importers[i].ImportParameters();
				if (mimeParameterCollection != null)
				{
					this.importedParameters[i].Add(mimeParameterCollection);
					return mimeParameterCollection;
				}
			}
			return null;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00019150 File Offset: 0x00018150
		private MimeReturn ImportMimeReturn()
		{
			if (base.OperationBinding.Output.Extensions.Count == 0)
			{
				return new MimeReturn
				{
					TypeName = typeof(void).FullName
				};
			}
			for (int i = 0; i < this.importers.Length; i++)
			{
				MimeReturn mimeReturn = this.importers[i].ImportReturn();
				if (mimeReturn != null)
				{
					this.importedReturns[i].Add(mimeReturn);
					return mimeReturn;
				}
			}
			return null;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000191C8 File Offset: 0x000181C8
		private MimeParameterCollection ImportUrlParameters()
		{
			if ((HttpUrlEncodedBinding)base.OperationBinding.Input.Extensions.Find(typeof(HttpUrlEncodedBinding)) == null)
			{
				return new MimeParameterCollection();
			}
			return this.ImportStringParametersMessage();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001920C File Offset: 0x0001820C
		internal MimeParameterCollection ImportStringParametersMessage()
		{
			MimeParameterCollection mimeParameterCollection = new MimeParameterCollection();
			foreach (object obj in base.InputMessage.Parts)
			{
				MessagePart messagePart = (MessagePart)obj;
				MimeParameter mimeParameter = this.ImportUrlParameter(messagePart);
				if (mimeParameter == null)
				{
					return null;
				}
				mimeParameterCollection.Add(mimeParameter);
			}
			return mimeParameterCollection;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001928C File Offset: 0x0001828C
		private MimeParameter ImportUrlParameter(MessagePart part)
		{
			return new MimeParameter
			{
				Name = CodeIdentifier.MakeValid(XmlConvert.DecodeName(part.Name)),
				TypeName = (this.IsRepeatingParameter(part) ? typeof(string[]).FullName : typeof(string).FullName)
			};
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000192E8 File Offset: 0x000182E8
		private bool IsRepeatingParameter(MessagePart part)
		{
			XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)base.Schemas.Find(part.Type, typeof(XmlSchemaComplexType));
			if (xmlSchemaComplexType == null)
			{
				return false;
			}
			if (xmlSchemaComplexType.ContentModel == null)
			{
				return false;
			}
			if (xmlSchemaComplexType.ContentModel.Content == null)
			{
				throw new ArgumentException(Res.GetString("Missing2", new object[]
				{
					xmlSchemaComplexType.Name,
					xmlSchemaComplexType.ContentModel.GetType().Name
				}), "part");
			}
			if (xmlSchemaComplexType.ContentModel.Content is XmlSchemaComplexContentExtension)
			{
				return ((XmlSchemaComplexContentExtension)xmlSchemaComplexType.ContentModel.Content).BaseTypeName == new XmlQualifiedName("Array", "http://schemas.xmlsoap.org/soap/encoding/");
			}
			return xmlSchemaComplexType.ContentModel.Content is XmlSchemaComplexContentRestriction && ((XmlSchemaComplexContentRestriction)xmlSchemaComplexType.ContentModel.Content).BaseTypeName == new XmlQualifiedName("Array", "http://schemas.xmlsoap.org/soap/encoding/");
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000193E4 File Offset: 0x000183E4
		private static void AppendMetadata(CodeAttributeDeclarationCollection from, CodeAttributeDeclarationCollection to)
		{
			foreach (object obj in from)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = (CodeAttributeDeclaration)obj;
				to.Add(codeAttributeDeclaration);
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001943C File Offset: 0x0001843C
		private CodeMemberMethod GenerateMethod(HttpMethodInfo method)
		{
			MimeParameterCollection mimeParameterCollection = ((method.MimeParameters != null) ? method.MimeParameters : method.UrlParameters);
			string[] array = new string[mimeParameterCollection.Count];
			string[] array2 = new string[mimeParameterCollection.Count];
			for (int i = 0; i < mimeParameterCollection.Count; i++)
			{
				MimeParameter mimeParameter = mimeParameterCollection[i];
				array2[i] = mimeParameter.Name;
				array[i] = mimeParameter.TypeName;
			}
			CodeAttributeDeclarationCollection codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection();
			CodeExpression[] array3 = new CodeExpression[2];
			if (method.MimeReturn.ReaderType == null)
			{
				array3[0] = new CodeTypeOfExpression(typeof(NopReturnReader).FullName);
			}
			else
			{
				array3[0] = new CodeTypeOfExpression(method.MimeReturn.ReaderType.FullName);
			}
			if (method.MimeParameters != null)
			{
				array3[1] = new CodeTypeOfExpression(method.MimeParameters.WriterType.FullName);
			}
			else
			{
				array3[1] = new CodeTypeOfExpression(typeof(UrlParameterWriter).FullName);
			}
			WebCodeGenerator.AddCustomAttribute(codeAttributeDeclarationCollection, typeof(HttpMethodAttribute), array3, new string[0], new CodeExpression[0]);
			CodeMemberMethod codeMemberMethod = WebCodeGenerator.AddMethod(base.CodeTypeDeclaration, method.Name, new CodeFlags[array.Length], array, array2, method.MimeReturn.TypeName, codeAttributeDeclarationCollection, CodeFlags.IsPublic | ((base.Style == ServiceDescriptionImportStyle.Client) ? ((CodeFlags)0) : CodeFlags.IsAbstract));
			HttpProtocolImporter.AppendMetadata(method.MimeReturn.Attributes, codeMemberMethod.ReturnTypeCustomAttributes);
			codeMemberMethod.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
			for (int j = 0; j < mimeParameterCollection.Count; j++)
			{
				HttpProtocolImporter.AppendMetadata(mimeParameterCollection[j].Attributes, codeMemberMethod.Parameters[j].CustomAttributes);
			}
			if (base.Style == ServiceDescriptionImportStyle.Client)
			{
				bool flag = (base.ServiceImporter.CodeGenerationOptions & CodeGenerationOptions.GenerateOldAsync) != CodeGenerationOptions.None;
				bool flag2 = (base.ServiceImporter.CodeGenerationOptions & CodeGenerationOptions.GenerateNewAsync) != CodeGenerationOptions.None && base.ServiceImporter.CodeGenerator.Supports(GeneratorSupport.DeclareEvents) && base.ServiceImporter.CodeGenerator.Supports(GeneratorSupport.DeclareDelegates);
				CodeExpression[] array4 = new CodeExpression[3];
				this.CreateInvokeParams(array4, method, array2);
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "Invoke", array4);
				if (method.MimeReturn.ReaderType != null)
				{
					codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(new CodeCastExpression(method.MimeReturn.TypeName, codeMethodInvokeExpression)));
				}
				else
				{
					codeMemberMethod.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
				}
				codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection();
				string[] array5 = new string[array.Length + 2];
				array.CopyTo(array5, 0);
				array5[array.Length] = typeof(AsyncCallback).FullName;
				array5[array.Length + 1] = typeof(object).FullName;
				string[] array6 = new string[array2.Length + 2];
				array2.CopyTo(array6, 0);
				array6[array2.Length] = "callback";
				array6[array2.Length + 1] = "asyncState";
				if (flag)
				{
					CodeMemberMethod codeMemberMethod2 = WebCodeGenerator.AddMethod(base.CodeTypeDeclaration, "Begin" + method.Name, new CodeFlags[array5.Length], array5, array6, typeof(IAsyncResult).FullName, codeAttributeDeclarationCollection, CodeFlags.IsPublic);
					codeMemberMethod2.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
					array4 = new CodeExpression[5];
					this.CreateInvokeParams(array4, method, array2);
					array4[3] = new CodeArgumentReferenceExpression("callback");
					array4[4] = new CodeArgumentReferenceExpression("asyncState");
					codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "BeginInvoke", array4);
					codeMemberMethod2.Statements.Add(new CodeMethodReturnStatement(codeMethodInvokeExpression));
					CodeMemberMethod codeMemberMethod3 = WebCodeGenerator.AddMethod(base.CodeTypeDeclaration, "End" + method.Name, new CodeFlags[1], new string[] { typeof(IAsyncResult).FullName }, new string[] { "asyncResult" }, method.MimeReturn.TypeName, codeAttributeDeclarationCollection, CodeFlags.IsPublic);
					codeMemberMethod3.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
					CodeExpression codeExpression = new CodeArgumentReferenceExpression("asyncResult");
					codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "EndInvoke", new CodeExpression[] { codeExpression });
					if (method.MimeReturn.ReaderType != null)
					{
						codeMemberMethod3.Statements.Add(new CodeMethodReturnStatement(new CodeCastExpression(method.MimeReturn.TypeName, codeMethodInvokeExpression)));
					}
					else
					{
						codeMemberMethod3.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
					}
				}
				if (flag2)
				{
					codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection();
					string name = method.Name;
					string text = ProtocolImporter.MethodSignature(name, method.MimeReturn.TypeName, new CodeFlags[array.Length], array);
					DelegateInfo delegateInfo = (DelegateInfo)base.ExportContext[text];
					if (delegateInfo == null)
					{
						string text2 = base.ClassNames.AddUnique(name + "CompletedEventHandler", name);
						string text3 = base.ClassNames.AddUnique(name + "CompletedEventArgs", name);
						delegateInfo = new DelegateInfo(text2, text3);
					}
					string text4 = base.MethodNames.AddUnique(name + "Completed", name);
					string text5 = base.MethodNames.AddUnique(name + "Async", name);
					string text6 = base.MethodNames.AddUnique(name + "OperationCompleted", name);
					string text7 = base.MethodNames.AddUnique("On" + name + "OperationCompleted", name);
					WebCodeGenerator.AddEvent(base.CodeTypeDeclaration.Members, delegateInfo.handlerType, text4);
					WebCodeGenerator.AddCallbackDeclaration(base.CodeTypeDeclaration.Members, text6);
					string text8 = ProtocolImporter.UniqueName("userState", array2);
					CodeMemberMethod codeMemberMethod4 = WebCodeGenerator.AddAsyncMethod(base.CodeTypeDeclaration, text5, array, array2, text6, text7, text8);
					array4 = new CodeExpression[5];
					this.CreateInvokeParams(array4, method, array2);
					array4[3] = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), text6);
					array4[4] = new CodeArgumentReferenceExpression(text8);
					codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "InvokeAsync", array4);
					codeMemberMethod4.Statements.Add(codeMethodInvokeExpression);
					bool flag3 = method.MimeReturn.ReaderType != null;
					WebCodeGenerator.AddCallbackImplementation(base.CodeTypeDeclaration, text7, text4, delegateInfo.handlerArgs, flag3);
					if (base.ExportContext[text] == null)
					{
						WebCodeGenerator.AddDelegate(base.ExtraCodeClasses, delegateInfo.handlerType, flag3 ? delegateInfo.handlerArgs : typeof(AsyncCompletedEventArgs).FullName);
						if (flag3)
						{
							base.ExtraCodeClasses.Add(WebCodeGenerator.CreateArgsClass(delegateInfo.handlerArgs, new string[] { method.MimeReturn.TypeName }, new string[] { "Result" }, base.ServiceImporter.CodeGenerator.Supports(GeneratorSupport.PartialTypes)));
						}
						base.ExportContext[text] = delegateInfo;
					}
				}
			}
			return codeMemberMethod;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00019B54 File Offset: 0x00018B54
		private void CreateInvokeParams(CodeExpression[] invokeParams, HttpMethodInfo method, string[] parameterNames)
		{
			invokeParams[0] = new CodePrimitiveExpression(method.Name);
			CodeExpression codeExpression = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Url");
			CodeExpression codeExpression2 = new CodePrimitiveExpression(method.Href);
			invokeParams[1] = new CodeBinaryOperatorExpression(codeExpression, CodeBinaryOperatorType.Add, codeExpression2);
			CodeExpression[] array = new CodeExpression[parameterNames.Length];
			for (int i = 0; i < parameterNames.Length; i++)
			{
				array[i] = new CodeArgumentReferenceExpression(parameterNames[i]);
			}
			invokeParams[2] = new CodeArrayCreateExpression(typeof(object).FullName, array);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00019BD0 File Offset: 0x00018BD0
		protected override bool IsOperationFlowSupported(OperationFlow flow)
		{
			return flow == OperationFlow.RequestResponse;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00019BD8 File Offset: 0x00018BD8
		protected override CodeMemberMethod GenerateMethod()
		{
			HttpOperationBinding httpOperationBinding = (HttpOperationBinding)base.OperationBinding.Extensions.Find(typeof(HttpOperationBinding));
			if (httpOperationBinding == null)
			{
				throw base.OperationBindingSyntaxException(Res.GetString("MissingHttpOperationElement0"));
			}
			HttpMethodInfo httpMethodInfo = new HttpMethodInfo();
			if (this.hasInputPayload)
			{
				httpMethodInfo.MimeParameters = this.ImportMimeParameters();
				if (httpMethodInfo.MimeParameters == null)
				{
					base.UnsupportedOperationWarning(Res.GetString("NoInputMIMEFormatsWereRecognized0"));
					return null;
				}
			}
			else
			{
				httpMethodInfo.UrlParameters = this.ImportUrlParameters();
				if (httpMethodInfo.UrlParameters == null)
				{
					base.UnsupportedOperationWarning(Res.GetString("NoInputHTTPFormatsWereRecognized0"));
					return null;
				}
			}
			httpMethodInfo.MimeReturn = this.ImportMimeReturn();
			if (httpMethodInfo.MimeReturn == null)
			{
				base.UnsupportedOperationWarning(Res.GetString("NoOutputMIMEFormatsWereRecognized0"));
				return null;
			}
			httpMethodInfo.Name = base.MethodNames.AddUnique(base.MethodName, httpMethodInfo);
			httpMethodInfo.Href = httpOperationBinding.Location;
			return this.GenerateMethod(httpMethodInfo);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00019CC4 File Offset: 0x00018CC4
		protected override CodeTypeDeclaration BeginClass()
		{
			base.MethodNames.Clear();
			base.ExtraCodeClasses.Clear();
			CodeAttributeDeclarationCollection codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection();
			if (base.Style == ServiceDescriptionImportStyle.Client)
			{
				WebCodeGenerator.AddCustomAttribute(codeAttributeDeclarationCollection, typeof(DebuggerStepThroughAttribute), new CodeExpression[0]);
				WebCodeGenerator.AddCustomAttribute(codeAttributeDeclarationCollection, typeof(DesignerCategoryAttribute), new CodeExpression[]
				{
					new CodePrimitiveExpression("code")
				});
			}
			Type[] array = new Type[]
			{
				typeof(SoapDocumentMethodAttribute),
				typeof(XmlAttributeAttribute),
				typeof(WebService),
				typeof(object),
				typeof(DebuggerStepThroughAttribute),
				typeof(DesignerCategoryAttribute),
				typeof(TransactionOption)
			};
			WebCodeGenerator.AddImports(base.CodeNamespace, WebCodeGenerator.GetNamespacesForTypes(array));
			CodeFlags codeFlags = (CodeFlags)0;
			if (base.Style == ServiceDescriptionImportStyle.Server)
			{
				codeFlags = CodeFlags.IsAbstract;
			}
			else if (base.Style == ServiceDescriptionImportStyle.ServerInterface)
			{
				codeFlags = CodeFlags.IsInterface;
			}
			CodeTypeDeclaration codeTypeDeclaration = WebCodeGenerator.CreateClass(base.ClassName, this.BaseClass.FullName, new string[0], codeAttributeDeclarationCollection, CodeFlags.IsPublic | codeFlags, base.ServiceImporter.CodeGenerator.Supports(GeneratorSupport.PartialTypes));
			codeTypeDeclaration.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
			CodeConstructor codeConstructor = WebCodeGenerator.AddConstructor(codeTypeDeclaration, new string[0], new string[0], null, CodeFlags.IsPublic);
			codeConstructor.Comments.Add(new CodeCommentStatement(Res.GetString("CodeRemarks"), true));
			HttpAddressBinding httpAddressBinding = ((base.Port == null) ? null : ((HttpAddressBinding)base.Port.Extensions.Find(typeof(HttpAddressBinding))));
			string text = ((httpAddressBinding != null) ? httpAddressBinding.Location : null);
			ServiceDescription serviceDescription = base.Binding.ServiceDescription;
			ProtocolImporterUtil.GenerateConstructorStatements(codeConstructor, text, serviceDescription.AppSettingUrlKey, serviceDescription.AppSettingBaseUrl, false);
			this.codeClasses.Add(codeTypeDeclaration);
			return codeTypeDeclaration;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00019EC4 File Offset: 0x00018EC4
		protected override void EndNamespace()
		{
			for (int i = 0; i < this.importers.Length; i++)
			{
				this.importers[i].GenerateCode((MimeReturn[])this.importedReturns[i].ToArray(typeof(MimeReturn)), (MimeParameterCollection[])this.importedParameters[i].ToArray(typeof(MimeParameterCollection)));
			}
			foreach (object obj in this.codeClasses)
			{
				CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj;
				if (codeTypeDeclaration.CustomAttributes == null)
				{
					codeTypeDeclaration.CustomAttributes = new CodeAttributeDeclarationCollection();
				}
				for (int j = 0; j < this.importers.Length; j++)
				{
					this.importers[j].AddClassMetadata(codeTypeDeclaration);
				}
			}
			foreach (object obj2 in base.ExtraCodeClasses)
			{
				CodeTypeDeclaration codeTypeDeclaration2 = (CodeTypeDeclaration)obj2;
				base.CodeNamespace.Types.Add(codeTypeDeclaration2);
			}
			CodeGenerator.ValidateIdentifiers(base.CodeNamespace);
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000516 RID: 1302
		internal abstract Type BaseClass { get; }

		// Token: 0x040003F9 RID: 1017
		private MimeImporter[] importers;

		// Token: 0x040003FA RID: 1018
		private ArrayList[] importedParameters;

		// Token: 0x040003FB RID: 1019
		private ArrayList[] importedReturns;

		// Token: 0x040003FC RID: 1020
		private bool hasInputPayload;

		// Token: 0x040003FD RID: 1021
		private ArrayList codeClasses = new ArrayList();
	}
}
