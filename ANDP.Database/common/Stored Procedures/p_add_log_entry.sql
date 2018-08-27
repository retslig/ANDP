CREATE PROCEDURE [common].[p_add_log_entry]
@TenantId nvarchar(50),
@UniqueRecordID uniqueidentifier,
@SourceMachineName varchar(50),
@AppCode varchar(50),
@PID int,
@UserCode varchar(20),
@MessageType varchar(20),
@StackTrace varchar(2000),
@ExceptionMessage varchar(2000),
@ObjectData XML,
@DestinationMachineName varchar(50)
AS

INSERT INTO common.ApplicationLogs (TenantId, SearchKey, SourceMachineName, AppCode,PID,UserCode,LoggedDateTime,MessageType,MessageData,StackTrace,ExceptionMessage,DestinationMachineName)
 VALUES(@TenantId, @UniqueRecordID,@SourceMachineName,@AppCode,@PID,@UserCode,SYSDATETIME(),@MessageType,@ObjectData,@StackTrace,@ExceptionMessage,@DestinationMachineName);

