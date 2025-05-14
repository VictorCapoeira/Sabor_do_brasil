document.addEventListener('DOMContentLoaded', function() {
    // Elementos DOM
    const loginBtn = document.getElementById('login-btn');
    const commentBtns = document.querySelectorAll('.btn-comment');
    const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
    const commentsModal = new bootstrap.Modal(document.getElementById('commentsModal'));

    // Abre modal de login
    if (loginBtn) {
        loginBtn.addEventListener('click', function() {
            loginModal.show();
        });
    }

    // Abre modal de comentários (para todos os botões de comentário)
    if (commentBtns) {
        commentBtns.forEach(btn => {
            btn.addEventListener('click', function() {
                commentsModal.show();
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
                window.location.reload();
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
});