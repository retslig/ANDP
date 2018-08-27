






CREATE VIEW [srtc].[PendingOrderItemsByDispatcherRules_VW]
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
from [srtc].[Order] O
join [srtc].[Service] S on S.OrderId = O.Id
join [srtc].[Item] I on I.ServiceId = S.Id
join srtc.StatusTypeEnum st on st.Id = o.StatusTypeId
join srtc.StatusTypeEnum st1 on st1.Id = s.StatusTypeId
join srtc.StatusTypeEnum st2 on st2.Id = I.StatusTypeId
join srtc.ActionTypeEnum at on at.id = I.ActionTypeId
join srtc.ItemTypeEnum it on it.id = I.ItemTypeId









