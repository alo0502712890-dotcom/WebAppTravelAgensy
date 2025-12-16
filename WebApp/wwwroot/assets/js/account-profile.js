document.addEventListener('DOMContentLoaded', () => {

    // ===== Password toggle block =====
    const toggleBtn = document.getElementById('togglePasswordBtn');
    const cancelBtn = document.getElementById('cancelPasswordBtn');
    const passwordBlock = document.getElementById('passwordBlock');

    if (toggleBtn && passwordBlock) {
        toggleBtn.addEventListener('click', () => {
            passwordBlock.classList.remove('hidden');
            toggleBtn.classList.add('hidden');

            const firstInput = passwordBlock.querySelector('input');
            if (firstInput) firstInput.focus();
        });
    }

    if (cancelBtn && passwordBlock && toggleBtn) {
        cancelBtn.addEventListener('click', () => {
            passwordBlock.classList.add('hidden');
            toggleBtn.classList.remove('hidden');

            passwordBlock.querySelectorAll('input')
                .forEach(i => i.value = '');
        });
    }

    // ===== Toast auto close =====
    const autoCloseToast = (id, timeout) => {
        const toast = document.getElementById(id);
        if (!toast) return;

        setTimeout(() => {
            toast.classList.add('opacity-0');
            setTimeout(() => toast.remove(), 500);
        }, timeout);
    };

    autoCloseToast('toast-success', 5000);
    autoCloseToast('toast-profile', 2500);
});

// ===== Navigation helper =====
function goBackOrHome() {
    if (window.history.length > 1) {
        window.history.back();
    } else {
        window.location.href = '/';
    }
}
