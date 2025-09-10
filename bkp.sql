create database plataforma_educacao;
use plataforma_educacao;

-- Usuários (pais, professores, especialistas, administradores)
create table usuarios (
  id_usuario int auto_increment primary key,
  nome varchar(100) not null,
  email varchar(100) unique not null,
  senha varchar(255) not null,
  perfil enum('pai','professor','especialista','admin') not null,
  telefone varchar(20),
  data_cadastro datetime default current_timestamp
);

-- Crianças
create table criancas (
  id_crianca int auto_increment primary key,
  nome varchar(100) not null,
  data_nascimento date not null,
  genero enum('M','F','Outro'),
  id_responsavel int,
  foreign key (id_responsavel) references usuarios(id_usuario)
);

-- Atividades pedagógicas
create table atividades (
  id_atividade int auto_increment primary key,
  titulo varchar(150) not null,
  descricao text,
  faixa_etaria varchar(20), -- ex: 4-6 anos
  categoria varchar(50), -- ex: leitura, matemática, jogos
  data_criacao datetime default current_timestamp
);

-- Registro das atividades realizadas pelas crianças
create table respostas_atividades (
  id_resposta int auto_increment primary key,
  id_crianca int not null,
  id_atividade int not null,
  desempenho int, -- nota ou percentual
  data_realizacao datetime default current_timestamp,
  foreign key (id_crianca) references criancas(id_crianca),
  foreign key (id_atividade) references atividades(id_atividade)
);

-- Relatórios de desempenho
create table relatorios (
  id_relatorio int auto_increment primary key,
  id_crianca int not null,
  periodo varchar(20), -- ex: 'Março/2025'
  resumo text,
  progresso_global int, -- percentual
  data_geracao datetime default current_timestamp,
  foreign key (id_crianca) references criancas(id_crianca)
);

-- Recursos de acessibilidade (ex: TEA, TDAH, leitura de tela, etc.)
create table acessibilidade (
  id_acessibilidade int auto_increment primary key,
  id_crianca int not null,
  recurso varchar(100) not null,
  descricao text,
  foreign key (id_crianca) references criancas(id_crianca)
);
