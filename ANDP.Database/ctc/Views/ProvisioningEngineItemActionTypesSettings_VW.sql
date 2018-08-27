


CREATE VIEW [ctc].[ProvisioningEngineItemActionTypesSettings_VW]
AS
SELECT p.[ProvisioningEngineSettingsId],
	  a.NAme
  FROM [ctc].[ProvisioningEngineItemActionTypesSettings] p
  join [ctc].[ActionTypeEnum] a on a.id = p.actiontypeenumid