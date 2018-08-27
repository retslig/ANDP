


CREATE PROCEDURE [dbo].[SP_PerformanceMaintenance] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT off;

	--DECLARE @SchemaNames table(SchemaName varchar(255))
	DECLARE @SchemaName varchar(255)
	DECLARE @TableName varchar(255)
	DECLARE @Command varchar(255)

	--INSERT INTO @SchemaNames 
	--VALUES ('test'), ('ctc'), ('srtc')
	--SELECT 'test' UNION ALL SELECT 'srtc' UNION ALL SELECT 'ctc';

	DECLARE SchemaCursor CURSOR FOR
	--SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA
	--SELECT SchemaName from @SchemaNames
	SELECT distinct(table_schema) FROM information_schema.tables 
	WHERE table_type = 'base table'
		OPEN SchemaCursor
			FETCH NEXT FROM SchemaCursor INTO @SchemaName
			WHILE @@FETCH_STATUS = 0
			BEGIN

				DECLARE TableCursor CURSOR FOR 
				SELECT distinct(table_name) FROM information_schema.tables 
				WHERE table_schema = @SchemaName and table_type = 'base table' 

				OPEN TableCursor 
					FETCH NEXT FROM TableCursor INTO @TableName 
					WHILE @@FETCH_STATUS = 0 
					BEGIN 
						--DBCC DBREINDEX(@TableName,' ',90) 
						SET @Command = 'ALTER INDEX ALL ON ' + @SchemaName + '.' + @TableName + ' REBUILD';
						EXECUTE(@Command);
						--ALTER INDEX ALL ON @TableName REBUILD;
						FETCH NEXT FROM TableCursor INTO @TableName 
					END 
				CLOSE TableCursor 
				--DEALLOCATE TableCursor
			END
		CLOSE SchemaCursor
	DEALLOCATE TableCursor
	DEALLOCATE SchemaCursor


	--rebuild indexes
	--EXECUTE SP_RebuildIndex 'srtc', 'equipment';
	--ALTER INDEX ALL ON srtc.equipment REBUILD;
	--REBUILD WITH 
	--(
	--	FILLFACTOR = 80, 
	--	SORT_IN_TEMPDB = ON,
	--	STATISTICS_NORECOMPUTE = ON,
	--	ONLINE = ON ( WAIT_AT_LOW_PRIORITY ( MAX_DURATION = 4 MINUTES, ABORT_AFTER_WAIT = BLOCKERS ) ), 
	--	DATA_COMPRESSION = ROW
	--);

	--do last
    --EXECUTE SP_UpdateStatistics;
	EXECUTE sp_updatestats;
END



