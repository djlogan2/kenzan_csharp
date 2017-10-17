USE [kenzan]
GO

/****** Object:  Table [dbo].[Employee]    Script Date: 10/12/2017 7:28:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [dbo].[EmployeeRoleJoin];
DROP TABLE IF EXISTS [dbo].[Employee];
DROP TABLE IF EXISTS [dbo].[EmployeeRole];

CREATE TABLE [dbo].[Employee](
	[id] [int] IDENTITY NOT NULL,
	[username] [varchar](60) NOT NULL,
	[firstName] [varchar](60) NOT NULL,
	[middleInitial] [char](1) NULL,
	[lastName] [varchar](60) NOT NULL,
	[dateOfBirth] [date] NOT NULL,
	[dateOfEmployment] [date] NULL,
	[bStatus] [int] NOT NULL,
	[password] [char](60) NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([id] ASC)
)
GO

CREATE TRIGGER InsertTrigger ON dbo.Employee
AFTER INSERT, UPDATE
AS
IF exists ( select * from Employee e 
    inner join inserted i on i.username=e.username and e.bStatus = 0 and i.id <> e.id)
BEGIN
    ROLLBACK
    RAISERROR ('Duplicate Data', 16, 1);
END
GO

CREATE TABLE [dbo].[EmployeeRole](
	[id] [int] IDENTITY NOT NULL,
	[role] [varchar](60) NOT NULL,
 CONSTRAINT [PK_EmployeeRole] PRIMARY KEY CLUSTERED ([id] ASC)
)
GO

CREATE TABLE [dbo].[EmployeeRoleJoin](
	[employee_id] [int] NOT NULL,
	[employee_role_id] [int] NOT NULL,
	CONSTRAINT [PK_EmployeeRoleJoin] PRIMARY KEY CLUSTERED 
	(
		[employee_id] ASC,
		[employee_role_id] ASC
	),
	CONSTRAINT [FK_EmployeeRoleJoin_Employee] FOREIGN KEY([employee_id]) REFERENCES [dbo].[Employee] ([id]),
	CONSTRAINT [FK_EmployeeRoleJoin_EmployeeRole] FOREIGN KEY([employee_role_id]) REFERENCES [dbo].[EmployeeRole] ([id])
)
GO

insert into employeerole (role) values ('ROLE_ADD_EMP');
insert into employeerole (role) values ('ROLE_UPDATE_EMP');
insert into employeerole (role) values ('ROLE_DELETE_EMP');
insert into employeerole (role) values ('ROLE_SET_PASSWORD');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-11-26', '2001-01-01', 'Kenzan', 'Test', 'A', 'kenzan');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-11-27', '2002-02-02', 'Kenzan', 'Test A', 'B', 'kenzana');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-11-28', '2003-03-03', 'Kenzan', 'Test AD', 'C', 'kenzanad');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-11-29', '2004-04-04', 'Kenzan', 'Test AU', 'D', 'kenzanau');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-11-30', '2005-05-05', 'Kenzan', 'Test ADU', 'E', 'kenzanadu');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-12-01', '2006-06-06', 'Kenzan', 'Test D', 'F', 'kenzand');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-12-02', '2007-07-07', 'Kenzan', 'Test DU', 'G', 'kenzandu');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-12-03', '2008-08-08', 'Kenzan', 'Test U', 'H', 'kenzanu');
insert into employee (bstatus, dateofbirth, dateofemployment, firstName, lastName, middleInitial, username) values (0, '1968-12-04', '2008-08-09', 'Kenzan', 'Test P', 'H', 'kenzanp');
update employee set password='$2a$10$i51OFodMCqcVjvDyQUt8IeYhtuMH7J6JqUXKwWWPCP00DcgHnIscG'; /* password='kenzan' */
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzana'), (select id from employeerole where role='ROLE_ADD_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanad'), (select id from employeerole where role='ROLE_ADD_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanau'), (select id from employeerole where role='ROLE_ADD_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanadu'), (select id from employeerole where role='ROLE_ADD_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzand'), (select id from employeerole where role='ROLE_DELETE_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzandu'), (select id from employeerole where role='ROLE_DELETE_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanad'), (select id from employeerole where role='ROLE_DELETE_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanadu'), (select id from employeerole where role='ROLE_DELETE_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanadu'), (select id from employeerole where role='ROLE_UPDATE_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzandu'), (select id from employeerole where role='ROLE_UPDATE_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanau'), (select id from employeerole where role='ROLE_UPDATE_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanu'), (select id from employeerole where role='ROLE_UPDATE_EMP'));
insert into EmployeeRoleJoin (employee_id, employee_role_id) values ((select id from employee where username='kenzanp'), (select id from employeerole where role='ROLE_SET_PASSWORD'));