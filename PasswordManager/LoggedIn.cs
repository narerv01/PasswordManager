using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordManager
{
    public partial class LoggedIn : Form
    {
        DatabaseManager db;
        PasswordHasher pw;
        private UserData userData;

        public LoggedIn(UserData userData)
        {
            InitializeComponent();

            db = new DatabaseManager();
            pw = new PasswordHasher();

            this.userData = userData;
            label2.Text = "Logged in as: " + userData.Email;
        }

         
        private void button1_Click(object sender, EventArgs e)
        {
            pw.GeneratePasswordForUrl(userData, textBox1.Text);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            List<PasswordData> storedPwUrls = db.GetPasswordsForUser(userData.UserID);
            int yOffset = (tabPage2.Height - (storedPwUrls.Count * 50)) / 2; // Calculate vertical offset

            foreach (PasswordData storedPw in storedPwUrls)
            {
                Label label = new Label();
                label.Text = storedPw.URL;

                // Calculate the position to center the label horizontally
                int xOffset = (tabPage2.Width - label.PreferredWidth - 100) / 2;

                // Set the location of the label
                label.Location = new Point(xOffset, yOffset);

                // Create a button
                Button button = new Button();
                button.Text = "Show Password";

                // Calculate the position for the button
                int buttonXOffset = xOffset + label.PreferredWidth + 10;
                int buttonYOffset = yOffset;

                // Set the location of the button
                button.Location = new Point(buttonXOffset, buttonYOffset);
                button.Tag = storedPw; // Store PasswordData in the Tag property of the button

                // Attach event handler for button click
                button.Click += Button_Click;

                // Add the label and button to the tabPage2's Controls collection
                tabPage2.Controls.Add(label);
                tabPage2.Controls.Add(button);

                // Adjust vertical position for the next label and button
                yOffset += 50;
            }

            // Force tabPage2 to repaint itself
            tabPage2.Invalidate();
        }




        private void Button_Click(object sender, EventArgs e)
        {
            // Get the PasswordData stored in the Tag property of the button
            PasswordData storedPw = (PasswordData)((Button)sender).Tag;

            // Decrypt the password using PasswordHasher
            string decryptedPassword = pw.DecryptPassword(storedPw.EncryptedPassword, userData.PasswordHash, storedPw.IV, userData.Email);

            // Show decrypted password in a MessageBox
            MessageBox.Show("Decrypted Password: " + decryptedPassword, "Password");
        }
    }
}
