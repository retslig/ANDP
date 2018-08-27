



CREATE PROCEDURE [common].[sp_RetrieveProvisioningEquipmentCommonErrors] @startDate datetime2, @endDate datetime2, @tenantId uniqueidentifier
AS
BEGIN
	DECLARE @sql varchar(MAX), @schema nvarchar(36)
	SELECT TOP 1 @schema = [Schema] 
	FROM [common].[Tenants] 
	WHERE Guid = @tenantId

    set @sql = 'SELECT ' +
				'	count(e.[ResultMessage]) as NumberOfErrors ' +
				'	,e.[EquipmentItemDescription] ' +
				'	,e.[ResultMessage] ' +
				'FROM ['+ @schema +'].[Equipment] e ' +
				'WHERE e.StatusTypeId = 3 AND CAST(e.CompletionDate As Date) BETWEEN CAST(''' + CONVERT(VARCHAR(10),@startDate, 126) + ''' As Date) and CAST(''' + CONVERT(VARCHAR(10),@endDate, 126) + ''' As Date) ' +
				'group by e.[EquipmentItemDescription], e.[ResultMessage] ' +
				'HAVING count(e.[ResultMessage]) > 3'

    exec(@sql)
	--select @sql  FROM [common].[Tenants] 

--SELECT 
--	count(e.[ResultMessage]) as NumberOfErrors
--	,e.[EquipmentItemDescription] 
--	,e.[ResultMessage]
--FROM [emp].[Equipment] e
--WHERE e.StatusTypeId = 3 AND CAST(e.CompletionDate As Date) BETWEEN CAST(@startDate As Date) and CAST(@endDate As Date)
--group by e.[EquipmentItemDescription], e.[ResultMessage]
--HAVING count(e.[ResultMessage]) > 3



END