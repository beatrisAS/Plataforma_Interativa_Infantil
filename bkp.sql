
CREATE DATABASE plataforma_educacao;
USE plataforma_educacao;

CREATE TABLE usuarios (
  id_usuario INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  email VARCHAR(100) UNIQUE NOT NULL,
  senha VARCHAR(255) NOT NULL,
  perfil ENUM('crianca','pai','professor','especialista','admin') NOT NULL,
  telefone VARCHAR(20),
  data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE criancas (
  id_crianca INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  data_nascimento DATE NOT NULL,
  genero ENUM('M','F','Outro'),
  id_responsavel INT,
  FOREIGN KEY (id_responsavel) REFERENCES usuarios(id_usuario)
);

CREATE TABLE atividades (
  id_atividade INT AUTO_INCREMENT PRIMARY KEY,
  titulo VARCHAR(150) NOT NULL,
  descricao TEXT,
  faixa_etaria VARCHAR(20),
  categoria VARCHAR(50),
  data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE questoes (
  id_questao INT AUTO_INCREMENT PRIMARY KEY,
  id_atividade INT NOT NULL,
  pergunta TEXT NOT NULL,
  tipo VARCHAR(50) NOT NULL DEFAULT 'multipla',
  ordem INT DEFAULT 0,
  FOREIGN KEY (id_atividade) REFERENCES atividades(id_atividade) ON DELETE CASCADE
);

CREATE TABLE opcoes_questao (
  id_opcao INT AUTO_INCREMENT PRIMARY KEY,
  id_questao INT NOT NULL,
  texto VARCHAR(500),
  recurso_mid VARCHAR(255),
  ordem INT DEFAULT 0,
  is_correta TINYINT(1) DEFAULT 0,
  FOREIGN KEY (id_questao) REFERENCES questoes(id_questao) ON DELETE CASCADE
);

CREATE TABLE alternativas (
  id_alternativa INT AUTO_INCREMENT PRIMARY KEY,
  texto VARCHAR(255) NOT NULL,
  correta BIT NOT NULL,
  questao_id INT NOT NULL,
  FOREIGN KEY (questao_id) REFERENCES questoes(id_questao) ON DELETE CASCADE
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

CREATE TABLE respostas_questoes (
  id_resposta_questao INT AUTO_INCREMENT PRIMARY KEY,
  id_resposta INT NOT NULL,
  id_questao INT NOT NULL,
  id_opcao INT,
  correta TINYINT(1) DEFAULT 0,
  FOREIGN KEY (id_resposta) REFERENCES respostas_atividades(id_resposta) ON DELETE CASCADE,
  FOREIGN KEY (id_questao) REFERENCES questoes(id_questao) ON DELETE CASCADE,
  FOREIGN KEY (id_opcao) REFERENCES opcoes_questao(id_opcao) ON DELETE CASCADE
);

CREATE TABLE comentarios (
  id_comentario INT AUTO_INCREMENT PRIMARY KEY,
  texto TEXT NOT NULL,
  id_usuario INT NOT NULL,
  id_atividade INT NOT NULL,
  status ENUM('Pending','Approved','Rejected') DEFAULT 'Pending',
  data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
  FOREIGN KEY (id_atividade) REFERENCES atividades(id_atividade)
);

CREATE TABLE relatorios (
  id_relatorio INT AUTO_INCREMENT PRIMARY KEY,
  id_crianca INT NOT NULL,
  periodo VARCHAR(20),
  resumo TEXT,
  progresso_global INT,
  data_geracao DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (id_crianca) REFERENCES criancas(id_crianca)
);

CREATE TABLE acessibilidade (
  id_acessibilidade INT AUTO_INCREMENT PRIMARY KEY,
  id_crianca INT NOT NULL,
  recurso VARCHAR(100) NOT NULL,
  descricao TEXT,
  FOREIGN KEY (id_crianca) REFERENCES criancas(id_crianca)
);

INSERT INTO usuarios (nome, email, senha, perfil, telefone) VALUES
('João Silva','joao@email.com','senha123','pai','(11) 99999-9999'),
('Maria Santos','maria@email.com','prof123','professor','(11) 88888-8888'),
('Pedro Costa','pedro@email.com','kids123','crianca',NULL);

INSERT INTO criancas (nome,data_nascimento,genero,id_responsavel) VALUES
('Ana Silva','2015-03-10','F',1),
('Carlos Santos','2016-07-15','M',1);

INSERT INTO atividades (titulo,descricao,faixa_etaria,categoria) VALUES
('Quebra-cabeça matemático','Atividade de matemática para crianças de 7-8 anos','7-8','matemática'),
('Leitura interativa','Exercício de leitura para crianças de 9-10 anos','9-10','leitura');

INSERT INTO questoes (id_atividade,pergunta) VALUES
(1,'Quanto é 7 + 5?'),
(1,'Resolva: 9 - 4'),
(2,'Quem é o autor do livro "O Pequeno Príncipe"?');

INSERT INTO opcoes_questao (id_questao,texto,is_correta) VALUES
(1,'12',1),
(1,'10',0),
(1,'14',0),
(2,'4',0),
(2,'5',1),
(2,'6',0),
(3,'Antoine de Saint-Exupéry',1),
(3,'Monteiro Lobato',0),
(3,'Machado de Assis',0);

INSERT INTO alternativas (texto,correta,questao_id) VALUES
('12',1,1),
('10',0,1),
('14',0,1),
('5',1,2),
('4',0,2),
('Antoine de Saint-Exupéry',1,3),
('Monteiro Lobato',0,3);

INSERT INTO comentarios (texto,id_usuario,id_atividade,status) VALUES
('Ótima atividade!',1,1,'Aprovado'),
('Minha filha adorou!',1,2,'Aprovado');
