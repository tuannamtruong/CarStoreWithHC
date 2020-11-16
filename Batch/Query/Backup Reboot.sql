USE master;  
GO  
DECLARE @db_id INT;  
SET @db_id = DB_ID('CarStoreBackupKilled');  
IF @db_id IS NOT NULL 
	BEGIN
	ALTER DATABASE CarStoreBackupKilled SET SINGLE_USER WITH ROLLBACK IMMEDIATE	
	ALTER DATABASE CarStoreBackupKilled MODIFY NAME = CarStoreBackup ;
	ALTER DATABASE CarStoreBackup SET MULTI_USER
	print 'Backup Server is rebooted'
	END