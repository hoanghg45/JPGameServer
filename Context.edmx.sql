
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/14/2023 13:58:34
-- Generated from EDMX file: C:\Users\hoang.nnm\source\repos\JPGame\Context.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [JP-GAME];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Gift_PersonalGift]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Gift] DROP CONSTRAINT [FK_Gift_PersonalGift];
GO
IF OBJECT_ID(N'[dbo].[FK_Gift_SpecialMemory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Gift] DROP CONSTRAINT [FK_Gift_SpecialMemory];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberCard_Account]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberCard] DROP CONSTRAINT [FK_MemberCard_Account];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberCard_MemberCardLevel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberCard] DROP CONSTRAINT [FK_MemberCard_MemberCardLevel];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberCardLevel_CardLevel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberCardLevel] DROP CONSTRAINT [FK_MemberCardLevel_CardLevel];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberCardLevel_Gift]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberCardLevel] DROP CONSTRAINT [FK_MemberCardLevel_Gift];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberCardLevel_VIPGift]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberCardLevel] DROP CONSTRAINT [FK_MemberCardLevel_VIPGift];
GO
IF OBJECT_ID(N'[dbo].[FK_Modules_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Modules] DROP CONSTRAINT [FK_Modules_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Modules_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Modules] DROP CONSTRAINT [FK_Modules_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_Promotion_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Promotion] DROP CONSTRAINT [FK_Promotion_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Slider_TypeSlider]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Slider] DROP CONSTRAINT [FK_Slider_TypeSlider];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Account];
GO
IF OBJECT_ID(N'[dbo].[Blog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Blog];
GO
IF OBJECT_ID(N'[dbo].[CardLevel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardLevel];
GO
IF OBJECT_ID(N'[dbo].[Games]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Games];
GO
IF OBJECT_ID(N'[dbo].[Gift]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Gift];
GO
IF OBJECT_ID(N'[dbo].[MemberCard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MemberCard];
GO
IF OBJECT_ID(N'[dbo].[MemberCardLevel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MemberCardLevel];
GO
IF OBJECT_ID(N'[dbo].[Modules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Modules];
GO
IF OBJECT_ID(N'[dbo].[PersonalGift]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonalGift];
GO
IF OBJECT_ID(N'[dbo].[Promotion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Promotion];
GO
IF OBJECT_ID(N'[dbo].[Slider]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Slider];
GO
IF OBJECT_ID(N'[dbo].[SpecialMemory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpecialMemory];
GO
IF OBJECT_ID(N'[dbo].[TypeSlider]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TypeSlider];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[VIPGift]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VIPGift];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [AccountID] nvarchar(100)  NOT NULL,
    [AccountName] varchar(100)  NOT NULL,
    [Avatar] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [MemberCardLevelID] varchar(50)  NULL,
    [DateOfBirth] datetime  NULL,
    [Wedding] datetime  NULL,
    [Email] varchar(max)  NULL,
    [Phone] varchar(50)  NULL,
    [Password] varchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nvarchar(250)  NULL,
    [ModifyDate] datetime  NULL,
    [ModifyBy] nvarchar(250)  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'Blogs'
CREATE TABLE [dbo].[Blogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Slug] varchar(250)  NULL,
    [Title] nvarchar(max)  NULL,
    [Des] nvarchar(max)  NULL,
    [Image] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nvarchar(250)  NULL,
    [ModifyDate] datetime  NULL,
    [ModifyBy] nvarchar(250)  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'CardLevels'
CREATE TABLE [dbo].[CardLevels] (
    [ID] varchar(50)  NOT NULL,
    [LevelName] nvarchar(200)  NULL,
    [LevelFee] float  NULL,
    [Color] varchar(50)  NULL
);
GO

-- Creating table 'Games'
CREATE TABLE [dbo].[Games] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Slug] varchar(250)  NULL,
    [Title] nvarchar(max)  NULL,
    [Des] nvarchar(max)  NULL,
    [Image] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nvarchar(250)  NULL,
    [ModifyDate] datetime  NULL,
    [ModifyBy] nvarchar(250)  NULL,
    [Status] bit  NULL,
    [Hot] bit  NULL,
    [PointReview] varchar(50)  NULL
);
GO

-- Creating table 'Gifts'
CREATE TABLE [dbo].[Gifts] (
    [ID] varchar(50)  NOT NULL,
    [GiftLevelName] nvarchar(max)  NULL,
    [GiftTypeID] varchar(50)  NULL,
    [RewardRate] float  NULL,
    [PointPlus] float  NULL,
    [SpecialMemoryID] varchar(50)  NULL,
    [PersonalGiftID] varchar(50)  NULL
);
GO

-- Creating table 'MemberCards'
CREATE TABLE [dbo].[MemberCards] (
    [AccountID] nvarchar(100)  NOT NULL,
    [MemberCardLevelID] int  NOT NULL,
    [Points] float  NULL,
    [Balance] float  NULL,
    [ModifyBy] nchar(50)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nchar(50)  NULL,
    [ModifyDate] datetime  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'MemberCardLevels'
CREATE TABLE [dbo].[MemberCardLevels] (
    [LevelID] int IDENTITY(1,1) NOT NULL,
    [CardLevelID] varchar(50)  NULL,
    [GiftLevelID] varchar(50)  NULL,
    [VIP] bit  NULL,
    [VIPGiftID] varchar(50)  NULL
);
GO

-- Creating table 'Modules'
CREATE TABLE [dbo].[Modules] (
    [ModulesID] int IDENTITY(1,1) NOT NULL,
    [Address] nvarchar(max)  NULL,
    [Hotline] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [AboutMe] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nchar(50)  NULL,
    [ModifyDate] datetime  NULL,
    [ModifyBy] nchar(50)  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'PersonalGifts'
CREATE TABLE [dbo].[PersonalGifts] (
    [ID] varchar(50)  NOT NULL,
    [SpecialDay] bit  NOT NULL,
    [Holiday] bit  NULL,
    [Personal] bit  NULL
);
GO

-- Creating table 'Promotions'
CREATE TABLE [dbo].[Promotions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [From] datetime  NULL,
    [To] datetime  NULL,
    [Title] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [Rate] float  NULL,
    [Status] bit  NULL,
    [ModifyBy] nchar(50)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nchar(50)  NULL,
    [ModifyDate] datetime  NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Sliders'
CREATE TABLE [dbo].[Sliders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TypeSlider] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Des] nvarchar(max)  NULL,
    [Image] nvarchar(max)  NULL,
    [TypeGame] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nvarchar(250)  NULL,
    [ModifyDate] datetime  NULL,
    [ModifyBy] nvarchar(250)  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'SpecialMemories'
CREATE TABLE [dbo].[SpecialMemories] (
    [ID] varchar(50)  NOT NULL,
    [AvailableTemplates] bit  NULL,
    [CustomizeAvailableTemplate] bit  NULL
);
GO

-- Creating table 'TypeSliders'
CREATE TABLE [dbo].[TypeSliders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameType] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nvarchar(250)  NULL,
    [ModifyDate] datetime  NULL,
    [ModifyBy] nvarchar(250)  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] nchar(50)  NOT NULL,
    [UserName] nchar(100)  NULL,
    [Name] nchar(200)  NULL,
    [Password] nchar(200)  NULL,
    [Role] nchar(10)  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] nvarchar(250)  NULL,
    [ModifyDate] datetime  NULL,
    [ModifyBy] nvarchar(250)  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'VIPGifts'
CREATE TABLE [dbo].[VIPGifts] (
    [VIPGiftID] varchar(50)  NOT NULL,
    [Moctail] bit  NULL,
    [VipRoom] bit  NULL,
    [Discount] float  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AccountID] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([AccountID] ASC);
GO

-- Creating primary key on [Id] in table 'Blogs'
ALTER TABLE [dbo].[Blogs]
ADD CONSTRAINT [PK_Blogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'CardLevels'
ALTER TABLE [dbo].[CardLevels]
ADD CONSTRAINT [PK_CardLevels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [PK_Games]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Gifts'
ALTER TABLE [dbo].[Gifts]
ADD CONSTRAINT [PK_Gifts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [AccountID], [MemberCardLevelID] in table 'MemberCards'
ALTER TABLE [dbo].[MemberCards]
ADD CONSTRAINT [PK_MemberCards]
    PRIMARY KEY CLUSTERED ([AccountID], [MemberCardLevelID] ASC);
GO

-- Creating primary key on [LevelID] in table 'MemberCardLevels'
ALTER TABLE [dbo].[MemberCardLevels]
ADD CONSTRAINT [PK_MemberCardLevels]
    PRIMARY KEY CLUSTERED ([LevelID] ASC);
GO

-- Creating primary key on [ModulesID] in table 'Modules'
ALTER TABLE [dbo].[Modules]
ADD CONSTRAINT [PK_Modules]
    PRIMARY KEY CLUSTERED ([ModulesID] ASC);
GO

-- Creating primary key on [ID] in table 'PersonalGifts'
ALTER TABLE [dbo].[PersonalGifts]
ADD CONSTRAINT [PK_PersonalGifts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Promotions'
ALTER TABLE [dbo].[Promotions]
ADD CONSTRAINT [PK_Promotions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'Sliders'
ALTER TABLE [dbo].[Sliders]
ADD CONSTRAINT [PK_Sliders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'SpecialMemories'
ALTER TABLE [dbo].[SpecialMemories]
ADD CONSTRAINT [PK_SpecialMemories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'TypeSliders'
ALTER TABLE [dbo].[TypeSliders]
ADD CONSTRAINT [PK_TypeSliders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [VIPGiftID] in table 'VIPGifts'
ALTER TABLE [dbo].[VIPGifts]
ADD CONSTRAINT [PK_VIPGifts]
    PRIMARY KEY CLUSTERED ([VIPGiftID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AccountID] in table 'MemberCards'
ALTER TABLE [dbo].[MemberCards]
ADD CONSTRAINT [FK_MemberCard_Account]
    FOREIGN KEY ([AccountID])
    REFERENCES [dbo].[Accounts]
        ([AccountID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CardLevelID] in table 'MemberCardLevels'
ALTER TABLE [dbo].[MemberCardLevels]
ADD CONSTRAINT [FK_MemberCardLevel_CardLevel]
    FOREIGN KEY ([CardLevelID])
    REFERENCES [dbo].[CardLevels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberCardLevel_CardLevel'
CREATE INDEX [IX_FK_MemberCardLevel_CardLevel]
ON [dbo].[MemberCardLevels]
    ([CardLevelID]);
GO

-- Creating foreign key on [PersonalGiftID] in table 'Gifts'
ALTER TABLE [dbo].[Gifts]
ADD CONSTRAINT [FK_Gift_PersonalGift]
    FOREIGN KEY ([PersonalGiftID])
    REFERENCES [dbo].[PersonalGifts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Gift_PersonalGift'
CREATE INDEX [IX_FK_Gift_PersonalGift]
ON [dbo].[Gifts]
    ([PersonalGiftID]);
GO

-- Creating foreign key on [SpecialMemoryID] in table 'Gifts'
ALTER TABLE [dbo].[Gifts]
ADD CONSTRAINT [FK_Gift_SpecialMemory]
    FOREIGN KEY ([SpecialMemoryID])
    REFERENCES [dbo].[SpecialMemories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Gift_SpecialMemory'
CREATE INDEX [IX_FK_Gift_SpecialMemory]
ON [dbo].[Gifts]
    ([SpecialMemoryID]);
GO

-- Creating foreign key on [GiftLevelID] in table 'MemberCardLevels'
ALTER TABLE [dbo].[MemberCardLevels]
ADD CONSTRAINT [FK_MemberCardLevel_Gift]
    FOREIGN KEY ([GiftLevelID])
    REFERENCES [dbo].[Gifts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberCardLevel_Gift'
CREATE INDEX [IX_FK_MemberCardLevel_Gift]
ON [dbo].[MemberCardLevels]
    ([GiftLevelID]);
GO

-- Creating foreign key on [MemberCardLevelID] in table 'MemberCards'
ALTER TABLE [dbo].[MemberCards]
ADD CONSTRAINT [FK_MemberCard_MemberCardLevel]
    FOREIGN KEY ([MemberCardLevelID])
    REFERENCES [dbo].[MemberCardLevels]
        ([LevelID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberCard_MemberCardLevel'
CREATE INDEX [IX_FK_MemberCard_MemberCardLevel]
ON [dbo].[MemberCards]
    ([MemberCardLevelID]);
GO

-- Creating foreign key on [VIPGiftID] in table 'MemberCardLevels'
ALTER TABLE [dbo].[MemberCardLevels]
ADD CONSTRAINT [FK_MemberCardLevel_VIPGift]
    FOREIGN KEY ([VIPGiftID])
    REFERENCES [dbo].[VIPGifts]
        ([VIPGiftID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberCardLevel_VIPGift'
CREATE INDEX [IX_FK_MemberCardLevel_VIPGift]
ON [dbo].[MemberCardLevels]
    ([VIPGiftID]);
GO

-- Creating foreign key on [CreateBy] in table 'Modules'
ALTER TABLE [dbo].[Modules]
ADD CONSTRAINT [FK_Modules_Users]
    FOREIGN KEY ([CreateBy])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Modules_Users'
CREATE INDEX [IX_FK_Modules_Users]
ON [dbo].[Modules]
    ([CreateBy]);
GO

-- Creating foreign key on [ModifyBy] in table 'Modules'
ALTER TABLE [dbo].[Modules]
ADD CONSTRAINT [FK_Modules_Users1]
    FOREIGN KEY ([ModifyBy])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Modules_Users1'
CREATE INDEX [IX_FK_Modules_Users1]
ON [dbo].[Modules]
    ([ModifyBy]);
GO

-- Creating foreign key on [CreateBy] in table 'Promotions'
ALTER TABLE [dbo].[Promotions]
ADD CONSTRAINT [FK_Promotion_Users]
    FOREIGN KEY ([CreateBy])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Promotion_Users'
CREATE INDEX [IX_FK_Promotion_Users]
ON [dbo].[Promotions]
    ([CreateBy]);
GO

-- Creating foreign key on [TypeSlider] in table 'Sliders'
ALTER TABLE [dbo].[Sliders]
ADD CONSTRAINT [FK_Slider_TypeSlider]
    FOREIGN KEY ([TypeSlider])
    REFERENCES [dbo].[TypeSliders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Slider_TypeSlider'
CREATE INDEX [IX_FK_Slider_TypeSlider]
ON [dbo].[Sliders]
    ([TypeSlider]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------