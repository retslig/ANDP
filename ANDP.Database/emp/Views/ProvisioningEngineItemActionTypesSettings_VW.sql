

CREATE VIEW [emp].[ProvisioningEngineItemActionTypesSettings_VW]
AS
SELECT p.[ProvisioningEngineSettingsId],
	  a.NAme
  FROM [emp].[ProvisioningEngineItemActionTypesSettings] p
  join [emp].[ActionTypeEnum] a on a.id = p.actiontypeenumid



