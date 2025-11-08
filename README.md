# ✨ CogniVerse (Plataforma Interativa Infantil)

 # 📌 Sobre o Projeto

**CogniVerse** é uma plataforma de aprendizado gamificada, desenvolvida em **C\# com .NET**, voltada para crianças de 7 a 12 anos. O objetivo é integrar conteúdos interativos complementares à educação, em um ambiente simples, seguro e atrativo.

Este projeto é parte do trabalho de conclusão de curso (TCC) dos autores, com foco em tecnologias modernas e **acessibilidade universal**.

## ⭐ Destaque Principal: Acessibilidade Universal
O CogniVerse foi construído com um módulo de acessibilidade completo e manual, garantindo que crianças com diferentes necessidades possam utilizar a plataforma. O widget, acessível pelo atalho Alt + A ou pelo ícone na barra de navegação.

## 🚀 Tecnologias Utilizadas

  * **C\# (.NET 9)** → Linguagem principal do backend.
  * **ASP.NET Core MVC** → Para construção do sistema web (páginas Razor).
  * **Entity Framework Core** → Para o gerenciamento da base de dados (ORM).
  * **SQL Server / MySQL** → Como banco de dados.
  * **ASP.NET Core Identity** → Para autenticação segura (Login/Cadastro).
  * **Bootstrap 5** → Para estilização da interface e responsividade.
  * **Bootstrap Icons** → Para a iconografia do sistema.
  * **JavaScript (Vanilla)** → Para o widget de acessibilidade e interatividade.
    
-----

## 📂 Estrutura do Projeto

O projeto segue uma estrutura padrão do **.NET MVC**, onde a lógica, os dados e as visualizações são mantidos em uma única aplicação web.

```
Plataforma_Interativa_Infantil/
│-- Plataforma_Interativa_Infantil.sln    # Solution principal
│-- Plataforma_Interativa_Infantil/       # Projeto Web MVC
│   ├── wwwroot/                          # Arquivos estáticos (CSS, JS, imagens)
│   ├── Views/                            # Páginas Razor (.cshtml)
│   │   ├── Home/
│   │   ├── Pai/
│   │   ├── Professor/
│   │   └── Crianca/
│   ├── Controllers/                      # Controladores (C#)
│   ├── Models/                           # Modelos de dados e ViewModels
│   ├── Data/                             # Contexto do Entity Framework
│   ├── Services/                         # Regras de negócio
│   └── appsettings.json
│-- .gitignore
│-- README.md
```

-----

## 🔑 Perfis de Usuário

O sistema possui três perfis de usuário distintos:

1.  **Criança (Aluno):** Acessa as atividades, jogos e vê seu progresso.
2.  **Pai (Responsável):** Pode vincular a conta do filho(a) para acompanhar o progresso.
3.  **Professor:** Pode gerenciar alunos, turmas e acompanhar o desempenho.

-----

## ⚙️ Como Executar o Projeto

### 🔹 Pré-requisitos

  * [.NET 9 SDK](https://dotnet.microsoft.com/) instalado
  * Banco de dados configurado (SQL Server ou MySQL)
  * Git instalado para versionamento

### 🔹 Passos para rodar localmente

1.  Clone o repositório:

    ```bash
    git clone https://github.com/beatrisAS/Plataforma_Interativa_Infantil.git
    ```

2.  Acesse a pasta do projeto:

    ```bash
    cd Plataforma_Interativa_Infantil
    ```

3.  Restaure os pacotes NuGet:

    ```bash
    dotnet restore
    ```

4.  Configure a **string de conexão** no `appsettings.json`:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=PlataformaInfantil;User Id=sa;Password=suasenha;"
    }
    ```

5.  Rode as migrations para criar o banco:

    ```bash
    dotnet ef database update
    ```

6.  Execute a aplicação:

    ```bash
    dotnet run
    ```

A aplicação estará disponível em:
👉 `https://localhost:XXXX`

-----

## 👩‍💻 Autores

Desenvolvido por [@BeatrisAS](https://github.com/beatrisAS), [@Albuquerque-bru](https://github.com/Albuquerque-bru), [@Camila290412](https://github.com/Camila290412), [@FranciscoURL](https://github.com/FranciscoURL) e [@mathburiti-collab](https://github.com/mathburiti-collab).
