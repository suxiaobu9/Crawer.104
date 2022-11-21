CREATE TABLE [dbo].[JobDescription] (
    [ID]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompanyId]         BIGINT         NOT NULL,
    [AppearDate]        DATETIME       NULL,
    [JobPlace]          NVARCHAR (MAX) NULL,
    [JobName]           NVARCHAR (MAX) NULL,
    [JobNo]             VARCHAR (50)   NOT NULL,
    [DetailUrl]         NVARCHAR (MAX) NULL,
    [Seniority]         NVARCHAR (10)  NULL,
    [SalaryDescription] NVARCHAR (50)  NULL,
    [MinimunSalary]     VARCHAR (10)   NULL,
    [HighestSalary]     VARCHAR (10)   NULL,
    [UpdatedDate]       DATETIME       NULL,
    [CreatedDate]       DATETIME       NULL,
    [Tags]              NVARCHAR (MAX) NULL,
    [WorkContent]       NVARCHAR (MAX) NULL,
    [WorkingHour]       NVARCHAR (MAX) NULL,
    [Request]           NVARCHAR (MAX) NULL,
    [RemoteWork]        BIT            NULL,
    [IsDeleted]         BIT            NULL,
    [HaveRead]          BIT            NULL,
    CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_職缺_公司] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_職缺]
    ON [dbo].[JobDescription]([JobNo] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'已讀', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'HaveRead';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'被刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'IsDeleted';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'遠端工作', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'RemoteWork';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'要求', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'Request';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上班時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'WorkingHour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'WorkContent';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'Tags';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'CreatedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'UpdatedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最高薪', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'HighestSalary';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最低薪', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'MinimunSalary';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'薪水說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'SalaryDescription';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'年資', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'Seniority';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'詳細內容網址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'DetailUrl';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'JobNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'JobName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作地點', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'JobPlace';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出現日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'JobDescription', @level2type = N'COLUMN', @level2name = N'AppearDate';

