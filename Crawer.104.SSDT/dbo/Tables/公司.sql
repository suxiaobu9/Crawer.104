CREATE TABLE [dbo].[公司] (
    [ID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [公司編號] VARCHAR (50)   NOT NULL,
    [公司名稱] NVARCHAR (MAX) NULL,
    [性質]   NVARCHAR (10)  NULL,
    [公司網址] NVARCHAR (MAX) NULL,
    [福利]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_公司] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_公司]
    ON [dbo].[公司]([公司編號] ASC);

