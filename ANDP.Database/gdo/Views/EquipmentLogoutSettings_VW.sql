﻿


create VIEW [gdo].[EquipmentLogoutSettings_VW]
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
  FROM [gdo].[EquipmentSetup] es
  join [gdo].EquipmentTypeEnum ete on ete.id = es.[EquipmentTypeId]
  join [gdo].Companies c on c.id = es.CompanyId
  Join [gdo].EquipmentConnectionSettings ecs on ecs.Id = es.EquipmentConnectionSettingsId
  join [gdo].EquipmentConnectionTypeEnum eete on eete.id = ecs.[EquipmentConnectionTypeId]
  join [gdo].EquipmentEncodingTypeEnum eete2 on eete2.Id = ecs.[EquipmentEncodingTypeId]
  join [gdo].EquipmentAuthenticationTypeEnum eate on eate.Id = ecs.EquipmentAuthenticationTypeId
  join [gdo].EquipmentIpVersionTypeEnum eivte on eivte.Id = ecs.EquipmentIpVersionTypeId
  left join [gdo].EquipmentConnectionLogoutSequences ecls on ecls.EquipmentConnectionSettingsId = ecs.id


