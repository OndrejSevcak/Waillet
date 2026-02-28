

# Waillet – Product Specification

## 1. Product Overview

### 1.1 Product Name

**Waillet**

### 1.2 Product Vision

Waillet is a secure, ledger-driven digital wallet platform that allows users to manage multi-asset balances, execute transfers, perform asset swaps, and maintain an immutable financial transaction history.

The system is designed with:

* Strong financial consistency guarantees
* Ledger-based accounting
* Clean Architecture principles
* Scalability and extensibility
* Event-driven capabilities for future expansion

---

## 2. Objectives

### 2.1 Business Objectives

* Provide a secure and auditable wallet infrastructure
* Support multi-asset accounting
* Enable real-time balance tracking
* Allow extensibility for swaps, fees, and integrations
* Serve as a foundation for financial or crypto-based platforms

### 2.2 Technical Objectives

* Implement double-entry ledger system
* Ensure transactional integrity (ACID)
* Follow Clean Architecture
* Be cloud-deployable and horizontally scalable

---

## 3. Scope

### 3.1 In Scope

* User account management
* Wallet accounts
* Ledger entries (immutable)
* Deposits
* Withdrawals
* Internal transfers
* Asset swaps
* Transaction references
* API-based access

### 3.2 Out of Scope (Phase 1)

* Real payment gateway integrations
* Blockchain node integration
* KYC/AML
* Regulatory reporting
* Fiat on/off ramp

---

## 4. Core Functional Domains

---

## 4.1 Identity & User Management

### Purpose

Manage authenticated users and associate them with wallet accounts.

### Functional Requirements

* User registration
* Secure password storage
* Authentication
* Role-based authorization (Admin, User)

### Entities

* `User`
* `Role`
* `UserRole`

### Technical Considerations

* ASP.NET Identity
* JWT-based authentication
* Password hashing (PBKDF2 / ASP.NET Identity default)

---

## 4.2 Wallet Accounts

### Purpose

Represents a financial account owned by a user.
Each user will have a separate wallet account for each crypto asset.

### Functional Requirements

* Create wallet account
* Retrieve wallet balance per asset
* Retrieve account transaction history

### Entity: WalletAccount

- acckey
- useKey
- asset
- createdAt

### Business Rules

* One user can have multiple wallet accounts, one for each crypto asset
* Wallet accounts cannot be deleted if ledger entries exist

---

## 4.3 Ledger System (Core Component)

### Purpose

Maintain an immutable financial transaction record using double-entry accounting.

### Design Principles

* Ledger entries are append-only
* No update or delete allowed
* All balance calculations derive from ledger entries
* Every financial operation creates ≥ 1 ledger entries

---

### Entity: LedgerEntry

- Id
- AccKey
- Asset
- Amount
- Type (Deposit, Withdrawal, SwapDebit, SwapCredit, Fee)
- ReferenceId (ID of the business entity that caused this ledger entry - SwapId, DepositId, WithdrawalId..)
- ReferenceType (what kind of business operation it was - swap, deposit, withdrawal)
- CreatedAt

#### What is the concept behind ReferenceId and ReferenceType

- ReferenceId and ReferenceType are crucial for traceability and consistency.
- They exist to answer one fundamental question:
    “Why does this ledger entry exist?”

-A ledger entry should never exist in isolation.
- It must be the financial effect of a business operation
- *ReferenceId + ReferenceType link the ledger row to a business operation.*

#### Why This Is Architecturally Important

In financial systems, your ledger must be:

- Immutable
- Auditable
- Reconstructable

You must be able to:

- Rebuild balances from ledger entries
- Investigate issues
- Trace fees
- Trace swap calculations
- Reverse transactions safely

Without ReferenceId and ReferenceType, you lose audit traceability.

#### Example: BTC → ETH Swap

Suppose user swaps:

1 BTC → 14 ETH
Fee: 0.01 BTC

You would likely create 3 ledger entries:

| Id | Asset | Amount | EntryType  | ReferenceType | ReferenceId |
| -- | ----- | ------ | ---------- | ------------- | ----------- |
| 1  | BTC   | -1.00  | SwapDebit  | Swap          | 123456      |
| 2  | BTC   | -0.01  | Fee        | Swap          | 123456      |
| 3  | ETH   | +14.00 | SwapCredit | Swap          | 123456      |

#### Example Deposit

User deposits 0.5 BTC:

| Asset | Amount | EntryType | ReferenceType | ReferenceId |
| ----- | ------ | --------- | ------------- | ----------- |
| BTC   | +0.5   | Deposit   | Deposit       | 5484651   |


---

### Balance Calculation

Balance is computed based on ledger entries, never stored 

---

## 4.4 Deposits

### Purpose

Increase wallet balance.

### Flow

1. Validate request
2. Create LedgerEntry (positive amount)
3. Save transaction

### Ledger Entry Example

| Amount | EntryType | ReferenceType |
| ------ | --------- | ------------- |
| +100   | Deposit   | Deposit       |

---

## 4.5 Withdrawals

### Purpose

Decrease wallet balance.

### Flow

1. Validate available balance
2. Create LedgerEntry (negative amount)
3. Persist transaction

### Ledger Entry Example

| Amount | EntryType  | ReferenceType |
| ------ | ---------- | ------------- |
| -50    | Withdrawal | Withdrawal    |

### Business Rule

Withdrawal must not allow negative balance unless overdraft feature is enabled.

---

## 4.6 Asset Swap

### Purpose

Convert one asset to another within the same wallet.

### Example

Swap 100 USD → 0.002 BTC

### Flow

1. Validate USD balance
2. Calculate conversion rate
3. Create two ledger entries:

   * Debit USD
   * Credit BTC
4. Optionally add fee entry

### Ledger Example

| Asset | Amount | EntryType  |
| ----- | ------ | ---------- |
| USD   | -100   | SwapDebit  |
| BTC   | +0.002 | SwapCredit |
| USD   | -1     | Fee        |

---

## 4.7 Transfers (Internal)

### Purpose

Move funds between wallet accounts.

### Flow

1. Validate sender balance
2. Create debit entry for sender
3. Create credit entry for receiver
4. Use shared ReferenceId

---

## 5. Non-Functional Requirements

### 5.1 Consistency

* All financial operations must run inside a database transaction.

### 5.2 Precision

* Use `DECIMAL(38,18)`
* Never use floating point types

### 5.3 Performance

* Index on:

  * AccKey
  * ReferenceId
  * Asset

### 5.4 Security

* JWT authentication
* HTTPS only
* Role-based authorization
* Input validation
* Audit logging

### 5.5 Scalability

* Stateless API
* Horizontal scaling supported
* Background workers optional

---

## 6. High-Level Architecture

### Architecture Style

Clean Architecture

### Layers

#### 1. Domain

* Entities
* Value Objects
* Business Rules
* Domain Events

#### 2. Application

* Use Cases
* Interfaces
* DTOs
* Validators

#### 3. Infrastructure

* EF Core
* Repositories
* External services

#### 4. API

* Controllers
* Middleware
* Authentication

---

## 7. Data Integrity Strategy

* All financial operations wrapped in transactions
* Ledger entries immutable
* No balance column stored
* Optional future:

  * Snapshot table for performance
  * Event sourcing support

---

## 8. API Endpoints (Initial Design)

### Authentication

* POST `/api/auth/register`
* POST `/api/auth/login`

### Wallet

* GET `/api/wallets`
* POST `/api/wallets`

### Transactions

* POST `/api/wallets/{id}/deposit`
* POST `/api/wallets/{id}/withdraw`
* POST `/api/wallets/{id}/swap`
* POST `/api/wallets/transfer`

### Ledger

* GET `/api/wallets/{id}/ledger`

---

## 9. Future Enhancements

* Redis caching for balance snapshots
* Event-driven processing with Kafka
* Real-time updates via SignalR
* AI fraud detection module
* Blockchain integration
* Multi-tenant support
* Rate limiting
* Admin dashboard

---

## 10. Risks

| Risk                    | Mitigation             |
| ----------------------- | ---------------------- |
| Race conditions         | DB transactions        |
| Double spend            | Row-level locking      |
| Precision loss          | DECIMAL(38,18)         |
| Ledger tampering        | Immutable table policy |
| Performance degradation | Snapshot balances      |

---

## 11. Definition of Done

* All financial operations are ACID compliant
* Ledger entries are immutable
* 100% unit test coverage on domain logic
* Integration tests for financial flows
* API secured
* Swagger documentation complete

---
