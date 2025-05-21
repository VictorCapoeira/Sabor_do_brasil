document.addEventListener('DOMContentLoaded', function() {
    // Elementos DOM
    const loginBtn = document.getElementById('login-btn');
    const commentBtns = document.querySelectorAll('.btn-comment');
    const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
    const commentsModal = new bootstrap.Modal(document.getElementById('commentsModal'));
    let isLoggedIn = false;
    let comentarioPublicacaoId = null;

    // Função para trocar o botão
    function setLoggedInUI() {
        loginBtn.textContent = 'Sair';
        isLoggedIn = true;
    }
    function setLoggedOutUI() {
        loginBtn.textContent = 'Entrar';
        isLoggedIn = false;
    }

    // Abre modal de login ou realiza logout
    if (loginBtn) {
        loginBtn.addEventListener('click', async function() {
            if (isLoggedIn) {
                // Logout
                const response = await fetch('/Account/Logout', { method: 'POST' });
                if (response.ok) {
                    setLoggedOutUI();
                    window.location.reload();
                }
            } else {
                // Login
                loginModal.show();
            }
        });
    }

    // Abre modal de comentários (para todos os botões de comentário)
    if (commentBtns) {
        commentBtns.forEach(btn => {
            btn.addEventListener('click', function() {
                // Pega os dados do botão
                const titulo = btn.getAttribute('data-titulo');
                const imagem = btn.getAttribute('data-imagem');
                const local = btn.getAttribute('data-local');
                comentarioPublicacaoId = btn.getAttribute('data-id');

                // Preenche o modal
                document.getElementById('modal-post-title').textContent = titulo;
                document.querySelector('#commentsModal img.img-fluid').src = imagem;
                document.getElementById('modal-post-location').innerHTML = `<i class="fas fa-map-marker-alt me-1"></i> ${local}`;

                carregarComentarios(comentarioPublicacaoId);
                commentsModal.show();
            });
        });
    }

    async function carregarComentarios(publicacaoId) {
        const lista = document.getElementById('comments-list');
        lista.innerHTML = '<div class="text-center text-muted">Carregando...</div>';
        const resp = await fetch(`/Comentario/PorPublicacao/${publicacaoId}`);
        const comentarios = await resp.json();
        lista.innerHTML = '';
        if (comentarios.length === 0) {
            lista.innerHTML = '<div class="text-center text-muted">Nenhum comentário ainda.</div>';
        } else {
            comentarios.forEach(c => {
                const div = document.createElement('div');
                div.className = 'd-flex align-items-start mb-3';
                div.innerHTML = `
                    <img src="${c.fotoUsuario || '/img/user.png'}" class="rounded-circle me-2" style="width:40px;height:40px;object-fit:cover;">
                    <div>
                        <strong>${c.usuarioNome}</strong><br>
                        <span>${c.texto}</span>
                        ${c.podeEditar ? `
                            <div class="comentario-acoes-editar">
                                <button class="btn btn-sm btn-link text-primary editar-comentario" data-id="${c.comentarioId}"><i class="fa-solid fa-pen-to-square"></i></button>
                                <button class="btn btn-sm btn-link text-danger excluir-comentario" data-id="${c.comentarioId}"><i class="fa-solid fa-trash"></i></button> 
                            </div>
                        ` : ''}
                    </div>
                `;
                lista.appendChild(div);
            });
            adicionarEventosComentario();
        }
    }

    // Envio do comentário
    const commentText = document.getElementById('commentText');
    const submitComment = document.getElementById('submitComment');
    if (commentText && submitComment) {
        commentText.addEventListener('input', function() {
            submitComment.disabled = !commentText.value.trim();
        });
        submitComment.addEventListener('click', async function() {
            if (!commentText.value.trim()) return;
            const resp = await fetch('/Comentario/Adicionar', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ texto: commentText.value, publicacaoId: comentarioPublicacaoId })
            });
            if (!resp.ok) {
                alert('Erro ao comentar: ' + (await resp.text()));
                return;
            }
            commentText.value = '';
            submitComment.disabled = true;
            carregarComentarios(comentarioPublicacaoId);
        });
    }

    // Editar/Excluir comentário
    function adicionarEventosComentario() {
        document.querySelectorAll('.editar-comentario').forEach(btn => {
            btn.onclick = function() {
                const divComentario = btn.closest('.d-flex.align-items-start.mb-3');
                const spanTexto = divComentario.querySelector('span');
                const textoOriginal = spanTexto.textContent;

                // Evita múltiplas áreas de edição
                if (divComentario.querySelector('.edit-area')) return;

                // Cria textarea e botões
                const textarea = document.createElement('textarea');
                textarea.className = 'form-control edit-area mb-2';
                textarea.value = textoOriginal;

                const btnSalvar = document.createElement('button');
                btnSalvar.textContent = 'Salvar';
                btnSalvar.className = 'btn btn-sm btn-success me-2';

                const btnCancelar = document.createElement('button');
                btnCancelar.textContent = 'Cancelar';
                btnCancelar.className = 'btn btn-sm btn-secondary';

                // Substitui o texto pelo editor
                spanTexto.style.display = 'none';
                spanTexto.parentNode.insertBefore(textarea, spanTexto);
                spanTexto.parentNode.insertBefore(btnSalvar, spanTexto.nextSibling);
                spanTexto.parentNode.insertBefore(btnCancelar, btnSalvar.nextSibling);

                btnSalvar.onclick = async function() {
                    const novoTexto = textarea.value.trim();
                    if (!novoTexto) return;
                    const id = btn.getAttribute('data-id');
                    const resp = await fetch('/Comentario/Editar', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ Id: id, Texto: novoTexto })
                    });
                    if (resp.ok) {
                        carregarComentarios(comentarioPublicacaoId);
                    } else {
                        alert('Erro ao editar: ' + (await resp.text()));
                    }
                };

                btnCancelar.onclick = function() {
                    textarea.remove();
                    btnSalvar.remove();
                    btnCancelar.remove();
                    spanTexto.style.display = '';
                };
            };
        });

        document.querySelectorAll('.excluir-comentario').forEach(btn => {
            btn.onclick = async function() {
                const id = btn.getAttribute('data-id');
                if (confirm('Deseja excluir este comentário?')) {
                    await fetch('/Comentario/Excluir', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ Id: id })
                    });
                    carregarComentarios(comentarioPublicacaoId);
                }
            };
        });
    }

    // Evento de submit do formulário de login
    const loginForm = document.getElementById('loginForm');
    const loginError = document.getElementById('loginError');
    if (loginForm) {
        loginForm.addEventListener('submit', async function (e) {
            e.preventDefault(); // Evita recarregar a página

            const email = document.getElementById('loginEmail').value;
            const senha = document.getElementById('loginPassword').value;
            loginError.style.display = 'none';

            const response = await fetch('/Account/Login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, senha })
            });

            if (response.ok) {
                window.location.reload(); // Recarrega a página após login bem-sucedido
            } else {
                loginError.style.display = 'block';
            }
        });
    }

    // Evento para o botão "Entrar" disparar o submit do formulário
    const submitLoginBtn = document.getElementById('submitLogin');
    if (submitLoginBtn && loginForm) {
        submitLoginBtn.addEventListener('click', function() {
            loginForm.requestSubmit();
        });
    }

    // Inicialização: se já estiver logado, troque o texto
    if (
        loginBtn &&
        document.getElementById('profile-column') &&
        document.getElementById('company-name') &&
        // Checa se o nome exibido é de usuário (não de empresa)
        document.getElementById('profile-column').innerHTML.includes('Foto do usuario')
    ) {
        setLoggedInUI();
    } else {
        setLoggedOutUI();
    }

    // Like/Deslike AJAX
    document.querySelectorAll('.btn-like, .btn-dislike').forEach(btn => {
        btn.addEventListener('click', async function() {
            const isLike = this.classList.contains('btn-like');
            const pubId = this.getAttribute('data-id');
            const url = isLike ? '/Interacao/Like' : '/Interacao/Deslike';

            const response = await fetch(url, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ publicacaoId: pubId })
            });

            if (response.ok) {
                window.location.reload(); // Atualiza para refletir o novo estado
            }
        });
    });
});