



CREATE PROCEDURE [common].[sp_RetrieveProvisioningItemCommonErrors] @startDate datetime2, @endDate datetime2, @tenantId uniqueidentifier
AS
BEGIN
	DECLARE @sql varchar(MAX), @schema nvarchar(36)
	SELECT TOP 1 @schema = [Schema] 
	FROM [common].[Tenants] 
	WHERE Guid = @tenantId

    set @sql = 'SELECT ' +
				'	count(i.[ResultMessage]) as NumberOfErrors ' +
				'	,it.[Name]  ' +
				'	,i.[ResultMessage] ' +
				'FROM ['+ @schema +'].[Item] i ' +
				'JOIN ['+ @schema +'].ItemTypeEnum it on it.Id = i.ItemTypeId  ' +
				'WHERE i.StatusTypeId = 3 AND CAST(i.CompletionDate As Date) BETWEEN  CAST(''' + CONVERT(VARCHAR(10),@startDate, 126) + ''' As Date) and CAST(''' + CONVERT(VARCHAR(10),@endDate, 126) + ''' As Date) ' +
				'group by it.[Name] , i.[ResultMessage] ' +
				'HAVING count(i.[ResultMessage]) > 3 '


    exec(@sql)
	--select @sql  FROM [common].[Tenants] 

--SELECT 
--	count(i.[ResultMessage]) as NumberOfErrors
--	,it.[Name] 
--	,i.[ResultMessage]
--FROM [emp].[Item] i
--JOIN [emp].ItemTypeEnum it on it.Id = i.ItemTypeId 
--WHERE i.StatusTypeId = 3 AND CAST(i.CompletionDate As Date) BETWEEN CAST(@startDate As Date) and CAST(@endDate As Date)
--group by it.[Name] , i.[ResultMessage]
--HAVING count(i.[ResultMessage]) > 3



END