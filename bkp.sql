DROP DATABASE IF EXISTS plataforma_educacao;
CREATE DATABASE plataforma_educacao;
USE plataforma_educacao;


CREATE TABLE usuarios (
  id_usuario INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  email VARCHAR(100) UNIQUE NOT NULL,
  senha VARCHAR(255) NOT NULL,
  perfil ENUM('pai','professor','admin', 'crianca') NOT NULL,
  telefone VARCHAR(20),
  data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE criancas (
  id_crianca INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  data_nascimento DATE NOT NULL,
  genero ENUM('M','F','Outro'),
  id_responsavel INT,
  estrelas INT DEFAULT 0,
  id_usuario INT NOT NULL UNIQUE,
  codigo_vinculo VARCHAR(10) NOT NULL UNIQUE,
  FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
  FOREIGN KEY (id_responsavel) REFERENCES usuarios(id_usuario)
);

CREATE TABLE atividades (
  id_atividade INT AUTO_INCREMENT PRIMARY KEY,
  titulo VARCHAR(150) NOT NULL,
  descricao TEXT,
  faixa_etaria VARCHAR(20),
  categoria VARCHAR(50) NOT NULL,
  icone VARCHAR(50) DEFAULT '📚',
  id_professor INT,
  FOREIGN KEY (id_professor) REFERENCES usuarios(id_usuario)
);

CREATE TABLE questoes (
  id_questao INT AUTO_INCREMENT PRIMARY KEY,
  atividade_id INT NOT NULL,
  pergunta TEXT NOT NULL,
  
  explicacao TEXT, 
  FOREIGN KEY (atividade_id) REFERENCES atividades(id_atividade)
);

CREATE TABLE alternativas (
  id_alternativa INT AUTO_INCREMENT PRIMARY KEY,
  questao_id INT NOT NULL,
  texto VARCHAR(255) NOT NULL,
  correta BOOLEAN NOT NULL DEFAULT FALSE,
  FOREIGN KEY (questao_id) REFERENCES questoes(id_questao)
);


CREATE TABLE respostas_atividades (
  id_resposta INT AUTO_INCREMENT PRIMARY KEY,
  id_crianca INT NOT NULL,
  id_atividade INT NOT NULL,
  desempenho INT,
  data_realizacao DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (id_crianca) REFERENCES criancas(id_crianca),
  FOREIGN KEY (id_atividade) REFERENCES atividades(id_atividade)
);

CREATE TABLE professor_alunos (
  id_professor INT NOT NULL,
  id_crianca INT NOT NULL,
  PRIMARY KEY (id_professor, id_crianca),
  FOREIGN KEY (id_professor) REFERENCES usuarios(id_usuario),
  FOREIGN KEY (id_crianca) REFERENCES criancas(id_crianca)
);


INSERT INTO atividades (titulo, categoria, id_professor) VALUES
('Mundo da Matemática', 'Matemática', NULL),
('Viagem Literária', 'Literatura', NULL),
('Explorando a Ciência', 'Ciências', NULL),
('Oficina de Artes', 'Artes', NULL), 
('Passaporte de Idiomas', 'Idiomas', NULL), 
('Viajando pelo Mapa', 'Geografia', NULL),
('Máquina do Tempo', 'História', NULL);