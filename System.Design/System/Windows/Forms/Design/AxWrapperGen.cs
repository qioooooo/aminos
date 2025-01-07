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
	public class AxWrapperGen
	{
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

		private bool ClassAlreadyExistsInNamespace(CodeNamespace ns, string clsName)
		{
			return AxWrapperGen.classesInNamespace.Contains(clsName);
		}

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

		private string CreateDataSourceFieldName(string propName)
		{
			return "ax" + propName;
		}

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

		private void FillConflicatableThings()
		{
			if (this.conflictableThings == null)
			{
				this.conflictableThings = new Hashtable();
				this.conflictableThings.Add("System", "System");
			}
		}

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

		private bool IsDispidKnown(int dp, string propName)
		{
			return dp == -513 || dp == -501 || dp == -512 || dp == -514 || dp == -516 || dp == -611 || dp == -517 || dp == -515 || (dp == 0 && propName.Equals(this.defMember));
		}

		private bool IsEventPresent(MethodInfo mievent)
		{
			return false;
		}

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

		private CodeConstructor WriteConstructor(CodeTypeDeclaration cls)
		{
			CodeConstructor codeConstructor = new CodeConstructor();
			codeConstructor.Attributes = MemberAttributes.Public;
			codeConstructor.BaseConstructorArgs.Add(new CodeSnippetExpression("\"" + this.clsidAx.ToString() + "\""));
			cls.Members.Add(codeConstructor);
			return codeConstructor;
		}

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

		private void WriteProperty(CodeTypeDeclaration cls, PropertyInfo pinfo, bool useLet)
		{
			CodeAttributeDeclaration codeAttributeDeclaration = null;
			DispIdAttribute dispIdAttribute = null;
			if (AxWrapperGen.nopersist == null)
			{
				AxWrapperGen.nopersist = new CodeAttributeDeclaration("System.ComponentModel.DesignerSerializationVisibility", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, "System.ComponentModel.DesignerSerializationVisibility"), "Hidden"))
				});
				AxWrapperGen.nobrowse = new CodeAttributeDeclaration("System.ComponentModel.Browsable", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(false))
				});
				AxWrapperGen.browse = new CodeAttributeDeclaration("System.ComponentModel.Browsable", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(true))
				});
				AxWrapperGen.bindable = new CodeAttributeDeclaration("System.ComponentModel.Bindable", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, "System.ComponentModel.BindableSupport"), "Yes"))
				});
				AxWrapperGen.defaultBind = new CodeAttributeDeclaration("System.ComponentModel.Bindable", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, "System.ComponentModel.BindableSupport"), "Default"))
				});
			}
			pinfo.GetCustomAttributes(typeof(ComAliasNameAttribute), false);
			AxWrapperGen.ComAliasEnum comAliasEnum = AxWrapperGen.ComAliasConverter.GetComAliasEnum(pinfo, pinfo.PropertyType, pinfo);
			Type type = pinfo.PropertyType;
			if (comAliasEnum != AxWrapperGen.ComAliasEnum.None)
			{
				type = AxWrapperGen.ComAliasConverter.GetWFTypeFromComType(type, comAliasEnum);
			}
			bool flag = type.GUID.Equals(AxWrapperGen.Guid_DataSource);
			if (flag)
			{
				CodeMemberField codeMemberField = new CodeMemberField(type.FullName, this.CreateDataSourceFieldName(pinfo.Name));
				codeMemberField.Attributes = (MemberAttributes)20482;
				cls.Members.Add(codeMemberField);
				this.dataSourceProps.Add(pinfo);
			}
			object[] customAttributes = pinfo.GetCustomAttributes(typeof(DispIdAttribute), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				dispIdAttribute = (DispIdAttribute)customAttributes[0];
				codeAttributeDeclaration = new CodeAttributeDeclaration(typeof(DispIdAttribute).FullName, new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(dispIdAttribute.Value))
				});
			}
			bool flag2 = false;
			bool flag3 = false;
			string text = this.ResolveConflict(pinfo.Name, type, out flag2, out flag3);
			if (flag2 || flag3)
			{
				if (dispIdAttribute == null)
				{
					return;
				}
				if (!this.IsDispidKnown(dispIdAttribute.Value, pinfo.Name))
				{
					text = "Ctl";
					flag2 = false;
					flag3 = false;
				}
			}
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Type = new CodeTypeReference(AxWrapperGen.MapTypeName(type));
			codeMemberProperty.Name = text + pinfo.Name;
			codeMemberProperty.Attributes = MemberAttributes.Public;
			if (flag2)
			{
				codeMemberProperty.Attributes |= MemberAttributes.Override;
			}
			else if (flag3)
			{
				codeMemberProperty.Attributes |= MemberAttributes.New;
			}
			bool flag4 = false;
			bool flag5 = this.IsPropertyBrowsable(pinfo, comAliasEnum);
			bool flag6 = this.IsPropertyBindable(pinfo, out flag4);
			CodeAttributeDeclarationCollection codeAttributeDeclarationCollection;
			if (!flag5 || comAliasEnum == AxWrapperGen.ComAliasEnum.Handle)
			{
				codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection(new CodeAttributeDeclaration[]
				{
					AxWrapperGen.nobrowse,
					AxWrapperGen.nopersist,
					codeAttributeDeclaration
				});
			}
			else if (flag)
			{
				codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection(new CodeAttributeDeclaration[] { codeAttributeDeclaration });
			}
			else if (flag2 || flag3)
			{
				codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection(new CodeAttributeDeclaration[]
				{
					AxWrapperGen.browse,
					AxWrapperGen.nopersist,
					codeAttributeDeclaration
				});
			}
			else
			{
				codeAttributeDeclarationCollection = new CodeAttributeDeclarationCollection(new CodeAttributeDeclaration[]
				{
					AxWrapperGen.nopersist,
					codeAttributeDeclaration
				});
			}
			if (comAliasEnum != AxWrapperGen.ComAliasEnum.None)
			{
				CodeAttributeDeclaration codeAttributeDeclaration2 = new CodeAttributeDeclaration(typeof(ComAliasNameAttribute).FullName, new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(pinfo.PropertyType.FullName))
				});
				codeAttributeDeclarationCollection.Add(codeAttributeDeclaration2);
			}
			if (flag4)
			{
				codeAttributeDeclarationCollection.Add(AxWrapperGen.defaultBind);
			}
			else if (flag6)
			{
				codeAttributeDeclarationCollection.Add(AxWrapperGen.bindable);
			}
			codeMemberProperty.CustomAttributes = codeAttributeDeclarationCollection;
			AxParameterData[] array = AxParameterData.Convert(pinfo.GetIndexParameters());
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					CodeParameterDeclarationExpression codeParameterDeclarationExpression = this.CreateParamDecl(array[i].TypeName, array[i].Name, false);
					codeParameterDeclarationExpression.Direction = array[i].Direction;
					codeMemberProperty.Parameters.Add(codeParameterDeclarationExpression);
				}
			}
			bool flag7 = useLet;
			if (pinfo.CanWrite)
			{
				MethodInfo methodInfo;
				if (useLet)
				{
					methodInfo = pinfo.DeclaringType.GetMethod("let_" + pinfo.Name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				}
				else
				{
					methodInfo = pinfo.GetSetMethod();
				}
				Type parameterType = methodInfo.GetParameters()[0].ParameterType;
				Type elementType = parameterType.GetElementType();
				if (elementType != null && parameterType != elementType)
				{
					flag7 = true;
				}
			}
			if (pinfo.CanRead)
			{
				this.WritePropertyGetter(codeMemberProperty, pinfo, comAliasEnum, array, flag7, flag2, flag);
			}
			if (pinfo.CanWrite)
			{
				this.WritePropertySetter(codeMemberProperty, pinfo, comAliasEnum, array, flag7, flag2, useLet, flag);
			}
			if (array.Length > 0 && codeMemberProperty.Name != "Item")
			{
				CodeAttributeDeclaration codeAttributeDeclaration3 = new CodeAttributeDeclaration("System.Runtime.CompilerServices.IndexerName", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(codeMemberProperty.Name))
				});
				codeMemberProperty.Name = "Item";
				codeMemberProperty.CustomAttributes.Add(codeAttributeDeclaration3);
			}
			if (this.defMember != null && this.defMember.Equals(pinfo.Name))
			{
				CodeAttributeDeclaration codeAttributeDeclaration4 = new CodeAttributeDeclaration("System.ComponentModel.DefaultProperty", new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(new CodePrimitiveExpression(codeMemberProperty.Name))
				});
				cls.CustomAttributes.Add(codeAttributeDeclaration4);
			}
			cls.Members.Add(codeMemberProperty);
		}

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

		private string axctlIface;

		private Type axctlType;

		private Guid clsidAx;

		private string axctlEvents;

		private Type axctlEventsType;

		private string axctl;

		private static string axctlNS;

		private string memIface;

		private string multicaster;

		private string cookie;

		private bool dispInterface;

		private bool enumerableInterface;

		private string defMember;

		private string aboutBoxMethod;

		private CodeFieldReferenceExpression memIfaceRef;

		private CodeFieldReferenceExpression multicasterRef;

		private CodeFieldReferenceExpression cookieRef;

		private ArrayList events;

		public static ArrayList GeneratedSources = new ArrayList();

		private static Guid Guid_DataSource = new Guid("{7C0FFAB3-CD84-11D0-949A-00A0C91110ED}");

		internal static BooleanSwitch AxWrapper = new BooleanSwitch("AxWrapper", "ActiveX WFW wrapper generation.");

		internal static BooleanSwitch AxCodeGen = new BooleanSwitch("AxCodeGen", "ActiveX WFW property generation.");

		private static CodeAttributeDeclaration nobrowse = null;

		private static CodeAttributeDeclaration browse = null;

		private static CodeAttributeDeclaration nopersist = null;

		private static CodeAttributeDeclaration bindable = null;

		private static CodeAttributeDeclaration defaultBind = null;

		private Hashtable axctlTypeMembers;

		private Hashtable axHostMembers;

		private Hashtable conflictableThings;

		private static Hashtable classesInNamespace;

		private static Hashtable axHostPropDescs;

		private ArrayList dataSourceProps = new ArrayList();

		private enum ComAliasEnum
		{
			None,
			Color,
			Font,
			FontDisp,
			Handle,
			Picture,
			PictureDisp
		}

		private static class ComAliasConverter
		{
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

			public static string GetComToWFParamConverter(AxWrapperGen.ComAliasEnum alias)
			{
				if (alias == AxWrapperGen.ComAliasEnum.Color)
				{
					return typeof(uint).FullName;
				}
				return "";
			}

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

			public static string GetWFToComParamConverter(AxWrapperGen.ComAliasEnum alias, Type t)
			{
				return t.FullName;
			}

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

			public static bool IsFont(AxWrapperGen.ComAliasEnum e)
			{
				return e == AxWrapperGen.ComAliasEnum.Font || e == AxWrapperGen.ComAliasEnum.FontDisp;
			}

			public static bool IsPicture(AxWrapperGen.ComAliasEnum e)
			{
				return e == AxWrapperGen.ComAliasEnum.Picture || e == AxWrapperGen.ComAliasEnum.PictureDisp;
			}

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

			private static Guid Guid_IPicture = new Guid("{7BF80980-BF32-101A-8BBB-00AA00300CAB}");

			private static Guid Guid_IPictureDisp = new Guid("{7BF80981-BF32-101A-8BBB-00AA00300CAB}");

			private static Guid Guid_IFont = new Guid("{BEF6E002-A874-101A-8BBA-00AA00300CAB}");

			private static Guid Guid_IFontDisp = new Guid("{BEF6E003-A874-101A-8BBA-00AA00300CAB}");

			private static Hashtable typeGuids;
		}

		private class EventEntry
		{
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

			public string eventName;

			public string resovledEventName;

			public string eventCls;

			public string eventHandlerCls;

			public Type retType;

			public AxParameterData[] parameters;

			public string eventParam;

			public string invokeMethodName;

			public MemberAttributes eventFlags;
		}

		private class AxMethodGenerator
		{
			internal AxMethodGenerator(MethodInfo method, bool removeOpts)
			{
				this._method = method;
				this._removeOptionals = removeOpts;
			}

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

			public static AxWrapperGen.AxMethodGenerator Create(MethodInfo method, bool removeOptionals)
			{
				bool flag = removeOptionals && AxWrapperGen.AxMethodGenerator.NonPrimitiveOptionalsOrMissingPresent(method);
				if (flag)
				{
					return new AxWrapperGen.AxReflectionInvokeMethodGenerator(method, removeOptionals);
				}
				return new AxWrapperGen.AxMethodGenerator(method, removeOptionals);
			}

			public CodeMemberMethod CreateMethod(string methodName)
			{
				return new CodeMemberMethod
				{
					Name = methodName,
					Attributes = MemberAttributes.Public,
					ReturnType = new CodeTypeReference(AxWrapperGen.MapTypeName(this._method.ReturnType))
				};
			}

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

			public CodeExpression DoMethodInvoke(CodeMemberMethod method, string methodName, CodeExpression targetObject, List<CodeExpression> parameters)
			{
				return this.DoMethodInvokeCore(method, methodName, this._method.ReturnType, targetObject, parameters);
			}

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

			private static object GetPrimitiveDefaultValue(Type destType)
			{
				if (destType == typeof(IntPtr) || destType == typeof(UIntPtr))
				{
					return 0;
				}
				return AxWrapperGen.AxMethodGenerator.GetClsPrimitiveValue(Convert.ChangeType(0, destType, CultureInfo.InvariantCulture));
			}

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

			private MethodInfo _method;

			private bool _removeOptionals;

			private AxParameterData[] _params;

			private Type _controlType;

			protected static object OriginalParamNameKey = new object();

			protected static string ReturnValueVariableName = "returnValue";
		}

		private class AxReflectionInvokeMethodGenerator : AxWrapperGen.AxMethodGenerator
		{
			internal AxReflectionInvokeMethodGenerator(MethodInfo method, bool removeOpts)
				: base(method, removeOpts)
			{
			}

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
