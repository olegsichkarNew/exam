1) The project IntreviewExam.ImportData is application which can process the data from the XML file
2) Firstly add restore DB from exam.bak OR create exam manually and run the script.sql
3) In the App.config configure 
	connectionString, 
	BulkMode (how records will be saved in DB in one request),
	FileName (path to xml file), 
	FileNameXsd (path to xsd)
4) Compile & run IntreviewExam.ImportData
5) Check DB tables if they are filled with data