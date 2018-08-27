





CREATE VIEW [gdo].[PendingOrderItemsByDispatcherRules_VW]
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
from [gdo].[Order] O
join [gdo].[Service] S on S.OrderId = O.Id
join [gdo].[Item] I on I.ServiceId = S.Id
join gdo.StatusTypeEnum st on st.Id = o.StatusTypeId
join gdo.StatusTypeEnum st1 on st1.Id = s.StatusTypeId
join gdo.StatusTypeEnum st2 on st2.Id = I.StatusTypeId
join gdo.ActionTypeEnum at on at.id = I.ActionTypeId
join gdo.ItemTypeEnum it on it.id = I.ItemTypeId








