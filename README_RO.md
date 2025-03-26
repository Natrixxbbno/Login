# Aplicație de Autentificare

O aplicație Windows Forms pentru autentificare și înregistrare utilizatorilor, dezvoltată în C#.

## Caracteristici

-   Interfață modernă și prietenoasă
-   Autentificare utilizator
-   Înregistrare utilizator nou
-   Validare completă a datelor de intrare
-   Stocare sigură a parolelor (hashing SHA256)
-   Gestionare cont (ștergere cont)
-   Validări pentru:
    -   Email
    -   Nume de utilizator (3-20 caractere, litere, cifre și underscore)
    -   Parolă (minim 8 caractere, litere mari și mici, cifre și caractere speciale)

## Cerințe de sistem

-   Windows 10 sau mai nou
-   .NET Framework 6.0 sau mai nou
-   PostgreSQL (inclus în proiect)

## Instalare

1. Clonați repository-ul
2. Deschideți soluția în Visual Studio
3. Compilați și rulați proiectul

## Structura proiectului

-   `Form1.cs` - Formularul principal al aplicației
-   `DatabaseManager.cs` - Gestionarea bazei de date
-   `Program.cs` - Punctul de intrare al aplicației

## Securitate

-   Parolele sunt stocate folosind hash SHA256
-   Validare completă a datelor de intrare
-   Protecție împotriva injecțiilor SQL
-   Gestionare sigură a sesiunilor

## Dezvoltare

Proiectul este dezvoltat folosind:

-   C# Windows Forms
-   PostgreSQL pentru stocarea datelor
-   .NET Framework 6.0

## Licență

Acest proiect este licențiat sub licența MIT.
