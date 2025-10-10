DROP DATABASE IF EXISTS plataforma_educacao;
CREATE DATABASE plataforma_educacao;
USE plataforma_educacao;

-- ========================
-- Tabela de Usuários 
-- ========================
CREATE TABLE usuarios (
  id_usuario INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  email VARCHAR(100) UNIQUE NOT NULL,
  senha VARCHAR(255) NOT NULL,
  perfil ENUM('pai','professor','admin', 'crianca') NOT NULL,
  telefone VARCHAR(20),
  data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ========================
-- Tabela de Crianças
-- ========================
CREATE TABLE criancas (
  id_crianca INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  data_nascimento DATE NOT NULL,
  genero ENUM('M','F','Outro'),
  id_responsavel INT,
  estrelas INT DEFAULT 0,
  FOREIGN KEY (id_responsavel) REFERENCES usuarios(id_usuario)
);

-- ========================
-- Atividades 
-- ========================
CREATE TABLE atividades (
  id_atividade INT AUTO_INCREMENT PRIMARY KEY,
  titulo VARCHAR(150) NOT NULL,
  descricao TEXT,
  faixa_etaria VARCHAR(20),
  categoria VARCHAR(50) UNIQUE NOT NULL,
  dificuldade ENUM('fácil','médio','difícil') DEFAULT 'fácil',
  icone VARCHAR(50) DEFAULT '📚'
);


-- ========================
-- Respostas e Progresso
-- ========================
CREATE TABLE respostas_atividades (
  id_resposta INT AUTO_INCREMENT PRIMARY KEY,
  id_crianca INT NOT NULL,
  id_atividade INT NOT NULL, 
  desempenho INT,
  data_realizacao DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (id_crianca) REFERENCES criancas(id_crianca),
  FOREIGN KEY (id_atividade) REFERENCES atividades(id_atividade)
);

-- ========================
-- Dados de Exemplo
-- ========================
INSERT INTO usuarios (nome, email, senha, perfil, telefone) VALUES
('João Silva','joao@email.com','senha123','pai', '(11) 99999-9999'),
('Maria Santos','maria@email.com','prof123','professor', '(11) 88888-8888'),
('Pedro Silva','pedro@email.com','kids123','crianca', NULL);


INSERT INTO criancas (nome, data_nascimento, genero, id_responsavel, estrelas) VALUES
('Pedro Silva','2015-03-10','M', 1, 0);


INSERT INTO atividades (titulo, categoria) VALUES
('Mundo da Matemática', 'Matemática'),
('Viagem Literária', 'Literatura'),
('Explorando a Ciência', 'Ciências'),
('Oficina de Artes', 'Artes'),
('Passaporte de Idiomas', 'Idiomas'),
('Viajando pelo Mapa', 'Geografia'),
('Máquina do Tempo', 'História');
