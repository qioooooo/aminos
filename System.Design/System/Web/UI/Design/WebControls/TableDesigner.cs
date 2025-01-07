using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class TableDesigner : ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			Table table = (Table)base.ViewControl;
			TableRowCollection rows = table.Rows;
			bool flag = rows.Count == 0;
			if (flag)
			{
				TableRow tableRow = new TableRow();
				rows.Add(tableRow);
				TableCell tableCell = new TableCell();
				tableCell.Text = "###";
				rows[0].Cells.Add(tableCell);
			}
			else
			{
				bool flag2 = true;
				for (int i = 0; i < rows.Count; i++)
				{
					if (rows[i].Cells.Count != 0)
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					TableCell tableCell2 = new TableCell();
					tableCell2.Text = "###";
					rows[0].Cells.Add(tableCell2);
				}
			}
			if (!flag)
			{
				foreach (object obj in rows)
				{
					TableRow tableRow2 = (TableRow)obj;
					foreach (object obj2 in tableRow2.Cells)
					{
						TableCell tableCell3 = (TableCell)obj2;
						if (tableCell3.Text.Length == 0 && !tableCell3.HasControls())
						{
							tableCell3.Text = "###";
						}
					}
				}
			}
			return base.GetDesignTimeHtml();
		}
	}
}
