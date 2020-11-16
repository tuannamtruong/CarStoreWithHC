USE master;  
GO  
DECLARE @db_id INT;  
SET @db_id = DB_ID('CarStoreKilled');  
IF @db_id IS NOT NULL 
	BEGIN
	ALTER DATABASE CarStoreKilled SET SINGLE_USER WITH ROLLBACK IMMEDIATE	
	ALTER DATABASE CarStoreKilled MODIFY NAME = CarStore ;
	ALTER DATABASE CarStore SET MULTI_USER
	print 'Server is rebooted'
	END