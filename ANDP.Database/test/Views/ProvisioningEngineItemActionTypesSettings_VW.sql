

CREATE VIEW [test].[ProvisioningEngineItemActionTypesSettings_VW]
AS
SELECT p.[ProvisioningEngineSettingsId],
	  a.NAme
  FROM [test].[ProvisioningEngineItemActionTypesSettings] p
  join [test].[ActionTypeEnum] a on a.id = p.actiontypeenumid



