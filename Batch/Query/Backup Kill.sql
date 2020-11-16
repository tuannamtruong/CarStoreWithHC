USE master;  
GO  
DECLARE @db_id INT;  
SET @db_id = DB_ID('CarStoreBackup');  
IF @db_id IS NOT NULL 
	BEGIN
	ALTER DATABASE CarStoreBackup SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	ALTER DATABASE CarStoreBackup MODIFY NAME = CarStoreBackupKilled ;
	ALTER DATABASE CarStoreBackupKilled SET MULTI_USER
	print 'Backup Server is killed'
	END