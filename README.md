# WailletAPI

A wallet management API built with ASP.NET Core 9.0 featuring JWT authentication.

## Features

- User registration and authentication
- JWT token-based security
- Password hashing using PBKDF2 with salt
- Account management

## Authentication

The API uses JWT (JSON Web Token) for authentication. After successful login, clients receive an access token that must be included in subsequent requests.

### Login

**Endpoint:** `POST /api/user/login`

**Request Body:**
```json
{
  "userName": "your-username",
  "password": "your-password"
}
```

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresIn": 3600
}
```

### Using the Token

Include the token in the Authorization header for protected endpoints:

```
Authorization: Bearer <accessToken>
```

## Configuration

### JWT Settings

Configure JWT authentication in `appsettings.json`:

```json
{
  "Jwt": {
    "Secret": "YOUR_SECRET_KEY_HERE",
    "Issuer": "WailletAPI",
    "Audience": "WailletAPI",
    "ExpirationMinutes": 60
  }
}
```

**Important Security Notes:**

1. **Never commit secrets to source control**
2. For development, use `appsettings.Development.json` (already in `.gitignore`)
3. For production, use:
   - Environment variables
   - Azure Key Vault
   - AWS Secrets Manager
   - Other secure secret management solutions

### Example using Environment Variables

```bash
export Jwt__Secret="your-production-secret-key-min-256-bits"
dotnet run
```

## Running the Application

```bash
# Development
dotnet run

# Production
dotnet run --environment Production
```

## Running Tests

```bash
dotnet test
```

## Security Features

- **Password Hashing:** PBKDF2 with SHA256, 100,000 iterations
- **JWT Tokens:** HMAC SHA256 signature algorithm
- **Token Validation:** Validates issuer, audience, signature, and expiration
- **Zero Clock Skew:** Strict expiration checking
- **HTTPS:** Required in production environment

## API Endpoints

### User Management

- `POST /api/user/register` - Register a new user
- `POST /api/user/login` - Authenticate and receive JWT token

### Protected Endpoints

All other endpoints require a valid JWT token in the Authorization header.
