CREATE TABLE [dbo].[WeddingPhotos] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [RelavivePath] VARCHAR (MAX) NULL,
    [DateCreated]  DATETIME      CONSTRAINT [DF_WeddingPhotos_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified] DATETIME      CONSTRAINT [DF_WeddingPhotos_DateModified] DEFAULT (getdate()) NOT NULL,
    [WeddingID]    INT           NOT NULL,
    [GalleryOrder] INT           CONSTRAINT [DF_WeddingPhotos_GalleryOrder] DEFAULT ((1)) NOT NULL,
    [Name]         VARCHAR (50)  NULL,
    CONSTRAINT [PK_WeddingPhotos] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_WeddingPhotos_Wedding] FOREIGN KEY ([WeddingID]) REFERENCES [dbo].[Wedding] ([ID])
);

