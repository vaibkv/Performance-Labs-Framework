// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoProfilingProject
{
    public static class Constants
    {
        public static String demoDBConnectionString = @"SERVER=localhost;DATABASE=master;Integrated Security=SSPI;";

        public static String deleteDemoDB =
@"USE master;
IF DB_ID('DemoDBBlog') IS NOT NULL DROP DATABASE DemoDBBlog;";

        public static String createDemoDB =
            @"USE master;
IF DB_ID('DemoDBBlog') IS NOT NULL DROP DATABASE DemoDBBlog;

IF (NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = 'DemoDBBlog' OR name = 'DemoDBBlog')))
BEGIN
USE master;
CREATE DATABASE DemoDBBlog;
END;
";

        public static String CreateTables =
@"USE DemoDBBlog
CREATE TABLE [dbo].[Categories](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY]

USE [DemoDBBlog]
CREATE TABLE [dbo].[BlogPosts](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[PublishDate] [datetime] NOT NULL,
	[CategoryId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BlogPosts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))

ALTER TABLE [dbo].[BlogPosts] ADD  CONSTRAINT [FK_BlogPosts_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
";

        public static String populateDemoDB =
            @"USE [DemoDBBlog];
declare @countStart as int
declare @countEnd as int
declare @catId1 as uniqueidentifier
declare @catId2 as uniqueidentifier
declare @catId3 as uniqueidentifier
set @countStart = 0
set @countEnd = 150
insert into Categories(Id, Name) values(NEWID(),'.Net Framework')
insert into Categories(Id, Name) values(NEWID(),'Sql Server')
insert into Categories(Id, Name) values(NEWID(),'JQuery')
select top 1 @catId1 = Id from Categories where Name = '.Net Framework'
select top 1 @catId2 = Id from Categories where Name = 'Sql Server'
select top 1 @catId3 = Id from Categories where Name = 'JQuery'
while(@countStart < @countEnd)
begin
	set @countStart = @countStart + 1
	
	insert into BlogPosts(Title,Content,PublishDate,CategoryId,Id) values('Title_CatId1_'+cast(@countStart as varchar),'some  content', GETDATE(),@catId1,NEWID())
end
set @countStart = 0
while(@countStart < @countEnd)
begin
	set @countStart = @countStart + 1
	
	insert into BlogPosts(Title,Content,PublishDate,CategoryId,Id) values('Title_CatId2_'+cast(@countStart as varchar),'some  content', GETDATE(),@catId1,NEWID())
end
set @countStart = 0
while(@countStart < @countEnd)
begin
	set @countStart = @countStart + 1
	
	insert into BlogPosts(Title,Content,PublishDate,CategoryId,Id) values('Title_CatId3_'+cast(@countStart as varchar),'some  content', GETDATE(),@catId1,NEWID())
end";

        public static String PerformanceLabsDBContext = "Test_PerformanceLabsDBContext";
        public static Int32 smallTime = 12;
        public static Int32 bigTime = 7000;

        public static String smallQuery = @"USE [DemoDBBlog];select * from BlogPosts; select * from Categories; WAITFOR DELAY '00:00:01'";
        public static String bigQuery = @"USE [DemoDBBlog];select * from BlogPosts; select * from Categories; WAITFOR DELAY '00:00:20'";
    }
}
