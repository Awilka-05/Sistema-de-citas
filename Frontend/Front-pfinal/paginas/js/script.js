
let users = [
    { id: 1, email: 'usuario@ejemplo.com', password: '123456', name: 'Juan Pérez', role: 'user' },
    { id: 2, email: 'admin@ejemplo.com', password: '123456', name: 'Administrador', role: 'admin' }
];

// Login functionality
document.addEventListener('DOMContentLoaded', function() {
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const email = document.getElementById('email').value;
            const password = document.getElementById('password').value;
            
            const user = users.find(u => u.email === email && u.password === password);
            
            if (user) {
                // Guardar usuario en sessionStorage
                sessionStorage.setItem('currentUser', JSON.stringify(user));
                
                // Redirigir según el rol
                if (user.role === 'admin') {
                    window.location.href = 'admin.html';
                } else {
                    window.location.href = 'usuario.html';
                }
            } else {
                alert('Credenciales incorrectas. Por favor, inténtalo de nuevo.');
            }
        });
    }
    
    // Logout functionality
    const logoutButtons = document.querySelectorAll('.btn-logout, #adminLogoutBtn');
    logoutButtons.forEach(button => {
        button.addEventListener('click', function() {
            sessionStorage.removeItem('currentUser');
            window.location.href = 'index.html';
        });
    });
    
    // Mostrar nombre de usuario si está logueado
    const userElements = document.querySelectorAll('.user-name strong, .admin-info strong');
    const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));
    
    if (currentUser && userElements.length > 0) {
        userElements.forEach(element => {
            element.textContent = currentUser.name;
        });
    } else if (window.location.pathname.includes('usuario.html') || 
               window.location.pathname.includes('admin.html')) {
        // Redirigir a login si no está autenticado
        window.location.href = 'index.html';
    }
});