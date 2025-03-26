using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LoginApp;

public partial class RegisterForm : Form
{
    private TextBox usernameTextBox;
    private TextBox passwordTextBox;
    private TextBox confirmPasswordTextBox;
    private TextBox emailTextBox;
    private Color primaryColor = Color.FromArgb(64, 86, 161);
    private Color backgroundColor = Color.FromArgb(240, 242, 245);
    private readonly DatabaseManager _databaseManager;

    public RegisterForm(DatabaseManager databaseManager)
    {
        InitializeComponent();
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.BackColor = backgroundColor;
        _databaseManager = databaseManager;
    }

    private void InitializeComponent()
    {
        // Form settings
        this.Text = "Înregistrare";
        this.Size = new System.Drawing.Size(400, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Title label
        Label titleLabel = new Label();
        titleLabel.Text = "Creați un cont nou";
        titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
        titleLabel.ForeColor = primaryColor;
        titleLabel.Location = new System.Drawing.Point(50, 40);
        titleLabel.Size = new System.Drawing.Size(300, 45);
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(titleLabel);

        // Email label
        Label emailLabel = new Label();
        emailLabel.Text = "Email";
        emailLabel.Font = new Font("Segoe UI", 10);
        emailLabel.ForeColor = Color.DimGray;
        emailLabel.Location = new System.Drawing.Point(50, 120);
        emailLabel.Size = new System.Drawing.Size(300, 20);
        this.Controls.Add(emailLabel);

        // Email textbox
        emailTextBox = new TextBox();
        emailTextBox.Location = new System.Drawing.Point(50, 145);
        emailTextBox.Size = new System.Drawing.Size(300, 25);
        emailTextBox.Font = new Font("Segoe UI", 12);
        emailTextBox.BorderStyle = BorderStyle.FixedSingle;
        this.Controls.Add(emailTextBox);

        // Username label
        Label usernameLabel = new Label();
        usernameLabel.Text = "Nume utilizator";
        usernameLabel.Font = new Font("Segoe UI", 10);
        usernameLabel.ForeColor = Color.DimGray;
        usernameLabel.Location = new System.Drawing.Point(50, 190);
        usernameLabel.Size = new System.Drawing.Size(300, 20);
        this.Controls.Add(usernameLabel);

        // Username textbox
        usernameTextBox = new TextBox();
        usernameTextBox.Location = new System.Drawing.Point(50, 215);
        usernameTextBox.Size = new System.Drawing.Size(300, 25);
        usernameTextBox.Font = new Font("Segoe UI", 12);
        usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
        this.Controls.Add(usernameTextBox);

        // Password label
        Label passwordLabel = new Label();
        passwordLabel.Text = "Parolă";
        passwordLabel.Font = new Font("Segoe UI", 10);
        passwordLabel.ForeColor = Color.DimGray;
        passwordLabel.Location = new System.Drawing.Point(50, 260);
        passwordLabel.Size = new System.Drawing.Size(300, 20);
        this.Controls.Add(passwordLabel);

        // Password textbox
        passwordTextBox = new TextBox();
        passwordTextBox.Location = new System.Drawing.Point(50, 285);
        passwordTextBox.Size = new System.Drawing.Size(300, 25);
        passwordTextBox.Font = new Font("Segoe UI", 12);
        passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
        passwordTextBox.PasswordChar = '●';
        this.Controls.Add(passwordTextBox);

        // Confirm Password label
        Label confirmPasswordLabel = new Label();
        confirmPasswordLabel.Text = "Confirmă parola";
        confirmPasswordLabel.Font = new Font("Segoe UI", 10);
        confirmPasswordLabel.ForeColor = Color.DimGray;
        confirmPasswordLabel.Location = new System.Drawing.Point(50, 330);
        confirmPasswordLabel.Size = new System.Drawing.Size(300, 20);
        this.Controls.Add(confirmPasswordLabel);

        // Confirm Password textbox
        confirmPasswordTextBox = new TextBox();
        confirmPasswordTextBox.Location = new System.Drawing.Point(50, 355);
        confirmPasswordTextBox.Size = new System.Drawing.Size(300, 25);
        confirmPasswordTextBox.Font = new Font("Segoe UI", 12);
        confirmPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
        confirmPasswordTextBox.PasswordChar = '●';
        this.Controls.Add(confirmPasswordTextBox);

        // Register button
        Button registerButton = new Button();
        registerButton.Text = "Înregistrare";
        registerButton.Location = new System.Drawing.Point(50, 400);
        registerButton.Size = new System.Drawing.Size(300, 45);
        registerButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        registerButton.ForeColor = Color.White;
        registerButton.BackColor = primaryColor;
        registerButton.FlatStyle = FlatStyle.Flat;
        registerButton.FlatAppearance.BorderSize = 0;
        registerButton.Cursor = Cursors.Hand;
        registerButton.Click += new EventHandler(RegisterButton_Click);
        this.Controls.Add(registerButton);

        // Back to login link
        LinkLabel backToLoginLink = new LinkLabel();
        backToLoginLink.Text = "Aveți deja cont? Conectați-vă";
        backToLoginLink.Font = new Font("Segoe UI", 10);
        backToLoginLink.LinkColor = primaryColor;
        backToLoginLink.Location = new System.Drawing.Point(50, 460);
        backToLoginLink.Size = new System.Drawing.Size(300, 20);
        backToLoginLink.TextAlign = ContentAlignment.MiddleCenter;
        backToLoginLink.Click += new EventHandler(BackToLoginLink_Click);
        this.Controls.Add(backToLoginLink);
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private async void RegisterButton_Click(object sender, EventArgs e)
    {
        string username = usernameTextBox.Text;
        string password = passwordTextBox.Text;
        string confirmPassword = confirmPasswordTextBox.Text;
        string email = emailTextBox.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || 
            string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(email))
        {
            MessageBox.Show("Vă rugăm să completați toate câmpurile!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (password != confirmPassword)
        {
            MessageBox.Show("Parolele nu se potrivesc!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            string hashedPassword = HashPassword(password);
            bool success = await _databaseManager.RegisterUserAsync(username, hashedPassword, email);

            if (success)
            {
                MessageBox.Show("Cont creat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Numele de utilizator există deja!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la înregistrare: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BackToLoginLink_Click(object sender, EventArgs e)
    {
        this.Close();
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