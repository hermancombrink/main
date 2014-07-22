CREATE TABLE [dbo].[Wedding] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [DateCreated]    DATETIME      CONSTRAINT [DF_Wedding_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]   DATETIME      CONSTRAINT [DF_Wedding_DateModified] DEFAULT (getdate()) NOT NULL,
    [Directions]     VARCHAR (MAX) NULL,
    [GoogleMapLink]  VARCHAR (MAX) NULL,
    [OurStory]       VARCHAR (MAX) NULL,
    [EventDetails]   VARCHAR (MAX) NULL,
    [Domain]         VARCHAR (200) NULL,
    [WeddingDate]    DATETIME      NULL,
    [ShowEvent]      BIT           CONSTRAINT [DF_Wedding_ShowEvent] DEFAULT ((0)) NOT NULL,
    [ShowStory]      BIT           CONSTRAINT [DF_Wedding_ShowStory] DEFAULT ((0)) NOT NULL,
    [ShowDirections] BIT           CONSTRAINT [DF_Wedding_ShowDirections] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Wedding] PRIMARY KEY CLUSTERED ([ID] ASC)
);

