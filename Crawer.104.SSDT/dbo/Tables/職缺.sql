CREATE TABLE [dbo].[職缺] (
    [ID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [公司ID]   BIGINT         NOT NULL,
    [出現日期]   DATETIME       NULL,
    [工作地點]   NVARCHAR (MAX) NULL,
    [工作名稱]   NVARCHAR (MAX) NULL,
    [工作編號]   VARCHAR (50)   NULL,
    [詳細內容網址] NVARCHAR (MAX) NULL,
    [年資]     NVARCHAR (10)  NULL,
    [薪水說明]   NVARCHAR (50)  NULL,
    [最低薪]    VARCHAR (10)   NULL,
    [最高薪]    VARCHAR (10)   NULL,
    [更新時間]   DATETIME       NULL,
    [建立時間]   DATETIME       NULL,
    [標記]     NVARCHAR (MAX) NULL,
    [工作內容]   NVARCHAR (MAX) NULL,
    [上班時間]   NVARCHAR (MAX) NULL,
    [要求]     NVARCHAR (MAX) NULL,
    [遠端工作]   BIT            NULL,
    [被刪除]    BIT            NULL,
    [已讀]     BIT            NULL,
    CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_職缺_公司] FOREIGN KEY ([公司ID]) REFERENCES [dbo].[公司] ([ID])
);



