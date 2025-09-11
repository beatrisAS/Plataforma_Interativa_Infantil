-- Criar o banco de dados
CREATE DATABASE IF NOT EXISTS plataforma_educacao;
USE plataforma_educacao;

-- Tabela de Usuários (pais, professores, especialistas, administradores, crianças)
CREATE TABLE usuarios (
  id_usuario INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  email VARCHAR(100) UNIQUE NOT NULL,
  senha VARCHAR(255) NOT NULL,
  perfil ENUM('crianca', 'pai', 'professor', 'especialista', 'admin') NOT NULL,
  telefone VARCHAR(20),
  data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de Crianças
CREATE TABLE criancas (
  id_crianca INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  data_nascimento DATE NOT NULL,
  genero ENUM('M', 'F', 'Outro'),
  id_responsavel INT,
  FOREIGN KEY (id_responsavel) REFERENCES usuarios(id_usuario)
);

-- Tabela de Atividades pedagógicas
CREATE TABLE atividades (
  id_atividade INT AUTO_INCREMENT PRIMARY KEY,
  titulo VARCHAR(150) NOT NULL,
  descricao TEXT,
  faixa_etaria VARCHAR(20),
  categoria VARCHAR(50),
  data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de Comentários
CREATE TABLE comentarios (
  id_comentario INT AUTO_INCREMENT PRIMARY KEY,
  texto TEXT NOT NULL,
  id_usuario INT NOT NULL,
  id_atividade INT NOT NULL,
  status ENUM('Pending', 'Approved', 'Rejected') DEFAULT 'Pending',
  data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
  FOREIGN KEY (id_atividade) REFERENCES atividades(id_atividade)
);

-- Tabela de Registro das atividades realizadas pelas crianças
CREATE TABLE respostas_atividades (
  id_resposta INT AUTO_INCREMENT PRIMARY KEY,
  id_crianca INT NOT NULL,
  id_atividade INT NOT NULL,
  desempenho INT,
  data_realizacao DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (id_crianca) REFERENCES criancas(id_crianca),
  FOREIGN KEY (id_atividade) REFERENCES atividades(id_atividade)
);

-- Tabela de Relatórios de desempenho
CREATE TABLE relatorios (
  id_relatorio INT AUTO_INCREMENT PRIMARY KEY,
  id_crianca INT NOT NULL,
  periodo VARCHAR(20),
  resumo TEXT,
  progresso_global INT,
  data_geracao DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (id_crianca) REFERENCES criancas(id_crianca)
);

-- Tabela de Recursos de acessibilidade
CREATE TABLE acessibilidade (
  id_acessibilidade INT AUTO_INCREMENT PRIMARY KEY,
  id_crianca INT NOT NULL,
  recurso VARCHAR(100) NOT NULL,
  descricao TEXT,
  FOREIGN KEY (id_crianca) REFERENCES criancas(id_crianca)
);

-- Inserir dados de exemplo com senhas em texto puro
INSERT INTO usuarios (nome, email, senha, perfil, telefone) VALUES
('João Silva', 'joao@email.com', 'senha123', 'pai', '(11) 99999-9999'),
('Maria Santos', 'maria@email.com', 'prof123', 'professor', '(11) 88888-8888'),
('Pedro Costa', 'pedro@email.com', 'kids123', 'crianca', NULL);

INSERT INTO criancas (nome, data_nascimento, genero, id_responsavel) VALUES
('Ana Silva', '2015-03-10', 'F', 1),
('Carlos Santos', '2016-07-15', 'M', 1);

INSERT INTO atividades (titulo, descricao, faixa_etaria, categoria) VALUES
('Quebra-cabeça matemático', 'Atividade de matemática para crianças de 7-8 anos', '7-8', 'matemática'),
('Leitura interativa', 'Exercício de leitura para crianças de 9-10 anos', '9-10', 'leitura');

INSERT INTO comentarios (texto, id_usuario, id_atividade, status) VALUES
('Ótima atividade!', 1, 1, 'Approved'),
('Minha filha adorou!', 1, 2, 'Approved');
