using System;
using System.CodeDom;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000144 RID: 324
	internal class VsaStaticCode : VsaItem, IVsaCodeItem, IVsaItem
	{
		// Token: 0x06000EE0 RID: 3808 RVA: 0x00064576 File Offset: 0x00063576
		internal VsaStaticCode(VsaEngine engine, string itemName, VsaItemFlag flag)
			: base(engine, itemName, VsaItemType.Code, flag)
		{
			this.compiledClass = null;
			this.codeContext = new Context(new DocumentContext(this), "");
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0006459F File Offset: 0x0006359F
		public void AddEventSource(string eventSourceName, string eventSourceType)
		{
			if (this.engine == null)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			throw new NotSupportedException();
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x000645B9 File Offset: 0x000635B9
		public CodeObject CodeDOM
		{
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				throw new VsaException(VsaError.CodeDOMNotAvailable);
			}
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x000645D8 File Offset: 0x000635D8
		public void AppendSourceText(string SourceCode)
		{
			if (this.engine == null)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			if (SourceCode == null || SourceCode.Length == 0)
			{
				return;
			}
			this.codeContext.SetSourceContext(this.codeContext.document, this.codeContext.source_string + SourceCode);
			this.compiledClass = null;
			this.isDirty = true;
			this.engine.IsDirty = true;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00064648 File Offset: 0x00063648
		internal override void CheckForErrors()
		{
			if (this.compiledClass == null)
			{
				JSParser jsparser = new JSParser(this.codeContext);
				jsparser.Parse();
			}
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00064670 File Offset: 0x00063670
		internal override void Close()
		{
			base.Close();
			this.codeContext = null;
			this.compiledClass = null;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00064688 File Offset: 0x00063688
		internal override Type GetCompiledType()
		{
			TypeBuilder typeBuilder = this.compiledClass as TypeBuilder;
			if (typeBuilder != null)
			{
				this.compiledClass = typeBuilder.CreateType();
			}
			return this.compiledClass;
		}

		// Token: 0x170003D2 RID: 978
		// (set) Token: 0x06000EE7 RID: 3815 RVA: 0x000646B8 File Offset: 0x000636B8
		public override string Name
		{
			set
			{
				base.Name = value;
				if (this.codebase == null)
				{
					string rootMoniker = this.engine.RootMoniker;
					this.codeContext.document.documentName = rootMoniker + (rootMoniker.EndsWith("/", StringComparison.Ordinal) ? "" : "/") + this.name;
				}
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00064718 File Offset: 0x00063718
		internal void Parse()
		{
			if (this.block == null && this.compiledClass == null)
			{
				GlobalScope globalScope = (GlobalScope)this.engine.GetGlobalScope().GetObject();
				globalScope.evilScript = !globalScope.fast || this.engine.GetStaticCodeBlockCount() > 1;
				this.engine.Globals.ScopeStack.Push(globalScope);
				try
				{
					JSParser jsparser = new JSParser(this.codeContext);
					this.block = jsparser.Parse();
					if (jsparser.HasAborted)
					{
						this.block = null;
					}
				}
				finally
				{
					this.engine.Globals.ScopeStack.Pop();
				}
			}
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x000647D8 File Offset: 0x000637D8
		internal void ProcessAssemblyAttributeLists()
		{
			if (this.block == null)
			{
				return;
			}
			this.block.ProcessAssemblyAttributeLists();
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x000647F0 File Offset: 0x000637F0
		internal void PartiallyEvaluate()
		{
			if (this.block != null && this.compiledClass == null)
			{
				GlobalScope globalScope = (GlobalScope)this.engine.GetGlobalScope().GetObject();
				this.engine.Globals.ScopeStack.Push(globalScope);
				try
				{
					this.block.PartiallyEvaluate();
					if (this.engine.HasErrors && !this.engine.alwaysGenerateIL)
					{
						throw new EndOfFile();
					}
				}
				finally
				{
					this.engine.Globals.ScopeStack.Pop();
				}
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00064890 File Offset: 0x00063890
		internal override void Remove()
		{
			if (this.engine == null)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			base.Remove();
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x000648AB File Offset: 0x000638AB
		public void RemoveEventSource(string eventSourceName)
		{
			if (this.engine == null)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000648C5 File Offset: 0x000638C5
		internal override void Reset()
		{
			this.compiledClass = null;
			this.block = null;
			this.codeContext = new Context(new DocumentContext(this), this.codeContext.source_string);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x000648F4 File Offset: 0x000638F4
		internal override void Run()
		{
			if (this.compiledClass != null)
			{
				GlobalScope globalScope = (GlobalScope)Activator.CreateInstance(this.GetCompiledType(), new object[] { this.engine.GetGlobalScope().GetObject() });
				this.engine.Globals.ScopeStack.Push(globalScope);
				try
				{
					MethodInfo method = this.compiledClass.GetMethod("Global Code");
					try
					{
						method.Invoke(globalScope, null);
					}
					catch (TargetInvocationException ex)
					{
						throw ex.InnerException;
					}
				}
				finally
				{
					this.engine.Globals.ScopeStack.Pop();
				}
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000649A4 File Offset: 0x000639A4
		public override void SetOption(string name, object value)
		{
			if (this.engine == null)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.codebase = (string)value;
				this.codeContext.document.documentName = this.codebase;
				this.isDirty = true;
				this.engine.IsDirty = true;
				return;
			}
			throw new VsaException(VsaError.OptionNotSupported);
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x00064A14 File Offset: 0x00063A14
		// (set) Token: 0x06000EF1 RID: 3825 RVA: 0x00064A17 File Offset: 0x00063A17
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

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x00064A19 File Offset: 0x00063A19
		// (set) Token: 0x06000EF3 RID: 3827 RVA: 0x00064A3C File Offset: 0x00063A3C
		public string SourceText
		{
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				return this.codeContext.source_string;
			}
			set
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				this.codeContext.SetSourceContext(this.codeContext.document, (value == null) ? "" : value);
				this.compiledClass = null;
				this.isDirty = true;
				this.engine.IsDirty = true;
			}
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00064A98 File Offset: 0x00063A98
		internal void TranslateToIL()
		{
			if (this.block != null && this.compiledClass == null)
			{
				GlobalScope globalScope = (GlobalScope)this.engine.GetGlobalScope().GetObject();
				this.engine.Globals.ScopeStack.Push(globalScope);
				try
				{
					this.compiledClass = this.block.TranslateToILClass(this.engine.CompilerGlobals, false);
				}
				finally
				{
					this.engine.Globals.ScopeStack.Pop();
				}
			}
		}

		// Token: 0x040007EC RID: 2028
		internal Context codeContext;

		// Token: 0x040007ED RID: 2029
		private Type compiledClass;

		// Token: 0x040007EE RID: 2030
		private ScriptBlock block;
	}
}
