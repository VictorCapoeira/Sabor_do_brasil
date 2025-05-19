document.addEventListener('DOMContentLoaded', function() {
    // Elementos DOM
    const loginBtn = document.getElementById('login-btn');
    const commentBtns = document.querySelectorAll('.btn-comment');
    const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
    const commentsModal = new bootstrap.Modal(document.getElementById('commentsModal'));
    let isLoggedIn = false;

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

                // Preenche o modal
                document.getElementById('modal-post-title').textContent = titulo;
                document.querySelector('#commentsModal img.img-fluid').src = imagem;
                document.getElementById('modal-post-location').innerHTML = `<i class="fas fa-map-marker-alt me-1"></i> ${local}`;

                commentsModal.show();
                // Aqui você pode carregar os comentários da publicação usando btn.getAttribute('data-id')
            });
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