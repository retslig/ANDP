

CREATE VIEW [gdo].[ProvisioningEngineItemActionTypesSettings_VW]
AS
SELECT p.[ProvisioningEngineSettingsId],
	  a.NAme
  FROM [gdo].[ProvisioningEngineItemActionTypesSettings] p
  join [gdo].[ActionTypeEnum] a on a.id = p.actiontypeenumid



