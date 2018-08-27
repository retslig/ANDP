

CREATE VIEW [ral].[ProvisioningEngineItemActionTypesSettings_VW]
AS
SELECT p.[ProvisioningEngineSettingsId],
	  a.NAme
  FROM [ral].[ProvisioningEngineItemActionTypesSettings] p
  join [ral].[ActionTypeEnum] a on a.id = p.actiontypeenumid



