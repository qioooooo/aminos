using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000038 RID: 56
	internal class XmlILModule
	{
		// Token: 0x06000346 RID: 838 RVA: 0x00015E5C File Offset: 0x00014E5C
		static XmlILModule()
		{
			XmlILModule.CreateModulePermissionSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit));
			XmlILModule.CreateModulePermissionSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
			XmlILModule.CreateModulePermissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.ControlEvidence));
			XmlILModule.AssemblyId = 0L;
			AssemblyName assemblyName = XmlILModule.CreateAssemblyName();
			AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			try
			{
				XmlILModule.CreateModulePermissionSet.Assert();
				assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(XmlILConstructors.Transparent, new object[0]));
				XmlILModule.LREModule = assemblyBuilder.DefineDynamicModule("System.Xml.Xsl.CompiledQuery", false);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00015F70 File Offset: 0x00014F70
		public XmlILModule(TypeBuilder typeBldr)
		{
			this.typeBldr = typeBldr;
			this.emitSymbols = ((ModuleBuilder)this.typeBldr.Module).GetSymWriter() != null;
			this.useLRE = false;
			this.persistAsm = false;
			this.methods = new Hashtable();
			if (this.emitSymbols)
			{
				this.urlToSymWriter = new Hashtable();
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00015FD8 File Offset: 0x00014FD8
		public XmlILModule(bool useLRE, bool emitSymbols)
		{
			this.useLRE = useLRE;
			this.emitSymbols = emitSymbols;
			this.persistAsm = false;
			this.methods = new Hashtable();
			if (!useLRE)
			{
				AssemblyName assemblyName = XmlILModule.CreateAssemblyName();
				if (XmlILTrace.IsEnabled)
				{
					this.modFile = "System.Xml.Xsl.CompiledQuery";
					this.persistAsm = true;
				}
				AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, this.persistAsm ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
				assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(XmlILConstructors.Transparent, new object[0]));
				if (emitSymbols)
				{
					this.urlToSymWriter = new Hashtable();
					DebuggableAttribute.DebuggingModes debuggingModes = DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints;
					assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(XmlILConstructors.Debuggable, new object[] { debuggingModes }));
				}
				ModuleBuilder moduleBuilder;
				if (this.persistAsm)
				{
					moduleBuilder = assemblyBuilder.DefineDynamicModule("System.Xml.Xsl.CompiledQuery", this.modFile + ".dll", emitSymbols);
				}
				else
				{
					moduleBuilder = assemblyBuilder.DefineDynamicModule("System.Xml.Xsl.CompiledQuery", emitSymbols);
				}
				this.typeBldr = moduleBuilder.DefineType("System.Xml.Xsl.CompiledQuery.Query", TypeAttributes.Public);
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000160DC File Offset: 0x000150DC
		public MethodInfo DefineMethod(string name, Type returnType, Type[] paramTypes, string[] paramNames, XmlILMethodAttributes xmlAttrs)
		{
			int num = 1;
			string text = name;
			bool flag = (xmlAttrs & XmlILMethodAttributes.Raw) != XmlILMethodAttributes.None;
			while (this.methods[name] != null)
			{
				num++;
				name = string.Concat(new object[] { text, " (", num, ")" });
			}
			if (!flag)
			{
				Type[] array = new Type[paramTypes.Length + 1];
				array[0] = typeof(XmlQueryRuntime);
				Array.Copy(paramTypes, 0, array, 1, paramTypes.Length);
				paramTypes = array;
			}
			MethodInfo methodInfo;
			if (!this.useLRE)
			{
				MethodBuilder methodBuilder = this.typeBldr.DefineMethod(name, MethodAttributes.Private | MethodAttributes.Static, returnType, paramTypes);
				if (this.emitSymbols && (xmlAttrs & XmlILMethodAttributes.NonUser) != XmlILMethodAttributes.None)
				{
					methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(XmlILConstructors.StepThrough, new object[0]));
					methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(XmlILConstructors.NonUserCode, new object[0]));
				}
				if (!flag)
				{
					methodBuilder.DefineParameter(1, ParameterAttributes.None, "{urn:schemas-microsoft-com:xslt-debug}runtime");
				}
				for (int i = 0; i < paramNames.Length; i++)
				{
					if (paramNames[i] != null && paramNames[i].Length != 0)
					{
						methodBuilder.DefineParameter(i + (flag ? 1 : 2), ParameterAttributes.None, paramNames[i]);
					}
				}
				methodInfo = methodBuilder;
			}
			else
			{
				DynamicMethod dynamicMethod = new DynamicMethod(name, returnType, paramTypes, XmlILModule.LREModule);
				dynamicMethod.InitLocals = true;
				if (!flag)
				{
					dynamicMethod.DefineParameter(1, ParameterAttributes.None, "{urn:schemas-microsoft-com:xslt-debug}runtime");
				}
				for (int j = 0; j < paramNames.Length; j++)
				{
					if (paramNames[j] != null && paramNames[j].Length != 0)
					{
						dynamicMethod.DefineParameter(j + (flag ? 1 : 2), ParameterAttributes.None, paramNames[j]);
					}
				}
				methodInfo = dynamicMethod;
			}
			this.methods[name] = methodInfo;
			return methodInfo;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00016294 File Offset: 0x00015294
		public static ILGenerator DefineMethodBody(MethodBase methInfo)
		{
			DynamicMethod dynamicMethod = methInfo as DynamicMethod;
			if (dynamicMethod != null)
			{
				return dynamicMethod.GetILGenerator();
			}
			MethodBuilder methodBuilder = methInfo as MethodBuilder;
			if (methodBuilder != null)
			{
				return methodBuilder.GetILGenerator();
			}
			return ((ConstructorBuilder)methInfo).GetILGenerator();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000162CE File Offset: 0x000152CE
		public MethodInfo FindMethod(string name)
		{
			return (MethodInfo)this.methods[name];
		}

		// Token: 0x0600034C RID: 844 RVA: 0x000162E1 File Offset: 0x000152E1
		public FieldInfo DefineInitializedData(string name, byte[] data)
		{
			return this.typeBldr.DefineInitializedData(name, data, FieldAttributes.Private | FieldAttributes.Static);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000162F2 File Offset: 0x000152F2
		public FieldInfo DefineField(string fieldName, Type type)
		{
			return this.typeBldr.DefineField(fieldName, type, FieldAttributes.Private | FieldAttributes.Static);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00016303 File Offset: 0x00015303
		public ConstructorInfo DefineTypeInitializer()
		{
			return this.typeBldr.DefineTypeInitializer();
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00016310 File Offset: 0x00015310
		public ISymbolDocumentWriter AddSourceDocument(string fileName)
		{
			ISymbolDocumentWriter symbolDocumentWriter = this.urlToSymWriter[fileName] as ISymbolDocumentWriter;
			if (symbolDocumentWriter == null)
			{
				symbolDocumentWriter = ((ModuleBuilder)this.typeBldr.Module).DefineDocument(fileName, XmlILModule.LanguageGuid, XmlILModule.VendorGuid, Guid.Empty);
				this.urlToSymWriter.Add(fileName, symbolDocumentWriter);
			}
			return symbolDocumentWriter;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00016368 File Offset: 0x00015368
		public void BakeMethods()
		{
			if (!this.useLRE)
			{
				Type type = this.typeBldr.CreateType();
				if (this.persistAsm)
				{
					((AssemblyBuilder)this.typeBldr.Module.Assembly).Save(this.modFile + ".dll");
				}
				Hashtable hashtable = new Hashtable(this.methods.Count);
				foreach (object obj in this.methods.Keys)
				{
					string text = (string)obj;
					hashtable[text] = type.GetMethod(text, BindingFlags.Static | BindingFlags.NonPublic);
				}
				this.methods = hashtable;
				this.typeBldr = null;
				this.urlToSymWriter = null;
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00016444 File Offset: 0x00015444
		public Delegate CreateDelegate(string name, Type typDelegate)
		{
			if (!this.useLRE)
			{
				return Delegate.CreateDelegate(typDelegate, (MethodInfo)this.methods[name]);
			}
			return ((DynamicMethod)this.methods[name]).CreateDelegate(typDelegate);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00016480 File Offset: 0x00015480
		private static AssemblyName CreateAssemblyName()
		{
			Interlocked.Increment(ref XmlILModule.AssemblyId);
			return new AssemblyName
			{
				Name = "System.Xml.Xsl.CompiledQuery." + XmlILModule.AssemblyId
			};
		}

		// Token: 0x040002C4 RID: 708
		private const string RuntimeName = "{urn:schemas-microsoft-com:xslt-debug}runtime";

		// Token: 0x040002C5 RID: 709
		public static readonly PermissionSet CreateModulePermissionSet = new PermissionSet(PermissionState.None);

		// Token: 0x040002C6 RID: 710
		private static long AssemblyId;

		// Token: 0x040002C7 RID: 711
		private static ModuleBuilder LREModule;

		// Token: 0x040002C8 RID: 712
		private TypeBuilder typeBldr;

		// Token: 0x040002C9 RID: 713
		private Hashtable methods;

		// Token: 0x040002CA RID: 714
		private Hashtable urlToSymWriter;

		// Token: 0x040002CB RID: 715
		private string modFile;

		// Token: 0x040002CC RID: 716
		private bool persistAsm;

		// Token: 0x040002CD RID: 717
		private bool useLRE;

		// Token: 0x040002CE RID: 718
		private bool emitSymbols;

		// Token: 0x040002CF RID: 719
		private static readonly Guid LanguageGuid = new Guid(1177373246U, 45655, 19182, 151, 205, 89, 24, 199, 83, 23, 88);

		// Token: 0x040002D0 RID: 720
		private static readonly Guid VendorGuid = new Guid(2571847108U, 59113, 4562, 144, 63, 0, 192, 79, 163, 2, 161);
	}
}
