using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace LoginApp;

public partial class Form1 : Form
{
    private TextBox usernameTextBox;
    private TextBox passwordTextBox;
    private Color primaryColor = Color.FromArgb(64, 86, 161);
    private Color backgroundColor = Color.FromArgb(240, 242, 245);
    private string connectionString = "Server=localhost;Database=LoginDB;Trusted_Connection=True;";

    public Form1()
    {
        InitializeComponent();
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.BackColor = backgroundColor;
    }

    private void InitializeComponent()
    {
        // Form settings
        this.Text = "Autentificare";
        this.Size = new System.Drawing.Size(400, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Title label
        Label titleLabel = new Label();
        titleLabel.Text = "Bine ați venit!";
        titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
        titleLabel.ForeColor = primaryColor;
        titleLabel.Location = new System.Drawing.Point(50, 40);
        titleLabel.Size = new System.Drawing.Size(300, 45);
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(titleLabel);

        // Subtitle
        Label subtitleLabel = new Label();
        subtitleLabel.Text = "Vă rugăm să vă autentificați";
        subtitleLabel.Font = new Font("Segoe UI", 12);
        subtitleLabel.ForeColor = Color.Gray;
        subtitleLabel.Location = new System.Drawing.Point(50, 90);
        subtitleLabel.Size = new System.Drawing.Size(300, 25);
        subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(subtitleLabel);

        // Username label
        Label usernameLabel = new Label();
        usernameLabel.Text = "Nume utilizator";
        usernameLabel.Font = new Font("Segoe UI", 10);
        usernameLabel.ForeColor = Color.DimGray;
        usernameLabel.Location = new System.Drawing.Point(50, 150);
        usernameLabel.Size = new System.Drawing.Size(300, 20);
        this.Controls.Add(usernameLabel);

        // Username textbox
        usernameTextBox = new TextBox();
        usernameTextBox.Location = new System.Drawing.Point(50, 175);
        usernameTextBox.Size = new System.Drawing.Size(300, 25);
        usernameTextBox.Font = new Font("Segoe UI", 12);
        usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
        this.Controls.Add(usernameTextBox);

        // Password label
        Label passwordLabel = new Label();
        passwordLabel.Text = "Parolă";
        passwordLabel.Font = new Font("Segoe UI", 10);
        passwordLabel.ForeColor = Color.DimGray;
        passwordLabel.Location = new System.Drawing.Point(50, 220);
        passwordLabel.Size = new System.Drawing.Size(300, 20);
        this.Controls.Add(passwordLabel);

        // Password textbox
        passwordTextBox = new TextBox();
        passwordTextBox.Location = new System.Drawing.Point(50, 245);
        passwordTextBox.Size = new System.Drawing.Size(300, 25);
        passwordTextBox.Font = new Font("Segoe UI", 12);
        passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
        passwordTextBox.PasswordChar = '●';
        this.Controls.Add(passwordTextBox);

        // Login button
        Button loginButton = new Button();
        loginButton.Text = "Conectare";
        loginButton.Location = new System.Drawing.Point(50, 310);
        loginButton.Size = new System.Drawing.Size(300, 45);
        loginButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        loginButton.ForeColor = Color.White;
        loginButton.BackColor = primaryColor;
        loginButton.FlatStyle = FlatStyle.Flat;
        loginButton.FlatAppearance.BorderSize = 0;
        loginButton.Cursor = Cursors.Hand;
        loginButton.Click += new EventHandler(LoginButton_Click);
        this.Controls.Add(loginButton);

        // Register link
        LinkLabel registerLink = new LinkLabel();
        registerLink.Text = "Nu aveți cont? Înregistrați-vă";
        registerLink.Font = new Font("Segoe UI", 10);
        registerLink.LinkColor = primaryColor;
        registerLink.Location = new System.Drawing.Point(50, 370);
        registerLink.Size = new System.Drawing.Size(300, 20);
        registerLink.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(registerLink);
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private bool ValidateUser(string username, string password)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string hashedPassword = HashPassword(password);
                
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la conectarea la baza de date: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    private void LoginButton_Click(object sender, EventArgs e)
    {
        string username = usernameTextBox.Text;
        string password = passwordTextBox.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Vă rugăm să completați toate câmpurile!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (ValidateUser(username, password))
        {
            MessageBox.Show("Autentificare reușită!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Aici puteți adăuga codul pentru deschiderea formularului principal
        }
        else
        {
            MessageBox.Show("Nume de utilizator sau parolă incorectă!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using (GraphicsPath path = new GraphicsPath())
        {
            int radius = 10;
            Rectangle bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            path.AddArc(bounds.X, bounds.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(bounds.Right - radius * 2, bounds.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(bounds.Right - radius * 2, bounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseAllFigures();

            this.Region = new Region(path);
        }
    }
}
