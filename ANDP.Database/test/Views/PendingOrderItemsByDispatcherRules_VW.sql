






CREATE VIEW [test].[PendingOrderItemsByDispatcherRules_VW]
AS

select 
o.Id as OrderId,
s.id as ServiceId,
i.id as ItemId,
o.ExternalOrderId,
it.Name as ItemName, 
ExternalAccountId, 
st.Name as OrderStatus,
st1.Name as ServiceStatus,
st2.Name as ItemStatus,
at.Name as ItemAction,
o.ProvisionDate,
o.Priority as OrderPriority,
o.xml,
i.ResultMessage
from [test].[Order] O
join [test].[Service] S on S.OrderId = O.Id
join [test].[Item] I on I.ServiceId = S.Id
join test.StatusTypeEnum st on st.Id = o.StatusTypeId
join test.StatusTypeEnum st1 on st1.Id = s.StatusTypeId
join test.StatusTypeEnum st2 on st2.Id = I.StatusTypeId
join test.ActionTypeEnum at on at.id = I.ActionTypeId
join test.ItemTypeEnum it on it.id = I.ItemTypeId









