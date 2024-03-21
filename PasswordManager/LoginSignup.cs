

using System.Text.RegularExpressions;

namespace PasswordManager
{
    public partial class LoginSignup : Form
    {
        DatabaseManager db;
        LoggedIn loggedInForm;
        PasswordHasher pw;

        public LoginSignup()
        {
            InitializeComponent();
            db = new DatabaseManager(); 
            pw = new PasswordHasher();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check password strength
            if (!IsPasswordStrong(textBox2.Text))
            {
                MessageBox.Show("Password does not meet strength requirements.");
                return;
            }
            if (!IsEmailValid(textBox1.Text))
            {
                MessageBox.Show("Invalid email address.");
                return;
            }
            byte[] salt = pw.GenerateSalt();
            byte[] password = pw.HashPassword(textBox2.Text, salt);
            db.InsertUser(textBox1.Text, password, salt);
            MessageBox.Show("OK, you can now login");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Login
            string email = textBox1.Text;
            string password = textBox2.Text;

            // Retrieve user data from the database
            UserData userData = db.GetUserByEmail(email);

            if (userData == null)
            {
                MessageBox.Show("Invalid email or password");
                return;
            }

            byte[] hashedPassword = pw.HashPassword(password, userData.Salt);

            // Compare the hashed password with the stored hashed password
            if (hashedPassword.SequenceEqual(userData.PasswordHash))
            {
                MessageBox.Show("Login successful");
                // Proceed to the main application or another form
                loggedInForm = new LoggedIn(userData);
                loggedInForm.Show();
                this.Hide(); // Hide the LoginSignup form
            }
            else
            {
                MessageBox.Show("Invalid email or password");
            }
        }
        private bool IsPasswordStrong(string password)
        {
            // Check password length
            if (password.Length < 8)
                return false;

            // Check for uppercase letter
            if (!password.Any(char.IsUpper))
                return false;

            // Check for lowercase letter
            if (!password.Any(char.IsLower))
                return false;

            // Check for digit
            if (!password.Any(char.IsDigit))
                return false;

            // Check for special character
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }
        private bool IsEmailValid(string email)
        {
            // Use a regular expression to validate email format
            string pattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
