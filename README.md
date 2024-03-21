
**Title: Secure Password Manager System with PBKDF2 and AES**

**Description:**
This Password Manager System is made using PBKDF2 (Password-Based Key Derivation Function 2) algorithm for hashing master passwords of the user and AES (Advanced Encryption Standard) for encrypting and decrypting URL passwords. 
This system offers users a centralized and secure platform to store their passwords, ensuring protection against unauthorized access and data breaches.

**Key Features:**

**User Registration and Authentication: **
Users can register for an account by providing their email address and creating a strong, securely hashed password using the PBKDF2 algorithm. 
The system enforces password strength requirements and securely authenticates users during login using the hashed passwords. 
![LoginSignup](https://github.com/narerv01/PasswordManager/assets/143809276/0feed548-db57-40c3-a98c-9b50abe071b5)
![YouCanLogin](https://github.com/narerv01/PasswordManager/assets/143809276/f8f88eb0-adc7-4fc1-ad03-534972a47be4)
![LoginSuccess](https://github.com/narerv01/PasswordManager/assets/143809276/42c61edf-c905-4c8c-bcbc-60e6bad0cfb2)
 
**Password Storage and Encryption: **
User passwords are securely hashed using the PBKDF2 algorithm with HMAC-SHA-512 for key derivation.
The hashed passwords, along with unique randomly generated salts, are stored in a local SQL database. 
Additionally, the system employs AES encryption with a unique initialization vector (IV) to encrypt URL passwords before storing them in the database.
![MyPasswords](https://github.com/narerv01/PasswordManager/assets/143809276/a58e237f-14d4-4f4d-9a07-b3df68add678)

**Password Generation: **
Users can generate strong, random passwords for new URLs or services using the system's built-in password generation feature. 
The generated passwords adhere to specified strength requirements, including length, uppercase, lowercase, digits, and special characters.

**Password Retrieval and Decryption: **
Authorized users can retrieve and decrypt stored passwords for their URLs. 
The system utilizes the decryption key, derived from the user's hashed password and user's email, to decrypt the AES-encrypted passwords stored in the database. 
This ensures that passwords are decrypted securely and accessed only by authorized users.
![DecryptedPass](https://github.com/narerv01/PasswordManager/assets/143809276/dd4217d8-eb9e-4ee6-af4c-f57fecdc15a8)
![AddNewUrlPass](https://github.com/narerv01/PasswordManager/assets/143809276/2f91e18e-169b-4c6b-8336-60e01f764563)

**System developed using:**
WinForms: C#
SQL Management studio database  






