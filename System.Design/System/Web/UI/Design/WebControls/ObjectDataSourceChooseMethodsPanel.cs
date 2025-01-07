using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Reflection;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class ObjectDataSourceChooseMethodsPanel : WizardPanel
	{
		public ObjectDataSourceChooseMethodsPanel(ObjectDataSourceDesigner objectDataSourceDesigner)
		{
			this._objectDataSourceDesigner = objectDataSourceDesigner;
			this.InitializeComponent();
			this.InitializeUI();
			this._objectDataSource = (ObjectDataSource)this._objectDataSourceDesigner.Component;
		}

		private Type DeleteMethodDataObjectType
		{
			get
			{
				return this._deleteObjectDataSourceMethodEditor.DataObjectType;
			}
		}

		private MethodInfo DeleteMethodInfo
		{
			get
			{
				return this._deleteObjectDataSourceMethodEditor.MethodInfo;
			}
		}

		private Type InsertMethodDataObjectType
		{
			get
			{
				return this._insertObjectDataSourceMethodEditor.DataObjectType;
			}
		}

		private MethodInfo InsertMethodInfo
		{
			get
			{
				return this._insertObjectDataSourceMethodEditor.MethodInfo;
			}
		}

		private MethodInfo SelectMethodInfo
		{
			get
			{
				return this._selectObjectDataSourceMethodEditor.MethodInfo;
			}
		}

		private Type UpdateMethodDataObjectType
		{
			get
			{
				return this._updateObjectDataSourceMethodEditor.DataObjectType;
			}
		}

		private MethodInfo UpdateMethodInfo
		{
			get
			{
				return this._updateObjectDataSourceMethodEditor.MethodInfo;
			}
		}

		private void InitializeComponent()
		{
			this._methodsTabControl = new TabControl();
			this._selectTabPage = new TabPage();
			this._selectObjectDataSourceMethodEditor = new ObjectDataSourceMethodEditor();
			this._updateTabPage = new TabPage();
			this._updateObjectDataSourceMethodEditor = new ObjectDataSourceMethodEditor();
			this._insertTabPage = new TabPage();
			this._insertObjectDataSourceMethodEditor = new ObjectDataSourceMethodEditor();
			this._deleteTabPage = new TabPage();
			this._deleteObjectDataSourceMethodEditor = new ObjectDataSourceMethodEditor();
			this._methodsTabControl.SuspendLayout();
			this._selectTabPage.SuspendLayout();
			this._updateTabPage.SuspendLayout();
			this._insertTabPage.SuspendLayout();
			this._deleteTabPage.SuspendLayout();
			base.SuspendLayout();
			this._methodsTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._methodsTabControl.Controls.Add(this._selectTabPage);
			this._methodsTabControl.Controls.Add(this._updateTabPage);
			this._methodsTabControl.Controls.Add(this._insertTabPage);
			this._methodsTabControl.Controls.Add(this._deleteTabPage);
			this._methodsTabControl.Location = new Point(0, 0);
			this._methodsTabControl.Name = "_methodsTabControl";
			this._methodsTabControl.SelectedIndex = 0;
			this._methodsTabControl.ShowToolTips = true;
			this._methodsTabControl.Size = new Size(544, 274);
			this._methodsTabControl.TabIndex = 0;
			this._selectTabPage.Controls.Add(this._selectObjectDataSourceMethodEditor);
			this._selectTabPage.Location = new Point(4, 22);
			this._selectTabPage.Name = "_selectTabPage";
			this._selectTabPage.Size = new Size(536, 248);
			this._selectTabPage.TabIndex = 10;
			this._selectTabPage.Text = "SELECT";
			this._selectObjectDataSourceMethodEditor.Dock = DockStyle.Fill;
			this._selectObjectDataSourceMethodEditor.Location = new Point(0, 0);
			this._selectObjectDataSourceMethodEditor.Name = "_selectObjectDataSourceMethodEditor";
			this._selectObjectDataSourceMethodEditor.TabIndex = 0;
			this._selectObjectDataSourceMethodEditor.MethodChanged += this.OnSelectMethodChanged;
			this._updateTabPage.Controls.Add(this._updateObjectDataSourceMethodEditor);
			this._updateTabPage.Location = new Point(4, 22);
			this._updateTabPage.Name = "_updateTabPage";
			this._updateTabPage.Size = new Size(536, 248);
			this._updateTabPage.TabIndex = 20;
			this._updateTabPage.Text = "UPDATE";
			this._updateObjectDataSourceMethodEditor.Dock = DockStyle.Fill;
			this._updateObjectDataSourceMethodEditor.Location = new Point(0, 0);
			this._updateObjectDataSourceMethodEditor.Name = "_updateObjectDataSourceMethodEditor";
			this._updateObjectDataSourceMethodEditor.TabIndex = 0;
			this._insertTabPage.Controls.Add(this._insertObjectDataSourceMethodEditor);
			this._insertTabPage.Location = new Point(4, 22);
			this._insertTabPage.Name = "_insertTabPage";
			this._insertTabPage.Size = new Size(536, 248);
			this._insertTabPage.TabIndex = 30;
			this._insertTabPage.Text = "INSERT";
			this._insertObjectDataSourceMethodEditor.Dock = DockStyle.Fill;
			this._insertObjectDataSourceMethodEditor.Location = new Point(0, 0);
			this._insertObjectDataSourceMethodEditor.Name = "_insertObjectDataSourceMethodEditor";
			this._insertObjectDataSourceMethodEditor.TabIndex = 0;
			this._deleteTabPage.Controls.Add(this._deleteObjectDataSourceMethodEditor);
			this._deleteTabPage.Location = new Point(4, 22);
			this._deleteTabPage.Name = "_deleteTabPage";
			this._deleteTabPage.Size = new Size(536, 248);
			this._deleteTabPage.TabIndex = 40;
			this._deleteTabPage.Text = "DELETE";
			this._deleteObjectDataSourceMethodEditor.Dock = DockStyle.Fill;
			this._deleteObjectDataSourceMethodEditor.Location = new Point(0, 0);
			this._deleteObjectDataSourceMethodEditor.Name = "_deleteObjectDataSourceMethodEditor";
			this._deleteObjectDataSourceMethodEditor.TabIndex = 0;
			base.Controls.Add(this._methodsTabControl);
			base.Name = "ObjectDataSourceChooseMethodsPanel";
			base.Size = new Size(544, 274);
			this._methodsTabControl.ResumeLayout(false);
			this._selectTabPage.ResumeLayout(false);
			this._updateTabPage.ResumeLayout(false);
			this._insertTabPage.ResumeLayout(false);
			this._deleteTabPage.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void InitializeUI()
		{
			base.Caption = SR.GetString("ObjectDataSourceChooseMethodsPanel_PanelCaption");
		}

		protected internal override void OnComplete()
		{
			MethodInfo methodInfo = this.DeleteMethodInfo;
			string text = ((methodInfo == null) ? string.Empty : methodInfo.Name);
			if (this._objectDataSource.DeleteMethod != text)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._objectDataSource)["DeleteMethod"];
				propertyDescriptor.ResetValue(this._objectDataSource);
				propertyDescriptor.SetValue(this._objectDataSource, text);
			}
			methodInfo = this.InsertMethodInfo;
			text = ((methodInfo == null) ? string.Empty : methodInfo.Name);
			if (this._objectDataSource.InsertMethod != text)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._objectDataSource)["InsertMethod"];
				propertyDescriptor.ResetValue(this._objectDataSource);
				propertyDescriptor.SetValue(this._objectDataSource, text);
			}
			methodInfo = this.SelectMethodInfo;
			text = ((methodInfo == null) ? string.Empty : methodInfo.Name);
			if (this._objectDataSource.SelectMethod != text)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._objectDataSource)["SelectMethod"];
				propertyDescriptor.ResetValue(this._objectDataSource);
				propertyDescriptor.SetValue(this._objectDataSource, text);
			}
			methodInfo = this.UpdateMethodInfo;
			text = ((methodInfo == null) ? string.Empty : methodInfo.Name);
			if (this._objectDataSource.UpdateMethod != text)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._objectDataSource)["UpdateMethod"];
				propertyDescriptor.ResetValue(this._objectDataSource);
				propertyDescriptor.SetValue(this._objectDataSource, text);
			}
			this._objectDataSource.SelectParameters.Clear();
			methodInfo = this.SelectMethodInfo;
			try
			{
				IDataSourceSchema dataSourceSchema = new TypeSchema(methodInfo.ReturnType);
				if (dataSourceSchema != null)
				{
					IDataSourceViewSchema[] views = dataSourceSchema.GetViews();
					if (views != null && views.Length > 0)
					{
						views[0].GetFields();
					}
				}
			}
			catch (Exception)
			{
			}
			ObjectDataSourceDesigner.MergeParameters(this._objectDataSource.DeleteParameters, this.DeleteMethodInfo, this.DeleteMethodDataObjectType);
			ObjectDataSourceDesigner.MergeParameters(this._objectDataSource.InsertParameters, this.InsertMethodInfo, this.InsertMethodDataObjectType);
			ObjectDataSourceDesigner.MergeParameters(this._objectDataSource.UpdateParameters, this.UpdateMethodInfo, this.UpdateMethodDataObjectType);
			string text2 = string.Empty;
			if (this.DeleteMethodDataObjectType != null)
			{
				text2 = this.DeleteMethodDataObjectType.FullName;
			}
			else if (this.InsertMethodDataObjectType != null)
			{
				text2 = this.InsertMethodDataObjectType.FullName;
			}
			else if (this.UpdateMethodDataObjectType != null)
			{
				text2 = this.UpdateMethodDataObjectType.FullName;
			}
			if (this._objectDataSource.DataObjectTypeName != text2)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._objectDataSource)["DataObjectTypeName"];
				propertyDescriptor.ResetValue(this._objectDataSource);
				propertyDescriptor.SetValue(this._objectDataSource, text2);
			}
			if (methodInfo != null)
			{
				this._objectDataSourceDesigner.RefreshSchema(methodInfo.ReflectedType, methodInfo.Name, methodInfo.ReturnType, true);
			}
		}

		public override bool OnNext()
		{
			List<Type> list = new List<Type>();
			Type deleteMethodDataObjectType = this.DeleteMethodDataObjectType;
			if (deleteMethodDataObjectType != null)
			{
				list.Add(deleteMethodDataObjectType);
			}
			Type insertMethodDataObjectType = this.InsertMethodDataObjectType;
			if (insertMethodDataObjectType != null)
			{
				list.Add(insertMethodDataObjectType);
			}
			Type updateMethodDataObjectType = this.UpdateMethodDataObjectType;
			if (updateMethodDataObjectType != null)
			{
				list.Add(updateMethodDataObjectType);
			}
			if (list.Count > 1)
			{
				Type type = list[0];
				for (int i = 1; i < list.Count; i++)
				{
					if (type != list[i])
					{
						UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("ObjectDataSourceChooseMethodsPanel_IncompatibleDataObjectTypes"));
						return false;
					}
				}
			}
			MethodInfo selectMethodInfo = this.SelectMethodInfo;
			if (selectMethodInfo == null)
			{
				return false;
			}
			ParameterInfo[] parameters = selectMethodInfo.GetParameters();
			if (parameters.Length <= 0)
			{
				return true;
			}
			ObjectDataSourceConfigureParametersPanel objectDataSourceConfigureParametersPanel = base.NextPanel as ObjectDataSourceConfigureParametersPanel;
			if (objectDataSourceConfigureParametersPanel == null)
			{
				objectDataSourceConfigureParametersPanel = ((ObjectDataSourceWizardForm)base.ParentWizard).GetParametersPanel();
				base.NextPanel = objectDataSourceConfigureParametersPanel;
				objectDataSourceConfigureParametersPanel.InitializeParameters(this._objectDataSource.SelectParameters);
			}
			objectDataSourceConfigureParametersPanel.SetMethod(this.SelectMethodInfo);
			return true;
		}

		public override void OnPrevious()
		{
		}

		private void OnSelectMethodChanged(object sender, EventArgs e)
		{
			this.UpdateEnabledState();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				this.UpdateEnabledState();
			}
		}

		private static MethodInfo[] GetMethods(Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			List<MethodInfo> list = new List<MethodInfo>();
			foreach (MethodInfo methodInfo in methods)
			{
				if (((methodInfo.GetBaseDefinition().DeclaringType != typeof(object)) & !methodInfo.IsSpecialName) && !methodInfo.IsAbstract)
				{
					list.Add(methodInfo);
				}
			}
			return list.ToArray();
		}

		public void SetType(Type type)
		{
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				MethodInfo[] methods = ObjectDataSourceChooseMethodsPanel.GetMethods(type);
				this._methodsTabControl.SelectedIndex = 0;
				Type type2 = ObjectDataSourceDesigner.GetType(base.ServiceProvider, this._objectDataSource.DataObjectTypeName, true);
				this._selectObjectDataSourceMethodEditor.SetMethodInformation(methods, this._objectDataSource.SelectMethod, this._objectDataSource.SelectParameters, DataObjectMethodType.Select, type2);
				this._insertObjectDataSourceMethodEditor.SetMethodInformation(methods, this._objectDataSource.InsertMethod, this._objectDataSource.InsertParameters, DataObjectMethodType.Insert, type2);
				this._updateObjectDataSourceMethodEditor.SetMethodInformation(methods, this._objectDataSource.UpdateMethod, this._objectDataSource.UpdateParameters, DataObjectMethodType.Update, type2);
				this._deleteObjectDataSourceMethodEditor.SetMethodInformation(methods, this._objectDataSource.DeleteMethod, this._objectDataSource.DeleteParameters, DataObjectMethodType.Delete, type2);
			}
			finally
			{
				Cursor.Current = cursor;
			}
			this.UpdateEnabledState();
		}

		private void UpdateEnabledState()
		{
			MethodInfo selectMethodInfo = this.SelectMethodInfo;
			if (selectMethodInfo != null)
			{
				ParameterInfo[] parameters = selectMethodInfo.GetParameters();
				bool flag = parameters.Length > 0;
				base.ParentWizard.NextButton.Enabled = flag;
				base.ParentWizard.FinishButton.Enabled = !flag;
				return;
			}
			base.ParentWizard.NextButton.Enabled = false;
			base.ParentWizard.FinishButton.Enabled = false;
		}

		private TabControl _methodsTabControl;

		private TabPage _selectTabPage;

		private TabPage _updateTabPage;

		private TabPage _insertTabPage;

		private TabPage _deleteTabPage;

		private ObjectDataSourceMethodEditor _updateObjectDataSourceMethodEditor;

		private ObjectDataSourceMethodEditor _selectObjectDataSourceMethodEditor;

		private ObjectDataSourceMethodEditor _insertObjectDataSourceMethodEditor;

		private ObjectDataSourceMethodEditor _deleteObjectDataSourceMethodEditor;

		private ObjectDataSource _objectDataSource;

		private ObjectDataSourceDesigner _objectDataSourceDesigner;
	}
}
