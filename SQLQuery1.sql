create table Medicine_Record ( 
id int primary key not null identity(1,1),
Medicine varchar(100),
Unit_Price float , 
Location varchar(10),
Stock int,
Manufacturer Varchar(50),
Supplier varchar(50),
Mfg_Date date,
Expiry_Date date
 )
  
 SET IDENTITY_INSERT Medicine_Record ON

 create table login (
 id int primary key,
 U_Name varchar(25),
 Password varchar(25)
 )


 create table rec1(
 R_Id int primary key not null Identity(1,1),
 Customer_Name varchar(50),
 Date date not null,
 Total float not null,
 disc float null,
 disc_percentage float null,
 Grand_Total float not null
 );

 create table rec2(
 S_No int not null primary key Identity(1,1),
 R_Id int not null foreign key references rec1(R_Id),
 Medicine varchar(50) not null,
 Unit_Price float not null,
 Qty int not null,
 Sub_Total float not null
 )

 insert into login values (1,'admin','admin')
 insert into login values (2,'salesman','salesman')


select Sum(Unit_Price*Stock) as [Total Price] from Medicine_Record
ALTER TABLE medicine_record ADD Mfg_Date date default Null , Expiry_Date date default Null
SET IDENTITY_INSERT Medicine_Record ON
drop table rec2
select rec1.R_Id , rec1.Customer_Name , rec1.date , rec2.Medicine , rec2.Unit_Price , rec2.Qty , rec2.Sub_Total , rec1.Total_Amount from rec1 inner join rec2 on rec1.R_Id=rec2.R_Id where day(rec1.date)='04' and YEAR(rec1.date)='2019'
select r_id  from rec1 where day(rec1.date)='11'
select sum(Total_Amount) from rec1 where Customer_Name='321' and Date='2019-11-04'
select sum(Total_Amount) from rec1 where Date='2019-11-04'
select GETDATE()
select * from Medicine_Record where id=2959
select * from rec2 where R_Id=9
delete from rec1 where R_Id=5
update medicine_record set mfg_date='2/10/2016' where id=2959
SET DATEFORMAT Mdy
Insert into Medicine_Record values('qe',20,20,20,'ABBOT','ijaz','23/10/2018','23/10/2019')
SELECT DATEDIFF(month,'8-18-2017', '5-18-2019') AS DateDiff;
select * from medicine_record where DATEDIFF(month ,  CONVERT(date, getdate()), expiry_date)<=6
SELECT CONVERT(date, getdate())
select * from Medicine_Record

DBCC USEROPTIONS;