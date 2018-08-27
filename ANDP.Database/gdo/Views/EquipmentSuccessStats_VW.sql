



CREATE VIEW [gdo].[EquipmentSuccessStats_VW]
AS
WITH Base AS (
	SELECT 
		o.id AS OrderId, 
		o.ExternalOrderId,	
		o.ExternalAccountId, 
		o.ExternalCompanyId, 
		es.Id as EquipmentId,
		es.Name as EquipmentName,
		it.Name as ItemType, 
		at.Name as EquipmentAction,
		st.Name as EquipmentResult,
		e.DateCreated as EquipmentReceivedDate,
		e.ProvisionDate as EquipmentProvisionDate,
		e.CompletionDate,
		e.StartDate,
		DATEDIFF(Second, i.StartDate, i.CompletionDate) as ProvisionTimeInSeconds,
		i.ResultMessage
	FROM [gdo].[Order] as o
	JOIN  [gdo].[Service]  as s on s.OrderId = o.Id
	JOIN  [gdo].[Item] as i on i.ServiceId = s.Id
	JOIN gdo.Equipment AS e on e.ItemId = i.Id
	JOIN [gdo].[EquipmentSetup] AS es on es.Id = e.EquipmentSetupId
	JOIN [gdo].[StatusTypeEnum] as st on st.Id = e.StatusTypeId
	JOIN [gdo].ActionTypeEnum as at on at.Id = e.ActionTypeId
	JOIN [gdo].ItemTypeEnum as it on it.id = i.ItemTypeId
	WHERE
		1 = 1
		AND CAST(I.CompletionDate As Date) >= CAST('2016-05-01' As Date)  
	--ORDER BY CompletionDate DESC
),
Counts AS (
		SELECT
			EquipmentName, EquipmentResult, COUNT(*) AS Number
		FROM Base
		GROUP BY EquipmentName, EquipmentResult
)

SELECT 
	c.EquipmentName,
	SUM(c.Number) AS TotalCount,
	SUM(s.Number) AS SuccessCount,
	SUM(f.Number) AS FailureCount,
	CONVERT(DECIMAL(4,3),(CONVERT(DECIMAL(8,3),SUM(s.Number)))/(CONVERT(DECIMAL(8,3),SUM(c.Number)))) AS SuccessRate
FROM Counts AS c
OUTER APPLY (
	SELECT EquipmentName, SUM(Number) AS Number
	FROM Counts
	WHERE c.EquipmentName = EquipmentName AND c.EquipmentResult = EquipmentResult AND EquipmentResult = 'Success'
	GROUP BY EquipmentName
) AS s
OUTER APPLY (
	SELECT EquipmentName, SUM(Number) AS Number
	FROM Counts
	WHERE c.EquipmentName = EquipmentName AND c.EquipmentResult = EquipmentResult AND EquipmentResult <> 'Success'
	GROUP BY EquipmentName
) AS f
GROUP BY c.EquipmentName