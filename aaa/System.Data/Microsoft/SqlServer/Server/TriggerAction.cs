using System;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000056 RID: 86
	public enum TriggerAction
	{
		// Token: 0x04000640 RID: 1600
		Invalid,
		// Token: 0x04000641 RID: 1601
		Insert,
		// Token: 0x04000642 RID: 1602
		Update,
		// Token: 0x04000643 RID: 1603
		Delete,
		// Token: 0x04000644 RID: 1604
		CreateTable = 21,
		// Token: 0x04000645 RID: 1605
		AlterTable,
		// Token: 0x04000646 RID: 1606
		DropTable,
		// Token: 0x04000647 RID: 1607
		CreateIndex,
		// Token: 0x04000648 RID: 1608
		AlterIndex,
		// Token: 0x04000649 RID: 1609
		DropIndex,
		// Token: 0x0400064A RID: 1610
		CreateSynonym = 34,
		// Token: 0x0400064B RID: 1611
		DropSynonym = 36,
		// Token: 0x0400064C RID: 1612
		CreateSecurityExpression = 31,
		// Token: 0x0400064D RID: 1613
		DropSecurityExpression = 33,
		// Token: 0x0400064E RID: 1614
		CreateView = 41,
		// Token: 0x0400064F RID: 1615
		AlterView,
		// Token: 0x04000650 RID: 1616
		DropView,
		// Token: 0x04000651 RID: 1617
		CreateProcedure = 51,
		// Token: 0x04000652 RID: 1618
		AlterProcedure,
		// Token: 0x04000653 RID: 1619
		DropProcedure,
		// Token: 0x04000654 RID: 1620
		CreateFunction = 61,
		// Token: 0x04000655 RID: 1621
		AlterFunction,
		// Token: 0x04000656 RID: 1622
		DropFunction,
		// Token: 0x04000657 RID: 1623
		CreateTrigger = 71,
		// Token: 0x04000658 RID: 1624
		AlterTrigger,
		// Token: 0x04000659 RID: 1625
		DropTrigger,
		// Token: 0x0400065A RID: 1626
		CreateEventNotification,
		// Token: 0x0400065B RID: 1627
		DropEventNotification = 76,
		// Token: 0x0400065C RID: 1628
		CreateType = 91,
		// Token: 0x0400065D RID: 1629
		DropType = 93,
		// Token: 0x0400065E RID: 1630
		CreateAssembly = 101,
		// Token: 0x0400065F RID: 1631
		AlterAssembly,
		// Token: 0x04000660 RID: 1632
		DropAssembly,
		// Token: 0x04000661 RID: 1633
		CreateUser = 131,
		// Token: 0x04000662 RID: 1634
		AlterUser,
		// Token: 0x04000663 RID: 1635
		DropUser,
		// Token: 0x04000664 RID: 1636
		CreateRole,
		// Token: 0x04000665 RID: 1637
		AlterRole,
		// Token: 0x04000666 RID: 1638
		DropRole,
		// Token: 0x04000667 RID: 1639
		CreateAppRole,
		// Token: 0x04000668 RID: 1640
		AlterAppRole,
		// Token: 0x04000669 RID: 1641
		DropAppRole,
		// Token: 0x0400066A RID: 1642
		CreateSchema = 141,
		// Token: 0x0400066B RID: 1643
		AlterSchema,
		// Token: 0x0400066C RID: 1644
		DropSchema,
		// Token: 0x0400066D RID: 1645
		CreateLogin,
		// Token: 0x0400066E RID: 1646
		AlterLogin,
		// Token: 0x0400066F RID: 1647
		DropLogin,
		// Token: 0x04000670 RID: 1648
		CreateMsgType = 151,
		// Token: 0x04000671 RID: 1649
		DropMsgType = 153,
		// Token: 0x04000672 RID: 1650
		CreateContract,
		// Token: 0x04000673 RID: 1651
		DropContract = 156,
		// Token: 0x04000674 RID: 1652
		CreateQueue,
		// Token: 0x04000675 RID: 1653
		AlterQueue,
		// Token: 0x04000676 RID: 1654
		DropQueue,
		// Token: 0x04000677 RID: 1655
		CreateService = 161,
		// Token: 0x04000678 RID: 1656
		AlterService,
		// Token: 0x04000679 RID: 1657
		DropService,
		// Token: 0x0400067A RID: 1658
		CreateRoute,
		// Token: 0x0400067B RID: 1659
		AlterRoute,
		// Token: 0x0400067C RID: 1660
		DropRoute,
		// Token: 0x0400067D RID: 1661
		GrantStatement,
		// Token: 0x0400067E RID: 1662
		DenyStatement,
		// Token: 0x0400067F RID: 1663
		RevokeStatement,
		// Token: 0x04000680 RID: 1664
		GrantObject,
		// Token: 0x04000681 RID: 1665
		DenyObject,
		// Token: 0x04000682 RID: 1666
		RevokeObject,
		// Token: 0x04000683 RID: 1667
		CreateBinding = 174,
		// Token: 0x04000684 RID: 1668
		AlterBinding,
		// Token: 0x04000685 RID: 1669
		DropBinding,
		// Token: 0x04000686 RID: 1670
		CreatePartitionFunction = 191,
		// Token: 0x04000687 RID: 1671
		AlterPartitionFunction,
		// Token: 0x04000688 RID: 1672
		DropPartitionFunction,
		// Token: 0x04000689 RID: 1673
		CreatePartitionScheme,
		// Token: 0x0400068A RID: 1674
		AlterPartitionScheme,
		// Token: 0x0400068B RID: 1675
		DropPartitionScheme
	}
}
