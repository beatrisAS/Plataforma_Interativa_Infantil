(async () => {
  // activities page script
  const tbl = document.querySelector('#tblActivities tbody');
  const btnNovo = document.getElementById('btnNovo');
  const mdl = document.getElementById('mdlActivity');
  if (!tbl || !mdl) return;

  let bsModal = new bootstrap.Modal(mdl);
  let editId = null;

  async function carregar() {
    try {
      const list = await api.get('/api/activities');
      tbl.innerHTML = list.map(a => `
        <tr>
          <td>${a.title}</td>
          <td>${a.type}</td>
          <td class="text-end">
            <button class="btn btn-sm btn-outline-primary" data-id="${a.id}" data-action="edit">Editar</button>
            <button class="btn btn-sm btn-outline-danger ms-2" data-id="${a.id}" data-action="del">Excluir</button>
          </td>
        </tr>`).join('');
    } catch (err) {
      console.error(err);
      tbl.innerHTML = '<tr><td colspan="3">Erro ao carregar.</td></tr>';
    }
  }

  btnNovo.addEventListener('click', () => {
    editId = null;
    document.getElementById('activityId').value = '';
    document.getElementById('activityTitle').value = '';
    document.getElementById('activityDesc').value = '';
    document.getElementById('activityType').value = '';
    document.getElementById('activityUrl').value = '';
    bsModal.show();
  });

  tbl.addEventListener('click', async (e) => {
    const btn = e.target.closest('button');
    if (!btn) return;
    const id = btn.getAttribute('data-id');
    const action = btn.getAttribute('data-action');

    if (action === 'edit') {
      const a = await api.get(`/api/activities/${id}`);
      editId = a.id;
      document.getElementById('activityId').value = a.id;
      document.getElementById('activityTitle').value = a.title;
      document.getElementById('activityDesc').value = a.description || '';
      document.getElementById('activityType').value = a.type || '';
      document.getElementById('activityUrl').value = a.url || '';
      bsModal.show();
    }

    if (action === 'del' && confirm('Excluir esta atividade?')) {
      await api.del(`/api/activities/${id}`);
      await carregar();
    }
  });

  document.getElementById('btnSaveActivity').addEventListener('click', async () => {
    const body = {
      title: document.getElementById('activityTitle').value,
      description: document.getElementById('activityDesc').value,
      type: document.getElementById('activityType').value,
      url: document.getElementById('activityUrl').value,
      createdAt: new Date().toISOString()
    };

    if (editId) await api.put(`/api/activities/${editId}`, body);
    else await api.post('/api/activities', body);

    bsModal.hide();
    await carregar();
  });

  await carregar();
})();
