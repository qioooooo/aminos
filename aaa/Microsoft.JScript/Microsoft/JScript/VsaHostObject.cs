using System;
using System.Reflection;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200013E RID: 318
	internal sealed class VsaHostObject : VsaItem, IVsaGlobalItem, IVsaItem
	{
		// Token: 0x06000E83 RID: 3715 RVA: 0x000624FD File Offset: 0x000614FD
		internal VsaHostObject(VsaEngine engine, string itemName, VsaItemType type)
			: this(engine, itemName, type, null)
		{
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0006250C File Offset: 0x0006150C
		internal VsaHostObject(VsaEngine engine, string itemName, VsaItemType type, VsaScriptScope scope)
			: base(engine, itemName, type, VsaItemFlag.None)
		{
			this.hostObject = null;
			this.exposeMembers = false;
			this.isVisible = false;
			this.exposed = false;
			this.compiled = false;
			this.scope = scope;
			this.field = null;
			this.typeString = "System.Object";
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x00062560 File Offset: 0x00061560
		// (set) Token: 0x06000E86 RID: 3718 RVA: 0x0006257B File Offset: 0x0006157B
		public bool ExposeMembers
		{
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				return this.exposeMembers;
			}
			set
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				this.exposeMembers = value;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x00062598 File Offset: 0x00061598
		internal FieldInfo Field
		{
			get
			{
				JSVariableField jsvariableField = this.field as JSVariableField;
				if (jsvariableField != null)
				{
					return (FieldInfo)jsvariableField.GetMetaData();
				}
				return this.field;
			}
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x000625C8 File Offset: 0x000615C8
		public object GetObject()
		{
			if (this.engine == null)
			{
				throw new VsaException(VsaError.EngineClosed);
			}
			if (this.hostObject == null)
			{
				if (this.engine.Site == null)
				{
					throw new VsaException(VsaError.SiteNotSet);
				}
				this.hostObject = this.engine.Site.GetGlobalInstance(this.name);
			}
			return this.hostObject;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0006262C File Offset: 0x0006162C
		private void AddNamedItemNamespace()
		{
			GlobalScope globalScope = (GlobalScope)this.Scope.GetObject();
			if (globalScope.isComponentScope)
			{
				globalScope = (GlobalScope)globalScope.GetParent();
			}
			ScriptObject parent = globalScope.GetParent();
			VsaNamedItemScope vsaNamedItemScope = new VsaNamedItemScope(this.GetObject(), parent, this.engine);
			globalScope.SetParent(vsaNamedItemScope);
			vsaNamedItemScope.SetParent(parent);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00062688 File Offset: 0x00061688
		private void RemoveNamedItemNamespace()
		{
			ScriptObject scriptObject = (ScriptObject)this.Scope.GetObject();
			for (ScriptObject scriptObject2 = scriptObject.GetParent(); scriptObject2 != null; scriptObject2 = scriptObject2.GetParent())
			{
				if (scriptObject2 is VsaNamedItemScope && ((VsaNamedItemScope)scriptObject2).namedItem == this.hostObject)
				{
					scriptObject.SetParent(scriptObject2.GetParent());
					return;
				}
				scriptObject = scriptObject2;
			}
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x000626E4 File Offset: 0x000616E4
		internal override void Remove()
		{
			base.Remove();
			if (this.exposed)
			{
				if (this.exposeMembers)
				{
					this.RemoveNamedItemNamespace();
				}
				if (this.isVisible)
				{
					((ScriptObject)this.Scope.GetObject()).DeleteMember(this.name);
				}
				this.hostObject = null;
				this.exposed = false;
			}
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0006273F File Offset: 0x0006173F
		internal override void CheckForErrors()
		{
			this.Compile();
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00062748 File Offset: 0x00061748
		internal override void Compile()
		{
			if (!this.compiled && this.isVisible)
			{
				ActivationObject activationObject = (ActivationObject)this.Scope.GetObject();
				JSVariableField jsvariableField = activationObject.AddFieldOrUseExistingField(this.name, null, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
				Type type = this.engine.GetType(this.typeString);
				if (type != null)
				{
					jsvariableField.type = new TypeExpression(new ConstantWrapper(type, null));
				}
				this.field = jsvariableField;
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x000627B4 File Offset: 0x000617B4
		internal override void Run()
		{
			if (!this.exposed)
			{
				if (this.isVisible)
				{
					ActivationObject activationObject = (ActivationObject)this.Scope.GetObject();
					this.field = activationObject.AddFieldOrUseExistingField(this.name, this.GetObject(), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
				}
				if (this.exposeMembers)
				{
					this.AddNamedItemNamespace();
				}
				this.exposed = true;
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00062814 File Offset: 0x00061814
		internal void ReRun(GlobalScope scope)
		{
			if (this.field is JSGlobalField)
			{
				((JSGlobalField)this.field).ILField = scope.GetField(this.name, BindingFlags.Static | BindingFlags.Public);
				this.field.SetValue(scope, this.GetObject());
				this.field = null;
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00062865 File Offset: 0x00061865
		internal override void Reset()
		{
			base.Reset();
			this.hostObject = null;
			this.exposed = false;
			this.compiled = false;
			this.scope = null;
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00062889 File Offset: 0x00061889
		private VsaScriptScope Scope
		{
			get
			{
				if (this.scope == null)
				{
					this.scope = (VsaScriptScope)this.engine.GetGlobalScope();
				}
				return this.scope;
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000628AF File Offset: 0x000618AF
		internal override void Close()
		{
			this.Remove();
			base.Close();
			this.hostObject = null;
			this.scope = null;
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x000628CB File Offset: 0x000618CB
		// (set) Token: 0x06000E94 RID: 3732 RVA: 0x000628E6 File Offset: 0x000618E6
		public string TypeString
		{
			get
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				return this.typeString;
			}
			set
			{
				if (this.engine == null)
				{
					throw new VsaException(VsaError.EngineClosed);
				}
				this.typeString = value;
				this.isDirty = true;
				this.engine.IsDirty = true;
			}
		}

		// Token: 0x040007CD RID: 1997
		private object hostObject;

		// Token: 0x040007CE RID: 1998
		internal bool exposeMembers;

		// Token: 0x040007CF RID: 1999
		internal bool isVisible;

		// Token: 0x040007D0 RID: 2000
		private bool exposed;

		// Token: 0x040007D1 RID: 2001
		private bool compiled;

		// Token: 0x040007D2 RID: 2002
		private VsaScriptScope scope;

		// Token: 0x040007D3 RID: 2003
		private FieldInfo field;

		// Token: 0x040007D4 RID: 2004
		private string typeString;
	}
}
