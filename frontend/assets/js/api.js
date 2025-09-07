const API_BASE = localStorage.getItem('apiBase') || 'https://localhost:7043';

const api = {
  async request(path, options = {}) {
    const token = localStorage.getItem('token');
    const headers = { 'Content-Type': 'application/json', ...(options.headers || {}) };
    if (token) headers['Authorization'] = `Bearer ${token}`;
    const res = await fetch(`${API_BASE}${path}`, { ...options, headers });
    if (!res.ok) {
      const text = await res.text();
      throw new Error(text || res.statusText);
    }
    return res.status === 204 ? null : res.json();
  },
  get: (p) => api.request(p),
  post: (p, body) => api.request(p, { method: 'POST', body: JSON.stringify(body) }),
  put: (p, body) => api.request(p, { method: 'PUT', body: JSON.stringify(body) }),
  del: (p) => api.request(p, { method: 'DELETE' })
};
