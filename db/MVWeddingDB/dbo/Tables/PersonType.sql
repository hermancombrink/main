CREATE TABLE [dbo].[PersonType] (
    [ID]             INT          IDENTITY (1, 1) NOT NULL,
    [PersonTypeDesc] VARCHAR (50) NULL,
    CONSTRAINT [PK_PersonType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

