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
});