use master
create database OnlineShop
go

use OnlineShop
go

CREATE TABLE [dbo].[AdminUser] (
    [ID]           INT            NOT NULL,
    [NameUser]     NVARCHAR (MAX) NULL,
    [RoleUser]     NVARCHAR (MAX) NULL,
    [PasswordUser] NCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Category] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [IDCate]   NCHAR (20)     NOT NULL,
    [NameCategory] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([IDCate] ASC)
);

CREATE TABLE [dbo].[Customer] (
    [IDCustomer]    INT            IDENTITY (1, 1) NOT NULL,
    [NameCus]  NVARCHAR (MAX) NULL,
    [PhoneCus] NVARCHAR (15)  NULL,
    [EmailCus] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([IDCustomer] ASC)
);

CREATE TABLE [dbo].[Products] (
    [ProductID]     INT             IDENTITY (1, 1) NOT NULL,
    [NameProduct]       NVARCHAR (MAX)  NULL,
    [DecriptionProduct] NVARCHAR (MAX)  NULL,
    [IDCate]      NCHAR (20)      NULL,
    [Price]         DECIMAL (18, 2) NULL,
    [ImageProduct]      NVARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC),
    CONSTRAINT [FK_Pro_Category] FOREIGN KEY ([IdCate]) REFERENCES [dbo].[Category] ([IDCate])
);

CREATE TABLE [dbo].[OrderProduct] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [DateOrder]        DATE           NULL,
    [IDCustomer]            INT            NULL,
    [AddressDeliverry] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDCustomer]) REFERENCES [dbo].[Customer] ([IDCustomer])
);

CREATE TABLE [dbo].[OrderDetail] (
    [ID]        INT        IDENTITY (1, 1) NOT NULL,
    [IDProduct] INT        NULL,
    [IDOrder]   INT        NULL,
    [Quantity]  INT        NULL,
    [UnitPrice] FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDProduct]) REFERENCES [dbo].[Products] ([ProductID]),
    FOREIGN KEY ([IDOrder]) REFERENCES [dbo].[OrderProduct] ([ID])
);