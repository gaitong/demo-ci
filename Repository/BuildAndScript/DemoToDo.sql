USE [Database_Name]

GO

CREATE TABLE MainTask (
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name nvarchar(255) NOT NULL
)


CREATE TABLE SubTask(
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	MainTaskId int NOT NULL,
	Detail nvarchar(3000),
	Active bit
	)


INSERT INTO [dbo].[MainTask]
           ([Name])
     VALUES
           ('Backlog')

INSERT INTO [dbo].[SubTask]
           ([MainTaskId]
           ,[Detail]
           ,[Active])
     VALUES
           (1
           ,'Design database domain'
           ,0)
INSERT INTO [dbo].[SubTask]
           ([MainTaskId]
           ,[Detail]
           ,[Active])
     VALUES
           (1
           ,'Create Repository'
           ,0)
INSERT INTO [dbo].[SubTask]
           ([MainTaskId]
           ,[Detail]
           ,[Active])
     VALUES
           (1
           ,'Create Service'
           ,0)
INSERT INTO [dbo].[SubTask]
           ([MainTaskId]
           ,[Detail]
           ,[Active])
     VALUES
           (1
           ,'Create API'
           ,0)
GO
