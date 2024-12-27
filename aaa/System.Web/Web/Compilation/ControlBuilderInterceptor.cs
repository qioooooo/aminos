using System;
using System.CodeDom;
using System.Collections;
using System.Reflection;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200016A RID: 362
	internal class ControlBuilderInterceptor
	{
		// Token: 0x06001047 RID: 4167 RVA: 0x000489AC File Offset: 0x000479AC
		internal static ControlBuilderInterceptor WrapImplementingObject(object cbiImplementation)
		{
			if (cbiImplementation == null)
			{
				return null;
			}
			if (!ControlBuilderInterceptor._initialized)
			{
				lock (ControlBuilderInterceptor._lockObject)
				{
					if (!ControlBuilderInterceptor._initialized)
					{
						Type type = cbiImplementation.GetType();
						MethodInfo method = type.GetMethod("PreControlBuilderInit", BindingFlags.Instance | BindingFlags.Public, null, ControlBuilderInterceptor.preInitArgs, null);
						MethodInfo method2 = type.GetMethod("OnProcessGeneratedCode", BindingFlags.Instance | BindingFlags.Public, null, ControlBuilderInterceptor.processCodeArgs, null);
						if (method == null)
						{
							throw new MissingMethodException(type.Name, "PreControlBuilderInit");
						}
						if (method2 == null)
						{
							throw new MissingMethodException(type.Name, "OnProcessGeneratedCode");
						}
						ControlBuilderInterceptor._preInit = method;
						ControlBuilderInterceptor._processCode = method2;
						ControlBuilderInterceptor._initialized = true;
					}
				}
			}
			return new ControlBuilderInterceptor(cbiImplementation);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00048A6C File Offset: 0x00047A6C
		private ControlBuilderInterceptor(object instance)
		{
			this._imp = instance;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00048A7C File Offset: 0x00047A7C
		internal void PreControlBuilderInit(ControlBuilder controlBuilder, TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attributes, IDictionary additionalState)
		{
			ControlBuilderInterceptor._preInit.Invoke(this._imp, new object[] { controlBuilder, parser, parentBuilder, type, tagName, id, attributes, additionalState });
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00048AC8 File Offset: 0x00047AC8
		internal void OnProcessGeneratedCode(ControlBuilder controlBuilder, CodeCompileUnit codeCompileUnit, CodeTypeDeclaration baseType, CodeTypeDeclaration derivedType, CodeMemberMethod buildMethod, CodeMemberMethod dataBindingMethod, IDictionary additionalState)
		{
			ControlBuilderInterceptor._processCode.Invoke(this._imp, new object[] { controlBuilder, codeCompileUnit, baseType, derivedType, buildMethod, dataBindingMethod, additionalState });
		}

		// Token: 0x04001643 RID: 5699
		private static readonly Type[] preInitArgs = new Type[]
		{
			typeof(ControlBuilder),
			typeof(TemplateParser),
			typeof(ControlBuilder),
			typeof(Type),
			typeof(string),
			typeof(string),
			typeof(IDictionary),
			typeof(IDictionary)
		};

		// Token: 0x04001644 RID: 5700
		private static readonly Type[] processCodeArgs = new Type[]
		{
			typeof(ControlBuilder),
			typeof(CodeCompileUnit),
			typeof(CodeTypeDeclaration),
			typeof(CodeTypeDeclaration),
			typeof(CodeMemberMethod),
			typeof(CodeMemberMethod),
			typeof(IDictionary)
		};

		// Token: 0x04001645 RID: 5701
		private static object _lockObject = new object();

		// Token: 0x04001646 RID: 5702
		private static volatile bool _initialized = false;

		// Token: 0x04001647 RID: 5703
		private static MethodInfo _preInit = null;

		// Token: 0x04001648 RID: 5704
		private static MethodInfo _processCode = null;

		// Token: 0x04001649 RID: 5705
		private object _imp;
	}
}
