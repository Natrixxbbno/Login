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
        this.Text = "Авторизация";
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
        titleLabel.Text = "Добро пожаловать!";
        titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
        titleLabel.ForeColor = primaryColor;
        titleLabel.Location = new System.Drawing.Point(50, 40);
        titleLabel.Size = new System.Drawing.Size(300, 45);
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        loginPanel.Controls.Add(titleLabel);

        // Subtitle
        Label subtitleLabel = new Label();
        subtitleLabel.Text = "Пожалуйста, войдите в систему";
        subtitleLabel.Font = new Font("Segoe UI", 12);
        subtitleLabel.ForeColor = Color.Gray;
        subtitleLabel.Location = new System.Drawing.Point(50, 90);
        subtitleLabel.Size = new System.Drawing.Size(300, 25);
        subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
        loginPanel.Controls.Add(subtitleLabel);

        // Username label
        Label usernameLabel = new Label();
        usernameLabel.Text = "Имя пользователя";
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
        passwordLabel.Text = "Пароль";
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
        loginButton.Text = "Войти";
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
        registerLink.Text = "Нет аккаунта? Зарегистрируйтесь";
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
        titleLabel.Text = "Регистрация";
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
        usernameLabel.Text = "Имя пользователя";
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
        passwordLabel.Text = "Пароль";
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
        confirmPasswordLabel.Text = "Подтвердите пароль";
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
        registerButton.Text = "Зарегистрироваться";
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
        backToLoginLink.Text = "Уже есть аккаунт? Войдите";
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
            MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            string hashedPassword = HashPassword(password);
            bool isValid = await _databaseManager.ValidateUserAsync(username, hashedPassword);

            if (isValid)
            {
                currentUsername = username;
                MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowMainForm();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при подключении к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private async void RegisterButton_Click(object sender, EventArgs e)
    {
        string username = registerUsernameTextBox.Text;
        string password = registerPasswordTextBox.Text;
        string confirmPassword = registerConfirmPasswordTextBox.Text;
        string email = registerEmailTextBox.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || 
            string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(email))
        {
            MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (password != confirmPassword)
        {
            MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            string hashedPassword = HashPassword(password);
            bool success = await _databaseManager.RegisterUserAsync(username, hashedPassword, email);

            if (success)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Пользователь с таким именем уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowMainForm()
    {
        // Скрываем форму авторизации
        this.Hide();

        // Создаем и показываем главную форму
        Form mainForm = new Form();
        mainForm.Text = "Главная панель";
        mainForm.Size = new System.Drawing.Size(400, 300);
        mainForm.StartPosition = FormStartPosition.CenterScreen;
        mainForm.BackColor = backgroundColor;

        // Добавляем приветствие
        Label welcomeLabel = new Label();
        welcomeLabel.Text = $"Добро пожаловать, {currentUsername}!";
        welcomeLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        welcomeLabel.ForeColor = primaryColor;
        welcomeLabel.Location = new System.Drawing.Point(50, 40);
        welcomeLabel.Size = new System.Drawing.Size(300, 30);
        welcomeLabel.TextAlign = ContentAlignment.MiddleCenter;
        mainForm.Controls.Add(welcomeLabel);

        // Добавляем кнопку удаления аккаунта
        Button deleteAccountButton = new Button();
        deleteAccountButton.Text = "Удалить аккаунт";
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
        logoutButton.Text = "Выйти";
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
            "Вы уверены, что хотите удалить свой аккаунт? Это действие нельзя отменить!",
            "Подтверждение удаления",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            try
            {
                bool success = await _databaseManager.DeleteUserAsync(currentUsername);
                if (success)
                {
                    MessageBox.Show("Аккаунт успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    currentUsername = null;
                    usernameTextBox.Clear();
                    passwordTextBox.Clear();
                    this.Show();
                    ((Button)sender).FindForm().Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении аккаунта!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении аккаунта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
