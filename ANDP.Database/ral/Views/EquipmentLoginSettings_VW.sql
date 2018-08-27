

create VIEW [ral].[EquipmentLoginSettings_VW]
AS
SELECT es.[Name] as EquipmentName
      ,es.Id as EquipmentId
	  ,c.Name as CompanyName
	  ,ete.Name as EquipmentType
	  ,eete.Name as EquipmentConnectionType
	  ,eete2.Name as EquipmentEncodingType
      ,ecs.[Url]
      ,ecs.[Ip]
      ,eivte.Name as EquipmentIpVersionType
      ,ecs.[Port]
      ,eate.Name as EquipmentAuthenticationType
      ,ecs.[Username]
      ,ecs.[Password]
      ,ecs.[ShowTelnetCodes]
      ,ecs.[RemoveNonPrintableChars]
      ,ecs.[ReplaceNonPrintableChars]
      ,ecs.[CustomBool1]
      ,ecs.[CustomString1]
      ,ecs.[CustomInt1]
      ,ecs.[CustomBool2]
      ,ecs.[CustomString2]
      ,ecs.[CustomInt2]
      ,ecs.[CustomBool3]
      ,ecs.[CustomString3]
      ,ecs.[CustomInt3]
	  ,ecls.[SequenceNumber]
	  ,ecls.[Command]
	  ,ecls.[ExpectedResponse]
	  ,ecls.[Timeout]
  FROM [ral].[EquipmentSetup] es
  join [ral].EquipmentTypeEnum ete on ete.id = es.[EquipmentTypeId]
  join [ral].Companies c on c.id = es.CompanyId
  Join [ral].EquipmentConnectionSettings ecs on ecs.Id = es.EquipmentConnectionSettingsId
  join [ral].EquipmentConnectionTypeEnum eete on eete.id = ecs.[EquipmentConnectionTypeId]
  join [ral].EquipmentEncodingTypeEnum eete2 on eete2.Id = ecs.[EquipmentEncodingTypeId]
  join [ral].EquipmentAuthenticationTypeEnum eate on eate.Id = ecs.EquipmentAuthenticationTypeId
  join [ral].EquipmentIpVersionTypeEnum eivte on eivte.Id = ecs.EquipmentIpVersionTypeId
  left join [ral].EquipmentConnectionLoginSequences ecls on ecls.EquipmentConnectionSettingsId = ecs.id


