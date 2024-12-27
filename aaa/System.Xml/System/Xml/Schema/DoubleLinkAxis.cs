using System;
using MS.Internal.Xml.XPath;

namespace System.Xml.Schema
{
	// Token: 0x0200017F RID: 383
	internal class DoubleLinkAxis : Axis
	{
		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x000571DC File Offset: 0x000561DC
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x000571E4 File Offset: 0x000561E4
		internal Axis Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x000571F0 File Offset: 0x000561F0
		internal DoubleLinkAxis(Axis axis, DoubleLinkAxis inputaxis)
			: base(axis.TypeOfAxis, inputaxis, axis.Prefix, axis.Name, axis.NodeType)
		{
			this.next = null;
			base.Urn = axis.Urn;
			this.abbrAxis = axis.AbbrAxis;
			if (inputaxis != null)
			{
				inputaxis.Next = this;
			}
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00057245 File Offset: 0x00056245
		internal static DoubleLinkAxis ConvertTree(Axis axis)
		{
			if (axis == null)
			{
				return null;
			}
			return new DoubleLinkAxis(axis, DoubleLinkAxis.ConvertTree((Axis)axis.Input));
		}

		// Token: 0x04000C5F RID: 3167
		internal Axis next;
	}
}
