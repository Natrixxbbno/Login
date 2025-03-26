using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LoginApp;

public partial class Form1 : Form
{
    private TextBox usernameTextBox;
    private TextBox passwordTextBox;
    private TextBox registerUsernameTextBox;
    private TextBox registerPasswordTextBox;
    private TextBox registerConfirmPasswordTextBox;
    private TextBox registerEmailTextBox;
    private Panel loginPanel;
    private Panel registerPanel;
    private Color primaryColor = Color.FromArgb(64, 86, 161);
    private Color backgroundColor = Color.FromArgb(240, 242, 245);
    private readonly DatabaseManager _databaseManager;
    private string currentUsername;

    public Form1()
    {
        InitializeComponent();
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.BackColor = backgroundColor;
        _databaseManager = new DatabaseManager();
        InitializeDatabaseAsync();
    }

    private async void InitializeDatabaseAsync()
    {
        try
        {
            await _databaseManager.InitializeAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void InitializeComponent()
    {
        // Form settings
        this.Text = "Autentificare";
        this.Size = new System.Drawing.Size(400, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Создаем панели для форм авторизации и регистрации
        loginPanel = new Panel();
        loginPanel.Size = new System.Drawing.Size(400, 600);
        loginPanel.Location = new System.Drawing.Point(0, 0);
        this.Controls.Add(loginPanel);

        registerPanel = new Panel();
        registerPanel.Size = new System.Drawing.Size(400, 600);
        registerPanel.Location = new System.Drawing.Point(0, 0);
        registerPanel.Visible = false;
        this.Controls.Add(registerPanel);

        InitializeLoginPanel();
        InitializeRegisterPanel();
    }

    private void InitializeLoginPanel()
    {
        // Title label
        Label titleLabel = new Label();
        titleLabel.Text = "Bine ați venit!";
        titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
        titleLabel.ForeColor = primaryColor;
        titleLabel.Location = new System.Drawing.Point(50, 40);
        titleLabel.Size = new System.Drawing.Size(300, 45);
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        loginPanel.Controls.Add(titleLabel);

        // Subtitle
        Label subtitleLabel = new Label();
        subtitleLabel.Text = "Vă rugăm să vă autentificați";
        subtitleLabel.Font = new Font("Segoe UI", 12);
        subtitleLabel.ForeColor = Color.Gray;
        subtitleLabel.Location = new System.Drawing.Point(50, 90);
        subtitleLabel.Size = new System.Drawing.Size(300, 25);
        subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
        loginPanel.Controls.Add(subtitleLabel);

        // Username label
        Label usernameLabel = new Label();
        usernameLabel.Text = "Nume de utilizator";
        usernameLabel.Font = new Font("Segoe UI", 10);
        usernameLabel.ForeColor = Color.DimGray;
        usernameLabel.Location = new System.Drawing.Point(50, 150);
        usernameLabel.Size = new System.Drawing.Size(300, 20);
        loginPanel.Controls.Add(usernameLabel);

        // Username textbox
        usernameTextBox = new TextBox();
        usernameTextBox.Location = new System.Drawing.Point(50, 175);
        usernameTextBox.Size = new System.Drawing.Size(300, 25);
        usernameTextBox.Font = new Font("Segoe UI", 12);
        usernameTextBox.BorderStyle = BorderStyle.FixedSingle;
        loginPanel.Controls.Add(usernameTextBox);

        // Password label
        Label passwordLabel = new Label();
        passwordLabel.Text = "Parolă";
        passwordLabel.Font = new Font("Segoe UI", 10);
        passwordLabel.ForeColor = Color.DimGray;
        passwordLabel.Location = new System.Drawing.Point(50, 220);
        passwordLabel.Size = new System.Drawing.Size(300, 20);
        loginPanel.Controls.Add(passwordLabel);

        // Password textbox
        passwordTextBox = new TextBox();
        passwordTextBox.Location = new System.Drawing.Point(50, 245);
        passwordTextBox.Size = new System.Drawing.Size(300, 25);
        passwordTextBox.Font = new Font("Segoe UI", 12);
        passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
        passwordTextBox.PasswordChar = '●';
        loginPanel.Controls.Add(passwordTextBox);

        // Login button
        Button loginButton = new Button();
        loginButton.Text = "Autentificare";
        loginButton.Location = new System.Drawing.Point(50, 310);
        loginButton.Size = new System.Drawing.Size(300, 45);
        loginButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        loginButton.ForeColor = Color.White;
        loginButton.BackColor = primaryColor;
        loginButton.FlatStyle = FlatStyle.Flat;
        loginButton.FlatAppearance.BorderSize = 0;
        loginButton.Cursor = Cursors.Hand;
        loginButton.Click += new EventHandler(LoginButton_Click);
        loginPanel.Controls.Add(loginButton);

        // Register link
        LinkLabel registerLink = new LinkLabel();
        registerLink.Text = "Nu aveți cont? Înregistrați-vă";
        registerLink.Font = new Font("Segoe UI", 10);
        registerLink.LinkColor = primaryColor;
        registerLink.Location = new System.Drawing.Point(50, 370);
        registerLink.Size = new System.Drawing.Size(300, 20);
        registerLink.TextAlign = ContentAlignment.MiddleCenter;
        registerLink.Click += new EventHandler(RegisterLink_Click);
        loginPanel.Controls.Add(registerLink);
    }

    private void InitializeRegisterPanel()
    {
        // Title label
        Label titleLabel = new Label();
        titleLabel.Text = "Înregistrare";
        titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
        titleLabel.ForeColor = primaryColor;
        titleLabel.Location = new System.Drawing.Point(50, 40);
        titleLabel.Size = new System.Drawing.Size(300, 45);
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        registerPanel.Controls.Add(titleLabel);

        // Email label
        Label emailLabel = new Label();
        emailLabel.Text = "Email";
        emailLabel.Font = new Font("Segoe UI", 10);
        emailLabel.ForeColor = Color.DimGray;
        emailLabel.Location = new System.Drawing.Point(50, 100);
        emailLabel.Size = new System.Drawing.Size(300, 20);
        registerPanel.Controls.Add(emailLabel);

        // Email textbox
        registerEmailTextBox = new TextBox();
        registerEmailTextBox.Location = new System.Drawing.Point(50, 125);
        registerEmailTextBox.Size = new System.Drawing.Size(300, 25);
        registerEmailTextBox.Font = new Font("Segoe UI", 12);
        registerEmailTextBox.BorderStyle = BorderStyle.FixedSingle;
        registerPanel.Controls.Add(registerEmailTextBox);

        // Username label
        Label usernameLabel = new Label();
        usernameLabel.Text = "Nume de utilizator";
        usernameLabel.Font = new Font("Segoe UI", 10);
        usernameLabel.ForeColor = Color.DimGray;
        usernameLabel.Location = new System.Drawing.Point(50, 170);
        usernameLabel.Size = new System.Drawing.Size(300, 20);
        registerPanel.Controls.Add(usernameLabel);

        // Username textbox
        registerUsernameTextBox = new TextBox();
        registerUsernameTextBox.Location = new System.Drawing.Point(50, 195);
        registerUsernameTextBox.Size = new System.Drawing.Size(300, 25);
        registerUsernameTextBox.Font = new Font("Segoe UI", 12);
        registerUsernameTextBox.BorderStyle = BorderStyle.FixedSingle;
        registerPanel.Controls.Add(registerUsernameTextBox);

        // Password label
        Label passwordLabel = new Label();
        passwordLabel.Text = "Parolă";
        passwordLabel.Font = new Font("Segoe UI", 10);
        passwordLabel.ForeColor = Color.DimGray;
        passwordLabel.Location = new System.Drawing.Point(50, 240);
        passwordLabel.Size = new System.Drawing.Size(300, 20);
        registerPanel.Controls.Add(passwordLabel);

        // Password textbox
        registerPasswordTextBox = new TextBox();
        registerPasswordTextBox.Location = new System.Drawing.Point(50, 265);
        registerPasswordTextBox.Size = new System.Drawing.Size(300, 25);
        registerPasswordTextBox.Font = new Font("Segoe UI", 12);
        registerPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
        registerPasswordTextBox.PasswordChar = '●';
        registerPanel.Controls.Add(registerPasswordTextBox);

        // Confirm Password label
        Label confirmPasswordLabel = new Label();
        confirmPasswordLabel.Text = "Confirmați parola";
        confirmPasswordLabel.Font = new Font("Segoe UI", 10);
        confirmPasswordLabel.ForeColor = Color.DimGray;
        confirmPasswordLabel.Location = new System.Drawing.Point(50, 310);
        confirmPasswordLabel.Size = new System.Drawing.Size(300, 20);
        registerPanel.Controls.Add(confirmPasswordLabel);

        // Confirm Password textbox
        registerConfirmPasswordTextBox = new TextBox();
        registerConfirmPasswordTextBox.Location = new System.Drawing.Point(50, 335);
        registerConfirmPasswordTextBox.Size = new System.Drawing.Size(300, 25);
        registerConfirmPasswordTextBox.Font = new Font("Segoe UI", 12);
        registerConfirmPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
        registerConfirmPasswordTextBox.PasswordChar = '●';
        registerPanel.Controls.Add(registerConfirmPasswordTextBox);

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
        registerPanel.Controls.Add(registerButton);

        // Back to login link
        LinkLabel backToLoginLink = new LinkLabel();
        backToLoginLink.Text = "Aveți deja cont? Autentificați-vă";
        backToLoginLink.Font = new Font("Segoe UI", 10);
        backToLoginLink.LinkColor = primaryColor;
        backToLoginLink.Location = new System.Drawing.Point(50, 460);
        backToLoginLink.Size = new System.Drawing.Size(300, 20);
        backToLoginLink.TextAlign = ContentAlignment.MiddleCenter;
        backToLoginLink.Click += new EventHandler(BackToLoginLink_Click);
        registerPanel.Controls.Add(backToLoginLink);
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

    private async void LoginButton_Click(object sender, EventArgs e)
    {
        string username = usernameTextBox.Text;
        string password = passwordTextBox.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Vă rugăm să completați toate câmpurile!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            string hashedPassword = HashPassword(password);
            bool isValid = await _databaseManager.ValidateUserAsync(username, hashedPassword);

            if (isValid)
            {
                currentUsername = username;
                MessageBox.Show("Autentificare reușită!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowMainForm();
            }
            else
            {
                MessageBox.Show("Nume de utilizator sau parolă incorectă!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la conectarea la baza de date: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RegisterLink_Click(object sender, EventArgs e)
    {
        loginPanel.Visible = false;
        registerPanel.Visible = true;
    }

    private void BackToLoginLink_Click(object sender, EventArgs e)
    {
        registerPanel.Visible = false;
        loginPanel.Visible = true;
    }

    private bool ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool ValidateUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
            return false;

        // Проверяем длину (от 3 до 20 символов)
        if (username.Length < 3 || username.Length > 20)
            return false;

        // Проверяем допустимые символы (только буквы, цифры и подчеркивание)
        return System.Text.RegularExpressions.Regex.IsMatch(username, "^[a-zA-Z0-9_]+$");
    }

    private bool ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        // Минимальная длина 8 символов
        if (password.Length < 8)
            return false;

        // Проверяем наличие хотя бы одной цифры
        bool hasNumber = System.Text.RegularExpressions.Regex.IsMatch(password, "[0-9]+");
        // Проверяем наличие хотя бы одной строчной буквы
        bool hasLowerChar = System.Text.RegularExpressions.Regex.IsMatch(password, "[a-z]+");
        // Проверяем наличие хотя бы одной заглавной буквы
        bool hasUpperChar = System.Text.RegularExpressions.Regex.IsMatch(password, "[A-Z]+");
        // Проверяем наличие хотя бы одного специального символа
        bool hasSpecialChar = System.Text.RegularExpressions.Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]+");

        return hasNumber && hasLowerChar && hasUpperChar && hasSpecialChar;
    }

    private async void RegisterButton_Click(object sender, EventArgs e)
    {
        string username = registerUsernameTextBox.Text;
        string password = registerPasswordTextBox.Text;
        string confirmPassword = registerConfirmPasswordTextBox.Text;
        string email = registerEmailTextBox.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || 
            string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(email))
        {
            MessageBox.Show("Vă rugăm să completați toate câmpurile!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!ValidateEmail(email))
        {
            MessageBox.Show("Vă rugăm să introduceți o adresă de email validă!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!ValidateUsername(username))
        {
            MessageBox.Show("Numele de utilizator trebuie să conțină între 3 și 20 de caractere și poate conține doar litere, cifre și underscore!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!ValidatePassword(password))
        {
            MessageBox.Show("Parola trebuie să conțină minim 8 caractere, inclusiv litere mari și mici, cifre și caractere speciale!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                // Очищаем поля
                registerUsernameTextBox.Clear();
                registerPasswordTextBox.Clear();
                registerConfirmPasswordTextBox.Clear();
                registerEmailTextBox.Clear();
                // Возвращаемся на форму входа
                registerPanel.Visible = false;
                loginPanel.Visible = true;
            }
            else
            {
                MessageBox.Show("Utilizatorul cu acest nume există deja!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la înregistrare: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowMainForm()
    {
        // Скрываем форму авторизации
        this.Hide();

        // Создаем и показываем главную форму
        Form mainForm = new Form();
        mainForm.Text = "Panou principal";
        mainForm.Size = new System.Drawing.Size(400, 300);
        mainForm.StartPosition = FormStartPosition.CenterScreen;
        mainForm.BackColor = backgroundColor;

        // Добавляем приветствие
        Label welcomeLabel = new Label();
        welcomeLabel.Text = $"Bine ați venit, {currentUsername}!";
        welcomeLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        welcomeLabel.ForeColor = primaryColor;
        welcomeLabel.Location = new System.Drawing.Point(50, 40);
        welcomeLabel.Size = new System.Drawing.Size(300, 30);
        welcomeLabel.TextAlign = ContentAlignment.MiddleCenter;
        mainForm.Controls.Add(welcomeLabel);

        // Добавляем кнопку удаления аккаунта
        Button deleteAccountButton = new Button();
        deleteAccountButton.Text = "Șterge contul";
        deleteAccountButton.Location = new System.Drawing.Point(50, 100);
        deleteAccountButton.Size = new System.Drawing.Size(300, 45);
        deleteAccountButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        deleteAccountButton.ForeColor = Color.White;
        deleteAccountButton.BackColor = Color.FromArgb(220, 53, 69); // Красный для удаления
        deleteAccountButton.FlatStyle = FlatStyle.Flat;
        deleteAccountButton.FlatAppearance.BorderSize = 0;
        deleteAccountButton.Cursor = Cursors.Hand;
        deleteAccountButton.Click += new EventHandler(DeleteAccountButton_Click);
        mainForm.Controls.Add(deleteAccountButton);

        // Добавляем кнопку выхода
        Button logoutButton = new Button();
        logoutButton.Text = "Deconectare";
        logoutButton.Location = new System.Drawing.Point(50, 160);
        logoutButton.Size = new System.Drawing.Size(300, 45);
        logoutButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        logoutButton.ForeColor = Color.White;
        logoutButton.BackColor = primaryColor;
        logoutButton.FlatStyle = FlatStyle.Flat;
        logoutButton.FlatAppearance.BorderSize = 0;
        logoutButton.Cursor = Cursors.Hand;
        logoutButton.Click += new EventHandler(LogoutButton_Click);
        mainForm.Controls.Add(logoutButton);

        // При закрытии формы показываем форму авторизации
        mainForm.FormClosed += (s, args) => 
        {
            if (currentUsername != null) // Если пользователь не удален и не вышел
            {
                this.Show();
            }
        };
        
        mainForm.Show();
    }

    private async void DeleteAccountButton_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Sunteți sigur că doriți să vă ștergeți contul? Această acțiune nu poate fi anulată!",
            "Confirmare ștergere",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            try
            {
                bool success = await _databaseManager.DeleteUserAsync(currentUsername);
                if (success)
                {
                    MessageBox.Show("Cont șters cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    currentUsername = null;
                    usernameTextBox.Clear();
                    passwordTextBox.Clear();
                    this.Show();
                    ((Button)sender).FindForm().Close();
                }
                else
                {
                    MessageBox.Show("Eroare la ștergerea contului!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea contului: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void LogoutButton_Click(object sender, EventArgs e)
    {
        currentUsername = null;
        usernameTextBox.Clear();
        passwordTextBox.Clear();
        this.Show();
        ((Button)sender).FindForm().Close();
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
