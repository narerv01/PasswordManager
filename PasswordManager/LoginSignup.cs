

namespace PasswordManager
{
    public partial class LoginSignup : Form
    {
        DatabaseManager db;
        LoggedIn loggedInForm;
        public LoginSignup()
        {
            InitializeComponent();
            db = new DatabaseManager();
            loggedInForm = new LoggedIn();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] salt = PasswordHasher.GenerateSalt();
            byte[] password = PasswordHasher.HashPassword(textBox2.Text, salt);
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

            byte[] hashedPassword = PasswordHasher.HashPassword(password, userData.Salt);

            // Compare the hashed password with the stored hashed password
            if (hashedPassword.SequenceEqual(userData.PasswordHash))
            {
                MessageBox.Show("Login successful");
                // Proceed to the main application or another form
                loggedInForm.Show();
                this.Hide(); // Hide the LoginSignup form
            }
            else
            {
                MessageBox.Show("Invalid email or password");
            }
        }
    }
}
