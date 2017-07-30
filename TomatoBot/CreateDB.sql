create table Users (
	Id int NOT NULL AUTO_INCREMENT,
	Nickname varchar(255) NULL,
	FirstName varchar(255) NULL,
	UserId varchar(255) NOT NULL,
	ConversationId varchar(255) NOT NULL,,
	Score int NOT NULL,
	PRIMARY KEY (Id)
);

create table Memeses (
	Id int NOT NULL AUTO_INCREMENT,
	SendTime datetime NOT NULL,
	UserId int NOT NULL
	PRIMARY KEY (Id)
);