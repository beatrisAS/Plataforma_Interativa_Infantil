CREATE DATABASE Plataforma;
USE Plataforma;
CREATE TABLE Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    SenhaHash VARCHAR(255) NOT NULL,
    Papel ENUM('pai', 'professor', 'admin', 'especialista') NOT NULL, 
    CriadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    AtualizadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
CREATE TABLE Criancas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    DataNascimento DATE NOT NULL,
    ResponsavelId INT, -- Chave estrangeira para Usuarios (responsável)
    PerfilAprendizagem TEXT, -- Perfil de aprendizado
    CriadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    AtualizadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (ResponsavelId) REFERENCES Usuarios(Id)
);
CREATE TABLE Atividades (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(255) NOT NULL,
    Descricao TEXT,
    Tipo ENUM('jogo', 'exercicio', 'quiz') NOT NULL, -- Tipo da atividade
    CriadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    AtualizadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
CREATE TABLE Progresso (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CriancaId INT,  -- Chave estrangeira para Criancas
    AtividadeId INT, -- Chave estrangeira para Atividades
    DataConclusao TIMESTAMP,
    Pontuacao DECIMAL(5, 2), -- Pontuação, se aplicável
    Observacoes TEXT, -- Observações sobre a atividade
    CriadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CriancaId) REFERENCES Criancas(Id),
    FOREIGN KEY (AtividadeId) REFERENCES Atividades(Id)
);
CREATE TABLE Relatorios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CriancaId INT, -- Chave estrangeira para Criancas
    GeradoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    DadosRelatorio TEXT, -- Relatório com o desempenho e sugestões
    FOREIGN KEY (CriancaId) REFERENCES Criancas(Id)
);
CREATE TABLE Comentarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CriancaId INT, -- Chave estrangeira para Criancas
    EspecialistaId INT, -- Chave estrangeira para Usuarios (especialistas)
    Comentario TEXT,
    CriadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CriancaId) REFERENCES Criancas(Id),
    FOREIGN KEY (EspecialistaId) REFERENCES Usuarios(Id)
);
CREATE TABLE Notificacoes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT, -- Chave estrangeira para Usuarios (responsáveis)
    Mensagem TEXT,
    Lida BOOLEAN DEFAULT FALSE,
    CriadoEm TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);
