namespace ecommerce.wwwroot.js {
    document.addEventListener('DOMContentLoaded', function () {
        const navLinks = document.querySelectorAll('.nav-link');

        navLinks.forEach(link => {
            link.addEventListener('click', function () {
                document.body.className = '';
                if (this.dataset.bsTarget === '#menu-breakfast') {
                    document.body.classList.add('breakfast-active');
                }
            });
        });
    });

}