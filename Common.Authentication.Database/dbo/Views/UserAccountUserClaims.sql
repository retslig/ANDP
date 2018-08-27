

CREATE VIEW [dbo].[UserAccountUserClaims]
AS

SELECT 
ua.tenant
,ua.username
,uc.[Type]
,uc.[Value]
,uc.ParentKey
  FROM [dbo].[UserAccounts] ua
  join [dbo].[UserClaims] uc on uc.ParentKey = ua.[Key]







