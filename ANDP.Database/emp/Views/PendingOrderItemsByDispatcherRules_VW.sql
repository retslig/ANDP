





CREATE VIEW [emp].[PendingOrderItemsByDispatcherRules_VW]
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
from [emp].[Order] O
join [emp].[Service] S on S.OrderId = O.Id
join [emp].[Item] I on I.ServiceId = S.Id
join emp.StatusTypeEnum st on st.Id = o.StatusTypeId
join emp.StatusTypeEnum st1 on st1.Id = s.StatusTypeId
join emp.StatusTypeEnum st2 on st2.Id = I.StatusTypeId
join emp.ActionTypeEnum at on at.id = I.ActionTypeId
join emp.ItemTypeEnum it on it.id = I.ItemTypeId








