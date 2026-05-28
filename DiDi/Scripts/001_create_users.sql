IF DB_ID(N'DiDi') IS NULL
BEGIN
    CREATE DATABASE DiDi;
END;
GO

USE DiDi;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        Email NVARCHAR(256) NOT NULL,
        PasswordHash CHAR(64) NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        Age INT NULL,
        Gender NVARCHAR(10) NOT NULL,
        Province NVARCHAR(50) NOT NULL,
        City NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME2(7) NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME()
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Users_Email' AND object_id = OBJECT_ID(N'dbo.Users'))
BEGIN
    CREATE UNIQUE INDEX IX_Users_Email ON dbo.Users (Email);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Users_Name_Age_Gender' AND object_id = OBJECT_ID(N'dbo.Users'))
BEGIN
    CREATE INDEX IX_Users_Name_Age_Gender ON dbo.Users (Name, Age, Gender);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Users_City_Gender' AND object_id = OBJECT_ID(N'dbo.Users'))
BEGIN
    CREATE INDEX IX_Users_City_Gender ON dbo.Users (City, Gender);
END;
GO

DECLARE @GenderM NVARCHAR(10) = N'男';
DECLARE @GenderF NVARCHAR(10) = N'女';
DECLARE @DefaultPasswordHash CHAR(64) = 'E7CF3EF4F17C3999A94F2C6F612E8A888E5B1026878E4E19398B23BD38EC221A';

DECLARE @Guangdong NVARCHAR(50) = N'廣東';
DECLARE @Guangzhou NVARCHAR(50) = N'廣州';
DECLARE @Shenzhen NVARCHAR(50) = N'深圳';
DECLARE @Zhuhai NVARCHAR(50) = N'珠海';
DECLARE @Fujian NVARCHAR(50) = N'福建';
DECLARE @Fuzhou NVARCHAR(50) = N'福州';
DECLARE @Xiamen NVARCHAR(50) = N'廈門';
DECLARE @Guangxi NVARCHAR(50) = N'廣西';
DECLARE @Nanning NVARCHAR(50) = N'南寧';
DECLARE @Guilin NVARCHAR(50) = N'桂林';
DECLARE @Shanghai NVARCHAR(50) = N'上海';
DECLARE @Beijing NVARCHAR(50) = N'北京';

DECLARE @ZhangSan NVARCHAR(100) = N'張三';
DECLARE @LiSi NVARCHAR(100) = N'李四';
DECLARE @WangWu NVARCHAR(100) = N'王五';
DECLARE @ZhaoLiu NVARCHAR(100) = N'趙六';
DECLARE @ChenQi NVARCHAR(100) = N'陳七';
DECLARE @LinBa NVARCHAR(100) = N'林八';
DECLARE @HuangJiu NVARCHAR(100) = N'黃九';
DECLARE @WuShi NVARCHAR(100) = N'吳十';
DECLARE @ZhouYi NVARCHAR(100) = N'周一';
DECLARE @XuEr NVARCHAR(100) = N'徐二';
DECLARE @SunSan NVARCHAR(100) = N'孫三';
DECLARE @GaoSi NVARCHAR(100) = N'高四';

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'zhang.san@example.com')
BEGIN
    INSERT INTO dbo.Users (Email, PasswordHash, Name, Age, Gender, Province, City)
    VALUES
        (N'zhang.san@example.com', @DefaultPasswordHash, @ZhangSan, 26, @GenderM, @Guangdong, @Guangzhou),
        (N'li.si@example.com', @DefaultPasswordHash, @LiSi, 25, @GenderF, @Shanghai, @Shanghai),
        (N'wang.wu@example.com', @DefaultPasswordHash, @WangWu, 20, @GenderM, @Guangdong, @Shenzhen),
        (N'zhao.liu@example.com', @DefaultPasswordHash, @ZhaoLiu, 31, @GenderF, @Guangdong, @Guangzhou),
        (N'chen.qi@example.com', @DefaultPasswordHash, @ChenQi, 29, @GenderM, @Guangdong, @Zhuhai),
        (N'lin.ba@example.com', @DefaultPasswordHash, @LinBa, 34, @GenderF, @Fujian, @Fuzhou),
        (N'huang.jiu@example.com', @DefaultPasswordHash, @HuangJiu, 22, @GenderM, @Fujian, @Xiamen),
        (N'wu.shi@example.com', @DefaultPasswordHash, @WuShi, 27, @GenderF, @Guangxi, @Nanning),
        (N'zhou.yi@example.com', @DefaultPasswordHash, @ZhouYi, 24, @GenderM, @Guangxi, @Guilin),
        (N'xu.er@example.com', @DefaultPasswordHash, @XuEr, 30, @GenderF, @Beijing, @Beijing),
        (N'sun.san@example.com', @DefaultPasswordHash, @SunSan, 28, @GenderM, @Beijing, @Beijing),
        (N'gao.si@example.com', @DefaultPasswordHash, @GaoSi, 23, @GenderF, @Guangdong, @Shenzhen);
END;
GO
