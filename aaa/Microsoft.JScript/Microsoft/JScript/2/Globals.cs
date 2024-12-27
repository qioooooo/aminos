using System;
using System.Configuration.Assemblies;
using System.Globalization;
using System.Reflection;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200008A RID: 138
	public sealed class Globals
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0002D9FC File Offset: 0x0002C9FC
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x0002DA1F File Offset: 0x0002CA1F
		internal static TypeReferences TypeRefs
		{
			get
			{
				TypeReferences typeReferences = Globals._typeRefs;
				if (typeReferences == null)
				{
					typeReferences = (Globals._typeRefs = Runtime.TypeRefs);
				}
				return typeReferences;
			}
			set
			{
				Globals._typeRefs = value;
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002DA28 File Offset: 0x0002CA28
		internal Globals(bool fast, VsaEngine engine)
		{
			this.engine = engine;
			this.callContextStack = null;
			this.scopeStack = null;
			this.caller = DBNull.Value;
			this.regExpTable = null;
			if (fast)
			{
				this.globalObject = GlobalObject.commonInstance;
				return;
			}
			this.globalObject = new LenientGlobalObject(engine);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0002DA94 File Offset: 0x0002CA94
		internal static BuiltinFunction BuiltinFunctionFor(object obj, MethodInfo meth)
		{
			if (Globals.BuiltinFunctionTable == null)
			{
				Globals.BuiltinFunctionTable = new SimpleHashtable(64U);
			}
			BuiltinFunction builtinFunction = (BuiltinFunction)Globals.BuiltinFunctionTable[meth];
			if (builtinFunction != null)
			{
				return builtinFunction;
			}
			builtinFunction = new BuiltinFunction(obj, meth);
			lock (Globals.BuiltinFunctionTable)
			{
				Globals.BuiltinFunctionTable[meth] = builtinFunction;
			}
			return builtinFunction;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0002DB04 File Offset: 0x0002CB04
		internal Stack CallContextStack
		{
			get
			{
				if (this.callContextStack == null)
				{
					this.callContextStack = new Stack();
				}
				return this.callContextStack;
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002DB1F File Offset: 0x0002CB1F
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public static ArrayObject ConstructArray(params object[] args)
		{
			return (ArrayObject)ArrayConstructor.ob.Construct(args);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002DB31 File Offset: 0x0002CB31
		public static ArrayObject ConstructArrayLiteral(object[] args)
		{
			return ArrayConstructor.ob.ConstructArray(args);
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0002DB3E File Offset: 0x0002CB3E
		internal SimpleHashtable RegExpTable
		{
			get
			{
				if (this.regExpTable == null)
				{
					this.regExpTable = new SimpleHashtable(8U);
				}
				return this.regExpTable;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0002DB5A File Offset: 0x0002CB5A
		internal Stack ScopeStack
		{
			get
			{
				if (this.scopeStack == null)
				{
					this.scopeStack = new Stack();
					this.scopeStack.Push(this.engine.GetGlobalScope().GetObject());
				}
				return this.scopeStack;
			}
		}

		// Token: 0x040002DB RID: 731
		[ThreadStatic]
		private static TypeReferences _typeRefs;

		// Token: 0x040002DC RID: 732
		private Stack callContextStack;

		// Token: 0x040002DD RID: 733
		private Stack scopeStack;

		// Token: 0x040002DE RID: 734
		internal object caller;

		// Token: 0x040002DF RID: 735
		private SimpleHashtable regExpTable;

		// Token: 0x040002E0 RID: 736
		internal GlobalObject globalObject;

		// Token: 0x040002E1 RID: 737
		internal VsaEngine engine;

		// Token: 0x040002E2 RID: 738
		internal bool assemblyDelaySign;

		// Token: 0x040002E3 RID: 739
		internal CultureInfo assemblyCulture;

		// Token: 0x040002E4 RID: 740
		internal AssemblyFlags assemblyFlags = (AssemblyFlags)49152;

		// Token: 0x040002E5 RID: 741
		internal AssemblyHashAlgorithm assemblyHashAlgorithm = AssemblyHashAlgorithm.SHA1;

		// Token: 0x040002E6 RID: 742
		internal string assemblyKeyFileName;

		// Token: 0x040002E7 RID: 743
		internal Context assemblyKeyFileNameContext;

		// Token: 0x040002E8 RID: 744
		internal string assemblyKeyName;

		// Token: 0x040002E9 RID: 745
		internal Context assemblyKeyNameContext;

		// Token: 0x040002EA RID: 746
		internal Version assemblyVersion;

		// Token: 0x040002EB RID: 747
		internal AssemblyVersionCompatibility assemblyVersionCompatibility;

		// Token: 0x040002EC RID: 748
		private static SimpleHashtable BuiltinFunctionTable;

		// Token: 0x040002ED RID: 749
		[ContextStatic]
		public static VsaEngine contextEngine;
	}
}
