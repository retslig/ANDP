


CREATE PROCEDURE [common].[sp_RetrieveProvisoningLog] @ExternalItemId nvarchar(36), @tenantId uniqueidentifier
AS
BEGIN
	DECLARE @sql varchar(MAX), @schema nvarchar(36)
	SELECT TOP 1 @schema = [Schema] 
	FROM [common].[Tenants] 
	WHERE Guid = @tenantId

    set @sql = 'SELECT  ' +
				'	''Equipment'' as RecordType, ' +
				'	o.Id as OrderId, ' +
				'	o.ExternalOrderId as  OrderNumber, ' +
				'	s.ExternalServiceId as ServiceNumber, ' +
				'	i.ExternalItemId as ItemNumber, ' +
				'	o.ExternalAccountId as AccountNumber, ' +
				'	i.ProvisionDate, ' +
				'	i.StartDate, ' +
				'	i.CompletionDate, ' +
				'	st.Name as Result, ' +
				'	at.Name as OrderType, ' +
				'	it.Name as ItemType, ' +
				'	i.ResultMessage, ' +
				'	i.Log ' +
				'FROM ['+ @schema +'].[Item] i  ' +
				'JOIN ['+ @schema +'].[Service] s on s.Id = i.ServiceId ' +
				'JOIN ['+ @schema +'].[Order] o on o.Id = s.OrderId ' +
				'JOIN ['+ @schema +'].StatusTypeEnum st on st.Id = i.StatusTypeId ' +
				'JOIN ['+ @schema +'].ActionTypeEnum at on at.Id = i.ActionTypeId ' +
				'JOIN ['+ @schema +'].ItemTypeEnum it on it.Id = i.ItemTypeId ' +
				'WHERE ExternalItemId = ''' + @ExternalItemId + ''''

    exec(@sql)
	--select @sql  FROM [common].[Tenants] 

--  SELECT 
--	'Equipment' as RecordType,
--	o.Id as OrderId, 
--	o.ExternalOrderId as  OrderNumber, 
--	s.ExternalServiceId as ServiceNumber, 
--	i.ExternalItemId as ItemNumber, 
--	o.ExternalAccountId as AccountNumber,
--	i.ProvisionDate, 
--	i.StartDate, 
--	i.CompletionDate, 
--	st.Name as Result, 
--	at.Name as OrderType, 
--	it.Name as ItemType,
--	i.ResultMessage, 
--	i.Log
--FROM [emp].[Item] i 
--JOIN [emp].[Service] s on s.Id = i.ServiceId
--JOIN [emp].[Order] o on o.Id = s.OrderId
--JOIN [emp].StatusTypeEnum st on st.Id = i.StatusTypeId
--JOIN [emp].ActionTypeEnum at on at.Id = i.ActionTypeId
--JOIN [emp].ItemTypeEnum it on it.Id = i.ItemTypeId
--WHERE ExternalItemId = @ExternalItemId


END