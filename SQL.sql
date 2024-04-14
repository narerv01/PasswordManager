-- Users Table
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARBINARY(MAX) NOT NULL,
    Salt VARBINARY(MAX) NOT NULL
);

-- Passwords Table
CREATE TABLE Passwords (
    PasswordID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    URL VARCHAR(255) NOT NULL,
    EncryptedPassword VARBINARY(MAX),
    IV VARBINARY(16), -- IV size for AES is typically 16 bytes
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
 

CREATE PROCEDURE InsertUser
    @Email VARCHAR(255),
    @PasswordHash VARBINARY(MAX),
    @Salt VARBINARY(MAX)
AS
BEGIN
    INSERT INTO Users (Email, PasswordHash, Salt)
    VALUES (@Email, @PasswordHash, @Salt)
END

-- Stored Procedure for Inserting Passwords
CREATE PROCEDURE InsertPassword
    @UserID INT,
    @URL VARCHAR(255),
    @EncryptedPassword VARBINARY(MAX),
    @IV VARBINARY(16)
AS
BEGIN
    INSERT INTO Passwords (UserID, URL, EncryptedPassword, IV)
    VALUES (@UserID, @URL, @EncryptedPassword, @IV)
END


CREATE PROCEDURE GetUserByEmail
    @Email VARCHAR(255)
AS
BEGIN
    SELECT UserID, Email, PasswordHash, Salt
    FROM Users
    WHERE Email = @Email
END


CREATE PROCEDURE GetPasswordsForUser
    @UserID INT
AS
BEGIN
    SELECT URL, EncryptedPassword, IV
    FROM Passwords
    WHERE UserID = @UserID
END
