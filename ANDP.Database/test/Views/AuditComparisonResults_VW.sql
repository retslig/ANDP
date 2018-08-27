







CREATE VIEW [test].[AuditComparisonResults_VW]
AS


SELECT 
	billingRecords.[RunNumber]
	,billingRecords.[RunDate]
	,billingRecords.[CompanyId]
	,CASE WHEN billingRecords.[BillingOrEquipmentIndicator] = 1 THEN 'Billing' ELSE 'Equipment' END AS BillingOrEquipmentIndicator
	,billingRecords.[ExternalAccountId]
	,billingRecords.[ExternalServiceId]
	,billingRecords.[ExternalItemId]
	,billingRecords.[RecordKey]
	,billingRecords.[RecordType]
	,billingRecords.[RecordValue]
	,billingRecords.[Ignore]
	,billingRecords.[MatchId]
	,billingRecords.[AddToEquiment]
	,billingRecords.[AddToBilling]
	,item.Name as ItemName
	,equipment.Name as EquipmentName
	,'Billed but not on equipment' as HasMatch 
	,2 as HasMatchSort 
FROM [test].[AuditRecords] billingRecords
JOIN [test].[ItemTypeEnum] as item on item.id = billingRecords.[ItemTypeId]
JOIN [test].[EquipmentSetup] as equipment on equipment.Id = billingRecords.[EquipmentSetupId]
WHERE NOT EXISTS (
	SELECT * FROM [test].[AuditRecords] as equipmentRecords 
	WHERE equipmentRecords.[BillingOrEquipmentIndicator] = 0
	and equipmentRecords.[RunNumber] = billingRecords.[RunNumber]
	and equipmentRecords.[CompanyId] = billingRecords.[CompanyId]
	and equipmentRecords.[EquipmentSetupId] = billingRecords.[EquipmentSetupId]
	and equipmentRecords.[ExternalAccountId] = billingRecords.[ExternalAccountId]
	and equipmentRecords.[ExternalServiceId] = billingRecords.[ExternalServiceId]
	and equipmentRecords.[ExternalItemId] = billingRecords.[ExternalItemId]
	and equipmentRecords.[ItemTypeId] = billingRecords.[ItemTypeId]
	and equipmentRecords.[RecordKey] = billingRecords.[RecordKey]
	and equipmentRecords.[RecordType] = billingRecords.[RecordType]
	and equipmentRecords.[RecordValue] = billingRecords.[RecordValue]
	and ((equipmentRecords.[MatchId] is null and billingRecords.[MatchId] is null ) or equipmentRecords.[MatchId] = billingRecords.[MatchId])
	and (equipmentRecords.[Ignore] is null or equipmentRecords.[Ignore] <> 1)
) 
--and billingRecords.[RunNumber] = 'D7DE14E8-F29C-452B-82FA-0C339A185412' 
and billingRecords.[BillingOrEquipmentIndicator] = 1
and (billingRecords.[Ignore] is null or billingRecords.[Ignore] <> 1)
union
SELECT 
	equipmentRecords.[RunNumber]
	,equipmentRecords.[RunDate]
	,equipmentRecords.[CompanyId]
	,CASE WHEN equipmentRecords.[BillingOrEquipmentIndicator] = 1 THEN 'Billing' ELSE 'Equipment' END AS BillingOrEquipmentIndicator
	,equipmentRecords.[ExternalAccountId]
	,equipmentRecords.[ExternalServiceId]
	,equipmentRecords.[ExternalItemId]
	,equipmentRecords.[RecordKey]
	,equipmentRecords.[RecordType]
	,equipmentRecords.[RecordValue]
	,equipmentRecords.[Ignore]
	,equipmentRecords.[MatchId]
	,equipmentRecords.[AddToEquiment]
	,equipmentRecords.[AddToBilling]
	,item.Name as ItemName
	,equipment.Name as EquipmentName
	,'Not billed but on equipment' as HasMatch 
	,3 as HasMatchSort 
FROM [test].[AuditRecords] equipmentRecords
JOIN [test].[ItemTypeEnum] as item on item.id = equipmentRecords.[ItemTypeId]
JOIN [test].[EquipmentSetup] as equipment on equipment.Id = equipmentRecords.[EquipmentSetupId]
where NOT EXISTS (
	SELECT * FROM [test].[AuditRecords] as billingRecords 
	WHERE billingRecords.[BillingOrEquipmentIndicator] = 1
	and billingRecords.[RunNumber] = equipmentRecords.[RunNumber]
	and billingRecords.[CompanyId] = equipmentRecords.[CompanyId]
	and billingRecords.[EquipmentSetupId] = equipmentRecords.[EquipmentSetupId]
	and billingRecords.[ExternalAccountId] = equipmentRecords.[ExternalAccountId]
	and billingRecords.[ExternalServiceId] = equipmentRecords.[ExternalServiceId]
	and billingRecords.[ExternalItemId] = equipmentRecords.[ExternalItemId]
	and billingRecords.[ItemTypeId] = equipmentRecords.[ItemTypeId]
	and billingRecords.[RecordKey] = equipmentRecords.[RecordKey]
	and billingRecords.[RecordType] = equipmentRecords.[RecordType]
	and billingRecords.[RecordValue] = equipmentRecords.[RecordValue]
	and ((billingRecords.[MatchId] is null and equipmentRecords.[MatchId] is null ) or billingRecords.[MatchId] = equipmentRecords.[MatchId])
	and (billingRecords.[Ignore] is null or billingRecords.[Ignore] <> 1)
) 
--and equipmentRecords.[RunNumber] = 'D7DE14E8-F29C-452B-82FA-0C339A185412' 
and equipmentRecords.[BillingOrEquipmentIndicator] = 0
and (equipmentRecords.[Ignore] is null or equipmentRecords.[Ignore] <> 1)
union
SELECT 
	billingRecords.[RunNumber]
	,billingRecords.[RunDate]
	,billingRecords.[CompanyId]
	,CASE WHEN billingRecords.[BillingOrEquipmentIndicator] = 1 THEN 'Billing' ELSE 'Equipment' END AS BillingOrEquipmentIndicator
	,billingRecords.[ExternalAccountId]
	,billingRecords.[ExternalServiceId]
	,billingRecords.[ExternalItemId]
	,billingRecords.[RecordKey]
	,billingRecords.[RecordType]
	,billingRecords.[RecordValue]
	,billingRecords.[Ignore]
	,billingRecords.[MatchId]
	,billingRecords.[AddToEquiment]
	,billingRecords.[AddToBilling]
	,item.Name as ItemName
	,equipment.Name as EquipmentName
	,'Matched' as HasMatch 
	,1 as HasMatchSort 
FROM [test].[AuditRecords] billingRecords
JOIN [test].[ItemTypeEnum] as item on item.id = billingRecords.[ItemTypeId]
JOIN [test].[EquipmentSetup] as equipment on equipment.Id = billingRecords.[EquipmentSetupId]
where EXISTS (
	SELECT * FROM [test].[AuditRecords] as equipmentRecords 
	WHERE equipmentRecords.[BillingOrEquipmentIndicator] = 0
	and equipmentRecords.[RunNumber] = billingRecords.[RunNumber]
	and equipmentRecords.[CompanyId] = billingRecords.[CompanyId]
	and equipmentRecords.[EquipmentSetupId] = billingRecords.[EquipmentSetupId]
	and equipmentRecords.[ExternalAccountId] = billingRecords.[ExternalAccountId]
	and equipmentRecords.[ExternalServiceId] = billingRecords.[ExternalServiceId]
	and equipmentRecords.[ExternalItemId] = billingRecords.[ExternalItemId]
	and equipmentRecords.[ItemTypeId] = billingRecords.[ItemTypeId]
	and equipmentRecords.[RecordKey] = billingRecords.[RecordKey]
	and equipmentRecords.[RecordType] = billingRecords.[RecordType]
	and equipmentRecords.[RecordValue] = billingRecords.[RecordValue]
	and ((equipmentRecords.[MatchId] is null and billingRecords.[MatchId] is null ) or equipmentRecords.[MatchId] = billingRecords.[MatchId])
	and (equipmentRecords.[Ignore] is null or equipmentRecords.[Ignore] <> 1)
) 
--and billingRecords.[RunNumber] = 'D7DE14E8-F29C-452B-82FA-0C339A185412' 
and billingRecords.[BillingOrEquipmentIndicator] = 1
and (billingRecords.[Ignore] is null or billingRecords.[Ignore] <> 1)





