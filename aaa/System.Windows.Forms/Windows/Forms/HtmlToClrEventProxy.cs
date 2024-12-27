using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000439 RID: 1081
	internal class HtmlToClrEventProxy : IReflect
	{
		// Token: 0x060040DE RID: 16606 RVA: 0x000E9278 File Offset: 0x000E8278
		public HtmlToClrEventProxy(object sender, string eventName, EventHandler eventHandler)
		{
			this.eventHandler = eventHandler;
			this.eventName = eventName;
			Type typeFromHandle = typeof(HtmlToClrEventProxy);
			this.typeIReflectImplementation = typeFromHandle;
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x060040DF RID: 16607 RVA: 0x000E92AB File Offset: 0x000E82AB
		public string EventName
		{
			get
			{
				return this.eventName;
			}
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x000E92B3 File Offset: 0x000E82B3
		[DispId(0)]
		public void OnHtmlEvent()
		{
			this.InvokeClrEvent();
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x000E92BB File Offset: 0x000E82BB
		private void InvokeClrEvent()
		{
			if (this.eventHandler != null)
			{
				this.eventHandler(this.sender, EventArgs.Empty);
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x060040E2 RID: 16610 RVA: 0x000E92DB File Offset: 0x000E82DB
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return this.typeIReflectImplementation.UnderlyingSystemType;
			}
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x000E92E8 File Offset: 0x000E82E8
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetField(name, bindingAttr);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x000E92F7 File Offset: 0x000E82F7
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetFields(bindingAttr);
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x000E9305 File Offset: 0x000E8305
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetMember(name, bindingAttr);
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x000E9314 File Offset: 0x000E8314
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetMembers(bindingAttr);
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x000E9322 File Offset: 0x000E8322
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetMethod(name, bindingAttr);
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x000E9331 File Offset: 0x000E8331
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeIReflectImplementation.GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x000E9345 File Offset: 0x000E8345
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetMethods(bindingAttr);
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x000E9353 File Offset: 0x000E8353
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetProperties(bindingAttr);
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x000E9361 File Offset: 0x000E8361
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetProperty(name, bindingAttr);
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x000E9370 File Offset: 0x000E8370
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeIReflectImplementation.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x000E9388 File Offset: 0x000E8388
		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (name == "[DISPID=0]")
			{
				this.OnHtmlEvent();
				return null;
			}
			return this.typeIReflectImplementation.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x04001F66 RID: 8038
		private EventHandler eventHandler;

		// Token: 0x04001F67 RID: 8039
		private IReflect typeIReflectImplementation;

		// Token: 0x04001F68 RID: 8040
		private object sender;

		// Token: 0x04001F69 RID: 8041
		private string eventName;
	}
}
