﻿
CREATE VIEW [ral].[ItemProvisioningResults_VW]
AS

select 
it.Name as ItemName, 
at.Name as OrderName,
O.ExternalOrderId as OrderNumber,
ExternalAccountId, 
st.Name as Result,
I.DateCreated as OrderReceivedDate,
I.CompletionDate,
I.StartDate,
DATEDIFF(Second, I.StartDate, I.CompletionDate) as ProvisionTimeInSeconds,
O.ResultMessage
from [ral].[Order] O
join [ral].[Service] S on S.OrderId = O.Id
join [ral].[Item] I on I.ServiceId = S.Id
join ral.StatusTypeEnum st on st.Id = I.StatusTypeId
join ral.ActionTypeEnum at on at.id = I.ActionTypeId
join ral.ItemTypeEnum it on it.id = I.ItemTypeId
--where I.ActionTypeId in(9,10) 
--and I.ItemTypeId = 1
--and cast(I.CompletionDate As Date) >= cast('10-9-2014' As Date)  
--order by it.Name, st.Name


