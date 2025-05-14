INSERT INTO usuarios (nome, email, apelido, senha, foto) VALUES
('João Silva', 'joao@gmail.com', 'joaos', 'senha123', 'img\usuario\joao.jpeg'),
('Maria Oliveira', 'maria@gmail.com', 'mariinha', 'senha123', 'img\usuario\maria.jpeg'),
('Carlos Souza', 'carlos@gmail.com', 'carlito', 'senha123', 'img\usuario\carlos.jpeg'),
('Ana Paula', 'ana@gmail.com', 'aninha', 'senha123', 'img\usuario\ana.jpeg'),
('Fernanda Lima', 'fernanda@gmail.com', 'ferlima', 'senha123', 'wwwroot\img\usuario\fernanda.jpeg');
INSERT INTO empresas (nome, logo) VALUES
('Delícias de Minas', 'img/empresa/logo.jpeg');
INSERT INTO empresas_endereco (estado, cidade, bairro, rua, numero, empresa_id) VALUES
('MG', 'Belo Horizonte', 'Centro', 'Rua das Flores', '100', 1);
INSERT INTO publicacoes (titulo, imagem, descricao, local, empresa_id, usuario_id) VALUES
('Feijoada da Vovó', 'img\publicacao\feijoada.jpeg', 'Uma feijoada mineira completa!', 'Restaurante Central', 1, 1),
('Pão de Queijo Gigante', 'img\publicacao\pao.jpeg', 'Nosso pão de queijo é uma refeição!', 'Padaria da Serra', 1, 2),
('Doce de Leite Artesanal', 'img\publicacao\doceLeite.jpeg', 'Direto de Araxá para você!', 'Doces da Vovó', 1, 3),
('Café com Bolo', 'img\publicacao\cafe.jpeg', 'A combinação perfeita da tarde!', 'Café Mineiro', 1, 4),
('Almoço Caipira', 'img\publicacao\almoco.jpeg', 'Arroz, feijão, carne e muito sabor!', 'Sabor do Interior', 1, 5);
INSERT INTO interacoes (tipo, publicacao_id, usuario_id) VALUES
('like', 1, 2),
('like', 2, 3),
('deslike', 3, 4),
('like', 4, 5),
('like', 5, 1);
INSERT INTO comentarios (texto, date, usuario_id, publicacao_id, foto) VALUES
('Amo essa feijoada!', CURDATE(), 2, 1, 'img\comentarios\comentario01.jpeg'),
('Esse pão de queijo é tudo!', CURDATE(), 3, 2, 'img\comentarios\comentario02.jpeg'),
('Já quero provar esse doce!', CURDATE(), 4, 3, 'img\comentarios\comentario03.jpeg'),
('Café + bolo = amor', CURDATE(), 5, 4, 'img\comentarios\comentario04.jpeg'),
('Melhor almoço da região!', CURDATE(), 1, 5, 'img\comentarios\comentario05.jpeg');
select * from publicacoes;
