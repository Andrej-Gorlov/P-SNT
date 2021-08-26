CREATE TABLE [dbo].[EnergyAuditConsulting_2_25_SO] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [F_I_O]  NVARCHAR (50) NULL,
    [Region] NVARCHAR (50) NULL,
    [KTP_TP] NVARCHAR (50) NULL,
    [P]      FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);