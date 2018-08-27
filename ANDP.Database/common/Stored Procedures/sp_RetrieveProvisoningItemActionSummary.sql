



CREATE PROCEDURE [common].[sp_RetrieveProvisoningItemActionSummary] @startDate datetime2, @endDate datetime2, @tenantId uniqueidentifier
AS
BEGIN
	DECLARE @sql varchar(MAX), @schema nvarchar(36)
	SELECT TOP 1 @schema = [Schema] 
	FROM [common].[Tenants] 
	WHERE Guid = @tenantId

    set @sql = 'WITH Base AS ( ' +
			'	SELECT  ' +
			'		st.Name as Result,  ' +
			'		at.Name as OrderType,  ' +
			'		it.Name as ItemType ' +
			'	FROM ['+ @schema +'].[Item] i ' +
			'	JOIN ['+ @schema +'].StatusTypeEnum st on st.Id = i.StatusTypeId ' +
			'	JOIN ['+ @schema +'].ActionTypeEnum at on at.Id = i.ActionTypeId ' +
			'	JOIN ['+ @schema +'].ItemTypeEnum it on it.Id = i.ItemTypeId ' +
			'	WHERE CAST(i.CompletionDate As Date) BETWEEN CAST(''' + CONVERT(VARCHAR(10),@startDate, 126) + ''' As Date) and CAST(''' + CONVERT(VARCHAR(10),@endDate, 126) + ''' As Date) ' +
			'), ' +
			'Counts AS ( ' +
			'		SELECT ' +
			'			ItemType, OrderType, Result, COUNT(*) AS Number ' +
			'		FROM Base ' +
			'		GROUP BY ItemType, OrderType, Result ' +
			') ' +
			' ' +
			'SELECT  ' +
			'	c.ItemType, ' +
			'	c.OrderType, ' +
			'	SUM(c.Number) AS TotalCount, ' +
			'	SUM(s.Number) AS SuccessCount, ' +
			'	SUM(f.Number) AS FailureCount, ' +
			'	CONVERT(DECIMAL(4,3),(CONVERT(DECIMAL(8,3),SUM(s.Number)))/(CONVERT(DECIMAL(8,3),SUM(c.Number)))) AS SuccessRate ' +
			'FROM Counts AS c ' +
			'OUTER APPLY ( ' +
			'	SELECT ItemType, OrderType, SUM(Number) AS Number ' +
			'	FROM Counts ' +
			'	WHERE c.ItemType = ItemType AND c.OrderType = OrderType AND c.Result = Result AND Result = ''Success'' ' +
			'	GROUP BY ItemType, OrderType ' +
			') AS s ' +
			'OUTER APPLY ( ' +
			'	SELECT ItemType, OrderType, SUM(Number) AS Number ' +
			'	FROM Counts ' +
			'	WHERE c.ItemType = ItemType AND c.OrderType = OrderType AND c.Result = Result AND Result <> ''Success'' ' +
			'	GROUP BY ItemType, OrderType ' +
			') AS f ' +
			'GROUP BY c.ItemType, c.OrderType ' 

    exec(@sql)
	--select @sql  FROM [common].[Tenants] 
	
--;WITH Base AS (
--	SELECT 
--		st.Name as Result, 
--		at.Name as OrderType, 
--		it.Name as ItemType
--	FROM [emp].[Item] i
--	JOIN [emp].StatusTypeEnum st on st.Id = i.StatusTypeId
--	JOIN [emp].ActionTypeEnum at on at.Id = i.ActionTypeId
--	JOIN [emp].ItemTypeEnum it on it.Id = i.ItemTypeId
--	WHERE CAST(i.CompletionDate As Date) between  CAST(@startDate As Date) and CAST(@endDate As Date)
--),
--Counts AS (
--		SELECT
--			ItemType, OrderType, Result, COUNT(*) AS Number
--		FROM Base
--		GROUP BY ItemType, OrderType, Result
--)

--SELECT 
--	c.ItemType,
--	c.OrderType,
--	SUM(c.Number) AS TotalCount,
--	SUM(s.Number) AS SuccessCount,
--	SUM(f.Number) AS FailureCount,
--	CONVERT(DECIMAL(4,3),(CONVERT(DECIMAL(8,3),SUM(s.Number)))/(CONVERT(DECIMAL(8,3),SUM(c.Number)))) AS SuccessRate
--FROM Counts AS c
--OUTER APPLY (
--	SELECT ItemType, OrderType, SUM(Number) AS Number
--	FROM Counts
--	WHERE c.ItemType = ItemType AND c.OrderType = OrderType AND c.Result = Result AND Result = 'Success'
--	GROUP BY ItemType, OrderType
--) AS s
--OUTER APPLY (
--	SELECT ItemType, OrderType, SUM(Number) AS Number
--	FROM Counts
--	WHERE c.ItemType = ItemType AND c.OrderType = OrderType AND c.Result = Result AND Result <> 'Success'
--	GROUP BY ItemType, OrderType
--) AS f
--GROUP BY c.ItemType, c.OrderType


END