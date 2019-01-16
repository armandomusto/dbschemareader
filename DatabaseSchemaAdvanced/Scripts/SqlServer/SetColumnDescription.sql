Create Procedure #SetColumnDescription(@schemaName sysname, @tableName sysname, @columnName sysname, @description sql_variant)
As
	If Exists (Select 1 From fn_listextendedproperty('MS_Description', 'SCHEMA', @schemaName, 'TABLE', @tableName, 'COLUMN', @columnName))
		EXEC sys.sp_updateextendedproperty @name=N'Description', @value=N'Test122' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProfiles', @level2type=N'COLUMN',@level2name=N'ID'
	else If (Not @description Is Null) And (Not @description = '')
		EXEC sys.sp_addextendedproperty @name=N'Description', @value=N'Test' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserProfiles', @level2type=N'COLUMN',@level2name=N'ID'


--Read extended properties
SELECT 
'Table' AS PropertyType
,SCH.name AS SchemaName
,TBL.name AS TableName
,SEP.name AS DescriptionType
,SEP.value AS DescriptionDefinition
FROM sys.tables TBL
 INNER JOIN sys.schemas SCH
 ON TBL.schema_id = SCH.schema_id 
 INNER JOIN sys.extended_properties SEP
 ON TBL.object_id = SEP.major_id 
WHERE SEP.class = 1 And SEP.name='Description'

