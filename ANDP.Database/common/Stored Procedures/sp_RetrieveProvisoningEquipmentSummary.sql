





--summary should include item level as well

CREATE PROCEDURE [common].[sp_RetrieveProvisoningEquipmentSummary] @startDate datetime2, @endDate datetime2, @tenantId uniqueidentifier
AS
BEGIN
	DECLARE @sql varchar(MAX), @schema nvarchar(36)
	SELECT TOP 1 @schema = [Schema] 
	FROM [common].[Tenants] 
	WHERE Guid = @tenantId

    set @sql = 'WITH Base AS ( ' +
			'	SELECT  ' +
			'		o.id AS OrderId, ' +
			'		o.ExternalOrderId,	 ' +
			'		o.ExternalAccountId, ' +
			'		o.ExternalCompanyId, ' +
			'		es.Id as EquipmentId, ' +
			'		es.Name as EquipmentName, ' +
			'		it.Name as ItemType, ' +
			'		at.Name as EquipmentAction, ' +
			'		st.Name as EquipmentResult, ' +
			'		e.DateCreated as EquipmentReceivedDate, ' +
			'		e.ProvisionDate as EquipmentProvisionDate, ' +
			'		e.CompletionDate, ' +
			'		e.StartDate, ' +
			'		DATEDIFF(Second, i.StartDate, i.CompletionDate) as ProvisionTimeInSeconds, ' +
			'		i.ResultMessage ' +
			'	FROM [QssAndp].['+ @schema +'].[Order] as o ' +
			'	JOIN  [QssAndp].['+ @schema +'].[Service]  as s on s.OrderId = o.Id ' +
			'	JOIN  [QssAndp].['+ @schema +'].[Item] as i on i.ServiceId = s.Id ' +
			'	JOIN QssAndp.emp.Equipment AS e on e.ItemId = i.Id ' +
			'	JOIN [QssAndp].['+ @schema +'].[EquipmentSetup] AS es on es.Id = e.EquipmentSetupId ' +
			'	JOIN [QssAndp].['+ @schema +'].[StatusTypeEnum] as st on st.Id = e.StatusTypeId ' +
			'	JOIN QssAndp.['+ @schema +'].ActionTypeEnum as at on at.Id = e.ActionTypeId ' +
			'	JOIN [QssAndp].['+ @schema +'].ItemTypeEnum as it on it.id = i.ItemTypeId ' +
			'	WHERE CAST(I.CompletionDate As Date) BETWEEN  CAST(''' + CONVERT(VARCHAR(10),@startDate, 126) + ''' As Date) and CAST(''' + CONVERT(VARCHAR(10),@endDate, 126) + ''' As Date) ' +
			'), ' +
			'Counts AS ( ' +
			'		SELECT ' +
			'			EquipmentName, EquipmentResult, COUNT(*) AS Number ' +
			'		FROM Base ' +
			'		GROUP BY EquipmentName, EquipmentResult ' +
			') ' +
			' ' +
			'SELECT  ' +
			'	c.EquipmentName, ' +
			'	SUM(c.Number) AS TotalCount, ' +
			'	SUM(s.Number) AS SuccessCount, ' +
			'	SUM(f.Number) AS FailureCount, ' +
			'	CONVERT(DECIMAL(4,3),(CONVERT(DECIMAL(8,3),SUM(s.Number)))/(CONVERT(DECIMAL(8,3),SUM(c.Number)))) AS SuccessRate ' +
			'FROM Counts AS c ' +
			'OUTER APPLY ( ' +
			'	SELECT EquipmentName, SUM(Number) AS Number ' +
			'	FROM Counts ' +
			'	WHERE c.EquipmentName = EquipmentName AND c.EquipmentResult = EquipmentResult AND EquipmentResult = ''Success'' ' +
			'	GROUP BY EquipmentName ' +
			') AS s ' +
			'OUTER APPLY ( ' +
			'	SELECT EquipmentName, SUM(Number) AS Number ' +
			'	FROM Counts ' +
			'	WHERE c.EquipmentName = EquipmentName AND c.EquipmentResult = EquipmentResult AND EquipmentResult <> ''Success'' ' +
			'	GROUP BY EquipmentName ' +
			') AS f ' +
			'GROUP BY c.EquipmentName ' 

    exec(@sql)
	--select @sql  FROM [common].[Tenants] 

--;WITH Base AS (
--	SELECT 
--		o.id AS OrderId, 
--		o.ExternalOrderId,	
--		o.ExternalAccountId, 
--		o.ExternalCompanyId, 
--		es.Id as EquipmentId,
--		es.Name as EquipmentName,
--		it.Name as ItemType, 
--		at.Name as EquipmentAction,
--		st.Name as EquipmentResult,
--		e.DateCreated as EquipmentReceivedDate,
--		e.ProvisionDate as EquipmentProvisionDate,
--		e.CompletionDate,
--		e.StartDate,
--		DATEDIFF(Second, i.StartDate, i.CompletionDate) as ProvisionTimeInSeconds,
--		i.ResultMessage
--	FROM [QssAndp].[emp].[Order] as o
--	JOIN  [QssAndp].[emp].[Service]  as s on s.OrderId = o.Id
--	JOIN  [QssAndp].[emp].[Item] as i on i.ServiceId = s.Id
--	JOIN QssAndp.emp.Equipment AS e on e.ItemId = i.Id
--	JOIN [QssAndp].[emp].[EquipmentSetup] AS es on es.Id = e.EquipmentSetupId
--	JOIN [QssAndp].[emp].[StatusTypeEnum] as st on st.Id = e.StatusTypeId
--	JOIN QssAndp.[emp].ActionTypeEnum as at on at.Id = e.ActionTypeId
--	JOIN [QssAndp].[emp].ItemTypeEnum as it on it.id = i.ItemTypeId
--	WHERE CAST(I.CompletionDate As Date) BETWEEN CAST(@startDate As Date) and CAST(@endDate As Date)
--),
--Counts AS (
--		SELECT
--			EquipmentName, EquipmentResult, COUNT(*) AS Number
--		FROM Base
--		GROUP BY EquipmentName, EquipmentResult
--)

--SELECT 
--	c.EquipmentName,
--	SUM(c.Number) AS TotalCount,
--	SUM(s.Number) AS SuccessCount,
--	SUM(f.Number) AS FailureCount,
--	CONVERT(DECIMAL(4,3),(CONVERT(DECIMAL(8,3),SUM(s.Number)))/(CONVERT(DECIMAL(8,3),SUM(c.Number)))) AS SuccessRate
--FROM Counts AS c
--OUTER APPLY (
--	SELECT EquipmentName, SUM(Number) AS Number
--	FROM Counts
--	WHERE c.EquipmentName = EquipmentName AND c.EquipmentResult = EquipmentResult AND EquipmentResult = 'Success'
--	GROUP BY EquipmentName
--) AS s
--OUTER APPLY (
--	SELECT EquipmentName, SUM(Number) AS Number
--	FROM Counts
--	WHERE c.EquipmentName = EquipmentName AND c.EquipmentResult = EquipmentResult AND EquipmentResult <> 'Success'
--	GROUP BY EquipmentName
--) AS f
--GROUP BY c.EquipmentName


END