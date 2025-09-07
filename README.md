# 📘 Plataforma Interativa Infantil

## 📖 Descrição

A **Plataforma Interativa Infantil** é uma aplicação desenvolvida como parte de um Trabalho de Conclusão de Curso (TCC).
O objetivo é oferecer um ambiente digital lúdico e interativo para crianças, com autenticação de usuários, gerenciamento de atividades e interface amigável.

---

## 🛠️ Tecnologias Utilizadas

### Frontend

* **HTML5**
* **CSS3 com Bootstrap 5**
* **JavaScript (ES6)**

### Backend

* **C# (.NET 8 Web API)**
* **Entity Framework Core**
* **JWT (JSON Web Token)**
* **Swagger**

### Banco de Dados

* **MySQL**

---

## ⚙️ Estrutura do Projeto

```
Plataforma_Interativa_Infantil/
├─ backend/Plataforma.API
│  ├─ Controllers/        → Endpoints da API
│  │   ├─ AuthController.cs
│  │   ├─ UsersController.cs
│  │   └─ ActivitiesController.cs
│  ├─ Models/             → User.cs, Activity.cs
│  ├─ Data/               → AppDbContext.cs
│  ├─ Services/           → JwtService.cs
│  ├─ DTOs/               → Objetos de transferência de dados
│  ├─ Program.cs          → Configuração principal da API
│  └─ appsettings.json    → Configurações de conexão / JWT
│
├─ frontend/
│  ├─ index.html          → Tela Home/Login
│  ├─ dashboard.html      → Tela Usuário
│  ├─ activities.html     → Tela Atividade
│  └─ assets/
│     ├─ css/styles.css   → Estilos
│     └─ js/              → api.js, auth.js, ui.js
│
├─ README.md
└─ .gitignore
```

---

## 🚀 Como Rodar o Projeto

### 🔧 Pré-requisitos

* .NET 8 SDK
* MySQL Server
* VS Code ou Visual Studio
* Extensão **Live Server** (para rodar o frontend)

---

### 🖥️ Configurando o Backend

1. Vá até a pasta do backend:

   ```bash
   cd backend/Plataforma.API
   ```

2. Abra o arquivo `appsettings.json` e configure o MySQL:

   ```json
   "ConnectionStrings": {
     "Default": "Server=localhost;Database=plataforma;User=root;Password=SUASENHA;"
   }
   ```

3. Execute as migrations para criar o banco:

   ```bash
   dotnet tool install --global dotnet-ef --version 8.0.0
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. Inicie a API:

   ```bash
   dotnet watch run
   ```

📌 A API rodará em:

* `https://localhost:5001` (padrão)
* Swagger: `https://localhost:5001/swagger`

---

### 🌐 Configurando o Frontend

1. Vá até a pasta do frontend:

   ```bash
   cd frontend
   ```

2. Abra o arquivo `index.html` no navegador **ou** use Live Server:

   ```
   http://localhost:5500
   ```

3. Certifique-se que o arquivo `frontend/assets/js/api.js` está apontando para a URL da sua API (exemplo: `https://localhost:5001`).

---

## 📑 Funcionalidades

* **Tela Home/Login (index.html):**

  * Login com autenticação JWT
  * Registro de usuários

* **Tela Usuário (dashboard.html):**

  * Exibe dados do usuário logado
  * Edição de informações

* **Tela Atividade (activities.html):**

  * Listar atividades cadastradas
  * Criar novas atividades
  * Editar ou remover atividades

---

## 📚 Endpoints Principais

* `POST /api/auth/register` → Registrar usuário
* `POST /api/auth/login` → Login + token JWT
* `GET /api/users/me` → Dados do usuário logado
* `GET /api/activities` → Listar atividades
* `POST /api/activities` → Criar atividade
* `PUT /api/activities/{id}` → Atualizar atividade
* `DELETE /api/activities/{id}` → Remover atividade

---

## 🔐 Autenticação

* O login retorna um **token JWT**.
* O token é salvo no `localStorage` pelo `auth.js`.
* Cada requisição autenticada deve incluir o header:

  ```
  Authorization: Bearer <token>
  ```

---

## 👨‍💻 Autor

Projeto desenvolvido como parte do **Trabalho de Conclusão de Curso (TCC)**.
