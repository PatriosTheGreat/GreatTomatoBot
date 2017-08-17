create table Users (
	Id int IDENTITY(1,1) PRIMARY KEY,
	Nickname varchar(255) NULL,
	FirstName varchar(255) NULL,
	UserId varchar(255) NOT NULL,
	ConversationId varchar(255) NOT NULL,
	Score int NOT NULL
);

create table Memeses (
	Id int IDENTITY(1,1) PRIMARY KEY,
	SendTime datetime NOT NULL,
	UserId int NOT NULL,
	Hash varchar(255) NOT NULL,
	ConversationId varchar(255) NOT NULL
);

create table Messages (
	ConversationId varchar(255) NOT NULL,
	UserId varchar(255) NOT NULL,
	WordsCount int NOT NULL
);