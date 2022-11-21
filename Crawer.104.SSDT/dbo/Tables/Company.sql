CREATE TABLE [dbo].[Company] (
    [ID]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompanyNo]   VARCHAR (50)   NOT NULL,
    [CompanyName] NVARCHAR (MAX) NULL,
    [Property]    NVARCHAR (10)  NULL,
    [CompanyUrl]  NVARCHAR (MAX) NULL,
    [Welfare]     NVARCHAR (MAX) NULL,
    [Ignore]      BIT            NULL,
    CONSTRAINT [PK_公司] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_公司]
    ON [dbo].[Company]([CompanyNo] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'福利', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Welfare';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司網址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'CompanyUrl';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'性質', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Property';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'CompanyName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'CompanyNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'忽視', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Ignore';

