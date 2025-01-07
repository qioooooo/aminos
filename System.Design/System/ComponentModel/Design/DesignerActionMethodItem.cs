﻿using System;
using System.Design;
using System.Reflection;

namespace System.ComponentModel.Design
{
	public class DesignerActionMethodItem : DesignerActionItem
	{
		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, string description, bool includeAsDesignerVerb)
			: base(displayName, category, description)
		{
			this.actionList = actionList;
			this.memberName = memberName;
			this.includeAsDesignerVerb = includeAsDesignerVerb;
		}

		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName)
			: this(actionList, memberName, displayName, null, null, false)
		{
		}

		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, bool includeAsDesignerVerb)
			: this(actionList, memberName, displayName, null, null, includeAsDesignerVerb)
		{
		}

		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category)
			: this(actionList, memberName, displayName, category, null, false)
		{
		}

		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, bool includeAsDesignerVerb)
			: this(actionList, memberName, displayName, category, null, includeAsDesignerVerb)
		{
		}

		public DesignerActionMethodItem(DesignerActionList actionList, string memberName, string displayName, string category, string description)
			: this(actionList, memberName, displayName, category, description, false)
		{
		}

		internal DesignerActionMethodItem()
		{
		}

		public virtual string MemberName
		{
			get
			{
				return this.memberName;
			}
		}

		public IComponent RelatedComponent
		{
			get
			{
				return this.relatedComponent;
			}
			set
			{
				this.relatedComponent = value;
			}
		}

		public virtual bool IncludeAsDesignerVerb
		{
			get
			{
				return this.includeAsDesignerVerb;
			}
		}

		internal void Invoke(object sender, EventArgs args)
		{
			this.Invoke();
		}

		public virtual void Invoke()
		{
			if (this.methodInfo == null)
			{
				this.methodInfo = this.actionList.GetType().GetMethod(this.memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			}
			if (this.methodInfo != null)
			{
				this.methodInfo.Invoke(this.actionList, null);
				return;
			}
			throw new InvalidOperationException(SR.GetString("DesignerActionPanel_CouldNotFindMethod", new object[] { this.MemberName }));
		}

		private string memberName;

		private bool includeAsDesignerVerb;

		private DesignerActionList actionList;

		private MethodInfo methodInfo;

		private IComponent relatedComponent;
	}
}
