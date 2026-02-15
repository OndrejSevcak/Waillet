use WailletDb2
go

/*
drop table dbo.Ledger
drop table dbo.Accounts
drop table dbo.WithdrawalRequests
drop table dbo.SwapTransactions
drop table dbo.Users
*/

CREATE TABLE dbo.Users (
    UserKey BIGINT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARBINARY(MAX) NOT NULL,
    PasswordSalt VARBINARY(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsActive BIT NOT NULL DEFAULT 1
);

--Wallet accounts -> each user gets one wallet account per asset (BTC, ETH)
CREATE TABLE dbo.Accounts (
    AccKey BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserKey BIGINT NOT NULL FOREIGN KEY REFERENCES Users(UserKey),
    Asset VARCHAR(10) NOT NULL, -- BTC, ETH
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT UQ_User_Asset UNIQUE (UserKey, Asset)
);

--swaps -> user swaps from one asset to another, e.g. BTC to ETH
CREATE TABLE dbo.SwapTransactions (
    SwapId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserKey BIGINT NOT NULL FOREIGN KEY REFERENCES Users(UserKey),
    FromAsset VARCHAR(10) NOT NULL,
    ToAsset VARCHAR(10) NOT NULL,
    FromAmount DECIMAL(38,18) NOT NULL,
    ToAmount DECIMAL(38,18) NOT NULL,
    Rate DECIMAL(38,18) NOT NULL,
    FeeAmount DECIMAL(38,18) NOT NULL,
    Status VARCHAR(30) NOT NULL, -- Pending, Completed, Failed
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

--withdrawal requests -> user requests to withdraw an asset to an external address
CREATE TABLE dbo.WithdrawalRequests (
    WithId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserKey BIGINT NOT NULL FOREIGN KEY REFERENCES Users(UserKey),
    Asset VARCHAR(10) NOT NULL,
    Amount DECIMAL(38,18) NOT NULL,
    FeeAmount DECIMAL(38,18) NOT NULL,
    ToAddress NVARCHAR(255) NOT NULL,
    BlockchainTxId NVARCHAR(255) NULL,
    Status VARCHAR(30) NOT NULL, -- Pending, Broadcasting, Confirmed, Failed
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

--ledger entries
CREATE TABLE dbo.Ledger (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    AccKey BIGINT NOT NULL FOREIGN KEY REFERENCES Accounts(AccKey),
    Asset VARCHAR(10) NOT NULL, -- BTC, ETH
    Amount DECIMAL(38,18) NOT NULL, -- high precision
    Type VARCHAR(50) NOT NULL, -- Deposit, Withdrawal, SwapDebit, SwapCredit, Fee
    ReferenceId BIGINT NOT NULL, --SwapId for swaps, WithId withdrawals, DepId for deposits
    ReferenceType VARCHAR(50) NOT NULL, -- Swap, Withdrawal, Deposit
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);