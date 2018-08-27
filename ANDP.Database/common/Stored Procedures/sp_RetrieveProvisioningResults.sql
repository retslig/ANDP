

CREATE PROCEDURE [common].[sp_RetrieveProvisioningResults] @date datetime2, @tenantId uniqueidentifier
AS
BEGIN
	DECLARE @sql varchar(MAX), @schema nvarchar(36)
	SELECT TOP 1 @schema = [Schema] 
	FROM [common].[Tenants] 
	WHERE Guid = @tenantId

    set @sql = 'SELECT ' +
			'	''Order'' as RecordType, ' +
			'	o.Id as OrderId, ' +
			'	o.ExternalOrderId as  OrderNumber, ' +
			'	null as ServiceNumber, ' +
			'	null as ItemNumber, ' +
			'	o.ExternalAccountId as AccountNumber, ' +
			'	ProvisionDate, ' +
			'	StartDate, ' +
			'	CompletionDate, ' +
			'	st.Name as Result, ' +
			'	at.Name as OrderType, ' +
			'   null as ItemType, ' +
			'	ResultMessage, ' +
			'	Log, ' +
			'	null as EquipmentName ' +
			'FROM ['+ @schema +'].[Order] o ' +
			'join ['+ @schema +'].StatusTypeEnum st on st.Id = o.StatusTypeId ' +
			'join ['+ @schema +'].ActionTypeEnum at on at.Id = o.ActionTypeId ' +
			'WHERE cast(o.CompletionDate As Date) =  cast(''' + CONVERT(VARCHAR(10),@date, 126) + ''' As Date) ' +
			'' +
			'UNION ALL ' +
			'' +
			'SELECT ' +
			'	''Service'' as RecordType, ' +
			'	o.Id as OrderId, ' +
			'	o.ExternalOrderId as  OrderNumber, ' +
			'	s.ExternalServiceId as ServiceNumber, ' +
			'	null as ItemNumber, ' +
			'	o.ExternalAccountId as AccountNumber,'  +
			'	s.ProvisionDate, ' +
			'	s.StartDate, ' +
			'	s.CompletionDate, ' +
			'	st.Name as Result, ' +
			'	at.Name as OrderType, ' +
			'   null as ItemType, ' +
			'	s.ResultMessage, ' +
			'	s.Log , ' +
			'	null as EquipmentName ' +
			'FROM ['+ @schema +'].[Service] s ' +
			'JOIN ['+ @schema +'].[Order] o on o.Id = s.OrderId ' +
			'JOIN ['+ @schema +'].StatusTypeEnum st on st.Id = s.StatusTypeId ' +
			'JOIN ['+ @schema +'].ActionTypeEnum at on at.Id = s.ActionTypeId ' +
			'WHERE cast(s.CompletionDate As Date) =  cast(''' + CONVERT(VARCHAR(10),@date, 126) + ''' As Date) ' +
			'' +
			'UNION ALL ' +
			'' +
			'SELECT ' +
			'	''Item'' as RecordType, ' +
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
			'   null as ItemType, ' +
			'	i.ResultMessage, ' +
			'	i.Log, ' +
			'	null as EquipmentName ' +
			'FROM ['+ @schema +'].[Item] i ' +
			'JOIN ['+ @schema +'].[Service] s on s.Id = i.ServiceId ' +
			'JOIN ['+ @schema +'].[Order] o on o.Id = s.OrderId ' +
			'JOIN ['+ @schema +'].StatusTypeEnum st on st.Id = i.StatusTypeId ' +
			'JOIN ['+ @schema +'].ActionTypeEnum at on at.Id = i.ActionTypeId ' +
			'JOIN ['+ @schema +'].ItemTypeEnum it on it.Id = i.ItemTypeId ' + 
			'WHERE cast(s.CompletionDate As Date) =  cast(''' + CONVERT(VARCHAR(10),@date, 126) + ''' As Date) ' +
			'' +
			'UNION ALL ' +
			'' +
			'SELECT ' +
			'	''Equipment'' as RecordType, ' +
			'	o.Id as OrderId, ' +
			'	o.ExternalOrderId as  OrderNumber, ' +
			'	s.ExternalServiceId as ServiceNumber, ' +
			'	i.ExternalItemId as ItemNumber, ' +
			'	o.ExternalAccountId as AccountNumber, ' +
			'	i.ProvisionDate, ' +
			'	e.StartDate, ' +
			'	e.CompletionDate, ' +
			'	st.Name as Result, ' +
			'	at.Name as OrderType, ' +
			'   null as ItemType, ' +
			'	e.ResultMessage, ' +
			'	e.Log, ' +
			'	es.Name as EquipmentName ' +
			'FROM ['+ @schema +'].[Equipment] e ' +
			'JOIN ['+ @schema +'].[Item] i on i.Id = e.ItemId ' +
			'JOIN ['+ @schema +'].[Service] s on s.Id = i.ServiceId ' +
			'JOIN ['+ @schema +'].[Order] o on o.Id = s.OrderId ' +
			'JOIN ['+ @schema +'].StatusTypeEnum st on st.Id = e.StatusTypeId ' +
			'JOIN ['+ @schema +'].ActionTypeEnum at on at.Id = e.ActionTypeId ' +
			'JOIN ['+ @schema +'].ItemTypeEnum it on it.Id = i.ItemTypeId ' + 
			'JOIN ['+ @schema +'].[EquipmentSetup] es on es.Id = e.EquipmentSetupId ' +
			'WHERE cast(s.CompletionDate As Date) =  cast(''' + CONVERT(VARCHAR(10),@date, 126) + ''' As Date) '
    exec(@sql)
	--select @sql  FROM [common].[Tenants] 

	

--declare  @date datetime2 = '2016-06-06 12:19:24.197';

--SELECT 
--	'Order' as RecordType,
--	o.Id as OrderId, 
--	o.ExternalOrderId as  OrderNumber, 
--	null as ServiceNumber, 
--	null as ItemNumber, 
--	o.ExternalAccountId as AccountNumber,
--	ProvisionDate, 
--	StartDate, 
--	CompletionDate, 
--	st.Name as Result, 
--	at.Name as OrderType, 
--	null as ItemType,
--	ResultMessage, 
--	Log,
--	null as EquipmentName
--FROM [emp].[Order] o
--join [emp].StatusTypeEnum st on st.Id = o.StatusTypeId
--join [emp].ActionTypeEnum at on at.Id = o.ActionTypeId
--WHERE cast(o.CompletionDate As Date) =  cast(@date As Date)

--UNION ALL

--SELECT 
--	'Service' as RecordType,
--	o.Id as OrderId, 
--	o.ExternalOrderId as  OrderNumber, 
--	s.ExternalServiceId as ServiceNumber, 
--	null as ItemNumber, 
--	o.ExternalAccountId as AccountNumber,
--	s.ProvisionDate, 
--	s.StartDate, 
--	s.CompletionDate, 
--	st.Name as Result, 
--	at.Name as OrderType,
--	null as ItemType,
--	s.ResultMessage, 
--	s.Log ,
--	null as EquipmentName	
--FROM [emp].[Service] s
--JOIN [emp].[Order] o on o.Id = s.OrderId
--JOIN [emp].StatusTypeEnum st on st.Id = s.StatusTypeId
--JOIN [emp].ActionTypeEnum at on at.Id = s.ActionTypeId
--WHERE cast(s.CompletionDate As Date) =  cast(@date As Date)

--UNION ALL

--SELECT 
--	'Item' as RecordType,
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
--	i.Log,
--	null as EquipmentName
--FROM [emp].[Item] i
--JOIN [emp].[Service] s on s.Id = i.ServiceId
--JOIN [emp].[Order] o on o.Id = s.OrderId
--JOIN [emp].StatusTypeEnum st on st.Id = i.StatusTypeId
--JOIN [emp].ActionTypeEnum at on at.Id = i.ActionTypeId
--JOIN [emp].ItemTypeEnum it on it.Id = i.ItemTypeId
--WHERE cast(s.CompletionDate As Date) =  cast(@date As Date)

--UNION ALL

--SELECT 
--	'Equipment' as RecordType,
--	o.Id as OrderId, 
--	o.ExternalOrderId as  OrderNumber, 
--	s.ExternalServiceId as ServiceNumber, 
--	i.ExternalItemId as ItemNumber, 
--	o.ExternalAccountId as AccountNumber,
--	i.ProvisionDate, 
--	e.StartDate, 
--	e.CompletionDate, 
--	st.Name as Result, 
--	at.Name as OrderType, 
--	it.Name as ItemType,
--	e.ResultMessage, 
--	e.Log,
--	es.Name as EquipmentName
--FROM [emp].[Equipment] e
--JOIN [emp].[Item] i on i.Id = e.ItemId
--JOIN [emp].[Service] s on s.Id = i.ServiceId
--JOIN [emp].[Order] o on o.Id = s.OrderId
--JOIN [emp].StatusTypeEnum st on st.Id = i.StatusTypeId
--JOIN [emp].ActionTypeEnum at on at.Id = i.ActionTypeId
--JOIN [emp].ItemTypeEnum it on it.Id = i.ItemTypeId
--JOIN [emp].[EquipmentSetup] es on es.Id = e.EquipmentSetupId
--WHERE cast(s.CompletionDate As Date) =  cast(@date As Date)

END