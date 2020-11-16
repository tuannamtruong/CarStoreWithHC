taskkill /IM "Backup-health-service.exe" /F
taskkill /IM "Main-health-service.exe" /F

start "" /MIN "C:\Users\ttnam\Desktop\Seminar 2\Project\Batch\Backup-health-service.exe" 
start "" /MIN "C:\Users\ttnam\Desktop\Seminar 2\Project\Batch\Main-health-service.exe" 
