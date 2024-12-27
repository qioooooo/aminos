using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Win32;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000194 RID: 404
	public class AxWrapperGen
	{
		// Token: 0x06000F03 RID: 3843 RVA: 0x0003ED44 File Offset: 0x0003DD44
		public AxWrapperGen(Type axType)
		{
			this.axctl = axType.Name;
			this.axctl = this.axctl.TrimStart(new char[] { '_', '1' });
			this.axctl = "Ax" + this.axctl;
			this.clsidAx = axType.GUID;
			object[] array = axType.GetCustomAttributes(typeof(ComSourceInterfacesAttribute), false);
			if (array.Length == 0 && axType.BaseType.GUID.Equals(axType.GUID))
			{
				array = axType.BaseType.GetCustomAttributes(typeof(ComSourceInterfacesAttribute), false);
			}
			if (array.Length > 0)
			{
				ComSourceInterfacesAttribute comSourceInterfacesAttribute = (ComSourceInterfacesAttribute)array[0];
				string value = comSourceInterfacesAttribute.Value;
				char[] array2 = new char[1];
				int num = value.IndexOfAny(array2);
				string text = comSourceInterfacesAttribute.Value.Substring(0, num);
				this.axctlEventsType = axType.Module.Assembly.GetType(text);
				if (this.axctlEventsType == null)
				{
					this.axctlEventsType = Type.GetType(text, false);
				}
				if (this.axctlEventsType != null)
				{
					this.axctlEvents = this.axctlEventsType.FullName;
				}
			}
			Type[] interfaces = axType.GetInterfaces();
			this.axctlType = interfaces[0];
			foreach (Type type in interfaces)
			{
				array = type.GetCustomAttributes(typeof(CoClassAttribute), false);
				if (array.Length > 0)
				{
					Type[] interfaces2 = type.GetInterfaces();
					if (interfaces2 != null && interfaces2.Length > 0)
					{
						this.axctl = "Ax" + type.Name;
						this.axctlType = interfaces2[0];
						break;
					}
				}
			}
			this.axctlIface = this.axctlType.Name;
			foreach (Type type2 in interfaces)
			{
				if (type2 == typeof(IEnumerable))
				{
					this.enumerableInterface = true;
					break;
				}
			}
			try
			{
				array = this.axctlType.GetCustomAttributes(typeof(InterfaceTypeAttribute), false);
				if (array.Length > 0)
				{
					InterfaceTypeAttribute interfaceTypeAttribute = (InterfaceTypeAttribute)array[0];
					this.dispInterface = interfaceTypeAttribute.Value == ComInterfaceType.InterfaceIsIDispatch;
				}
			}
			catch (MissingMethodException)
			{
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0003EF8C File Offset: 0x0003DF8C
		private Hashtable AxHostMembers
		{
			get
			{
				if (this.axHostMembers == null)
				{
					this.FillAxHostMembers();
				}
				return this.axHostMembers;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0003EFA2 File Offset: 0x0003DFA2
		private Hashtable ConflictableThings
		{
			get
			{
				if (this.conflictableThings == null)
				{
					this.FillConflicatableThings();
				}
				return this.conflictableThings;
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0003EFB8 File Offset: 0x0003DFB8
		private void AddClassToNamespace(CodeNamespace ns, CodeTypeDeclaration cls)
		{
			if (AxWrapperGen.classesInNamespace == null)
			{
				AxWrapperGen.classesInNamespace = new Hashtable();
			}
			try
			{
				ns.Types.Add(cls);
				AxWrapperGen.classesInNamespace.Add(cls.Name, cls);
			}
			catch (Exception)
			{
			}
			catch
			{
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0003F018 File Offset: 0x0003E018
		private AxWrapperGen.EventEntry AddEvent(string name, string eventCls, string eventHandlerCls, Type retType, AxParameterData[] parameters)
		{
			if (this.events == null)
			{
				this.events = new ArrayList();
			}
			if (this.axctlTypeMembers == null)
			{
				this.axctlTypeMembers = new Hashtable();
				Type type = this.axctlType;
				MemberInfo[] members = type.GetMembers();
				foreach (MemberInfo memberInfo in members)
				{
					string name2 = memberInfo.Name;
					if (!this.axctlTypeMembers.Contains(name2))
					{
						this.axctlTypeMembers.Add(name2, memberInfo);
					}
				}
			}
			bool flag = this.axctlTypeMembers.Contains(name) || this.AxHostMembers.Contains(name) || this.ConflictableThings.Contains(name);
			AxWrapperGen.EventEntry eventEntry = new AxWrapperGen.EventEntry(name, eventCls, eventHandlerCls, retType, parameters, flag);
			this.events.Add(eventEntry);
			return eventEntry;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0003F0E4 File Offset: 0x0003E0E4
		private bool ClassAlreadyExistsInNamespace(CodeNamespace ns, string clsName)
		{
			return AxWrapperGen.classesInNamespace.Contains(clsName);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0003F0F4 File Offset: 0x0003E0F4
		private static string Compile(AxImporter importer, CodeNamespace ns, string[] refAssemblies, DateTime tlbTimeStamp, Version version)
		{
			CodeDomProvider codeDomProvider = new CSharpCodeProvider();
			ICodeGenerator codeGenerator = codeDomProvider.CreateGenerator();
			string outputName = importer.options.outputName;
			string text = Path.Combine(importer.options.outputDirectory, outputName);
			string text2 = Path.ChangeExtension(text, ".cs");
			CompilerParameters compilerParameters = new CompilerParameters(refAssemblies, text);
			compilerParameters.IncludeDebugInformation = importer.options.genSources;
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			codeCompileUnit.Namespaces.Add(ns);
			CodeAttributeDeclarationCollection assemblyCustomAttributes = codeCompileUnit.AssemblyCustomAttributes;
			assemblyCustomAttributes.Add(new CodeAttributeDeclaration("System.Reflection.AssemblyVersion", new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodePrimitiveExpression(version.ToString()))
			}));
			assemblyCustomAttributes.Add(new CodeAttributeDeclaration("System.Windows.Forms.AxHost.TypeLibraryTimeStamp", new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodePrimitiveExpression(tlbTimeStamp.ToString()))
			}));
			if (importer.options.delaySign)
			{
				assemblyCustomAttributes.Add(new CodeAttributeDeclaration("System.Reflection.AssemblyDelaySign", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(true))
				}));
			}
			if (importer.options.keyFile != null && importer.options.keyFile.Length > 0)
			{
				assemblyCustomAttributes.Add(new CodeAttributeDeclaration("System.Reflection.AssemblyKeyFile", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(importer.options.keyFile))
				}));
			}
			if (importer.options.keyContainer != null && importer.options.keyContainer.Length > 0)
			{
				assemblyCustomAttributes.Add(new CodeAttributeDeclaration("System.Reflection.AssemblyKeyName", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(importer.options.keyContainer))
				}));
			}
			CompilerResults compilerResults;
			if (importer.options.genSources)
			{
				AxWrapperGen.SaveCompileUnit(codeGenerator, codeCompileUnit, text2);
				compilerResults = ((ICodeCompiler)codeGenerator).CompileAssemblyFromFile(compilerParameters, text2);
			}
			else
			{
				compilerResults = ((ICodeCompiler)codeGenerator).CompileAssemblyFromDom(compilerParameters, codeCompileUnit);
			}
			if (compilerResults.Errors != null && compilerResults.Errors.Count > 0)
			{
				string text3 = null;
				CompilerErrorCollection errors = compilerResults.Errors;
				foreach (object obj in errors)
				{
					CompilerError compilerError = (CompilerError)obj;
					if (!compilerError.IsWarning)
					{
						text3 = text3 + compilerError.ToString() + "\r\n";
					}
				}
				if (text3 != null)
				{
					AxWrapperGen.SaveCompileUnit(codeGenerator, codeCompileUnit, text2);
					text3 = SR.GetString("AXCompilerError", new object[] { ns.Name, text2 }) + "\r\n" + text3;
					throw new Exception(text3);
				}
			}
			return text;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0003F3D8 File Offset: 0x0003E3D8
		private string CreateDataSourceFieldName(string propName)
		{
			return "ax" + propName;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0003F3E8 File Offset: 0x0003E3E8
		private CodeParameterDeclarationExpression CreateParamDecl(string type, string name, bool isOptional)
		{
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(type, name);
			if (!isOptional)
			{
				return codeParameterDeclarationExpression;
			}
			codeParameterDeclarationExpression.CustomAttributes = new CodeAttributeDeclarationCollection
			{
				new CodeAttributeDeclaration("System.Runtime.InteropServices.Optional", new CodeAttributeArgument[0])
			};
			return codeParameterDeclarationExpression;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0003F428 File Offset: 0x0003E428
		private CodeConditionStatement CreateValidStateCheck()
		{
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(this.memIfaceRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression2 = new CodeBinaryOperatorExpression(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "PropsValid", new CodeExpression[0]), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(true));
			CodeBinaryOperatorExpression codeBinaryOperatorExpression3 = new CodeBinaryOperatorExpression(codeBinaryOperatorExpression, CodeBinaryOperatorType.BooleanAnd, codeBinaryOperatorExpression2);
			return new CodeConditionStatement
			{
				Condition = codeBinaryOperatorExpression3
			};
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0003F490 File Offset: 0x0003E490
		private CodeStatement CreateInvalidStateException(string name, string kind)
		{
			CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(this.memIfaceRef, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeBinaryOperatorExpression;
			CodeExpression[] array = new CodeExpression[]
			{
				new CodePrimitiveExpression(name),
				new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, typeof(AxHost).FullName + ".ActiveXInvokeKind"), kind)
			};
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression(typeof(AxHost.InvalidActiveXStateException).FullName, array);
			codeConditionStatement.TrueStatements.Add(new CodeThrowExceptionStatement(codeObjectCreateExpression));
			return codeConditionStatement;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0003F524 File Offset: 0x0003E524
		private void FillAxHostMembers()
		{
			if (this.axHostMembers == null)
			{
				this.axHostMembers = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
				Type typeFromHandle = typeof(AxHost);
				MemberInfo[] members = typeFromHandle.GetMembers();
				foreach (MemberInfo memberInfo in members)
				{
					string name = memberInfo.Name;
					if (!this.axHostMembers.Contains(name))
					{
						FieldInfo fieldInfo = memberInfo as FieldInfo;
						if (fieldInfo != null && !fieldInfo.IsPrivate)
						{
							this.axHostMembers.Add(name, memberInfo);
						}
						else
						{
							PropertyInfo propertyInfo = memberInfo as PropertyInfo;
							if (propertyInfo != null)
							{
								this.axHostMembers.Add(name, memberInfo);
							}
							else
							{
								MethodBase methodBase = memberInfo as MethodBase;
								if (methodBase != null && !methodBase.IsPrivate)
								{
									this.axHostMembers.Add(name, memberInfo);
								}
								else
								{
									EventInfo eventInfo = memberInfo as EventInfo;
									if (eventInfo != null)
									{
										this.axHostMembers.Add(name, memberInfo);
									}
									else
									{
										Type type = memberInfo as Type;
										if (type != null && (type.IsPublic || type.IsNestedPublic))
										{
											this.axHostMembers.Add(name, memberInfo);
										}
										else
										{
											this.axHostMembers.Add(name, memberInfo);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0003F653 File Offset: 0x0003E653
		private void FillConflicatableThings()
		{
			if (this.conflictableThings == null)
			{
				this.conflictableThings = new Hashtable();
				this.conflictableThings.Add("System", "System");
			}
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0003F680 File Offset: 0x0003E680
		private static void SaveCompileUnit(ICodeGenerator codegen, CodeCompileUnit cu, string fileName)
		{
			try
			{
				try
				{
					if (File.Exists(fileName))
					{
						File.Delete(fileName);
					}
				}
				catch
				{
				}
				FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				StreamWriter streamWriter = new StreamWriter(fileStream, new UTF8Encoding(false));
				codegen.GenerateCodeFromCompileUnit(cu, streamWriter, null);
				streamWriter.Flush();
				streamWriter.Close();
				fileStream.Close();
				AxWrapperGen.GeneratedSources.Add(fileName);
			}
			catch (Exception)
			{
			}
			catch
			{
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0003F710 File Offset: 0x0003E710
		internal static string MapTypeName(Type type)
		{
			bool isArray = type.IsArray;
			Type elementType = type.GetElementType();
			if (elementType != null)
			{
				type = elementType;
			}
			string fullName = type.FullName;
			if (!isArray)
			{
				return fullName;
			}
			return fullName + "[]";
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0003F748 File Offset: 0x0003E748
		private static bool IsTypeActiveXControl(Type type)
		{
			if (type.IsClass && type.IsCOMObject && type.IsPublic && !type.GUID.Equals(Guid.Empty))
			{
				try
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(ComVisibleAttribute), false);
					if (customAttributes.Length != 0 && !((ComVisibleAttribute)customAttributes[0]).Value)
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
				string text = "CLSID\\{" + type.GUID.ToString() + "}\\Control";
				RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(text);
				if (registryKey == null)
				{
					return false;
				}
				registryKey.Close();
				Type[] interfaces = type.GetInterfaces();
				if (interfaces != null && interfaces.Length >= 1)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0003F828 File Offset: 0x0003E828
		internal static string GenerateWrappers(AxImporter importer, Guid axClsid, Assembly rcwAssem, string[] refAssemblies, DateTime tlbTimeStamp, out string assem)
		{
			assem = null;
			bool flag = false;
			CodeNamespace codeNamespace = null;
			string text = null;
			try
			{
				Type[] types = rcwAssem.GetTypes();
				for (int i = 0; i < types.Length; i++)
				{
					if (AxWrapperGen.IsTypeActiveXControl(types[i]))
					{
						flag = true;
						if (codeNamespace == null)
						{
							AxWrapperGen.axctlNS = "Ax" + types[i].Namespace;
							codeNamespace = new CodeNamespace(AxWrapperGen.axctlNS);
						}
						AxWrapperGen axWrapperGen = new AxWrapperGen(types[i]);
						axWrapperGen.GenerateAxHost(codeNamespace, refAssemblies);
						if (!axClsid.Equals(Guid.Empty) && axClsid.Equals(types[i].GUID))
						{
							text = axWrapperGen.axctl;
						}
						else if (axClsid.Equals(Guid.Empty) && text == null)
						{
							text = axWrapperGen.axctl;
						}
					}
				}
			}
			finally
			{
				if (AxWrapperGen.classesInNamespace != null)
				{
					AxWrapperGen.classesInNamespace.Clear();
					AxWrapperGen.classesInNamespace = null;
				}
			}
			AssemblyName name = rcwAssem.GetName();
			if (flag)
			{
				Version version = name.Version;
				assem = AxWrapperGen.Compile(importer, codeNamespace, refAssemblies, tlbTimeStamp, version);
				if (assem != null)
				{
					if (text == null)
					{
						throw new Exception(SR.GetString("AXNotValidControl", new object[] { "{" + axClsid + "}" }));
					}
					return string.Concat(new string[]
					{
						AxWrapperGen.axctlNS,
						".",
						text,
						",",
						AxWrapperGen.axctlNS
					});
				}
			}
			return null;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0003F9B0 File Offset: 0x0003E9B0
		private void GenerateAxHost(CodeNamespace ns, string[] refAssemblies)
		{
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration();
			codeTypeDeclaration.Name = this.axctl;
			codeTypeDeclaration.BaseTypes.Add(typeof(AxHost).FullName);
			if (this.enumerableInterface)
			{
				codeTypeDeclaration.BaseTypes.Add(typeof(IEnumerable));
			}
			CodeAttributeDeclarationCollection codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection();
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(typeof(AxHost.ClsidAttribute).FullName, new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodeSnippetExpression("\"{" + this.clsidAx.ToString() + "}\""))
			});
			codeAttributeDeclarationCollection.Add(codeAttributeDeclaration);
			CodeAttributeDeclaration codeAttributeDeclaration2 = new CodeAttributeDeclaration(typeof(DesignTimeVisibleAttribute).FullName, new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodePrimitiveExpression(true))
			});
			codeAttributeDeclarationCollection.Add(codeAttributeDeclaration2);
			codeTypeDeclaration.CustomAttributes = codeAttributeDeclarationCollection;
			object[] customAttributes = this.axctlType.GetCustomAttributes(typeof(DefaultMemberAttribute), true);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				this.defMember = ((DefaultMemberAttribute)customAttributes[0]).MemberName;
			}
			this.AddClassToNamespace(ns, codeTypeDeclaration);
			this.WriteMembersDecl(codeTypeDeclaration);
			if (this.axctlEventsType != null)
			{
				this.WriteEventMembersDecl(ns, codeTypeDeclaration);
			}
			CodeConstructor codeConstructor = this.WriteConstructor(codeTypeDeclaration);
			this.WriteProperties(codeTypeDeclaration);
			this.WriteMethods(codeTypeDeclaration);
			this.WriteHookupMethods(codeTypeDeclaration);
			if (this.aboutBoxMethod != null)
			{
				CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression("AboutBoxDelegate", new CodeExpression[0]);
				codeObjectCreateExpression.Parameters.Add(new CodeFieldReferenceExpression(null, this.aboutBoxMethod));
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "SetAboutBoxDelegate", new CodeExpression[0]);
				codeMethodInvokeExpression.Parameters.Add(codeObjectCreateExpression);
				codeConstructor.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			}
			if (this.axctlEventsType != null)
			{
				this.WriteEvents(ns, codeTypeDeclaration);
			}
			if (this.dataSourceProps.Count > 0)
			{
				this.WriteOnInPlaceActive(codeTypeDeclaration);
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0003FBAC File Offset: 0x0003EBAC
		private CodeExpression GetInitializer(Type type)
		{
			if (type == null)
			{
				return new CodePrimitiveExpression(null);
			}
			if (type == typeof(int) || type == typeof(short) || type == typeof(long) || type == typeof(float) || type == typeof(double) || typeof(Enum).IsAssignableFrom(type))
			{
				return new CodePrimitiveExpression(0);
			}
			if (type == typeof(char))
			{
				return new CodeCastExpression("System.Character", new CodePrimitiveExpression(0));
			}
			if (type == typeof(bool))
			{
				return new CodePrimitiveExpression(false);
			}
			return new CodePrimitiveExpression(null);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0003FC64 File Offset: 0x0003EC64
		private bool IsDispidKnown(int dp, string propName)
		{
			return dp == -513 || dp == -501 || dp == -512 || dp == -514 || dp == -516 || dp == -611 || dp == -517 || dp == -515 || (dp == 0 && propName.Equals(this.defMember));
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0003FCC4 File Offset: 0x0003ECC4
		private bool IsEventPresent(MethodInfo mievent)
		{
			return false;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0003FCC8 File Offset: 0x0003ECC8
		private bool IsPropertyBindable(PropertyInfo pinfo, out bool isDefaultBind)
		{
			isDefaultBind = false;
			MethodInfo getMethod = pinfo.GetGetMethod();
			if (getMethod == null)
			{
				return false;
			}
			object[] customAttributes = getMethod.GetCustomAttributes(typeof(TypeLibFuncAttribute), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				TypeLibFuncFlags value = ((TypeLibFuncAttribute)customAttributes[0]).Value;
				isDefaultBind = (value & TypeLibFuncFlags.FDefaultBind) != (TypeLibFuncFlags)0;
				if (isDefaultBind || (value & TypeLibFuncFlags.FBindable) != (TypeLibFuncFlags)0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0003FD28 File Offset: 0x0003ED28
		private bool IsPropertyBrowsable(PropertyInfo pinfo, AxWrapperGen.ComAliasEnum alias)
		{
			MethodInfo getMethod = pinfo.GetGetMethod();
			if (getMethod == null)
			{
				return false;
			}
			object[] customAttributes = getMethod.GetCustomAttributes(typeof(TypeLibFuncAttribute), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				TypeLibFuncFlags value = ((TypeLibFuncAttribute)customAttributes[0]).Value;
				if ((value & TypeLibFuncFlags.FNonBrowsable) != (TypeLibFuncFlags)0 || (value & TypeLibFuncFlags.FHidden) != (TypeLibFuncFlags)0)
				{
					return false;
				}
			}
			Type propertyType = pinfo.PropertyType;
			return alias != AxWrapperGen.ComAliasEnum.None || !propertyType.IsInterface || propertyType.GUID.Equals(AxWrapperGen.Guid_DataSource);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0003FDA8 File Offset: 0x0003EDA8
		private bool IsPropertySignature(PropertyInfo pinfo, out bool useLet)
		{
			int num = 0;
			bool flag = true;
			useLet = false;
			string text = ((this.defMember == null) ? "Item" : this.defMember);
			if (pinfo.Name.Equals(text))
			{
				num = pinfo.GetIndexParameters().Length;
			}
			if (pinfo.GetGetMethod() != null)
			{
				flag = this.IsPropertySignature(pinfo.GetGetMethod(), pinfo.PropertyType, true, num);
			}
			if (pinfo.GetSetMethod() != null)
			{
				flag = flag && this.IsPropertySignature(pinfo.GetSetMethod(), pinfo.PropertyType, false, num + 1);
				if (!flag)
				{
					MethodInfo method = pinfo.DeclaringType.GetMethod("let_" + pinfo.Name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
					if (method != null)
					{
						flag = this.IsPropertySignature(method, pinfo.PropertyType, false, num + 1);
						useLet = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0003FE68 File Offset: 0x0003EE68
		private bool IsPropertySignature(MethodInfo method, out bool hasPropInfo, out bool useLet)
		{
			useLet = false;
			hasPropInfo = false;
			if (!method.Name.StartsWith("get_") && !method.Name.StartsWith("set_") && !method.Name.StartsWith("let_"))
			{
				return false;
			}
			string text = method.Name.Substring(4, method.Name.Length - 4);
			PropertyInfo property = this.axctlType.GetProperty(text, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			return property != null && this.IsPropertySignature(property, out useLet);
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0003FEEC File Offset: 0x0003EEEC
		private bool IsPropertySignature(MethodInfo method, Type returnType, bool getter, int nParams)
		{
			if (method.IsConstructor)
			{
				return false;
			}
			if (getter)
			{
				string text = method.Name.Substring(4);
				if (this.axctlType.GetProperty(text) != null && method.GetParameters().Length == nParams)
				{
					return method.ReturnType == returnType;
				}
			}
			else
			{
				string text2 = method.Name.Substring(4);
				ParameterInfo[] parameters = method.GetParameters();
				if (this.axctlType.GetProperty(text2) != null && parameters.Length == nParams)
				{
					return parameters.Length <= 0 || parameters[parameters.Length - 1].ParameterType == returnType || (method.Name.StartsWith("let_") && parameters[parameters.Length - 1].ParameterType == typeof(object));
				}
			}
			return false;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0003FFAC File Offset: 0x0003EFAC
		private bool OptionalsPresent(MethodInfo method)
		{
			AxParameterData[] array = AxParameterData.Convert(method.GetParameters());
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].IsOptional)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0003FFEC File Offset: 0x0003EFEC
		private string ResolveConflict(string name, Type returnType, out bool fOverride, out bool fUseNew)
		{
			fOverride = false;
			fUseNew = false;
			string text = "";
			try
			{
				if (AxWrapperGen.axHostPropDescs == null)
				{
					AxWrapperGen.axHostPropDescs = new Hashtable();
					PropertyInfo[] properties = typeof(AxHost).GetProperties();
					foreach (PropertyInfo propertyInfo in properties)
					{
						AxWrapperGen.axHostPropDescs.Add(propertyInfo.Name + propertyInfo.PropertyType.GetHashCode(), propertyInfo);
					}
				}
				PropertyInfo propertyInfo2 = (PropertyInfo)AxWrapperGen.axHostPropDescs[name + returnType.GetHashCode()];
				if (propertyInfo2 != null)
				{
					if (returnType.Equals(propertyInfo2.PropertyType))
					{
						bool flag = propertyInfo2.CanRead && propertyInfo2.GetGetMethod().IsVirtual;
						if (flag)
						{
							fOverride = true;
						}
						else
						{
							fUseNew = true;
						}
					}
					else
					{
						text = "Ctl";
					}
				}
				else if (this.AxHostMembers.Contains(name) || this.ConflictableThings.Contains(name))
				{
					text = "Ctl";
				}
				else if ((name.StartsWith("get_") || name.StartsWith("set_")) && TypeDescriptor.GetProperties(typeof(AxHost))[name.Substring(4)] != null)
				{
					text = "Ctl";
				}
			}
			catch (AmbiguousMatchException)
			{
				text = "Ctl";
			}
			return text;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00040158 File Offset: 0x0003F158
		private CodeConstructor WriteConstructor(CodeTypeDeclaration cls)
		{
			CodeConstructor codeConstructor = new CodeConstructor();
			codeConstructor.Attributes = MemberAttributes.Public;
			codeConstructor.BaseConstructorArgs.Add(new CodeSnippetExpression("\"" + this.clsidAx.ToString() + "\""));
			cls.Members.Add(codeConstructor);
			return codeConstructor;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x000401B8 File Offset: 0x0003F1B8
		private void WriteOnInPlaceActive(CodeTypeDeclaration cls)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = "OnInPlaceActive";
			codeMemberMethod.Attributes = (MemberAttributes)12292;
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "OnInPlaceActive", new CodeExpression[0]);
			codeMemberMethod.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			foreach (object obj in this.dataSourceProps)
			{
				PropertyInfo propertyInfo = (PropertyInfo)obj;
				string text = this.CreateDataSourceFieldName(propertyInfo.Name);
				CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), text), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
				CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
				codeConditionStatement.Condition = codeBinaryOperatorExpression;
				CodeExpression codeExpression = new CodeFieldReferenceExpression(this.memIfaceRef, propertyInfo.Name);
				CodeExpression codeExpression2 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), text);
				codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(codeExpression, codeExpression2));
				codeMemberMethod.Statements.Add(codeConditionStatement);
			}
			cls.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x000402E4 File Offset: 0x0003F2E4
		private string WriteEventClass(CodeNamespace ns, MethodInfo mi, ParameterInfo[] pinfos)
		{
			string text = this.axctlEventsType.Name + "_" + mi.Name + "Event";
			if (this.ClassAlreadyExistsInNamespace(ns, text))
			{
				return text;
			}
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration();
			codeTypeDeclaration.Name = text;
			AxParameterData[] array = AxParameterData.Convert(pinfos);
			for (int i = 0; i < array.Length; i++)
			{
				CodeMemberField codeMemberField = new CodeMemberField(array[i].TypeName, array[i].Name);
				codeMemberField.Attributes = (MemberAttributes)24578;
				codeTypeDeclaration.Members.Add(codeMemberField);
			}
			CodeConstructor codeConstructor = new CodeConstructor();
			codeConstructor.Attributes = MemberAttributes.Public;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].Direction != FieldDirection.Out)
				{
					codeConstructor.Parameters.Add(this.CreateParamDecl(array[j].TypeName, array[j].Name, false));
					CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), array[j].Name);
					CodeFieldReferenceExpression codeFieldReferenceExpression2 = new CodeFieldReferenceExpression(null, array[j].Name);
					CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeFieldReferenceExpression, codeFieldReferenceExpression2);
					codeConstructor.Statements.Add(codeAssignStatement);
				}
			}
			codeTypeDeclaration.Members.Add(codeConstructor);
			this.AddClassToNamespace(ns, codeTypeDeclaration);
			return text;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00040420 File Offset: 0x0003F420
		private string WriteEventHandlerClass(CodeNamespace ns, MethodInfo mi)
		{
			string text = this.axctlEventsType.Name + "_" + mi.Name + "EventHandler";
			if (this.ClassAlreadyExistsInNamespace(ns, text))
			{
				return text;
			}
			this.AddClassToNamespace(ns, new CodeTypeDelegate
			{
				Name = text,
				Parameters = 
				{
					this.CreateParamDecl(typeof(object).FullName, "sender", false),
					this.CreateParamDecl(this.axctlEventsType.Name + "_" + mi.Name + "Event", "e", false)
				},
				ReturnType = new CodeTypeReference(mi.ReturnType)
			});
			return text;
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x000404E0 File Offset: 0x0003F4E0
		private void WriteEventMembersDecl(CodeNamespace ns, CodeTypeDeclaration cls)
		{
			bool flag = false;
			MethodInfo[] methods = this.axctlEventsType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < methods.Length; i++)
			{
				AxWrapperGen.EventEntry eventEntry = null;
				if (!this.IsEventPresent(methods[i]))
				{
					ParameterInfo[] parameters = methods[i].GetParameters();
					if (parameters.Length > 0 || methods[i].ReturnType != typeof(void))
					{
						string text = this.WriteEventHandlerClass(ns, methods[i]);
						string text2 = this.WriteEventClass(ns, methods[i], parameters);
						eventEntry = this.AddEvent(methods[i].Name, text2, text, methods[i].ReturnType, AxParameterData.Convert(parameters));
					}
					else
					{
						eventEntry = this.AddEvent(methods[i].Name, "System.EventArgs", "System.EventHandler", typeof(void), new AxParameterData[0]);
					}
				}
				if (!flag)
				{
					object[] customAttributes = methods[i].GetCustomAttributes(typeof(DispIdAttribute), false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						DispIdAttribute dispIdAttribute = (DispIdAttribute)customAttributes[0];
						if (dispIdAttribute.Value == 1)
						{
							string text3 = ((eventEntry != null) ? eventEntry.resovledEventName : methods[i].Name);
							CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.ComponentModel.DefaultEvent", new CodeAttributeArgument[]
							{
								new CodeAttributeArgument(new CodePrimitiveExpression(text3))
							});
							cls.CustomAttributes.Add(codeAttributeDeclaration);
							flag = true;
						}
					}
				}
			}
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00040630 File Offset: 0x0003F630
		private string WriteEventMulticaster(CodeNamespace ns)
		{
			string text = this.axctl + "EventMulticaster";
			if (this.ClassAlreadyExistsInNamespace(ns, text))
			{
				return text;
			}
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration();
			codeTypeDeclaration.Name = text;
			codeTypeDeclaration.BaseTypes.Add(this.axctlEvents);
			CodeAttributeDeclarationCollection codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection();
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.Runtime.InteropServices.ClassInterface", new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, "System.Runtime.InteropServices.ClassInterfaceType"), "None"))
			});
			codeAttributeDeclarationCollection.Add(codeAttributeDeclaration);
			codeTypeDeclaration.CustomAttributes = codeAttributeDeclarationCollection;
			CodeMemberField codeMemberField = new CodeMemberField(this.axctl, "parent");
			codeMemberField.Attributes = (MemberAttributes)20482;
			codeTypeDeclaration.Members.Add(codeMemberField);
			CodeConstructor codeConstructor = new CodeConstructor();
			codeConstructor.Attributes = MemberAttributes.Public;
			codeConstructor.Parameters.Add(this.CreateParamDecl(this.axctl, "parent", false));
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "parent");
			CodeFieldReferenceExpression codeFieldReferenceExpression2 = new CodeFieldReferenceExpression(null, "parent");
			codeConstructor.Statements.Add(new CodeAssignStatement(codeFieldReferenceExpression, codeFieldReferenceExpression2));
			codeTypeDeclaration.Members.Add(codeConstructor);
			MethodInfo[] methods = this.axctlEventsType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			int num = 0;
			for (int i = 0; i < methods.Length; i++)
			{
				AxParameterData[] array = AxParameterData.Convert(methods[i].GetParameters());
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = methods[i].Name;
				codeMemberMethod.Attributes = MemberAttributes.Public;
				codeMemberMethod.ReturnType = new CodeTypeReference(AxWrapperGen.MapTypeName(methods[i].ReturnType));
				for (int j = 0; j < array.Length; j++)
				{
					CodeParameterDeclarationExpression codeParameterDeclarationExpression = this.CreateParamDecl(AxWrapperGen.MapTypeName(array[j].ParameterType), array[j].Name, array[j].IsOptional);
					codeParameterDeclarationExpression.Direction = array[j].Direction;
					codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
				}
				CodeFieldReferenceExpression codeFieldReferenceExpression3 = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "parent");
				if (!this.IsEventPresent(methods[i]))
				{
					AxWrapperGen.EventEntry eventEntry = (AxWrapperGen.EventEntry)this.events[num++];
					CodeExpressionCollection codeExpressionCollection = new CodeExpressionCollection();
					codeExpressionCollection.Add(codeFieldReferenceExpression3);
					if (eventEntry.eventCls.Equals("EventArgs"))
					{
						codeExpressionCollection.Add(new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, "EventArgs"), "Empty"));
						CodeExpression[] array2 = new CodeExpression[codeExpressionCollection.Count];
						((ICollection)codeExpressionCollection).CopyTo(array2, 0);
						CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeFieldReferenceExpression3, eventEntry.invokeMethodName, array2);
						if (methods[i].ReturnType == typeof(void))
						{
							codeMemberMethod.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
						}
						else
						{
							codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(codeMethodInvokeExpression));
						}
					}
					else
					{
						CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression(eventEntry.eventCls, new CodeExpression[0]);
						for (int k = 0; k < eventEntry.parameters.Length; k++)
						{
							if (!eventEntry.parameters[k].IsOut)
							{
								codeObjectCreateExpression.Parameters.Add(new CodeFieldReferenceExpression(null, eventEntry.parameters[k].Name));
							}
						}
						CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(eventEntry.eventCls, eventEntry.eventParam);
						codeVariableDeclarationStatement.InitExpression = codeObjectCreateExpression;
						codeMemberMethod.Statements.Add(codeVariableDeclarationStatement);
						codeExpressionCollection.Add(new CodeFieldReferenceExpression(null, eventEntry.eventParam));
						CodeExpression[] array3 = new CodeExpression[codeExpressionCollection.Count];
						((ICollection)codeExpressionCollection).CopyTo(array3, 0);
						CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression(codeFieldReferenceExpression3, eventEntry.invokeMethodName, array3);
						if (methods[i].ReturnType == typeof(void))
						{
							codeMemberMethod.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression2));
						}
						else
						{
							CodeVariableDeclarationStatement codeVariableDeclarationStatement2 = new CodeVariableDeclarationStatement(eventEntry.retType, eventEntry.invokeMethodName);
							codeMemberMethod.Statements.Add(codeVariableDeclarationStatement2);
							codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(null, codeVariableDeclarationStatement2.Name), codeMethodInvokeExpression2));
						}
						for (int l = 0; l < array.Length; l++)
						{
							if (array[l].IsByRef)
							{
								codeMemberMethod.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(null, array[l].Name), new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, codeVariableDeclarationStatement.Name), array[l].Name)));
							}
						}
						if (methods[i].ReturnType != typeof(void))
						{
							codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(null, eventEntry.invokeMethodName)));
						}
					}
				}
				else
				{
					CodeExpressionCollection codeExpressionCollection2 = new CodeExpressionCollection();
					for (int m = 0; m < array.Length; m++)
					{
						codeExpressionCollection2.Add(new CodeFieldReferenceExpression(null, array[m].Name));
					}
					CodeExpression[] array4 = new CodeExpression[codeExpressionCollection2.Count];
					((ICollection)codeExpressionCollection2).CopyTo(array4, 0);
					CodeMethodInvokeExpression codeMethodInvokeExpression3 = new CodeMethodInvokeExpression(codeFieldReferenceExpression3, "RaiseOn" + methods[i].Name, array4);
					if (methods[i].ReturnType == typeof(void))
					{
						codeMemberMethod.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression3));
					}
					else
					{
						codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(codeMethodInvokeExpression3));
					}
				}
				codeTypeDeclaration.Members.Add(codeMemberMethod);
			}
			this.AddClassToNamespace(ns, codeTypeDeclaration);
			return text;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00040BC0 File Offset: 0x0003FBC0
		private void WriteEvents(CodeNamespace ns, CodeTypeDeclaration cls)
		{
			int num = 0;
			while (this.events != null && num < this.events.Count)
			{
				AxWrapperGen.EventEntry eventEntry = (AxWrapperGen.EventEntry)this.events[num];
				CodeMemberEvent codeMemberEvent = new CodeMemberEvent();
				codeMemberEvent.Name = eventEntry.resovledEventName;
				codeMemberEvent.Attributes = eventEntry.eventFlags;
				codeMemberEvent.Type = new CodeTypeReference(eventEntry.eventHandlerCls);
				cls.Members.Add(codeMemberEvent);
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = eventEntry.invokeMethodName;
				codeMemberMethod.ReturnType = new CodeTypeReference(eventEntry.retType);
				codeMemberMethod.Attributes = (MemberAttributes)4098;
				codeMemberMethod.Parameters.Add(this.CreateParamDecl(AxWrapperGen.MapTypeName(typeof(object)), "sender", false));
				codeMemberMethod.Parameters.Add(this.CreateParamDecl(eventEntry.eventCls, "e", false));
				CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), eventEntry.resovledEventName);
				CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(codeFieldReferenceExpression, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
				CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
				codeConditionStatement.Condition = codeBinaryOperatorExpression;
				CodeExpressionCollection codeExpressionCollection = new CodeExpressionCollection();
				codeExpressionCollection.Add(new CodeFieldReferenceExpression(null, "sender"));
				codeExpressionCollection.Add(new CodeFieldReferenceExpression(null, "e"));
				CodeExpression[] array = new CodeExpression[codeExpressionCollection.Count];
				((ICollection)codeExpressionCollection).CopyTo(array, 0);
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), eventEntry.resovledEventName, array);
				if (eventEntry.retType == typeof(void))
				{
					codeConditionStatement.TrueStatements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
				}
				else
				{
					codeConditionStatement.TrueStatements.Add(new CodeMethodReturnStatement(codeMethodInvokeExpression));
					codeConditionStatement.FalseStatements.Add(new CodeMethodReturnStatement(this.GetInitializer(eventEntry.retType)));
				}
				codeMemberMethod.Statements.Add(codeConditionStatement);
				cls.Members.Add(codeMemberMethod);
				num++;
			}
			this.WriteEventMulticaster(ns);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00040DBC File Offset: 0x0003FDBC
		private void WriteHookupMethods(CodeTypeDeclaration cls)
		{
			if (this.axctlEventsType != null)
			{
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.Name = "CreateSink";
				codeMemberMethod.Attributes = (MemberAttributes)12292;
				CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression(this.axctl + "EventMulticaster", new CodeExpression[0]);
				codeObjectCreateExpression.Parameters.Add(new CodeThisReferenceExpression());
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement(this.multicasterRef, codeObjectCreateExpression);
				CodeObjectCreateExpression codeObjectCreateExpression2 = new CodeObjectCreateExpression(typeof(AxHost.ConnectionPointCookie).FullName, new CodeExpression[0]);
				codeObjectCreateExpression2.Parameters.Add(this.memIfaceRef);
				codeObjectCreateExpression2.Parameters.Add(this.multicasterRef);
				codeObjectCreateExpression2.Parameters.Add(new CodeTypeOfExpression(this.axctlEvents));
				CodeAssignStatement codeAssignStatement2 = new CodeAssignStatement(this.cookieRef, codeObjectCreateExpression2);
				CodeTryCatchFinallyStatement codeTryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
				codeTryCatchFinallyStatement.TryStatements.Add(codeAssignStatement);
				codeTryCatchFinallyStatement.TryStatements.Add(codeAssignStatement2);
				codeTryCatchFinallyStatement.CatchClauses.Add(new CodeCatchClause("", new CodeTypeReference(typeof(Exception))));
				codeMemberMethod.Statements.Add(codeTryCatchFinallyStatement);
				cls.Members.Add(codeMemberMethod);
				CodeMemberMethod codeMemberMethod2 = new CodeMemberMethod();
				codeMemberMethod2.Name = "DetachSink";
				codeMemberMethod2.Attributes = (MemberAttributes)12292;
				CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.cookie);
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeFieldReferenceExpression, "Disconnect", new CodeExpression[0]);
				codeTryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
				codeTryCatchFinallyStatement.TryStatements.Add(codeMethodInvokeExpression);
				codeTryCatchFinallyStatement.CatchClauses.Add(new CodeCatchClause("", new CodeTypeReference(typeof(Exception))));
				codeMemberMethod2.Statements.Add(codeTryCatchFinallyStatement);
				cls.Members.Add(codeMemberMethod2);
			}
			CodeMemberMethod codeMemberMethod3 = new CodeMemberMethod();
			codeMemberMethod3.Name = "AttachInterfaces";
			codeMemberMethod3.Attributes = (MemberAttributes)12292;
			CodeCastExpression codeCastExpression = new CodeCastExpression(this.axctlType.FullName, new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "GetOcx", new CodeExpression[0]));
			CodeAssignStatement codeAssignStatement3 = new CodeAssignStatement(this.memIfaceRef, codeCastExpression);
			CodeTryCatchFinallyStatement codeTryCatchFinallyStatement2 = new CodeTryCatchFinallyStatement();
			codeTryCatchFinallyStatement2.TryStatements.Add(codeAssignStatement3);
			codeTryCatchFinallyStatement2.CatchClauses.Add(new CodeCatchClause("", new CodeTypeReference(typeof(Exception))));
			codeMemberMethod3.Statements.Add(codeTryCatchFinallyStatement2);
			cls.Members.Add(codeMemberMethod3);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00041040 File Offset: 0x00040040
		private void WriteMembersDecl(CodeTypeDeclaration cls)
		{
			this.memIface = "ocx";
			this.memIfaceRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.memIface);
			cls.Members.Add(new CodeMemberField(AxWrapperGen.MapTypeName(this.axctlType), this.memIface));
			if (this.axctlEventsType != null)
			{
				this.multicaster = "eventMulticaster";
				this.multicasterRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.multicaster);
				cls.Members.Add(new CodeMemberField(this.axctl + "EventMulticaster", this.multicaster));
				this.cookie = "cookie";
				this.cookieRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.cookie);
				cls.Members.Add(new CodeMemberField(typeof(AxHost.ConnectionPointCookie).FullName, this.cookie));
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0004112C File Offset: 0x0004012C
		private void WriteMethod(CodeTypeDeclaration cls, MethodInfo method, bool hasPropInfo, bool removeOptionals)
		{
			AxWrapperGen.AxMethodGenerator axMethodGenerator = AxWrapperGen.AxMethodGenerator.Create(method, removeOptionals);
			axMethodGenerator.ControlType = this.axctlType;
			string text = method.Name;
			bool flag = false;
			bool flag2 = false;
			this.ResolveConflict(method.Name, method.ReturnType, out flag, out flag2);
			if (flag)
			{
				text = "Ctl" + text;
			}
			CodeMemberMethod codeMemberMethod = axMethodGenerator.CreateMethod(text);
			codeMemberMethod.Statements.Add(this.CreateInvalidStateException(codeMemberMethod.Name, "MethodInvoke"));
			List<CodeExpression> list = axMethodGenerator.GenerateAndMarshalParameters(codeMemberMethod);
			CodeExpression codeExpression = axMethodGenerator.DoMethodInvoke(codeMemberMethod, method.Name, this.memIfaceRef, list);
			axMethodGenerator.UnmarshalParameters(codeMemberMethod, list);
			axMethodGenerator.GenerateReturn(codeMemberMethod, codeExpression);
			cls.Members.Add(codeMemberMethod);
			object[] customAttributes = method.GetCustomAttributes(typeof(DispIdAttribute), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				DispIdAttribute dispIdAttribute = (DispIdAttribute)customAttributes[0];
				if (dispIdAttribute.Value == -552 && method.GetParameters().Length == 0)
				{
					this.aboutBoxMethod = codeMemberMethod.Name;
				}
			}
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0004123C File Offset: 0x0004023C
		private void WriteMethods(CodeTypeDeclaration cls)
		{
			MethodInfo[] methods = this.axctlType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < methods.Length; i++)
			{
				bool flag;
				bool flag2;
				if (!this.IsPropertySignature(methods[i], out flag, out flag2))
				{
					if (this.OptionalsPresent(methods[i]))
					{
						this.WriteMethod(cls, methods[i], flag, true);
					}
					this.WriteMethod(cls, methods[i], flag, false);
				}
			}
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0004129C File Offset: 0x0004029C
		private void WriteProperty(CodeTypeDeclaration cls, PropertyInfo pinfo, bool useLet)
		{
			/*
An exception occurred when decompiling this method (06000F2A)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.AxWrapperGen::WriteProperty(System.CodeDom.CodeTypeDeclaration,System.Reflection.PropertyInfo,System.Boolean)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 519
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00041830 File Offset: 0x00040830
		private void WritePropertyGetter(CodeMemberProperty prop, PropertyInfo pinfo, AxWrapperGen.ComAliasEnum alias, AxParameterData[] parameters, bool fMethodSyntax, bool fOverride, bool dataSourceProp)
		{
			if (dataSourceProp)
			{
				string text = this.CreateDataSourceFieldName(pinfo.Name);
				CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), text));
				prop.GetStatements.Add(codeMethodReturnStatement);
				return;
			}
			if (fOverride)
			{
				CodeConditionStatement codeConditionStatement = this.CreateValidStateCheck();
				codeConditionStatement.TrueStatements.Add(this.GetPropertyGetRValue(pinfo, this.memIfaceRef, alias, parameters, fMethodSyntax));
				codeConditionStatement.FalseStatements.Add(this.GetPropertyGetRValue(pinfo, new CodeBaseReferenceExpression(), AxWrapperGen.ComAliasEnum.None, parameters, false));
				prop.GetStatements.Add(codeConditionStatement);
				return;
			}
			prop.GetStatements.Add(this.CreateInvalidStateException(prop.Name, "PropertyGet"));
			prop.GetStatements.Add(this.GetPropertyGetRValue(pinfo, this.memIfaceRef, alias, parameters, fMethodSyntax));
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x000418FC File Offset: 0x000408FC
		private void WritePropertySetter(CodeMemberProperty prop, PropertyInfo pinfo, AxWrapperGen.ComAliasEnum alias, AxParameterData[] parameters, bool fMethodSyntax, bool fOverride, bool useLet, bool dataSourceProp)
		{
			if (!fOverride && !dataSourceProp)
			{
				prop.SetStatements.Add(this.CreateInvalidStateException(prop.Name, "PropertySet"));
			}
			if (dataSourceProp)
			{
				string text = this.CreateDataSourceFieldName(pinfo.Name);
				this.WriteDataSourcePropertySetter(prop, pinfo, text);
				return;
			}
			if (!fMethodSyntax)
			{
				this.WritePropertySetterProp(prop, pinfo, alias, parameters, fOverride, useLet);
				return;
			}
			this.WritePropertySetterMethod(prop, pinfo, alias, parameters, fOverride, useLet);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0004196C File Offset: 0x0004096C
		private void WriteDataSourcePropertySetter(CodeMemberProperty prop, PropertyInfo pinfo, string dataSourceName)
		{
			CodeExpression codeExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), dataSourceName);
			CodeExpression codeExpression2 = new CodeFieldReferenceExpression(null, "value");
			CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeExpression, codeExpression2);
			prop.SetStatements.Add(codeAssignStatement);
			CodeConditionStatement codeConditionStatement = this.CreateValidStateCheck();
			codeExpression = new CodeFieldReferenceExpression(this.memIfaceRef, pinfo.Name);
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(codeExpression, codeExpression2));
			prop.SetStatements.Add(codeConditionStatement);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x000419E0 File Offset: 0x000409E0
		private void WritePropertySetterMethod(CodeMemberProperty prop, PropertyInfo pinfo, AxWrapperGen.ComAliasEnum alias, AxParameterData[] parameters, bool fOverride, bool useLet)
		{
			CodeExpression codeExpression = null;
			CodeConditionStatement codeConditionStatement = null;
			if (fOverride)
			{
				if (parameters.Length > 0)
				{
					codeExpression = new CodeIndexerExpression(this.memIfaceRef, new CodeExpression[0]);
				}
				else
				{
					codeExpression = new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), pinfo.Name);
				}
				CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(this.memIfaceRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
				codeConditionStatement = new CodeConditionStatement();
				codeConditionStatement.Condition = codeBinaryOperatorExpression;
			}
			string text = (useLet ? ("let_" + pinfo.Name) : pinfo.GetSetMethod().Name);
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(this.memIfaceRef, text, new CodeExpression[0]);
			for (int i = 0; i < parameters.Length; i++)
			{
				if (fOverride)
				{
					((CodeIndexerExpression)codeExpression).Indices.Add(new CodeFieldReferenceExpression(null, parameters[i].Name));
				}
				codeMethodInvokeExpression.Parameters.Add(new CodeFieldReferenceExpression(null, parameters[i].Name));
			}
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(null, "value");
			CodeExpression propertySetRValue = this.GetPropertySetRValue(alias, pinfo.PropertyType);
			CodeFieldReferenceExpression codeFieldReferenceExpression2;
			if (alias != AxWrapperGen.ComAliasEnum.None)
			{
				string wftoComParamConverter = AxWrapperGen.ComAliasConverter.GetWFToComParamConverter(alias, pinfo.PropertyType);
				CodeParameterDeclarationExpression codeParameterDeclarationExpression;
				if (wftoComParamConverter.Length == 0)
				{
					codeParameterDeclarationExpression = this.CreateParamDecl(AxWrapperGen.MapTypeName(pinfo.PropertyType), "paramTemp", false);
				}
				else
				{
					codeParameterDeclarationExpression = this.CreateParamDecl(wftoComParamConverter, "paramTemp", false);
				}
				prop.SetStatements.Add(new CodeAssignStatement(codeParameterDeclarationExpression, propertySetRValue));
				codeFieldReferenceExpression2 = new CodeFieldReferenceExpression(null, "paramTemp");
			}
			else
			{
				codeFieldReferenceExpression2 = codeFieldReferenceExpression;
			}
			codeMethodInvokeExpression.Parameters.Add(new CodeDirectionExpression(useLet ? FieldDirection.In : FieldDirection.Ref, codeFieldReferenceExpression2));
			if (fOverride)
			{
				prop.SetStatements.Add(new CodeAssignStatement(codeExpression, codeFieldReferenceExpression));
				codeConditionStatement.TrueStatements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
				prop.SetStatements.Add(codeConditionStatement);
				return;
			}
			prop.SetStatements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00041BC0 File Offset: 0x00040BC0
		private void WritePropertySetterProp(CodeMemberProperty prop, PropertyInfo pinfo, AxWrapperGen.ComAliasEnum alias, AxParameterData[] parameters, bool fOverride, bool useLet)
		{
			CodeExpression codeExpression = null;
			CodeConditionStatement codeConditionStatement = null;
			if (fOverride)
			{
				if (parameters.Length > 0)
				{
					codeExpression = new CodeIndexerExpression(this.memIfaceRef, new CodeExpression[0]);
				}
				else
				{
					codeExpression = new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), pinfo.Name);
				}
				CodeBinaryOperatorExpression codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(this.memIfaceRef, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
				codeConditionStatement = new CodeConditionStatement();
				codeConditionStatement.Condition = codeBinaryOperatorExpression;
			}
			CodeExpression codeExpression2;
			if (parameters.Length > 0)
			{
				codeExpression2 = new CodeIndexerExpression(this.memIfaceRef, new CodeExpression[0]);
			}
			else
			{
				codeExpression2 = new CodePropertyReferenceExpression(this.memIfaceRef, pinfo.Name);
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				if (fOverride)
				{
					((CodeIndexerExpression)codeExpression).Indices.Add(new CodeFieldReferenceExpression(null, parameters[i].Name));
				}
				((CodeIndexerExpression)codeExpression2).Indices.Add(new CodeFieldReferenceExpression(null, parameters[i].Name));
			}
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(null, "value");
			CodeExpression propertySetRValue = this.GetPropertySetRValue(alias, pinfo.PropertyType);
			if (fOverride)
			{
				prop.SetStatements.Add(new CodeAssignStatement(codeExpression, codeFieldReferenceExpression));
				codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(codeExpression2, propertySetRValue));
				prop.SetStatements.Add(codeConditionStatement);
				return;
			}
			prop.SetStatements.Add(new CodeAssignStatement(codeExpression2, propertySetRValue));
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00041D14 File Offset: 0x00040D14
		private CodeMethodReturnStatement GetPropertyGetRValue(PropertyInfo pinfo, CodeExpression reference, AxWrapperGen.ComAliasEnum alias, AxParameterData[] parameters, bool fMethodSyntax)
		{
			CodeExpression codeExpression;
			if (fMethodSyntax)
			{
				codeExpression = new CodeMethodInvokeExpression(reference, pinfo.GetGetMethod().Name, new CodeExpression[0]);
				for (int i = 0; i < parameters.Length; i++)
				{
					((CodeMethodInvokeExpression)codeExpression).Parameters.Add(new CodeFieldReferenceExpression(null, parameters[i].Name));
				}
			}
			else if (parameters.Length > 0)
			{
				codeExpression = new CodeIndexerExpression(reference, new CodeExpression[0]);
				for (int j = 0; j < parameters.Length; j++)
				{
					((CodeIndexerExpression)codeExpression).Indices.Add(new CodeFieldReferenceExpression(null, parameters[j].Name));
				}
			}
			else
			{
				codeExpression = new CodePropertyReferenceExpression(reference, (parameters.Length == 0) ? pinfo.Name : "");
			}
			if (alias != AxWrapperGen.ComAliasEnum.None)
			{
				string comToManagedConverter = AxWrapperGen.ComAliasConverter.GetComToManagedConverter(alias);
				string comToWFParamConverter = AxWrapperGen.ComAliasConverter.GetComToWFParamConverter(alias);
				CodeExpression[] array;
				if (comToWFParamConverter.Length == 0)
				{
					array = new CodeExpression[] { codeExpression };
				}
				else
				{
					CodeCastExpression codeCastExpression = new CodeCastExpression(comToWFParamConverter, codeExpression);
					array = new CodeExpression[] { codeCastExpression };
				}
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(null, comToManagedConverter, array);
				return new CodeMethodReturnStatement(codeMethodInvokeExpression);
			}
			return new CodeMethodReturnStatement(codeExpression);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00041E38 File Offset: 0x00040E38
		private CodeExpression GetPropertySetRValue(AxWrapperGen.ComAliasEnum alias, Type propertyType)
		{
			CodeExpression codeExpression = new CodePropertySetValueReferenceExpression();
			if (alias == AxWrapperGen.ComAliasEnum.None)
			{
				return codeExpression;
			}
			string wftoComConverter = AxWrapperGen.ComAliasConverter.GetWFToComConverter(alias);
			string wftoComParamConverter = AxWrapperGen.ComAliasConverter.GetWFToComParamConverter(alias, propertyType);
			CodeExpression[] array = new CodeExpression[] { codeExpression };
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(null, wftoComConverter, array);
			if (wftoComParamConverter.Length == 0)
			{
				return codeMethodInvokeExpression;
			}
			return new CodeCastExpression(wftoComParamConverter, codeMethodInvokeExpression);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00041E8C File Offset: 0x00040E8C
		private void WriteProperties(CodeTypeDeclaration cls)
		{
			PropertyInfo[] properties = this.axctlType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < properties.Length; i++)
			{
				bool flag;
				if (this.IsPropertySignature(properties[i], out flag))
				{
					this.WriteProperty(cls, properties[i], flag);
				}
			}
		}

		// Token: 0x04000FAC RID: 4012
		private string axctlIface;

		// Token: 0x04000FAD RID: 4013
		private Type axctlType;

		// Token: 0x04000FAE RID: 4014
		private Guid clsidAx;

		// Token: 0x04000FAF RID: 4015
		private string axctlEvents;

		// Token: 0x04000FB0 RID: 4016
		private Type axctlEventsType;

		// Token: 0x04000FB1 RID: 4017
		private string axctl;

		// Token: 0x04000FB2 RID: 4018
		private static string axctlNS;

		// Token: 0x04000FB3 RID: 4019
		private string memIface;

		// Token: 0x04000FB4 RID: 4020
		private string multicaster;

		// Token: 0x04000FB5 RID: 4021
		private string cookie;

		// Token: 0x04000FB6 RID: 4022
		private bool dispInterface;

		// Token: 0x04000FB7 RID: 4023
		private bool enumerableInterface;

		// Token: 0x04000FB8 RID: 4024
		private string defMember;

		// Token: 0x04000FB9 RID: 4025
		private string aboutBoxMethod;

		// Token: 0x04000FBA RID: 4026
		private CodeFieldReferenceExpression memIfaceRef;

		// Token: 0x04000FBB RID: 4027
		private CodeFieldReferenceExpression multicasterRef;

		// Token: 0x04000FBC RID: 4028
		private CodeFieldReferenceExpression cookieRef;

		// Token: 0x04000FBD RID: 4029
		private ArrayList events;

		// Token: 0x04000FBE RID: 4030
		public static ArrayList GeneratedSources = new ArrayList();

		// Token: 0x04000FBF RID: 4031
		private static Guid Guid_DataSource = new Guid("{7C0FFAB3-CD84-11D0-949A-00A0C91110ED}");

		// Token: 0x04000FC0 RID: 4032
		internal static BooleanSwitch AxWrapper = new BooleanSwitch("AxWrapper", "ActiveX WFW wrapper generation.");

		// Token: 0x04000FC1 RID: 4033
		internal static BooleanSwitch AxCodeGen = new BooleanSwitch("AxCodeGen", "ActiveX WFW property generation.");

		// Token: 0x04000FC2 RID: 4034
		private static CodeAttributeDeclaration nobrowse = null;

		// Token: 0x04000FC3 RID: 4035
		private static CodeAttributeDeclaration browse = null;

		// Token: 0x04000FC4 RID: 4036
		private static CodeAttributeDeclaration nopersist = null;

		// Token: 0x04000FC5 RID: 4037
		private static CodeAttributeDeclaration bindable = null;

		// Token: 0x04000FC6 RID: 4038
		private static CodeAttributeDeclaration defaultBind = null;

		// Token: 0x04000FC7 RID: 4039
		private Hashtable axctlTypeMembers;

		// Token: 0x04000FC8 RID: 4040
		private Hashtable axHostMembers;

		// Token: 0x04000FC9 RID: 4041
		private Hashtable conflictableThings;

		// Token: 0x04000FCA RID: 4042
		private static Hashtable classesInNamespace;

		// Token: 0x04000FCB RID: 4043
		private static Hashtable axHostPropDescs;

		// Token: 0x04000FCC RID: 4044
		private ArrayList dataSourceProps = new ArrayList();

		// Token: 0x02000195 RID: 405
		private enum ComAliasEnum
		{
			// Token: 0x04000FCE RID: 4046
			None,
			// Token: 0x04000FCF RID: 4047
			Color,
			// Token: 0x04000FD0 RID: 4048
			Font,
			// Token: 0x04000FD1 RID: 4049
			FontDisp,
			// Token: 0x04000FD2 RID: 4050
			Handle,
			// Token: 0x04000FD3 RID: 4051
			Picture,
			// Token: 0x04000FD4 RID: 4052
			PictureDisp
		}

		// Token: 0x02000196 RID: 406
		private static class ComAliasConverter
		{
			// Token: 0x06000F34 RID: 3892 RVA: 0x00041F3C File Offset: 0x00040F3C
			public static string GetComToManagedConverter(AxWrapperGen.ComAliasEnum alias)
			{
				if (alias == AxWrapperGen.ComAliasEnum.Color)
				{
					return "GetColorFromOleColor";
				}
				if (AxWrapperGen.ComAliasConverter.IsFont(alias))
				{
					return "GetFontFromIFont";
				}
				if (AxWrapperGen.ComAliasConverter.IsPicture(alias))
				{
					return "GetPictureFromIPicture";
				}
				return "";
			}

			// Token: 0x06000F35 RID: 3893 RVA: 0x00041F69 File Offset: 0x00040F69
			public static string GetComToWFParamConverter(AxWrapperGen.ComAliasEnum alias)
			{
				if (alias == AxWrapperGen.ComAliasEnum.Color)
				{
					return typeof(uint).FullName;
				}
				return "";
			}

			// Token: 0x06000F36 RID: 3894 RVA: 0x00041F84 File Offset: 0x00040F84
			private static Guid GetGuid(Type t)
			{
				Guid guid = Guid.Empty;
				if (AxWrapperGen.ComAliasConverter.typeGuids == null)
				{
					AxWrapperGen.ComAliasConverter.typeGuids = new Hashtable();
				}
				else if (AxWrapperGen.ComAliasConverter.typeGuids.Contains(t))
				{
					return (Guid)AxWrapperGen.ComAliasConverter.typeGuids[t];
				}
				guid = t.GUID;
				AxWrapperGen.ComAliasConverter.typeGuids.Add(t, guid);
				return guid;
			}

			// Token: 0x06000F37 RID: 3895 RVA: 0x00041FE4 File Offset: 0x00040FE4
			public static Type GetWFTypeFromComType(Type t, AxWrapperGen.ComAliasEnum alias)
			{
				if (!AxWrapperGen.ComAliasConverter.IsValidType(alias, t))
				{
					return t;
				}
				if (alias == AxWrapperGen.ComAliasEnum.Color)
				{
					return typeof(Color);
				}
				if (AxWrapperGen.ComAliasConverter.IsFont(alias))
				{
					return typeof(Font);
				}
				if (AxWrapperGen.ComAliasConverter.IsPicture(alias))
				{
					return typeof(Image);
				}
				return t;
			}

			// Token: 0x06000F38 RID: 3896 RVA: 0x00042032 File Offset: 0x00041032
			public static string GetWFToComConverter(AxWrapperGen.ComAliasEnum alias)
			{
				if (alias == AxWrapperGen.ComAliasEnum.Color)
				{
					return "GetOleColorFromColor";
				}
				if (AxWrapperGen.ComAliasConverter.IsFont(alias))
				{
					return "GetIFontFromFont";
				}
				if (AxWrapperGen.ComAliasConverter.IsPicture(alias))
				{
					return "GetIPictureFromPicture";
				}
				return "";
			}

			// Token: 0x06000F39 RID: 3897 RVA: 0x0004205F File Offset: 0x0004105F
			public static string GetWFToComParamConverter(AxWrapperGen.ComAliasEnum alias, Type t)
			{
				return t.FullName;
			}

			// Token: 0x06000F3A RID: 3898 RVA: 0x00042068 File Offset: 0x00041068
			public static AxWrapperGen.ComAliasEnum GetComAliasEnum(MemberInfo memberInfo, Type type, ICustomAttributeProvider attrProvider)
			{
				string text = null;
				int num = -1;
				object[] array = new object[0];
				if (attrProvider != null)
				{
					array = attrProvider.GetCustomAttributes(typeof(ComAliasNameAttribute), false);
				}
				if (array != null && array.Length > 0)
				{
					ComAliasNameAttribute comAliasNameAttribute = (ComAliasNameAttribute)array[0];
					text = comAliasNameAttribute.Value;
				}
				if (text != null && text.Length != 0)
				{
					if (text.EndsWith(".OLE_COLOR") && AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Color, type))
					{
						return AxWrapperGen.ComAliasEnum.Color;
					}
					if (text.EndsWith(".OLE_HANDLE") && AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Handle, type))
					{
						return AxWrapperGen.ComAliasEnum.Handle;
					}
				}
				if (memberInfo is PropertyInfo && string.Equals(memberInfo.Name, "hWnd", StringComparison.OrdinalIgnoreCase) && AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Handle, type))
				{
					return AxWrapperGen.ComAliasEnum.Handle;
				}
				if (attrProvider != null)
				{
					array = attrProvider.GetCustomAttributes(typeof(DispIdAttribute), false);
					if (array != null && array.Length > 0)
					{
						DispIdAttribute dispIdAttribute = (DispIdAttribute)array[0];
						num = dispIdAttribute.Value;
					}
				}
				if ((num == -501 || num == -513 || num == -510 || num == -503) && AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Color, type))
				{
					return AxWrapperGen.ComAliasEnum.Color;
				}
				if (num == -512 && AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Font, type))
				{
					return AxWrapperGen.ComAliasEnum.Font;
				}
				if (num == -523 && AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Picture, type))
				{
					return AxWrapperGen.ComAliasEnum.Picture;
				}
				if (num == -515 && AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Handle, type))
				{
					return AxWrapperGen.ComAliasEnum.Handle;
				}
				if (AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Font, type))
				{
					return AxWrapperGen.ComAliasEnum.Font;
				}
				if (AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.FontDisp, type))
				{
					return AxWrapperGen.ComAliasEnum.FontDisp;
				}
				if (AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.Picture, type))
				{
					return AxWrapperGen.ComAliasEnum.Picture;
				}
				if (AxWrapperGen.ComAliasConverter.IsValidType(AxWrapperGen.ComAliasEnum.PictureDisp, type))
				{
					return AxWrapperGen.ComAliasEnum.PictureDisp;
				}
				return AxWrapperGen.ComAliasEnum.None;
			}

			// Token: 0x06000F3B RID: 3899 RVA: 0x000421D0 File Offset: 0x000411D0
			public static bool IsFont(AxWrapperGen.ComAliasEnum e)
			{
				return e == AxWrapperGen.ComAliasEnum.Font || e == AxWrapperGen.ComAliasEnum.FontDisp;
			}

			// Token: 0x06000F3C RID: 3900 RVA: 0x000421DC File Offset: 0x000411DC
			public static bool IsPicture(AxWrapperGen.ComAliasEnum e)
			{
				return e == AxWrapperGen.ComAliasEnum.Picture || e == AxWrapperGen.ComAliasEnum.PictureDisp;
			}

			// Token: 0x06000F3D RID: 3901 RVA: 0x000421E8 File Offset: 0x000411E8
			private static bool IsValidType(AxWrapperGen.ComAliasEnum e, Type t)
			{
				switch (e)
				{
				case AxWrapperGen.ComAliasEnum.Color:
					return t == typeof(ushort) || t == typeof(uint) || t == typeof(int) || t == typeof(short);
				case AxWrapperGen.ComAliasEnum.Font:
					return AxWrapperGen.ComAliasConverter.GetGuid(t).Equals(AxWrapperGen.ComAliasConverter.Guid_IFont);
				case AxWrapperGen.ComAliasEnum.FontDisp:
					return AxWrapperGen.ComAliasConverter.GetGuid(t).Equals(AxWrapperGen.ComAliasConverter.Guid_IFontDisp);
				case AxWrapperGen.ComAliasEnum.Handle:
					return t == typeof(uint) || t == typeof(int) || t == typeof(IntPtr) || t == typeof(UIntPtr);
				case AxWrapperGen.ComAliasEnum.Picture:
					return AxWrapperGen.ComAliasConverter.GetGuid(t).Equals(AxWrapperGen.ComAliasConverter.Guid_IPicture);
				case AxWrapperGen.ComAliasEnum.PictureDisp:
					return AxWrapperGen.ComAliasConverter.GetGuid(t).Equals(AxWrapperGen.ComAliasConverter.Guid_IPictureDisp);
				default:
					return false;
				}
			}

			// Token: 0x04000FD5 RID: 4053
			private static Guid Guid_IPicture = new Guid("{7BF80980-BF32-101A-8BBB-00AA00300CAB}");

			// Token: 0x04000FD6 RID: 4054
			private static Guid Guid_IPictureDisp = new Guid("{7BF80981-BF32-101A-8BBB-00AA00300CAB}");

			// Token: 0x04000FD7 RID: 4055
			private static Guid Guid_IFont = new Guid("{BEF6E002-A874-101A-8BBA-00AA00300CAB}");

			// Token: 0x04000FD8 RID: 4056
			private static Guid Guid_IFontDisp = new Guid("{BEF6E003-A874-101A-8BBA-00AA00300CAB}");

			// Token: 0x04000FD9 RID: 4057
			private static Hashtable typeGuids;
		}

		// Token: 0x02000197 RID: 407
		private class EventEntry
		{
			// Token: 0x06000F3F RID: 3903 RVA: 0x0004231C File Offset: 0x0004131C
			public EventEntry(string eventName, string eventCls, string eventHandlerCls, Type retType, AxParameterData[] parameters, bool conflict)
			{
				this.eventName = eventName;
				this.eventCls = eventCls;
				this.eventHandlerCls = eventHandlerCls;
				this.retType = retType;
				this.parameters = parameters;
				this.eventParam = eventName.ToLower(CultureInfo.InvariantCulture) + "Event";
				this.resovledEventName = (conflict ? (eventName + "Event") : eventName);
				this.invokeMethodName = "RaiseOn" + this.resovledEventName;
				this.eventFlags = (MemberAttributes)24578;
			}

			// Token: 0x04000FDA RID: 4058
			public string eventName;

			// Token: 0x04000FDB RID: 4059
			public string resovledEventName;

			// Token: 0x04000FDC RID: 4060
			public string eventCls;

			// Token: 0x04000FDD RID: 4061
			public string eventHandlerCls;

			// Token: 0x04000FDE RID: 4062
			public Type retType;

			// Token: 0x04000FDF RID: 4063
			public AxParameterData[] parameters;

			// Token: 0x04000FE0 RID: 4064
			public string eventParam;

			// Token: 0x04000FE1 RID: 4065
			public string invokeMethodName;

			// Token: 0x04000FE2 RID: 4066
			public MemberAttributes eventFlags;
		}

		// Token: 0x02000198 RID: 408
		private class AxMethodGenerator
		{
			// Token: 0x06000F40 RID: 3904 RVA: 0x000423A8 File Offset: 0x000413A8
			internal AxMethodGenerator(MethodInfo method, bool removeOpts)
			{
				this._method = method;
				this._removeOptionals = removeOpts;
			}

			// Token: 0x1700026D RID: 621
			// (get) Token: 0x06000F41 RID: 3905 RVA: 0x000423BE File Offset: 0x000413BE
			// (set) Token: 0x06000F42 RID: 3906 RVA: 0x000423C6 File Offset: 0x000413C6
			public Type ControlType
			{
				get
				{
					return this._controlType;
				}
				set
				{
					this._controlType = value;
				}
			}

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x06000F43 RID: 3907 RVA: 0x000423D0 File Offset: 0x000413D0
			private AxParameterData[] Parameters
			{
				get
				{
					if (this._params == null && this._method != null)
					{
						this._params = AxParameterData.Convert(this._method.GetParameters());
						if (this._params == null)
						{
							this._params = new AxParameterData[0];
						}
					}
					return this._params;
				}
			}

			// Token: 0x06000F44 RID: 3908 RVA: 0x00042420 File Offset: 0x00041420
			public static AxWrapperGen.AxMethodGenerator Create(MethodInfo method, bool removeOptionals)
			{
				bool flag = removeOptionals && AxWrapperGen.AxMethodGenerator.NonPrimitiveOptionalsOrMissingPresent(method);
				if (flag)
				{
					return new AxWrapperGen.AxReflectionInvokeMethodGenerator(method, removeOptionals);
				}
				return new AxWrapperGen.AxMethodGenerator(method, removeOptionals);
			}

			// Token: 0x06000F45 RID: 3909 RVA: 0x0004244C File Offset: 0x0004144C
			public CodeMemberMethod CreateMethod(string methodName)
			{
				return new CodeMemberMethod
				{
					Name = methodName,
					Attributes = MemberAttributes.Public,
					ReturnType = new CodeTypeReference(AxWrapperGen.MapTypeName(this._method.ReturnType))
				};
			}

			// Token: 0x06000F46 RID: 3910 RVA: 0x00042490 File Offset: 0x00041490
			public List<CodeExpression> GenerateAndMarshalParameters(CodeMemberMethod method)
			{
				List<CodeExpression> list = new List<CodeExpression>();
				foreach (AxParameterData axParameterData in this.Parameters)
				{
					if (axParameterData.IsOptional && this._removeOptionals)
					{
						CodeExpression defaultExpressionForInvoke = AxWrapperGen.AxMethodGenerator.GetDefaultExpressionForInvoke(this._method, axParameterData);
						list.Add(defaultExpressionForInvoke);
					}
					else
					{
						Type parameterBaseType = axParameterData.ParameterBaseType;
						AxWrapperGen.ComAliasEnum comAliasEnum = AxWrapperGen.ComAliasConverter.GetComAliasEnum(this._method, parameterBaseType, null);
						CodeVariableReferenceExpression codeVariableReferenceExpression = new CodeVariableReferenceExpression(axParameterData.Name);
						codeVariableReferenceExpression.UserData[typeof(AxParameterData)] = axParameterData;
						if (comAliasEnum != AxWrapperGen.ComAliasEnum.None)
						{
							Type wftypeFromComType = AxWrapperGen.ComAliasConverter.GetWFTypeFromComType(parameterBaseType, comAliasEnum);
							CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(wftypeFromComType.FullName, axParameterData.Name);
							codeParameterDeclarationExpression.Direction = axParameterData.Direction;
							method.Parameters.Add(codeParameterDeclarationExpression);
							string wftoComConverter = AxWrapperGen.ComAliasConverter.GetWFToComConverter(comAliasEnum);
							CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(null, wftoComConverter, new CodeExpression[0]);
							codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression(axParameterData.Name));
							codeVariableReferenceExpression.UserData[AxWrapperGen.AxMethodGenerator.OriginalParamNameKey] = axParameterData.Name;
							codeVariableReferenceExpression.VariableName = "_" + axParameterData.Name;
							CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(parameterBaseType.FullName, codeVariableReferenceExpression.VariableName, new CodeCastExpression(parameterBaseType, codeMethodInvokeExpression));
							method.Statements.Add(codeVariableDeclarationStatement);
						}
						else
						{
							CodeParameterDeclarationExpression codeParameterDeclarationExpression2 = new CodeParameterDeclarationExpression(axParameterData.TypeName, axParameterData.Name);
							codeParameterDeclarationExpression2.Direction = axParameterData.Direction;
							method.Parameters.Add(codeParameterDeclarationExpression2);
						}
						list.Add(codeVariableReferenceExpression);
					}
				}
				return list;
			}

			// Token: 0x06000F47 RID: 3911 RVA: 0x0004262F File Offset: 0x0004162F
			public CodeExpression DoMethodInvoke(CodeMemberMethod method, string methodName, CodeExpression targetObject, List<CodeExpression> parameters)
			{
				return this.DoMethodInvokeCore(method, methodName, this._method.ReturnType, targetObject, parameters);
			}

			// Token: 0x06000F48 RID: 3912 RVA: 0x00042648 File Offset: 0x00041648
			public virtual CodeExpression DoMethodInvokeCore(CodeMemberMethod method, string methodName, Type returnType, CodeExpression targetObject, List<CodeExpression> parameters)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(targetObject, methodName, new CodeExpression[0]);
				foreach (CodeExpression codeExpression in parameters)
				{
					AxParameterData axParameterData = (AxParameterData)codeExpression.UserData[typeof(AxParameterData)];
					CodeExpression codeExpression2 = codeExpression;
					if (axParameterData != null)
					{
						codeExpression2 = new CodeDirectionExpression(axParameterData.Direction, codeExpression);
					}
					codeMethodInvokeExpression.Parameters.Add(codeExpression2);
				}
				if (returnType == typeof(void))
				{
					method.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
					return null;
				}
				CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(returnType, AxWrapperGen.AxMethodGenerator.ReturnValueVariableName, new CodeCastExpression(returnType, codeMethodInvokeExpression));
				method.Statements.Add(codeVariableDeclarationStatement);
				return new CodeVariableReferenceExpression(AxWrapperGen.AxMethodGenerator.ReturnValueVariableName);
			}

			// Token: 0x06000F49 RID: 3913 RVA: 0x00042728 File Offset: 0x00041728
			public void UnmarshalParameters(CodeMemberMethod method, List<CodeExpression> parameters)
			{
				foreach (CodeExpression codeExpression in parameters)
				{
					if (codeExpression is CodeVariableReferenceExpression)
					{
						AxParameterData axParameterData = (AxParameterData)codeExpression.UserData[typeof(AxParameterData)];
						string text = (string)codeExpression.UserData[AxWrapperGen.AxMethodGenerator.OriginalParamNameKey];
						if (axParameterData.Direction != FieldDirection.In && text != null)
						{
							CodeExpression codeExpression2 = new CodeVariableReferenceExpression(text);
							CodeExpression codeExpression3 = new CodeCastExpression(axParameterData.ParameterBaseType, codeExpression);
							AxWrapperGen.ComAliasEnum comAliasEnum = AxWrapperGen.ComAliasConverter.GetComAliasEnum(this._method, axParameterData.ParameterBaseType, null);
							if (comAliasEnum != AxWrapperGen.ComAliasEnum.None)
							{
								string comToManagedConverter = AxWrapperGen.ComAliasConverter.GetComToManagedConverter(comAliasEnum);
								codeExpression3 = new CodeMethodInvokeExpression(null, comToManagedConverter, new CodeExpression[0])
								{
									Parameters = { codeExpression3 }
								};
							}
							CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeExpression2, codeExpression3);
							method.Statements.Add(codeAssignStatement);
						}
					}
				}
			}

			// Token: 0x06000F4A RID: 3914 RVA: 0x00042830 File Offset: 0x00041830
			public void GenerateReturn(CodeMemberMethod method, CodeExpression returnExpression)
			{
				if (returnExpression == null)
				{
					return;
				}
				AxWrapperGen.ComAliasEnum comAliasEnum = AxWrapperGen.ComAliasConverter.GetComAliasEnum(this._method, this._method.ReturnType, this._method.ReturnTypeCustomAttributes);
				if (comAliasEnum != AxWrapperGen.ComAliasEnum.None)
				{
					string comToManagedConverter = AxWrapperGen.ComAliasConverter.GetComToManagedConverter(comAliasEnum);
					returnExpression = new CodeMethodInvokeExpression(null, comToManagedConverter, new CodeExpression[0])
					{
						Parameters = { returnExpression }
					};
					method.ReturnType = new CodeTypeReference(AxWrapperGen.ComAliasConverter.GetWFTypeFromComType(this._method.ReturnType, comAliasEnum));
				}
				method.Statements.Add(new CodeMethodReturnStatement(returnExpression));
			}

			// Token: 0x06000F4B RID: 3915 RVA: 0x000428BC File Offset: 0x000418BC
			private static bool NonPrimitiveOptionalsOrMissingPresent(MethodInfo method)
			{
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters != null && parameters.Length > 0)
				{
					for (int i = 0; i < parameters.Length; i++)
					{
						if (parameters[i].IsOptional && ((!parameters[i].ParameterType.IsPrimitive && !parameters[i].ParameterType.IsEnum) || parameters[i].DefaultValue == Missing.Value))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06000F4C RID: 3916 RVA: 0x00042924 File Offset: 0x00041924
			private static object GetClsPrimitiveValue(object value)
			{
				if (value is uint)
				{
					return Convert.ChangeType(value, typeof(int), CultureInfo.InvariantCulture);
				}
				if (value is ushort)
				{
					return Convert.ChangeType(value, typeof(short), CultureInfo.InvariantCulture);
				}
				if (value is ulong)
				{
					return Convert.ChangeType(value, typeof(long), CultureInfo.InvariantCulture);
				}
				if (value is sbyte)
				{
					return Convert.ChangeType(value, typeof(byte), CultureInfo.InvariantCulture);
				}
				return value;
			}

			// Token: 0x06000F4D RID: 3917 RVA: 0x000429AC File Offset: 0x000419AC
			private static object GetDefaultValueForUnsignedType(Type parameterType, object value)
			{
				if (parameterType == typeof(uint))
				{
					int num = 0;
					if (value is short)
					{
						num = (int)((short)value);
					}
					if (value is int)
					{
						num = (int)value;
					}
					if (value is long)
					{
						num = (int)value;
					}
					return Convert.ToUInt32(Convert.ToString(num, 16), 16);
				}
				if (parameterType == typeof(ushort))
				{
					short num2 = (short)value;
					return Convert.ToUInt16(Convert.ToString(num2, 16), 16);
				}
				if (parameterType == typeof(ulong))
				{
					long num3 = 0L;
					if (value is short)
					{
						num3 = (long)((short)value);
					}
					if (value is int)
					{
						num3 = (long)((int)value);
					}
					if (value is long)
					{
						num3 = (long)value;
					}
					return Convert.ToUInt64(Convert.ToString(num3, 16), 16);
				}
				return value;
			}

			// Token: 0x06000F4E RID: 3918 RVA: 0x00042A88 File Offset: 0x00041A88
			private static object GetPrimitiveDefaultValue(Type destType)
			{
				if (destType == typeof(IntPtr) || destType == typeof(UIntPtr))
				{
					return 0;
				}
				return AxWrapperGen.AxMethodGenerator.GetClsPrimitiveValue(Convert.ChangeType(0, destType, CultureInfo.InvariantCulture));
			}

			// Token: 0x06000F4F RID: 3919 RVA: 0x00042AC4 File Offset: 0x00041AC4
			private static CodeExpression GetDefaultExpressionForInvoke(MethodInfo method, AxParameterData parameterInfo)
			{
				object obj = parameterInfo.ParameterInfo.DefaultValue;
				Type type = parameterInfo.ParameterBaseType;
				if (obj == Missing.Value)
				{
					if (type.IsPrimitive)
					{
						obj = AxWrapperGen.AxMethodGenerator.GetPrimitiveDefaultValue(type);
					}
					else if (type.IsEnum)
					{
						obj = 0;
						if (!Enum.IsDefined(type, 0) && Enum.GetValues(type).Length > 0)
						{
							obj = Enum.GetValues(type).GetValue(0);
						}
					}
					else
					{
						if (type == typeof(object))
						{
							return new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, "System.Reflection.Missing"), "Value");
						}
						if (!type.IsValueType)
						{
							if (type == typeof(string))
							{
								obj = "";
							}
							else
							{
								obj = null;
							}
							type = null;
						}
						else
						{
							if (type.GetConstructor(new Type[0]) != null)
							{
								return new CodeObjectCreateExpression(type, new CodeExpression[0]);
							}
							if (type == typeof(decimal))
							{
								return new CodeObjectCreateExpression(typeof(decimal), new CodeExpression[]
								{
									new CodePrimitiveExpression(0.0)
								});
							}
							if (type == typeof(DateTime))
							{
								return new CodeObjectCreateExpression(typeof(DateTime), new CodeExpression[]
								{
									new CodePrimitiveExpression(0L)
								});
							}
							throw new Exception(SR.GetString("AxImpNoDefaultValue", new object[] { method.Name, parameterInfo.Name, type.FullName }));
						}
					}
				}
				else if (type.IsPrimitive)
				{
					obj = AxWrapperGen.AxMethodGenerator.GetClsPrimitiveValue(obj);
					obj = AxWrapperGen.AxMethodGenerator.GetDefaultValueForUnsignedType(type, obj);
				}
				else if (obj != null && type.IsInstanceOfType(obj) && (obj is DateTime || obj is decimal || obj is bool))
				{
					if (obj is DateTime)
					{
						return new CodeObjectCreateExpression(typeof(DateTime), new CodeExpression[]
						{
							new CodeCastExpression(typeof(long), new CodePrimitiveExpression(((DateTime)obj).Ticks))
						});
					}
					if (obj is decimal)
					{
						return new CodeObjectCreateExpression(typeof(decimal), new CodeExpression[]
						{
							new CodeCastExpression(typeof(double), new CodePrimitiveExpression(decimal.ToDouble((decimal)obj)))
						});
					}
					if (obj is bool)
					{
						return new CodePrimitiveExpression((bool)obj);
					}
					if (!(obj is string))
					{
						throw new Exception(SR.GetString("AxImpUnrecognizedDefaultValueType", new object[] { method.Name, parameterInfo.Name, type.FullName }));
					}
					type = null;
				}
				else if (!type.IsValueType)
				{
					if (obj is DispatchWrapper)
					{
						obj = null;
					}
					if (obj == null || obj is string)
					{
						return new CodePrimitiveExpression(obj);
					}
					throw new Exception(SR.GetString("AxImpUnrecognizedDefaultValueType", new object[] { method.Name, parameterInfo.Name, type.FullName }));
				}
				if (type != null && type.IsEnum)
				{
					obj = (int)obj;
				}
				CodeExpression codeExpression = new CodePrimitiveExpression(obj);
				if (type != null)
				{
					codeExpression = new CodeCastExpression(type, codeExpression);
				}
				return codeExpression;
			}

			// Token: 0x04000FE3 RID: 4067
			private MethodInfo _method;

			// Token: 0x04000FE4 RID: 4068
			private bool _removeOptionals;

			// Token: 0x04000FE5 RID: 4069
			private AxParameterData[] _params;

			// Token: 0x04000FE6 RID: 4070
			private Type _controlType;

			// Token: 0x04000FE7 RID: 4071
			protected static object OriginalParamNameKey = new object();

			// Token: 0x04000FE8 RID: 4072
			protected static string ReturnValueVariableName = "returnValue";
		}

		// Token: 0x02000199 RID: 409
		private class AxReflectionInvokeMethodGenerator : AxWrapperGen.AxMethodGenerator
		{
			// Token: 0x06000F51 RID: 3921 RVA: 0x00042E33 File Offset: 0x00041E33
			internal AxReflectionInvokeMethodGenerator(MethodInfo method, bool removeOpts)
				: base(method, removeOpts)
			{
			}

			// Token: 0x06000F52 RID: 3922 RVA: 0x00042E40 File Offset: 0x00041E40
			public override CodeExpression DoMethodInvokeCore(CodeMemberMethod method, string methodName, Type returnType, CodeExpression targetObject, List<CodeExpression> parameters)
			{
				CodeExpression[] array = parameters.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					CodeVariableReferenceExpression codeVariableReferenceExpression = array[i] as CodeVariableReferenceExpression;
					if (codeVariableReferenceExpression != null)
					{
						AxParameterData axParameterData = codeVariableReferenceExpression.UserData[typeof(AxParameterData)] as AxParameterData;
						if (axParameterData != null && axParameterData.Direction == FieldDirection.Out)
						{
							array[i] = new CodePrimitiveExpression(null);
						}
					}
				}
				CodeArrayCreateExpression codeArrayCreateExpression = new CodeArrayCreateExpression(typeof(object), array);
				CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(typeof(object[]), "paramArray", codeArrayCreateExpression);
				method.Statements.Add(codeVariableDeclarationStatement);
				CodeTypeOfExpression codeTypeOfExpression = new CodeTypeOfExpression(base.ControlType);
				CodeVariableDeclarationStatement codeVariableDeclarationStatement2 = new CodeVariableDeclarationStatement(typeof(Type), "typeVar", codeTypeOfExpression);
				method.Statements.Add(codeVariableDeclarationStatement2);
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("typeVar"), "GetMethod", new CodeExpression[]
				{
					new CodePrimitiveExpression(methodName)
				});
				CodeVariableDeclarationStatement codeVariableDeclarationStatement3 = new CodeVariableDeclarationStatement(typeof(MethodInfo), "methodToInvoke", codeMethodInvokeExpression);
				method.Statements.Add(codeVariableDeclarationStatement3);
				List<CodeExpression> list = new List<CodeExpression>();
				list.Add(targetObject);
				CodeVariableReferenceExpression codeVariableReferenceExpression2 = new CodeVariableReferenceExpression("paramArray");
				list.Add(codeVariableReferenceExpression2);
				CodeExpression codeExpression = base.DoMethodInvokeCore(method, "Invoke", returnType, new CodeVariableReferenceExpression("methodToInvoke"), list);
				for (int j = 0; j < parameters.Count; j++)
				{
					CodeVariableReferenceExpression codeVariableReferenceExpression3 = parameters[j] as CodeVariableReferenceExpression;
					if (codeVariableReferenceExpression3 != null)
					{
						AxParameterData axParameterData2 = codeVariableReferenceExpression3.UserData[typeof(AxParameterData)] as AxParameterData;
						if (axParameterData2 != null && axParameterData2.Direction != FieldDirection.In)
						{
							CodeExpression codeExpression2 = new CodeCastExpression(axParameterData2.TypeName, new CodeArrayIndexerExpression(codeVariableReferenceExpression2, new CodeExpression[]
							{
								new CodePrimitiveExpression(j)
							}));
							CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeVariableReferenceExpression3, codeExpression2);
							method.Statements.Add(codeAssignStatement);
						}
					}
				}
				return codeExpression;
			}
		}
	}
}
