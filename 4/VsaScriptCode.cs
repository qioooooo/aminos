using System;
using System.CodeDom;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000142 RID: 322
	internal class VsaScriptCode : VsaItem, IVsaScriptCodeItem, IVsaCodeItem, IVsaItem, IDebugVsaScriptCodeItem
	{
		// Token: 0x06000EB4 RID: 3764 RVA: 0x00063898 File Offset: 0x00062898
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal VsaScriptCode(VsaEngine engine, string itemName, VsaItemType type, IVsaScriptScope scope)
			: base(engine, itemName, type, VsaItemFlag.None)
		{
			this.binaryCode = null;
			this.executed = false;
			this.scope = (VsaScriptScope)scope;
			this.codeContext = new Context(new DocumentContext(this), null);
			this.compiledBlock = null;
			this.compileToIL = true;
			this.optimize = true;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x000638F1 File Offset: 0x000628F1
		internal override void Close()
		{
			base.Close();
			this.binaryCode = null;
			this.scope = null;
			this.codeContext = null;
			this.compiledBlock = null;
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x00063915 File Offset: 0x00062915
		public CodeObject CodeDOM
		{
			get
			{
				throw new VsaException(VsaError.CodeDOMNotAvailable);
			}
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00063924 File Offset: 0x00062924
		internal override void Compile()
		{
			if (this.binaryCode == null)
			{
				JSParser jsparser = new JSParser(this.codeContext);
				if (base.ItemType == (VsaItemType)22)
				{
					this.binaryCode = jsparser.ParseExpressionItem();
				}
				else
				{
					this.binaryCode = jsparser.Parse();
				}
				if (this.optimize && !jsparser.HasAborted)
				{
					this.binaryCode.ProcessAssemblyAttributeLists();
					this.binaryCode.PartiallyEvaluate();
				}
				if (this.engine.HasErrors && !this.engine.alwaysGenerateIL)
				{
					throw new EndOfFile();
				}
				if (this.compileToIL)
				{
					this.compiledBlock = this.binaryCode.TranslateToILClass(this.engine.CompilerGlobals).CreateType();
				}
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x000639DC File Offset: 0x000629DC
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual object Execute()
		{
			if (!this.engine.IsRunning)
			{
				throw new VsaException(VsaError.EngineNotRunning);
			}
			this.engine.Globals.ScopeStack.Push((ScriptObject)this.scope.GetObject());
			object obj;
			try
			{
				this.Compile();
				obj = this.RunCode();
			}
			finally
			{
				this.engine.Globals.ScopeStack.Pop();
			}
			return obj;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00063A60 File Offset: 0x00062A60
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual object Evaluate()
		{
			return this.Execute();
		}

		// Token: 0x170003CA RID: 970
		// (set) Token: 0x06000EBA RID: 3770 RVA: 0x00063A68 File Offset: 0x00062A68
		public override string Name
		{
			set
			{
				this.name = value;
				if (this.codebase == null)
				{
					string rootMoniker = this.engine.RootMoniker;
					this.codeContext.document.documentName = rootMoniker + (rootMoniker.EndsWith("/", StringComparison.Ordinal) ? "" : "/") + this.name;
				}
			}
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00063AC8 File Offset: 0x00062AC8
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual bool ParseNamedBreakPoint(string input, out string functionName, out int nargs, out string arguments, out string returnType, out ulong offset)
		{
			functionName = "";
			nargs = 0;
			arguments = "";
			returnType = "";
			offset = 0UL;
			JSParser jsparser = new JSParser(this.codeContext);
			string[] array = jsparser.ParseNamedBreakpoint(out nargs);
			if (array == null || array.Length != 4)
			{
				return false;
			}
			if (array[0] != null)
			{
				functionName = array[0];
			}
			if (array[1] != null)
			{
				arguments = array[1];
			}
			if (array[2] != null)
			{
				returnType = array[2];
			}
			if (array[3] != null)
			{
				offset = ((IConvertible)Convert.LiteralToNumber(array[3])).ToUInt64(null);
			}
			return true;
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00063B4F File Offset: 0x00062B4F
		internal override Type GetCompiledType()
		{
			return this.compiledBlock;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00063B57 File Offset: 0x00062B57
		public override object GetOption(string name)
		{
			if (string.Compare(name, "il", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.compileToIL;
			}
			if (string.Compare(name, "optimize", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.optimize;
			}
			return base.GetOption(name);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00063B94 File Offset: 0x00062B94
		internal override void Reset()
		{
			this.binaryCode = null;
			this.compiledBlock = null;
			this.executed = false;
			this.codeContext = new Context(new DocumentContext(this), this.codeContext.source_string);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00063BC7 File Offset: 0x00062BC7
		internal override void Run()
		{
			if (!this.executed)
			{
				this.RunCode();
			}
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00063BD8 File Offset: 0x00062BD8
		private object RunCode()
		{
			if (this.binaryCode != null)
			{
				object obj = null;
				if (this.compiledBlock != null)
				{
					GlobalScope globalScope = (GlobalScope)Activator.CreateInstance(this.compiledBlock, new object[] { this.scope.GetObject() });
					this.scope.ReRun(globalScope);
					MethodInfo method = this.compiledBlock.GetMethod("Global Code");
					try
					{
						CallContext.SetData("JScript:" + this.compiledBlock.Assembly.FullName, this.engine);
						obj = method.Invoke(globalScope, null);
						goto IL_00A0;
					}
					catch (TargetInvocationException ex)
					{
						throw ex.InnerException;
					}
				}
				obj = this.binaryCode.Evaluate();
				IL_00A0:
				this.executed = true;
				return obj;
			}
			return null;
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00063CA0 File Offset: 0x00062CA0
		public IVsaScriptScope Scope
		{
			get
			{
				return this.scope;
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00063CA8 File Offset: 0x00062CA8
		public override void SetOption(string name, object value)
		{
			if (string.Compare(name, "il", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.compileToIL = (bool)value;
				if (this.compileToIL)
				{
					this.optimize = true;
					return;
				}
			}
			else if (string.Compare(name, "optimize", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.optimize = (bool)value;
				if (!this.optimize)
				{
					this.compileToIL = false;
					return;
				}
			}
			else
			{
				if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.codebase = (string)value;
					this.codeContext.document.documentName = this.codebase;
					return;
				}
				base.SetOption(name, value);
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x00063D42 File Offset: 0x00062D42
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x00063D45 File Offset: 0x00062D45
		public object SourceContext
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00063D47 File Offset: 0x00062D47
		public void AddEventSource(string EventSourceName, string EventSourceType)
		{
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00063D49 File Offset: 0x00062D49
		public void RemoveEventSource(string EventSourceName)
		{
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00063D4B File Offset: 0x00062D4B
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x00063D58 File Offset: 0x00062D58
		public string SourceText
		{
			get
			{
				return this.codeContext.source_string;
			}
			set
			{
				this.codeContext.SetSourceContext(this.codeContext.document, (value == null) ? "" : value);
				this.executed = false;
				this.binaryCode = null;
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00063D8C File Offset: 0x00062D8C
		public void AppendSourceText(string SourceCode)
		{
			if (SourceCode == null || SourceCode.Length == 0)
			{
				return;
			}
			this.codeContext.SetSourceContext(this.codeContext.document, this.codeContext.source_string + SourceCode);
			this.executed = false;
			this.binaryCode = null;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x00063DDA File Offset: 0x00062DDA
		// (set) Token: 0x06000ECB RID: 3787 RVA: 0x00063DEC File Offset: 0x00062DEC
		public int StartColumn
		{
			get
			{
				return this.codeContext.document.startCol;
			}
			set
			{
				this.codeContext.document.startCol = value;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00063DFF File Offset: 0x00062DFF
		// (set) Token: 0x06000ECD RID: 3789 RVA: 0x00063E11 File Offset: 0x00062E11
		public int StartLine
		{
			get
			{
				return this.codeContext.document.startLine;
			}
			set
			{
				this.codeContext.document.startLine = value;
			}
		}

		// Token: 0x040007E0 RID: 2016
		private Context codeContext;

		// Token: 0x040007E1 RID: 2017
		private ScriptBlock binaryCode;

		// Token: 0x040007E2 RID: 2018
		internal bool executed;

		// Token: 0x040007E3 RID: 2019
		private VsaScriptScope scope;

		// Token: 0x040007E4 RID: 2020
		private Type compiledBlock;

		// Token: 0x040007E5 RID: 2021
		private bool compileToIL;

		// Token: 0x040007E6 RID: 2022
		private bool optimize;
	}
}
