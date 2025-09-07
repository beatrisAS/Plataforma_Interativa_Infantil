// Login form behavior
const formLogin = document.getElementById('formLogin');
if (formLogin) {
  formLogin.addEventListener('submit', async (e) => {
    e.preventDefault();
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    try {
      const resp = await api.post('/api/auth/login', { email, password });
      localStorage.setItem('token', resp.token);
      localStorage.setItem('userName', resp.name);
      window.location.href = 'dashboard.html';
    } catch (err) {
      alert('Erro ao entrar: ' + err.message);
    }
  });
}

// Register form
const formRegister = document.getElementById('formRegister');
if (formRegister) {
  formRegister.addEventListener('submit', async (e) => {
    e.preventDefault();
    const name = document.getElementById('regName').value;
    const email = document.getElementById('regEmail').value;
    const password = document.getElementById('regPassword').value;
    try {
      const resp = await api.post('/api/auth/register', { name, email, password });
      localStorage.setItem('token', resp.token);
      localStorage.setItem('userName', resp.name);
      window.location.href = 'dashboard.html';
    } catch (err) {
      alert('Erro ao registrar: ' + err.message);
    }
  });
}

// logout helper
function logout() { localStorage.removeItem('token'); localStorage.removeItem('userName'); window.location.href = 'index.html'; }
