--use master
--go
--create Database BookDB
use BookDB
go

create table Customer(
CustomerId int primary key identity not null,
CustomerName varchar(30) not null,
BirthDate datetime not null,
IsRegular bit not null,
Picture varchar(max) not null
)
go
create table Book(
BookId int primary key identity not null,
BookName varchar(30)
)
go
create table BookEntry(
BookEntryId int primary key identity not null,
BookId int REFERENCES Book(BookId) not null,
CustomerId int REFERENCES Customer(CustomerId) not null
)
go