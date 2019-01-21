-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SetColumnDescription
	-- Add the parameters for the stored procedure here
	@schemaName sysname, 
	@tableName sysname, 
	@columnName sysname, 
	@description sql_variant
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    If Exists (Select 1 From fn_listextendedproperty('MS_Description', 'SCHEMA', @schemaName, 'TABLE', @tableName, 'COLUMN', @columnName))
		EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=@description , @level0type=N'SCHEMA',@level0name=@schemaName, @level1type=N'TABLE',@level1name=@tableName, @level2type=N'COLUMN',@level2name=@columnName
	else If (Not @description Is Null) And (Not @description = '')
		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@description , @level0type=N'SCHEMA',@level0name=@schemaName, @level1type=N'TABLE',@level1name=@tableName, @level2type=N'COLUMN',@level2name=@columnName
END
GO

--Read extended properties
--SELECT 
--'Table' AS PropertyType
--,SCH.name AS SchemaName
--,TBL.name AS TableName
--,SEP.name AS DescriptionType
--,SEP.value AS DescriptionDefinition
--FROM sys.tables TBL
-- INNER JOIN sys.schemas SCH
-- ON TBL.schema_id = SCH.schema_id 
-- INNER JOIN sys.extended_properties SEP
-- ON TBL.object_id = SEP.major_id 
--WHERE SEP.class = 1 And SEP.name='MS_Description'

