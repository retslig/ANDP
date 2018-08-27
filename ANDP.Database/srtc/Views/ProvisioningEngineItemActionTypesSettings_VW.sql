


CREATE VIEW [srtc].[ProvisioningEngineItemActionTypesSettings_VW]
AS
SELECT p.[ProvisioningEngineSettingsId],
	  a.NAme
  FROM [srtc].[ProvisioningEngineItemActionTypesSettings] p
  join [srtc].[ActionTypeEnum] a on a.id = p.actiontypeenumid